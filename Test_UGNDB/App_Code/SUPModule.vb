''************************************************************************************************
''Name:		SUPModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Purchasing Module
''
''Date		    Author	    
''09/07/2010    LRey        Created .Net application
''05/12/2011    LRey        Added GetSupplierLookUp function which includes both Supplier_Request
''                          and AVM vendor listing for Supplier look up.
''06/29/2012    LRey        Created a GetSupplierRequestSearch function used only for search screen
''                          Modified the GetSupplierRequest function so that it uses less pass through param's 
''11/26/2012    LRey        Modified Insert/UpdateSupplierRequest by adding new columns for RemitToCountry and ShipFromCountry
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

Public Class SUPModule
    Public Shared Function DeleteSupplierRequest(ByVal SUPNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Supplier_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteSupplierRequest")
            DeleteSupplierRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteSupplierRequest") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteSupplierRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteSupplierRequest

    Public Shared Function DeleteSupplierRequestApproval(ByVal SUPNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Supplier_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteSupplierRequestApproval")
            DeleteSupplierRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteSupplierRequestApproval") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteSupplierRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteSupplierRequestApproval

    Public Shared Function DeleteSupplierRequestDocuments(ByVal DocID As Integer, ByVal SUPNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Supplier_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteSupplierRequestDocuments")
            DeleteSupplierRequestDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteSupplierRequestDocuments") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteSupplierRequestDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteSupplierRequestDocuments

    Public Shared Sub DeleteSupplierRequestCookies()

        Try
            HttpContext.Current.Response.Cookies("SUP_SUPNo").Value = ""
            HttpContext.Current.Response.Cookies("SUP_SUPNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_RBID").Value = ""
            HttpContext.Current.Response.Cookies("SUP_RBID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_Vendor").Value = ""
            HttpContext.Current.Response.Cookies("SUP_Vendor").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_SName").Value = ""
            HttpContext.Current.Response.Cookies("SUP_SName").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_PDesc").Value = ""
            HttpContext.Current.Response.Cookies("SUP_PDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_VTYPE").Value = ""
            HttpContext.Current.Response.Cookies("SUP_VTYPE").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_VTDesc").Value = ""
            HttpContext.Current.Response.Cookies("SUP_VTDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_Loc").Value = ""
            HttpContext.Current.Response.Cookies("SUP_Loc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_RSTAT").Value = ""
            HttpContext.Current.Response.Cookies("SUP_RSTAT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_DSF").Value = ""
            HttpContext.Current.Response.Cookies("SUP_DSF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SUP_DST").Value = ""
            HttpContext.Current.Response.Cookies("SUP_DST").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierRequestCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierRequestCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub 'EOF DeleteSupplierRequestCookies

    Public Shared Sub DeleteSupplierLookUpCookies()
        Try
            HttpContext.Current.Response.Cookies("SUPLU_SUPNo").Value = ""
            HttpContext.Current.Response.Cookies("SUPLU_SUPNo").Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies("SUPLU_SName").Value = ""
            HttpContext.Current.Response.Cookies("SUPLU_SName").Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies("SUPLU_VTYPE").Value = ""
            HttpContext.Current.Response.Cookies("SUPLU_VTYPE").Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies("SUPLU_RSTAT").Value = ""
            HttpContext.Current.Response.Cookies("SUPLU_RSTAT").Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies("SUPLU_Vendor").Value = ""
            HttpContext.Current.Response.Cookies("SUPLU_Vendor").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierLookUpCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierLookUp.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierLookUpCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub 'EOF DeleteSupplierLookUpCookies

    Public Shared Function GetSupplierRequest(ByVal SUPNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.VarChar)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SUP")

            GetSupplierRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SUPNo: " & SUPNo

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequest") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequest

    Public Shared Function GetSupplierRequestSearch(ByVal SUPNo As String, ByVal RequestedByTMID As Integer, ByVal VendorNo As String, ByVal VendorName As String, ByVal ProdDesc As String, ByVal VendorType As String, ByVal VTypeDesc As String, ByVal UGNFacility As String, ByVal RecStatus As String, ByVal DateSubFrom As String, ByVal DateSubTo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.VarChar)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@VendorNo", SqlDbType.VarChar)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@ProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ProdDesc").Value = commonFunctions.replaceSpecialChar(ProdDesc, False)

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VTypeDesc", SqlDbType.VarChar)
            myCommand.Parameters("@VTypeDesc").Value = VTypeDesc

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@DateSubFrom", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubFrom").Value = DateSubFrom

            myCommand.Parameters.Add("@DateSubTo", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubTo").Value = DateSubTo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SUP")

            GetSupplierRequestSearch = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SUPNo: " & SUPNo

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestSearch : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequestSearch") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestSearch : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequestSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequestSearch

    Public Shared Function GetSupplierLookUp(ByVal SUPNo As String, ByVal VendorName As String, ByVal VendorType As String, ByVal RecStatus As String, ByVal VendorNo As String, ByVal BtnSrch As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.VarChar)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@VendorNo", SqlDbType.VarChar)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@BtnSrch", SqlDbType.VarChar)
            myCommand.Parameters("@BtnSrch").Value = BtnSrch

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SUP")

            GetSupplierLookUp = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SUPNo: " & SUPNo

            HttpContext.Current.Session("BLLerror") = "GetSupplierLookUp : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierLookUp") = "~/SUP/SupplierLookUp.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierLookUp : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierLookUp = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierLookUp

    Public Shared Function GetLastSupplierRequestNo(ByVal RequestedByTMID As Integer, ByVal SUPDescription As String, ByVal RoutingStatus As String, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Supplier_RequestNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@SUPDescription", SqlDbType.VarChar)
            myCommand.Parameters("@SUPDescription").Value = commonFunctions.replaceSpecialChar(SUPDescription, False)


            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SUPNo")

            GetLastSupplierRequestNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", RequestedByTMID: " & RequestedByTMID & ", SUPDescription: " & SUPDescription & ", RoutingStatus: " & RoutingStatus & ", DepartmentID: " & DepartmentID & ", GLNo: " & GLNo & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "GetLastSupplierRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastSupplierRequestNo") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastSupplierRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastSupplierRequestNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastSupplierRequestNo

    Public Shared Function GetSupplierRequestExpenditure(ByVal SUPNo As Integer, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupplierRequestExtension")

            GetSupplierRequestExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequestExpenditure") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequestExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequestExpenditure

    Public Shared Function GetSupplierRequestDocuments(ByVal SUPNo As Integer, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupplierRequestDocuments")

            GetSupplierRequestDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SUPNo: " & SUPNo & ", DocID: " & DocID
            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequestDocuments") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequestDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequestDocuments

    Public Shared Function GetSupplierRequestHistory(ByVal SUPNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupplierRequestHistory")

            GetSupplierRequestHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SUPNo: " & SUPNo
            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequestHistory") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequestHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequestHistory

    Public Shared Function GetVendorTerm(ByVal Term As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vendor_Term"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@Term", SqlDbType.VarChar)
            myCommand.Parameters("@Term").Value = Term

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Term")

            GetVendorTerm = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVendorTerm : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetVendorTerm") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetVendorTerm : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetVendorTerm = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetVendorTerm

    Public Shared Function GetSupplierRequiredForms(ByVal FormName As String, ByVal VendorType As String, ByVal ShowObsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Required_Forms"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormName", SqlDbType.VarChar)
            myCommand.Parameters("@FormName").Value = FormName

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@ShowObsolete", SqlDbType.Bit)
            myCommand.Parameters("@ShowObsolete").Value = ShowObsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupplierRequiredForms")

            GetSupplierRequiredForms = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequiredForms") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequiredForms = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSupplierRequiredForms

    Public Shared Function GetLastSupplierRequestNo(ByVal RequestedByTMID As Integer, ByVal VendorName As String, ByVal ProdDesc As String, ByVal RoutingStatus As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Supplier_RequestNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@ProdDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ProdDesc").Value = commonFunctions.replaceSpecialChar(ProdDesc, False)

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IORNo")

            GetLastSupplierRequestNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "GetLastSupplierRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastSupplierRequestNo") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastSupplierRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastSupplierRequestNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastSupplierRequestNo

    Public Shared Function GetSupplierRequestApproval(ByVal SUPNo As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Supplier_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSupplierRequestApproval")

            GetSupplierRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSupplierRequestApproval") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupplierRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSupplierRequestApproval

    Public Shared Function InsertSupplierRequest(ByVal VendorType As String, ByVal VTypeDesc As String, ByVal VendorName As String, ByVal VendorNo As Integer, ByVal InBPCS As Boolean, ByVal Ten99 As Boolean, ByVal Phone As String, ByVal ProductDescription As String, ByVal RequestedByTMID As Integer, ByVal DateSubmitted As String, ByVal UT As Boolean, ByVal UN As Boolean, ByVal UP As Boolean, ByVal UR As Boolean, ByVal US As Boolean, ByVal UW As Boolean, ByVal OH As Boolean, ByVal NewVendor As Boolean, ByVal ChangeToCurrentVendor As Boolean, ByVal SalesContactName As String, ByVal SalesFax As String, ByVal AcctContact As String, ByVal AcctPhone As String, ByVal AcctFax As String, ByVal RemitToAddr1 As String, ByVal RemitToAddr2 As String, ByVal RemitToAddr3 As String, ByVal RemitToAddr4 As String, ByVal RemitToCity As String, ByVal RemitToState As String, ByVal RemitToZip As String, ByVal RemitToCountry As String, ByVal CustServContact As String, ByVal CustServPhone As String, ByVal CustServFax As String, ByVal CustServEmail As String, ByVal ShipFromAddr1 As String, ByVal ShipFromAddr2 As String, ByVal ShipFromAddr3 As String, ByVal ShipFromAddr4 As String, ByVal ShipFromCity As String, ByVal ShipFromState As String, ByVal ShipFromZip As String, ByVal ShipFromCountry As String, ByVal Terms As Integer, ByVal PaymentType As String, ByVal InitialPurchaseAmt As Decimal, ByVal EstAmtAnnualPurchase As Decimal, ByVal ReplacesCurrentVendor As Boolean, ByVal ReplacesVendorNo As Integer, ByVal ReasonForAddition As String, ByVal RoutingStatus As String, ByVal RecStatus As String, ByVal FamilyID As Integer, ByVal SubFamilyID As Integer, ByVal ContractorOnSite As Boolean, ByVal FutureVendor As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VTypeDesc", SqlDbType.VarChar)
            myCommand.Parameters("@VTypeDesc").Value = VTypeDesc

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@InBPCS", SqlDbType.Bit)
            myCommand.Parameters("@InBPCS").Value = InBPCS

            myCommand.Parameters.Add("@Ten99", SqlDbType.Bit)
            myCommand.Parameters("@Ten99").Value = Ten99

            myCommand.Parameters.Add("@Phone", SqlDbType.VarChar)
            myCommand.Parameters("@Phone").Value = Phone

            myCommand.Parameters.Add("@ProductDescription", SqlDbType.VarChar)
            myCommand.Parameters("@ProductDescription").Value = commonFunctions.replaceSpecialChar(ProductDescription, False)

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

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

            myCommand.Parameters.Add("@NewVendor", SqlDbType.Bit)
            myCommand.Parameters("@NewVendor").Value = NewVendor

            myCommand.Parameters.Add("@ChangeToCurrentVendor", SqlDbType.Bit)
            myCommand.Parameters("@ChangeToCurrentVendor").Value = ChangeToCurrentVendor

            myCommand.Parameters.Add("@SalesContactName", SqlDbType.VarChar)
            myCommand.Parameters("@SalesContactName").Value = commonFunctions.replaceSpecialChar(SalesContactName, False)

            myCommand.Parameters.Add("@SalesFax", SqlDbType.VarChar)
            myCommand.Parameters("@SalesFax").Value = SalesFax

            myCommand.Parameters.Add("@AcctContact", SqlDbType.VarChar)
            myCommand.Parameters("@AcctContact").Value = commonFunctions.replaceSpecialChar(AcctContact, False)

            myCommand.Parameters.Add("@AcctPhone", SqlDbType.VarChar)
            myCommand.Parameters("@AcctPhone").Value = AcctPhone

            myCommand.Parameters.Add("@AcctFax", SqlDbType.VarChar)
            myCommand.Parameters("@AcctFax").Value = AcctFax

            myCommand.Parameters.Add("@RemitToAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr1").Value = commonFunctions.replaceSpecialChar(RemitToAddr1, False)

            myCommand.Parameters.Add("@RemitToAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr2").Value = commonFunctions.replaceSpecialChar(RemitToAddr2, False)

            myCommand.Parameters.Add("@RemitToAddr3", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr3").Value = commonFunctions.replaceSpecialChar(RemitToAddr3, False)

            myCommand.Parameters.Add("@RemitToAddr4", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr4").Value = commonFunctions.replaceSpecialChar(RemitToAddr4, False)

            myCommand.Parameters.Add("@RemitToCity", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToCity").Value = commonFunctions.replaceSpecialChar(RemitToCity, False)

            myCommand.Parameters.Add("@RemitToState", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToState").Value = RemitToState

            myCommand.Parameters.Add("@RemitToZip", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToZip").Value = RemitToZip

            myCommand.Parameters.Add("@RemitToCountry", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToCountry").Value = RemitToCountry

            myCommand.Parameters.Add("@CustServContact", SqlDbType.VarChar)
            myCommand.Parameters("@CustServContact").Value = commonFunctions.replaceSpecialChar(CustServContact, False)

            myCommand.Parameters.Add("@CustServPhone", SqlDbType.VarChar)
            myCommand.Parameters("@CustServPhone").Value = CustServPhone

            myCommand.Parameters.Add("@CustServFax", SqlDbType.VarChar)
            myCommand.Parameters("@CustServFax").Value = CustServFax

            myCommand.Parameters.Add("@CustServEmail", SqlDbType.VarChar)
            myCommand.Parameters("@CustServEmail").Value = commonFunctions.replaceSpecialChar(CustServEmail, False)

            myCommand.Parameters.Add("@ShipFromAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr1").Value = commonFunctions.replaceSpecialChar(ShipFromAddr1, False)

            myCommand.Parameters.Add("@ShipFromAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr2").Value = commonFunctions.replaceSpecialChar(ShipFromAddr2, False)

            myCommand.Parameters.Add("@ShipFromAddr3", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr3").Value = commonFunctions.replaceSpecialChar(ShipFromAddr3, False)

            myCommand.Parameters.Add("@ShipFromAddr4", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr4").Value = commonFunctions.replaceSpecialChar(ShipFromAddr4, False)

            myCommand.Parameters.Add("@ShipFromCity", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromCity").Value = commonFunctions.replaceSpecialChar(ShipFromCity, False)

            myCommand.Parameters.Add("@ShipFromState", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromState").Value = commonFunctions.replaceSpecialChar(ShipFromState, False)

            myCommand.Parameters.Add("@ShipFromZip", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromZip").Value = ShipFromZip

            myCommand.Parameters.Add("@ShipFromCountry", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromCountry").Value = ShipFromCountry

            myCommand.Parameters.Add("@Terms", SqlDbType.Int)
            myCommand.Parameters("@Terms").Value = Terms

            myCommand.Parameters.Add("@PaymentType", SqlDbType.VarChar)
            myCommand.Parameters("@PaymentType").Value = PaymentType

            myCommand.Parameters.Add("@InitialPurchaseAmt", SqlDbType.Decimal)
            myCommand.Parameters("@InitialPurchaseAmt").Value = InitialPurchaseAmt

            myCommand.Parameters.Add("@EstAmtAnnualPurchase", SqlDbType.Decimal)
            myCommand.Parameters("@EstAmtAnnualPurchase").Value = EstAmtAnnualPurchase

            myCommand.Parameters.Add("@ReplacesCurrentVendor", SqlDbType.Bit)
            myCommand.Parameters("@ReplacesCurrentVendor").Value = ReplacesCurrentVendor

            myCommand.Parameters.Add("@ReplacesVendorNo", SqlDbType.Int)
            myCommand.Parameters("@ReplacesVendorNo").Value = ReplacesVendorNo

            myCommand.Parameters.Add("@ReasonForAddition", SqlDbType.VarChar)
            myCommand.Parameters("@ReasonForAddition").Value = commonFunctions.replaceSpecialChar(ReasonForAddition, False)

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@ContractorOnSite", SqlDbType.Bit)
            myCommand.Parameters("@ContractorOnSite").Value = ContractorOnSite

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Bit)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewSupplierRequest")
            InsertSupplierRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", VendorType: " & VendorType & ", VTypeDesc: " & VTypeDesc & ", VendorName: " & VendorName & ", VendorNo: " & VendorNo & ", InBPCS: " & InBPCS & ", Ten99: " & Ten99 & ", Phone: " & Phone & ", ProductDescription: " & ProductDescription & ", RequestedByTMID: " & RequestedByTMID & ", DateSubmitted: " & DateSubmitted & ", UT: " & UT & ", UN: " & UN & ", UP: " & UP & ", UR: " & UR & ", US: " & US & ", NewVendor: " & NewVendor & ", ChangeToCurrentVendor: " & ChangeToCurrentVendor & ", SalesContactName: " & SalesContactName & ", AcctContact: " & AcctContact & ", AcctPhone: " & AcctPhone & ", AcctFax: " & AcctFax & ", RemitToAddr1: " & RemitToAddr1 & ", RemitToAddr2: " & RemitToAddr2 & ", RemitToAddr3: " & RemitToAddr3 & ", RemitToAddr4: " & RemitToAddr4 & ", RemitToCity: " & RemitToCity & ", RemitToState: " & RemitToState & ", RemitToZip: " & RemitToZip & ", CustServContact: " & CustServContact & ", CustServPhone: " & CustServPhone & ", CustServFax: " & CustServFax & ", ShipFromAddr1: " & ShipFromAddr1 & ", ShipFromAddr2: " & ShipFromAddr2 & ", ShipFromAddr3: " & ShipFromAddr3 & ", ShipFromAddr4: " & ShipFromAddr4 & ", ShipFromCity: " & ShipFromCity & ", ShipFromState: " & ShipFromState & ", ShipFromZip: " & ShipFromZip & ", Terms: " & Terms & ", Terms: " & Terms & ", PaymentType: " & PaymentType & ", InitialPurchaseAmt: " & InitialPurchaseAmt & ", EstAmtAnnualPurchase: " & EstAmtAnnualPurchase & ", ReplacesCurrentVendor: " & ReplacesCurrentVendor & ", ReplacesVendorNo: " & ReplacesVendorNo & ", ReasonForAddition: " & ReasonForAddition & ", RoutingStatus: " & RoutingStatus & ", RecStatus: " & RecStatus & ", FamilyID: " & FamilyID & ", SubFamilyID: " & SubFamilyID & ", ContractorOnSite : " & ContractorOnSite & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequest") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequest

    Public Shared Function InsertSupplierRequestDocuments(ByVal SUPNo As Integer, ByVal SRFID As Integer, ByVal TeamMemberID As Integer, ByVal FormName As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@SRFID", SqlDbType.Int)
            myCommand.Parameters("@SRFID").Value = SRFID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@FormName", SqlDbType.VarChar)
            myCommand.Parameters("@FormName").Value = commonFunctions.replaceSpecialChar(FormName, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSupplierRequestDocuments")

            InsertSupplierRequestDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", TeamMember: " & TeamMemberID & ", FormName: " & FormName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequestDocuments") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequestDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequestDocuments

    Public Shared Function InsertSupplierRequestApproval(ByVal SUPNo As Integer, ByVal RequestedByTMID As Integer, ByVal FamilyID As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSupplierRequestApproval")

            InsertSupplierRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", RequestedByTMID: " & RequestedByTMID & ", FamilyID: " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequestApproval") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequestApproval

    Public Shared Function InsertSupplierRequestRSS(ByVal SUPNo As Integer, ByVal SUPDescription As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@SUPDescription", SqlDbType.VarChar)
            myCommand.Parameters("@SUPDescription").Value = commonFunctions.replaceSpecialChar(SUPDescription, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSupplierRequestHistory")

            InsertSupplierRequestRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", SUPDescription: " & commonFunctions.replaceSpecialChar(SUPDescription, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequestRSS") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequestRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequestRSS

    Public Shared Function InsertSupplierRequestRSSReply(ByVal SUPNo As Integer, ByVal RSSID As Integer, ByVal SUPDescription As String, ByVal TeamMemberID As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@SUPDescription", SqlDbType.VarChar)
            myCommand.Parameters("@SUPDescription").Value = commonFunctions.replaceSpecialChar(SUPDescription, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSupplierRequestRSSReply")

            InsertSupplierRequestRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", SUPDescription: " & commonFunctions.replaceSpecialChar(SUPDescription, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequestRSSReply") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequestRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequestRSSReply

    Public Shared Function InsertSupplierRequestHistory(ByVal SUPNo As Integer, ByVal VendorName As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Supplier_Request_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSupplierRequestHistory")

            InsertSupplierRequestHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", VendorName: " & commonFunctions.replaceSpecialChar(VendorName, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSupplierRequestHistory") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSupplierRequestHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertSupplierRequestHistory

    Public Shared Function UpdateSupplierRequest(ByVal SUPNo As Integer, ByVal VendorType As String, ByVal VTypeDesc As String, ByVal VendorName As String, ByVal VendorNo As Integer, ByVal InBPCS As Boolean, ByVal Ten99 As Boolean, ByVal Phone As String, ByVal ProductDescription As String, ByVal RequestedByTMID As Integer, ByVal DateSubmitted As String, ByVal UT As Boolean, ByVal UN As Boolean, ByVal UP As Boolean, ByVal UR As Boolean, ByVal US As Boolean, ByVal UW As Boolean, ByVal OH As Boolean, ByVal NewVendor As Boolean, ByVal ChangeToCurrentVendor As Boolean, ByVal SalesContactName As String, ByVal SalesFax As String, ByVal AcctContact As String, ByVal AcctPhone As String, ByVal AcctFax As String, ByVal RemitToAddr1 As String, ByVal RemitToAddr2 As String, ByVal RemitToAddr3 As String, ByVal RemitToAddr4 As String, ByVal RemitToCity As String, ByVal RemitToState As String, ByVal RemitToZip As String, ByVal RemitToCountry As String, ByVal CustServContact As String, ByVal CustServPhone As String, ByVal CustServFax As String, ByVal CustServEmail As String, ByVal ShipFromAddr1 As String, ByVal ShipFromAddr2 As String, ByVal ShipFromAddr3 As String, ByVal ShipFromAddr4 As String, ByVal ShipFromCity As String, ByVal ShipFromState As String, ByVal ShipFromZip As String, ByVal ShipFromCountry As String, ByVal Terms As Integer, ByVal PaymentType As String, ByVal InitialPurchaseAmt As Decimal, ByVal EstAmtAnnualPurchase As Decimal, ByVal ReplacesCurrentVendor As Boolean, ByVal ReplacesVendorNo As Integer, ByVal ReasonForAddition As String, ByVal RoutingStatus As String, ByVal RecStatus As String, ByVal FamilyID As Integer, ByVal SubFamilyID As Integer, ByVal ContractorOnSite As Boolean, ByVal VoidReason As String, ByVal FutureVendor As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Supplier_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VTypeDesc", SqlDbType.VarChar)
            myCommand.Parameters("@VTypeDesc").Value = VTypeDesc

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@InBPCS", SqlDbType.Bit)
            myCommand.Parameters("@InBPCS").Value = InBPCS

            myCommand.Parameters.Add("@Ten99", SqlDbType.Bit)
            myCommand.Parameters("@Ten99").Value = Ten99

            myCommand.Parameters.Add("@Phone", SqlDbType.VarChar)
            myCommand.Parameters("@Phone").Value = Phone

            myCommand.Parameters.Add("@ProductDescription", SqlDbType.VarChar)
            myCommand.Parameters("@ProductDescription").Value = commonFunctions.replaceSpecialChar(ProductDescription, False)

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

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

            myCommand.Parameters.Add("@NewVendor", SqlDbType.Bit)
            myCommand.Parameters("@NewVendor").Value = NewVendor

            myCommand.Parameters.Add("@ChangeToCurrentVendor", SqlDbType.Bit)
            myCommand.Parameters("@ChangeToCurrentVendor").Value = ChangeToCurrentVendor

            myCommand.Parameters.Add("@SalesContactName", SqlDbType.VarChar)
            myCommand.Parameters("@SalesContactName").Value = commonFunctions.replaceSpecialChar(SalesContactName, False)

            myCommand.Parameters.Add("@SalesFax", SqlDbType.VarChar)
            myCommand.Parameters("@SalesFax").Value = SalesFax

            myCommand.Parameters.Add("@AcctContact", SqlDbType.VarChar)
            myCommand.Parameters("@AcctContact").Value = commonFunctions.replaceSpecialChar(AcctContact, False)

            myCommand.Parameters.Add("@AcctPhone", SqlDbType.VarChar)
            myCommand.Parameters("@AcctPhone").Value = AcctPhone

            myCommand.Parameters.Add("@AcctFax", SqlDbType.VarChar)
            myCommand.Parameters("@AcctFax").Value = AcctFax

            myCommand.Parameters.Add("@RemitToAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr1").Value = commonFunctions.replaceSpecialChar(RemitToAddr1, False)

            myCommand.Parameters.Add("@RemitToAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr2").Value = commonFunctions.replaceSpecialChar(RemitToAddr2, False)

            myCommand.Parameters.Add("@RemitToAddr3", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr3").Value = commonFunctions.replaceSpecialChar(RemitToAddr3, False)

            myCommand.Parameters.Add("@RemitToAddr4", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToAddr4").Value = commonFunctions.replaceSpecialChar(RemitToAddr4, False)

            myCommand.Parameters.Add("@RemitToCity", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToCity").Value = commonFunctions.replaceSpecialChar(RemitToCity, False)

            myCommand.Parameters.Add("@RemitToState", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToState").Value = RemitToState

            myCommand.Parameters.Add("@RemitToZip", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToZip").Value = RemitToZip

            myCommand.Parameters.Add("@RemitToCountry", SqlDbType.VarChar)
            myCommand.Parameters("@RemitToCountry").Value = RemitToCountry

            myCommand.Parameters.Add("@CustServContact", SqlDbType.VarChar)
            myCommand.Parameters("@CustServContact").Value = commonFunctions.replaceSpecialChar(CustServContact, False)

            myCommand.Parameters.Add("@CustServPhone", SqlDbType.VarChar)
            myCommand.Parameters("@CustServPhone").Value = CustServPhone

            myCommand.Parameters.Add("@CustServFax", SqlDbType.VarChar)
            myCommand.Parameters("@CustServFax").Value = CustServFax

            myCommand.Parameters.Add("@CustServEmail", SqlDbType.VarChar)
            myCommand.Parameters("@CustServEmail").Value = commonFunctions.replaceSpecialChar(CustServEmail, False)

            myCommand.Parameters.Add("@ShipFromAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr1").Value = commonFunctions.replaceSpecialChar(ShipFromAddr1, False)

            myCommand.Parameters.Add("@ShipFromAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr2").Value = commonFunctions.replaceSpecialChar(ShipFromAddr2, False)

            myCommand.Parameters.Add("@ShipFromAddr3", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr3").Value = commonFunctions.replaceSpecialChar(ShipFromAddr3, False)

            myCommand.Parameters.Add("@ShipFromAddr4", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromAddr4").Value = commonFunctions.replaceSpecialChar(ShipFromAddr4, False)

            myCommand.Parameters.Add("@ShipFromCity", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromCity").Value = commonFunctions.replaceSpecialChar(ShipFromCity, False)

            myCommand.Parameters.Add("@ShipFromState", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromState").Value = commonFunctions.replaceSpecialChar(ShipFromState, False)

            myCommand.Parameters.Add("@ShipFromZip", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromZip").Value = ShipFromZip

            myCommand.Parameters.Add("@ShipFromCountry", SqlDbType.VarChar)
            myCommand.Parameters("@ShipFromCountry").Value = ShipFromCountry

            myCommand.Parameters.Add("@Terms", SqlDbType.Int)
            myCommand.Parameters("@Terms").Value = Terms

            myCommand.Parameters.Add("@PaymentType", SqlDbType.VarChar)
            myCommand.Parameters("@PaymentType").Value = PaymentType

            myCommand.Parameters.Add("@InitialPurchaseAmt", SqlDbType.Decimal)
            myCommand.Parameters("@InitialPurchaseAmt").Value = InitialPurchaseAmt

            myCommand.Parameters.Add("@EstAmtAnnualPurchase", SqlDbType.Decimal)
            myCommand.Parameters("@EstAmtAnnualPurchase").Value = EstAmtAnnualPurchase

            myCommand.Parameters.Add("@ReplacesCurrentVendor", SqlDbType.Bit)
            myCommand.Parameters("@ReplacesCurrentVendor").Value = ReplacesCurrentVendor

            myCommand.Parameters.Add("@ReplacesVendorNo", SqlDbType.Int)
            myCommand.Parameters("@ReplacesVendorNo").Value = ReplacesVendorNo

            myCommand.Parameters.Add("@ReasonForAddition", SqlDbType.VarChar)
            myCommand.Parameters("@ReasonForAddition").Value = commonFunctions.replaceSpecialChar(ReasonForAddition, False)

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@ContractorOnSite", SqlDbType.Bit)
            myCommand.Parameters("@ContractorOnSite").Value = ContractorOnSite

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Bit)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateSupplierRequest")
            UpdateSupplierRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", VendorType: " & VendorType & ", VTypeDesc: " & VTypeDesc & ", VendorName: " & VendorName & ", VendorNo: " & VendorNo & ", InBPCS: " & InBPCS & ", Ten99: " & Ten99 & ", Phone: " & Phone & ", ProductDescription: " & ProductDescription & ", RequestedByTMID: " & RequestedByTMID & ", DateSubmitted: " & DateSubmitted & ", UT: " & UT & ", UN: " & UN & ", UP: " & UP & ", UR: " & UR & ", NewVendor: " & NewVendor & ", ChangeToCurrentVendor: " & ChangeToCurrentVendor & ", SalesContactName: " & SalesContactName & ", AcctContact: " & AcctContact & ", AcctPhone: " & AcctPhone & ", AcctFax: " & AcctFax & ", RemitToAddr1: " & RemitToAddr1 & ", RemitToAddr2: " & RemitToAddr2 & ", RemitToAddr3: " & RemitToAddr3 & ", RemitToAddr4: " & RemitToAddr4 & ", RemitToCity: " & RemitToCity & ", RemitToState: " & RemitToState & ", RemitToZip: " & RemitToZip & ", CustServContact: " & CustServContact & ", CustServPhone: " & CustServPhone & ", CustServFax: " & CustServFax & ", ShipFromAddr1: " & ShipFromAddr1 & ", ShipFromAddr2: " & ShipFromAddr2 & ", ShipFromAddr3: " & ShipFromAddr3 & ", ShipFromAddr4: " & ShipFromAddr4 & ", ShipFromCity: " & ShipFromCity & ", ShipFromState: " & ShipFromState & ", ShipFromZip: " & ShipFromZip & ", Terms: " & Terms & ", Terms: " & Terms & ", PaymentType: " & PaymentType & ", InitialPurchaseAmt: " & InitialPurchaseAmt & ", EstAmtAnnualPurchase: " & EstAmtAnnualPurchase & ", ReplacesCurrentVendor: " & ReplacesCurrentVendor & ", ReplacesVendorNo: " & ReplacesVendorNo & ", ReasonForAddition: " & ReasonForAddition & ", RoutingStatus: " & RoutingStatus & ", RecStatus: " & RecStatus & ", FamilyID: " & FamilyID & ", SubFamilyID: " & SubFamilyID & ", ContractorOnSite : " & ContractorOnSite & ", UpdatedBy : " & UpdatedBy & ", UpdatedOn: " & UpdatedOn

            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateSupplierRequest") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateSupplierRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateSupplierRequest

    Public Shared Function UpdateSupplierRequestExpenditure(ByVal EID As Integer, ByVal SUPNo As Integer, ByVal SizePN As String, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Notes As String, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Supplier_Request_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@SizePN", SqlDbType.VarChar)
            myCommand.Parameters("@SizePN").Value = commonFunctions.replaceSpecialChar(SizePN, False)

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = Notes

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateSupplierRequestExpenditure")

            UpdateSupplierRequestExpenditure = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequestExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateSupplierRequestExpenditure") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateSupplierRequestExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateSupplierRequestExpenditure

    Public Shared Function UpdateSupplierRequestDocuments(ByVal SUPNo As Integer, ByVal DocID As Integer, ByVal SRFID As Integer, ByVal TeamMemberID As Integer, ByVal FormName As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Supplier_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myCommand.Parameters.Add("@SRFID", SqlDbType.Int)
            myCommand.Parameters("@SRFID").Value = SRFID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@FormName", SqlDbType.VarChar)
            myCommand.Parameters("@FormName").Value = commonFunctions.replaceSpecialChar(FormName, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateSupplierRequestDocuments")

            UpdateSupplierRequestDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", DocID: " & DocID & ", SRFID: " & SRFID & ", TeamMember: " & TeamMemberID & ", FormName: " & FormName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateSupplierRequestDocuments") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateSupplierRequestDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateSupplierRequestDocuments

    Public Shared Function UpdateSupplierRequestApproval(ByVal SUPNo As Integer, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Supplier_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

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

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateSupplierRequestApproval")

            UpdateSupplierRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", TMID: " & TMID & ", TMSigned: " & TMSigned & ", Status: " & Status & ", Comments: " & Comments & ", SeqNo: " & SeqNo & ", SameTMID: " & SameTMID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SUPModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateSupplierRequestApproval") = "~/SUP/SupplierRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateSupplierRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateSupplierRequestApproval

    Public Shared Sub UpdateSupplierRequestStatus(ByVal SUPNo As Integer, ByVal ProjectStatus As String, ByVal RoutingStatus As String, ByVal InBPCS As Boolean, ByVal Ten99 As Boolean, ByVal VendorNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Supplier_Request_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SUPNo", SqlDbType.Int)
            myCommand.Parameters("@SUPNo").Value = SUPNo

            myCommand.Parameters.Add("@ProjectStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectStatus").Value = ProjectStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@InBPCS", SqlDbType.Bit)
            myCommand.Parameters("@InBPCS").Value = InBPCS

            myCommand.Parameters.Add("@Ten99", SqlDbType.Bit)
            myCommand.Parameters("@Ten99").Value = Ten99

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequestStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateSupplierRequestStatus") = "~/SUP/SupplierRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequestStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SUPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateSupplierRequestStatus
End Class
