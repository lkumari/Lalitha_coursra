''************************************************************************************************
''Name:		ARGroupModule.vb
''Purpose:	These are the functions behind the New AR Module from 2011, replacing the original ARGroupModule.VB
''          
''Date		    Author	    
'' 03/23/2010   Roderick Carlson		Created .Net application
'' 08/22/2011   Roderick Carlson        Added GetARPendingPartNo
'' 04/05/2012   LRey                    Added Region for "AR Operations Deduction" and related SP's
'' 04/09/2012   Roderick Carlson        Added Price Code and Facility to GetARPendingPartList Parameters
'' 05/25/2012   Roderick Carlson        Clear Cookies when GetShipHistory Stored Procedure has an error
'' 08/28/2012   LRey                    Added Additional Parameters to filter Search Page and clear cached cookies in delete
'' 09/20/2012   Roderick Carlson        Removed UpdatedBy from DeleteAREventDetail and DeleteAREventApprovalStatus, cleaned up some redirections
'' 09/21/2012   Roderick Carlson        Added Function Copy_AR_Event_Accrual_Override_Criteria
'' 10/11/2012   Roderick Carlson        Added parameters to GetShipHistory and GetShipHistoryTotal
'' 02/01/2013   LRey                    Added WSoldTo and PartNo to GetARDeduction
'' 03/06/2013   LRey                    Added a function for Counter Measure
'' 05/21/2013   LRey                    Added a function for DeleteARDeductionReportCookies
'' 12/20/2013   LRey                    Replaced SoldTo/CABBV with Customer.
''************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.VisualBasic

Public Class ARGroupModule
#Region "AR EVENT"
    Public Shared Sub CleanARCrystalReports()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("ARPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("ARPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ARPreview") = Nothing
                HttpContext.Current.Session("ARPreviewAREID") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanARCrystalReports: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanARCrystalReports: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub DeleteARShippingHistoryCookies()

        Try
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveFacilityHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveFacilityHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveStartShipDateHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveStartShipDateHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEndShipDateHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEndShipDateHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEffectiveDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEffectiveDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustomerHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustomerHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNoHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNoHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveSoldToHistory").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveSoldToHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCABBVHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCABBVHistory").Expires = DateTime.Now.AddDays(-1)



            HttpContext.Current.Response.Cookies("ARGroupModule_SaveQuantityShippedHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveQuantityShippedHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveINVNOHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveINVNOHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveStartREQDateHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveStartREQDateHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEndREQDateHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEndREQDateHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePriceCodeHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePriceCodeHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveRANNOHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveRANNOHistory").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePONOHistory").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePONOHistory").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteARShippingHistoryCookies: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARShippingHistoryCookies: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub DeleteAREventDetail(ByVal AREID As Integer, ByVal DeleteType As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Event_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If DeleteType Is Nothing Then
                DeleteType = ""
            End If

            myCommand.Parameters.Add("@DeleteType", SqlDbType.VarChar)
            myCommand.Parameters("@DeleteType").Value = DeleteType

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", DeleteType: " & DeleteType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAREventDetail: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetARShippingHistoryDynamically(ByVal AREID As Integer, ByVal KeyColumn As String, ByVal Columns As String, ByVal SQLWhereClause As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Shipping_History_Dynamically"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@KeyColumn", SqlDbType.VarChar)
            myCommand.Parameters("@KeyColumn").Value = KeyColumn

            myCommand.Parameters.Add("@Columns", SqlDbType.VarChar)
            myCommand.Parameters("@Columns").Value = Columns

            myCommand.Parameters.Add("@SQLWhere", SqlDbType.VarChar)
            myCommand.Parameters("@SQLWhere").Value = SQLWhereClause

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShippingHistory")
            GetARShippingHistoryDynamically = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SQLWhereClause: " & SQLWhereClause & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetARShippingHistoryDynamically: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARShippingHistoryDynamically: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARShippingHistoryDynamically = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARShippingHistory(ByVal COMPNY As String, ByVal CABBV As String, _
        ByVal SOLDTO As String, ByVal PARTNO As String, ByVal PRCCDE As String, _
        ByVal StartShipDate As String, ByVal EndShipDate As String, ByVal INVNO As String, _
        ByVal StartReqDate As String, ByVal EndReqDate As String, _
        ByVal RANNO As String, ByVal PONO As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Shipping_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            myCommand.Parameters.Add("@StartShipDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartShipDate").Value = StartShipDate

            myCommand.Parameters.Add("@EndShipDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndShipDate").Value = EndShipDate

            myCommand.Parameters.Add("@INVNO", SqlDbType.VarChar)
            myCommand.Parameters("@INVNO").Value = INVNO

            myCommand.Parameters.Add("@StartReqDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartReqDate").Value = StartReqDate

            myCommand.Parameters.Add("@EndReqDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndReqDate").Value = EndReqDate

            myCommand.Parameters.Add("@RANNO", SqlDbType.VarChar)
            myCommand.Parameters("@RANNO").Value = RANNO

            myCommand.Parameters.Add("@PONO", SqlDbType.VarChar)
            myCommand.Parameters("@PONO").Value = PONO

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShippingHistory")
            GetARShippingHistory = GetData
        Catch ex As Exception
            DeleteARShippingHistoryCookies()

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "COMPNY: " & COMPNY _
            & ", CABBV: " & CABBV _
            & ", SOLDTO: " & SOLDTO _
            & ", PARTNO: " & PARTNO _
            & ", PRCCDE: " & PRCCDE _
            & ", StartShipDate: " & StartShipDate _
            & ", EndShipDate: " & EndShipDate _
            & ", INVNO: " & INVNO _
            & ", StartReqDate: " & StartReqDate _
            & ", EndReqDate: " & EndReqDate _
            & ", RANNO: " & RANNO _
            & ", PONO: " & PONO _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARShippingHistory: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            'if a download is running this may error out
            'UGNErrorTrapping.InsertErrorLog("GetARShippingHistory: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARShippingHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARShippingHistoryByCustomerPartNo(ByVal PARTNO As String, ByVal CustomerPartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Shipping_History_By_Customer_PartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If PARTNO Is Nothing Then
                PARTNO = ""
            End If

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = CustomerPartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShippingHistoryParts")
            GetARShippingHistoryByCustomerPartNo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PARTNO: " & PARTNO & ", CustomerPartNo: " & CustomerPartNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARShippingHistoryByCustomerPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARShippingHistoryByCustomerPartNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARShippingHistoryTotal(ByVal COMPNY As String, ByVal CABBV As String, _
     ByVal SOLDTO As String, ByVal PARTNO As String, ByVal PRCCDE As String, _
     ByVal StartShipDate As String, ByVal EndShipDate As String, ByVal INVNO As String, _
     ByVal RANNO As String, ByVal PONO As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Shipping_History_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SOLDTO", SqlDbType.VarChar)
            myCommand.Parameters("@SOLDTO").Value = SOLDTO

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            myCommand.Parameters.Add("@StartShipDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartShipDate").Value = StartShipDate

            myCommand.Parameters.Add("@EndShipDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndShipDate").Value = EndShipDate

            myCommand.Parameters.Add("@INVNO", SqlDbType.VarChar)
            myCommand.Parameters("@INVNO").Value = INVNO

            myCommand.Parameters.Add("@RANNO", SqlDbType.VarChar)
            myCommand.Parameters("@RANNO").Value = RANNO

            myCommand.Parameters.Add("@PONO", SqlDbType.VarChar)
            myCommand.Parameters("@PONO").Value = PONO

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShippingHistoryTotal")
            GetARShippingHistoryTotal = GetData
        Catch ex As Exception
            DeleteARShippingHistoryCookies()

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "COMPNY: " & COMPNY _
            & ", CABBV: " & CABBV _
            & ", SOLDTO: " & SOLDTO _
            & ", PARTNO: " & PARTNO _
            & ", PRCCDE: " & PRCCDE _
            & ", StartShipDate: " & StartShipDate _
            & ", EndShipDate: " & EndShipDate _
            & ", INVNO: " & INVNO _
            & ", RANNO: " & RANNO _
            & ", PONO: " & PONO _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARShippingHistoryTotal: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARShippingHistoryTotal: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARShippingHistoryTotal = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARShippingPriceDynamically(ByVal AREID As Integer, ByVal SQLWhereClause As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Shipping_Price_Dynamically"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        'Dim strSQLWhereClause As String = " AND PartNo='552100E03000' and COMPNY='US' AND SHPDTE >= '20110101' and SHPDTE <= '20110401'"

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@SQLWhere", SqlDbType.VarChar)
            myCommand.Parameters("@SQLWhere").Value = SQLWhereClause

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShippingPrice")
            GetARShippingPriceDynamically = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & "SQLWhereClause: " _
            & ", SQLWhereClause: " & SQLWhereClause _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARShippingPriceDynamically: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARShippingPriceDynamically: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARShippingPriceDynamically = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetInvoicesOnHold(ByVal PartNo As String, ByVal PriceCode As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Invoices_On_Hold"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = commonFunctions.convertSpecialChar(PartNo, False)

            If PriceCode Is Nothing Then
                PriceCode = ""
            End If

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = commonFunctions.convertSpecialChar(PriceCode, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InvoicesOnHold")
            GetInvoicesOnHold = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo _
            & ", PriceCode: " & PriceCode _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInvoicesOnHold: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInvoicesOnHold: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInvoicesOnHold = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetInvoicesOnHoldPartList(ByVal AREID As Integer, ByVal PartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Invoices_On_Hold_Part_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = commonFunctions.convertSpecialChar(PartNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InvoiceOnHoldParts")
            GetInvoicesOnHoldPartList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetInvoicesOnHoldPartList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInvoicesOnHoldPartList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInvoicesOnHoldPartList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetInvoicesOnHoldPriceCodeByPartList(ByVal AREID As Integer, ByVal PartNo As String, _
        ByVal PriceCode As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Invoices_On_Hold_Price_Code_By_Part_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = commonFunctions.convertSpecialChar(PartNo, False)

            If PriceCode Is Nothing Then
                PriceCode = ""
            End If

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = commonFunctions.convertSpecialChar(PriceCode, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InvoiceOnHoldListPriceCodes")
            GetInvoicesOnHoldPriceCodeByPartList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo _
            & ", Price Code: " & PriceCode _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInvoicesOnHoldPriceCodeByPartList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInvoicesOnHoldPriceCodeByPartList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetInvoicesOnHoldPriceCodeByPartList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventAccrualPartList(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Accrual_Part_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AREventAccrualParts")
            GetAREventAccrualPartList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventAccrualPartList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventAccrualPartList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventAccrualPartList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventAccrualPriceCodeList(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Accrual_Price_Code_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AREventAccrualPriceCodes")
            GetAREventAccrualPriceCodeList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventAccrualPriceCodeList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventAccrualPriceCodeList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventAccrualPriceCodeList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventSearch(ByVal AREID As String, ByVal EventStatusID As Integer, ByVal EventDesc As String, _
      ByVal EventTypeID As Integer, ByVal AcctMgrTMID As Integer, ByVal FilterCustomerApproved As Boolean, _
      ByVal isCustomerApproved As Boolean, ByVal CustApprvEffDate As String, _
      ByVal CustApprvEndDate As String, ByVal UGNFacility As String, _
      ByVal Customer As String, _
      ByVal PartNo As String, ByVal PRCCDE As String, _
      ByVal PartName As String, ByVal ShowVoid As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If AREID Is Nothing Then
                AREID = ""
            End If

            myCommand.Parameters.Add("@AREID", SqlDbType.VarChar)
            myCommand.Parameters("@AREID").Value = commonFunctions.convertSpecialChar(AREID, False)

            myCommand.Parameters.Add("@EventStatusID", SqlDbType.Int)
            myCommand.Parameters("@EventStatusID").Value = EventStatusID

            myCommand.Parameters.Add("@EventDesc", SqlDbType.VarChar)
            myCommand.Parameters("@EventDesc").Value = EventDesc

            myCommand.Parameters.Add("@EventTypeID", SqlDbType.Int)
            myCommand.Parameters("@EventTypeID").Value = EventTypeID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@FilterCustomerApproved", SqlDbType.Bit)
            myCommand.Parameters("@FilterCustomerApproved").Value = FilterCustomerApproved

            myCommand.Parameters.Add("@isCustomerApproved", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerApproved").Value = isCustomerApproved

            If CustApprvEffDate Is Nothing Then CustApprvEffDate = ""

            myCommand.Parameters.Add("@CustApprvEffDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEffDate").Value = CustApprvEffDate

            If CustApprvEndDate Is Nothing Then CustApprvEndDate = ""
      
            myCommand.Parameters.Add("@CustApprvEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEndDate").Value = CustApprvEndDate

            If UGNFacility Is Nothing Then UGNFacility = ""

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If Customer Is Nothing Then Customer = ""

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            If PartName Is Nothing Then
                PartName = ""
            End If

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = PartName

            myCommand.Parameters.Add("@ShowVoid", SqlDbType.Bit)
            myCommand.Parameters("@ShowVoid").Value = ShowVoid

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventSearchResults")
            GetAREventSearch = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", EventStatusID:" & EventStatusID _
            & ", EventDesc :" & EventDesc & ", EventTypeID:" & EventTypeID _
            & ", AcctMgrTMID:" & AcctMgrTMID & ", isCustomerApproved:" & isCustomerApproved & ", CustApprvEffDate:" & CustApprvEffDate _
            & ", CustApprvEndDate:" & CustApprvEndDate & ", UGNFacility:" & UGNFacility _
            & ", Customer:" & Customer & ", PartNo:" & PartNo _
            & ", PRCCDE:" & PRCCDE & ", PartName:" & PartName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventSearch: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventSearch: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventSearch = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREvent(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventInfo")
            GetAREvent = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREvent: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREvent = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventStatusList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Status_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventStatusList")
            GetAREventStatusList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventStatusList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventStatusList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventStatusList = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventTypeList(ByVal isSales As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Type_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@isSales", SqlDbType.Bit)
            myCommand.Parameters("@isSales").Value = isSales

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventTypeList")
            GetAREventTypeList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "isSales: " & isSales _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventTypeList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventTypeList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventTypeList = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARApprovalStatusList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Approval_Status_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ApprovalTypeList")
            GetARApprovalStatusList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARApprovalStatusList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARApprovalStatusList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARApprovalStatusList = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetARFuturePartNo(ByVal AREID As Integer, ByVal PartNo As String, _
        ByVal PartDesc As String, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Future_PartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If PartNo = Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartDesc = Nothing Then
                PartDesc = ""
            End If

            myCommand.Parameters.Add("@PartDesc", SqlDbType.VarChar)
            myCommand.Parameters("@PartDesc").Value = PartDesc

            If CreatedBy = Nothing Then
                CreatedBy = ""
            End If

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetARFuturePartNo")
            GetARFuturePartNo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARFuturePartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARFuturePartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARFuturePartNo = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetARPendingPartNo(ByVal AREID As Integer, ByVal PartNo As String, _
        ByVal PRCCDE As String, ByVal COMPNY As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Pending_Part_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If PartNo = Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PRCCDE = Nothing Then
                PRCCDE = ""
            End If

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            If COMPNY = Nothing Then
                COMPNY = ""
            End If

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetARPendingPartNo")
            GetARPendingPartNo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARPendingPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARPendingPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARPendingPartNo = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventSoldTo(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_SoldTo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventSoldTo")
            GetAREventSoldTo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventSoldTo: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventSoldTo: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventSoldTo = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventFacility(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Facility"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventFacility")
            GetAREventFacility = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventFacility: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventFacility: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventFacility = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Function GetAREventPriceMasterList(ByVal AREID As Integer) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_AR_Event_Price_Master_List"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@AREID", SqlDbType.Int)
    '        myCommand.Parameters("@AREID").Value = AREID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "EventPriceMasterList")
    '        GetAREventPriceMasterList = GetData
    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "AREID: " & AREID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetAREventPriceMasterList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetAREventPriceMasterList : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetAREventPriceMasterList = Nothing

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetAREventDetail(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventDetail")
            GetAREventDetail = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventDetail = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventAccrual(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Accrual"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventAccrual")
            GetAREventAccrual = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventAccrual: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventAccrual: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventAccrual = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventAccrualTotals(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Accrual_Totals"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventAccrualTotals")
            GetAREventAccrualTotals = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventAccrualTotals: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventAccrualTotals: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventAccrualTotals = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventInvoicesOnHold(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Invoices_On_Hold"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventInvoicesOnHold")
            GetAREventInvoicesOnHold = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventInvoicesOnHold: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventInvoicesOnHold: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventInvoicesOnHold = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventSupportingDocList(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Supporting_Doc_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventSupportingDocList")
            GetAREventSupportingDocList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventSupportingDocList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventSupportingDocList: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventSupportingDocList = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventSupportingDoc(ByVal RowID As Integer, ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventSupportingDoc")
            GetAREventSupportingDoc = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventSupportingDoc = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetAREventHistory(ByVal AREID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Event_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "EventHistory")
            GetAREventHistory = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventHistory: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventHistory: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetAREventHistory = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function InsertAREvent(ByVal PrevAREID As Integer, ByVal EventTypeID As Integer, _
        ByVal EventStatusID As Integer, ByVal EventDesc As String, ByVal AcctMgrTMID As Integer, _
        ByVal CustApprvEffDate As String, ByVal CustApprvEndDate As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PrevAREID", SqlDbType.Int)
            myCommand.Parameters("@PrevAREID").Value = PrevAREID

            myCommand.Parameters.Add("@EventTypeID", SqlDbType.Int)
            myCommand.Parameters("@EventTypeID").Value = EventTypeID

            myCommand.Parameters.Add("@EventStatusID", SqlDbType.Int)
            myCommand.Parameters("@EventStatusID").Value = EventStatusID

            If EventDesc Is Nothing Then
                EventDesc = ""
            End If

            myCommand.Parameters.Add("@EventDesc", SqlDbType.VarChar)
            myCommand.Parameters("@EventDesc").Value = commonFunctions.convertSpecialChar(EventDesc, False)

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            'If Notes Is Nothing Then
            '    Notes = ""
            'End If

            'myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            'myCommand.Parameters("@Notes").Value = Notes

            If CustApprvEffDate Is Nothing Then
                CustApprvEffDate = ""
            End If

            myCommand.Parameters.Add("@CustApprvEffDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEffDate").Value = CustApprvEffDate

            If CustApprvEndDate Is Nothing Then
                CustApprvEndDate = ""
            End If

            myCommand.Parameters.Add("@CustApprvEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEndDate").Value = CustApprvEndDate

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewEventData")
            InsertAREvent = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PrevAREID:" & PrevAREID _
            & ", EventTypeID:" & EventTypeID _
            & ", EventStatusID:" & EventStatusID _
            & ", EventDesc:" & EventDesc _
            & ", AcctMgrTMID:" & AcctMgrTMID _
            & ", CustApprvEffDate:" & CustApprvEffDate _
            & ", CustApprvEndDate:" & CustApprvEndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREvent: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertAREvent = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub InsertAREventApproval(ByVal AREID As Integer, ByVal RoutingLevel As Integer, ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Integer, ByVal StatusID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@RoutingLevel", SqlDbType.Int)
            myCommand.Parameters("@RoutingLevel").Value = RoutingLevel

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", RoutingLevel:" & RoutingLevel _
            & ", TeamMemberID:" & TeamMemberID & ", SubscriptionID:" & SubscriptionID _
            & ", StatusID:" & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventApproval: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertAREventDetail(ByVal AREID As Integer, ByVal COMPNY As String, _
        ByVal Customer As String, ByVal PARTNO As String, ByVal CPART As String, _
        ByVal BARPT As String, ByVal PRCCDE As String, _
        ByVal PRCPRNT As Double, ByVal PRCDOLR As Double, ByVal USE_RELPRC As Double, _
        ByVal isFuture As Boolean, ByVal ESTPRC As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If COMPNY Is Nothing Then COMPNY = ""

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            If Customer Is Nothing Then Customer = ""

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            If PARTNO Is Nothing Then PARTNO = ""

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            If CPART Is Nothing Then CPART = ""

            myCommand.Parameters.Add("@CPART", SqlDbType.VarChar)
            myCommand.Parameters("@CPART").Value = CPART

            If BARPT Is Nothing Then BARPT = ""

            myCommand.Parameters.Add("@BARPT#", SqlDbType.VarChar)
            myCommand.Parameters("@BARPT#").Value = BARPT

            If PRCCDE Is Nothing Then PRCCDE = ""

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            myCommand.Parameters.Add("@PRCPRNT", SqlDbType.Decimal)
            myCommand.Parameters("@PRCPRNT").Value = PRCPRNT

            myCommand.Parameters.Add("@PRCDOLR", SqlDbType.Decimal)
            myCommand.Parameters("@PRCDOLR").Value = PRCDOLR

            myCommand.Parameters.Add("@USE_RELPRC", SqlDbType.Decimal)
            myCommand.Parameters("@USE_RELPRC").Value = USE_RELPRC

            myCommand.Parameters.Add("@isFuture", SqlDbType.Bit)
            myCommand.Parameters("@isFuture").Value = isFuture

            myCommand.Parameters.Add("@ESTPRC", SqlDbType.Decimal)
            myCommand.Parameters("@ESTPRC").Value = ESTPRC

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", COMPNY: " & COMPNY _
            & ", Customer: " & Customer _
            & ", PARTNO :" & PARTNO _
            & ", CPART: " & CPART _
            & ", BARPT: " & BARPT _
            & ", PRCCDE: " & PRCCDE _
            & ", PRCPRNT: " & PRCPRNT _
            & ", PRCDOLR :" & PRCDOLR _
            & ", USE_RELPRC :" & USE_RELPRC _
            & ", isFuture :" & isFuture _
            & ", ESTPRC :" & ESTPRC _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventDetail: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertAREventInvoicesOnHold(ByVal AREID As Integer, ByVal PARTNO As String, ByVal PRCCDE As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event_Invoices_On_Hold"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If PARTNO Is Nothing Then
                PARTNO = ""
            End If

            myCommand.Parameters.Add("@PARTNO", SqlDbType.VarChar)
            myCommand.Parameters("@PARTNO").Value = PARTNO

            If PRCCDE Is Nothing Then
                PRCCDE = ""
            End If

            myCommand.Parameters.Add("@PRCCDE", SqlDbType.VarChar)
            myCommand.Parameters("@PRCCDE").Value = PRCCDE

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", PARTNO :" & PARTNO _
            & ", PRCCDE: " & PRCCDE _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventInvoicesOnHold: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventInvoicesOnHold: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertAREventHistory(ByVal AREID As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

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
            Dim strUserEditedData As String = "AREID: " & AREID & ", ActionTakenTMID:" & ActionTakenTMID _
            & ", ActionDesc:" & ActionDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventHistory: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventHistory: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertAREventSupportingDocument(ByVal AREID As Integer, ByVal SupportingDocName As String, _
    ByVal SupportingDocDesc As String, ByVal SupportingDocBinary As Byte(), ByVal SupportingDocBinarySizeInBytes As Integer, _
    ByVal SupportingDocEncodeType As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Event_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = commonFunctions.convertSpecialChar(SupportingDocName, False)

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = commonFunctions.convertSpecialChar(SupportingDocDesc, False)

            myCommand.Parameters.Add("@SupportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@SupportingDocBinary").Value = SupportingDocBinary

            myCommand.Parameters.Add("@SupportingDocBinarySizeInBytes", SqlDbType.Int)
            myCommand.Parameters("@SupportingDocBinarySizeInBytes").Value = SupportingDocBinarySizeInBytes

            myCommand.Parameters.Add("@SupportingDocEncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocEncodeType").Value = commonFunctions.convertSpecialChar(SupportingDocEncodeType, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", SupportingDocName:" & SupportingDocName _
            & ", SupportingDocDesc:" & SupportingDocDesc _
            & ", SupportingDocBinarySizeInBytes:" & SupportingDocBinarySizeInBytes _
            & ", SupportingDocEncodeType:" & SupportingDocEncodeType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventSupportingDocument: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventSupportingDocument: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertARRSS(ByVal AREID As Integer, ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Integer, ByVal Comment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            If Comment Is Nothing Then
                Comment = ""
            End If

            myCommand.Parameters.Add("@Comment", SqlDbType.VarChar)
            myCommand.Parameters("@Comment").Value = commonFunctions.convertSpecialChar(Comment, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", TeamMemberID:" & TeamMemberID & ", SubscriptionID:" & SubscriptionID _
            & ", Comment:" & Comment & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARRSS: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARRSS: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertARRSSReply(ByVal AREID As Integer, ByVal RSSID As Integer, ByVal TeamMemberID As Integer, ByVal Comment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            If Comment Is Nothing Then
                Comment = ""
            End If

            myCommand.Parameters.Add("@Comment", SqlDbType.VarChar)
            myCommand.Parameters("@Comment").Value = commonFunctions.convertSpecialChar(Comment, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", RSSID: " & RSSID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", Comment:" & Comment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARRSSReply: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARRSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateAREventSales(ByVal AREID As Integer, ByVal EventDesc As String, _
            ByVal EventTypeID As Integer, ByVal AcctMgrTMID As Integer, _
            ByVal CustApprvEffDate As String, ByVal CustApprvEndDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Sales"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If EventDesc Is Nothing Then
                EventDesc = ""
            End If

            myCommand.Parameters.Add("@EventDesc", SqlDbType.VarChar)
            myCommand.Parameters("@EventDesc").Value = commonFunctions.convertSpecialChar(EventDesc, False)

            myCommand.Parameters.Add("@EventTypeID", SqlDbType.Int)
            myCommand.Parameters("@EventTypeID").Value = EventTypeID

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@CustApprvEffDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEffDate").Value = CustApprvEffDate

            myCommand.Parameters.Add("@CustApprvEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEndDate").Value = CustApprvEndDate

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", EventDesc:" & EventDesc _
            & ", EventTypeID:" & EventTypeID _
            & ", AcctMgrTMID:" & AcctMgrTMID _
            & ", CustApprvEffDate:" & CustApprvEffDate _
            & ", CustApprvEndDate:" & CustApprvEndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventSales: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventSales: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventCustomerApproved(ByVal AREID As Integer, ByVal isCustomerApproved As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Customer_Approved"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@isCustomerApproved", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerApproved").Value = isCustomerApproved

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", isCustomerApproved:" & isCustomerApproved _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventCustomerApproved: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventCustomerApproved: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventBilling(ByVal AREID As Integer, ByVal EventDesc As String, _
        ByVal AcctMgrTMID As Integer, ByVal CustApprvEndDate As String, ByVal CalculatedQuantityShipped As Double, _
        ByVal CalculatedDeductionAmount As Double, ByVal FinalDeductionAmount As Double, _
        ByVal DeductionReason As String, ByVal isPriceUpdatedByAccounting As Boolean, ByVal PriceChangeDate As String, _
        ByVal CreditDebitMemo As String, ByVal CreditDebitDate As String, ByVal BPCSInvoiceNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Billing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If EventDesc Is Nothing Then
                EventDesc = ""
            End If

            myCommand.Parameters.Add("@EventDesc", SqlDbType.VarChar)
            myCommand.Parameters("@EventDesc").Value = commonFunctions.convertSpecialChar(EventDesc, False)

            If CustApprvEndDate Is Nothing Then
                CustApprvEndDate = ""
            End If

            myCommand.Parameters.Add("@AcctMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AcctMgrTMID").Value = AcctMgrTMID

            myCommand.Parameters.Add("@CustApprvEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustApprvEndDate").Value = CustApprvEndDate

            myCommand.Parameters.Add("@CalculatedQuantityShipped", SqlDbType.Int)
            myCommand.Parameters("@CalculatedQuantityShipped").Value = CalculatedQuantityShipped

            myCommand.Parameters.Add("@CalculatedDeductionAmount", SqlDbType.Decimal)
            myCommand.Parameters("@CalculatedDeductionAmount").Value = CalculatedDeductionAmount

            myCommand.Parameters.Add("@FinalDeductionAmount", SqlDbType.Decimal)
            myCommand.Parameters("@FinalDeductionAmount").Value = FinalDeductionAmount

            If DeductionReason Is Nothing Then
                DeductionReason = ""
            End If

            myCommand.Parameters.Add("@DeductionReason", SqlDbType.VarChar)
            myCommand.Parameters("@DeductionReason").Value = commonFunctions.convertSpecialChar(DeductionReason, False)

            myCommand.Parameters.Add("@isPriceUpdatedByAccounting", SqlDbType.Bit)
            myCommand.Parameters("@isPriceUpdatedByAccounting").Value = isPriceUpdatedByAccounting

            If PriceChangeDate Is Nothing Then
                PriceChangeDate = ""
            End If

            myCommand.Parameters.Add("@PriceChangeDate", SqlDbType.VarChar)
            myCommand.Parameters("@PriceChangeDate").Value = commonFunctions.convertSpecialChar(PriceChangeDate, False)

            If CreditDebitMemo Is Nothing Then
                CreditDebitMemo = ""
            End If

            myCommand.Parameters.Add("@CreditDebitMemo", SqlDbType.VarChar)
            myCommand.Parameters("@CreditDebitMemo").Value = commonFunctions.convertSpecialChar(CreditDebitMemo, False)

            If CreditDebitDate Is Nothing Then
                CreditDebitDate = ""
            End If

            myCommand.Parameters.Add("@CreditDebitDate", SqlDbType.VarChar)
            myCommand.Parameters("@CreditDebitDate").Value = commonFunctions.convertSpecialChar(CreditDebitDate, False)

            If BPCSInvoiceNo Is Nothing Then
                BPCSInvoiceNo = ""
            End If

            myCommand.Parameters.Add("@BPCSInvoiceNo", SqlDbType.VarChar)
            myCommand.Parameters("@BPCSInvoiceNo").Value = commonFunctions.convertSpecialChar(BPCSInvoiceNo, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", EventDesc:" & EventDesc _
            & ", AcctMgrTMID:" & AcctMgrTMID _
            & ", CalculatedQuantityShipped:" & CalculatedQuantityShipped _
            & ", CalculatedDeductionAmount:" & CalculatedDeductionAmount _
            & ", FinalDeductionAmount:" & FinalDeductionAmount _
            & ", DeductionReason:" & DeductionReason _
            & ", isPriceUpdatedByAccounting:" & isPriceUpdatedByAccounting _
            & ", CreditDebitMemo:" & CreditDebitMemo _
            & ", CreditDebitDate:" & CreditDebitDate _
            & ", BPCSInvoiceNo:" & BPCSInvoiceNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventBilling: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventBilling: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventOverridePriceComment(ByVal AREID As Integer, ByVal OverrideCurrentPriceComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Override_Price_Comment"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If OverrideCurrentPriceComment Is Nothing Then
                OverrideCurrentPriceComment = ""
            End If

            myCommand.Parameters.Add("@OverrideCurrentPriceComment", SqlDbType.VarChar)
            myCommand.Parameters("@OverrideCurrentPriceComment").Value = commonFunctions.convertSpecialChar(OverrideCurrentPriceComment, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", OverrideCurrentPriceComment:" & OverrideCurrentPriceComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventOverridePriceComment: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventOverridePriceComment: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventStatus(ByVal AREID As Integer, ByVal EventStatusID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@EventStatusID", SqlDbType.Int)
            myCommand.Parameters("@EventStatusID").Value = EventStatusID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", EventStatusID:" & EventStatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventStatus: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventStatus: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventApprovalNotify(ByVal AREID As Integer, ByVal SubscriptionID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Approval_Notify"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", SubscriptionID:" & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventApprovalNotify: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventApprovalNotify: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventApprovalReset(ByVal AREID As Integer, ByVal TeamMemberID As Integer, ByVal EventTypeID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Approval_Reset"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@EventTypeID", SqlDbType.Int)
            myCommand.Parameters("@EventTypeID").Value = EventTypeID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", EventTypeID:" & EventTypeID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventApprovalReset: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventApprovalReset: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteAREvent(ByVal AREID As Integer, ByVal VoidReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Event"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            If VoidReason Is Nothing Then
                VoidReason = ""
            End If

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = VoidReason

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", VoidReason:" & VoidReason _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAREvent: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteAREventApprovalStatus(ByVal AREID As Integer, ByVal SubscriptionID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Event_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", SubscriptionID:" & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAREventApprovalStatus: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREventApprovalStatus: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetAccountManagerEmailAndBackUp(ByVal AcctMgrTMID As Integer) As String

        Dim strEmailAddress As String = ""

        Try
            Dim dsTeamMember As DataSet
            Dim dsBackup As DataSet

            If AcctMgrTMID > 0 Then
                dsTeamMember = SecurityModule.GetTeamMember(AcctMgrTMID, "", "", "", "", "", True, Nothing)

                'only get info from working account manager
                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                        If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                            strEmailAddress = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                            'get backup if out
                            dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(AcctMgrTMID, 9)
                            If commonFunctions.CheckDataSet(dsBackup) = True Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                    If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then

                                        'make sure backup team member is not already in either recipient list
                                        If InStr(strEmailAddress, strEmailAddress) <= 0 Then
                                            If strEmailAddress <> "" Then
                                                strEmailAddress += ";"
                                            End If

                                            strEmailAddress += dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If strEmailAddress = "" Then 'notify application group if there is a problem with account manager info
                UGNErrorTrapping.UpdateUGNErrorLog("AR Module: Failed getting team member Email to Account Manager: " & AcctMgrTMID, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AcctMgrTMID: " & AcctMgrTMID _
             & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAccountManagerEmailAndBackUp: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAccountManagerEmailAndBackUp: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

        Return strEmailAddress

    End Function
    Public Shared Function GetTeamMemberEmailAndBackUpBySubscriptionID(ByVal SubscriptionID As Integer, ByVal UGNFacility As String, ByVal isBackUpNeeded As Boolean) As String

        Dim strEmailAddress As String = ""

        Try
            Dim dsSubscription As DataSet
            Dim dsBackup As DataSet

            Dim bWorking As Boolean = False
            Dim iTMID As Integer = 0

            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNFacility)

            If commonFunctions.CheckDataSet(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("WorkStatus") IsNot System.DBNull.Value Then
                        bWorking = dsSubscription.Tables(0).Rows(0).Item("WorkStatus")

                        'only get working team members
                        If bWorking = True Then
                            If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 Then
                                iTMID = dsSubscription.Tables(0).Rows(0).Item("TMID")
                            End If

                            'do not allow duplicates
                            If InStr(strEmailAddress, dsSubscription.Tables(0).Rows(0).Item("Email").ToString) = 0 Then
                                strEmailAddress = dsSubscription.Tables(0).Rows(0).Item("Email").ToString
                            End If

                        End If

                        If isBackUpNeeded = True Then
                            'get backup if out
                            dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTMID, SubscriptionID)
                            If commonFunctions.CheckDataSet(dsBackup) = True Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                    If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then

                                        'make sure backup team member is not already in either recipient list
                                        If InStr(strEmailAddress, dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString) <= 0 Then
                                            If strEmailAddress <> "" Then
                                                strEmailAddress += ";"
                                            End If

                                            'do not allow duplicates
                                            If InStr(strEmailAddress, dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString) = 0 Then
                                                strEmailAddress += dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                            End If

                                        End If
                                    End If
                                End If
                            End If
                        End If 'end backup needed

                    End If
                End If
            End If

            If iTMID = 0 Then 'notify application group if subscription has not been assigned to working team member
                UGNErrorTrapping.UpdateUGNErrorLog("AR Module: Failed getting team member Email to subscriptionID: " & SubscriptionID & ", UGNFacility: " & UGNFacility, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubscriptionID: " & SubscriptionID _
            & ", UGNFacility: " & UGNFacility & ", isBackUpNeeded:" & isBackUpNeeded _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAccountManagerEmailAndBackUp: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAccountManagerEmailAndBackUp: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        Return strEmailAddress

    End Function
    Public Shared Sub DeleteARCookies()

        Try
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAREIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAREIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventDescSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventDescSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAccrualTypeIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAccrualTypeIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveFilterCustomerApproved").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveFilterCustomerApproved").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveIsCustomerApproved").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveIsCustomerApproved").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveInvoiceNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveInvoiceNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustomerSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePriceCodeSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePriceCodeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("ARGroupModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("ARGroupModule_SaveAnyCABBVSearch").Value = 0
            'HttpContext.Current.Response.Cookies("ARGroupModule_SaveAnyCABBVSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARGroupModule_SaveShowVoidSearch").Value = 0
            HttpContext.Current.Response.Cookies("ARGroupModule_SaveShowVoidSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteARCookies: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARCookies: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub UpdateAREventDetailPrice(ByVal AREID As Integer, ByVal PRCPRNT As Double, _
          ByVal PriceAdjustment As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Detail_Price"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@PRCPRNT", SqlDbType.Decimal)
            myCommand.Parameters("@PRCPRNT").Value = PRCPRNT

            myCommand.Parameters.Add("@PriceAdjustment", SqlDbType.Decimal)
            myCommand.Parameters("@PriceAdjustment").Value = PriceAdjustment

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", PRCPRNT:" & PRCPRNT _
            & ", PriceAdjustment:" & PriceAdjustment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventDetailPrice: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventDetailPrice: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyAREventDetail(ByVal NewAREID As Integer, ByVal OldAREID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_AR_Event_Detail"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewAREID", SqlDbType.Int)
            myCommand.Parameters("@NewAREID").Value = NewAREID

            myCommand.Parameters.Add("@OldAREID", SqlDbType.Int)
            myCommand.Parameters("@OldAREID").Value = OldAREID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewAREID: " & NewAREID _
            & ", OldAREID:" & OldAREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyAREventDetail: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyAREventAccrualOverrideCriteria(ByVal NewAREID As Integer, ByVal OldAREID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_AR_Event_Accrual_Override_Criteria"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewAREID", SqlDbType.Int)
            myCommand.Parameters("@NewAREID").Value = NewAREID

            myCommand.Parameters.Add("@OldAREID", SqlDbType.Int)
            myCommand.Parameters("@OldAREID").Value = OldAREID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewAREID: " & NewAREID _
            & ", OldAREID:" & OldAREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyAREventAccrualOverrideCriteria: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyAREventAccrualOverrideCriteria: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateAREventAccrual(ByVal AREID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Accrual"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 120

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventAccrual: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventAccrual: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventAccrualClose(ByVal AREID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Accrual_Close"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 120

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            'myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID            '& ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventAccrualClose: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventAccrualClose: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateAREventAccrualCurrentPrice(ByVal AREID As Integer, ByVal original_RowID As Integer, _
        ByVal Override_RELPRC As Double, ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Event_Accrual_Current_Price"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@AREID", SqlDbType.Int)
            myCommand.Parameters("@AREID").Value = AREID

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@Override_RELPRC", SqlDbType.Decimal)
            myCommand.Parameters("@Override_RELPRC").Value = Override_RELPRC

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", RowID: " & RowID _
            & ", Override_RELPRC: " & Override_RELPRC _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventAccrualCurrentPrice: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventAccrualCurrentPrice: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
#End Region

#Region "AR OPERATIONS DEDUCTION"
    Public Shared Sub CleanARDeductionCrystalReports()
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
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CleanARDeductionCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanARDeductionCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanARDeductionCrystalReports

    Public Shared Function GetARDeductionReason(ByVal ReasonDesc As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_Reason"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ReasonDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ReasonDesc").Value = commonFunctions.convertSpecialChar(ReasonDesc, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductRsn")
            GetARDeductionReason = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionReason : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionReason = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionReason

    Public Shared Function GetARDeduction(ByVal ARDID As String, ByVal ReferenceNo As String, ByVal SubmittedByTMID As Integer, ByVal Comments As String, ByVal UGNFacility As String, ByVal Customer As String, ByVal DateSubFrom As String, ByVal DateSubTo As String, ByVal RecStatus As String, ByVal Reason As Integer, ByVal ClosedDateFrom As String, ByVal ClosedDateTo As String, ByVal SortBy As String, ByVal PartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.VarChar)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@ReferenceNo", SqlDbType.VarChar)
            myCommand.Parameters("@ReferenceNo").Value = ReferenceNo

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@DateSubFrom", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubFrom").Value = DateSubFrom

            myCommand.Parameters.Add("@DateSubTo", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubTo").Value = DateSubTo

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@ClosedDateFrom", SqlDbType.VarChar)
            myCommand.Parameters("@ClosedDateFrom").Value = ClosedDateFrom

            myCommand.Parameters.Add("@ClosedDateTo", SqlDbType.VarChar)
            myCommand.Parameters("@ClosedDateTo").Value = ClosedDateTo

            myCommand.Parameters.Add("@SortBy", SqlDbType.VarChar)
            myCommand.Parameters("@SortBy").Value = SortBy

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeduct")
            GetARDeduction = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeduction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeduction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeduction

    Public Shared Function GetLastARDeductionRecNo(ByVal SubmittedByTMID As Integer, ByVal Reason As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal RecStatus As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_AR_Deduction_RecNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductRecNo")
            GetLastARDeductionRecNo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetLastARDeductionRecNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLastARDeductionRecNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastARDeductionRecNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetLastARDeductionRecNo

    Public Shared Function GetARDeductionLead(ByVal ARDID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductLead")
            GetARDeductionLead = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionLead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionLead : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionLead = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionLead

    Public Shared Function GetARDeductionApproval(ByVal ARDID As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.VarChar)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductAprvl")
            GetARDeductionApproval = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionApproval

    Public Shared Function GetARDeductionDocuments(ByVal ARDID As Integer, ByVal DocID As Integer, ByVal MaxDateOfUpload As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myCommand.Parameters.Add("@MaxDateOfUpload", SqlDbType.Bit)
            myCommand.Parameters("@MaxDateOfUpload").Value = MaxDateOfUpload

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductDoc")
            GetARDeductionDocuments = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionDocuments = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionDocuments

    Public Shared Function GetARDeductionHistory(ByVal ARDID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductHistory")
            GetARDeductionHistory = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionHistory = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionHistory

    Public Shared Function GetARDeductionRSS(ByVal ARDID As Integer, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductRSS")
            GetARDeductionRSS = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionRSS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionRSS = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionRSS

    Public Shared Function GetARDeductionRSSReply(ByVal ARDID As Integer, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ARDeductRSSReply")
            GetARDeductionRSSReply = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionRSSReply = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionRSSReply

    Public Shared Function GetARDeductionCntrMsr(ByVal ARDID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_AR_Deduction_CntrMsr"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CntrMsr")
            GetARDeductionCntrMsr = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARDeductionCntrMsr : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetARDeductionCntrMsr : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetARDeductionCntrMsr = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetARDeductionCntrMsr

    Public Shared Sub InsertARDeduction(ByVal SubmittedByTMID As Integer, ByVal UGNFacility As String, ByVal DeductionAmount As Decimal, ByVal Customer As String, ByVal ReferenceNo As String, ByVal IncidentDate As String, ByVal Reason As Integer, ByVal Comments As String, ByVal RecStatus As String, ByVal PartNo As String, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeductionAmount", SqlDbType.Decimal)
            myCommand.Parameters("@DeductionAmount").Value = DeductionAmount

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ReferenceNo", SqlDbType.VarChar)
            myCommand.Parameters("@ReferenceNo").Value = ReferenceNo

            myCommand.Parameters.Add("@IncidentDate", SqlDbType.VarChar)
            myCommand.Parameters("@IncidentDate").Value = IncidentDate

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubmittedByTMID: " & SubmittedByTMID & ", UGNFacility: " & UGNFacility _
            & ", DeductionAmount: " & DeductionAmount _
            & ", Customer: " & Customer & ", ReferenceNo: " & ReferenceNo _
            & ", IncidentDate: " & IncidentDate & ", Reason: " & Reason _
            & ", Comments: " & Comments & ", RecStatus: " & RecStatus _
            & ", PartNo: " & PartNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeduction : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertARDeduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeduction

    Public Shared Sub InsertARDeductionApproval(ByVal ARDID As Integer, ByVal UGNFacility As String, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionApproval

    Public Shared Sub InsertARDeductionDocuments(ByVal ARDID As Integer, ByVal TeamMemberID As Integer, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

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

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", TeamMemberID: " & TeamMemberID _
            & ", Description: " & Description & ", FileName: " & FileName & ", EncodeType: " & EncodeType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionDocuments : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionDocuments

    Public Shared Sub InsertARDeductionHistory(ByVal ARDID As Integer, ByVal Reason As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String, ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.convertSpecialChar(ActionDesc, False)

            myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            myCommand.Parameters("@FieldChange").Value = commonFunctions.convertSpecialChar(FieldChange, False)

            myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            myCommand.Parameters("@PreviousValue").Value = commonFunctions.convertSpecialChar(PreviousValue, False)

            myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            myCommand.Parameters("@NewValue").Value = commonFunctions.convertSpecialChar(NewValue, False)

            myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeReason").Value = commonFunctions.convertSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", Reason: " & Reason & ", ActionTakenTMID: " & ActionTakenTMID _
            & ", ActionDesc: " & ActionDesc & ", FieldChange: " & FieldChange _
            & ", PreviousValue: " & PreviousValue & ", NewValue: " & NewValue _
            & ", ChangeReason: " & ChangeReason _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionHistory : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionHistory

    Public Shared Sub InsertARDeductionRSS(ByVal ARDID As Integer, ByVal Reason As Integer, ByVal TeamMemberID As Integer, ByVal Comments As String, ByVal ApprovalLevel As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@Reason", SqlDbType.VarChar)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", Reason: " & Reason & ", TeamMemberID: " & TeamMemberID _
            & ", Comments: " & Comments & ", ApprovalLevel: " & ApprovalLevel _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionRSS : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionRSS

    Public Shared Sub InsertARDeductionRSSReply(ByVal ARDID As Integer, ByVal RSSID As Integer, ByVal Reason As Integer, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", Reason: " & Reason & ", TeamMemberID: " & TeamMemberID _
            & ", Comments: " & Comments & ", RSSID: " & RSSID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionRSSReply : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionRSSReply

    Public Shared Sub InsertARDeductionCntrMsr(ByVal ARDID As Integer, ByVal TeamMemberID As Integer, ByVal CounterMeasure As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_AR_Deduction_CntrMsr"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@CounterMeasure", SqlDbType.VarChar)
            myCommand.Parameters("@CounterMeasure").Value = commonFunctions.replaceSpecialChar(CounterMeasure, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", TeamMemberID:" & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertARDeductionCntrMsr: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertARDeductionCntrMsr: " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertARDeductionCntrMsr

    Public Shared Sub UpdateARDeduction(ByVal ARDID As Integer, ByVal SubmittedByTMID As Integer, ByVal UGNFacility As String, ByVal DeductionAmount As Decimal, ByVal Customer As String, ByVal ReferenceNo As String, ByVal IncidentDate As String, ByVal Reason As Integer, ByVal Comments As String, ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal DateSubmitted As String, ByVal VoidReason As String, ByVal CreditDebitDate As String, ByVal CreditDebitMemo As String, ByVal PartNo As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Deduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@SubmittedByTMID", SqlDbType.Int)
            myCommand.Parameters("@SubmittedByTMID").Value = SubmittedByTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeductionAmount", SqlDbType.Decimal)
            myCommand.Parameters("@DeductionAmount").Value = DeductionAmount

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ReferenceNo", SqlDbType.VarChar)
            myCommand.Parameters("@ReferenceNo").Value = ReferenceNo

            myCommand.Parameters.Add("@IncidentDate", SqlDbType.VarChar)
            myCommand.Parameters("@IncidentDate").Value = IncidentDate

            myCommand.Parameters.Add("@Reason", SqlDbType.Int)
            myCommand.Parameters("@Reason").Value = Reason

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@DateSubmitted", SqlDbType.VarChar)
            myCommand.Parameters("@DateSubmitted").Value = DateSubmitted

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.convertSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@CreditDebitDate", SqlDbType.VarChar)
            myCommand.Parameters("@CreditDebitDate").Value = CreditDebitDate

            myCommand.Parameters.Add("@CreditDebitMemo", SqlDbType.VarChar)
            myCommand.Parameters("@CreditDebitMemo").Value = CreditDebitMemo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", DeductionAmount: " & DeductionAmount _
            & ", SubmittedByTMID: " & SubmittedByTMID & ", UGNFacility: " & UGNFacility _
            & ", Customer: " & Customer & ", ReferenceNo: " & ReferenceNo _
            & ", IncidentDate: " & IncidentDate & ", Reason: " & Reason _
            & ", Comments: " & Comments & ", RecStatus: " & RecStatus _
            & ", DateSubmitted: " & DateSubmitted & ", VoidReason: " & VoidReason _
            & ", CreditDebitDate: " & CreditDebitDate & ", CreditDebitMemo: " & CreditDebitMemo _
            & ", PartNo: " & PartNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeduction : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateARDeduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF UpdateARDeduction

    Public Shared Sub UpdateARDeductionApproval(ByVal ARDID As Integer, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal SameTMID As Boolean, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Deduction_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

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
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", TMID: " & TMID _
            & ", TMSigned: " & TMSigned & ", Status: " & Status _
            & ", Comments: " & Comments & ", SeqNo: " & SeqNo _
            & ", SameTMID: " & SameTMID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeductionApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF UpdateARDeductionApproval

    Public Shared Sub UpdateARDeductionStatus(ByVal ARDID As Integer, ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal IncidentDate As String, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Deduction_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@IncidentDate", SqlDbType.VarChar)
            myCommand.Parameters("@IncidentDate").Value = IncidentDate

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID & ", RecStatus: " & RecStatus _
            & ", RoutingStatus: " & RoutingStatus _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeductionStatus : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateARDeductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF UpdateARDeductionStatus

    Public Shared Sub UpdateARDeductionCntrMsr(ByVal ARDID As Integer, ByVal Resolution As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_AR_Deduction_CntrMsr"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@Resolution", SqlDbType.VarChar)
            myCommand.Parameters("@Resolution").Value = commonFunctions.replaceSpecialChar(Resolution, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", Resolution: " & Resolution _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateARDeductionCntrMsr : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateARDeductionCntrMsr : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF UpdateARDeductionCntrMsr

    Public Shared Sub DeleteARDeduction(ByVal ARDID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Deduction"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeduction : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARDeduction : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF DeleteARDeduction

    Public Shared Sub DeleteARDeductionApproval(ByVal ARDID As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Deduction_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID").Value = ResponsibleTMID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeductionApproval : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARDeductionApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF DeleteARDeductionApproval

    Public Shared Sub DeleteARDeductionDocuments(ByVal DocID As Integer, ByVal ARDID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_AR_Deduction_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myCommand.Parameters.Add("@ARDID", SqlDbType.Int)
            myCommand.Parameters("@ARDID").Value = ARDID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ARDID: " & ARDID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeductionDocuments : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARDeductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF DeleteARDeductionDocuments

    Public Shared Sub DeleteARDeductionCookies()

        Try
            HttpContext.Current.Response.Cookies("AR_ARDID").Value = ""
            HttpContext.Current.Response.Cookies("AR_ARDID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DREFNO").Value = ""
            HttpContext.Current.Response.Cookies("AR_DREFNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_SBTMID").Value = ""
            HttpContext.Current.Response.Cookies("AR_SBTMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DCOM").Value = ""
            HttpContext.Current.Response.Cookies("AR_DCOM").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DUFAC").Value = ""
            HttpContext.Current.Response.Cookies("AR_DUFAC").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("AR_DSOLDTO").Value = ""
            'HttpContext.Current.Response.Cookies("AR_DSOLDTO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DCUST").Value = ""
            HttpContext.Current.Response.Cookies("AR_DCUST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DSF").Value = ""
            HttpContext.Current.Response.Cookies("AR_DSF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DST").Value = ""
            HttpContext.Current.Response.Cookies("AR_DST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DRSTS").Value = ""
            HttpContext.Current.Response.Cookies("AR_DRSTS").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_DRSN").Value = ""
            HttpContext.Current.Response.Cookies("AR_DRSN").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_CDF").Value = ""
            HttpContext.Current.Response.Cookies("AR_CDF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_CDT").Value = ""
            HttpContext.Current.Response.Cookies("AR_CDT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_SB").Value = ""
            HttpContext.Current.Response.Cookies("AR_SB").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("AR_PNO").Value = ""
            HttpContext.Current.Response.Cookies("AR_PNO").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeductionCookies : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARDeductionCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteARDeductionCookies

    Public Shared Sub DeleteARDeductionReportCookies()

        Try
            HttpContext.Current.Response.Cookies("ARR_ARDID").Value = ""
            HttpContext.Current.Response.Cookies("ARR_ARDID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DREFNO").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DREFNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_SBTMID").Value = ""
            HttpContext.Current.Response.Cookies("ARR_SBTMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DCOM").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DCOM").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DUFAC").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DUFAC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DCUST").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DCUST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DSF").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DSF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DST").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DRSTS").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DRSTS").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_DRSN").Value = ""
            HttpContext.Current.Response.Cookies("ARR_DRSN").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_CDF").Value = ""
            HttpContext.Current.Response.Cookies("ARR_CDF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_CDT").Value = ""
            HttpContext.Current.Response.Cookies("ARR_CDT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_SB").Value = ""
            HttpContext.Current.Response.Cookies("ARR_SB").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ARR_PNO").Value = ""
            HttpContext.Current.Response.Cookies("ARR_PNO").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteARDeductionCookies : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARGroupModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteARDeductionCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARGroupModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteARDeductionReportCookies
#End Region 'EOF AR OPERATIONS DEDUCTION

End Class
