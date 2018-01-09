''************************************************************************************************
''Name:		PURModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Purchasing Module
''
''Date		    Author	    
''08/27/2010    LRey			Created .Net application
''05/16/2012    LRey            Added VendorEmail and Notes to Insert/Update functions
''06/27/2012    LRey            Added BuyerTMID to the InsertInternalOrderRequest and UpdateInternalOrderRequest
''                              Removed all unecessary parameters in the GetInternalOrderRequest function
''                              leaving the IORNo for data retrieval. The search page uses 
''                              GetInternalOrderRequestwSecurity.
''07/02/2012    LRey            Added FutureVendor value to UpdateInternalOrderRequest function
''07/19/2012	LRey	        Changed the data type to PONo from int to varchar to allow
''								Buyer's to type in PCARD when it doesn't required a PONo
''07/20/2012    LRey            Added SubmittedByTMID for IS Infrastructure to issue IOR's for other Requisitioner's
''04/26/2013    LRey            Added POinPesos field to the Insert/UpdateInternalOrderRequests functions
''02/25/2014    LRey            Replaced DepartmentID int param to Department string in GetInternalOrderRequestwSecurity function.
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

Public Class PURModule

    Public Shared Sub CleanFormCrystalReports()

        Dim tempRpt As ReportDocument = New ReportDocument()
        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then

            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If

    End Sub 'EOF CleanFormCrystalReports

    Public Shared Sub DeleteInternalOrderRequestCookies()

        Try
            HttpContext.Current.Response.Cookies("IOR_IORNo").Value = ""
            HttpContext.Current.Response.Cookies("IOR_IORNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_IDesc").Value = ""
            HttpContext.Current.Response.Cookies("IOR_IDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_Loc").Value = ""
            HttpContext.Current.Response.Cookies("IOR_Loc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_RBID").Value = ""
            HttpContext.Current.Response.Cookies("IOR_RBID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_DeptID").Value = ""
            HttpContext.Current.Response.Cookies("IOR_DeptID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_GLNo").Value = ""
            HttpContext.Current.Response.Cookies("IOR_GLNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_PONO").Value = ""
            HttpContext.Current.Response.Cookies("IOR_PONO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_VTYPE").Value = ""
            HttpContext.Current.Response.Cookies("IOR_VTYPE").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_VNDNO").Value = ""
            HttpContext.Current.Response.Cookies("IOR_VNDNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_VNDNO").Value = ""
            HttpContext.Current.Response.Cookies("IOR_VNDNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_IStat").Value = ""
            HttpContext.Current.Response.Cookies("IOR_IStat").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_CapEx").Value = ""
            HttpContext.Current.Response.Cookies("IOR_CapEx").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_BUYER").Value = ""
            HttpContext.Current.Response.Cookies("IOR_BUYER").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_DSF").Value = ""
            HttpContext.Current.Response.Cookies("IOR_DSF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_DST").Value = ""
            HttpContext.Current.Response.Cookies("IOR_DST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("IOR_SUB").Value = ""
            HttpContext.Current.Response.Cookies("IOR_SUB").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteInternalOrderRequestCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteInternalOrderRequestCookies : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeleteInternalOrderRequestCookies

    Public Shared Function DeleteInternalOrderRequest(ByVal IORNO As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Internal_Order_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeleteInternalOrderRequest")
            DeleteInternalOrderRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteInternalOrderRequest") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteInternalOrderRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeleteInternalOrderRequest

    Public Shared Sub DeleteInternalOrderRequestApproval(ByVal IORNO As Integer, ByVal Sequence As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Internal_Order_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteInternalOrderRequestApproval") = "~/PUR/InternalOrderRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteInternalOrderRequestApproval  

    Public Shared Function GetInternalOrderRequest(ByVal IORNO As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.VarChar)
            myCommand.Parameters("@IORNO").Value = IORNO

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IOR")

            GetInternalOrderRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", IORNO: " & IORNO

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequest") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetInternalOrderRequest

    Public Shared Function GetInternalOrderRequestwSecurity(ByVal IORNO As String, ByVal IORDescription As String, ByVal ShipToLocation As String, ByVal RequestedByTMID As Integer, ByVal BuyerTMID As Integer, ByVal Department As String, ByVal GLNo As Integer, ByVal PONo As String, ByVal VendorType As String, ByVal VendorNo As Integer, ByVal IORStatus As String, ByVal RoutingStatus As String, ByVal AppropriationCode As String, ByVal DateSubFrom As String, ByVal DateSubTo As String, ByVal DfltFacView As String, ByVal SubscriptionID As Integer, ByVal SubmittedByTMID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_wSecurity"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.VarChar)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@ShipToLocation", SqlDbType.VarChar)
            myCommand.Parameters("@ShipToLocation").Value = ShipToLocation

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@BuyerTMID", SqlDbType.Int)
            myCommand.Parameters("@BuyerTMID").Value = BuyerTMID

            myCommand.Parameters.Add("@Department", SqlDbType.VarChar)
            myCommand.Parameters("@Department").Value = Department

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@PONo", SqlDbType.VarChar)
            myCommand.Parameters("@PONo").Value = PONo

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@IORStatus", SqlDbType.VarChar)
            myCommand.Parameters("@IORStatus").Value = IORStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@AppropriationCode", SqlDbType.VarChar)
            myCommand.Parameters("@AppropriationCode").Value = AppropriationCode

            myCommand.Parameters.Add("@DateSubFrom", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubFrom").Value = DateSubFrom

            myCommand.Parameters.Add("@DateSubTo", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubTo").Value = DateSubTo

            myCommand.Parameters.Add("@DfltFacView", SqlDbType.VarChar)
            myCommand.Parameters("@DfltFacView").Value = DfltFacView

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IOR2")

            GetInternalOrderRequestwSecurity = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", IORNO: " & IORNO

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestwSecurity : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestwSecurity") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestwSecurity : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestwSecurity = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetInternalOrderRequestwSecurity


    Public Shared Function GetLastInternalOrderRequestNo(ByVal RequestedByTMID As Integer, ByVal IORDescription As String, ByVal ShipToLocation As String, ByVal RoutingStatus As String, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_Internal_Order_RequestNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@ShipToLocation", SqlDbType.VarChar)
            myCommand.Parameters("@ShipToLocation").Value = ShipToLocation

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
            myAdapter.Fill(GetData, "IORNo")

            GetLastInternalOrderRequestNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", RequestedByTMID: " & RequestedByTMID & ", IORDescription: " & IORDescription & ", ShipToLocation: " & ShipToLocation & ", RoutingStatus: " & RoutingStatus & ", DepartmentID: " & DepartmentID & ", GLNo: " & GLNo & ", CreatedBy: " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "GetLastInternalOrderRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastInternalOrderRequestNo") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastInternalOrderRequestNo : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastInternalOrderRequestNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastInternalOrderRequestNo

    Public Shared Function GetInternalOrderRequestExpenditure(ByVal IORNO As Integer, ByVal EID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InternalOrderRequestExtension")

            GetInternalOrderRequestExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", EID: " & EID
            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestExpenditure") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetInternalOrderRequestExpenditure

    Public Shared Function GetInternalOrderRequestDocument(ByVal IORNO As Integer, ByVal DocID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO ", SqlDbType.Int)
            myCommand.Parameters("@IORNO ").Value = IORNO

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, " GetInternalOrderRequestDocument ")

            GetInternalOrderRequestDocument = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNo: " & IORNO & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = " GetInternalOrderRequestDocument: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestDocument ") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestDocument: " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestDocument = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetInternalOrderRequestDocument

    Public Shared Function GetIORUGNFacility(ByVal facID As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_IOR_UGNFacility"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Facility")
            GetIORUGNFacility = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "facID: " & facID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetIORUGNFacility : " & commonFunctions.replaceSpecialChar(ex.Message, False) & _
            " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetIORUGNFacility : " & _
            commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetIORUGNFacility = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetIORUGNFacility

    Public Shared Function GetInternalOrderRequestApproval(ByVal IORNo As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNo", SqlDbType.Int)
            myCommand.Parameters("@IORNo").Value = IORNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetInternalOrderRequestApproval")

            GetInternalOrderRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestApproval") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetInternalOrderRequestApproval

    Public Shared Function GetInternalOrderRequestHistory(ByVal IORNO As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO ", SqlDbType.Int)
            myCommand.Parameters("@IORNO ").Value = IORNO

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, " GetInternalOrderRequestHistory ")

            GetInternalOrderRequestHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNo: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = " GetInternalOrderRequestHistory: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestHistory ") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestHistory: " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetInternalOrderRequestHistory

    Public Shared Function GetInternalOrderRequestCapEx(ByVal IORNO As Integer, ByVal ProjectNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_CapEx"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO ", SqlDbType.Int)
            myCommand.Parameters("@IORNO ").Value = IORNO

            myCommand.Parameters.Add("@ProjectNo ", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo ").Value = ProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, " GetInternalOrderRequestCapEx ")

            GetInternalOrderRequestCapEx = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNo: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = " GetInternalOrderRequestCapEx: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetInternalOrderRequestCapEx ") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestCapEx: " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestCapEx = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetInternalOrderRequestCapEx

    Public Shared Function GetInternalOrderRequestRSS(ByVal IORNO As Integer, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.VarChar)
            myCommand.Parameters("@IORNO").Value = IORNO


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IORRSS")

            GetInternalOrderRequestRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetInternalOrderRequestRSS") = "~/PUR/InternalOrderRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "EXPModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetInternalOrderRequestRSS

    Public Shared Function GetInternalOrderRequestRSSReply(ByVal IORNO As Integer, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Internal_Order_Request_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IORRSSReply")

            GetInternalOrderRequestRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetInternalOrderRequestRSSReply") = "~/PUR/InternalOrderRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInternalOrderRequestRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetAssetsExpProjRSSReply

    Public Shared Function GetTeamMemberDeptInChargeBySubscription(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_DeptInCharge_by_Subscription"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IOR")

            GetTeamMemberDeptInChargeBySubscription = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberDeptInChargeBySubscription : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetTeamMemberDeptInChargeBySubscription") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTeamMemberDeptInChargeBySubscription : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTeamMemberDeptInChargeBySubscription = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMemberDeptInChargeBySubscription

    Public Shared Function GetTeamMemberLocation(ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_Location"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "IOR")

            GetTeamMemberLocation = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberLocation : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetTeamMemberLocation") = "~/PUR/InternalOrderRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTeamMemberLocation : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTeamMemberLocation = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMemberLocation


    Public Shared Function InsertInternalOrderRequest(ByVal IORDescription As String, ByVal IORStatus As String, ByVal RoutingStatus As String, ByVal RequestedByTMID As Integer, ByVal PONo As String, ByVal AppropriationCode As String, ByVal ApprovedSpending As Decimal, ByVal ExpectedDeliveryDate As String, ByVal ShipToAttention As Integer, ByVal ShipTo As String, ByVal ShipToLocation As String, ByVal POinPesos As Boolean, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal VendorType As String, ByVal VendorNo As Integer, ByVal VendorName As String, ByVal VendorAddr1 As String, ByVal VendorAddr2 As String, ByVal VendorCountry As String, ByVal VendorCity As String, ByVal VendorState As String, ByVal VendorZip As String, ByVal VendorContact As String, ByVal VendorPhone As String, ByVal VendorFax As String, ByVal VendorWebSite As String, ByVal VendorEmail As String, ByVal TaxExempt As Boolean, ByVal Taxable As Boolean, ByVal ShippingPoint As Boolean, ByVal Destination As Boolean, ByVal Terms As String, ByVal Notes As String, ByVal BuyerTMID As Integer, ByVal SubmittedByTMID As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@IORStatus", SqlDbType.VarChar)
            myCommand.Parameters("@IORStatus").Value = IORStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@PONo", SqlDbType.VarChar)
            myCommand.Parameters("@PONo").Value = PONo

            myCommand.Parameters.Add("@AppropriationCode", SqlDbType.VarChar)
            myCommand.Parameters("@AppropriationCode").Value = AppropriationCode

            myCommand.Parameters.Add("@ApprovedSpending", SqlDbType.Decimal)
            myCommand.Parameters("@ApprovedSpending").Value = ApprovedSpending

            myCommand.Parameters.Add("@ExpectedDeliveryDate", SqlDbType.VarChar)
            myCommand.Parameters("@ExpectedDeliveryDate").Value = ExpectedDeliveryDate

            myCommand.Parameters.Add("@ShipToAttention", SqlDbType.Int)
            myCommand.Parameters("@ShipToAttention").Value = ShipToAttention

            myCommand.Parameters.Add("@ShipTo", SqlDbType.VarChar)
            myCommand.Parameters("@ShipTo").Value = ShipTo

            myCommand.Parameters.Add("@ShipToLocation", SqlDbType.VarChar)
            myCommand.Parameters("@ShipToLocation").Value = ShipToLocation

            myCommand.Parameters.Add("@POinPesos", SqlDbType.Bit)
            myCommand.Parameters("@POinPesos").Value = POinPesos

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@VendorAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@VendorAddr1").Value = commonFunctions.replaceSpecialChar(VendorAddr1, False)

            myCommand.Parameters.Add("@VendorAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@VendorAddr2").Value = commonFunctions.replaceSpecialChar(VendorAddr2, False)

            myCommand.Parameters.Add("@VendorCountry", SqlDbType.VarChar)
            myCommand.Parameters("@VendorCountry").Value = commonFunctions.replaceSpecialChar(VendorCountry, False)

            myCommand.Parameters.Add("@VendorCity", SqlDbType.VarChar)
            myCommand.Parameters("@VendorCity").Value = commonFunctions.replaceSpecialChar(VendorCity, False)

            myCommand.Parameters.Add("@VendorState", SqlDbType.VarChar)
            myCommand.Parameters("@VendorState").Value = commonFunctions.replaceSpecialChar(VendorState, False)

            myCommand.Parameters.Add("@VendorZip", SqlDbType.VarChar)
            myCommand.Parameters("@VendorZip").Value = VendorZip

            myCommand.Parameters.Add("@VendorContact", SqlDbType.VarChar)
            myCommand.Parameters("@VendorContact").Value = commonFunctions.replaceSpecialChar(VendorContact, False)

            myCommand.Parameters.Add("@VendorPhone", SqlDbType.VarChar)
            myCommand.Parameters("@VendorPhone").Value = VendorPhone

            myCommand.Parameters.Add("@VendorFax", SqlDbType.VarChar)
            myCommand.Parameters("@VendorFax").Value = VendorFax

            myCommand.Parameters.Add("@VendorWebSite", SqlDbType.VarChar)
            myCommand.Parameters("@VendorWebSite").Value = commonFunctions.replaceSpecialChar(VendorWebSite, False)

            myCommand.Parameters.Add("@VendorEmail", SqlDbType.VarChar)
            myCommand.Parameters("@VendorEmail").Value = commonFunctions.replaceSpecialChar(VendorEmail, False)

            myCommand.Parameters.Add("@TaxExempt", SqlDbType.Bit)
            myCommand.Parameters("@TaxExempt").Value = TaxExempt

            myCommand.Parameters.Add("@Taxable", SqlDbType.Bit)
            myCommand.Parameters("@Taxable").Value = Taxable

            myCommand.Parameters.Add("@ShippingPoint", SqlDbType.Bit)
            myCommand.Parameters("@ShippingPoint").Value = ShippingPoint

            myCommand.Parameters.Add("@Destination", SqlDbType.Bit)
            myCommand.Parameters("@Destination").Value = Destination

            myCommand.Parameters.Add("@Terms", SqlDbType.VarChar)
            myCommand.Parameters("@Terms").Value = Terms

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@BuyerTMID", SqlDbType.Int)
            myCommand.Parameters("@BuyerTMID").Value = BuyerTMID

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewInternalOrderRequest")
            InsertInternalOrderRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", IORDescription: " & IORDescription & ", IORStatus: " & IORStatus & ", RoutingStatus: " & RoutingStatus & ", RequestedByTMID: " & RequestedByTMID & ", PONo: " & PONo & ", AppropriationCode: " & AppropriationCode & ", ApprovedSpending: " & ApprovedSpending & ", ExpectedDeliveryDate: " & ExpectedDeliveryDate & ", ShipToAttention: " & ShipToAttention & ", ShipToLocation: " & ShipToLocation & ", DepartmentID: " & DepartmentID & ", GLNo: " & GLNo & ", VendorType: " & VendorType & ", VendorNo: " & VendorNo & ", VendorName: " & VendorName & ", VendorAddr1: " & VendorAddr1 & ", VendorAddr2: " & VendorAddr2 & ", VendorCountry: " & VendorCountry & ", VendorCity: " & VendorCity & ", VendorState: " & VendorState & ", VendorZip: " & VendorZip & ", VendorContact: " & VendorContact & ", VendorPhone: " & VendorPhone & ", VendorFax: " & VendorFax & ", VendorWebSite: " & VendorWebSite & ", TaxExempt: " & TaxExempt & ", Taxable: " & Taxable & ", ShippingPoint: " & ShippingPoint & ", Destination: " & Destination & ", Terms: " & Terms & ", CreatedBy : " & CreatedBy & ", CreatedOn: " & CreatedOn

            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequest") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF InsertInternalOrderRequest

    Public Shared Function InsertInternalOrderRequestExpenditure(ByVal IORNO As Integer, ByVal SizePN As String, ByVal Unit As Integer, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Currency As String, ByVal Notes As String, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@SizePN", SqlDbType.VarChar)
            myCommand.Parameters("@SizePN").Value = commonFunctions.replaceSpecialChar(SizePN, False)

            myCommand.Parameters.Add("@Unit", SqlDbType.Int)
            myCommand.Parameters("@Unit").Value = Unit

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Currency", SqlDbType.VarChar)
            myCommand.Parameters("@Currency").Value = Currency

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewInternalOrderRequestExpenditure")
            InsertInternalOrderRequestExpenditure = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestExpenditure") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestExpenditure

    Public Shared Function InsertInternalOrderRequestDocuments(ByVal IORNO As Integer, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertInternalOrderRequestDocuments")

            InsertInternalOrderRequestDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", TeamMember: " & TeamMemberID & ", File Description: " & commonFunctions.replaceSpecialChar(Description, False) & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestDocuments") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestDocuments

    Public Shared Function InsertInternalOrderRequestHistory(ByVal IORNO As Integer, ByVal IORDescription As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertInternalOrderRequestHistory")
            InsertInternalOrderRequestHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", IORDescription: " & commonFunctions.replaceSpecialChar(IORDescription, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestHistory") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestHistory

    Public Shared Function InsertInternalOrderRequestRSS(ByVal IORNO As Integer, ByVal IORDescription As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertInternalOrderRequestHistory")

            InsertInternalOrderRequestRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", IORDescription: " & commonFunctions.replaceSpecialChar(IORDescription, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestRSS") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestRSS

    Public Shared Function InsertInternalOrderRequestRSSReply(ByVal IORNO As Integer, ByVal RSSID As Integer, ByVal IORDescription As String, ByVal TeamMemberID As Integer, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDescription, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertInternalOrderRequestRSSReply")

            InsertInternalOrderRequestRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", IORDescription: " & commonFunctions.replaceSpecialChar(IORDescription, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestRSSReply") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestRSSReply

    Public Shared Function InsertInternalOrderRequestApproval(ByVal IORNO As Integer, ByVal TeamMemberID As Integer, ByVal TotalExpense As Decimal, ByVal HighLevel As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@HighLevel", SqlDbType.Bit)
            myCommand.Parameters("@HighLevel").Value = HighLevel

            myCommand.Parameters.Add("@TotalExpense", SqlDbType.Decimal)
            myCommand.Parameters("@TotalExpense").Value = TotalExpense

            'myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            'myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertInternalOrderRequestApproval")

            InsertInternalOrderRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", TeamMember: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestApproval") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestApproval

    Public Shared Function InsertInternalOrderRequestApprovalDefault(ByVal IORNO As Integer, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Internal_Order_Request_Approval_Default"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DefaultPUR1")

            InsertInternalOrderRequestApprovalDefault = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", TeamMember: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertInternalOrderRequestApprovalDefault : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertInternalOrderRequestApprovalDefault") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertInternalOrderRequestApprovalDefault : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertInternalOrderRequestApprovalDefault = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF InsertInternalOrderRequestApprovalDefault

    Public Shared Function UpdateInternalOrderRequest(ByVal IORNO As Integer, ByVal IORDEscription As String, ByVal IORStatus As String, ByVal RoutingStatus As String, ByVal RequestedByTMID As Integer, ByVal PONo As String, ByVal AppropriationCode As String, ByVal ApprovedSpending As Decimal, ByVal ExpectedDeliveryDate As String, ByVal ShipToAttention As Integer, ByVal ShipTo As String, ByVal ShipToLocation As String, ByVal POinPesos As Boolean, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal VendorType As String, ByVal FutureVendor As Boolean, ByVal VendorNo As String, ByVal VendorName As String, ByVal VendorAddr1 As String, ByVal VendorAddr2 As String, ByVal VendorCountry As String, ByVal VendorCity As String, ByVal VendorState As String, ByVal VendorZip As String, ByVal VendorContact As String, ByVal VendorPhone As String, ByVal VendorFax As String, ByVal VendorWebSite As String, ByVal VendorEmail As String, ByVal TaxExempt As Boolean, ByVal Taxable As Boolean, ByVal ShippingPoint As Boolean, ByVal Destination As Boolean, ByVal Terms As String, ByVal VoidReason As String, ByVal Notes As String, ByVal BuyerTMID As Integer, ByVal SubmittedOn As String, ByVal SubmittedByTMID As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Internal_Order_Request"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@IORDescription", SqlDbType.VarChar)
            myCommand.Parameters("@IORDescription").Value = commonFunctions.replaceSpecialChar(IORDEscription, False)

            myCommand.Parameters.Add("@IORStatus", SqlDbType.VarChar)
            myCommand.Parameters("@IORStatus").Value = IORStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@RequestedByTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTMID").Value = RequestedByTMID

            myCommand.Parameters.Add("@PONo", SqlDbType.VarChar)
            myCommand.Parameters("@PONo").Value = PONo

            myCommand.Parameters.Add("@AppropriationCode", SqlDbType.VarChar)
            myCommand.Parameters("@AppropriationCode").Value = AppropriationCode

            myCommand.Parameters.Add("@ApprovedSpending", SqlDbType.Decimal)
            myCommand.Parameters("@ApprovedSpending").Value = ApprovedSpending

            myCommand.Parameters.Add("@ExpectedDeliveryDate", SqlDbType.VarChar)
            myCommand.Parameters("@ExpectedDeliveryDate").Value = ExpectedDeliveryDate

            myCommand.Parameters.Add("@ShipToAttention", SqlDbType.Int)
            myCommand.Parameters("@ShipToAttention").Value = ShipToAttention

            myCommand.Parameters.Add("@ShipTo", SqlDbType.VarChar)
            myCommand.Parameters("@ShipTo").Value = ShipTo

            myCommand.Parameters.Add("@ShipToLocation", SqlDbType.VarChar)
            myCommand.Parameters("@ShipToLocation").Value = ShipToLocation

            myCommand.Parameters.Add("@POinPesos", SqlDbType.Bit)
            myCommand.Parameters("@POinPesos").Value = POinPesos

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@VendorType", SqlDbType.VarChar)
            myCommand.Parameters("@VendorType").Value = VendorType

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Bit)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myCommand.Parameters.Add("@VendorNo", SqlDbType.Int)
            myCommand.Parameters("@VendorNo").Value = VendorNo

            myCommand.Parameters.Add("@VendorName", SqlDbType.VarChar)
            myCommand.Parameters("@VendorName").Value = commonFunctions.replaceSpecialChar(VendorName, False)

            myCommand.Parameters.Add("@VendorAddr1", SqlDbType.VarChar)
            myCommand.Parameters("@VendorAddr1").Value = commonFunctions.replaceSpecialChar(VendorAddr1, False)

            myCommand.Parameters.Add("@VendorAddr2", SqlDbType.VarChar)
            myCommand.Parameters("@VendorAddr2").Value = commonFunctions.replaceSpecialChar(VendorAddr2, False)

            myCommand.Parameters.Add("@VendorCountry", SqlDbType.VarChar)
            myCommand.Parameters("@VendorCountry").Value = commonFunctions.replaceSpecialChar(VendorCountry, False)

            myCommand.Parameters.Add("@VendorCity", SqlDbType.VarChar)
            myCommand.Parameters("@VendorCity").Value = commonFunctions.replaceSpecialChar(VendorCity, False)

            myCommand.Parameters.Add("@VendorState", SqlDbType.VarChar)
            myCommand.Parameters("@VendorState").Value = commonFunctions.replaceSpecialChar(VendorState, False)

            myCommand.Parameters.Add("@VendorZip", SqlDbType.VarChar)
            myCommand.Parameters("@VendorZip").Value = VendorZip

            myCommand.Parameters.Add("@VendorContact", SqlDbType.VarChar)
            myCommand.Parameters("@VendorContact").Value = commonFunctions.replaceSpecialChar(VendorContact, False)

            myCommand.Parameters.Add("@VendorPhone", SqlDbType.VarChar)
            myCommand.Parameters("@VendorPhone").Value = VendorPhone

            myCommand.Parameters.Add("@VendorFax", SqlDbType.VarChar)
            myCommand.Parameters("@VendorFax").Value = VendorFax

            myCommand.Parameters.Add("@VendorWebSite", SqlDbType.VarChar)
            myCommand.Parameters("@VendorWebSite").Value = commonFunctions.replaceSpecialChar(VendorWebSite, False)

            myCommand.Parameters.Add("@VendorEmail", SqlDbType.VarChar)
            myCommand.Parameters("@VendorEmail").Value = commonFunctions.replaceSpecialChar(VendorEmail, False)

            myCommand.Parameters.Add("@TaxExempt", SqlDbType.Bit)
            myCommand.Parameters("@TaxExempt").Value = TaxExempt

            myCommand.Parameters.Add("@Taxable", SqlDbType.Bit)
            myCommand.Parameters("@Taxable").Value = Taxable

            myCommand.Parameters.Add("@ShippingPoint", SqlDbType.Bit)
            myCommand.Parameters("@ShippingPoint").Value = ShippingPoint

            myCommand.Parameters.Add("@Destination", SqlDbType.Bit)
            myCommand.Parameters("@Destination").Value = Destination

            myCommand.Parameters.Add("@Terms", SqlDbType.VarChar)
            myCommand.Parameters("@Terms").Value = Terms

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@BuyerTMID", SqlDbType.Int)
            myCommand.Parameters("@BuyerTMID").Value = BuyerTMID

            myCommand.Parameters.Add("@SubmittedOn", SqlDbType.VarChar)
            myCommand.Parameters("@SubmittedOn").Value = SubmittedOn

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateInternalOrderRequest")
            UpdateInternalOrderRequest = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", IORNO: " & IORNO & ", IORDescription: " & IORDEscription & ", IORStatus: " & IORStatus & ", RoutingStatus: " & RoutingStatus & ", RequestedByTMID: " & RequestedByTMID & ", PONo: " & PONo & ", AppropriationCode: " & AppropriationCode & ", ApprovedSpending: " & ApprovedSpending & ", ExpectedDeliveryDate: " & ExpectedDeliveryDate & ", ShipToAttention: " & ShipToAttention & ", ShipToLocation: " & ShipToLocation & ", DepartmentID: " & DepartmentID & ", GLNo: " & GLNo & ", VendorType: " & VendorType & ", VendorNo: " & VendorNo & ", VendorName: " & VendorName & ", VendorAddr1: " & VendorAddr1 & ", VendorAddr2: " & VendorAddr2 & ", VendorCountry: " & VendorCountry & ", VendorCity: " & VendorCity & ", VendorState: " & VendorState & ", VendorZip: " & VendorZip & ", VendorContact: " & VendorContact & ", VendorPhone: " & VendorPhone & ", VendorFax: " & VendorFax & ", VendorWebSite: " & VendorWebSite & ", TaxExempt: " & TaxExempt & ", Taxable: " & Taxable & ", ShippingPoint: " & ShippingPoint & ", Destination: " & Destination & ", Terms: " & Terms & ", UpdatedBy : " & UpdatedBy & ", UpdatedOn: " & UpdatedOn

            HttpContext.Current.Session("BLLerror") = "UpdateInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateInternalOrderRequest") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateInternalOrderRequest : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateInternalOrderRequest = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF UpdateInternalOrderRequest

    Public Shared Function UpdateInternalOrderRequestExpenditure(ByVal EID As Integer, ByVal IORNO As Integer, ByVal SizePN As String, ByVal Unit As Integer, ByVal Description As String, ByVal Quantity As Integer, ByVal Amount As Decimal, ByVal Currency As String, ByVal Notes As String, ByVal DepartmentID As Integer, ByVal GLNo As Integer, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Internal_Order_Request_Expenditure"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@EID", SqlDbType.Int)
            myCommand.Parameters("@EID").Value = EID

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

            myCommand.Parameters.Add("@SizePN", SqlDbType.VarChar)
            myCommand.Parameters("@SizePN").Value = commonFunctions.replaceSpecialChar(SizePN, False)

            myCommand.Parameters.Add("@Unit", SqlDbType.Int)
            myCommand.Parameters("@Unit").Value = Unit

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@Quantity", SqlDbType.Int)
            myCommand.Parameters("@Quantity").Value = Quantity

            myCommand.Parameters.Add("@Amount", SqlDbType.Decimal)
            myCommand.Parameters("@Amount").Value = Amount

            myCommand.Parameters.Add("@Currency", SqlDbType.VarChar)
            myCommand.Parameters("@Currency").Value = Currency

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.replaceSpecialChar(Notes, False)

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@GLNo", SqlDbType.Int)
            myCommand.Parameters("@GLNo").Value = GLNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateInternalOrderRequestExpenditure")

            UpdateInternalOrderRequestExpenditure = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", EID: " & EID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateInternalOrderRequestExpenditure: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateInternalOrderRequestExpenditure") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateInternalOrderRequestExpenditure : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateInternalOrderRequestExpenditure = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateInternalOrderRequestExpenditure

    Public Shared Function UpdateInternalOrderRequestApproval(ByVal IORNO As Integer, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Boolean, ByVal BackupTMID As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Internal_Order_Request_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNO", SqlDbType.Int)
            myCommand.Parameters("@IORNO").Value = IORNO

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

            myCommand.Parameters.Add("@BackupTMID", SqlDbType.Int)
            myCommand.Parameters("@BackupTMID").Value = BackupTMID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateInternalOrderRequestApproval")

            UpdateInternalOrderRequestApproval = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNO: " & IORNO & ", TMID: " & TMID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateInternalOrderRequestApproval: " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateInternalOrderRequestApproval") = "~/PUR/InternalOrderRequest.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateInternalOrderRequestApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateInternalOrderRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF UpdateInternalOrderRequestApproval

    Public Shared Sub UpdateInternalOrderRequestStatus(ByVal IORNo As Integer, ByVal IORStatus As String, ByVal RoutingStatus As String, ByVal PONO As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Internal_Order_Request_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@IORNo", SqlDbType.Int)
            myCommand.Parameters("@IORNo").Value = IORNo

            myCommand.Parameters.Add("@IORStatus", SqlDbType.VarChar)
            myCommand.Parameters("@IORStatus").Value = IORStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@PONO", SqlDbType.VarChar)
            myCommand.Parameters("@PONO").Value = PONO

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IORNo: " & IORNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateInternalOrderRequestStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PURModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateInternalOrderRequestStatus") = "~/PUR/InternalOrderRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateInternalOrderRequestStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PURModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub ' EOF UpdateInternalOrderRequestStatus

End Class
