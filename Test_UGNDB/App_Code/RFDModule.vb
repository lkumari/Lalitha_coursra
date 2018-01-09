
''************************************************************************************************
''Name:		RFDModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the RFD Module (and sometimes the Costing Module)
''
''Date		    Author	    
''01/21/2009    Roderick Carlson	Created  
''03/01/2011    Roderick Carlson    - allow uploaded file types of several kinds
''04/28/2011    Roderick Carlson    - added isMaterialSizeChange, isContinuousLine, and isCapitalRequired
''07/07/2011    Roderick Carlson    - on getRFDSearch, remove special char
''09/28/2011    Roderick Carlson    - add isECIRequired to UpdateRFD
''12/01/2011    Roderick Carlson    - added copyreason
''04/12/2012    Roderick Carlson    - added UpdateRFDClose Function
''04/24/2012    Roderick Carlson    - added Program Manager ID to get, insert, update, search functions
''05/07/2012    Roderick Carlson    - added Purchasing for External RFQ - trying new way to replace single quotes in insert and update RFD functions
''05/15/2012    Roderick Carlson    - added isMeetingRequired to insert update RFD
''05/16/2012    Roderick Carlson    - added Capital and Tooling Lead time and units, number of cavities, supporting doc rules and fields
''05/25/2012    Roderick Carlson    - added Child Part Lead time and units
''06/12/2012    Roderick Carlson    - added isExternalRFQrequired field to child part stored procedures
''10/09/2012    Roderick Carlson    - removed updatedby on some delete functions
''01/22/2014    LRey                - Replaced "BPCSPart" with "PART" and SoldTo|CABBV with Customer wherever used.
''05/07/2014    LRey                - Added isCostReduction to InsertRFD and UpdateRFD
''************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.VisualBasic

Public Class RFDModule
    Public Shared Sub CleanRFDCrystalReports()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("RFDPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("RFDPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("RFDPreview") = Nothing
                HttpContext.Current.Session("RFDPreviewRFDNo") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanRFDCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanRFDCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeleteRFDCookies()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            HttpContext.Current.Response.Cookies("RFDModule_SaveRFDNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveRFDNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveRFDDescSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveRFDDescSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveApproverStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveApproverStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveDrawingNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SavePrioritySearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SavePrioritySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveCustomerPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveCustomerPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveDesignLevelSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveDesignLevelSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SavePartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SavePartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveInitiatorIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveInitiatorIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveApproverIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveApproverIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveAccountManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveProgramManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveProgramManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveBusinessProcessActionIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveBusinessProcessTypeIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveDesignationTypeSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveCustomerSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveProgramIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveCommodityIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveProductTechnologyIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveProductTechnologyIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveSubFamilyIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveSubFamilyIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveUGNDBVendorIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SavePurchasedGoodIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SavePurchasedGoodIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveCostSheetIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveCostSheetIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveECINoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveECINoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveCapExProjectNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveCapExProjectNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SavePurchasingPONoSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SavePurchasingPONoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveDueDateStartSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveDueDateStartSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveDueDateEndSearch").Value = ""
            HttpContext.Current.Response.Cookies("RFDModule_SaveDueDateEndSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveSubscriptionIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveSubscriptionIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveFilterBusinessAwarded").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveFilterBusinessAwarded").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveIsBusinessAwarded").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveIsBusinessAwarded").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("RFDModule_SaveIncludeArchiveSearch").Value = 0
            HttpContext.Current.Response.Cookies("RFDModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteRFDCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeleteRFDBusinessAwarded(ByVal RFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_RFD_Business_Awarded"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            'myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "BusinessAwarded : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("BusinessAwarded : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteRFD(ByVal RFDNo As Integer, ByVal VoidComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@VoidComment", SqlDbType.VarChar)
            myCommand.Parameters("@VoidComment").Value = commonFunctions.convertSpecialChar(VoidComment, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", VoidComment: " & VoidComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDClose(ByVal RFDNo As Integer, ByVal CloseComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Close"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CloseComment", SqlDbType.VarChar)
            myCommand.Parameters("@CloseComment").Value = commonFunctions.convertSpecialChar(CloseComment, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", CloseComment: " & CloseComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDClose : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDClose : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetRFD(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDDetail")
            GetRFD = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFD.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFD = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetRFDChildPart(ByVal RowID As Integer, ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDChildPart")
            GetRFDChildPart = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDChildPart = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDInitiatorList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Initiator_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDInitiatorList")
            GetRFDInitiatorList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDInitiatorList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDInitiatorList : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDInitiatorList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDApproval(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, ByVal TeamMemberID As Integer, _
        ByVal filterNotified As Boolean, ByVal isNotified As Boolean, ByVal isHistorical As Boolean, ByVal filterWorking As Boolean, _
        ByVal isWorking As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@filterNotified", SqlDbType.Bit)
            myCommand.Parameters("@filterNotified").Value = filterNotified

            myCommand.Parameters.Add("@isNotified", SqlDbType.Bit)
            myCommand.Parameters("@isNotified").Value = isNotified

            myCommand.Parameters.Add("@isHistorical", SqlDbType.Bit)
            myCommand.Parameters("@isHistorical").Value = isHistorical

            myCommand.Parameters.Add("@filterWorking", SqlDbType.Bit)
            myCommand.Parameters("@filterWorking").Value = filterWorking

            myCommand.Parameters.Add("@isWorking", SqlDbType.Bit)
            myCommand.Parameters("@isWorking").Value = isWorking

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDApproval")
            GetRFDApproval = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDProcess(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Process"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDProcess")
            GetRFDProcess = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDProcess = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDCapital(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDCapital")
            GetRFDCapital = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDCapital = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetRFDTooling(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDTooling")
            GetRFDTooling = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDTooling : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDTooling : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDTooling = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDSubscriptionByApprover(ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Subscription_By_Approver"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDSubscriptionByApprover")
            GetRFDSubscriptionByApprover = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDSubscriptionByApprover : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDSubscriptionByApprover : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDSubscriptionByApprover = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDApproverList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Approver_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDApproverList")
            GetRFDApproverList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDApproverList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDApproverList : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDApproverList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDSearch(ByVal RFDNo As String, ByVal RFDDesc As String, _
        ByVal StatusID As Integer, ByVal ApproverStatusID As Integer, _
        ByVal DrawingNo As String, ByVal PriorityID As Integer, ByVal CustomerPartNo As String, _
        ByVal DesignLevel As String, ByVal PartNo As String, _
        ByVal PartName As String, ByVal InitiatorID As Integer, _
        ByVal ApproverID As Integer, ByVal AccountManagerID As Integer, ByVal ProgramManagerID As Integer, _
        ByVal BusinessProcessActionID As Integer, ByVal BusinessProcessTypeID As Integer, _
        ByVal DesignationTypeID As String, ByVal Customer As String, _
        ByVal UGNFacility As String, ByVal ProgramID As Integer, ByVal CommodityID As Integer, _
        ByVal ProductTechnologyID As Integer, ByVal SubFamilyID As Integer, ByVal UGNDBVendorID As Integer, _
        ByVal PurchasedGoodID As Integer, ByVal CostSheetID As String, ByVal ECINo As String, ByVal CapExProjectNo As String, _
        ByVal PurchasingPONo As String, ByVal DueDateStart As String, _
        ByVal DueDateEnd As String, ByVal SubscriptionID As Integer, ByVal FilterBusinessAwarded As Boolean, _
        ByVal IsBusinessAwarded As Boolean, ByVal IncludeArchive As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.VarChar)
            myCommand.Parameters("@RFDNo").Value = commonFunctions.convertSpecialChar(If(RFDNo Is Nothing, "", RFDNo), False)

            myCommand.Parameters.Add("@RFDDesc", SqlDbType.VarChar)
            myCommand.Parameters("@RFDDesc").Value = commonFunctions.convertSpecialChar(RFDDesc, False)

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ApproverStatusID", SqlDbType.Int)
            myCommand.Parameters("@ApproverStatusID").Value = ApproverStatusID

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(If(DrawingNo Is Nothing, "", DrawingNo), False)

            myCommand.Parameters.Add("@PriorityID", SqlDbType.Int)
            myCommand.Parameters("@PriorityID").Value = PriorityID

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = commonFunctions.convertSpecialChar(If(CustomerPartNo Is Nothing, "", CustomerPartNo), False)

            myCommand.Parameters.Add("@DesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@DesignLevel").Value = commonFunctions.convertSpecialChar(If(DesignLevel Is Nothing, "", DesignLevel), False)

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = commonFunctions.convertSpecialChar(If(PartNo Is Nothing, "", PartNo), False)

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = commonFunctions.convertSpecialChar(If(PartName Is Nothing, "", PartName), False)

            myCommand.Parameters.Add("@InitiatorID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorID").Value = InitiatorID

            myCommand.Parameters.Add("@ApproverID", SqlDbType.Int)
            myCommand.Parameters("@ApproverID").Value = ApproverID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            myCommand.Parameters.Add("@BusinessProcessActionID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessActionID").Value = BusinessProcessActionID

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = If(DesignationTypeID Is Nothing, "", DesignationTypeID)

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = If(UGNFacility Is Nothing, "", UGNFacility)

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@UGNDBVendorID", SqlDbType.Int)
            myCommand.Parameters("@UGNDBVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = commonFunctions.convertSpecialChar(If(CostSheetID Is Nothing, "", CostSheetID), False)

            myCommand.Parameters.Add("@ECINo", SqlDbType.VarChar)
            myCommand.Parameters("@ECINo").Value = commonFunctions.convertSpecialChar(If(ECINo Is Nothing, "", ECINo), False)

            myCommand.Parameters.Add("@CapExProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjectNo").Value = commonFunctions.convertSpecialChar(If(CapExProjectNo Is Nothing, "", CapExProjectNo), False)

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = commonFunctions.convertSpecialChar(If(PurchasingPONo Is Nothing, "", PurchasingPONo), False)

            myCommand.Parameters.Add("@DueDateStart", SqlDbType.VarChar)
            myCommand.Parameters("@DueDateStart").Value = If(DueDateStart Is Nothing, "", DueDateStart)

            myCommand.Parameters.Add("@DueDateEnd", SqlDbType.VarChar)
            myCommand.Parameters("@DueDateEnd").Value = If(DueDateEnd Is Nothing, "", DueDateEnd)

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@FilterBusinessAwarded", SqlDbType.Bit)
            myCommand.Parameters("@FilterBusinessAwarded").Value = FilterBusinessAwarded

            myCommand.Parameters.Add("@IsBusinessAwarded", SqlDbType.Bit)
            myCommand.Parameters("@IsBusinessAwarded").Value = IsBusinessAwarded

            myCommand.Parameters.Add("@IncludeArchive", SqlDbType.Bit)
            myCommand.Parameters("@IncludeArchive").Value = IncludeArchive

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDList")
            GetRFDSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", RFDDesc: " & RFDDesc _
            & ", StatusID: " & StatusID _
            & ", ApproverStatusID: " & ApproverStatusID _
            & ", DrawingNo: " & DrawingNo _
            & ", PriorityID: " & PriorityID _
            & ", CustomerPartNo: " & CustomerPartNo _
            & ", DesignLevel: " & DesignLevel _
            & ", PartNo: " & PartNo _
            & ", PartName: " & PartName _
            & ", InitiatorID: " & InitiatorID _
            & ", ApproverID: " & ApproverID _
            & ", AccountManagerID: " & AccountManagerID _
            & ", ProgramManagerID: " & ProgramManagerID _
            & ", BusinessProcessActionID: " & BusinessProcessActionID _
            & ", BusinessProcessTypeID: " & BusinessProcessTypeID _
            & ", DesignationTypeID: " & DesignationTypeID _
            & ", Customer: " & Customer _
            & ", UGNFacility: " & UGNFacility _
            & ", ProgramID: " & ProgramID _
            & ", CommodityID: " & CommodityID _
            & ", ProductTechnologyID: " & ProductTechnologyID _
            & ", SubFamilyID: " & SubFamilyID _
            & ", UGNDBVendorID: " & UGNDBVendorID _
            & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", CostSheetID: " & CostSheetID _
            & ", ECINo: " & ECINo _
            & ", CapExProjectNo: " & CapExProjectNo _
            & ", PurchasingPONo: " & PurchasingPONo _
            & ", DueDateStart: " & DueDateStart _
            & ", DueDateEnd: " & DueDateEnd _
            & ", SubscriptionID: " & SubscriptionID _
            & ", FilterBusinessAwarded: " & FilterBusinessAwarded _
            & ", IsFilterBusinessAwarded: " & IsBusinessAwarded _
            & ", IncludeArchive: " & IncludeArchive _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDSearch : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDCostingSearch(ByVal RFDNo As String, ByVal RFDDesc As String, _
      ByVal StatusID As Integer, ByVal ApproverStatusID As Integer, _
      ByVal DrawingNo As String, ByVal CustomerPartNo As String, _
      ByVal BPCSPartNo As String, _
      ByVal PartName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Costing_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If RFDNo Is Nothing Then
                RFDNo = ""
            End If

            myCommand.Parameters.Add("@RFDNo", SqlDbType.VarChar)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@RFDDesc", SqlDbType.VarChar)
            myCommand.Parameters("@RFDDesc").Value = RFDDesc

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ApproverStatusID", SqlDbType.Int)
            myCommand.Parameters("@ApproverStatusID").Value = ApproverStatusID

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = CustomerPartNo

            If BPCSPartNo Is Nothing Then
                BPCSPartNo = ""
            End If

            myCommand.Parameters.Add("@BPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@BPCSPartNo").Value = BPCSPartNo

            If PartName Is Nothing Then
                PartName = ""
            End If

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = PartName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDCostingList")
            GetRFDCostingSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", RFDDesc: " & RFDDesc _
            & ", StatusID: " & StatusID _
            & ", ApproverStatusID: " & ApproverStatusID _
            & ", DrawingNo: " & DrawingNo _
            & ", CustomerPartNo: " & CustomerPartNo _
            & ", BPCSPartNo: " & BPCSPartNo _
            & ", PartName: " & PartName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDCostingSearch : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDCostingSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDCostingSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetRFDHistory(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetRFDHistory")
            GetRFDHistory = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDStatus(ByVal StatusID As Integer, ByVal filterEditable As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Status_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@filterEditable", SqlDbType.Bit)
            myCommand.Parameters("@filterEditable").Value = filterEditable

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDStatusList")
            GetRFDStatus = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDPriority(ByVal PriorityID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Priority"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PriorityID", SqlDbType.Int)
            myCommand.Parameters("@PriorityID").Value = PriorityID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDPriority")
            GetRFDPriority = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PriorityID: " & PriorityID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRFDPriority : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDPriority : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDPriority = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateRFD(ByVal RFDNo As Integer, ByVal RFDDesc As String, _
        ByVal BusinessProcessActionID As Integer, ByVal BusinessProcessTypeID As Integer, ByVal DesignationType As String, _
        ByVal PriceCode As String, ByVal PriorityID As Integer, ByVal DueDate As String, _
        ByVal InitiatorTeamMemberID As Integer, ByVal AccountManagerID As Integer, ByVal ProgramManagerID As Integer, _
        ByVal ImpactOnUGN As String, ByVal TargetPrice As Double, _
        ByVal TargetAnnualVolume As Integer, ByVal TargetAnnualSales As Double, _
        ByVal CurrentCustomerPartNo As String, ByVal NewCustomerPartNo As String, ByVal CurrentCustomerDrawingNo As String, _
        ByVal NewCustomerDrawingNo As String, ByVal CurrentCustomerPartName As String, ByVal NewCustomerPartName As String, _
        ByVal CurrentDesignLevel As String, ByVal NewDesignLevel As String, _
        ByVal CurrentDrawingNo As String, ByVal NewDrawingNo As String, _
        ByVal NewInStepTracking As Integer, ByVal NewAMDValue As Double, _
        ByVal NewAMDUnits As String, ByVal NewAMDTolerance As String, ByVal NewWMDValue As Double, _
        ByVal NewWMDUnits As String, ByVal NewWMDTolerance As String, _
        ByVal NewConstruction As String, ByVal NewDensityValue As Double, _
        ByVal NewDensityUnits As String, ByVal NewDensityTolerance As String, _
        ByVal NewDrawingNotes As String, ByVal NewCommodityID As Integer, _
        ByVal NewProductTechnologyID As Integer, ByVal NewSubFamilyID As Integer, _
        ByVal FamilyID As Integer, ByVal Make As String, _
        ByVal CostSheetID As Integer, ByVal ECINo As Integer, _
        ByVal isECIRequired As Boolean, ByVal CapExProjectNo As String, _
        ByVal PurchasingPONo As String, ByVal isAffectsCostSheetOnly As Boolean, _
        ByVal isCostingRequired As Boolean, ByVal isCustomerApprovalRequired As Boolean, _
        ByVal isDVPRrequired As Boolean, ByVal isPackagingRequired As Boolean, ByVal isPlantControllerRequired As Boolean, _
        ByVal isProcessRequired As Boolean, ByVal isProductDevelopmentRequired As Boolean, _
        ByVal isPurchasingExternalRFQRequired As Boolean, ByVal isPurchasingRequired As Boolean, _
        ByVal isQualityEngineeringRequired As Boolean, ByVal isRDRequired As Boolean, ByVal isToolingRequired As Boolean, _
        ByVal ProductDevelopmentCommodityTeamMemberID As Integer, ByVal PurchasingFamilyTeamMemberID As Integer, _
        ByVal PurchasingMakeTeamMemberID As Integer, ByVal isPPAP As Boolean, ByVal VendorRequirement As String, _
        ByVal isMaterialSizeChange As Boolean, _
        ByVal isContinuousLine As Boolean, ByVal isCapitalRequired As Boolean, _
        ByVal CopyReason As String, ByVal isMeetingRequired As Boolean, ByVal isCostReduction As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If RFDDesc Is Nothing Then
                RFDDesc = ""
            End If

            myCommand.Parameters.Add("@RFDDesc", SqlDbType.VarChar)
            myCommand.Parameters("@RFDDesc").Value = Replace(RFDDesc, "'", "`") 'commonFunctions.convertSpecialChar(RFDDesc, False)

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            myCommand.Parameters.Add("@BusinessProcessActionID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessActionID").Value = BusinessProcessActionID

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            If PriceCode Is Nothing Then
                PriceCode = ""
            End If

            myCommand.Parameters.Add("@PriceCode", SqlDbType.VarChar)
            myCommand.Parameters("@PriceCode").Value = PriceCode

            myCommand.Parameters.Add("@PriorityID", SqlDbType.Int)
            myCommand.Parameters("@PriorityID").Value = PriorityID

            If DueDate Is Nothing Then
                DueDate = ""
            End If

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            myCommand.Parameters.Add("@InitiatorTeamMemberID ", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID ").Value = InitiatorTeamMemberID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            If ImpactOnUGN Is Nothing Then
                ImpactOnUGN = ""
            End If

            myCommand.Parameters.Add("@ImpactOnUGN", SqlDbType.VarChar)
            myCommand.Parameters("@ImpactOnUGN").Value = Replace(ImpactOnUGN, "'", "`") 'commonFunctions.convertSpecialChar(ImpactOnUGN, False)

            myCommand.Parameters.Add("@TargetPrice", SqlDbType.Decimal)
            myCommand.Parameters("@TargetPrice").Value = TargetPrice

            myCommand.Parameters.Add("@TargetAnnualVolume", SqlDbType.Int)
            myCommand.Parameters("@TargetAnnualVolume").Value = TargetAnnualVolume

            myCommand.Parameters.Add("@TargetAnnualSales", SqlDbType.Decimal)
            myCommand.Parameters("@TargetAnnualSales").Value = TargetAnnualSales

            If CurrentCustomerPartNo Is Nothing Then
                CurrentCustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CurrentCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentCustomerPartNo").Value = commonFunctions.convertSpecialChar(CurrentCustomerPartNo, False)

            If NewCustomerPartNo Is Nothing Then
                NewCustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@NewCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewCustomerPartNo").Value = commonFunctions.convertSpecialChar(NewCustomerPartNo, False)

            If CurrentCustomerDrawingNo Is Nothing Then
                CurrentCustomerDrawingNo = ""
            End If

            myCommand.Parameters.Add("@CurrentCustomerDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentCustomerDrawingNo").Value = commonFunctions.convertSpecialChar(CurrentCustomerDrawingNo, False)

            If NewCustomerDrawingNo Is Nothing Then
                NewCustomerDrawingNo = ""
            End If

            myCommand.Parameters.Add("@NewCustomerDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewCustomerDrawingNo").Value = commonFunctions.convertSpecialChar(NewCustomerDrawingNo, False)

            If CurrentCustomerPartName Is Nothing Then
                CurrentCustomerPartName = ""
            End If

            myCommand.Parameters.Add("@CurrentCustomerPartName", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentCustomerPartName").Value = commonFunctions.convertSpecialChar(CurrentCustomerPartName, False)

            If NewCustomerPartName Is Nothing Then
                NewCustomerPartName = ""
            End If

            myCommand.Parameters.Add("@NewCustomerPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewCustomerPartName").Value = commonFunctions.convertSpecialChar(NewCustomerPartName, False)

            If CurrentDesignLevel Is Nothing Then
                CurrentDesignLevel = ""
            End If

            myCommand.Parameters.Add("@CurrentDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDesignLevel").Value = commonFunctions.convertSpecialChar(CurrentDesignLevel, False)

            If NewDesignLevel Is Nothing Then
                NewDesignLevel = ""
            End If

            myCommand.Parameters.Add("@NewDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@NewDesignLevel").Value = commonFunctions.convertSpecialChar(NewDesignLevel, False)

            If CurrentDrawingNo Is Nothing Then
                CurrentDrawingNo = ""
            End If

            myCommand.Parameters.Add("@CurrentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDrawingNo").Value = commonFunctions.convertSpecialChar(CurrentDrawingNo, False)

            If NewDrawingNo Is Nothing Then
                NewDrawingNo = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = commonFunctions.convertSpecialChar(NewDrawingNo, False)

            myCommand.Parameters.Add("@NewInStepTracking", SqlDbType.Int)
            myCommand.Parameters("@NewInStepTracking").Value = NewInStepTracking

            myCommand.Parameters.Add("@NewAMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewAMDValue").Value = NewAMDValue

            If NewAMDUnits Is Nothing Then
                NewAMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewAMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDUnits").Value = NewAMDUnits

            If NewAMDTolerance Is Nothing Then
                NewAMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewAMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDTolerance").Value = NewAMDTolerance

            myCommand.Parameters.Add("@NewWMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewWMDValue").Value = NewWMDValue

            If NewWMDUnits Is Nothing Then
                NewWMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewWMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDUnits").Value = NewWMDUnits

            If NewWMDTolerance Is Nothing Then
                NewWMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewWMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDTolerance").Value = NewWMDTolerance

            If NewConstruction Is Nothing Then
                NewConstruction = ""
            End If

            myCommand.Parameters.Add("@NewConstruction", SqlDbType.VarChar)
            myCommand.Parameters("@NewConstruction").Value = commonFunctions.convertSpecialChar(NewConstruction, False)

            myCommand.Parameters.Add("@NewDensityValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewDensityValue").Value = NewDensityValue

            If NewDensityUnits Is Nothing Then
                NewDensityUnits = ""
            End If

            myCommand.Parameters.Add("@NewDensityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityUnits").Value = NewDensityUnits

            If NewDensityTolerance Is Nothing Then
                NewDensityTolerance = ""
            End If

            myCommand.Parameters.Add("@NewDensityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityTolerance").Value = NewDensityTolerance

            If NewDrawingNotes Is Nothing Then
                NewDrawingNotes = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNotes").Value = commonFunctions.convertSpecialChar(NewDrawingNotes, False)

            myCommand.Parameters.Add("@NewCommodityID", SqlDbType.Int)
            myCommand.Parameters("@NewCommodityID").Value = NewCommodityID

            myCommand.Parameters.Add("@NewProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@NewProductTechnologyID").Value = NewProductTechnologyID

            myCommand.Parameters.Add("@NewSubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@NewSubFamilyID").Value = NewSubFamilyID

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@isECIRequired", SqlDbType.Bit)
            myCommand.Parameters("@isECIRequired").Value = isECIRequired

            myCommand.Parameters.Add("@CapExProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjectNo").Value = CapExProjectNo

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = PurchasingPONo

            myCommand.Parameters.Add("@isAffectsCostSheetOnly", SqlDbType.Bit)
            myCommand.Parameters("@isAffectsCostSheetOnly").Value = isAffectsCostSheetOnly

            myCommand.Parameters.Add("@isCostingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCostingRequired").Value = isCostingRequired

            myCommand.Parameters.Add("@isCustomerApprovalRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerApprovalRequired").Value = isCustomerApprovalRequired

            myCommand.Parameters.Add("@isDVPRrequired", SqlDbType.Bit)
            myCommand.Parameters("@isDVPRrequired").Value = isDVPRrequired

            myCommand.Parameters.Add("@isPackagingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPackagingRequired").Value = isPackagingRequired

            myCommand.Parameters.Add("@isPlantControllerRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerRequired").Value = isPlantControllerRequired

            myCommand.Parameters.Add("@isProcessRequired", SqlDbType.Bit)
            myCommand.Parameters("@isProcessRequired").Value = isProcessRequired

            myCommand.Parameters.Add("@isProductDevelopmentRequired", SqlDbType.Bit)
            myCommand.Parameters("@isProductDevelopmentRequired").Value = isProductDevelopmentRequired

            myCommand.Parameters.Add("@isPurchasingExternalRFQRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPurchasingExternalRFQRequired").Value = isPurchasingExternalRFQRequired

            myCommand.Parameters.Add("@isPurchasingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPurchasingRequired").Value = isPurchasingRequired

            myCommand.Parameters.Add("@isQualityEngineeringRequired", SqlDbType.Bit)
            myCommand.Parameters("@isQualityEngineeringRequired").Value = isQualityEngineeringRequired

            myCommand.Parameters.Add("@isRDRequired", SqlDbType.Bit)
            myCommand.Parameters("@isRDRequired").Value = isRDRequired

            myCommand.Parameters.Add("@isToolingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isToolingRequired").Value = isToolingRequired

            myCommand.Parameters.Add("@ProductDevelopmentCommodityTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@ProductDevelopmentCommodityTeamMemberID").Value = ProductDevelopmentCommodityTeamMemberID

            myCommand.Parameters.Add("@PurchasingFamilyTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingFamilyTeamMemberID").Value = PurchasingFamilyTeamMemberID

            myCommand.Parameters.Add("@PurchasingMakeTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingMakeTeamMemberID").Value = PurchasingMakeTeamMemberID

            myCommand.Parameters.Add("@isPPAP", SqlDbType.Bit)
            myCommand.Parameters("@isPPAP").Value = isPPAP

            myCommand.Parameters.Add("@VendorRequirement", SqlDbType.VarChar)
            myCommand.Parameters("@VendorRequirement").Value = VendorRequirement

            myCommand.Parameters.Add("@isMaterialSizeChange", SqlDbType.Bit)
            myCommand.Parameters("@isMaterialSizeChange").Value = isMaterialSizeChange

            myCommand.Parameters.Add("@isContinuousLine", SqlDbType.Bit)
            myCommand.Parameters("@isContinuousLine").Value = isContinuousLine

            myCommand.Parameters.Add("@isCapitalRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCapitalRequired").Value = isCapitalRequired

            If CopyReason Is Nothing Then
                CopyReason = ""
            End If

            myCommand.Parameters.Add("@CopyReason", SqlDbType.VarChar)
            myCommand.Parameters("@CopyReason").Value = Replace(CopyReason, "'", "`") 'commonFunctions.convertSpecialChar(CopyReason, False)

            myCommand.Parameters.Add("@isMeetingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isMeetingRequired").Value = isMeetingRequired

            myCommand.Parameters.Add("@isCostReduction", SqlDbType.Bit)
            myCommand.Parameters("@isCostReduction").Value = isCostReduction

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", BusinessProcessActionID: " & BusinessProcessActionID _
            & ", BusinessProcessTypeID: " & BusinessProcessTypeID _
            & ", DesignationType: " & DesignationType _
            & ", PriceCode: " & PriceCode _
            & ", PriorityID: " & PriorityID _
            & ", DueDate: " & DueDate _
            & ", InitiatorTeamMemberID : " & InitiatorTeamMemberID _
            & ", AccountManagerID: " & AccountManagerID _
            & ", ProgramManagerID: " & ProgramManagerID _
            & ", ImpactOnUGN : " & ImpactOnUGN _
            & ", TargetPrice: " & TargetPrice _
            & ", TargetAnnualVolume : " & TargetAnnualVolume _
            & ", TargetAnnualSales: " & TargetAnnualSales _
            & ", CurrentCustomerPartNo : " & CurrentCustomerPartNo _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", CurrentCustomerDrawingNo : " & CurrentCustomerDrawingNo _
            & ", NewCustomerDrawingNo: " & NewCustomerDrawingNo _
            & ", CurrentCustomerPartName : " & CurrentCustomerPartName _
            & ", NewCustomerPartName: " & NewCustomerPartName _
            & ", CurrentDesignLevel : " & CurrentDesignLevel _
            & ", NewDesignLevel: " & NewDesignLevel _
            & ", CurrentDrawingNo : " & CurrentDrawingNo _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", NewInStepTracking : " & NewInStepTracking _
            & ", NewAMDValue : " & NewAMDValue _
            & ", NewAMDUnits: " & NewAMDUnits _
            & ", NewAMDTolerance : " & NewAMDTolerance _
            & ", NewWMDValue: " & NewWMDValue _
            & ", NewWMDUnits : " & NewWMDUnits _
            & ", NewWMDTolerance: " & NewWMDTolerance _
            & ", NewConstruction : " & NewConstruction _
            & ", NewDensityValue: " & NewDensityValue _
            & ", NewDensityUnits : " & NewDensityUnits _
            & ", NewDensityTolerance: " & NewDensityTolerance _
            & ", NewDrawingNotes  : " & NewDrawingNotes _
            & ", NewCommodityID: " & NewCommodityID _
            & ", NewProductTechnologyID  : " & NewProductTechnologyID _
            & ", NewSubFamilyID  : " & NewSubFamilyID _
            & ", FamilyID  : " & FamilyID _
            & ", Make  : " & Make _
            & ", CostSheetID: " & CostSheetID _
            & ", ECINo  : " & ECINo _
            & ", isECIRequired  : " & isECIRequired _
            & ", CapExProjectNo: " & CapExProjectNo _
            & ", PurchasingPONo  : " & PurchasingPONo _
            & ", isAffectsCostSheetOnly: " & isAffectsCostSheetOnly _
            & ", isCostingRequired  : " & isCostingRequired _
            & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", isDVPRrequired  : " & isDVPRrequired _
            & ", isPackagingRequired: " & isPackagingRequired _
            & ", isPlantControllerRequired  : " & isPlantControllerRequired _
            & ", isProcessRequired: " & isProcessRequired _
            & ", isProductDevelopmentRequired  : " & isProductDevelopmentRequired _
            & ", isPurchasingExternalRFQRequired: " & isPurchasingExternalRFQRequired _
            & ", isPurchasingRequired: " & isPurchasingRequired _
            & ", isQualityEngineeringRequired  : " & isQualityEngineeringRequired _
            & ", isRDRequired: " & isRDRequired _
            & ", isToolingRequired  : " & isToolingRequired _
            & ", ProductDevelopmentCommodityTeamMemberID  : " & ProductDevelopmentCommodityTeamMemberID _
            & ", PurchasingFamilyTeamMemberID: " & PurchasingFamilyTeamMemberID _
            & ", PurchasingMakeTeamMemberID  : " & PurchasingMakeTeamMemberID _
            & ", isPPAP  : " & isPPAP _
            & ", VendorRequirement  : " & VendorRequirement _
            & ", isMaterialSizeChange  : " & isMaterialSizeChange _
            & ", isContinuousLine  : " & isContinuousLine _
            & ", isCapitalRequired  : " & isCapitalRequired _
            & ", CopyReason  : " & CopyReason _
            & ", isMeetingRequired: " & isMeetingRequired _
            & ", isCostReduction: " & isCostReduction _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDFromCosting(ByVal RFDNo As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_From_Costing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDFromCosting : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDFromCosting : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateDrawingFromRFD(ByVal DrawingNo As String, ByVal OldPartName As String, ByVal ReleaseTypeID As Integer, _
        ByVal InStepTracking As Integer, ByVal RFDNo As Integer, ByVal DesignationType As String, ByVal SubFamilyID As Integer, _
        ByVal ProductTechnologyID As Integer, ByVal CommodityID As Integer, _
        ByVal PurchasedGoodID As Integer, ByVal DrawingByEngineerID As Integer, _
        ByVal DensityValue As Double, ByVal DensityUnits As String, ByVal DensityTolerance As String, _
        ByVal AMDValue As Double, ByVal AMDUnits As String, ByVal AMDTolerance As String, _
        ByVal WMDValue As Double, ByVal WMDUnits As String, ByVal WMDTolerance As String, _
        ByVal Construction As String, ByVal Notes As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_From_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            If OldPartName Is Nothing Then
                OldPartName = ""
            End If

            myCommand.Parameters.Add("@OldPartName", SqlDbType.VarChar)
            myCommand.Parameters("@OldPartName").Value = commonFunctions.convertSpecialChar(OldPartName, False)

            myCommand.Parameters.Add("@ReleaseTypeID", SqlDbType.Int)
            myCommand.Parameters("@ReleaseTypeID").Value = ReleaseTypeID

            myCommand.Parameters.Add("@InStepTracking", SqlDbType.Int)
            myCommand.Parameters("@InStepTracking").Value = InStepTracking

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@DrawingByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@DrawingByEngineerID").Value = DrawingByEngineerID

            myCommand.Parameters.Add("@DensityValue", SqlDbType.Decimal)
            myCommand.Parameters("@DensityValue").Value = DensityValue

            If DensityUnits Is Nothing Then
                DensityUnits = ""
            End If

            myCommand.Parameters.Add("@DensityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@DensityUnits").Value = DensityUnits

            If DensityTolerance Is Nothing Then
                DensityTolerance = ""
            End If

            myCommand.Parameters.Add("@DensityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@DensityTolerance").Value = DensityTolerance

            myCommand.Parameters.Add("@AMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@AMDValue").Value = AMDValue

            If AMDUnits Is Nothing Then
                AMDUnits = ""
            End If

            myCommand.Parameters.Add("@AMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@AMDUnits").Value = AMDUnits

            If AMDTolerance Is Nothing Then
                AMDTolerance = ""
            End If

            myCommand.Parameters.Add("@AMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@AMDTolerance").Value = AMDTolerance

            myCommand.Parameters.Add("@WMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@WMDValue").Value = WMDValue

            If WMDUnits Is Nothing Then
                WMDUnits = ""
            End If

            myCommand.Parameters.Add("@WMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@WMDUnits").Value = WMDUnits

            If WMDTolerance Is Nothing Then
                WMDTolerance = ""
            End If

            myCommand.Parameters.Add("@WMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@WMDTolerance").Value = WMDTolerance

            If Construction Is Nothing Then
                Construction = ""
            End If

            myCommand.Parameters.Add("@Construction", SqlDbType.VarChar)
            myCommand.Parameters("@Construction").Value = commonFunctions.convertSpecialChar(Construction, False)

            If Notes Is Nothing Then
                Notes = ""
            End If

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", OldPartName: " & OldPartName _
            & ", ReleaseTypeID: " & ReleaseTypeID _
            & ", RFDNo: " & RFDNo _
            & ", DesignationType : " & DesignationType _
            & ", SubFamilyID: " & SubFamilyID _
            & ", ProductTechnologyID: " & ProductTechnologyID _
            & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", DrawingByEngineerID : " & DrawingByEngineerID _
            & ", DensityValue: " & DensityValue _
            & ", DensityUnits: " & DensityUnits _
            & ", DensityTolerance: " & DensityTolerance _
            & ", AMDValue: " & AMDValue _
            & ", AMDUnits: " & AMDUnits _
            & ", AMDTolerance: " & AMDTolerance _
            & ", WMDValue: " & WMDValue _
            & ", WMDUnits: " & WMDUnits _
            & ", WMDTolerance: " & WMDTolerance _
            & ", Construction: " & Construction _
            & ", Notes: " & Notes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingFromRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingFromRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDApprovalStatus(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, _
        ByVal TeamMemberID As Integer, ByVal Comments As String, ByVal CavityCount As Integer, _
        ByVal StatusID As Integer, ByVal NotificationDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            If Comments Is Nothing Then
                Comments = ""
            End If

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@CavityCount", SqlDbType.Int)
            myCommand.Parameters("@CavityCount").Value = CavityCount

            If NotificationDate Is Nothing Then
                NotificationDate = ""
            End If

            myCommand.Parameters.Add("@NotificationDate", SqlDbType.VarChar)
            myCommand.Parameters("@NotificationDate").Value = commonFunctions.convertSpecialChar(NotificationDate, False)

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", SubscriptionID: " & SubscriptionID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", Comments: " & Comments _
            & ", StatusID: " & StatusID _
            & ", CavityCount: " & CavityCount _
            & ", NotificationDate: " & NotificationDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDBusinessAwarded(ByVal RFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Business_Awarded"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDBusinessAwarded : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDBusinessAwarded : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub


    Public Shared Sub UpdateRFDOverallStatus(ByVal RFDNo As Integer, ByVal StatusID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Overall_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", StatusID: " & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDFinishedGood(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal PartNo As String, _
        ByVal PartName As String, ByVal DrawingNo As String, ByVal CostSheetID As Integer, _
        ByVal ECINo As Integer, ByVal CapExProjectNo As String, ByVal PurchasingPONo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Finished_Good"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartName Is Nothing Then PartName = ""

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = PartName

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            If CapExProjectNo Is Nothing Then
                CapExProjectNo = ""
            End If

            myCommand.Parameters.Add("@CapExProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjectNo").Value = CapExProjectNo

            If PurchasingPONo Is Nothing Then
                PurchasingPONo = ""
            End If

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = PurchasingPONo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", RFDNo: " & RFDNo _
            & ", PartNo: " & PartNo _
            & ", PartName: " & PartName & ", CostSheetID: " & CostSheetID _
            & ", ECINo: " & ECINo & ", CapExProjectNo: " & CapExProjectNo _
            & ", PurchasingPONo: " & PurchasingPONo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDChildPart(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal CurrentPartNo As String, _
        ByVal NewPartNo As String, _
        ByVal CurrentPartName As String, ByVal NewPartName As String, ByVal CurrentDrawingNo As String, _
        ByVal NewDrawingNo As String, ByVal CostSheetID As Integer, _
        ByVal ECINo As Integer, ByVal isECIRequired As Boolean, _
        ByVal PurchasingPONo As String, ByVal ExternalRFQNo As String, ByVal isExternalRFQrequired As Boolean, _
        ByVal NewInStepTracking As Integer, ByVal NewAMDValue As Double, ByVal NewAMDUnits As String, _
        ByVal NewAMDTolerance As String, ByVal NewWMDValue As Double, _
        ByVal NewWMDUnits As String, ByVal NewWMDTolerance As String, _
        ByVal NewConstruction As String, ByVal NewDensityValue As Double, _
        ByVal NewDensityUnits As String, ByVal NewDensityTolerance As String, ByVal NewDrawingNotes As String, _
        ByVal NewDesignationType As String, ByVal NewSubFamilyID As Integer, ByVal NewPurchasedGoodID As Integer, _
        ByVal NewPartLeadTime As Integer, ByVal NewPartLeadUnits As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If CurrentPartNo Is Nothing Then CurrentPartNo = ""

            myCommand.Parameters.Add("@CurrentPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartNo").Value = commonFunctions.convertSpecialChar(CurrentPartNo, False)

            If NewPartNo Is Nothing Then NewPartNo = ""

            myCommand.Parameters.Add("@NewPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartNo").Value = commonFunctions.convertSpecialChar(NewPartNo, False)

            If CurrentPartName Is Nothing Then CurrentPartName = ""

            myCommand.Parameters.Add("@CurrentPartName", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartName").Value = commonFunctions.convertSpecialChar(CurrentPartName, False)

            If NewPartName Is Nothing Then NewPartName = ""

            myCommand.Parameters.Add("@NewPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartName").Value = commonFunctions.convertSpecialChar(NewPartName, False)

            If CurrentDrawingNo Is Nothing Then CurrentDrawingNo = ""

            myCommand.Parameters.Add("@CurrentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDrawingNo").Value = commonFunctions.convertSpecialChar(CurrentDrawingNo, False)

            If NewDrawingNo Is Nothing Then NewDrawingNo = ""

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = commonFunctions.convertSpecialChar(NewDrawingNo, False)

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@isECIRequired", SqlDbType.Bit)
            myCommand.Parameters("@isECIRequired").Value = isECIRequired

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = Replace(PurchasingPONo, "'", "`")

            If ExternalRFQNo Is Nothing Then
                ExternalRFQNo = ""
            End If

            myCommand.Parameters.Add("@ExternalRFQNo", SqlDbType.VarChar)
            myCommand.Parameters("@ExternalRFQNo").Value = Replace(ExternalRFQNo, "'", "`")

            myCommand.Parameters.Add("@isExternalRFQrequired", SqlDbType.Bit)
            myCommand.Parameters("@isExternalRFQrequired").Value = isExternalRFQrequired

            myCommand.Parameters.Add("@NewInStepTracking", SqlDbType.Int)
            myCommand.Parameters("@NewInStepTracking").Value = NewInStepTracking

            myCommand.Parameters.Add("@NewAMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewAMDValue").Value = NewAMDValue

            If NewAMDUnits Is Nothing Then
                NewAMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewAMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDUnits").Value = NewAMDUnits

            If NewAMDTolerance Is Nothing Then
                NewAMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewAMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDTolerance").Value = NewAMDTolerance

            myCommand.Parameters.Add("@NewWMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewWMDValue").Value = NewWMDValue

            If NewWMDUnits Is Nothing Then
                NewWMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewWMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDUnits").Value = NewWMDUnits

            If NewWMDTolerance Is Nothing Then
                NewWMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewWMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDTolerance").Value = NewWMDTolerance

            myCommand.Parameters.Add("@NewConstruction", SqlDbType.VarChar)
            myCommand.Parameters("@NewConstruction").Value = Replace(NewConstruction, "'", "`") 'commonFunctions.convertSpecialChar(NewConstruction, False)

            myCommand.Parameters.Add("@NewDensityValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewDensityValue").Value = NewDensityValue

            If NewDensityUnits Is Nothing Then
                NewDensityUnits = ""
            End If

            myCommand.Parameters.Add("@NewDensityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityUnits").Value = NewDensityUnits

            If NewDensityTolerance Is Nothing Then
                NewDensityTolerance = ""
            End If

            myCommand.Parameters.Add("@NewDensityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityTolerance").Value = NewDensityTolerance

            If NewDensityTolerance Is Nothing Then
                NewDrawingNotes = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNotes").Value = Replace(NewDrawingNotes, "'", "`") 'commonFunctions.convertSpecialChar(NewDrawingNotes, False)

            If NewDesignationType Is Nothing Then
                NewDesignationType = ""
            End If

            myCommand.Parameters.Add("@NewDesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@NewDesignationType").Value = NewDesignationType

            myCommand.Parameters.Add("@NewSubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@NewSubFamilyID").Value = NewSubFamilyID

            myCommand.Parameters.Add("@NewPurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@NewPurchasedGoodID").Value = NewPurchasedGoodID

            myCommand.Parameters.Add("@NewPartLeadTime", SqlDbType.Decimal)
            myCommand.Parameters("@NewPartLeadTime").Value = NewPartLeadTime

            If NewPartLeadUnits Is Nothing Then
                NewPartLeadUnits = ""
            End If

            If NewPartLeadTime <> 0 And NewPartLeadUnits = "" Then
                NewPartLeadUnits = "weeks"
            End If

            myCommand.Parameters.Add("@NewPartLeadUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartLeadUnits").Value = NewPartLeadUnits

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", RFDNo: " & RFDNo _
            & ", CurrentPartNo: " & CurrentPartNo _
            & ", NewPartNo: " & NewPartNo _
            & ", CurrentPartName: " & CurrentPartName _
            & ", NewPartName: " & NewPartName _
            & ", CostSheetID: " & CostSheetID _
            & ", ECINo: " & ECINo _
            & ", isECIRequired: " & isECIRequired _
            & ", ExternalRFQNo" & ExternalRFQNo _
            & ", isExternalRFQrequired" & isExternalRFQrequired _
            & ", PurchasingPONo: " & PurchasingPONo _
            & ", NewInStepTracking: " & NewInStepTracking _
            & ", NewAMDValue: " & NewAMDValue _
            & ", NewAMDUnits: " & NewAMDUnits _
            & ", NewAMDTolerance: " & NewAMDTolerance _
            & ", NewWMDValue: " & NewWMDValue _
            & ", NewWMDUnits: " & NewWMDUnits _
            & ", NewWMDTolerance: " & NewWMDTolerance _
            & ", NewConstruction: " & NewConstruction _
            & ", NewDensityValue: " & NewDensityValue _
            & ", NewDensityUnits: " & NewDensityUnits _
            & ", NewDensityTolerance: " & NewDensityTolerance _
            & ", NewDrawingNotes : " & NewDrawingNotes _
            & ", NewDesignationType : " & NewDesignationType _
            & ", NewSubFamilyID : " & NewSubFamilyID _
            & ", NewPurchasedGoodID : " & NewPurchasedGoodID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDChildPart: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDChildPart : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDChildPartFromCosting(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Child_Part_From_Costing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", RFDNo: " & RFDNo _
            & ", CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDChildPartFromCosting : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDChildPartFromCosting : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDProcess(ByVal RFDNo As Integer, ByVal ProcessNotes As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Process"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If ProcessNotes Is Nothing Then
                ProcessNotes = ""
            End If

            myCommand.Parameters.Add("@ProcessNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProcessNotes").Value = Replace(ProcessNotes, "'", "`") 'commonFunctions.convertSpecialChar(ProcessNotes, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", ProcessNotes: " & ProcessNotes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDProcess : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDProcess : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDCapital(ByVal RFDNo As Integer, ByVal CapitalNotes As String, _
        ByVal CapitalLeadTime As Integer, ByVal CapitalLeadUnits As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If CapitalNotes Is Nothing Then
                CapitalNotes = ""
            End If

            myCommand.Parameters.Add("@CapitalNotes", SqlDbType.VarChar)
            myCommand.Parameters("@CapitalNotes").Value = Replace(CapitalNotes, "'", "`") 'commonFunctions.convertSpecialChar(CapitalNotes, False)

            myCommand.Parameters.Add("@CapitalLeadTime", SqlDbType.Decimal)
            myCommand.Parameters("@CapitalLeadTime").Value = CapitalLeadTime

            If CapitalLeadUnits Is Nothing Then
                CapitalLeadUnits = ""
            End If

            If CapitalLeadTime <> 0 And CapitalLeadUnits = "" Then
                CapitalLeadUnits = "weeks"
            End If

            myCommand.Parameters.Add("@CapitalLeadUnits", SqlDbType.VarChar)
            myCommand.Parameters("@CapitalLeadUnits").Value = CapitalLeadUnits

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", CapitalNotes: " & CapitalNotes _
            & ", CapitalLeadTime: " & CapitalLeadTime _
            & ", CapitalLeadUnits: " & CapitalLeadUnits _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDCapital : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDCapital : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDTooling(ByVal RFDNo As Integer, ByVal ToolingNotes As String, _
        ByVal ToolingLeadTime As Integer, ByVal ToolingLeadUnits As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Tooling"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If ToolingNotes Is Nothing Then
                ToolingNotes = ""
            End If

            myCommand.Parameters.Add("@ToolingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ToolingNotes").Value = Replace(ToolingNotes, "'", "`") 'commonFunctions.convertSpecialChar(ToolingNotes, False)

            myCommand.Parameters.Add("@ToolingLeadTime", SqlDbType.Decimal)
            myCommand.Parameters("@ToolingLeadTime").Value = ToolingLeadTime

            If ToolingLeadUnits Is Nothing Then
                ToolingLeadUnits = ""
            End If

            If ToolingLeadTime <> 0 And ToolingLeadUnits = "" Then
                ToolingLeadUnits = "weeks"
            End If

            myCommand.Parameters.Add("@ToolingLeadUnits", SqlDbType.VarChar)
            myCommand.Parameters("@ToolingLeadUnits").Value = ToolingLeadUnits

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", ToolingNotes: " & ToolingNotes _
            & ", ToolingLeadTime: " & ToolingLeadTime _
            & ", ToolingLeadUnits: " & ToolingLeadUnits _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDTooling : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDTooling : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDChildPart(ByVal RFDNo As Integer, ByVal CurrentPartNo As String, _
       ByVal NewPartNo As String, _
       ByVal CurrentPartName As String, ByVal NewPartName As String, ByVal CurrentDrawingNo As String, _
       ByVal NewDrawingNo As String, ByVal CostSheetID As Integer, ByVal ECINo As Integer, _
       ByVal PurchasingPONo As String, ByVal ExternalRFQNo As String, ByVal isExternalRFQrequired As Boolean, _
       ByVal NewInStepTracking As Integer, ByVal NewAMDValue As Double, ByVal NewAMDUnits As String, _
       ByVal NewAMDTolerance As String, ByVal NewWMDValue As Double, _
       ByVal NewWMDUnits As String, ByVal NewWMDTolerance As String, ByVal NewConstruction As String, ByVal NewDensityValue As Double, _
       ByVal NewDensityUnits As String, ByVal NewDensityTolerance As String, ByVal NewDrawingNotes As String, _
       ByVal NewDesignationType As String, ByVal NewSubFamilyID As Integer, ByVal NewPurchasedGoodID As Integer, _
       ByVal NewPartLeadTime As Integer, ByVal NewPartLeadUnits As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If CurrentPartNo Is Nothing Then CurrentPartNo = ""

            myCommand.Parameters.Add("@CurrentPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartNo").Value = commonFunctions.convertSpecialChar(CurrentPartNo, False)

            If NewPartNo Is Nothing Then NewPartNo = ""

            myCommand.Parameters.Add("@NewPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartNo").Value = commonFunctions.convertSpecialChar(NewPartNo, False)


            If CurrentPartName Is Nothing Then CurrentPartName = ""

            myCommand.Parameters.Add("@CurrentPartName", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartName").Value = commonFunctions.convertSpecialChar(CurrentPartName, False)

            If NewPartName Is Nothing Then NewPartName = ""

            myCommand.Parameters.Add("@NewPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartName").Value = commonFunctions.convertSpecialChar(NewPartName, False)

            If CurrentDrawingNo Is Nothing Then
                CurrentDrawingNo = ""
            End If

            myCommand.Parameters.Add("@CurrentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDrawingNo").Value = commonFunctions.convertSpecialChar(CurrentDrawingNo, False)

            If NewDrawingNo Is Nothing Then
                NewDrawingNo = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = Replace(NewDrawingNo, "'", "`") 'commonFunctions.convertSpecialChar(NewDrawingNo, False)

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            'myCommand.Parameters.Add("@PONo", SqlDbType.Int)
            'myCommand.Parameters("@PONo").Value = PONo

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = Replace(PurchasingPONo, "'", "`")

            If ExternalRFQNo Is Nothing Then
                ExternalRFQNo = ""
            End If

            myCommand.Parameters.Add("@ExternalRFQNo", SqlDbType.VarChar)
            myCommand.Parameters("@ExternalRFQNo").Value = Replace(ExternalRFQNo, "'", "`")

            myCommand.Parameters.Add("@isExternalRFQrequired", SqlDbType.Bit)
            myCommand.Parameters("@isExternalRFQrequired").Value = isExternalRFQrequired

            myCommand.Parameters.Add("@NewInStepTracking", SqlDbType.Int)
            myCommand.Parameters("@NewInStepTracking").Value = NewInStepTracking

            myCommand.Parameters.Add("@NewAMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewAMDValue").Value = NewAMDValue

            If NewAMDUnits Is Nothing Then
                NewAMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewAMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDUnits").Value = NewAMDUnits

            If NewAMDTolerance Is Nothing Then
                NewAMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewAMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewAMDTolerance").Value = NewAMDTolerance

            myCommand.Parameters.Add("@NewWMDValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewWMDValue").Value = NewWMDValue

            If NewWMDUnits Is Nothing Then
                NewWMDUnits = ""
            End If

            myCommand.Parameters.Add("@NewWMDUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDUnits").Value = NewAMDUnits

            If NewWMDTolerance Is Nothing Then
                NewWMDTolerance = ""
            End If

            myCommand.Parameters.Add("@NewWMDTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewWMDTolerance").Value = NewWMDTolerance

            If NewConstruction Is Nothing Then
                NewConstruction = ""
            End If

            myCommand.Parameters.Add("@NewConstruction", SqlDbType.VarChar)
            myCommand.Parameters("@NewConstruction").Value = Replace(NewConstruction, "'", "`") 'commonFunctions.convertSpecialChar(NewConstruction, False)

            myCommand.Parameters.Add("@NewDensityValue", SqlDbType.Decimal)
            myCommand.Parameters("@NewDensityValue").Value = NewDensityValue

            If NewDensityUnits Is Nothing Then
                NewDensityUnits = ""
            End If

            myCommand.Parameters.Add("@NewDensityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityUnits").Value = NewDensityUnits

            If NewDensityTolerance Is Nothing Then
                NewDensityTolerance = ""
            End If

            myCommand.Parameters.Add("@NewDensityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@NewDensityTolerance").Value = NewDensityTolerance

            If NewDrawingNotes Is Nothing Then
                NewDrawingNotes = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNotes", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNotes").Value = Replace(NewDrawingNotes, "'", "`") 'commonFunctions.convertSpecialChar(NewDrawingNotes, False)

            If NewDesignationType Is Nothing Then
                NewDesignationType = ""
            End If

            myCommand.Parameters.Add("@NewDesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@NewDesignationType").Value = NewDesignationType

            myCommand.Parameters.Add("@NewSubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@NewSubFamilyID").Value = NewSubFamilyID

            myCommand.Parameters.Add("@NewPurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@NewPurchasedGoodID").Value = NewPurchasedGoodID

            myCommand.Parameters.Add("@NewPartLeadTime", SqlDbType.Decimal)
            myCommand.Parameters("@NewPartLeadTime").Value = NewPartLeadTime

            If NewPartLeadUnits Is Nothing Then
                NewPartLeadUnits = ""
            End If

            If NewPartLeadTime <> 0 And NewPartLeadUnits = "" Then
                NewPartLeadUnits = "weeks"
            End If

            myCommand.Parameters.Add("@NewPartLeadUnits", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartLeadUnits").Value = NewPartLeadUnits

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", CurrentPartNo: " & CurrentPartNo _
            & ", NewPartNo: " & NewPartNo _
            & ", CurrentPartName: " & CurrentPartName _
            & ", NewPartName: " & NewPartName _
            & ", CostSheetID: " & CostSheetID _
            & ", ECINo: " & ECINo _
            & ", PurchasingPONo: " & PurchasingPONo _
            & ", ExternalRFQNo: " & ExternalRFQNo _
            & ", isExternalRFQrequired: " & isExternalRFQrequired _
            & ", NewInStepTracking: " & NewInStepTracking _
            & ", NewAMDValue: " & NewAMDValue _
            & ", NewAMDUnits: " & NewAMDUnits _
            & ", NewAMDTolerance: " & NewAMDTolerance _
            & ", NewWMDValue: " & NewWMDValue _
            & ", NewWMDUnits: " & NewWMDUnits _
            & ", NewWMDTolerance: " & NewWMDTolerance _
            & ", NewConstruction: " & NewConstruction _
            & ", NewDensityValue: " & NewDensityValue _
            & ", NewDensityUnits: " & NewDensityUnits _
            & ", NewDensityTolerance: " & NewDensityTolerance _
            & ", NewDrawingNotes : " & NewDrawingNotes _
            & ", NewDesignationType : " & NewDesignationType _
            & ", NewSubFamilyID : " & NewSubFamilyID _
            & ", NewPurchasedGoodID : " & NewPurchasedGoodID _
            & ", NewPartLeadTime : " & NewPartLeadTime _
            & ", NewPartLeadUnits : " & NewPartLeadUnits _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDChildPart : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDChildPart : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDFinishedGood(ByVal RFDNo As Integer, ByVal PartNo As String, _
       ByVal PartName As String, ByVal DrawingNo As String, ByVal CostSheetID As Integer, _
      ByVal ECINo As Integer, ByVal CapExProjectNo As String, ByVal PurchasingPONo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Finished_Good"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartName Is Nothing Then PartName = ""

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = PartName

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            If CapExProjectNo Is Nothing Then
                CapExProjectNo = ""
            End If

            myCommand.Parameters.Add("@CapExProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@CapExProjectNo").Value = CapExProjectNo

            If PurchasingPONo Is Nothing Then
                PurchasingPONo = ""
            End If

            myCommand.Parameters.Add("@PurchasingPONo", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingPONo").Value = PurchasingPONo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", PartNo: " & PartNo _
            & ", PartName: " & PartName & ", CostSheetID: " & CostSheetID _
            & ", ECINo: " & ECINo & ", CapExProjectNo: " & CapExProjectNo _
            & ", PurchasingPONo: " & PurchasingPONo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateDrawingCustomerProgramBasedOnRFD(ByVal DrawingNo As String, ByVal RFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Customer_Program_Based_On_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingCustomerProgramBasedOnRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingCustomerProgramBasedOnRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function InsertRFD(ByVal PreviousRFDNo As Integer, ByVal StatusID As Integer, _
        ByVal RFDDesc As String, ByVal BusinessProcessActionID As Integer, ByVal BusinessProcessTypeID As Integer, _
        ByVal DesignationType As String, _
        ByVal PriceCode As String, ByVal PriorityID As Integer, ByVal DueDate As String, _
        ByVal InitiatorTeamMemberID As Integer, ByVal AccountManagerID As Integer, _
        ByVal ProgramManagerID As Integer, ByVal ImpactOnUGN As String, ByVal TargetPrice As Double, _
        ByVal TargetAnnualVolume As Integer, ByVal TargetAnnualSales As Double, _
        ByVal NewCommodityID As Integer, ByVal FamilyID As Integer, ByVal Make As String, ByVal isAffectsCostSheetOnly As Boolean, _
        ByVal isCostingRequired As Boolean, ByVal isCustomerApprovalRequired As Boolean, _
        ByVal isDVPRrequired As Boolean, _
        ByVal isPackagingRequired As Boolean, ByVal isPlantControllerRequired As Boolean, _
        ByVal isProcessRequired As Boolean, ByVal isProductDevelopmentRequired As Boolean, _
        ByVal isPurchasingExternalRFQRequired As Boolean, ByVal isPurchasingRequired As Boolean, _
        ByVal isQualityEngineeringRequired As Boolean, ByVal isRDRequired As Boolean, ByVal isToolingRequired As Boolean, _
        ByVal ProductDevelopmentCommodityTeamMemberID As Integer, ByVal PurchasingFamilyTeamMemberID As Integer, _
        ByVal PurchasingMakeTeamMemberID As Integer, ByVal isMaterialSizeChange As Boolean, _
        ByVal isContinuousLine As Boolean, ByVal isCapitalRequired As Boolean, _
        ByVal CopyReason As String, ByVal isMeetingRequired As Boolean, ByVal isCostReduction As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PreviousRFDNo", SqlDbType.Int)
            myCommand.Parameters("@PreviousRFDNo").Value = PreviousRFDNo

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            If RFDDesc Is Nothing Then
                RFDDesc = ""
            End If

            myCommand.Parameters.Add("@RFDDesc", SqlDbType.VarChar)
            myCommand.Parameters("@RFDDesc").Value = Replace(RFDDesc, "'", "`") 'commonFunctions.convertSpecialChar(RFDDesc, False)

            myCommand.Parameters.Add("@BusinessProcessActionID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessActionID").Value = BusinessProcessActionID

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            If PriceCode Is Nothing Then
                PriceCode = ""
            End If

            myCommand.Parameters.Add("@PriceCode", SqlDbType.VarChar)
            myCommand.Parameters("@PriceCode").Value = PriceCode

            myCommand.Parameters.Add("@PriorityID", SqlDbType.Int)
            myCommand.Parameters("@PriorityID").Value = PriorityID

            If DueDate Is Nothing Then
                DueDate = ""
            End If

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            myCommand.Parameters.Add("@InitiatorTeamMemberID ", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID ").Value = InitiatorTeamMemberID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            If ImpactOnUGN Is Nothing Then
                ImpactOnUGN = ""
            End If

            myCommand.Parameters.Add("@ImpactOnUGN", SqlDbType.VarChar)
            myCommand.Parameters("@ImpactOnUGN").Value = commonFunctions.convertSpecialChar(ImpactOnUGN, False)

            myCommand.Parameters.Add("@TargetPrice", SqlDbType.Decimal)
            myCommand.Parameters("@TargetPrice").Value = TargetPrice

            myCommand.Parameters.Add("@TargetAnnualVolume", SqlDbType.Int)
            myCommand.Parameters("@TargetAnnualVolume").Value = TargetAnnualVolume

            myCommand.Parameters.Add("@TargetAnnualSales", SqlDbType.Decimal)
            myCommand.Parameters("@TargetAnnualSales").Value = TargetAnnualSales

            myCommand.Parameters.Add("@NewCommodityID", SqlDbType.Int)
            myCommand.Parameters("@NewCommodityID").Value = NewCommodityID

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@isAffectsCostSheetOnly", SqlDbType.Bit)
            myCommand.Parameters("@isAffectsCostSheetOnly").Value = isAffectsCostSheetOnly

            myCommand.Parameters.Add("@isCostingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCostingRequired").Value = isCostingRequired

            myCommand.Parameters.Add("@isCustomerApprovalRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerApprovalRequired").Value = isCustomerApprovalRequired

            myCommand.Parameters.Add("@isDVPRrequired", SqlDbType.Bit)
            myCommand.Parameters("@isDVPRrequired").Value = isDVPRrequired

            myCommand.Parameters.Add("@isPackagingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPackagingRequired").Value = isPackagingRequired

            myCommand.Parameters.Add("@isPlantControllerRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPlantControllerRequired").Value = isPlantControllerRequired

            myCommand.Parameters.Add("@isProcessRequired", SqlDbType.Bit)
            myCommand.Parameters("@isProcessRequired").Value = isProcessRequired

            myCommand.Parameters.Add("@isProductDevelopmentRequired", SqlDbType.Bit)
            myCommand.Parameters("@isProductDevelopmentRequired").Value = isProductDevelopmentRequired

            myCommand.Parameters.Add("@isPurchasingExternalRFQRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPurchasingExternalRFQRequired").Value = isPurchasingExternalRFQRequired

            myCommand.Parameters.Add("@isPurchasingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isPurchasingRequired").Value = isPurchasingRequired

            myCommand.Parameters.Add("@isQualityEngineeringRequired", SqlDbType.Bit)
            myCommand.Parameters("@isQualityEngineeringRequired").Value = isQualityEngineeringRequired

            myCommand.Parameters.Add("@isRDRequired", SqlDbType.Bit)
            myCommand.Parameters("@isRDRequired").Value = isRDRequired

            myCommand.Parameters.Add("@isToolingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isToolingRequired").Value = isToolingRequired

            myCommand.Parameters.Add("@ProductDevelopmentCommodityTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@ProductDevelopmentCommodityTeamMemberID").Value = ProductDevelopmentCommodityTeamMemberID

            myCommand.Parameters.Add("@PurchasingFamilyTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingFamilyTeamMemberID").Value = PurchasingFamilyTeamMemberID

            myCommand.Parameters.Add("@PurchasingMakeTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingMakeTeamMemberID").Value = PurchasingMakeTeamMemberID

            myCommand.Parameters.Add("@isMaterialSizeChange", SqlDbType.Bit)
            myCommand.Parameters("@isMaterialSizeChange").Value = isMaterialSizeChange

            myCommand.Parameters.Add("@isContinuousLine", SqlDbType.Bit)
            myCommand.Parameters("@isContinuousLine").Value = isContinuousLine

            myCommand.Parameters.Add("@isCapitalRequired", SqlDbType.Bit)
            myCommand.Parameters("@isCapitalRequired").Value = isCapitalRequired

            If CopyReason Is Nothing Then
                CopyReason = ""
            End If

            myCommand.Parameters.Add("@CopyReason", SqlDbType.VarChar)
            myCommand.Parameters("@CopyReason").Value = commonFunctions.convertSpecialChar(CopyReason, False)

            myCommand.Parameters.Add("@isMeetingRequired", SqlDbType.Bit)
            myCommand.Parameters("@isMeetingRequired").Value = isMeetingRequired

            myCommand.Parameters.Add("@isCostReduction", SqlDbType.Bit)
            myCommand.Parameters("@isCostReduction").Value = isCostReduction

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewRFD")
            InsertRFD = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PreviousRFDNo: " & PreviousRFDNo _
            & ", StatusID: " & StatusID _
            & ", RFDDesc: " & RFDDesc _
            & ", BusinessProcessActionID: " & BusinessProcessActionID _
            & ", BusinessProcessTypeID: " & BusinessProcessTypeID _
            & ", DesignationType: " & DesignationType _
            & ", PriceCode: " & PriceCode _
            & ", PriorityID: " & PriorityID _
            & ", DueDate: " & DueDate _
            & ", InitiatorTeamMemberID : " & InitiatorTeamMemberID _
            & ", AccountManagerID: " & AccountManagerID _
            & ", ProgramManagerID: " & ProgramManagerID _
            & ", ImpactOnUGN : " & ImpactOnUGN _
            & ", TargetPrice: " & TargetPrice _
            & ", TargetAnnualVolume : " & TargetAnnualVolume _
            & ", TargetAnnualSales: " & TargetAnnualSales _
            & ", NewCommodityID: " & NewCommodityID _
            & ", FamilyID  : " & FamilyID _
            & ", Make  : " & Make _
            & ", isAffectsCostSheetOnly: " & isAffectsCostSheetOnly _
            & ", isCostingRequired  : " & isCostingRequired _
            & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", isDVPRrequired  : " & isDVPRrequired _
            & ", isPackagingRequired: " & isPackagingRequired _
            & ", isPlantControllerRequired  : " & isPlantControllerRequired _
            & ", isProcessRequired: " & isProcessRequired _
            & ", isProductDevelopmentRequired  : " & isProductDevelopmentRequired _
            & ", isPurchasingExternalRFQRequired: " & isPurchasingExternalRFQRequired _
            & ", isPurchasingRequired: " & isPurchasingRequired _
            & ", isQualityEngineeringRequired  : " & isQualityEngineeringRequired _
            & ", isRDRequired: " & isRDRequired _
            & ", ProductDevelopmentCommodityTeamMemberID: " & ProductDevelopmentCommodityTeamMemberID _
            & ", PurchasingFamilyTeamMemberID: " & PurchasingFamilyTeamMemberID _
            & ", PurchasingMakeTeamMemberID: " & PurchasingMakeTeamMemberID _
            & ", isMaterialSizeChange: " & isMaterialSizeChange _
            & ", isContinuousLine: " & isContinuousLine _
            & ", isCapitalRequired: " & isCapitalRequired _
            & ", isMeetingRequired: " & isMeetingRequired _
            & ", isCostReduction: " & isCostReduction _
            & ", CopyReason: " & CopyReason _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFD : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFD : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertRFD = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub CopyRFDChildPart(ByVal CopyType As String, ByVal NewRFDNo As Integer, ByVal OldRFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_RFD_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If CopyType Is Nothing Then
                CopyType = "N"
            End If

            myCommand.Parameters.Add("@CopyType", SqlDbType.VarChar)
            myCommand.Parameters("@CopyType").Value = CopyType

            myCommand.Parameters.Add("@NewRFDNo", SqlDbType.Int)
            myCommand.Parameters("@NewRFDNo").Value = NewRFDNo

            myCommand.Parameters.Add("@OldRFDNo", SqlDbType.Int)
            myCommand.Parameters("@OldRFDNo").Value = OldRFDNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CopyType: " & CopyType _
            & ", NewRFDNo: " & NewRFDNo _
            & ", OldRFDNo: " & OldRFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyRFDCustomerProgram(ByVal NewRFDNo As Integer, ByVal OldRFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_RFD_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewRFDNo", SqlDbType.Int)
            myCommand.Parameters("@NewRFDNo").Value = NewRFDNo

            myCommand.Parameters.Add("@OldRFDNo", SqlDbType.Int)
            myCommand.Parameters("@OldRFDNo").Value = OldRFDNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewRFDNo: " & NewRFDNo _
            & ", OldRFDNo: " & OldRFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyRFDFacilityDept(ByVal NewRFDNo As Integer, ByVal OldRFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_RFD_Facility_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewRFDNo", SqlDbType.Int)
            myCommand.Parameters("@NewRFDNo").Value = NewRFDNo

            myCommand.Parameters.Add("@OldRFDNo", SqlDbType.Int)
            myCommand.Parameters("@OldRFDNo").Value = OldRFDNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewRFDNo: " & NewRFDNo _
            & ", OldRFDNo: " & OldRFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyRFDVendor(ByVal NewRFDNo As Integer, ByVal OldRFDNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_RFD_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewRFDNo", SqlDbType.Int)
            myCommand.Parameters("@NewRFDNo").Value = NewRFDNo

            myCommand.Parameters.Add("@OldRFDNo", SqlDbType.Int)
            myCommand.Parameters("@OldRFDNo").Value = OldRFDNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewRFDNo: " & NewRFDNo _
            & ", OldRFDNo: " & OldRFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDApprovalStatus(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", SubscriptionID: " & SubscriptionID & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub InsertRFDGeneralComments(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer, _
    '    ByVal TeamMemberID As Integer, ByVal Comments As String)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_RFD_General_Comments"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
    '        myCommand.Parameters("@RFDNo").Value = RFDNo

    '        myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
    '        myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

    '        myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
    '        myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

    '        If Comments Is Nothing Then
    '            Comments = ""
    '        End If

    '        myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
    '        myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo: " & RFDNo _
    '        & ", SubscriptionID: " & SubscriptionID & ", TeamMemberID: " & TeamMemberID _
    '        & ", Comments: " & Comments _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    Public Shared Sub InsertRFDHistory(ByVal RFDNo As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

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
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", ActionTakenTMID:" & ActionTakenTMID _
            & ", ActionDesc:" & ActionDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDHistory : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteRFDApprovalStatus(ByVal RFDNo As Integer, ByVal SubscriptionID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_RFD_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            'myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetRFDSupportingDoc(ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDSupportingDoc")
            GetRFDSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetRFDSupportingDocList(ByVal RFDNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_RFD_Supporting_Doc_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RFDSupportingDocList")
            GetRFDSupportingDocList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetRFDSupportingDocList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertRFDSupportingDoc(ByVal RFDNo As Integer, _
        ByVal SupportingDocName As String, ByVal SupportingDocDesc As String, _
        ByVal DocBytes As Byte(), ByVal EncodeType As String, _
        ByVal FileSize As Integer, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If SupportingDocName Is Nothing Then
                SupportingDocName = ""
            End If

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = SupportingDocName

            If SupportingDocDesc Is Nothing Then
                SupportingDocDesc = ""
            End If

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = Replace(SupportingDocDesc, "'", "`")

            myCommand.Parameters.Add("@supportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@supportingDocBinary").Value = DocBytes

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewRFDSupportingDoc")
            InsertRFDSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", SupportingDocName: " & SupportingDocName _
            & ", SupportingDocDesc: " & SupportingDocDesc _
            & ", EncodeType: " & EncodeType _
            & ", FileSize: " & FileSize _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertRFDSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertRFDNetworkFileReference(ByVal RFDNo As Integer, ByVal FilePath As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Network_File"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@FilePath", SqlDbType.VarChar)
            myCommand.Parameters("@FilePath").Value = FilePath

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", FilePath: " & FilePath _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDNetworkFileReference : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDNetworkFileReference : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateRFDCustomerProgram(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal isCustomerApprovalRequired As Boolean, ByVal CustomerApprovalDate As String, _
        ByVal CustomerApprovalNo As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer, ByVal SOPDate As String, _
        ByVal EOPDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_RFD_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@isCustomerApprovalRequired", SqlDbType.Int)
            myCommand.Parameters("@isCustomerApprovalRequired").Value = isCustomerApprovalRequired

            myCommand.Parameters.Add("@CustomerApprovalDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerApprovalDate").Value = CustomerApprovalDate

            myCommand.Parameters.Add("@CustomerApprovalNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerApprovalNo").Value = CustomerApprovalNo

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@SOPDate", SqlDbType.VarChar)
            myCommand.Parameters("@SOPDate").Value = SOPDate

            myCommand.Parameters.Add("@EOPDate", SqlDbType.VarChar)
            myCommand.Parameters("@EOPDate").Value = EOPDate

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", CustomerApprovalDate: " & CustomerApprovalDate & ", ProgramID: " & ProgramID _
            & ", SOPDate: " & SOPDate & ", EOPDate: " & EOPDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDCustomerProgram(ByVal RFDNo As Integer, ByVal isCustomerApprovalRequired As Boolean, ByVal CustomerApprovalDate As String, _
    ByVal CustomerApprovalNo As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer, ByVal SOPDate As String, _
    ByVal EOPDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@isCustomerApprovalRequired", SqlDbType.Int)
            myCommand.Parameters("@isCustomerApprovalRequired").Value = isCustomerApprovalRequired

            myCommand.Parameters.Add("@CustomerApprovalDate", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerApprovalDate").Value = CustomerApprovalDate

            myCommand.Parameters.Add("@CustomerApprovalNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerApprovalNo").Value = CustomerApprovalNo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@SOPDate", SqlDbType.VarChar)
            myCommand.Parameters("@SOPDate").Value = SOPDate

            myCommand.Parameters.Add("@EOPDate", SqlDbType.VarChar)
            myCommand.Parameters("@EOPDate").Value = EOPDate

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", CustomerApprovalDate: " & CustomerApprovalDate & ", ProgramID: " & ProgramID _
            & ", SOPDate: " & SOPDate & ", EOPDate: " & EOPDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDFacilityDept(ByVal RFDNo As Integer, ByVal UGNFacility As String, ByVal DepartmentID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Facility_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", UGNFacility: " & UGNFacility _
            & ", DepartmentID: " & DepartmentID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDProcess(ByVal RFDNo As Integer, ByVal ProcessNotes As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_Process"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            If ProcessNotes Is Nothing Then
                ProcessNotes = ""
            End If

            myCommand.Parameters.Add("@ProcessNotes", SqlDbType.VarChar)
            myCommand.Parameters("@ProcessNotes").Value = Replace(ProcessNotes, "'", "`") 'commonFunctions.convertSpecialChar(ProcessNotes, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", ProcessNotes: " & ProcessNotes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub InsertRFDCapital(ByVal RFDNo As Integer, ByVal CapitalNotes As String)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_RFD_Capital"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
    '        myCommand.Parameters("@RFDNo").Value = RFDNo

    '        If CapitalNotes Is Nothing Then
    '            CapitalNotes = ""
    '        End If

    '        myCommand.Parameters.Add("@CapitalNotes", SqlDbType.VarChar)
    '        myCommand.Parameters("@CapitalNotes").Value = commonFunctions.convertSpecialChar(CapitalNotes, False)

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo: " & RFDNo _
    '        & ", CapitalNotes: " & CapitalNotes _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertRFDCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Sub InsertRFDTooling(ByVal RFDNo As Integer, ByVal ToolingNotes As String)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_RFD_Tooling"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
    '        myCommand.Parameters("@RFDNo").Value = RFDNo

    '        If ToolingNotes Is Nothing Then
    '            ToolingNotes = ""
    '        End If

    '        myCommand.Parameters.Add("@ToolingNotes", SqlDbType.VarChar)
    '        myCommand.Parameters("@ToolingNotes").Value = commonFunctions.convertSpecialChar(ToolingNotes, False)

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", ToolingNotes: " & ToolingNotes _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDTooling : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertRFDTooling : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    Public Shared Sub InsertRFDRSS(ByVal RFDNo As Integer, ByVal TeamMemberID As Integer, _
      ByVal SubscriptionID As Integer, ByVal Comment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

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
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", TeamMemberID:" & TeamMemberID & ", SubscriptionID:" & SubscriptionID _
            & ", Comment:" & Comment & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDRSS : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertRFDRSSReply(ByVal RFDNo As Integer, ByVal RSSID As Integer, ByVal TeamMemberID As Integer, ByVal Comment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_RFD_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

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
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", RSSID: " & RSSID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", Comment:" & Comment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDRSSReply : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

End Class
