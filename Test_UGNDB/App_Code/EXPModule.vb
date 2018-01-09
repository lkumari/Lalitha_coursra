''************************************************************************************************
'' Name:	EXPModule.vb
'' Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Expensed Projects Module
''
'' Date		    Author	    
'' 06/03/2009   LRey			Created .Net application
'' 10/06/2011   LRey            Added new parameters to the GetExpProjTolling function and DeleteToolingExpProjCookies
'' 10/06/2011   LRey            Add Functions for the CapEx Developement
'' 07/26/2012   RCarlson        Added RFDNo to InsertFuturePartNo
'' 07/30/2012   LRey            Replace LeadTime with LeadTimeVal and LeadTimeWM according to 
''                              ehancement document on 06/11/2012
'' 07/31/2012   LRey            Added Memo at Program Awarded fields to the follwing functions
''                              - InsertExpProjTooling
''                              - UpdateExpProjTooling
''                              - InsertExpProjToolingExpenditure
''                              - UpdateExpProjToolingExpenditure
'' 02/01/2014	LRey            Replaced DeptOrCostCenter with new ERP values. Replaced SoldTo|CABBV with Customer.
'' 04/04/2014   LRey            Added Subscription to InsertExpProjAssetsApproval
'' 04/09/2014   LRey            Added UGNFacility to InsertExpProjDevelopmentApproval
'' 04/14/2014   LRey            Added UGNFacility to InsertExpProjRepairApproval
'' 04/15/2014   LRey            Added UGNFacility to InsertExpProjPackagingApproval
'' 04/21/2014   LRey            Added UGNFacility to InsertExpProjToolingApproval
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

Public Class EXPModule
#Region "Common Functions used in global CapEx Only"
    Public Shared Function GetExpProjCategory(ByVal CategoryName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Category"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CategoryName", SqlDbType.VarChar)
            myCommand.Parameters("@CategoryName").Value = CategoryName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpProjCategory")

            GetExpProjCategory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjCategory") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjCategory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjCategory

    Public Shared Function GetExpProjCapitalSpending(ByVal CapitalSpendingName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Capital_Spending"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CapitalSpendingName", SqlDbType.VarChar)
            myCommand.Parameters("@CapitalSpendingName").Value = CapitalSpendingName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpProjCapitalSpending")

            GetExpProjCapitalSpending = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjCapitalSpending") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjCapitalSpending = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjCapitalSpending

    Public Shared Function GetNextExpProjectNo(ByVal ParentProjectNo As String, ByVal UGNFacility As String, ByVal ExpProject As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ExpProject", SqlDbType.VarChar)
            myCommand.Parameters("@ExpProject").Value = ExpProject


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NextProjNo")

            GetNextExpProjectNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetNextExpProjectNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetNextExpProjectNo") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetNextExpProjectNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetNextExpProjectNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetNextExpProjectNo

    Public Shared Function GetUGNDatabaseNextProjNo(ByVal ParentProjectNo As String, ByVal ProjectNo As String, ByVal ExpProject As String, ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGNDatabase_NextProjectNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ExpProject", SqlDbType.VarChar)
            myCommand.Parameters("@ExpProject").Value = ExpProject

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UGNDatabaseNextProjNo")

            GetUGNDatabaseNextProjNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetUGNDatabaseNextProjNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetUGNDatabaseNextProjNo") = "~/EXP/ToolingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNDatabaseNextProjNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetUGNDatabaseNextProjNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetUGNDatabaseNextProjNo

    Public Shared Function InsertFuturePartNo(ByVal PartNo As String, ByVal PartDesc As String, ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal DesignationType As String, ByVal RFDNo As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Future_PartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = commonFunctions.replaceSpecialChar(PartDesc, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertFuturePartNo")

            InsertFuturePartNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", PartDesc: " & PartDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFuturePartNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertFuturePartNo") = "~/Exp/ToolingExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFuturePartNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertFuturePartNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertFuturePartNo

    Public Shared Sub CleanExpCrystalReports()

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
            HttpContext.Current.Session("BLLerror") = "CleanExpCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanExpCrystalReports : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanExpCrystalReports

    Public Shared Sub DeleteSpendingRequestReportCookies()

        Try
            HttpContext.Current.Response.Cookies("SR_SRType").Value = ""
            HttpContext.Current.Response.Cookies("SR_SRType").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SR_UGNFac").Value = ""
            HttpContext.Current.Response.Cookies("SR_UGNFac").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SR_ProjStat").Value = ""
            HttpContext.Current.Response.Cookies("SR_ProjStat").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SR_FromDate").Value = ""
            HttpContext.Current.Response.Cookies("SR_FromDate").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SR_ToDate").Value = ""
            HttpContext.Current.Response.Cookies("SR_ToDate").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteRepairExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRepairExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteRepairExpProjCookies

#End Region 'EOF Common Functions used in CapEx Only

#Region "Assets Expensed Projects"
    Public Shared Function GetExpProjAssets(ByVal ProjectNo As String, ByVal SupProjectNo As String, ByVal ProjectTitle As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal DeptOrCostCenter As String, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal ProjectStatus As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@SupProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupProjectNo").Value = SupProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = ProjectTitle

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssetsExpProj")

            GetExpProjAssets = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjAssets") = "~/EXP/AssetsExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjAssets = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjAssets

    Public Shared Function GetExpProjAssetsLead(ByVal ProjectNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssetsExpProjLead")

            GetExpProjAssetsLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjAssetsLead") = "~/EXP/AssetsExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjAssetsLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjAssetsLead

    Public Shared Function GetExpProjAssetsCustomer(ByVal ProjectNo As String, ByVal TCID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TCID", SqlDbType.Int)
            myCommand.Parameters("@TCID").Value = TCID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolCustomer")

            GetExpProjAssetsCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", TCID: " & TCID
            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjAssetsCustomer") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjAssetsCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjAssetsCustomer

    Public Shared Function GetExpProjAssetsExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolExpenditure")

            GetExpProjAssetsExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjAssetsExpenditure") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjAssetsExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetExpProjAssetsExpenditure

    Public Shared Function GetAssetsExpProjApproval(ByVal ProjectNo As String, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetAssetsExpProjApproval")

            GetAssetsExpProjApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssetsExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAssetsExpProjApproval") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssetsExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAssetsExpProjApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAssetsExpProjApproval

    Public Shared Function GetAssetsExpProjHistory(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssetsExpProjHistory")

            GetAssetsExpProjHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssetsExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAssetsExpProjHistory") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssetsExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAssetsExpProjHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAssetsExpProjHistory

    Public Shared Function GetAssetsExpProjRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssetsExpProjRSS")

            GetAssetsExpProjRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssetsExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAssetsExpProjRSS") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssetsExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAssetsExpProjRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAssetsExpProjRSS

    Public Shared Function GetAssetsExpProjRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssetsExpProjRSSReply")

            GetAssetsExpProjRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssetsExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetAssetsExpProjRSSReply") = "~/EXP/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssetsExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAssetsExpProjRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetAssetsExpProjRSSReply

    Public Shared Function GetAssetsExpDocument(ByVal ProjectNo As String, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetAssetsExpDocument")

            GetAssetsExpDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAssetsExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetAssetsExpDocument") = "~/EXP/AssetsExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("GetAssetsExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAssetsExpDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAssetsExpDocument

    Public Shared Function GetCostReductionList(ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Reduction_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostReductionList")

            GetCostReductionList = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostReductionList : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetCostReductionList") = "~/EXP/AssetsExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionList : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostReductionList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCostReductionList

    Public Shared Sub InsertExpProjAssets(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal Analysis As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal OriginalApprovedDt As String, ByVal DeptOrCostCenter As String, ByVal ProjectInLatestForecast As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@Analysis", SqlDbType.VarChar)
            myCommand.Parameters("@Analysis").Value = commonFunctions.replaceSpecialChar(Analysis, False)

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@OriginalApprovedDt", SqlDbType.VarChar)
            myCommand.Parameters("@OriginalApprovedDt").Value = OriginalApprovedDt

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssets") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssets

    Public Shared Sub InsertExpProjAssetsApproval(ByVal ProjectNo As String, ByVal UGNFacility As String, ByVal Subscription As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Subscription", SqlDbType.Int)
            myCommand.Parameters("@Subscription").Value = Subscription

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsApproval") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsApproval

    Public Shared Sub InsertExpProjAssetsExpenditure(ByVal ProjectNo As String, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal CreatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsExpenditure") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsExpenditure

    Public Shared Sub InsertExpProjAssetsHistory(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = commonFunctions.replaceSpecialChar(FieldChange, False)

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = commonFunctions.replaceSpecialChar(PreviousValue, False)

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = commonFunctions.replaceSpecialChar(NewValue, False)

            myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeReason").Value = commonFunctions.replaceSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsHistory") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsHistory

    Public Shared Sub InsertExpProjAssetsRSS(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsRSS") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsRSS

    Public Shared Sub InsertExpProjAssetsRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsRSSReply") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsRSSReply

    Public Shared Sub InsertExpProjAssetsDocuments(ByVal ProjectNo As String, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal EID As Integer, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal ExpenseDescr As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Assets_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@ExpenseDescr", SqlDbType.VarChar)
            myCommand.Parameters("@ExpenseDescr").Value = commonFunctions.replaceSpecialChar(ExpenseDescr, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjAssetsDocuments") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjAssetsDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjAssetsDocuments

    Public Shared Sub UpdateExpProjAssets(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal Analysis As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal RoutingStatus As String, ByVal ActualCost As Decimal, ByVal CustomerCost As Decimal, ByVal ClosingNotes As String, ByVal VoidReason As String, ByVal DeptOrCostCenter As String, ByVal RtdEqpValue As Decimal, ByVal WorkingCapital As Decimal, ByVal StartupExpense As Decimal, ByVal CustReimb As Decimal, ByVal NotRequired As Boolean, ByVal PaybackInYears As Decimal, ByVal ReturnAvgAssets As Decimal, ByVal ProjectInLatestForecast As Boolean, ByVal SalvageValue As Decimal, ByVal NetBookValue As Decimal, ByVal NetTaxValue As Decimal, ByVal RepairSavings As Decimal, ByVal ScrapSavings As Decimal, ByVal ConsumableSavings As Decimal, ByVal LaborSavings As Decimal, ByVal OtherSavings As Decimal, ByVal CRPRojectNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Assets"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@Analysis", SqlDbType.VarChar)
            myCommand.Parameters("@Analysis").Value = commonFunctions.replaceSpecialChar(Analysis, False)

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ActualCost", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCost").Value = ActualCost

            myCommand.Parameters.Add("@CustomerCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerCost").Value = CustomerCost

            myCommand.Parameters.Add("@ClosingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ClosingNotes").Value = commonFunctions.replaceSpecialChar(ClosingNotes, False)

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@RtdEqpValue", SqlDbType.Decimal)
            myCommand.Parameters("@RtdEqpValue").Value = RtdEqpValue

            myCommand.Parameters.Add("@WorkingCapital", SqlDbType.Decimal)
            myCommand.Parameters("@WorkingCapital").Value = WorkingCapital

            myCommand.Parameters.Add("@StartUpExpense", SqlDbType.Decimal)
            myCommand.Parameters("@StartUpExpense").Value = StartupExpense

            myCommand.Parameters.Add("@CustReimb", SqlDbType.Decimal)
            myCommand.Parameters("@CustReimb").Value = CustReimb

            myCommand.Parameters.Add("@NotRequired", SqlDbType.Bit)
            myCommand.Parameters("@NotRequired").Value = NotRequired

            myCommand.Parameters.Add("@PaybackInYears", SqlDbType.Decimal)
            myCommand.Parameters("@PaybackInYears").Value = PaybackInYears

            myCommand.Parameters.Add("@ReturnAvgAssets", SqlDbType.Decimal)
            myCommand.Parameters("@ReturnAvgAssets").Value = ReturnAvgAssets

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@SalvageValue", SqlDbType.Decimal)
            myCommand.Parameters("@SalvageValue").Value = SalvageValue

            myCommand.Parameters.Add("@NetBookValue", SqlDbType.Decimal)
            myCommand.Parameters("@NetBookValue").Value = NetBookValue

            myCommand.Parameters.Add("@NetTaxValue", SqlDbType.Decimal)
            myCommand.Parameters("@NetTaxValue").Value = NetTaxValue

            myCommand.Parameters.Add("@RepairSavings", SqlDbType.Decimal)
            myCommand.Parameters("@RepairSavings").Value = RepairSavings

            myCommand.Parameters.Add("@ScrapSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapSavings").Value = ScrapSavings

            myCommand.Parameters.Add("@ConsumableSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ConsumableSavings").Value = ConsumableSavings

            myCommand.Parameters.Add("@LaborSavings", SqlDbType.Decimal)
            myCommand.Parameters("@LaborSavings").Value = LaborSavings

            myCommand.Parameters.Add("@OtherSavings", SqlDbType.Decimal)
            myCommand.Parameters("@OtherSavings").Value = OtherSavings

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRPRojectNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjAssets") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjAssets

    Public Shared Sub UpdateExpProjAssetsStatus(ByVal ProjectNo As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Assets_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssetsStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjAssetsStatus") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssetsStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateExpProjAssetsStatus

    Public Shared Sub UpdateExpProjAssetsApproval(ByVal ProjectNo As String, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Assets_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@SameTMID", SqlDbType.Bit)
            myCommand.Parameters("@SameTMID").Value = SameTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjAssetsApproval") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjAssetsApproval

    Public Shared Sub UpdateExpProjAssetsExpenditure(ByVal EID As Integer, ByVal ProjectNo As String, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Assets_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjAssetsExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjAssetsExpenditure") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjAssetsExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjAssetsExpenditure

    Public Shared Sub DeleteExpProjAssets(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DeleteSupplement As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Assets"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DeleteSupplement", SqlDbType.Bit)
            myCommand.Parameters("@DeleteSupplement").Value = DeleteSupplement

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjAssets") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjAssets : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjAssets

    Public Shared Sub DeleteExpProjAssetsApproval(ByVal ProjectNo As String, ByVal Sequence As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Assets_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjAssetsApproval") = "~/Exp/AssetsExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjAssetsApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjAssetsApproval    

    Public Shared Sub DeleteAssetsExpProjCookies()

        Try
            HttpContext.Current.Response.Cookies("EXPA_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_SupProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_SupProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_ProjTitle").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_ProjTitle").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_PLDRID").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_PLDRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_DEPT").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_DEPT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_CatID").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_CatID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_CapCls").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_CapCls").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPA_PStatus").Value = ""
            HttpContext.Current.Response.Cookies("EXPA_PStatus").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAssetsExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/AssetsExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAssetsExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteAssetsExpProjCookies

#End Region 'EOF Assets Expensed Project

#Region "Development Expensed Projects"
    Public Shared Function GetExpProjDevelopment(ByVal ProjectNo As String, ByVal SupProjectNo As String, ByVal ProjectTitle As String, ByVal RequestedByTMID As Integer, ByVal ProjectLeaderTMID As Integer, ByVal AcctMgrTMID As Integer, ByVal UGNFacility As String, ByVal DeptOrCostCenter As Integer, ByVal ProgramID As Integer, ByVal Customer As String, ByVal CommodityId As Integer, ByVal RoutingStatus As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@SupProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupProjectNo").Value = SupProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = ProjectTitle

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjectLeaderTMID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.Int)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityId

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DevelopmentExpProj")

            GetExpProjDevelopment = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjDevelopment") = "~/EXP/DevelopmentExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjDevelopment = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjDevelopment

    Public Shared Function GetExpProjDevelopmentLead(ByVal ProjectNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DevelopmentExpProjLead")

            GetExpProjDevelopmentLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjDevelopmentLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjDevelopmentLead") = "~/EXP/DevelopmentExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjDevelopmentLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjDevelopmentLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjDevelopmentLead

    Public Shared Function GetDevelopmentExpProjApproval(ByVal ProjectNo As String, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDevelopmentExpProjApproval")

            GetDevelopmentExpProjApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDevelopmentExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetDevelopmentExpProjApproval") = "~/EXP/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDevelopmentExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDevelopmentExpProjApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDevelopmentExpProjApproval

    Public Shared Function GetDevelopmentExpProjHistory(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DevelopmentExpProjHistory")

            GetDevelopmentExpProjHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDevelopmentExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetDevelopmentExpProjHistory") = "~/EXP/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDevelopmentExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDevelopmentExpProjHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDevelopmentExpProjHistory

    Public Shared Function GetDevelopmentExpProjRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DevelopmentExpProjRSS")

            GetDevelopmentExpProjRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDevelopmentExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetDevelopmentExpProjRSS") = "~/EXP/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDevelopmentExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDevelopmentExpProjRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDevelopmentExpProjRSS

    Public Shared Function GetDevelopmentExpProjRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DevelopmentExpProjRSSReply")

            GetDevelopmentExpProjRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDevelopmentExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetDevelopmentExpProjRSSReply") = "~/EXP/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDevelopmentExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDevelopmentExpProjRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetDevelopmentExpProjRSSReply

    Public Shared Function GetDevelopmentExpDocument(ByVal ProjectNo As String, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Development_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDevelopmentExpDocument")

            GetDevelopmentExpDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDevelopmentExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetDevelopmentExpDocument") = "~/EXP/DevelopmentExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("GetDevelopmentExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDevelopmentExpDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDevelopmentExpDocument

    Public Shared Sub InsertExpProjDevelopment(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DateSubmitted As String, ByVal RequestedByTMID As Integer, ByVal ProjLdrTMID As Integer, ByVal AcctMgrTMID As Integer, ByVal ProjectTitle As String, ByVal Year As String, ByVal ProgramID As Integer, ByVal VehicleSOP As String, ByVal CommodityID As Integer, ByVal Budgeted As Boolean, ByVal UGNFacility As String, ByVal DeptOrCostCenter As String, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal OriginalApprovedDt As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal PreDvp As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@Year", SqlDbType.VarChar)
            myCommand.Parameters("@Year").Value = Year

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@VehicleSOP", SqlDbType.VarChar)
            myCommand.Parameters("@VehicleSOP").Value = VehicleSOP

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@Budgeted", SqlDbType.Bit)
            myCommand.Parameters("@Budgeted").Value = Budgeted

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@OriginalApprovedDt", SqlDbType.VarChar)
            myCommand.Parameters("@OriginalApprovedDt").Value = OriginalApprovedDt

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@PreDvp", SqlDbType.Bit)
            myCommand.Parameters("@PreDvp").Value = PreDvp

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopment") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopment

    Public Shared Sub InsertExpProjDevelopmentApproval(ByVal ProjectNo As String, ByVal UGNFacility As String, ByVal SubscriptionID As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopmentApproval") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopmentApproval

    Public Shared Sub InsertExpProjDevelopmentHistory(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = commonFunctions.replaceSpecialChar(FieldChange, False)

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = commonFunctions.replaceSpecialChar(PreviousValue, False)

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = commonFunctions.replaceSpecialChar(NewValue, False)

            myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeReason").Value = commonFunctions.replaceSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopmentHistory") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopmentHistory

    Public Shared Sub InsertExpProjDevelopmentRSS(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopmentRSS") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopmentRSS

    Public Shared Sub InsertExpProjDevelopmentRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopmentRSSReply") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopmentRSSReply

    Public Shared Sub InsertExpProjDevelopmentDocuments(ByVal ProjectNo As String, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal EID As Integer, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal ExpenseDescr As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Development_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@ExpenseDescr", SqlDbType.VarChar)
            myCommand.Parameters("@ExpenseDescr").Value = commonFunctions.replaceSpecialChar(ExpenseDescr, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjDevelopmentDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjDevelopmentDocuments") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjDevelopmentDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjDevelopmentDocuments

    Public Shared Sub UpdateExpProjDevelopment(ByVal ProjectNo As String, ByVal OrigProjectNo As String, ByVal ParentProjectNo As String, ByVal DateSubmitted As String, ByVal RequestedByTMID As Integer, ByVal ProjLdrTMID As Integer, ByVal AcctMgrTMID As Integer, ByVal ProjectTitle As String, ByVal VehicleSOP As String, ByVal Budgeted As Boolean, ByVal UGNFacility As String, ByVal DeptOrCostCenter As String, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal CRPRojectNo As Integer, ByVal CRPRojectNoRequested As Boolean, ByVal GeneralNotes As String, ByVal Materials As Decimal, ByVal LaborOH As Decimal, ByVal Packaging As Decimal, ByVal Freight As Decimal, ByVal TravelExpenditures As Decimal, ByVal NITUGN As Decimal, ByVal FarmingtonAcousticTestingCharges As Decimal, ByVal OtherTesting As Decimal, ByVal TotalRequest As Decimal, ByVal CustReimb As Decimal, ByVal TotalInv As Decimal, ByVal VoidReason As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal DevSavings As Decimal, ByVal ScrapSavings As Decimal, ByVal ConsumSavings As Decimal, ByVal LaborSavings As Decimal, ByVal OtherSavings As Decimal, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Development"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@OrigProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@OrigProjectNo").Value = OrigProjectNo

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@VehicleSOP", SqlDbType.VarChar)
            myCommand.Parameters("@VehicleSOP").Value = VehicleSOP

            myCommand.Parameters.Add("@Budgeted", SqlDbType.Bit)
            myCommand.Parameters("@Budgeted").Value = Budgeted

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRPRojectNo

            myCommand.Parameters.Add("@CRProjectNoRequested", SqlDbType.Bit)
            myCommand.Parameters("@CRProjectNoRequested").Value = CRPRojectNoRequested

            myCommand.Parameters.Add("@GeneralNotes", SqlDbType.VarChar)
            myCommand.Parameters("@GeneralNotes").Value = commonFunctions.replaceSpecialChar(GeneralNotes, False)

            myCommand.Parameters.Add("@Materials", SqlDbType.Decimal)
            myCommand.Parameters("@Materials").Value = Materials

            myCommand.Parameters.Add("@LaborOH", SqlDbType.Decimal)
            myCommand.Parameters("@LaborOH").Value = LaborOH

            myCommand.Parameters.Add("@Packaging", SqlDbType.Decimal)
            myCommand.Parameters("@Packaging").Value = Packaging

            myCommand.Parameters.Add("@Freight", SqlDbType.Decimal)
            myCommand.Parameters("@Freight").Value = Freight

            myCommand.Parameters.Add("@TravelExpenditures", SqlDbType.Decimal)
            myCommand.Parameters("@TravelExpenditures").Value = TravelExpenditures

            myCommand.Parameters.Add("@NITUGN", SqlDbType.Decimal)
            myCommand.Parameters("@NITUGN").Value = NITUGN

            myCommand.Parameters.Add("@FarmingtonAcousticTestingCharges", SqlDbType.Decimal)
            myCommand.Parameters("@FarmingtonAcousticTestingCharges").Value = FarmingtonAcousticTestingCharges

            myCommand.Parameters.Add("@OtherTesting", SqlDbType.Decimal)
            myCommand.Parameters("@OtherTesting").Value = OtherTesting

            myCommand.Parameters.Add("@TotalRequest", SqlDbType.Decimal)
            myCommand.Parameters("@TotalRequest").Value = TotalRequest

            myCommand.Parameters.Add("@CustReimb", SqlDbType.Decimal)
            myCommand.Parameters("@CustReimb").Value = CustReimb

            myCommand.Parameters.Add("@TotalInv", SqlDbType.Decimal)
            myCommand.Parameters("@TotalInv").Value = TotalInv

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@DevSavings", SqlDbType.Decimal)
            myCommand.Parameters("@DevSavings").Value = DevSavings

            myCommand.Parameters.Add("@ScrapSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapSavings").Value = ScrapSavings

            myCommand.Parameters.Add("@ConsumSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ConsumSavings").Value = ConsumSavings

            myCommand.Parameters.Add("@LaborSavings", SqlDbType.Decimal)
            myCommand.Parameters("@LaborSavings").Value = LaborSavings

            myCommand.Parameters.Add("@OtherSavings", SqlDbType.Decimal)
            myCommand.Parameters("@OtherSavings").Value = OtherSavings

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjDevelopment") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjDevelopment

    Public Shared Sub UpdateExpProjDevelopmentStatus(ByVal ProjectNo As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Development_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjDevelopmentStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjDevelopmentStatus") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjDevelopmentStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateExpProjDevelopmentStatus

    Public Shared Sub UpdateExpProjDevelopmentApproval(ByVal ProjectNo As String, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Development_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@SameTMID", SqlDbType.Bit)
            myCommand.Parameters("@SameTMID").Value = SameTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjDevelopmentApproval") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjDevelopmentApproval

    Public Shared Sub DeleteExpProjDevelopment(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DeleteSupplement As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Development"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DeleteSupplement", SqlDbType.Bit)
            myCommand.Parameters("@DeleteSupplement").Value = DeleteSupplement

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjDevelopment") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjDevelopment : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjDevelopment

    Public Shared Sub DeleteExpProjDevelopmentApproval(ByVal ProjectNo As String, ByVal Sequence As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Development_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjDevelopmentApproval") = "~/Exp/DevelopmentExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjDevelopmentApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjDevelopmentApproval    

    Public Shared Sub DeleteDevelopmentExpProjCookies()

        Try
            HttpContext.Current.Response.Cookies("EXPD_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_SupProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_SupProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_ProjTitle").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_ProjTitle").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_RBID").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_RBID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_PLDRID").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_PLDRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_AMID").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_AMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_DEPT").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_DEPT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_Program").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_CABBV").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_SoldTo").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_SoldTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_COMID").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_COMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPD_RSTAT").Value = ""
            HttpContext.Current.Response.Cookies("EXPD_RSTAT").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDevelopmentExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/DevelopmentExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteDevelopmentExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteDevelopmentExpProjCookies

#End Region 'EOF Development Expensed Project

#Region "Repair Expensed Projects"
    Public Shared Function GetExpProjRepair(ByVal ProjectNo As String, ByVal SupProjectNo As String, ByVal ProjectTitle As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal DeptOrCostCenter As String, ByVal ProjectStatus As String) As DataSet
        ' '', ByVal CategoryID As Integer, ByVal CSCode As String

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@SupProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupProjectNo").Value = SupProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = ProjectTitle

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RepairExpProj")

            GetExpProjRepair = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjRepair") = "~/EXP/RepairExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjRepair = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjRepair

    Public Shared Function GetExpProjRepairLead(ByVal ProjectNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RepairExpProjLead")

            GetExpProjRepairLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjRepairLead") = "~/EXP/RepairExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjRepairLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjRepairLead

    Public Shared Function GetExpProjRepairExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolExpenditure")

            GetExpProjRepairExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjRepairExpenditure") = "~/EXP/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjRepairExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetExpProjRepairExpenditure

    Public Shared Function GetRepairExpProjApproval(ByVal ProjectNo As String, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetRepairExpProjApproval")

            GetRepairExpProjApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRepairExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetRepairExpProjApproval") = "~/EXP/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRepairExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRepairExpProjApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetRepairExpProjApproval

    Public Shared Function GetRepairExpProjHistory(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RepairExpProjHistory")

            GetRepairExpProjHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRepairExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetRepairExpProjHistory") = "~/EXP/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRepairExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRepairExpProjHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetRepairExpProjHistory

    Public Shared Function GetRepairExpProjRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RepairExpProjRSS")

            GetRepairExpProjRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRepairExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetRepairExpProjRSS") = "~/EXP/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRepairExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRepairExpProjRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetRepairExpProjRSS

    Public Shared Function GetRepairExpProjRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RepairExpProjRSSReply")

            GetRepairExpProjRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRepairExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetRepairExpProjRSSReply") = "~/EXP/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRepairExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRepairExpProjRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetRepairExpProjRSSReply

    Public Shared Function GetRepairExpDocument(ByVal ProjectNo As String, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Repair_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetRepairExpDocument")

            GetRepairExpDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRepairExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetRepairExpDocument") = "~/EXP/RepairExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("GetRepairExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRepairExpDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetRepairExpDocument

    Public Shared Sub InsertExpProjRepair(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal RoutingStatus As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal OriginalApprovedDt As String, ByVal DeptOrCostCenter As String, ByVal ProjectInLatestForecast As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@OriginalApprovedDt", SqlDbType.VarChar)
            myCommand.Parameters("@OriginalApprovedDt").Value = OriginalApprovedDt

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepair") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepair

    Public Shared Sub InsertExpProjRepairApproval(ByVal ProjectNo As String, ByVal UGNFacility As String, ByVal Subscription As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Subscription", SqlDbType.VarChar)
            myCommand.Parameters("@Subscription").Value = Subscription

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairApproval") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairApproval

    Public Shared Sub InsertExpProjRepairExpenditure(ByVal ProjectNo As String, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal CreatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairExpenditure") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairExpenditure

    Public Shared Sub InsertExpProjRepairHistory(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = commonFunctions.replaceSpecialChar(FieldChange, False)

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = commonFunctions.replaceSpecialChar(PreviousValue, False)

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = commonFunctions.replaceSpecialChar(NewValue, False)

            myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeReason").Value = commonFunctions.replaceSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairHistory") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairHistory

    Public Shared Sub InsertExpProjRepairRSS(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairRSS") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairRSS

    Public Shared Sub InsertExpProjRepairRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairRSSReply") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairRSSReply

    Public Shared Sub InsertExpProjRepairDocuments(ByVal ProjectNo As String, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal EID As Integer, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal ExpenseDescr As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Repair_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@ExpenseDescr", SqlDbType.VarChar)
            myCommand.Parameters("@ExpenseDescr").Value = commonFunctions.replaceSpecialChar(ExpenseDescr, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjRepairDocuments") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjRepairDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjRepairDocuments

    Public Shared Sub UpdateExpProjRepair(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal ProjDtNotes As String, ByVal Justification As String, ByVal Analysis As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal RoutingStatus As String, ByVal ActualCost As Decimal, ByVal CustomerCost As Decimal, ByVal ClosingNotes As String, ByVal VoidReason As String, ByVal DeptOrCostCenter As String, ByVal RtdEqpValue As Decimal, ByVal WorkingCapital As Decimal, ByVal StartupExpense As Decimal, ByVal CustReimb As Decimal, ByVal NotRequired As Boolean, ByVal ProjectInLatestForecast As Boolean, ByVal RepairSavings As Decimal, ByVal ScrapSavings As Decimal, ByVal ConsumableSavings As Decimal, ByVal LaborSavings As Decimal, ByVal OtherSavings As Decimal, ByVal CRPRojectNo As Integer, ByVal CRPRojectNoRequested As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Repair"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@Justification", SqlDbType.VarChar)
            myCommand.Parameters("@Justification").Value = commonFunctions.replaceSpecialChar(Justification, False)

            myCommand.Parameters.Add("@Analysis", SqlDbType.VarChar)
            myCommand.Parameters("@Analysis").Value = commonFunctions.replaceSpecialChar(Analysis, False)

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ActualCost", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCost").Value = ActualCost

            myCommand.Parameters.Add("@CustomerCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerCost").Value = CustomerCost

            myCommand.Parameters.Add("@ClosingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ClosingNotes").Value = commonFunctions.replaceSpecialChar(ClosingNotes, False)

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.VarChar)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@RtdEqpValue", SqlDbType.Decimal)
            myCommand.Parameters("@RtdEqpValue").Value = RtdEqpValue

            myCommand.Parameters.Add("@WorkingCapital", SqlDbType.Decimal)
            myCommand.Parameters("@WorkingCapital").Value = WorkingCapital

            myCommand.Parameters.Add("@StartUpExpense", SqlDbType.Decimal)
            myCommand.Parameters("@StartUpExpense").Value = StartupExpense

            myCommand.Parameters.Add("@CustReimb", SqlDbType.Decimal)
            myCommand.Parameters("@CustReimb").Value = CustReimb

            myCommand.Parameters.Add("@NotRequired", SqlDbType.Bit)
            myCommand.Parameters("@NotRequired").Value = NotRequired

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@RepairSavings", SqlDbType.Decimal)
            myCommand.Parameters("@RepairSavings").Value = RepairSavings

            myCommand.Parameters.Add("@ScrapSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapSavings").Value = ScrapSavings

            myCommand.Parameters.Add("@ConsumableSavings", SqlDbType.Decimal)
            myCommand.Parameters("@ConsumableSavings").Value = ConsumableSavings

            myCommand.Parameters.Add("@LaborSavings", SqlDbType.Decimal)
            myCommand.Parameters("@LaborSavings").Value = LaborSavings

            myCommand.Parameters.Add("@OtherSavings", SqlDbType.Decimal)
            myCommand.Parameters("@OtherSavings").Value = OtherSavings

            myCommand.Parameters.Add("@CRProjectNo", SqlDbType.Int)
            myCommand.Parameters("@CRProjectNo").Value = CRPRojectNo

            myCommand.Parameters.Add("@CRProjectNoRequested", SqlDbType.Bit)
            myCommand.Parameters("@CRProjectNoRequested").Value = CRPRojectNoRequested

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjRepair") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjRepair

    Public Shared Sub UpdateExpProjRepairStatus(ByVal ProjectNo As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Repair_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepairStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjRepairStatus") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepairStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateExpProjRepairStatus

    Public Shared Sub UpdateExpProjRepairApproval(ByVal ProjectNo As String, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Repair_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@SameTMID", SqlDbType.Bit)
            myCommand.Parameters("@SameTMID").Value = SameTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjRepairApproval") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjRepairApproval

    Public Shared Sub UpdateExpProjRepairExpenditure(ByVal EID As Integer, ByVal ProjectNo As String, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Repair_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjRepairExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjRepairExpenditure") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjRepairExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjRepairExpenditure

    Public Shared Sub DeleteExpProjRepair(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DeleteSupplement As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Repair"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DeleteSupplement", SqlDbType.Bit)
            myCommand.Parameters("@DeleteSupplement").Value = DeleteSupplement

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjRepair") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjRepair : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjRepair

    Public Shared Sub DeleteExpProjRepairApproval(ByVal ProjectNo As String, ByVal Sequence As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Repair_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjRepairApproval") = "~/Exp/RepairExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjRepairApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjRepairApproval    

    Public Shared Sub DeleteRepairExpProjCookies()

        Try
            HttpContext.Current.Response.Cookies("EXPR_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_SupProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_SupProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_ProjTitle").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_ProjTitle").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_PLDRID").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_PLDRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_DEPT").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_DEPT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPR_PSTAT").Value = ""
            HttpContext.Current.Response.Cookies("EXPR_PSTAT").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteRepairExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/RepairExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRepairExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteRepairExpProjCookies

#End Region 'EOF Repair Expensed Project

#Region "Tooling Expensed Project"
    Public Shared Function GetExpProjTooling(ByVal ProjectNo As String, ByVal SupProjectNo As String, ByVal ProjectTitle As String, ByVal UGNFacility As String, ByVal Customer As String, ByVal ProgramID As Integer, ByVal AcctMgrTMID As Integer, ByVal PrgmMgrTMID As Integer, ByVal ToolLeadTMID As Integer, ByVal PurchLeadTMID As Integer, ByVal ProjectType As String, ByVal PartNo As String, ByVal PartDesc As String, ByVal ProjectStatus As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@SupProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupProjectNo").Value = SupProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = ProjectTitle

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@PrgmMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@PrgmMgrTMID").Value = PrgmMgrTMID

            myCommand.Parameters.Add("@ToolLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@ToolLeadTMID").Value = ToolLeadTMID

            myCommand.Parameters.Add("@PurchLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@PurchLeadTMID").Value = PurchLeadTMID

            myCommand.Parameters.Add("@ProjectType", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectType").Value = ProjectType

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = PartDesc

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ToolingExpProj")

            GetExpProjTooling = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjTooling") = "~/EXP/ToolingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjTooling = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjTooling

    Public Shared Function GetExpProjToolingLead(ByVal ProjectNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ToolingExpProjLead")

            GetExpProjToolingLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjToolingLead") = "~/EXP/ToolingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjToolingLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjToolingLead

    Public Shared Function GetExpProjToolingCustomer(ByVal ProjectNo As String, ByVal TCID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TCID", SqlDbType.Int)
            myCommand.Parameters("@TCID").Value = TCID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolCustomer")

            GetExpProjToolingCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", TCID: " & TCID
            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjToolingCustomer") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjToolingCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjToolingCustomer

    Public Shared Function GetExpProjToolingExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolExpenditure")

            GetExpProjToolingExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjToolingExpenditure") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjToolingExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetExpProjToolingExpenditure

    Public Shared Function GetToolingExpProjApproval(ByVal ProjectNo As String, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetToolingExpProjApproval")

            GetToolingExpProjApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetToolingExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetToolingExpProjApproval") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetToolingExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetToolingExpProjApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetToolingExpProjApproval

    Public Shared Function GetToolingExpProjHistory(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ToolingExpProjHistory")

            GetToolingExpProjHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetToolingExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetToolingExpProjHistory") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetToolingExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetToolingExpProjHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetToolingExpProjHistory

    Public Shared Function GetToolingExpProjRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ToolingExpProjRSS")

            GetToolingExpProjRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetToolingExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetToolingExpProjRSS") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetToolingExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetToolingExpProjRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetToolingExpProjRSS

    Public Shared Function GetToolingExpProjRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ToolingExpProjRSSReply")

            GetToolingExpProjRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetToolingExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetToolingExpProjRSSReply") = "~/EXP/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetToolingExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetToolingExpProjRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetToolingExpProjRSSReply

    Public Shared Function GetToolingExpDocument(ByVal ProjectNo As String, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetToolingExpDocument")

            GetToolingExpDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetToolingExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetToolingExpDocument") = "~/EXP/ToolingExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("GetToolingExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetToolingExpDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetToolingExpDocument

    Public Shared Function GetExpProjToolingLastSupplementNo(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Tooling_LastSupplementNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpProjToolingLastSupplementNo")

            GetExpProjToolingLastSupplementNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjToolingLastSupplementNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjToolingLastSupplementNo") = "~/EXP/ToolingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjToolingLastSupplementNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjToolingLastSupplementNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjToolingLastSupplementNo

    Public Shared Function InsertExpProjTooling(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal ProjectType As String, ByVal UGNFacility As String, ByVal AcctMgrTMID As Integer, ByVal PrgmMgrTMID As Integer, ByVal ToolLeadTMID As Integer, ByVal PurchLeadTMID As Integer, ByVal ProjDtNotes As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal ExpToolRtnDt As String, ByVal EstSpendDt As String, ByVal EstRecoveryDt As String, ByVal AmtToRecover As Decimal, ByVal OriginalToolApprovedDt As String, ByVal MPAAmtToBeRecovered As Decimal, ByVal CreatedBy As String, ByVal CreatedOn As String, ByVal OldSysCarryOver As Boolean) As DataSet 'ByVal EstRecoveryDt As String, 

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@ProjectType", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectType").Value = ProjectType

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@PrgmMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@PrgmMgrTMID").Value = PrgmMgrTMID

            myCommand.Parameters.Add("@ToolLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@ToolLeadTMID").Value = ToolLeadTMID

            myCommand.Parameters.Add("@PurchLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@PurchLeadTMID").Value = PurchLeadTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@ExpToolRtnDt", SqlDbType.VarChar)
            myCommand.Parameters("@ExpToolRtnDt").Value = ExpToolRtnDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstRecoveryDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstRecoveryDt").Value = EstRecoveryDt

            myCommand.Parameters.Add("@AmtToRecover", SqlDbType.Decimal)
            myCommand.Parameters("@AmtToRecover").Value = AmtToRecover

            myCommand.Parameters.Add("@OriginalToolApprovedDt", SqlDbType.VarChar)
            myCommand.Parameters("@OriginalToolApprovedDt").Value = OriginalToolApprovedDt

            myCommand.Parameters.Add("@MPA_AmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@MPA_AmtToBeRecovered").Value = MPAAmtToBeRecovered

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myCommand.Parameters.Add("@OldSysCarryOver", SqlDbType.Bit)
            myCommand.Parameters("@OldSysCarryOver").Value = OldSysCarryOver

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewExpProjTooling")
            InsertExpProjTooling = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjTooling") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjTooling = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF InsertExpProjTooling

    Public Shared Function InsertExpProjToolingApproval(ByVal ProjectNo As String, ByVal UGNFacility As String, ByVal Subscription As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Subscription", SqlDbType.Int)
            myCommand.Parameters("@Subscription").Value = Subscription

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BuildExpProjTooling")
            InsertExpProjToolingApproval = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingApproval") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingApproval

    Public Shared Function InsertExpProjToolingCustomer(ByVal ProjectNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal PartNo As String, ByVal ProgramID As Integer, ByVal OEM As String, ByVal RevisionLevel As String, ByVal LeadTimeVal As Decimal, ByVal LeadTimeWM As String, ByVal LeadTimeComments As String, ByVal SOP As String, ByVal EOP As String, ByVal PPAP As String, ByVal PartDesc As String, ByVal DesignationType As String, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            myCommand.Parameters("@RevisionLevel").Value = commonFunctions.replaceSpecialChar(RevisionLevel, False)

            myCommand.Parameters.Add("@LeadTimeVal", SqlDbType.Decimal)
            myCommand.Parameters("@LeadTimeVal").Value = LeadTimeVal

            myCommand.Parameters.Add("@LeadTimeWM", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTimeWM").Value = LeadTimeWM

            myCommand.Parameters.Add("@LeadTimeComments", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTimeComments").Value = commonFunctions.replaceSpecialChar(LeadTimeComments, False)

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@PPAP", SqlDbType.VarChar)
            myCommand.Parameters("@PPAP").Value = PPAP

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = commonFunctions.replaceSpecialChar(PartDesc, False)

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewExpProjToolingCustomer")
            InsertExpProjToolingCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingCustomer") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingCustomer

    Public Shared Function InsertExpProjToolingExpenditure(ByVal ProjectNo As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal MPAAmount As Decimal, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@MPAAmount", SqlDbType.Decimal)
            myCommand.Parameters("@MPAAmount").Value = MPAAmount

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewExpProjToolingExpenditure")
            InsertExpProjToolingExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingExpenditure") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingExpenditure

    Public Shared Function InsertExpProjToolingHistory(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertExpProjToolingHistory")
            InsertExpProjToolingHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingHistory") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingHistory

    Public Shared Function InsertExpProjToolingRSS(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertExpProjToolingHistory")

            InsertExpProjToolingRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingRSS") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingRSS

    Public Shared Function InsertExpProjToolingRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertExpProjToolingRSSReply")

            InsertExpProjToolingRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingRSSReply") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingRSSReply

    Public Shared Function InsertExpProjToolingDocuments(ByVal ProjectNo As String, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Tooling_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertExpProjToolingDocuments")

            InsertExpProjToolingDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjToolingDocuments") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjToolingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertExpProjToolingDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertExpProjToolingDocuments

    Public Shared Function UpdateExpProjTooling(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal ProjectType As String, ByVal UGNFacility As String, ByVal AcctMgrTMID As Integer, ByVal PrgmMgrTMID As Integer, ByVal ToolLeadTMID As Integer, ByVal PurchLeadTMID As Integer, ByVal ProjDtNotes As String, ByVal DateSubmitted As String, ByVal EstCmpltDt As String, ByVal ExpToolRtnDt As String, ByVal EstSpendDt As String, ByVal EstRecoveryDt As String, ByVal AmtToRecover As Decimal, ByVal RoutingStatus As String, ByVal ActualCost As Decimal, ByVal CustomerCost As Decimal, ByVal ClosingNotes As String, ByVal VoidReason As String, ByVal LumpSum As Boolean, ByVal FirstRecoveryAmount As Decimal, ByVal FirstRecoverydate As String, ByVal SecondRecoveryAmount As Decimal, ByVal SecondRecoveryDate As String, ByVal PiecePrice As Boolean, ByVal MPAAmtToBeRecovered As Decimal, ByVal UpdatedBy As String, ByVal UpdatedOn As String, ByRef ToolEngrMgrTMID As Integer, ByVal NotifyToolEngrMgr As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@ProjectType", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectType").Value = ProjectType

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@PrgmMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@PrgmMgrTMID").Value = PrgmMgrTMID

            myCommand.Parameters.Add("@ToolLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@ToolLeadTMID").Value = ToolLeadTMID

            myCommand.Parameters.Add("@PurchLeadTMID", SqlDbType.Int)
            myCommand.Parameters("@PurchLeadTMID").Value = PurchLeadTMID

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@ExpToolRtnDt", SqlDbType.VarChar)
            myCommand.Parameters("@ExpToolRtnDt").Value = ExpToolRtnDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstRecoveryDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstRecoveryDt").Value = EstRecoveryDt

            myCommand.Parameters.Add("@AmtToRecover", SqlDbType.Decimal)
            myCommand.Parameters("@AmtToRecover").Value = AmtToRecover

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ActualCost", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCost").Value = ActualCost

            myCommand.Parameters.Add("@CustomerCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerCost").Value = CustomerCost

            myCommand.Parameters.Add("@ClosingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ClosingNotes").Value = commonFunctions.replaceSpecialChar(ClosingNotes, False)

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@LumpSum", SqlDbType.Bit)
            myCommand.Parameters("@LumpSum").Value = LumpSum

            myCommand.Parameters.Add("@FirstRecoveryAmount", SqlDbType.Decimal)
            myCommand.Parameters("@FirstRecoveryAmount").Value = FirstRecoveryAmount

            myCommand.Parameters.Add("@FirstRecoveryDate", SqlDbType.VarChar)
            myCommand.Parameters("@FirstRecoveryDate").Value = FirstRecoverydate

            myCommand.Parameters.Add("@SecondRecoveryAmount", SqlDbType.Decimal)
            myCommand.Parameters("@SecondRecoveryAmount").Value = SecondRecoveryAmount

            myCommand.Parameters.Add("@SecondRecoverydate", SqlDbType.VarChar)
            myCommand.Parameters("@SecondRecoverydate").Value = SecondRecoveryDate

            myCommand.Parameters.Add("@PiecePrice", SqlDbType.Bit)
            myCommand.Parameters("@PiecePrice").Value = PiecePrice

            myCommand.Parameters.Add("@MPA_AmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@MPA_AmtToBeRecovered").Value = MPAAmtToBeRecovered

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myCommand.Parameters.Add("@NotifyToolEngrMgr", SqlDbType.Bit)
            myCommand.Parameters("@NotifyToolEngrMgr").Value = NotifyToolEngrMgr

            myCommand.Parameters.Add("@ToolEngrMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@ToolEngrMgrTMID").Value = ToolEngrMgrTMID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateExpProjTooling")
            UpdateExpProjTooling = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjTooling") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateExpProjTooling = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF UpdateExpProjTooling

    Public Shared Function UpdateExpProjToolingStatus(ByVal ProjectNo As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Tooling_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateExpProjToolingStatus")
            UpdateExpProjToolingStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjToolingStatus") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateExpProjToolingStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF UpdateExpProjToolingStatus

    Public Shared Function UpdateExpProjToolingApproval(ByVal ProjectNo As String, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Tooling_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            'myCommand.Parameters.Add("@OrigTMID", SqlDbType.Int)
            'myCommand.Parameters("@OrigTMID").Value = OrigTMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status
            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateExpProjTooling")
            UpdateExpProjToolingApproval = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjToolingApproval") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateExpProjToolingApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateExpProjToolingApproval

    Public Shared Function UpdateExpProjToolingCustomer(ByVal TCID As Integer, ByVal ProjectNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer, ByVal PartNo As String, ByVal OEM As String, ByVal RevisionLevel As String, ByVal LeadTimeVal As Decimal, ByVal LeadTimeWM As String, ByVal LeadTimeComments As String, ByVal SOP As String, ByVal EOP As String, ByVal PPAP As String, ByVal PartDesc As String, ByVal DesignationType As String, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Tooling_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TCID", SqlDbType.Int)
            myCommand.Parameters("@TCID").Value = TCID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            myCommand.Parameters("@RevisionLevel").Value = commonFunctions.replaceSpecialChar(RevisionLevel, False)

            myCommand.Parameters.Add("@LeadTimeVal", SqlDbType.Decimal)
            myCommand.Parameters("@LeadTimeVal").Value = LeadTimeVal

            myCommand.Parameters.Add("@LeadTimeWM", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTimeWM").Value = LeadTimeWM

            myCommand.Parameters.Add("@LeadTimeComments", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTimeComments").Value = commonFunctions.replaceSpecialChar(LeadTimeComments, False)

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@PPAP", SqlDbType.VarChar)
            myCommand.Parameters("@PPAP").Value = PPAP

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = commonFunctions.replaceSpecialChar(PartDesc, False)

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateExpProjToolingCustomer")

            UpdateExpProjToolingCustomer = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TCID: " & TCID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjToolingCustomer") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateExpProjToolingCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateExpProjToolingCustomer

    Public Shared Function UpdateExpProjToolingExpenditure(ByVal EID As Integer, ByVal ProjectNo As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal MPAAmount As Decimal, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Tooling_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@MPAAmount", SqlDbType.Decimal)
            myCommand.Parameters("@MPAAmount").Value = MPAAmount

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateExpProjToolingExpenditure")

            UpdateExpProjToolingExpenditure = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjToolingExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjToolingExpenditure") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjToolingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateExpProjToolingExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateExpProjToolingExpenditure

    Public Shared Function DeleteExpProjTooling(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DeleteSupplement As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DeleteSupplement", SqlDbType.Bit)
            myCommand.Parameters("@DeleteSupplement").Value = DeleteSupplement

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteExpProjTooling")
            DeleteExpProjTooling = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjTooling") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjTooling : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteExpProjTooling = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteExpProjTooling

    Public Shared Function DeleteExpProjToolingApproval(ByVal ProjectNo As String, ByVal Sequence As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Tooling_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteExpProjToolingApproval")
            DeleteExpProjToolingApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjToolingApproval") = "~/Exp/ToolingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjToolingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteExpProjToolingApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteExpProjToolingApproval    

    Public Shared Sub DeleteToolingExpProjCookies()

        Try
            HttpContext.Current.Response.Cookies("EXPT_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_SupProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_SupProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_ProjTitle").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_ProjTitle").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_CABBV").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("EXPT_SoldTo").Value = ""
            'HttpContext.Current.Response.Cookies("EXPT_SoldTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_Program").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_AMGRID").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_AMGRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_PMID").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_PMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_TLID").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_TLID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_PLID").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_PLID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_ProjType").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_ProjType").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_PartNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_PartNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_PartDesc").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_PartDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPT_ProjStatus").Value = ""
            HttpContext.Current.Response.Cookies("EXPT_ProjStatus").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteToolingExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ToolingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteToolingExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteToolingExpProjCookies

#End Region 'EOF Tooling Expensed Project

#Region "Packaging Expensed Projects"
    Public Shared Function GetExpProjPackaging(ByVal ProjectNo As String, ByVal SupProjectNo As String, ByVal ProjectTitle As String, ByVal UGNFacility As String, ByVal ProjLdrTMID As Integer, ByVal Customer As String, ByVal ProgramID As Integer, ByVal PartNo As String, ByVal PartDesc As String, ByVal ProjectStatus As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@SupProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupProjectNo").Value = SupProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = ProjectTitle

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjLdrTMID

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = PartDesc

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PackagingExpProj")

            GetExpProjPackaging = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjPackaging") = "~/EXP/PackagingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjPackaging = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjPackaging

    Public Shared Function GetExpProjPackagingLead(ByVal ProjectNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PackagingExpProjLead")

            GetExpProjPackagingLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjPackagingLead") = "~/EXP/PackagingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjPackagingLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjPackagingLead

    Public Shared Function GetExpProjPackagingCustomer(ByVal ProjectNo As String, ByVal PCID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@PCID", SqlDbType.Int)
            myCommand.Parameters("@PCID").Value = PCID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpPackagingCustomer")

            GetExpProjPackagingCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PCID: " & PCID
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjPackagingCustomer") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjPackagingCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjPackagingCustomer

    Public Shared Function GetExpProjPackagingExpenditure(ByVal ProjectNo As String, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpToolExpenditure")

            GetExpProjPackagingExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjPackagingExpenditure") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjPackagingExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetExpProjPackagingExpenditure

    Public Shared Function GetPackagingExpProjApproval(ByVal ProjectNo As String, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPackagingExpProjApproval")

            GetPackagingExpProjApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPackagingExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetPackagingExpProjApproval") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPackagingExpProjApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPackagingExpProjApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPackagingExpProjApproval

    Public Shared Function GetPackagingExpProjHistory(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PackagingExpProjHistory")

            GetPackagingExpProjHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPackagingExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetPackagingExpProjHistory") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPackagingExpProjHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPackagingExpProjHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPackagingExpProjHistory

    Public Shared Function GetPackagingExpProjRSS(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PackagingExpProjRSS")

            GetPackagingExpProjRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPackagingExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetPackagingExpProjRSS") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPackagingExpProjRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPackagingExpProjRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPackagingExpProjRSS

    Public Shared Function GetPackagingExpProjRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PackagingExpProjRSSReply")

            GetPackagingExpProjRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPackagingExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetPackagingExpProjRSSReply") = "~/EXP/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPackagingExpProjRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPackagingExpProjRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetPackagingExpProjRSSReply

    Public Shared Function GetPackagingExpDocument(ByVal ProjectNo As String, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPackagingExpDocument")

            GetPackagingExpDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPackagingExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPackagingExpDocument") = "~/EXP/PackagingExpProj.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPackagingExpDocument : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPackagingExpDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPackagingExpDocument

    Public Shared Function GetExpProjPackagingLastSupplementNo(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Packaging_LastSupplementNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ExpProjPackagingLastSupplementNo")

            GetExpProjPackagingLastSupplementNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ProjectNo: " & ProjectNo
            HttpContext.Current.Session("BLLerror") = "GetExpProjPackagingLastSupplementNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetExpProjPackagingLastSupplementNo") = "~/EXP/PackagingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetExpProjPackagingLastSupplementNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjPackagingLastSupplementNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjPackagingLastSupplementNo

    Public Shared Sub InsertExpProjPackaging(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal ProjectLeaderTMID As Integer, ByVal AcctMgrTMID As Integer, ByVal DateSubmitted As String, ByVal UT As Boolean, ByVal UN As Boolean, ByVal UP As Boolean, ByVal UR As Boolean, ByVal US As Boolean, ByVal UW As Boolean, ByVal OH As Boolean, ByVal ProjDtNotes As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal AmtToBeRecovered As Decimal, ByVal OriginalApprovedDt As String, ByVal OldSysCarryOver As Boolean, ByVal ProjectInLatestForecast As Boolean, ByVal MPAAmtToBeRecovered As Decimal, ByVal EstRecoveryDt As String, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjectLeaderTMID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@UT", SqlDbType.Bit)
            myCommand.Parameters("@UT").Value = UT

            myCommand.Parameters.Add("@UN", SqlDbType.Bit)
            myCommand.Parameters("@UN").Value = UN

            myCommand.Parameters.Add("@UP", SqlDbType.Bit)
            myCommand.Parameters("@UP").Value = UP

            myCommand.Parameters.Add("@UR", SqlDbType.Bit)
            myCommand.Parameters("@UR").Value = UR

            myCommand.Parameters.Add("@US", SqlDbType.Bit)
            myCommand.Parameters("@US").Value = US

            myCommand.Parameters.Add("@UW", SqlDbType.Bit)
            myCommand.Parameters("@UW").Value = UW

            myCommand.Parameters.Add("@OH", SqlDbType.Bit)
            myCommand.Parameters("@OH").Value = OH

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@AmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@AmtToBeRecovered").Value = AmtToBeRecovered

            myCommand.Parameters.Add("@OriginalApprovedDt", SqlDbType.VarChar)
            myCommand.Parameters("@OriginalApprovedDt").Value = OriginalApprovedDt

            myCommand.Parameters.Add("@OldSysCarryOver", SqlDbType.Bit)
            myCommand.Parameters("@OldSysCarryOver").Value = OldSysCarryOver

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@MPAAmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@MPAAmtToBeRecovered").Value = MPAAmtToBeRecovered

            myCommand.Parameters.Add("@EstRecoveryDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstRecoveryDt").Value = EstRecoveryDt

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackaging") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackaging

    Public Shared Sub InsertExpProjPackagingApproval(ByVal ProjectNo As String, ByVal UT As Boolean, ByVal UN As Boolean, ByVal UP As Boolean, ByVal UR As Boolean, ByVal US As Boolean, ByVal UW As Boolean, ByVal OH As Boolean, ByVal Subscription As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@UT", SqlDbType.Bit)
            myCommand.Parameters("@UT").Value = UT

            myCommand.Parameters.Add("@UN", SqlDbType.Bit)
            myCommand.Parameters("@UN").Value = UN

            myCommand.Parameters.Add("@UP", SqlDbType.Bit)
            myCommand.Parameters("@UP").Value = UP

            myCommand.Parameters.Add("@UR", SqlDbType.Bit)
            myCommand.Parameters("@UR").Value = UR

            myCommand.Parameters.Add("@US", SqlDbType.Bit)
            myCommand.Parameters("@US").Value = US

            myCommand.Parameters.Add("@UW", SqlDbType.Bit)
            myCommand.Parameters("@UW").Value = UW

            myCommand.Parameters.Add("@OH", SqlDbType.Bit)
            myCommand.Parameters("@OH").Value = OH

            myCommand.Parameters.Add("@Subscription", SqlDbType.Int)
            myCommand.Parameters("@Subscription").Value = Subscription

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingApproval") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingApproval

    Public Shared Sub InsertExpProjPackagingCustomer(ByVal ProjectNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal PartNo As String, ByVal ProgramID As Integer, ByVal OEM As String, ByVal RevisionLevel As String, ByVal LeadTime As String, ByVal SOP As String, ByVal EOP As String, ByVal PPAP As String, ByVal PartDesc As String, ByVal DesignationType As String, ByVal CreatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            myCommand.Parameters("@RevisionLevel").Value = commonFunctions.replaceSpecialChar(RevisionLevel, False)

            myCommand.Parameters.Add("@LeadTime", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTime").Value = commonFunctions.replaceSpecialChar(LeadTime, False)

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@PPAP", SqlDbType.VarChar)
            myCommand.Parameters("@PPAP").Value = PPAP

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = PartDesc

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingCustomer") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingCustomer

    Public Shared Sub InsertExpProjPackagingExpenditure(ByVal ProjectNo As String, ByVal FutureVendor As Boolean, ByVal VendorType As String, ByVal VendorNo As Integer, ByVal Description As String, ByVal UGNFacility As String, ByVal DeptOrCostCenter As Integer, ByVal Quantity As Integer, ByVal UGNUnitCost As Decimal, ByVal CustUnitCost As Decimal, ByVal Notes As String, ByVal MPATotalCost As Decimal, ByVal CreatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Bit)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.Int)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@UGNUnitCost", SqlDbType.Decimal)
            myCommand.Parameters("@UGNUnitCost").Value = UGNUnitCost

            myCommand.Parameters.Add("@CustUnitCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustUnitCost").Value = CustUnitCost

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@MPATotalCost", SqlDbType.Decimal)
            myCommand.Parameters("@MPATotalCost").Value = MPATotalCost

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingExpenditure") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingExpenditure

    Public Shared Sub InsertExpProjPackagingHistory(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = commonFunctions.replaceSpecialChar(FieldChange, False)

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = commonFunctions.replaceSpecialChar(PreviousValue, False)

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = commonFunctions.replaceSpecialChar(NewValue, False)

            myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeReason").Value = commonFunctions.replaceSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingHistory") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingHistory

    Public Shared Sub InsertExpProjPackagingRSS(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingRSS") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingRSS

    Public Shared Sub InsertExpProjPackagingRSSReply(ByVal ProjectNo As String, ByVal RSSID As Integer, ByVal ProjectTitle As String, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ProjectTitle: " & commonFunctions.replaceSpecialChar(ProjectTitle, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingRSSReply") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingRSSReply

    Public Shared Sub InsertExpProjPackagingDocuments(ByVal ProjectNo As String, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal EID As Integer, ByVal CategoryID As Integer, ByVal CSCode As String, ByVal ExpenseDescr As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ExpProj_Packaging_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@CategoryID", SqlDbType.Int)
            myCommand.Parameters("@CategoryID").Value = CategoryID

            myCommand.Parameters.Add("@CSCode", SqlDbType.VarChar)
            myCommand.Parameters("@CSCode").Value = CSCode

            myCommand.Parameters.Add("@ExpenseDescr", SqlDbType.VarChar)
            myCommand.Parameters("@ExpenseDescr").Value = commonFunctions.replaceSpecialChar(ExpenseDescr, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertExpProjPackagingDocuments") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertExpProjPackagingDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertExpProjPackagingDocuments

    Public Shared Sub UpdateExpProjPackaging(ByVal ProjectNo As String, ByVal ProjectTitle As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal ProjectLeaderTMID As Integer, ByVal AcctMgrTMID As Integer, ByVal DateSubmitted As String, ByVal UT As Boolean, ByVal UN As Boolean, ByVal UP As Boolean, ByVal UR As Boolean, ByVal US As Boolean, ByVal UW As Boolean, ByVal OH As Boolean, ByVal ProjDtNotes As String, ByVal EstCmpltDt As String, ByVal EstSpendDt As String, ByVal EstEndSpendDt As String, ByVal AmtToBeRecovered As Decimal, ByVal ActualCost As Decimal, ByVal CustomerCost As Decimal, ByVal ClosingNotes As String, ByVal VoidReason As String, ByVal NotRequired As Boolean, ByVal DiscountReturned As Decimal, ByVal PaybackInYears As Decimal, ByVal ReturnAvgAssets As Decimal, ByVal ProjectInLatestForecast As Boolean, ByVal MPAAmtToBeRecovered As Decimal, ByVal EstRecoveryDt As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectTitle", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectTitle").Value = commonFunctions.replaceSpecialChar(ProjectTitle, False)

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ProjectLeaderTMID", SqlDbType.Int)
            myCommand.Parameters("@ProjectLeaderTMID").Value = ProjectLeaderTMID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@UT", SqlDbType.Bit)
            myCommand.Parameters("@UT").Value = UT

            myCommand.Parameters.Add("@UN", SqlDbType.Bit)
            myCommand.Parameters("@UN").Value = UN

            myCommand.Parameters.Add("@UP", SqlDbType.Bit)
            myCommand.Parameters("@UP").Value = UP

            myCommand.Parameters.Add("@UR", SqlDbType.Bit)
            myCommand.Parameters("@UR").Value = UR

            myCommand.Parameters.Add("@US", SqlDbType.Bit)
            myCommand.Parameters("@US").Value = US

            myCommand.Parameters.Add("@UW", SqlDbType.Bit)
            myCommand.Parameters("@UW").Value = UW

            myCommand.Parameters.Add("@OH", SqlDbType.Bit)
            myCommand.Parameters("@OH").Value = OH

            myCommand.Parameters.Add("@ProjDtNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProjDtNotes").Value = commonFunctions.replaceSpecialChar(ProjDtNotes, False)

            myCommand.Parameters.Add("@EstCmpltDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstCmpltDt").Value = EstCmpltDt

            myCommand.Parameters.Add("@EstSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstSpendDt").Value = EstSpendDt

            myCommand.Parameters.Add("@EstEndSpendDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstEndSpendDt").Value = EstEndSpendDt

            myCommand.Parameters.Add("@AmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@AmtToBeRecovered").Value = AmtToBeRecovered

            myCommand.Parameters.Add("@ActualCost", SqlDbType.Decimal)
            myCommand.Parameters("@ActualCost").Value = ActualCost

            myCommand.Parameters.Add("@CustomerCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustomerCost").Value = CustomerCost

            myCommand.Parameters.Add("@ClosingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ClosingNotes").Value = commonFunctions.replaceSpecialChar(ClosingNotes, False)

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@NotRequired", SqlDbType.Bit)
            myCommand.Parameters("@NotRequired").Value = NotRequired

            myCommand.Parameters.Add("@DiscountReturned", SqlDbType.Decimal)
            myCommand.Parameters("@DiscountReturned").Value = DiscountReturned

            myCommand.Parameters.Add("@PaybackInYears", SqlDbType.Decimal)
            myCommand.Parameters("@PaybackInYears").Value = PaybackInYears

            myCommand.Parameters.Add("@ReturnAvgAssets", SqlDbType.Decimal)
            myCommand.Parameters("@ReturnAvgAssets").Value = ReturnAvgAssets

            myCommand.Parameters.Add("@ProjectInLatestForecast", SqlDbType.Bit)
            myCommand.Parameters("@ProjectInLatestForecast").Value = ProjectInLatestForecast

            myCommand.Parameters.Add("@MPAAmtToBeRecovered", SqlDbType.Decimal)
            myCommand.Parameters("@MPAAmtToBeRecovered").Value = MPAAmtToBeRecovered

            myCommand.Parameters.Add("@EstRecoveryDt", SqlDbType.VarChar)
            myCommand.Parameters("@EstRecoveryDt").Value = EstRecoveryDt

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjPackaging") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjPackaging

    Public Shared Sub UpdateExpProjPackagingStatus(ByVal ProjectNo As String, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Packaging_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackagingStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjPackagingStatus") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackagingStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateExpProjPackagingStatus

    Public Shared Sub UpdateExpProjPackagingApproval(ByVal ProjectNo As String, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Packaging_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjPackagingApproval") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjPackagingApproval

    Public Shared Sub UpdateExpProjPackagingCustomer(ByVal PCID As Integer, ByVal ProjectNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal PartNo As String, ByVal ProgramID As Integer, ByVal OEM As String, ByVal RevisionLevel As String, ByVal LeadTime As String, ByVal SOP As String, ByVal EOP As String, ByVal PPAP As String, ByVal PartDesc As String, ByVal DesignationType As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Packaging_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PCID", SqlDbType.Int)
            myCommand.Parameters("@PCID").Value = PCID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            myCommand.Parameters("@RevisionLevel").Value = RevisionLevel

            myCommand.Parameters.Add("@LeadTime", SqlDbType.VarChar)
            myCommand.Parameters("@LeadTime").Value = commonFunctions.replaceSpecialChar(LeadTime, False)

            myCommand.Parameters.Add("@SOP", SqlDbType.VarChar)
            myCommand.Parameters("@SOP").Value = SOP

            myCommand.Parameters.Add("@EOP", SqlDbType.VarChar)
            myCommand.Parameters("@EOP").Value = EOP

            myCommand.Parameters.Add("@PPAP", SqlDbType.VarChar)
            myCommand.Parameters("@PPAP").Value = PPAP

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = PartDesc

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", PCID: " & PCID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjPackagingCustomer") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackagingCustomer : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjPackagingCustomer

    Public Shared Sub UpdateExpProjPackagingExpenditure(ByVal EID As Integer, ByVal ProjectNo As String, ByVal Description As String, ByVal VendorType As String, ByVal VendorNo As Integer, ByVal FutureVendor As Boolean, ByVal UGNFacility As String, ByVal DeptOrCostCenter As Integer, ByVal Quantity As Integer, ByVal UGNUnitCost As Decimal, ByVal CustUnitCost As Decimal, ByVal Notes As String, ByVal MPATotalCost As Decimal, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ExpProj_Packaging_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Bit)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptOrCostCenter", SqlDbType.Int)
            myCommand.Parameters("@DeptOrCostCenter").Value = DeptOrCostCenter

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@UGNUnitCost", SqlDbType.Decimal)
            myCommand.Parameters("@UGNUnitCost").Value = UGNUnitCost

            myCommand.Parameters.Add("@CustUnitCost", SqlDbType.Decimal)
            myCommand.Parameters("@CustUnitCost").Value = CustUnitCost

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@MPATotalCost", SqlDbType.Decimal)
            myCommand.Parameters("@MPATotalCost").Value = MPATotalCost

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateExpProjPackagingExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateExpProjPackagingExpenditure") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateExpProjPackagingExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateExpProjPackagingExpenditure

    Public Shared Sub DeleteExpProjPackaging(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal DeleteSupplement As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@ParentProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentProjectNo").Value = ParentProjectNo

            myCommand.Parameters.Add("@DeleteSupplement", SqlDbType.Bit)
            myCommand.Parameters("@DeleteSupplement").Value = DeleteSupplement

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjPackaging") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjPackaging : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjPackaging

    Public Shared Sub DeleteExpProjPackagingApproval(ByVal ProjectNo As String, ByVal Sequence As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ExpProj_Packaging_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteExpProjPackagingApproval") = "~/Exp/PackagingExpProj.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteExpProjPackagingApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteExpProjPackagingApproval    

    Public Shared Sub DeletePackagingExpProjCookies()

        Try
            HttpContext.Current.Response.Cookies("EXPP_ProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_ProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_SupProjNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_SupProjNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_ProjTitle").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_ProjTitle").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_UGNFacility").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_UGNFacility").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_PLDRID").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_PLDRID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_CABBV").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_CABBV").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("EXPP_SoldTo").Value = ""
            'HttpContext.Current.Response.Cookies("EXPP_SoldTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_Program").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_Program").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_PartNo").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_PartNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_PartDesc").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_PartDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("EXPP_PStatus").Value = ""
            HttpContext.Current.Response.Cookies("EXPP_PStatus").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeletePackagingExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/PackagingExpProjList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePackagingExpProjCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeletePackagingExpProjCookies

#End Region 'EOF Packaging Expensed Project

#Region "Global Functions"
    Public Shared Function GetExpProjDocuments(ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetToolingExpDocument")

            GetExpProjDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetExpProjDocuments") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetExpProjDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetExpProjDocuments

#End Region 'EOF Global Functions

End Class