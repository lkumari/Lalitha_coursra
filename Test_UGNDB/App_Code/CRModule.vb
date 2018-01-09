''************************************************************************************************
''Name:		CRModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the CRensed Projects Module
''
''Date		    Author	    
''01/12/2010    LRey			Created .Net application
''02/17/2010    RCarlson        Added Phase 2 functionality, removed file upload
''03/10/2010    RCarlson        Added GetCostReductionProjectLeaderList
''05/17/2010    RCarlson        Added GetCostReductionSearch function
''06/24/2010    RCarlson        CR-2920 - isOffsetsCostDowns
''07/14/2011    LRey            Added new function UpdateExpProjRepairCRProjectNo
''08/31/2011    RCarlson        Added Customer Give Back fields to InsertCostReductionDetail and UpdateCostReductionDetail
''09/12/2011    RCarlson        Added Budget Fields to InsertCostReductionDetail and UpdateCostReductionDetail.
''05/11/2012    LRey            Added new function UpdateExpProjDevelopmentCRProjectNo
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

Public Class CRModule
    Public Shared Function GetProjectCategory(ByVal ProjectCategoryName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Project_Category"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectCategoryName", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectCategoryName").Value = ProjectCategoryName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProjectCategory")

            GetProjectCategory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectCategoryName: " & ProjectCategoryName
            HttpContext.Current.Session("BLLerror") = "GetProjectCategory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProjectCategory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProjectCategory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetProjectCategory

    Public Shared Function GetCostReduction(ByVal ProjectNo As String, ByVal LeaderTMID As Integer, ByVal UGNFacility As String, _
        ByVal CommodityID As Integer, ByVal ProjectCategoryID As Integer, ByVal Description As String, _
        ByVal RFDNo As Integer, ByVal filterPlantControllerReviewed As Boolean, ByVal isPlantControllerReviewed As Boolean, ByVal CapExProjNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
            myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@filterPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@filterPlantControllerReviewed").Value = filterPlantControllerReviewed

            myCommand.Parameters.Add("@isPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerReviewed").Value = isPlantControllerReviewed

            myCommand.Parameters.Add("@CapExProjNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjNo").Value = CapExProjNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cost_Reduction")

            GetCostReduction = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", ProjectNo: " & ProjectNo & ", LeaderTMID: " & LeaderTMID & ", UGNFacility: " & UGNFacility _
            & ", CommodityID: " & CommodityID & ", Description: " & Description _
            & ", RFDNo: " & RFDNo & ", filterPlantControllerReviewed: " & filterPlantControllerReviewed _
            & ", isPlantControllerReviewed: " & isPlantControllerReviewed

            HttpContext.Current.Session("BLLerror") = "GetCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReduction

    Public Shared Function GetCostReductionSearch(ByVal ProjectNo As String, ByVal LeaderTMID As Integer, ByVal UGNFacility As String, _
        ByVal CommodityID As Integer, ByVal ProjectCategoryID As Integer, _
        ByVal Description As String, ByVal RFDNo As Integer, _
        ByVal filterPlantControllerReviewed As Boolean, ByVal isPlantControllerReviewed As Boolean, _
        ByVal filterOffsetsCostDowns As Boolean, ByVal isOffsetsCostDowns As Boolean, _
        ByVal includeCompleted As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
            myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@filterPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@filterPlantControllerReviewed").Value = filterPlantControllerReviewed

            myCommand.Parameters.Add("@isPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerReviewed").Value = isPlantControllerReviewed

            myCommand.Parameters.Add("@filterOffsetsCostDowns", SqlDbType.Bit)
            myCommand.Parameters("@filterOffsetsCostDowns").Value = filterOffsetsCostDowns

            myCommand.Parameters.Add("@isOffsetsCostDowns", SqlDbType.Bit)
            myCommand.Parameters("@isOffsetsCostDowns").Value = isOffsetsCostDowns

            myCommand.Parameters.Add("@includeCompleted", SqlDbType.Bit)
            myCommand.Parameters("@includeCompleted").Value = includeCompleted

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cost_Reduction_List")

            GetCostReductionSearch = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", ProjectNo: " & ProjectNo _
            & ", LeaderTMID: " & LeaderTMID _
            & ", UGNFacility: " & UGNFacility _
            & ", CommodityID: " & CommodityID _
            & ", Description: " & Description _
            & ", RFDNo: " & RFDNo _
            & ", filterPlantControllerReviewed: " & filterPlantControllerReviewed _
            & ", isPlantControllerReviewed: " & isPlantControllerReviewed _
            & ", filterOffsetsCostDowns: " & filterOffsetsCostDowns _
            & ", isOffsetsCostDowns: " & isOffsetsCostDowns _
            & ", includeCompleted: " & includeCompleted

            HttpContext.Current.Session("BLLerror") = "GetCostReductionSearch : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionSearch

    Public Shared Function GetCostReductionDetail(ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cost_Reduction_Detail")

            GetCostReductionDetail = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionDetail = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionDetail

    Public Shared Function GetCostReductionOverheadCurrentTotal(ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Overhead_Current_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cost_Reduction_Overhead_Current")

            GetCostReductionOverheadCurrentTotal = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetCostReductionOverheadCurrentTotal : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionOverheadCurrentTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionOverheadCurrentTotal = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionOverheadCurrentTotal

    Public Shared Function GetCostReductionOverheadProposedTotal(ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Overhead_Proposed_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cost_Reduction_Overhead_Proposed")

            GetCostReductionOverheadProposedTotal = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetCostReductionOverheadProposedTotal : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionOverheadProposedTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionOverheadProposedTotal = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionOverheadProposedTotal

    Public Shared Function GetCostReductionSteps(ByVal StepID As Integer, ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Steps"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StepID", SqlDbType.Int)
            myCommand.Parameters("@StepID").Value = StepID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostReductionSteps")

            GetCostReductionSteps = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo & ", StepID: " & StepID
            HttpContext.Current.Session("BLLerror") = "GetCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionSteps = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionSteps

    Public Shared Function GetCostReductionStatus(ByVal StatusID As Integer, ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostReductionSteps")

            GetCostReductionStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo & ", StatusID: " & StatusID
            HttpContext.Current.Session("BLLerror") = "GetCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionStatus

    Public Shared Function GetLastCostReductionProjectNo(ByVal LeaderTMID As Integer, ByVal UGNFacility As String, ByVal CommodityID As Integer, ByVal ProjectCategoryID As Integer, ByVal Description As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Cost_Reduction_ProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
            myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "LastProjNo")

            GetLastCostReductionProjectNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetLastCostReductionProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLastCostReductionProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastCostReductionProjectNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastCostReductionProjectNo

    Public Shared Function GetCostReductionDocument(ByVal ProjectNo As Integer, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo ", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo ").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, " GetCostReductionDocument ")

            GetCostReductionDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = " GetCostReductionDocument: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetCostReductionDocument ") = "~/CR/CostReductionProposedDetail.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostReductionDocument: " & commonFunctions.convertSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCostReductionDocument

    Public Shared Sub InsertCostReduction(ByVal Description As String, ByVal ProjectCategoryID As Integer, _
        ByVal LeaderTMID As Integer, ByVal UGNFacility As String, ByVal CommodityID As Integer, _
        ByVal EstImpDate As String, ByVal Completion As Decimal, ByVal RFDNo As Integer, _
        ByVal SuccessRate As Decimal, ByVal EstAnnCostSave As Decimal, ByVal CapEx As Decimal, _
        ByVal isOffsetsCostDowns As Boolean, ByVal isPlantControllerReviewed As Boolean, _
        ByVal CapExProjNo As String, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
            myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

            myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@EstImpDate", SqlDbType.VarChar)
            myCommand.Parameters("@EstImpDate").Value = EstImpDate

            myCommand.Parameters.Add("@Completion", SqlDbType.Decimal)
            myCommand.Parameters("@Completion").Value = Completion

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SuccessRate", SqlDbType.Decimal)
            myCommand.Parameters("@SuccessRate").Value = SuccessRate

            myCommand.Parameters.Add("@EstAnnCostSave", SqlDbType.Decimal)
            myCommand.Parameters("@EstAnnCostSave").Value = EstAnnCostSave

            myCommand.Parameters.Add("@CapEx", SqlDbType.Decimal)
            myCommand.Parameters("@CapEx").Value = CapEx

            myCommand.Parameters.Add("@isOffsetsCostDowns", SqlDbType.Bit)
            myCommand.Parameters("@isOffsetsCostDowns").Value = isOffsetsCostDowns

            myCommand.Parameters.Add("@isPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerReviewed").Value = isPlantControllerReviewed

            myCommand.Parameters.Add("@CapExProjNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjNo").Value = CapExProjNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Description: " & Description _
            & ", ProjectCategoryID: " & ProjectCategoryID _
            & ", LeaderTMID: " & LeaderTMID _
            & ", UGNFacility: " & UGNFacility _
            & ", CommodityID: " & CommodityID _
            & ", EstImpDate: " & EstImpDate _
            & ", Completion: " & Completion _
            & ", RFDNo: " & RFDNo _
            & ", SuccessRate: " & SuccessRate _
            & ", EstAnnCostSave: " & EstAnnCostSave _
            & ", CapEx: " & CapEx _
            & ", isPlantControllerReviewed: " & isPlantControllerReviewed _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'InsertCostReduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertCostReduction

    Public Shared Sub InsertCostReductionSteps(ByVal ProjectNo As Integer, ByVal DateEntered As String, ByVal TeamMemberID As Integer, ByVal StepsComments As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_Steps"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DateEntered", SqlDbType.VarChar)
            myCommand.Parameters("@DateEntered").Value = DateEntered

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@StepsComments", SqlDbType.VarChar)
            myCommand.Parameters("@StepsComments").Value = commonFunctions.convertSpecialChar(StepsComments, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "NewCostReductionSteps")
            'InsertCostReductionSteps = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'InsertCostReductionSteps = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertCostReductionSteps

    Public Shared Sub InsertCostReductionStatus(ByVal ProjectNo As Integer, ByVal DateEntered As String, ByVal Status As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DateEntered", SqlDbType.VarChar)
            myCommand.Parameters("@DateEntered").Value = DateEntered

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = commonFunctions.convertSpecialChar(Status, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "NewCostReductionStatus")
            'InsertCostReductionStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'InsertCostReductionStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertCostReductionStatus

    Public Shared Sub InsertCostReductionHistory(ByVal ProjectNo As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.convertSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = FieldChange

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = PreviousValue

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = NewValue

            myConnection.Open()
            myCommand.ExecuteNonQuery()

            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "InsertCostReductionHistory")
            'InsertCostReductionHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostReductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'InsertCostReductionHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertCostReductionHistory

    Public Shared Sub InsertCostReductionFileUpload(ByVal ProjectNo As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_FileUpload"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.convertSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "InsertCostReductionFileUpload")
            'InsertCostReductionFileUpload = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostReductionFileUpload : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionFileUpload : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'InsertCostReductionFileUpload = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF InsertCostReductionFileUpload

    Public Shared Function InsertCostReductionDocuments(ByVal ProjectNo As Integer, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.convertSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertCostReductionDocuments")

            InsertCostReductionDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.convertSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertCostReductionDocuments") = "~/CR/CostReductionProposedDetail.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertCostReductionDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertCostReductionDocuments

    Public Shared Sub UpdateCostReduction(ByVal ProjectNo As String, ByVal Description As String, _
        ByVal ProjectCategoryID As Integer, ByVal LeaderTMID As Integer, ByVal UGNFacility As String, _
        ByVal CommodityID As String, ByVal EstImpDate As String, ByVal Completion As Decimal, _
        ByVal RFDNo As Integer, ByVal SuccessRate As Decimal, ByVal EstAnnCostSave As Decimal, _
        ByVal CapEx As Decimal, ByVal isOffsetsCostDowns As Boolean, _
        ByVal isPlantControllerReviewed As Boolean, ByVal CapExProjNo As String, _
        ByVal UpdatedBy As String, ByVal UpdatedOn As String, ByVal Submitted As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

            myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
            myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

            myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@EstImpDate", SqlDbType.VarChar)
            myCommand.Parameters("@EstImpDate").Value = EstImpDate

            myCommand.Parameters.Add("@Completion", SqlDbType.Decimal)
            myCommand.Parameters("@Completion").Value = Completion

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SuccessRate", SqlDbType.Decimal)
            myCommand.Parameters("@SuccessRate").Value = SuccessRate

            myCommand.Parameters.Add("@EstAnnCostSave", SqlDbType.Decimal)
            myCommand.Parameters("@EstAnnCostSave").Value = EstAnnCostSave

            myCommand.Parameters.Add("@CapEx", SqlDbType.Decimal)
            myCommand.Parameters("@CapEx").Value = CapEx

            myCommand.Parameters.Add("@isOffsetsCostDowns", SqlDbType.Bit)
            myCommand.Parameters("@isOffsetsCostDowns").Value = isOffsetsCostDowns

            myCommand.Parameters.Add("@isPlantControllerReviewed", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerReviewed").Value = isPlantControllerReviewed

            myCommand.Parameters.Add("@CapExProjNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjNo").Value = CapExProjNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myCommand.Parameters.Add("@Submitted", SqlDbType.VarChar)
            myCommand.Parameters("@Submitted").Value = Submitted

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "UpdateCostReduction")
            'UpdateCostReduction = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo _
            & ", Description: " & Description _
            & ", ProjectCategoryID: " & ProjectCategoryID _
            & ", LeaderTMID: " & LeaderTMID _
            & ", UGNFacility: " & UGNFacility _
            & ", CommodityID: " & CommodityID _
            & ", EstImpDate: " & EstImpDate _
            & ", Completion: " & Completion _
            & ", RFDNo: " & RFDNo _
            & ", SuccessRate: " & SuccessRate _
            & ", EstAnnCostSave: " & EstAnnCostSave _
            & ", CapEx: " & CapEx _
            & ", isPlantControllerReviewed: " & isPlantControllerReviewed _
            & ", Submitted: " & Submitted _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'UpdateCostReduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateCostReduction

    Public Shared Sub UpdateCostReductionSavingsAndCapEx(ByVal ProjectNo As Integer, ByVal EstAnnCostSave As Decimal, ByVal CapEx As Decimal)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction_Savings_And_CapEx"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@EstAnnCostSave", SqlDbType.Decimal)
            myCommand.Parameters("@EstAnnCostSave").Value = EstAnnCostSave

            myCommand.Parameters.Add("@CapEx", SqlDbType.Decimal)
            myCommand.Parameters("@CapEx").Value = CapEx

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionSavingsAndCapEx : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionSavingsAndCapEx : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateCostReductionSavingsAndCapEx

    'Public Shared Sub UpdateCostReductionFileUpload(ByVal ProjectNo As String, ByVal Description As String, _
    '    ByVal ProjectCategoryID As Integer, ByVal LeaderTMID As Integer, ByVal UGNFacility As String, _
    '    ByVal CommodityID As String, ByVal EstImpDate As String, ByVal Completion As Decimal, _
    '    ByVal RFDNo As Integer, ByVal SuccessRate As Decimal, ByVal EstAnnCostSave As Decimal, _
    '    ByVal CapEx As Decimal, ByVal UpdatedBy As String, ByVal UpdatedOn As String, _
    '    ByVal Submitted As Boolean, _
    '    ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Update_Cost_Reduction"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
    '        myCommand.Parameters("@ProjectNo").Value = ProjectNo

    '        myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
    '        myCommand.Parameters("@Description").Value = commonFunctions.convertSpecialChar(Description, False)

    '        myCommand.Parameters.Add("@ProjectCategoryID", SqlDbType.Int)
    '        myCommand.Parameters("@ProjectCategoryID").Value = ProjectCategoryID

    '        myCommand.Parameters.Add("@LeaderTMID", SqlDbType.Int)
    '        myCommand.Parameters("@LeaderTMID").Value = LeaderTMID

    '        myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
    '        myCommand.Parameters("@UGNFacility").Value = UGNFacility

    '        myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
    '        myCommand.Parameters("@CommodityID").Value = CommodityID

    '        myCommand.Parameters.Add("@EstImpDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@EstImpDate").Value = EstImpDate

    '        myCommand.Parameters.Add("@Completion", SqlDbType.Decimal)
    '        myCommand.Parameters("@Completion").Value = Completion

    '        myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
    '        myCommand.Parameters("@RFDNo").Value = RFDNo

    '        myCommand.Parameters.Add("@SuccessRate", SqlDbType.Decimal)
    '        myCommand.Parameters("@SuccessRate").Value = SuccessRate

    '        myCommand.Parameters.Add("@EstAnnCostSave", SqlDbType.Decimal)
    '        myCommand.Parameters("@EstAnnCostSave").Value = EstAnnCostSave

    '        myCommand.Parameters.Add("@CapEx", SqlDbType.Decimal)
    '        myCommand.Parameters("@CapEx").Value = CapEx

    '        myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

    '        myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
    '        myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

    '        myCommand.Parameters.Add("@Submitted", SqlDbType.VarChar)
    '        myCommand.Parameters("@Submitted").Value = Submitted

    '        myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
    '        myCommand.Parameters("@BinaryFile").Value = BinaryFile

    '        myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
    '        myCommand.Parameters("@FileName").Value = commonFunctions.convertSpecialChar(FileName, False)

    '        myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
    '        myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

    '        myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
    '        myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()
    '        'myAdapter = New SqlDataAdapter(myCommand)
    '        'myAdapter.Fill(GetData, "UpdateCostReductionFileUpload")
    '        'UpdateCostReductionFileUpload = GetData

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "UpdateCostReductionFileUpload : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateCostReductionFileUpload : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        'UpdateCostReductionFileUpload = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    'End Sub ' EOF UpdateCostReductionFileUpload

    Public Shared Sub UpdateCostReductionSteps(ByVal StepID As Integer, ByVal ProjectNo As Integer, ByVal TeamMemberID As Integer, ByVal StepsComments As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction_Steps"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StepID", SqlDbType.Int)
            myCommand.Parameters("@StepID").Value = StepID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@StepsComments", SqlDbType.VarChar)
            myCommand.Parameters("@StepsComments").Value = commonFunctions.convertSpecialChar(StepsComments, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "UpdateCostReductionSteps")
            'UpdateCostReductionSteps = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StepID: " & StepID & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'UpdateCostReductionSteps = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateCostReductionSteps

    Public Shared Sub UpdateCostReductionStatus(ByVal StatusID As Integer, ByVal ProjectNo As Integer, ByVal Status As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = commonFunctions.convertSpecialChar(Status, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "UpdateCostReductionStatus")
            'UpdateCostReductionStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'UpdateCostReductionStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateCostReductionStatus

    Public Shared Sub UpdateExpProjAssetsCRProjectNo(ByVal ProjectNo As String, ByVal CRProjectNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Assets_CRProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRProjectNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CRProjectNo: " & CRProjectNo & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssetsCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssetsCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjAssetsCRProjectNo

    Public Shared Sub UpdateExpProjRepairCRProjectNo(ByVal ProjectNo As String, ByVal CRProjectNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Repair_CRProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRProjectNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CRProjectNo: " & CRProjectNo & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepairCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepairCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjAssetsCRProjectNo

    Public Shared Sub UpdateExpProjDevelopmentCRProjectNo(ByVal ProjectNo As String, ByVal CRProjectNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Development_CRProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRProjectNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CRProjectNo: " & CRProjectNo & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjDevelopmentCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjDevelopmentCRProjectNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjDevelopmentCRProjectNo


    Public Shared Sub DeleteCostReduction(ByVal ProjectNo As String)
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Reduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            'myAdapter = New SqlDataAdapter(myCommand)
            'myAdapter.Fill(GetData, "DeleteCRProjTooling")
            'DeleteCostReduction = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            'DeleteCostReduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteCostReduction

    Public Shared Sub DeleteCostReductionCookies()

        Try
            HttpContext.Current.Response.Cookies("CR_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("CR_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Leader").Value = ""
            HttpContext.Current.Response.Cookies("CR_Leader").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("CR_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Commodity").Value = ""
            HttpContext.Current.Response.Cookies("CR_Commodity").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_ProjCat").Value = ""
            HttpContext.Current.Response.Cookies("CR_ProjCat").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Desc").Value = ""
            HttpContext.Current.Response.Cookies("CR_Desc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_RFDNo").Value = ""
            HttpContext.Current.Response.Cookies("CR_RFDNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_FilterPlantControllerReviewed").Value = 0
            HttpContext.Current.Response.Cookies("CR_FilterPlantControllerReviewed").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_IsPlantControllerReviewed").Value = 0
            HttpContext.Current.Response.Cookies("CR_IsPlantControllerReviewed").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_FilterOffsetsCostDowns").Value = 0
            HttpContext.Current.Response.Cookies("CR_FilterOffsetsCostDowns").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_IsOffsetsCostDowns").Value = 0
            HttpContext.Current.Response.Cookies("CR_IsOffsetsCostDowns").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_IncludeCompleted").Value = 0
            HttpContext.Current.Response.Cookies("CR_IncludeCompleted").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteCostReductionCookies

    Public Shared Sub DeleteCostReductionReportCookies()

        Try
            HttpContext.Current.Response.Cookies("CR_ImpDtFrom").Value = ""
            HttpContext.Current.Response.Cookies("CR_ImpDtFrom").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_ImpDtTo").Value = ""
            HttpContext.Current.Response.Cookies("CR_ImpDtTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Fac").Value = ""
            HttpContext.Current.Response.Cookies("CR_Fac").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_LMTID").Value = ""
            HttpContext.Current.Response.Cookies("CR_LMTID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_CID").Value = ""
            HttpContext.Current.Response.Cookies("CR_CID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_PCID").Value = ""
            HttpContext.Current.Response.Cookies("CR_PCID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Sort").Value = ""
            HttpContext.Current.Response.Cookies("CR_Sort").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("CR_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_SoldTo").Value = ""
            HttpContext.Current.Response.Cookies("CR_SoldTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_Pgm").Value = ""
            HttpContext.Current.Response.Cookies("CR_Pgm").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CR_PCR").Value = ""
            HttpContext.Current.Response.Cookies("CR_PCR").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionReportCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionReport.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionReportCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteCostReductionReportCookies

    Public Shared Sub CleanCRCrystalReports()

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
            HttpContext.Current.Session("BLLerror") = "CleanCRCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanCRCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub 'EOF CleanCRCrystalReports

    Public Shared Sub InsertCostReductionCustomerProgram(ByVal ProjectNo As Integer, ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo _
            & ", ProgramID: " & ProgramID _
            & ", ProgramYear: " & ProgramYear _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostReductionCustomerProgram(ByVal RowID As Integer, ByVal ProjectNo As Integer,  ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", RowID: " & RowID & ", ProgramID: " & ProgramID _
            & ", ProgramYear: " & ProgramYear _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertCostReductionDetail(ByVal ProjectNo As Integer, _
       ByVal CurrentMethod As String, _
       ByVal ProposedMethod As String, _
       ByVal Benefits As String, _
       ByVal CustomerPartNo As String, _
       ByVal MaterialPriceCurrentPrice As Double, _
       ByVal MaterialPriceCurrentPriceBudget As Double, _
       ByVal MaterialPriceCurrentFreight As Double, _
       ByVal MaterialPriceCurrentFreightBudget As Double, _
       ByVal MaterialPriceCurrentVolume As Integer, _
       ByVal MaterialPriceCurrentVolumeBudget As Integer, _
       ByVal MaterialPriceCurrentPriceByVolume As Double, _
       ByVal MaterialPriceCurrentPriceByVolumeBudget As Double, _
       ByVal MaterialPriceCurrentFreightByVolume As Double, _
       ByVal MaterialPriceCurrentFreightByVolumeBudget As Double, _
       ByVal MaterialPriceCurrentMaterialLanded As Double, _
       ByVal MaterialPriceCurrentMaterialLandedBudget As Double, _
       ByVal MaterialPriceCurrentMaterialLandedTotal As Double, _
       ByVal MaterialPriceCurrentMaterialLandedTotalBudget As Double, _
       ByVal MaterialPriceProposedPrice As Double, _
       ByVal MaterialPriceProposedFreight As Double, _
       ByVal MaterialPriceProposedVolume As Integer, _
       ByVal MaterialPriceProposedPriceByVolume As Double, _
       ByVal MaterialPriceProposedFreightByVolume As Double, _
       ByVal MaterialPriceProposedMaterialLanded As Double, _
       ByVal MaterialPriceProposedMaterialLandedTotal As Double, _
       ByVal MaterialPriceCurrentMethod As Double, _
       ByVal MaterialPriceCurrentMethodBudget As Double, _
       ByVal MaterialPriceProposedMethod As Double, _
       ByVal MaterialPriceSavings As Double, _
       ByVal MaterialPriceSavingsBudget As Double, _
       ByVal MaterialPriceCECapital As Double, _
       ByVal MaterialPriceCEMaterial As Double, _
       ByVal MaterialPriceCEOutsideSupport As Double, _
       ByVal MaterialPriceCEMisc As Double, _
       ByVal MaterialPriceCEInHouseSupport As Double, _
       ByVal MaterialPriceCETotal As Double, _
       ByVal MaterialPricePayback As Double, _
       ByVal MaterialUsageCurrentCostPerUnit As Double, _
       ByVal MaterialUsageCurrentCostPerUnitBudget As Double, _
       ByVal MaterialUsageCurrentUnitPerParent As Double, _
       ByVal MaterialUsageCurrentUnitPerParentBudget As Double, _
       ByVal MaterialUsageCurrentCostTotal As Double, _
       ByVal MaterialUsageCurrentCostTotalBudget As Double, _
       ByVal MaterialUsageProposedCostPerUnit As Double, _
       ByVal MaterialUsageProposedUnitPerParent As Double, _
       ByVal MaterialUsageProposedCostTotal As Double, _
       ByVal MaterialUsageProgramVolume As Integer, _
       ByVal MaterialUsageProgramVolumeBudget As Integer, _
       ByVal MaterialUsageCurrentMethod As Double, _
       ByVal MaterialUsageCurrentMethodBudget As Double, _
       ByVal MaterialUsageProposedMethod As Double, _
       ByVal MaterialUsageSavings As Double, _
       ByVal MaterialUsageSavingsBudget As Double, _
       ByVal MaterialUsageCECapital As Double, _
       ByVal MaterialUsageCEMaterial As Double, _
       ByVal MaterialUsageCEOutsideSupport As Double, _
       ByVal MaterialUsageCEMisc As Double, _
       ByVal MaterialUsageCEInHouseSupport As Double, _
       ByVal MaterialUsageCETotal As Double, _
       ByVal MaterialUsagePayback As Double, _
       ByVal CycleTimeCurrentPiecesPerHour As Double, _
       ByVal CycleTimeCurrentPiecesPerHourBudget As Double, _
       ByVal CycleTimeCurrentCrewSize As Double, _
       ByVal CycleTimeCurrentCrewSizeBudget As Double, _
       ByVal CycleTimeCurrentVolume As Integer, _
       ByVal CycleTimeCurrentVolumeBudget As Integer, _
       ByVal CycleTimeCurrentMachineHourPerPieces As Double, _
       ByVal CycleTimeCurrentMachineHourPerPiecesBudget As Double, _
       ByVal CycleTimeCurrentManHourPerPieces As Double, _
       ByVal CycleTimeCurrentManHourPerPiecesBudget As Double, _
       ByVal CycleTimeCurrentTotalManHours As Double, _
       ByVal CycleTimeCurrentTotalManHoursBudget As Double, _
       ByVal CycleTimeProposedPiecesPerHour As Double, _
       ByVal CycleTimeProposedCrewSize As Double, _
       ByVal CycleTimeProposedVolume As Integer, _
       ByVal CycleTimeProposedMachineHourPerPieces As Double, _
       ByVal CycleTimeProposedManHourPerPieces As Double, _
       ByVal CycleTimeProposedTotalManHours As Double, _
       ByVal CycleTimeFUTARate As Double, _
       ByVal CycleTimeSUTARate As Double, _
       ByVal CycleTimeFICARate As Double, _
       ByVal CycleTimeVariableFringes As Double, _
       ByVal CycleTimeWages As Double, _
       ByVal CycleTimeWagesPlusFringes As Double, _
       ByVal CycleTimeCurrentMethod As Double, _
       ByVal CycleTimeCurrentMethodBudget As Double, _
       ByVal CycleTimeProposedMethod As Double, _
       ByVal CycleTimeMethodDifference As Double, _
       ByVal CycleTimeMethodDifferenceBudget As Double, _
       ByVal CycleTimeSavings As Double, _
       ByVal CycleTimeSavingsBudget As Double, _
       ByVal CycleTimeCECapital As Double, _
       ByVal CycleTimeCEMaterial As Double, _
       ByVal CycleTimeCEOutsideSupport As Double, _
       ByVal CycleTimeCEMisc As Double, _
       ByVal CycleTimeCEInHouseSupport As Double, _
       ByVal CycleTimeCETotal As Double, _
       ByVal CycleTimePayback As Double, _
       ByVal HeadCountWages As Double, _
       ByVal HeadCountWagesBudget As Double, _
       ByVal HeadCountAnnualLaborCost As Double, _
       ByVal HeadCountAnnualLaborCostBudget As Double, _
       ByVal HeadCountCurrentLaborCount As Double, _
       ByVal HeadCountCurrentLaborCountBudget As Double, _
       ByVal HeadCountCurrentLaborCost As Double, _
       ByVal HeadCountCurrentLaborCostBudget As Double, _
       ByVal HeadCountCurrentLaborFringes As Double, _
       ByVal HeadCountCurrentLaborTotal As Double, _
       ByVal HeadCountCurrentLaborTotalBudget As Double, _
       ByVal HeadCountProposedLaborCount As Double, _
       ByVal HeadCountProposedLaborCost As Double, _
       ByVal HeadCountProposedLaborFringes As Double, _
       ByVal HeadCountProposedLaborTotal As Double, _
       ByVal HeadCountCurrentMethod As Double, _
       ByVal HeadCountCurrentMethodBudget As Double, _
       ByVal HeadCountProposedMethod As Double, _
       ByVal HeadCountSavings As Double, _
       ByVal HeadCountSavingsBudget As Double, _
       ByVal HeadCountFUTA As Double, _
       ByVal HeadCountSUTA As Double, _
       ByVal HeadCountFICA As Double, _
       ByVal HeadCountPension As Double, _
       ByVal HeadCountBonus As Double, _
       ByVal HeadCountLife As Double, _
       ByVal HeadCountGroupInsurance As Double, _
       ByVal HeadCountWorkersComp As Double, _
       ByVal HeadCountPensionQuarterly As Double, _
       ByVal HeadCountTotalFringes As Double, _
       ByVal HeadCountCECapital As Double, _
       ByVal HeadCountCEMaterial As Double, _
       ByVal HeadCountCEOutsideSupport As Double, _
       ByVal HeadCountCEMisc As Double, _
       ByVal HeadCountCEInHouseSupport As Double, _
       ByVal HeadCountCETotal As Double, _
       ByVal HeadCountPayback As Double, _
       ByVal OverheadCurrentMethod As Double, _
       ByVal OverheadCurrentMethodBudget As Double, _
       ByVal OverheadProposedMethod As Double, _
       ByVal OverheadSavings As Double, _
       ByVal OverheadSavingsBudget As Double, _
       ByVal OverheadCECapital As Double, _
       ByVal OverheadCEMaterial As Double, _
       ByVal OverheadCEOutsideSupport As Double, _
       ByVal OverheadCEMisc As Double, _
       ByVal OverheadCEInHouseSupport As Double, _
       ByVal OverheadCEWriteOff As Double, _
       ByVal OverheadCETotal As Double, _
       ByVal OverheadPayback As Double, _
       ByVal TotalSavings As Double, _
       ByVal TotalSavingsBudget As Double, _
       ByVal TotalCE As Double, _
       ByVal TotalPayback As Double, _
       ByVal CustomerGiveBackDollar As Double, _
       ByVal CustomerGiveBackPercent As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Reduction_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            If CurrentMethod Is Nothing Then
                CurrentMethod = ""
            End If

            myCommand.Parameters.Add("@CurrentMethod", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentMethod").Value = commonFunctions.convertSpecialChar(CurrentMethod, False)

            If ProposedMethod Is Nothing Then
                ProposedMethod = ""
            End If

            myCommand.Parameters.Add("@ProposedMethod", SqlDbType.VarChar)
            myCommand.Parameters("@ProposedMethod").Value = commonFunctions.convertSpecialChar(ProposedMethod, False)

            If Benefits Is Nothing Then
                Benefits = ""
            End If

            myCommand.Parameters.Add("@Benefits", SqlDbType.VarChar)
            myCommand.Parameters("@Benefits").Value = commonFunctions.convertSpecialChar(Benefits, False)

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = commonFunctions.convertSpecialChar(CustomerPartNo, False)

            myCommand.Parameters.Add("@MaterialPriceCurrentPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPrice").Value = MaterialPriceCurrentPrice

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceBudget").Value = MaterialPriceCurrentPriceBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentFreight", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreight").Value = MaterialPriceCurrentFreight

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightBudget").Value = MaterialPriceCurrentFreightBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentVolume", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceCurrentVolume").Value = MaterialPriceCurrentVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentVolumeBudget", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceCurrentVolumeBudget").Value = MaterialPriceCurrentVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceByVolume").Value = MaterialPriceCurrentPriceByVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceByVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceByVolumeBudget").Value = MaterialPriceCurrentPriceByVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightByVolume").Value = MaterialPriceCurrentFreightByVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightByVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightByVolumeBudget").Value = MaterialPriceCurrentFreightByVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLanded", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLanded").Value = MaterialPriceCurrentMaterialLanded

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedBudget").Value = MaterialPriceCurrentMaterialLandedBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedTotal").Value = MaterialPriceCurrentMaterialLandedTotal

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedTotalBudget").Value = MaterialPriceCurrentMaterialLandedTotalBudget

            myCommand.Parameters.Add("@MaterialPriceProposedPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedPrice").Value = MaterialPriceProposedPrice

            myCommand.Parameters.Add("@MaterialPriceProposedFreight", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedFreight").Value = MaterialPriceProposedFreight

            myCommand.Parameters.Add("@MaterialPriceProposedVolume", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceProposedVolume").Value = MaterialPriceProposedVolume

            myCommand.Parameters.Add("@MaterialPriceProposedPriceByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedPriceByVolume").Value = MaterialPriceProposedPriceByVolume

            myCommand.Parameters.Add("@MaterialPriceProposedFreightByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedFreightByVolume").Value = MaterialPriceProposedFreightByVolume

            myCommand.Parameters.Add("@MaterialPriceProposedMaterialLanded", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMaterialLanded").Value = MaterialPriceProposedMaterialLanded

            myCommand.Parameters.Add("@MaterialPriceProposedMaterialLandedTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMaterialLandedTotal").Value = MaterialPriceProposedMaterialLandedTotal

            myCommand.Parameters.Add("@MaterialPriceCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMethod").Value = MaterialPriceCurrentMethod

            myCommand.Parameters.Add("@MaterialPriceCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMethodBudget").Value = MaterialPriceCurrentMethodBudget

            myCommand.Parameters.Add("@MaterialPriceProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMethod").Value = MaterialPriceProposedMethod

            myCommand.Parameters.Add("@MaterialPriceSavings", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceSavings").Value = MaterialPriceSavings

            myCommand.Parameters.Add("@MaterialPriceSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceSavingsBudget").Value = MaterialPriceSavingsBudget

            myCommand.Parameters.Add("@MaterialPriceCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCECapital").Value = MaterialPriceCECapital

            myCommand.Parameters.Add("@MaterialPriceCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEMaterial").Value = MaterialPriceCEMaterial

            myCommand.Parameters.Add("@MaterialPriceCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEOutsideSupport").Value = MaterialPriceCEOutsideSupport

            myCommand.Parameters.Add("@MaterialPriceCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEMisc").Value = MaterialPriceCEMisc

            myCommand.Parameters.Add("@MaterialPriceCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEInHouseSupport").Value = MaterialPriceCEInHouseSupport

            myCommand.Parameters.Add("@MaterialPriceCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCETotal").Value = MaterialPriceCETotal

            myCommand.Parameters.Add("@MaterialPricePayback", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPricePayback").Value = MaterialPricePayback

            myCommand.Parameters.Add("@MaterialUsageCurrentCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostPerUnit").Value = MaterialUsageCurrentCostPerUnit

            myCommand.Parameters.Add("@MaterialUsageCurrentCostPerUnitBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostPerUnitBudget").Value = MaterialUsageCurrentCostPerUnitBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentUnitPerParent", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentUnitPerParent").Value = MaterialUsageCurrentUnitPerParent

            myCommand.Parameters.Add("@MaterialUsageCurrentUnitPerParentBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentUnitPerParentBudget").Value = MaterialUsageCurrentUnitPerParentBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostTotal").Value = MaterialUsageCurrentCostTotal

            myCommand.Parameters.Add("@MaterialUsageCurrentCostTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostTotalBudget").Value = MaterialUsageCurrentCostTotalBudget

            myCommand.Parameters.Add("@MaterialUsageProposedCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedCostPerUnit").Value = MaterialUsageProposedCostPerUnit

            myCommand.Parameters.Add("@MaterialUsageProposedUnitPerParent", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedUnitPerParent").Value = MaterialUsageProposedUnitPerParent

            myCommand.Parameters.Add("@MaterialUsageProposedCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedCostTotal").Value = MaterialUsageProposedCostTotal

            myCommand.Parameters.Add("@MaterialUsageProgramVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProgramVolume").Value = MaterialUsageProgramVolume

            myCommand.Parameters.Add("@MaterialUsageProgramVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProgramVolumeBudget").Value = MaterialUsageProgramVolumeBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentMethod").Value = MaterialUsageCurrentMethod

            myCommand.Parameters.Add("@MaterialUsageCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentMethodBudget").Value = MaterialUsageCurrentMethodBudget

            myCommand.Parameters.Add("@MaterialUsageProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedMethod").Value = MaterialUsageProposedMethod

            myCommand.Parameters.Add("@MaterialUsageSavings", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageSavings").Value = MaterialUsageSavings

            myCommand.Parameters.Add("@MaterialUsageSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageSavingsBudget").Value = MaterialUsageSavingsBudget

            myCommand.Parameters.Add("@MaterialUsageCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCECapital").Value = MaterialUsageCECapital

            myCommand.Parameters.Add("@MaterialUsageCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEMaterial").Value = MaterialUsageCEMaterial

            myCommand.Parameters.Add("@MaterialUsageCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEOutsideSupport").Value = MaterialUsageCEOutsideSupport

            myCommand.Parameters.Add("@MaterialUsageCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEMisc").Value = MaterialUsageCEMisc

            myCommand.Parameters.Add("@MaterialUsageCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEInHouseSupport").Value = MaterialUsageCEInHouseSupport

            myCommand.Parameters.Add("@MaterialUsageCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCETotal").Value = MaterialUsageCETotal

            myCommand.Parameters.Add("@MaterialUsagePayback", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsagePayback").Value = MaterialUsagePayback

            myCommand.Parameters.Add("@CycleTimeCurrentPiecesPerHour", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentPiecesPerHour").Value = CycleTimeCurrentPiecesPerHour

            myCommand.Parameters.Add("@CycleTimeCurrentPiecesPerHourBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentPiecesPerHourBudget").Value = CycleTimeCurrentPiecesPerHourBudget

            myCommand.Parameters.Add("@CycleTimeCurrentCrewSize", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentCrewSize").Value = CycleTimeCurrentCrewSize

            myCommand.Parameters.Add("@CycleTimeCurrentCrewSizeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentCrewSizeBudget").Value = CycleTimeCurrentCrewSizeBudget

            myCommand.Parameters.Add("@CycleTimeCurrentVolume", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeCurrentVolume").Value = CycleTimeCurrentVolume

            myCommand.Parameters.Add("@CycleTimeCurrentVolumeBudget", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeCurrentVolumeBudget").Value = CycleTimeCurrentVolumeBudget

            myCommand.Parameters.Add("@CycleTimeCurrentMachineHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMachineHourPerPieces").Value = CycleTimeCurrentMachineHourPerPieces

            myCommand.Parameters.Add("@CycleTimeCurrentMachineHourPerPiecesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMachineHourPerPiecesBudget").Value = CycleTimeCurrentMachineHourPerPiecesBudget

            myCommand.Parameters.Add("@CycleTimeCurrentManHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentManHourPerPieces").Value = CycleTimeCurrentManHourPerPieces

            myCommand.Parameters.Add("@CycleTimeCurrentManHourPerPiecesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentManHourPerPiecesBudget").Value = CycleTimeCurrentManHourPerPiecesBudget

            myCommand.Parameters.Add("@CycleTimeCurrentTotalManHours", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentTotalManHours").Value = CycleTimeCurrentTotalManHours

            myCommand.Parameters.Add("@CycleTimeCurrentTotalManHoursBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentTotalManHoursBudget").Value = CycleTimeCurrentTotalManHoursBudget

            myCommand.Parameters.Add("@CycleTimeProposedPiecesPerHour", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedPiecesPerHour").Value = CycleTimeProposedPiecesPerHour

            myCommand.Parameters.Add("@CycleTimeProposedCrewSize", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedCrewSize").Value = CycleTimeProposedCrewSize

            myCommand.Parameters.Add("@CycleTimeProposedVolume", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeProposedVolume").Value = CycleTimeProposedVolume

            myCommand.Parameters.Add("@CycleTimeProposedMachineHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedMachineHourPerPieces").Value = CycleTimeProposedMachineHourPerPieces

            myCommand.Parameters.Add("@CycleTimeProposedManHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedManHourPerPieces").Value = CycleTimeProposedManHourPerPieces

            myCommand.Parameters.Add("@CycleTimeProposedTotalManHours", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedTotalManHours").Value = CycleTimeProposedTotalManHours

            myCommand.Parameters.Add("@CycleTimeFUTARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeFUTARate").Value = CycleTimeFUTARate

            myCommand.Parameters.Add("@CycleTimeSUTARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSUTARate").Value = CycleTimeSUTARate

            myCommand.Parameters.Add("@CycleTimeFICARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeFICARate").Value = CycleTimeFICARate

            myCommand.Parameters.Add("@CycleTimeVariableFringes", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeVariableFringes").Value = CycleTimeVariableFringes

            myCommand.Parameters.Add("@CycleTimeWages", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeWages").Value = CycleTimeWages

            myCommand.Parameters.Add("@CycleTimeWagesPlusFringes", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeWagesPlusFringes").Value = CycleTimeWagesPlusFringes

            myCommand.Parameters.Add("@CycleTimeCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMethod").Value = CycleTimeCurrentMethod

            myCommand.Parameters.Add("@CycleTimeCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMethodBudget").Value = CycleTimeCurrentMethodBudget

            myCommand.Parameters.Add("@CycleTimeProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedMethod").Value = CycleTimeProposedMethod

            myCommand.Parameters.Add("@CycleTimeMethodDifference", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeMethodDifference").Value = CycleTimeMethodDifference

            myCommand.Parameters.Add("@CycleTimeMethodDifferenceBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeMethodDifferenceBudget").Value = CycleTimeMethodDifferenceBudget

            myCommand.Parameters.Add("@CycleTimeSavings", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSavings").Value = CycleTimeSavings

            myCommand.Parameters.Add("@CycleTimeSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSavingsBudget").Value = CycleTimeSavingsBudget

            myCommand.Parameters.Add("@CycleTimeCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCECapital").Value = CycleTimeCECapital

            myCommand.Parameters.Add("@CycleTimeCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEMaterial").Value = CycleTimeCEMaterial

            myCommand.Parameters.Add("@CycleTimeCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEOutsideSupport").Value = CycleTimeCEOutsideSupport

            myCommand.Parameters.Add("@CycleTimeCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEMisc").Value = CycleTimeCEMisc

            myCommand.Parameters.Add("@CycleTimeCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEInHouseSupport").Value = CycleTimeCEInHouseSupport

            myCommand.Parameters.Add("@CycleTimeCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCETotal").Value = CycleTimeCETotal

            myCommand.Parameters.Add("@CycleTimePayback", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimePayback").Value = CycleTimePayback

            myCommand.Parameters.Add("@HeadCountWages", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWages").Value = HeadCountWages

            myCommand.Parameters.Add("@HeadCountWagesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWagesBudget").Value = HeadCountWagesBudget

            myCommand.Parameters.Add("@HeadCountAnnualLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountAnnualLaborCost").Value = HeadCountAnnualLaborCost

            myCommand.Parameters.Add("@HeadCountAnnualLaborCostBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountAnnualLaborCostBudget").Value = HeadCountAnnualLaborCostBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborCount", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCount").Value = HeadCountCurrentLaborCount

            myCommand.Parameters.Add("@HeadCountCurrentLaborCountBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCountBudget").Value = HeadCountCurrentLaborCountBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCost").Value = HeadCountCurrentLaborCost

            myCommand.Parameters.Add("@HeadCountCurrentLaborCostBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCostBudget").Value = HeadCountCurrentLaborCostBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborFringes").Value = HeadCountCurrentLaborFringes

            myCommand.Parameters.Add("@HeadCountCurrentLaborTotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborTotal").Value = HeadCountCurrentLaborTotal

            myCommand.Parameters.Add("@HeadCountCurrentLaborTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborTotalBudget").Value = HeadCountCurrentLaborTotalBudget

            myCommand.Parameters.Add("@HeadCountProposedLaborCount", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborCount").Value = HeadCountProposedLaborCount

            myCommand.Parameters.Add("@HeadCountProposedLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborCost").Value = HeadCountProposedLaborCost

            myCommand.Parameters.Add("@HeadCountProposedLaborFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborFringes").Value = HeadCountProposedLaborFringes

            myCommand.Parameters.Add("@HeadCountProposedLaborTotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborTotal").Value = HeadCountProposedLaborTotal

            myCommand.Parameters.Add("@HeadCountCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentMethod").Value = HeadCountCurrentMethod

            myCommand.Parameters.Add("@HeadCountCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentMethodBudget").Value = HeadCountCurrentMethodBudget

            myCommand.Parameters.Add("@HeadCountProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedMethod").Value = HeadCountProposedMethod

            myCommand.Parameters.Add("@HeadCountSavings", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSavings").Value = HeadCountSavings

            myCommand.Parameters.Add("@HeadCountSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSavingsBudget").Value = HeadCountSavingsBudget

            myCommand.Parameters.Add("@HeadCountFUTA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountFUTA").Value = HeadCountFUTA

            myCommand.Parameters.Add("@HeadCountSUTA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSUTA").Value = HeadCountSUTA

            myCommand.Parameters.Add("@HeadCountFICA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountFICA").Value = HeadCountFICA

            myCommand.Parameters.Add("@HeadCountPension", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPension").Value = HeadCountPension

            myCommand.Parameters.Add("@HeadCountBonus", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountBonus").Value = HeadCountBonus

            myCommand.Parameters.Add("@HeadCountLife", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountLife").Value = HeadCountLife

            myCommand.Parameters.Add("@HeadCountGroupInsurance", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountGroupInsurance").Value = HeadCountGroupInsurance

            myCommand.Parameters.Add("@HeadCountWorkersComp", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWorkersComp").Value = HeadCountWorkersComp

            myCommand.Parameters.Add("@HeadCountPensionQuarterly", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPensionQuarterly").Value = HeadCountPensionQuarterly

            myCommand.Parameters.Add("@HeadCountTotalFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountTotalFringes").Value = HeadCountTotalFringes

            myCommand.Parameters.Add("@HeadCountCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCECapital").Value = HeadCountCECapital

            myCommand.Parameters.Add("@HeadCountCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEMaterial").Value = HeadCountCEMaterial

            myCommand.Parameters.Add("@HeadCountCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEOutsideSupport").Value = HeadCountCEOutsideSupport

            myCommand.Parameters.Add("@HeadCountCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEMisc").Value = HeadCountCEMisc

            myCommand.Parameters.Add("@HeadCountCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEInHouseSupport").Value = HeadCountCEInHouseSupport

            myCommand.Parameters.Add("@HeadCountCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCETotal").Value = HeadCountCETotal

            myCommand.Parameters.Add("@HeadCountPayback", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPayback").Value = HeadCountPayback

            myCommand.Parameters.Add("@OverheadCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCurrentMethod").Value = OverheadCurrentMethod

            myCommand.Parameters.Add("@OverheadCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCurrentMethodBudget").Value = OverheadCurrentMethodBudget

            myCommand.Parameters.Add("@OverheadProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadProposedMethod").Value = OverheadProposedMethod

            myCommand.Parameters.Add("@OverheadSavings", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadSavings").Value = OverheadSavings

            myCommand.Parameters.Add("@OverheadSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadSavingsBudget").Value = OverheadSavingsBudget

            myCommand.Parameters.Add("@OverheadCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCECapital").Value = OverheadCECapital

            myCommand.Parameters.Add("@OverheadCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEMaterial").Value = OverheadCEMaterial

            myCommand.Parameters.Add("@OverheadCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEOutsideSupport").Value = OverheadCEOutsideSupport

            myCommand.Parameters.Add("@OverheadCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEMisc").Value = OverheadCEMisc

            myCommand.Parameters.Add("@OverheadCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEInHouseSupport").Value = OverheadCEInHouseSupport

            myCommand.Parameters.Add("@OverheadCEWriteOff", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEWriteOff").Value = OverheadCEWriteOff

            myCommand.Parameters.Add("@OverheadCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCETotal").Value = OverheadCETotal

            myCommand.Parameters.Add("@OverheadPayback", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadPayback").Value = OverheadPayback

            myCommand.Parameters.Add("@TotalSavings", SqlDbType.Decimal)
            myCommand.Parameters("@TotalSavings").Value = TotalSavings

            myCommand.Parameters.Add("@TotalSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@TotalSavingsBudget").Value = TotalSavingsBudget

            myCommand.Parameters.Add("@TotalCE", SqlDbType.Decimal)
            myCommand.Parameters("@TotalCE").Value = TotalCE

            myCommand.Parameters.Add("@TotalPayback", SqlDbType.Decimal)
            myCommand.Parameters("@TotalPayback").Value = TotalPayback

            myCommand.Parameters.Add("@CustomerGiveBackDollar", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerGiveBackDollar").Value = CustomerGiveBackDollar

            myCommand.Parameters.Add("@CustomerGiveBackPercent", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerGiveBackPercent").Value = CustomerGiveBackPercent

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo _
            & ", CurrentMethod: " & CurrentMethod _
            & ", ProposedMethod: " & ProposedMethod _
            & ", Benefits: " & Benefits _
            & ", CustomerPartNo: " & CustomerPartNo _
            & ", MaterialPriceCurrentPrice: " & MaterialPriceCurrentPrice _
            & ", MaterialPriceCurrentPriceBudget: " & MaterialPriceCurrentPriceBudget _
            & ", MaterialPriceCurrentFreight: " & MaterialPriceCurrentFreight _
            & ", MaterialPriceCurrentFreightBudget: " & MaterialPriceCurrentFreightBudget _
            & ", MaterialPriceCurrentVolume: " & MaterialPriceCurrentVolume _
            & ", MaterialPriceCurrentVolumeBudget: " & MaterialPriceCurrentVolumeBudget _
            & ", MaterialPriceCurrentPriceByVolume: " & MaterialPriceCurrentPriceByVolume _
            & ", MaterialPriceCurrentPriceByVolumeBudget: " & MaterialPriceCurrentPriceByVolumeBudget _
            & ", MaterialPriceCurrentFreightByVolume: " & MaterialPriceCurrentFreightByVolume _
            & ", MaterialPriceCurrentFreightByVolumeBudget: " & MaterialPriceCurrentFreightByVolumeBudget _
            & ", MaterialPriceCurrentMaterialLanded: " & MaterialPriceCurrentMaterialLanded _
            & ", MaterialPriceCurrentMaterialLandedBudget: " & MaterialPriceCurrentMaterialLandedBudget _
            & ", MaterialPriceCurrentMaterialLandedTotal: " & MaterialPriceCurrentMaterialLandedTotal _
            & ", MaterialPriceCurrentMaterialLandedTotalBudget: " & MaterialPriceCurrentMaterialLandedTotalBudget _
            & ", MaterialPriceProposedPrice: " & MaterialPriceProposedPrice _
            & ", MaterialPriceProposedFreight: " & MaterialPriceProposedFreight _
            & ", MaterialPriceProposedVolume: " & MaterialPriceProposedVolume _
            & ", MaterialPriceProposedPriceByVolume: " & MaterialPriceProposedPriceByVolume _
            & ", MaterialPriceProposedFreightByVolume: " & MaterialPriceProposedFreightByVolume _
            & ", MaterialPriceProposedMaterialLanded: " & MaterialPriceProposedMaterialLanded _
            & ", MaterialPriceProposedMaterialLandedTotal: " & MaterialPriceProposedMaterialLandedTotal _
            & ", MaterialPriceCurrentMethod: " & MaterialPriceCurrentMethod _
            & ", MaterialPriceCurrentMethodBudget: " & MaterialPriceCurrentMethodBudget _
            & ", MaterialPriceProposedMethod: " & MaterialPriceProposedMethod _
            & ", MaterialPriceSavings: " & MaterialPriceSavings _
            & ", MaterialPriceSavingsBudget: " & MaterialPriceSavingsBudget _
            & ", MaterialPriceCECapital: " & MaterialPriceCECapital _
            & ", MaterialPriceCEMaterial: " & MaterialPriceCEMaterial _
            & ", MaterialPriceCEOutsideSupport: " & MaterialPriceCEOutsideSupport _
            & ", MaterialPriceCEMisc: " & MaterialPriceCEMisc _
            & ", MaterialPriceCEInHouseSupport: " & MaterialPriceCEInHouseSupport _
            & ", MaterialPriceCETotal: " & MaterialPriceCETotal _
            & ", MaterialPricePayback: " & MaterialPricePayback _
            & ", MaterialUsageCurrentCostPerUnit: " & MaterialUsageCurrentCostPerUnit _
            & ", MaterialUsageCurrentCostPerUnitBudget: " & MaterialUsageCurrentCostPerUnitBudget _
            & ", MaterialUsageCurrentUnitPerParent: " & MaterialUsageCurrentUnitPerParent _
            & ", MaterialUsageCurrentUnitPerParentBudget: " & MaterialUsageCurrentUnitPerParentBudget _
            & ", MaterialUsageCurrentCostTotal: " & MaterialUsageCurrentCostTotal _
            & ", MaterialUsageCurrentCostTotalBudget: " & MaterialUsageCurrentCostTotalBudget _
            & ", MaterialUsageProposedCostPerUnit: " & MaterialUsageProposedCostPerUnit _
            & ", MaterialUsageProposedUnitPerParent: " & MaterialUsageProposedUnitPerParent _
            & ", MaterialUsageProposedCostTotal: " & MaterialUsageProposedCostTotal _
            & ", MaterialUsageProgramVolume: " & MaterialUsageProgramVolume _
            & ", MaterialUsageProgramVolumeBudget: " & MaterialUsageProgramVolumeBudget _
            & ", MaterialUsageCurrentMethod: " & MaterialUsageCurrentMethod _
            & ", MaterialUsageCurrentMethodBudget: " & MaterialUsageCurrentMethodBudget _
            & ", MaterialUsageProposedMethod: " & MaterialUsageProposedMethod _
            & ", MaterialUsageSavings: " & MaterialUsageSavings _
            & ", MaterialUsageSavingsBudget: " & MaterialUsageSavingsBudget _
            & ", MaterialUsageCECapital: " & MaterialUsageCECapital _
            & ", MaterialUsageCEMaterial: " & MaterialUsageCEMaterial _
            & ", MaterialUsageCEOutsideSupport: " & MaterialUsageCEOutsideSupport _
            & ", MaterialUsageCEMisc: " & MaterialUsageCEMisc _
            & ", MaterialUsageCEInHouseSupport: " & MaterialUsageCEInHouseSupport _
            & ", MaterialUsageCETotal: " & MaterialUsageCETotal _
            & ", MaterialUsagePayback: " & MaterialUsagePayback _
            & ", CycleTimeCurrentPiecesPerHour: " & CycleTimeCurrentPiecesPerHour _
            & ", CycleTimeCurrentPiecesPerHourBudget: " & CycleTimeCurrentPiecesPerHourBudget _
            & ", CycleTimeCurrentCrewSize: " & CycleTimeCurrentCrewSize _
            & ", CycleTimeCurrentCrewSizeBudget: " & CycleTimeCurrentCrewSizeBudget _
            & ", CycleTimeCurrentVolume: " & CycleTimeCurrentVolume _
            & ", CycleTimeCurrentVolumeBudget: " & CycleTimeCurrentVolumeBudget _
            & ", CycleTimeCurrentMachineHourPerPieces: " & CycleTimeCurrentMachineHourPerPieces _
            & ", CycleTimeCurrentMachineHourPerPiecesBudget: " & CycleTimeCurrentMachineHourPerPiecesBudget _
            & ", CycleTimeCurrentManHourPerPieces: " & CycleTimeCurrentManHourPerPieces _
            & ", CycleTimeCurrentManHourPerPiecesBudget: " & CycleTimeCurrentManHourPerPiecesBudget _
            & ", CycleTimeCurrentTotalManHours: " & CycleTimeCurrentTotalManHours _
            & ", CycleTimeCurrentTotalManHoursBudget: " & CycleTimeCurrentTotalManHoursBudget _
            & ", CycleTimeProposedPiecesPerHour: " & CycleTimeProposedPiecesPerHour _
            & ", CycleTimeProposedCrewSize: " & CycleTimeProposedCrewSize _
            & ", CycleTimeProposedVolume: " & CycleTimeProposedVolume _
            & ", CycleTimeProposedMachineHourPerPieces: " & CycleTimeProposedMachineHourPerPieces _
            & ", CycleTimeProposedManHourPerPieces: " & CycleTimeProposedManHourPerPieces _
            & ", CycleTimeProposedTotalManHours: " & CycleTimeProposedTotalManHours _
            & ", CycleTimeFUTARate: " & CycleTimeFUTARate _
            & ", CycleTimeSUTARate: " & CycleTimeSUTARate _
            & ", CycleTimeFICARate: " & CycleTimeFICARate _
            & ", CycleTimeVariableFringes: " & CycleTimeVariableFringes _
            & ", CycleTimeWages: " & CycleTimeWages _
            & ", CycleTimeWagesPlusFringes: " & CycleTimeWagesPlusFringes _
            & ", CycleTimeCurrentMethod: " & CycleTimeCurrentMethod _
            & ", CycleTimeCurrentMethodBudget: " & CycleTimeCurrentMethodBudget _
            & ", CycleTimeProposedMethod: " & CycleTimeProposedMethod _
            & ", CycleTimeMethodDifference: " & CycleTimeMethodDifference _
            & ", CycleTimeMethodDifferenceBudget: " & CycleTimeMethodDifferenceBudget _
            & ", CycleTimeSavings: " & CycleTimeSavings _
            & ", CycleTimeSavingsBudget: " & CycleTimeSavingsBudget _
            & ", CycleTimeCECapital: " & CycleTimeCECapital _
            & ", CycleTimeCEMaterial: " & CycleTimeCEMaterial _
            & ", CycleTimeCEOutsideSupport: " & CycleTimeCEOutsideSupport _
            & ", CycleTimeCEMisc: " & CycleTimeCEMisc _
            & ", CycleTimeCEInHouseSupport: " & CycleTimeCEInHouseSupport _
            & ", CycleTimeCETotal: " & CycleTimeCETotal _
            & ", CycleTimePayback: " & CycleTimePayback _
            & ", HeadCountWages: " & HeadCountWages _
            & ", HeadCountWagesBudget: " & HeadCountWagesBudget _
            & ", HeadCountAnnualLaborCost: " & HeadCountAnnualLaborCost _
            & ", HeadCountAnnualLaborCostBudget: " & HeadCountAnnualLaborCostBudget _
            & ", HeadCountCurrentLaborCount: " & HeadCountCurrentLaborCount _
            & ", HeadCountCurrentLaborCountBudget: " & HeadCountCurrentLaborCountBudget _
            & ", HeadCountCurrentLaborCost: " & HeadCountCurrentLaborCost _
            & ", HeadCountCurrentLaborCostBudget: " & HeadCountCurrentLaborCostBudget _
            & ", HeadCountCurrentLaborFringes: " & HeadCountCurrentLaborFringes _
            & ", HeadCountCurrentLaborTotal: " & HeadCountCurrentLaborTotal _
            & ", HeadCountCurrentLaborTotalBudget: " & HeadCountCurrentLaborTotalBudget _
            & ", HeadCountProposedLaborCount: " & HeadCountProposedLaborCount _
            & ", HeadCountProposedLaborCost: " & HeadCountProposedLaborCost _
            & ", HeadCountProposedLaborFringes: " & HeadCountProposedLaborFringes _
            & ", HeadCountProposedLaborTotal: " & HeadCountProposedLaborTotal _
            & ", HeadCountCurrentMethod: " & HeadCountCurrentMethod _
            & ", HeadCountCurrentMethodBudget: " & HeadCountCurrentMethodBudget _
            & ", HeadCountProposedMethod: " & HeadCountProposedMethod _
            & ", HeadCountSavings: " & HeadCountSavings _
            & ", HeadCountSavingsBudget: " & HeadCountSavingsBudget _
            & ", HeadCountFUTA: " & HeadCountFUTA _
            & ", HeadCountSUTA: " & HeadCountSUTA _
            & ", HeadCountFICA: " & HeadCountFICA _
            & ", HeadCountPension: " & HeadCountPension _
            & ", HeadCountBonus: " & HeadCountBonus _
            & ", HeadCountLife: " & HeadCountLife _
            & ", HeadCountGroupInsurance: " & HeadCountGroupInsurance _
            & ", HeadCountWorkersComp: " & HeadCountWorkersComp _
            & ", HeadCountPensionQuarterly: " & HeadCountPensionQuarterly _
            & ", HeadCountTotalFringes: " & HeadCountTotalFringes _
            & ", HeadCountCECapital: " & HeadCountCECapital _
            & ", HeadCountCEMaterial: " & HeadCountCEMaterial _
            & ", HeadCountCEOutsideSupport: " & HeadCountCEOutsideSupport _
            & ", HeadCountCEMisc: " & HeadCountCEMisc _
            & ", HeadCountCEInHouseSupport: " & HeadCountCEInHouseSupport _
            & ", HeadCountCETotal: " & HeadCountCETotal _
            & ", HeadCountPayback: " & HeadCountPayback _
            & ", OverheadCurrentMethod: " & OverheadCurrentMethod _
            & ", OverheadCurrentMethodBudget: " & OverheadCurrentMethodBudget _
            & ", OverheadProposedMethod: " & OverheadProposedMethod _
            & ", OverheadSavings: " & OverheadSavings _
            & ", OverheadSavingsBudget: " & OverheadSavingsBudget _
            & ", OverheadCECapital: " & OverheadCECapital _
            & ", OverheadCEMaterial: " & OverheadCEMaterial _
            & ", OverheadCEOutsideSupport: " & OverheadCEOutsideSupport _
            & ", OverheadCEMisc: " & OverheadCEMisc _
            & ", OverheadCEInHouseSupport: " & OverheadCEInHouseSupport _
            & ", OverheadCEWriteOff: " & OverheadCEWriteOff _
            & ", OverheadCETotal: " & OverheadCETotal _
            & ", OverheadPayback: " & OverheadPayback _
            & ", TotalSavings: " & TotalSavings _
            & ", TotalSavingsBudget: " & TotalSavingsBudget _
            & ", TotalCE: " & TotalCE _
            & ", TotalPayback: " & TotalPayback _
            & ", CustomerGiveBackDollar: " & CustomerGiveBackDollar _
            & ", CustomerGiveBackPercent: " & CustomerGiveBackPercent _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub ' EOF InsertCostReductionDetail

    Public Shared Function CopyCostReduction(ByVal ProjectNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Reduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OldProjectNo", SqlDbType.Int)
            myCommand.Parameters("@OldProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewCostReduction")

            CopyCostReduction = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo

            HttpContext.Current.Session("BLLerror") = "CopyCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"

            UGNErrorTrapping.InsertErrorLog("CopyCostReduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            CopyCostReduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionStatus

    Public Shared Sub UpdateCostReductionDetail(ByVal ProjectNo As Integer, _
       ByVal CurrentMethod As String, _
       ByVal ProposedMethod As String, _
       ByVal Benefits As String, _
       ByVal CustomerPartNo As String, _
       ByVal MaterialPriceCurrentPrice As Double, _
       ByVal MaterialPriceCurrentPriceBudget As Double, _
       ByVal MaterialPriceCurrentFreight As Double, _
       ByVal MaterialPriceCurrentFreightBudget As Double, _
       ByVal MaterialPriceCurrentVolume As Integer, _
       ByVal MaterialPriceCurrentVolumeBudget As Integer, _
       ByVal MaterialPriceCurrentPriceByVolume As Double, _
       ByVal MaterialPriceCurrentPriceByVolumeBudget As Double, _
       ByVal MaterialPriceCurrentFreightByVolume As Double, _
       ByVal MaterialPriceCurrentFreightByVolumeBudget As Double, _
       ByVal MaterialPriceCurrentMaterialLanded As Double, _
       ByVal MaterialPriceCurrentMaterialLandedBudget As Double, _
       ByVal MaterialPriceCurrentMaterialLandedTotal As Double, _
       ByVal MaterialPriceCurrentMaterialLandedTotalBudget As Double, _
       ByVal MaterialPriceProposedPrice As Double, _
       ByVal MaterialPriceProposedFreight As Double, _
       ByVal MaterialPriceProposedVolume As Integer, _
       ByVal MaterialPriceProposedPriceByVolume As Double, _
       ByVal MaterialPriceProposedFreightByVolume As Double, _
       ByVal MaterialPriceProposedMaterialLanded As Double, _
       ByVal MaterialPriceProposedMaterialLandedTotal As Double, _
       ByVal MaterialPriceCurrentMethod As Double, _
       ByVal MaterialPriceCurrentMethodBudget As Double, _
       ByVal MaterialPriceProposedMethod As Double, _
       ByVal MaterialPriceSavings As Double, _
       ByVal MaterialPriceSavingsBudget As Double, _
       ByVal MaterialPriceCECapital As Double, _
       ByVal MaterialPriceCEMaterial As Double, _
       ByVal MaterialPriceCEOutsideSupport As Double, _
       ByVal MaterialPriceCEMisc As Double, _
       ByVal MaterialPriceCEInHouseSupport As Double, _
       ByVal MaterialPriceCETotal As Double, _
       ByVal MaterialPricePayback As Double, _
       ByVal MaterialUsageCurrentCostPerUnit As Double, _
       ByVal MaterialUsageCurrentCostPerUnitBudget As Double, _
       ByVal MaterialUsageCurrentUnitPerParent As Double, _
       ByVal MaterialUsageCurrentUnitPerParentBudget As Double, _
       ByVal MaterialUsageCurrentCostTotal As Double, _
       ByVal MaterialUsageCurrentCostTotalBudget As Double, _
       ByVal MaterialUsageProposedCostPerUnit As Double, _
       ByVal MaterialUsageProposedUnitPerParent As Double, _
       ByVal MaterialUsageProposedCostTotal As Double, _
       ByVal MaterialUsageProgramVolume As Integer, _
       ByVal MaterialUsageProgramVolumeBudget As Integer, _
       ByVal MaterialUsageCurrentMethod As Double, _
       ByVal MaterialUsageCurrentMethodBudget As Double, _
       ByVal MaterialUsageProposedMethod As Double, _
       ByVal MaterialUsageSavings As Double, _
       ByVal MaterialUsageSavingsBudget As Double, _
       ByVal MaterialUsageCECapital As Double, _
       ByVal MaterialUsageCEMaterial As Double, _
       ByVal MaterialUsageCEOutsideSupport As Double, _
       ByVal MaterialUsageCEMisc As Double, _
       ByVal MaterialUsageCEInHouseSupport As Double, _
       ByVal MaterialUsageCETotal As Double, _
       ByVal MaterialUsagePayback As Double, _
       ByVal CycleTimeCurrentPiecesPerHour As Double, _
       ByVal CycleTimeCurrentPiecesPerHourBudget As Double, _
       ByVal CycleTimeCurrentCrewSize As Double, _
       ByVal CycleTimeCurrentCrewSizeBudget As Double, _
       ByVal CycleTimeCurrentVolume As Integer, _
       ByVal CycleTimeCurrentVolumeBudget As Integer, _
       ByVal CycleTimeCurrentMachineHourPerPieces As Double, _
       ByVal CycleTimeCurrentMachineHourPerPiecesBudget As Double, _
       ByVal CycleTimeCurrentManHourPerPieces As Double, _
       ByVal CycleTimeCurrentManHourPerPiecesBudget As Double, _
       ByVal CycleTimeCurrentTotalManHours As Double, _
       ByVal CycleTimeCurrentTotalManHoursBudget As Double, _
       ByVal CycleTimeProposedPiecesPerHour As Double, _
       ByVal CycleTimeProposedCrewSize As Double, _
       ByVal CycleTimeProposedVolume As Integer, _
       ByVal CycleTimeProposedMachineHourPerPieces As Double, _
       ByVal CycleTimeProposedManHourPerPieces As Double, _
       ByVal CycleTimeProposedTotalManHours As Double, _
       ByVal CycleTimeFUTARate As Double, _
       ByVal CycleTimeSUTARate As Double, _
       ByVal CycleTimeFICARate As Double, _
       ByVal CycleTimeVariableFringes As Double, _
       ByVal CycleTimeWages As Double, _
       ByVal CycleTimeWagesPlusFringes As Double, _
       ByVal CycleTimeCurrentMethod As Double, _
       ByVal CycleTimeCurrentMethodBudget As Double, _
       ByVal CycleTimeProposedMethod As Double, _
       ByVal CycleTimeMethodDifference As Double, _
       ByVal CycleTimeMethodDifferenceBudget As Double, _
       ByVal CycleTimeSavings As Double, _
       ByVal CycleTimeSavingsBudget As Double, _
       ByVal CycleTimeCECapital As Double, _
       ByVal CycleTimeCEMaterial As Double, _
       ByVal CycleTimeCEOutsideSupport As Double, _
       ByVal CycleTimeCEMisc As Double, _
       ByVal CycleTimeCEInHouseSupport As Double, _
       ByVal CycleTimeCETotal As Double, _
       ByVal CycleTimePayback As Double, _
       ByVal HeadCountWages As Double, _
       ByVal HeadCountWagesBudget As Double, _
       ByVal HeadCountAnnualLaborCost As Double, _
       ByVal HeadCountAnnualLaborCostBudget As Double, _
       ByVal HeadCountCurrentLaborCount As Double, _
       ByVal HeadCountCurrentLaborCountBudget As Double, _
       ByVal HeadCountCurrentLaborCost As Double, _
       ByVal HeadCountCurrentLaborCostBudget As Double, _
       ByVal HeadCountCurrentLaborFringes As Double, _
       ByVal HeadCountCurrentLaborTotal As Double, _
       ByVal HeadCountCurrentLaborTotalBudget As Double, _
       ByVal HeadCountProposedLaborCount As Double, _
       ByVal HeadCountProposedLaborCost As Double, _
       ByVal HeadCountProposedLaborFringes As Double, _
       ByVal HeadCountProposedLaborTotal As Double, _
       ByVal HeadCountCurrentMethod As Double, _
       ByVal HeadCountCurrentMethodBudget As Double, _
       ByVal HeadCountProposedMethod As Double, _
       ByVal HeadCountSavings As Double, _
       ByVal HeadCountSavingsBudget As Double, _
       ByVal HeadCountFUTA As Double, _
       ByVal HeadCountSUTA As Double, _
       ByVal HeadCountFICA As Double, _
       ByVal HeadCountPension As Double, _
       ByVal HeadCountBonus As Double, _
       ByVal HeadCountLife As Double, _
       ByVal HeadCountGroupInsurance As Double, _
       ByVal HeadCountWorkersComp As Double, _
       ByVal HeadCountPensionQuarterly As Double, _
       ByVal HeadCountTotalFringes As Double, _
       ByVal HeadCountCECapital As Double, _
       ByVal HeadCountCEMaterial As Double, _
       ByVal HeadCountCEOutsideSupport As Double, _
       ByVal HeadCountCEMisc As Double, _
       ByVal HeadCountCEInHouseSupport As Double, _
       ByVal HeadCountCETotal As Double, _
       ByVal HeadCountPayback As Double, _
       ByVal OverheadCurrentMethod As Double, _
       ByVal OverheadCurrentMethodBudget As Double, _
       ByVal OverheadProposedMethod As Double, _
       ByVal OverheadSavings As Double, _
       ByVal OverheadSavingsBudget As Double, _
       ByVal OverheadCECapital As Double, _
       ByVal OverheadCEMaterial As Double, _
       ByVal OverheadCEOutsideSupport As Double, _
       ByVal OverheadCEMisc As Double, _
       ByVal OverheadCEInHouseSupport As Double, _
       ByVal OverheadCEWriteOff As Double, _
       ByVal OverheadCETotal As Double, _
       ByVal OverheadPayback As Double, _
       ByVal TotalSavings As Double, _
       ByVal TotalSavingsBudget As Double, _
       ByVal TotalCE As Double, _
       ByVal TotalPayback As Double, _
       ByVal CustomerGiveBackDollar As Double, _
       ByVal CustomerGiveBackPercent As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Reduction_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.Int)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            If CurrentMethod Is Nothing Then
                CurrentMethod = ""
            End If

            myCommand.Parameters.Add("@CurrentMethod", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentMethod").Value = commonFunctions.convertSpecialChar(CurrentMethod, False)

            If ProposedMethod Is Nothing Then
                ProposedMethod = ""
            End If

            myCommand.Parameters.Add("@ProposedMethod", SqlDbType.VarChar)
            myCommand.Parameters("@ProposedMethod").Value = commonFunctions.convertSpecialChar(ProposedMethod, False)

            If Benefits Is Nothing Then
                Benefits = ""
            End If

            myCommand.Parameters.Add("@Benefits", SqlDbType.VarChar)
            myCommand.Parameters("@Benefits").Value = commonFunctions.convertSpecialChar(Benefits, False)

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = commonFunctions.convertSpecialChar(CustomerPartNo, False)

            myCommand.Parameters.Add("@MaterialPriceCurrentPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPrice").Value = MaterialPriceCurrentPrice

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceBudget").Value = MaterialPriceCurrentPriceBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentFreight", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreight").Value = MaterialPriceCurrentFreight

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightBudget").Value = MaterialPriceCurrentFreightBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentVolume", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceCurrentVolume").Value = MaterialPriceCurrentVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentVolumeBudget", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceCurrentVolumeBudget").Value = MaterialPriceCurrentVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceByVolume").Value = MaterialPriceCurrentPriceByVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentPriceByVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentPriceByVolumeBudget").Value = MaterialPriceCurrentPriceByVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightByVolume").Value = MaterialPriceCurrentFreightByVolume

            myCommand.Parameters.Add("@MaterialPriceCurrentFreightByVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentFreightByVolumeBudget").Value = MaterialPriceCurrentFreightByVolumeBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLanded", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLanded").Value = MaterialPriceCurrentMaterialLanded

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedBudget").Value = MaterialPriceCurrentMaterialLandedBudget

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedTotal").Value = MaterialPriceCurrentMaterialLandedTotal

            myCommand.Parameters.Add("@MaterialPriceCurrentMaterialLandedTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMaterialLandedTotalBudget").Value = MaterialPriceCurrentMaterialLandedTotalBudget

            myCommand.Parameters.Add("@MaterialPriceProposedPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedPrice").Value = MaterialPriceProposedPrice

            myCommand.Parameters.Add("@MaterialPriceProposedFreight", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedFreight").Value = MaterialPriceProposedFreight

            myCommand.Parameters.Add("@MaterialPriceProposedVolume", SqlDbType.Int)
            myCommand.Parameters("@MaterialPriceProposedVolume").Value = MaterialPriceProposedVolume

            myCommand.Parameters.Add("@MaterialPriceProposedPriceByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedPriceByVolume").Value = MaterialPriceProposedPriceByVolume

            myCommand.Parameters.Add("@MaterialPriceProposedFreightByVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedFreightByVolume").Value = MaterialPriceProposedFreightByVolume

            myCommand.Parameters.Add("@MaterialPriceProposedMaterialLanded", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMaterialLanded").Value = MaterialPriceProposedMaterialLanded

            myCommand.Parameters.Add("@MaterialPriceProposedMaterialLandedTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMaterialLandedTotal").Value = MaterialPriceProposedMaterialLandedTotal

            myCommand.Parameters.Add("@MaterialPriceCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMethod").Value = MaterialPriceCurrentMethod

            myCommand.Parameters.Add("@MaterialPriceCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCurrentMethodBudget").Value = MaterialPriceCurrentMethodBudget

            myCommand.Parameters.Add("@MaterialPriceProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceProposedMethod").Value = MaterialPriceProposedMethod

            myCommand.Parameters.Add("@MaterialPriceSavings", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceSavings").Value = MaterialPriceSavings

            myCommand.Parameters.Add("@MaterialPriceSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceSavingsBudget").Value = MaterialPriceSavingsBudget

            myCommand.Parameters.Add("@MaterialPriceCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCECapital").Value = MaterialPriceCECapital

            myCommand.Parameters.Add("@MaterialPriceCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEMaterial").Value = MaterialPriceCEMaterial

            myCommand.Parameters.Add("@MaterialPriceCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEOutsideSupport").Value = MaterialPriceCEOutsideSupport

            myCommand.Parameters.Add("@MaterialPriceCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEMisc").Value = MaterialPriceCEMisc

            myCommand.Parameters.Add("@MaterialPriceCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCEInHouseSupport").Value = MaterialPriceCEInHouseSupport

            myCommand.Parameters.Add("@MaterialPriceCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPriceCETotal").Value = MaterialPriceCETotal

            myCommand.Parameters.Add("@MaterialPricePayback", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialPricePayback").Value = MaterialPricePayback

            myCommand.Parameters.Add("@MaterialUsageCurrentCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostPerUnit").Value = MaterialUsageCurrentCostPerUnit

            myCommand.Parameters.Add("@MaterialUsageCurrentCostPerUnitBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostPerUnitBudget").Value = MaterialUsageCurrentCostPerUnitBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentUnitPerParent", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentUnitPerParent").Value = MaterialUsageCurrentUnitPerParent

            myCommand.Parameters.Add("@MaterialUsageCurrentUnitPerParentBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentUnitPerParentBudget").Value = MaterialUsageCurrentUnitPerParentBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostTotal").Value = MaterialUsageCurrentCostTotal

            myCommand.Parameters.Add("@MaterialUsageCurrentCostTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentCostTotalBudget").Value = MaterialUsageCurrentCostTotalBudget

            myCommand.Parameters.Add("@MaterialUsageProposedCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedCostPerUnit").Value = MaterialUsageProposedCostPerUnit

            myCommand.Parameters.Add("@MaterialUsageProposedUnitPerParent", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedUnitPerParent").Value = MaterialUsageProposedUnitPerParent

            myCommand.Parameters.Add("@MaterialUsageProposedCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedCostTotal").Value = MaterialUsageProposedCostTotal

            myCommand.Parameters.Add("@MaterialUsageProgramVolume", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProgramVolume").Value = MaterialUsageProgramVolume

            myCommand.Parameters.Add("@MaterialUsageProgramVolumeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProgramVolumeBudget").Value = MaterialUsageProgramVolumeBudget

            myCommand.Parameters.Add("@MaterialUsageCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentMethod").Value = MaterialUsageCurrentMethod

            myCommand.Parameters.Add("@MaterialUsageCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCurrentMethodBudget").Value = MaterialUsageCurrentMethodBudget

            myCommand.Parameters.Add("@MaterialUsageProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageProposedMethod").Value = MaterialUsageProposedMethod

            myCommand.Parameters.Add("@MaterialUsageSavings", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageSavings").Value = MaterialUsageSavings

            myCommand.Parameters.Add("@MaterialUsageSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageSavingsBudget").Value = MaterialUsageSavingsBudget

            myCommand.Parameters.Add("@MaterialUsageCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCECapital").Value = MaterialUsageCECapital

            myCommand.Parameters.Add("@MaterialUsageCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEMaterial").Value = MaterialUsageCEMaterial

            myCommand.Parameters.Add("@MaterialUsageCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEOutsideSupport").Value = MaterialUsageCEOutsideSupport

            myCommand.Parameters.Add("@MaterialUsageCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEMisc").Value = MaterialUsageCEMisc

            myCommand.Parameters.Add("@MaterialUsageCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCEInHouseSupport").Value = MaterialUsageCEInHouseSupport

            myCommand.Parameters.Add("@MaterialUsageCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsageCETotal").Value = MaterialUsageCETotal

            myCommand.Parameters.Add("@MaterialUsagePayback", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialUsagePayback").Value = MaterialUsagePayback

            myCommand.Parameters.Add("@CycleTimeCurrentPiecesPerHour", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentPiecesPerHour").Value = CycleTimeCurrentPiecesPerHour

            myCommand.Parameters.Add("@CycleTimeCurrentPiecesPerHourBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentPiecesPerHourBudget").Value = CycleTimeCurrentPiecesPerHourBudget

            myCommand.Parameters.Add("@CycleTimeCurrentCrewSize", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentCrewSize").Value = CycleTimeCurrentCrewSize

            myCommand.Parameters.Add("@CycleTimeCurrentCrewSizeBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentCrewSizeBudget").Value = CycleTimeCurrentCrewSizeBudget

            myCommand.Parameters.Add("@CycleTimeCurrentVolume", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeCurrentVolume").Value = CycleTimeCurrentVolume

            myCommand.Parameters.Add("@CycleTimeCurrentVolumeBudget", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeCurrentVolumeBudget").Value = CycleTimeCurrentVolumeBudget

            myCommand.Parameters.Add("@CycleTimeCurrentMachineHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMachineHourPerPieces").Value = CycleTimeCurrentMachineHourPerPieces

            myCommand.Parameters.Add("@CycleTimeCurrentMachineHourPerPiecesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMachineHourPerPiecesBudget").Value = CycleTimeCurrentMachineHourPerPiecesBudget

            myCommand.Parameters.Add("@CycleTimeCurrentManHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentManHourPerPieces").Value = CycleTimeCurrentManHourPerPieces

            myCommand.Parameters.Add("@CycleTimeCurrentManHourPerPiecesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentManHourPerPiecesBudget").Value = CycleTimeCurrentManHourPerPiecesBudget

            myCommand.Parameters.Add("@CycleTimeCurrentTotalManHours", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentTotalManHours").Value = CycleTimeCurrentTotalManHours

            myCommand.Parameters.Add("@CycleTimeCurrentTotalManHoursBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentTotalManHoursBudget").Value = CycleTimeCurrentTotalManHoursBudget

            myCommand.Parameters.Add("@CycleTimeProposedPiecesPerHour", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedPiecesPerHour").Value = CycleTimeProposedPiecesPerHour

            myCommand.Parameters.Add("@CycleTimeProposedCrewSize", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedCrewSize").Value = CycleTimeProposedCrewSize

            myCommand.Parameters.Add("@CycleTimeProposedVolume", SqlDbType.Int)
            myCommand.Parameters("@CycleTimeProposedVolume").Value = CycleTimeProposedVolume

            myCommand.Parameters.Add("@CycleTimeProposedMachineHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedMachineHourPerPieces").Value = CycleTimeProposedMachineHourPerPieces

            myCommand.Parameters.Add("@CycleTimeProposedManHourPerPieces", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedManHourPerPieces").Value = CycleTimeProposedManHourPerPieces

            myCommand.Parameters.Add("@CycleTimeProposedTotalManHours", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedTotalManHours").Value = CycleTimeProposedTotalManHours

            myCommand.Parameters.Add("@CycleTimeFUTARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeFUTARate").Value = CycleTimeFUTARate

            myCommand.Parameters.Add("@CycleTimeSUTARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSUTARate").Value = CycleTimeSUTARate

            myCommand.Parameters.Add("@CycleTimeFICARate", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeFICARate").Value = CycleTimeFICARate

            myCommand.Parameters.Add("@CycleTimeVariableFringes", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeVariableFringes").Value = CycleTimeVariableFringes

            myCommand.Parameters.Add("@CycleTimeWages", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeWages").Value = CycleTimeWages

            myCommand.Parameters.Add("@CycleTimeWagesPlusFringes", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeWagesPlusFringes").Value = CycleTimeWagesPlusFringes

            myCommand.Parameters.Add("@CycleTimeCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMethod").Value = CycleTimeCurrentMethod

            myCommand.Parameters.Add("@CycleTimeCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCurrentMethodBudget").Value = CycleTimeCurrentMethodBudget

            myCommand.Parameters.Add("@CycleTimeProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeProposedMethod").Value = CycleTimeProposedMethod

            myCommand.Parameters.Add("@CycleTimeMethodDifference", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeMethodDifference").Value = CycleTimeMethodDifference

            myCommand.Parameters.Add("@CycleTimeMethodDifferenceBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeMethodDifferenceBudget").Value = CycleTimeMethodDifferenceBudget

            myCommand.Parameters.Add("@CycleTimeSavings", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSavings").Value = CycleTimeSavings

            myCommand.Parameters.Add("@CycleTimeSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeSavingsBudget").Value = CycleTimeSavingsBudget

            myCommand.Parameters.Add("@CycleTimeCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCECapital").Value = CycleTimeCECapital

            myCommand.Parameters.Add("@CycleTimeCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEMaterial").Value = CycleTimeCEMaterial

            myCommand.Parameters.Add("@CycleTimeCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEOutsideSupport").Value = CycleTimeCEOutsideSupport

            myCommand.Parameters.Add("@CycleTimeCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEMisc").Value = CycleTimeCEMisc

            myCommand.Parameters.Add("@CycleTimeCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCEInHouseSupport").Value = CycleTimeCEInHouseSupport

            myCommand.Parameters.Add("@CycleTimeCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimeCETotal").Value = CycleTimeCETotal

            myCommand.Parameters.Add("@CycleTimePayback", SqlDbType.Decimal)
            myCommand.Parameters("@CycleTimePayback").Value = CycleTimePayback

            myCommand.Parameters.Add("@HeadCountWages", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWages").Value = HeadCountWages

            myCommand.Parameters.Add("@HeadCountWagesBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWagesBudget").Value = HeadCountWagesBudget

            myCommand.Parameters.Add("@HeadCountAnnualLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountAnnualLaborCost").Value = HeadCountAnnualLaborCost

            myCommand.Parameters.Add("@HeadCountAnnualLaborCostBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountAnnualLaborCostBudget").Value = HeadCountAnnualLaborCostBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborCount", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCount").Value = HeadCountCurrentLaborCount

            myCommand.Parameters.Add("@HeadCountCurrentLaborCountBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCountBudget").Value = HeadCountCurrentLaborCountBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCost").Value = HeadCountCurrentLaborCost

            myCommand.Parameters.Add("@HeadCountCurrentLaborCostBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborCostBudget").Value = HeadCountCurrentLaborCostBudget

            myCommand.Parameters.Add("@HeadCountCurrentLaborFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborFringes").Value = HeadCountCurrentLaborFringes

            myCommand.Parameters.Add("@HeadCountCurrentLaborTotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborTotal").Value = HeadCountCurrentLaborTotal

            myCommand.Parameters.Add("@HeadCountCurrentLaborTotalBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentLaborTotalBudget").Value = HeadCountCurrentLaborTotalBudget

            myCommand.Parameters.Add("@HeadCountProposedLaborCount", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborCount").Value = HeadCountProposedLaborCount

            myCommand.Parameters.Add("@HeadCountProposedLaborCost", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborCost").Value = HeadCountProposedLaborCost

            myCommand.Parameters.Add("@HeadCountProposedLaborFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborFringes").Value = HeadCountProposedLaborFringes

            myCommand.Parameters.Add("@HeadCountProposedLaborTotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedLaborTotal").Value = HeadCountProposedLaborTotal

            myCommand.Parameters.Add("@HeadCountCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentMethod").Value = HeadCountCurrentMethod

            myCommand.Parameters.Add("@HeadCountCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCurrentMethodBudget").Value = HeadCountCurrentMethodBudget

            myCommand.Parameters.Add("@HeadCountProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountProposedMethod").Value = HeadCountProposedMethod

            myCommand.Parameters.Add("@HeadCountSavings", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSavings").Value = HeadCountSavings

            myCommand.Parameters.Add("@HeadCountSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSavingsBudget").Value = HeadCountSavingsBudget

            myCommand.Parameters.Add("@HeadCountFUTA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountFUTA").Value = HeadCountFUTA

            myCommand.Parameters.Add("@HeadCountSUTA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountSUTA").Value = HeadCountSUTA

            myCommand.Parameters.Add("@HeadCountFICA", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountFICA").Value = HeadCountFICA

            myCommand.Parameters.Add("@HeadCountPension", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPension").Value = HeadCountPension

            myCommand.Parameters.Add("@HeadCountBonus", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountBonus").Value = HeadCountBonus

            myCommand.Parameters.Add("@HeadCountLife", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountLife").Value = HeadCountLife

            myCommand.Parameters.Add("@HeadCountGroupInsurance", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountGroupInsurance").Value = HeadCountGroupInsurance

            myCommand.Parameters.Add("@HeadCountWorkersComp", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountWorkersComp").Value = HeadCountWorkersComp

            myCommand.Parameters.Add("@HeadCountPensionQuarterly", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPensionQuarterly").Value = HeadCountPensionQuarterly

            myCommand.Parameters.Add("@HeadCountTotalFringes", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountTotalFringes").Value = HeadCountTotalFringes

            myCommand.Parameters.Add("@HeadCountCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCECapital").Value = HeadCountCECapital

            myCommand.Parameters.Add("@HeadCountCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEMaterial").Value = HeadCountCEMaterial

            myCommand.Parameters.Add("@HeadCountCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEOutsideSupport").Value = HeadCountCEOutsideSupport

            myCommand.Parameters.Add("@HeadCountCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEMisc").Value = HeadCountCEMisc

            myCommand.Parameters.Add("@HeadCountCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCEInHouseSupport").Value = HeadCountCEInHouseSupport

            myCommand.Parameters.Add("@HeadCountCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountCETotal").Value = HeadCountCETotal

            myCommand.Parameters.Add("@HeadCountPayback", SqlDbType.Decimal)
            myCommand.Parameters("@HeadCountPayback").Value = HeadCountPayback

            myCommand.Parameters.Add("@OverheadCurrentMethod", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCurrentMethod").Value = OverheadCurrentMethod

            myCommand.Parameters.Add("@OverheadCurrentMethodBudget", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCurrentMethodBudget").Value = OverheadCurrentMethodBudget

            myCommand.Parameters.Add("@OverheadProposedMethod", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadProposedMethod").Value = OverheadProposedMethod

            myCommand.Parameters.Add("@OverheadSavings", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadSavings").Value = OverheadSavings

            myCommand.Parameters.Add("@OverheadSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadSavingsBudget").Value = OverheadSavingsBudget

            myCommand.Parameters.Add("@OverheadCECapital", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCECapital").Value = OverheadCECapital

            myCommand.Parameters.Add("@OverheadCEMaterial", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEMaterial").Value = OverheadCEMaterial

            myCommand.Parameters.Add("@OverheadCEOutsideSupport", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEOutsideSupport").Value = OverheadCEOutsideSupport

            myCommand.Parameters.Add("@OverheadCEMisc", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEMisc").Value = OverheadCEMisc

            myCommand.Parameters.Add("@OverheadCEInHouseSupport", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEInHouseSupport").Value = OverheadCEInHouseSupport

            myCommand.Parameters.Add("@OverheadCEWriteOff", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCEWriteOff").Value = OverheadCEWriteOff

            myCommand.Parameters.Add("@OverheadCETotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCETotal").Value = OverheadCETotal

            myCommand.Parameters.Add("@OverheadPayback", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadPayback").Value = OverheadPayback

            myCommand.Parameters.Add("@TotalSavings", SqlDbType.Decimal)
            myCommand.Parameters("@TotalSavings").Value = TotalSavings

            myCommand.Parameters.Add("@TotalSavingsBudget", SqlDbType.Decimal)
            myCommand.Parameters("@TotalSavingsBudget").Value = TotalSavingsBudget

            myCommand.Parameters.Add("@TotalCE", SqlDbType.Decimal)
            myCommand.Parameters("@TotalCE").Value = TotalCE

            myCommand.Parameters.Add("@TotalPayback", SqlDbType.Decimal)
            myCommand.Parameters("@TotalPayback").Value = TotalPayback

            myCommand.Parameters.Add("@CustomerGiveBackDollar", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerGiveBackDollar").Value = CustomerGiveBackDollar

            myCommand.Parameters.Add("@CustomerGiveBackPercent", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerGiveBackPercent").Value = CustomerGiveBackPercent

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo _
            & ", CurrentMethod: " & CurrentMethod _
            & ", ProposedMethod: " & ProposedMethod _
            & ", Benefits: " & Benefits _
            & ", CustomerPartNo: " & CustomerPartNo _
            & ", MaterialPriceCurrentPrice: " & MaterialPriceCurrentPrice _
            & ", MaterialPriceCurrentPriceBudget: " & MaterialPriceCurrentPriceBudget _
            & ", MaterialPriceCurrentFreight: " & MaterialPriceCurrentFreight _
            & ", MaterialPriceCurrentFreightBudget: " & MaterialPriceCurrentFreightBudget _
            & ", MaterialPriceCurrentVolume: " & MaterialPriceCurrentVolume _
            & ", MaterialPriceCurrentVolumeBudget: " & MaterialPriceCurrentVolumeBudget _
            & ", MaterialPriceCurrentPriceByVolume: " & MaterialPriceCurrentPriceByVolume _
            & ", MaterialPriceCurrentPriceByVolumeBudget: " & MaterialPriceCurrentPriceByVolumeBudget _
            & ", MaterialPriceCurrentFreightByVolume: " & MaterialPriceCurrentFreightByVolume _
            & ", MaterialPriceCurrentFreightByVolumeBudget: " & MaterialPriceCurrentFreightByVolumeBudget _
            & ", MaterialPriceCurrentMaterialLanded: " & MaterialPriceCurrentMaterialLanded _
            & ", MaterialPriceCurrentMaterialLandedBudget: " & MaterialPriceCurrentMaterialLandedBudget _
            & ", MaterialPriceCurrentMaterialLandedTotal: " & MaterialPriceCurrentMaterialLandedTotal _
            & ", MaterialPriceCurrentMaterialLandedTotalBudget: " & MaterialPriceCurrentMaterialLandedTotalBudget _
            & ", MaterialPriceProposedPrice: " & MaterialPriceProposedPrice _
            & ", MaterialPriceProposedFreight: " & MaterialPriceProposedFreight _
            & ", MaterialPriceProposedVolume: " & MaterialPriceProposedVolume _
            & ", MaterialPriceProposedPriceByVolume: " & MaterialPriceProposedPriceByVolume _
            & ", MaterialPriceProposedFreightByVolume: " & MaterialPriceProposedFreightByVolume _
            & ", MaterialPriceProposedMaterialLanded: " & MaterialPriceProposedMaterialLanded _
            & ", MaterialPriceProposedMaterialLandedTotal: " & MaterialPriceProposedMaterialLandedTotal _
            & ", MaterialPriceCurrentMethod: " & MaterialPriceCurrentMethod _
            & ", MaterialPriceCurrentMethodBudget: " & MaterialPriceCurrentMethodBudget _
            & ", MaterialPriceProposedMethod: " & MaterialPriceProposedMethod _
            & ", MaterialPriceSavings: " & MaterialPriceSavings _
            & ", MaterialPriceSavingsBudget: " & MaterialPriceSavingsBudget _
            & ", MaterialPriceCECapital: " & MaterialPriceCECapital _
            & ", MaterialPriceCEMaterial: " & MaterialPriceCEMaterial _
            & ", MaterialPriceCEOutsideSupport: " & MaterialPriceCEOutsideSupport _
            & ", MaterialPriceCEMisc: " & MaterialPriceCEMisc _
            & ", MaterialPriceCEInHouseSupport: " & MaterialPriceCEInHouseSupport _
            & ", MaterialPriceCETotal: " & MaterialPriceCETotal _
            & ", MaterialPricePayback: " & MaterialPricePayback _
            & ", MaterialUsageCurrentCostPerUnit: " & MaterialUsageCurrentCostPerUnit _
            & ", MaterialUsageCurrentCostPerUnitBudget: " & MaterialUsageCurrentCostPerUnitBudget _
            & ", MaterialUsageCurrentUnitPerParent: " & MaterialUsageCurrentUnitPerParent _
            & ", MaterialUsageCurrentUnitPerParentBudget: " & MaterialUsageCurrentUnitPerParentBudget _
            & ", MaterialUsageCurrentCostTotal: " & MaterialUsageCurrentCostTotal _
            & ", MaterialUsageCurrentCostTotalBudget: " & MaterialUsageCurrentCostTotalBudget _
            & ", MaterialUsageProposedCostPerUnit: " & MaterialUsageProposedCostPerUnit _
            & ", MaterialUsageProposedUnitPerParent: " & MaterialUsageProposedUnitPerParent _
            & ", MaterialUsageProposedCostTotal: " & MaterialUsageProposedCostTotal _
            & ", MaterialUsageProgramVolume: " & MaterialUsageProgramVolume _
            & ", MaterialUsageProgramVolumeBudget: " & MaterialUsageProgramVolumeBudget _
            & ", MaterialUsageCurrentMethod: " & MaterialUsageCurrentMethod _
            & ", MaterialUsageCurrentMethodBudget: " & MaterialUsageCurrentMethodBudget _
            & ", MaterialUsageProposedMethod: " & MaterialUsageProposedMethod _
            & ", MaterialUsageSavings: " & MaterialUsageSavings _
            & ", MaterialUsageSavingsBudget: " & MaterialUsageSavingsBudget _
            & ", MaterialUsageCECapital: " & MaterialUsageCECapital _
            & ", MaterialUsageCEMaterial: " & MaterialUsageCEMaterial _
            & ", MaterialUsageCEOutsideSupport: " & MaterialUsageCEOutsideSupport _
            & ", MaterialUsageCEMisc: " & MaterialUsageCEMisc _
            & ", MaterialUsageCEInHouseSupport: " & MaterialUsageCEInHouseSupport _
            & ", MaterialUsageCETotal: " & MaterialUsageCETotal _
            & ", MaterialUsagePayback: " & MaterialUsagePayback _
            & ", CycleTimeCurrentPiecesPerHour: " & CycleTimeCurrentPiecesPerHour _
            & ", CycleTimeCurrentPiecesPerHourBudget: " & CycleTimeCurrentPiecesPerHourBudget _
            & ", CycleTimeCurrentCrewSize: " & CycleTimeCurrentCrewSize _
            & ", CycleTimeCurrentCrewSizeBudget: " & CycleTimeCurrentCrewSizeBudget _
            & ", CycleTimeCurrentVolume: " & CycleTimeCurrentVolume _
            & ", CycleTimeCurrentVolumeBudget: " & CycleTimeCurrentVolumeBudget _
            & ", CycleTimeCurrentMachineHourPerPieces: " & CycleTimeCurrentMachineHourPerPieces _
            & ", CycleTimeCurrentMachineHourPerPiecesBudget: " & CycleTimeCurrentMachineHourPerPiecesBudget _
            & ", CycleTimeCurrentManHourPerPieces: " & CycleTimeCurrentManHourPerPieces _
            & ", CycleTimeCurrentManHourPerPiecesBudget: " & CycleTimeCurrentManHourPerPiecesBudget _
            & ", CycleTimeCurrentTotalManHours: " & CycleTimeCurrentTotalManHours _
            & ", CycleTimeCurrentTotalManHoursBudget: " & CycleTimeCurrentTotalManHoursBudget _
            & ", CycleTimeProposedPiecesPerHour: " & CycleTimeProposedPiecesPerHour _
            & ", CycleTimeProposedCrewSize: " & CycleTimeProposedCrewSize _
            & ", CycleTimeProposedVolume: " & CycleTimeProposedVolume _
            & ", CycleTimeProposedMachineHourPerPieces: " & CycleTimeProposedMachineHourPerPieces _
            & ", CycleTimeProposedManHourPerPieces: " & CycleTimeProposedManHourPerPieces _
            & ", CycleTimeProposedTotalManHours: " & CycleTimeProposedTotalManHours _
            & ", CycleTimeFUTARate: " & CycleTimeFUTARate _
            & ", CycleTimeSUTARate: " & CycleTimeSUTARate _
            & ", CycleTimeFICARate: " & CycleTimeFICARate _
            & ", CycleTimeVariableFringes: " & CycleTimeVariableFringes _
            & ", CycleTimeWages: " & CycleTimeWages _
            & ", CycleTimeWagesPlusFringes: " & CycleTimeWagesPlusFringes _
            & ", CycleTimeCurrentMethod: " & CycleTimeCurrentMethod _
            & ", CycleTimeCurrentMethodBudget: " & CycleTimeCurrentMethodBudget _
            & ", CycleTimeProposedMethod: " & CycleTimeProposedMethod _
            & ", CycleTimeMethodDifference: " & CycleTimeMethodDifference _
            & ", CycleTimeMethodDifferenceBudget: " & CycleTimeMethodDifferenceBudget _
            & ", CycleTimeSavings: " & CycleTimeSavings _
            & ", CycleTimeSavingsBudget: " & CycleTimeSavingsBudget _
            & ", CycleTimeCECapital: " & CycleTimeCECapital _
            & ", CycleTimeCEMaterial: " & CycleTimeCEMaterial _
            & ", CycleTimeCEOutsideSupport: " & CycleTimeCEOutsideSupport _
            & ", CycleTimeCEMisc: " & CycleTimeCEMisc _
            & ", CycleTimeCEInHouseSupport: " & CycleTimeCEInHouseSupport _
            & ", CycleTimeCETotal: " & CycleTimeCETotal _
            & ", CycleTimePayback: " & CycleTimePayback _
            & ", HeadCountWages: " & HeadCountWages _
            & ", HeadCountWagesBudget: " & HeadCountWagesBudget _
            & ", HeadCountAnnualLaborCost: " & HeadCountAnnualLaborCost _
            & ", HeadCountAnnualLaborCostBudget: " & HeadCountAnnualLaborCostBudget _
            & ", HeadCountCurrentLaborCount: " & HeadCountCurrentLaborCount _
            & ", HeadCountCurrentLaborCountBudget: " & HeadCountCurrentLaborCountBudget _
            & ", HeadCountCurrentLaborCost: " & HeadCountCurrentLaborCost _
            & ", HeadCountCurrentLaborCostBudget: " & HeadCountCurrentLaborCostBudget _
            & ", HeadCountCurrentLaborFringes: " & HeadCountCurrentLaborFringes _
            & ", HeadCountCurrentLaborTotal: " & HeadCountCurrentLaborTotal _
            & ", HeadCountCurrentLaborTotalBudget: " & HeadCountCurrentLaborTotalBudget _
            & ", HeadCountProposedLaborCount: " & HeadCountProposedLaborCount _
            & ", HeadCountProposedLaborCost: " & HeadCountProposedLaborCost _
            & ", HeadCountProposedLaborFringes: " & HeadCountProposedLaborFringes _
            & ", HeadCountProposedLaborTotal: " & HeadCountProposedLaborTotal _
            & ", HeadCountCurrentMethod: " & HeadCountCurrentMethod _
            & ", HeadCountCurrentMethodBudget: " & HeadCountCurrentMethodBudget _
            & ", HeadCountProposedMethod: " & HeadCountProposedMethod _
            & ", HeadCountSavings: " & HeadCountSavings _
            & ", HeadCountSavingsBudget: " & HeadCountSavingsBudget _
            & ", HeadCountFUTA: " & HeadCountFUTA _
            & ", HeadCountSUTA: " & HeadCountSUTA _
            & ", HeadCountFICA: " & HeadCountFICA _
            & ", HeadCountPension: " & HeadCountPension _
            & ", HeadCountBonus: " & HeadCountBonus _
            & ", HeadCountLife: " & HeadCountLife _
            & ", HeadCountGroupInsurance: " & HeadCountGroupInsurance _
            & ", HeadCountWorkersComp: " & HeadCountWorkersComp _
            & ", HeadCountPensionQuarterly: " & HeadCountPensionQuarterly _
            & ", HeadCountTotalFringes: " & HeadCountTotalFringes _
            & ", HeadCountCECapital: " & HeadCountCECapital _
            & ", HeadCountCEMaterial: " & HeadCountCEMaterial _
            & ", HeadCountCEOutsideSupport: " & HeadCountCEOutsideSupport _
            & ", HeadCountCEMisc: " & HeadCountCEMisc _
            & ", HeadCountCEInHouseSupport: " & HeadCountCEInHouseSupport _
            & ", HeadCountCETotal: " & HeadCountCETotal _
            & ", HeadCountPayback: " & HeadCountPayback _
            & ", OverheadCurrentMethod: " & OverheadCurrentMethod _
            & ", OverheadCurrentMethodBudget: " & OverheadCurrentMethodBudget _
            & ", OverheadProposedMethod: " & OverheadProposedMethod _
            & ", OverheadSavings: " & OverheadSavings _
            & ", OverheadSavingsBudget: " & OverheadSavingsBudget _
            & ", OverheadCECapital: " & OverheadCECapital _
            & ", OverheadCEMaterial: " & OverheadCEMaterial _
            & ", OverheadCEOutsideSupport: " & OverheadCEOutsideSupport _
            & ", OverheadCEMisc: " & OverheadCEMisc _
            & ", OverheadCEInHouseSupport: " & OverheadCEInHouseSupport _
            & ", OverheadCEWriteOff: " & OverheadCEWriteOff _
            & ", OverheadCETotal: " & OverheadCETotal _
            & ", OverheadPayback: " & OverheadPayback _
            & ", TotalSavings: " & TotalSavings _
            & ", TotalSavingsBudget: " & TotalSavingsBudget _
            & ", TotalCE: " & TotalCE _
            & ", TotalPayback: " & TotalPayback _
            & ", CustomerGiveBackDollar: " & CustomerGiveBackDollar _
            & ", CustomerGiveBackPercent: " & CustomerGiveBackPercent _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateCostReductionDetail

    Public Shared Function GetCostReductionProjectLeaders() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_Project_Leaders"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostReductionProjectLeaders")

            GetCostReductionProjectLeaders = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionProjectLeaders : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionProjectLeaders : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionProjectLeaders = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetProjectCategory

End Class
