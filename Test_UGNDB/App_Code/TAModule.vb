''************************************************************************************************
''Name:		TAModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the ECI Module
''
''Date		    Author	 
''12/9/2011     Roderick Carlson    Created 
''02/13/2014    LRey     Replace SoldTo|CABBV with Customer.
''************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class TAModule

    Public Shared Sub CleanTACrystalReports()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("ToolingAuthPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("ToolingAuthPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ToolingAuthPreview") = Nothing
                HttpContext.Current.Session("ToolingAuthoPreviewTANo") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("DieBoardPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("DieBoardPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DieBoardPreviewTANo") = Nothing
                HttpContext.Current.Session("DieBoardPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanTACrystalReports: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanTACrystalReports: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeleteTACookies()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTAProjectNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTAProjectNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTADescSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveTADescSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveRFDNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveRFDNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveCostSheetIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveCostSheetIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SavePartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SavePartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveDesignLevelSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveDesignLevelSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveCustomerSearch").Value = ""
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Value = 0
            HttpContext.Current.Response.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteTACookies: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTACookies: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GetTASearch(ByVal TAProjectNo As String, ByVal StatusID As Integer, ByVal TADesc As String, ByVal PartName As String, ByVal RFDNo As String, ByVal CostSheetID As String, ByVal PartNo As String, ByVal DesignLevel As String, ByVal InitiatorTeamMemberID As Integer, ByVal QualityEngineerID As Integer, ByVal AccountManagerID As Integer, ByVal ProgramManagerID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal ProgramID As Integer, ByVal IncludeArchive As Boolean) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TAProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@TAProjectNo").Value = IIf(TAProjectNo Is Nothing, "", TAProjectNo)

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@TADesc", SqlDbType.VarChar)
            myCommand.Parameters("@TADesc").Value = commonFunctions.convertSpecialChar(IIf(TADesc Is Nothing, "", TADesc), False)

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = commonFunctions.convertSpecialChar(IIf(PartName Is Nothing, "", PartName), False)

            myCommand.Parameters.Add("@RFDNo", SqlDbType.VarChar)
            myCommand.Parameters("@RFDNo").Value = commonFunctions.convertSpecialChar(IIf(RFDNo Is Nothing, "", RFDNo), False)

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = commonFunctions.convertSpecialChar(IIf(CostSheetID Is Nothing, "", CostSheetID), False)

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = commonFunctions.convertSpecialChar(IIf(PartNo Is Nothing, "", PartNo), False)

            myCommand.Parameters.Add("@DesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@DesignLevel").Value = commonFunctions.convertSpecialChar(IIf(DesignLevel Is Nothing, "", DesignLevel), False)

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = IIf(UGNFacility Is Nothing, "", UGNFacility)

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = IIf(Customer Is Nothing, "", Customer)

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@IncludeArchive", SqlDbType.Bit)
            myCommand.Parameters("@IncludeArchive").Value = IncludeArchive

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TASearchList")
            GetTASearch = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TAProjectNo: " & TAProjectNo _
            & ", StatusID: " & StatusID _
            & ", TADesc: " & TADesc _
            & ", PartName: " & PartName _
            & ", RFDNo: " & RFDNo _
            & ", CostSheetID: " & CostSheetID _
            & ", PartNo " & PartNo _
            & ", DesignLevel " & DesignLevel _
            & ", InitiatorTeamMemberID " & InitiatorTeamMemberID _
            & ", QualityEngineerID " & QualityEngineerID _
            & ", AccountManagerID " & AccountManagerID _
            & ", ProgramManagerID " & ProgramManagerID _
            & ", UGNFaciity " & UGNFacility _
            & ", ProgramID " & ProgramID _
            & ", IncludeArchive " & IncludeArchive _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTASearch: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTASearch: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTASearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTAHistory(ByVal TANo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetTAHistory")
            GetTAHistory = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAHistory: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAHistory: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTAHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertTAHistory(ByVal TANo As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

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
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", ActionTakenTMID:" & ActionTakenTMID _
            & ", ActionDesc:" & ActionDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTAHistory: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTAHistory: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetTA(ByVal TANo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            'If TAProjectNo Is Nothing Then
            '    TAProjectNo = ""
            'End If

            'myCommand.Parameters.Add("@TAProjectNo", SqlDbType.VarChar)
            'myCommand.Parameters("@TAProjectNo").Value = TAProjectNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TADetail")

            GetTA = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTA: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTA: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTA = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTAStatusMaint() As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Status_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TAStatusMaint")
            GetTAStatusMaint = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAStatusMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAStatusMaint: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTAStatusMaint = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTAChangeTypeMaint(ByVal ChangeTypeID As Integer, ByVal ChangeTypeName As String) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Change_Type_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChangeTypeID", SqlDbType.Int)
            myCommand.Parameters("@ChangeTypeID").Value = ChangeTypeID

            If ChangeTypeName Is Nothing Then
                ChangeTypeName = ""
            End If

            myCommand.Parameters.Add("@ChangeTypeName", SqlDbType.VarChar)
            myCommand.Parameters("@ChangeTypeName").Value = ChangeTypeName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TAChangeTypeMaint")
            GetTAChangeTypeMaint = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChangeTypeID: " & ChangeTypeID _
            & ", ChangeTypeName: " & ChangeTypeName _
            & ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAChangeTypeMaint: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTAChangeTypeMaint = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTACustomerProgram(ByVal TANo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TACustomerProgram")
            GetTACustomerProgram = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTACustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTACustomerProgram: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTACustomerProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertTACustomerProgram(ByVal TANo As Integer, ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

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
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", ProgramID: " & ProgramID _
            & ", ProgramYear: " & ProgramYear _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTACustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTACustomerProgram: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteTA(ByVal TANo As Integer, ByVal VoidComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_TA"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@VoidComment", SqlDbType.VarChar)
            myCommand.Parameters("@VoidComment").Value = commonFunctions.convertSpecialChar(VoidComment, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", VoidComment: " & VoidComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTA: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTA: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteTACustomerProgram(ByVal RowID As Integer, ByVal original_RowID As Integer)

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_TA_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTACustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTACustomerProgram: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function InsertTA(ByVal StatusID As Integer, _
    ByVal ChangeTypeID As Integer, ByVal DueDate As String, _
    ByVal IssueDate As String, ByVal ImplementationDate As String, _
    ByVal AccountManagerID As Integer, ByVal InitiatorTeamMemberID As Integer, _
    ByVal ProgramManagerID As Integer, ByVal QualityEngineerID As Integer, _
    ByVal RFDNo As Integer, ByVal CostSheetID As Integer, ByVal TADesc As String, _
    ByVal ChargeOther As String, ByVal UGNFacility As String, _
    ByVal Instructions As String, ByVal Rules As String, _
    ByVal SerialNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ChangeTypeID", SqlDbType.Int)
            myCommand.Parameters("@ChangeTypeID").Value = ChangeTypeID

            If DueDate Is Nothing Then
                DueDate = ""
            End If

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            If IssueDate Is Nothing Then
                IssueDate = ""
            End If

            myCommand.Parameters.Add("@IssueDate", SqlDbType.VarChar)
            myCommand.Parameters("@IssueDate").Value = IssueDate

            If ImplementationDate Is Nothing Then
                ImplementationDate = ""
            End If

            myCommand.Parameters.Add("@ImplementationDate", SqlDbType.VarChar)
            myCommand.Parameters("@ImplementationDate").Value = ImplementationDate

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            If TADesc Is Nothing Then
                TADesc = ""
            End If

            myCommand.Parameters.Add("@TADesc", SqlDbType.VarChar)
            myCommand.Parameters("@TADesc").Value = Replace(TADesc, "'", "`") 'commonFunctions.convertSpecialChar(ChangeDescription, False)

            If ChargeOther Is Nothing Then
                ChargeOther = ""
            End If

            myCommand.Parameters.Add("@ChargeOther", SqlDbType.VarChar)
            myCommand.Parameters("@ChargeOther").Value = ChargeOther

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If Instructions Is Nothing Then
                Instructions = ""
            End If

            myCommand.Parameters.Add("@Instructions", SqlDbType.VarChar)
            myCommand.Parameters("@Instructions").Value = Instructions

            If Rules Is Nothing Then
                Rules = ""
            End If

            myCommand.Parameters.Add("@Rules", SqlDbType.VarChar)
            myCommand.Parameters("@Rules").Value = Rules


            If SerialNo Is Nothing Then
                SerialNo = ""
            End If

            myCommand.Parameters.Add("@SerialNo", SqlDbType.VarChar)
            myCommand.Parameters("@SerialNo").Value = SerialNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewTA")
            InsertTA = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID _
            & ", ChangeTypeID: " & ChangeTypeID _
            & ", DueDate: " & DueDate _
            & ", IssueDate: " & IssueDate _
            & ", ImplementationDate: " & ImplementationDate _
            & ", AccountManagerID: " & AccountManagerID _
            & ", InitiatorTeamMemberID: " & InitiatorTeamMemberID _
            & ", ProgramManagerID: " & ProgramManagerID _
            & ", QualityEngineerID: " & QualityEngineerID _
            & ", RFDNo: " & RFDNo _
            & ", CostSheetID: " & CostSheetID _
            & ", TADesc: " & TADesc _
            & ", ChargeOther: " & ChargeOther _
            & ", UGNFacility: " & UGNFacility _
            & ", Instructions: " & Instructions _
            & ", Rules: " & Rules _
            & ", SerialNo: " & SerialNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTA: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTA: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTA = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateTA(ByVal TANo As Integer, _
        ByVal StatusID As Integer, _
        ByVal ChangeTypeID As Integer, ByVal DueDate As String, _
        ByVal ImplementationDate As String, _
        ByVal AccountManagerID As Integer, ByVal InitiatorTeamMemberID As Integer, _
        ByVal ProgramManagerID As Integer, ByVal QualityEngineerID As Integer, _
        ByVal RFDNo As Integer, ByVal CostSheetID As Integer, ByVal TADesc As String, _
        ByVal ChargeOther As String, ByVal UGNFacility As String, _
        ByVal Instructions As String, ByVal Rules As String, _
        ByVal SerialNo As String, ByVal isDieshopComplete As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_TA"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@ChangeTypeID", SqlDbType.Int)
            myCommand.Parameters("@ChangeTypeID").Value = ChangeTypeID

            If DueDate Is Nothing Then
                DueDate = ""
            End If

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            If ImplementationDate Is Nothing Then
                ImplementationDate = ""
            End If

            myCommand.Parameters.Add("@ImplementationDate", SqlDbType.VarChar)
            myCommand.Parameters("@ImplementationDate").Value = ImplementationDate

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

            myCommand.Parameters.Add("@ProgramManagerID", SqlDbType.Int)
            myCommand.Parameters("@ProgramManagerID").Value = ProgramManagerID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            If TADesc Is Nothing Then
                TADesc = ""
            End If

            myCommand.Parameters.Add("@TADesc", SqlDbType.VarChar)
            myCommand.Parameters("@TADesc").Value = Replace(TADesc, "'", "`") 'commonFunctions.convertSpecialChar(ChangeDescription, False)

            If ChargeOther Is Nothing Then
                ChargeOther = ""
            End If

            myCommand.Parameters.Add("@ChargeOther", SqlDbType.VarChar)
            myCommand.Parameters("@ChargeOther").Value = ChargeOther

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            If Instructions Is Nothing Then
                Instructions = ""
            End If

            myCommand.Parameters.Add("@Instructions", SqlDbType.VarChar)
            myCommand.Parameters("@Instructions").Value = Instructions

            If Rules Is Nothing Then
                Rules = ""
            End If

            myCommand.Parameters.Add("@Rules", SqlDbType.VarChar)
            myCommand.Parameters("@Rules").Value = Rules

            myCommand.Parameters.Add("@isDieshopComplete", SqlDbType.Bit)
            myCommand.Parameters("@isDieshopComplete").Value = isDieshopComplete

            If SerialNo Is Nothing Then
                SerialNo = ""
            End If

            myCommand.Parameters.Add("@SerialNo", SqlDbType.VarChar)
            myCommand.Parameters("@SerialNo").Value = SerialNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", StatusID: " & StatusID _
            & ", ChangeTypeID: " & ChangeTypeID _
            & ", DueDate: " & DueDate _
            & ", ImplementationDate: " & ImplementationDate _
            & ", AccountManagerID: " & AccountManagerID _
            & ", InitiatorTeamMemberID: " & InitiatorTeamMemberID _
            & ", ProgramManagerID: " & ProgramManagerID _
            & ", QualityEngineerID: " & QualityEngineerID _
            & ", RFDNo: " & RFDNo _
            & ", CostSheetID: " & CostSheetID _
            & ", TADesc: " & TADesc _
            & ", ChargeOther: " & ChargeOther _
            & ", UGNFacility: " & UGNFacility _
            & ", Instructions: " & Instructions _
            & ", Rules: " & Rules _
            & ", isDieshopComplete: " & isDieshopComplete _
            & ", SerialNo: " & SerialNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTA: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTA: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub UpdateTADieshop(ByVal DSID As Integer, ByVal TANo As Integer, _
    '    ByVal Instructions As String, ByVal Rules As String, _
    '    ByVal SerialNo As String, ByVal isComplete As Boolean)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Update_TA_Die_Shop"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@DSID", SqlDbType.Int)
    '        myCommand.Parameters("@DSID").Value = DSID

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        If Instructions Is Nothing Then
    '            Instructions = ""
    '        End If

    '        myCommand.Parameters.Add("@Instructions", SqlDbType.VarChar)
    '        myCommand.Parameters("@Instructions").Value = Instructions

    '        If Rules Is Nothing Then
    '            Rules = ""
    '        End If

    '        myCommand.Parameters.Add("@Rules", SqlDbType.VarChar)
    '        myCommand.Parameters("@Rules").Value = Rules

    '        myCommand.Parameters.Add("@isComplete", SqlDbType.Bit)
    '        myCommand.Parameters("@isComplete").Value = isComplete

    '        If SerialNo Is Nothing Then
    '            SerialNo = ""
    '        End If

    '        myCommand.Parameters.Add("@SerialNo", SqlDbType.VarChar)
    '        myCommand.Parameters("@SerialNo").Value = SerialNo

    '        myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DSID: " & DSID _
    '        & ", TANo: " & TANo _
    '        & ", Instructions: " & Instructions _
    '        & ", Rules: " & Rules _
    '        & ", isComplete: " & isComplete _
    '        & ", SerialNo: " & SerialNo _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateTADieshop: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateTADieshop: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    Public Shared Function GetTAFinishedPart(ByVal TANo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Finished_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TAFinishedPart")
            GetTAFinishedPart = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAFinishedPart: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTAFinishedPart = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTAChildPart(ByVal TANo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TAChildPart")
            GetTAChildPart = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAChildPart: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTAChildPart = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTAInitiator() As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Initiator"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TAInitiator")
            GetTAInitiator = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAInitiator: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAInitiator: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTAInitiator = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTATaskTeamMember() As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Task_TeamMember"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TATaskTeamMember")
            GetTATaskTeamMember = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTATaskTeamMember: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTATaskTeamMember: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTATaskTeamMember = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub DeleteTAFinishedPart(ByVal RowID As Integer, ByVal original_RowID As Integer)

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_TA_Finished_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTAFinishedPart: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteTAChildPart(ByVal RowID As Integer, ByVal original_RowID As Integer)

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_TA_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTAChildPart: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetTASupportingDoc(ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Supporting_Doc"
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
            myAdapter.Fill(GetData, "TASupportingDoc")
            GetTASupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTASupportingDoc: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Tooling_AuthorizationModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTASupportingDoc: " & commonFunctions.convertSpecialChar(ex.Message, False), "Tooling_AuthorizationModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTASupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTASupportingDocList(ByVal TANo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Supporting_Doc_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TASupportingDocList")
            GetTASupportingDocList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTASupportingDocList: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Tooling_AuthorizationModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTASupportingDocList: " & commonFunctions.convertSpecialChar(ex.Message, False), "Tooling_AuthorizationModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTASupportingDocList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertTASupportingDoc(ByVal TANo As Integer, ByVal SupportingDocName As String, _
        ByVal SupportingDocDesc As String, ByVal SupportingDocBinary As Byte(), _
        ByVal BinaryFileSizeInBytes As Integer, ByVal EncodeType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = commonFunctions.convertSpecialChar(SupportingDocName, False)

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = Replace(SupportingDocDesc, "'", "`") 'commonFunctions.convertSpecialChar(SupportingDocDesc, False)

            myCommand.Parameters.Add("@SupportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@SupportingDocBinary").Value = SupportingDocBinary

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = EncodeType

            myCommand.Parameters.Add("@BinaryFileSizeInBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeInBytes").Value = BinaryFileSizeInBytes

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewTASupportingDoc")
            InsertTASupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", SupportingDocName: " & SupportingDocName _
            & ", SupportingDocDesc: " & SupportingDocDesc _
            & ", BinaryFileSizeInBytes: " & BinaryFileSizeInBytes _
            & ", EncodeType: " & EncodeType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTASupportingDoc: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTASupportingDoc: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTASupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub DeleteTASupportingDoc(ByVal RowID As Integer, ByVal original_RowID As Integer)

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_TA_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTASupportingDoc: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTASupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub InsertToolingAuthorizationApprovalStatus(ByVal TANo As Integer, ByVal SubscriptionID As Integer, ByVal TeamMemberID As Integer)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_ToolingAuthorization_Approval_Status"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
    '        myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

    '        myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
    '        myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", SubscriptionID: " & SubscriptionID _
    '        & ", TeamMemberID: " & TeamMemberID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertToolingAuthorizationApprovalStatus: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertToolingAuthorizationApprovalStatus: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    Public Shared Sub InsertTAFinishedPart(ByVal TANo As Integer, _
        ByVal CurrentCustomerPartNo As String, ByVal CurrentCustomerPartName As String, _
        ByVal CurrentInternalPartNo As String, ByVal CurrentDesignLevel As String, ByVal CurrentDrawingNo As String, _
        ByVal NewCustomerPartNo As String, ByVal NewCustomerPartName As String, ByVal NewInternalPartNo As String, _
        ByVal NewDesignLevel As String, ByVal NewDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_Finished_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            If CurrentCustomerPartNo Is Nothing Then
                CurrentCustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CurrentCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentCustomerPartNo").Value = CurrentCustomerPartNo

            If CurrentCustomerPartName Is Nothing Then
                CurrentCustomerPartName = ""
            End If

            myCommand.Parameters.Add("@CurrentCustomerPartName", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentCustomerPartName").Value = commonFunctions.convertSpecialChar(IIf(CurrentCustomerPartName Is Nothing, "", CurrentCustomerPartName), False)

            If CurrentInternalPartNo Is Nothing Then
                CurrentInternalPartNo = ""
            End If

            myCommand.Parameters.Add("@CurrentInternalPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentInternalPartNo").Value = CurrentInternalPartNo

            If CurrentDesignLevel Is Nothing Then
                CurrentDesignLevel = ""
            End If

            myCommand.Parameters.Add("@CurrentDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDesignLevel").Value = CurrentDesignLevel

            If CurrentDrawingNo Is Nothing Then
                CurrentDrawingNo = ""
            End If

            myCommand.Parameters.Add("@CurrentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDrawingNo").Value = CurrentDrawingNo

            If NewCustomerPartNo Is Nothing Then
                NewCustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@NewCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewCustomerPartNo").Value = NewCustomerPartNo

            If NewCustomerPartName Is Nothing Then
                NewCustomerPartName = ""
            End If

            myCommand.Parameters.Add("@NewCustomerPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewCustomerPartName").Value = commonFunctions.convertSpecialChar(IIf(NewCustomerPartName Is Nothing, "", NewCustomerPartName), False)

            If NewInternalPartNo Is Nothing Then
                NewInternalPartNo = ""
            End If

            myCommand.Parameters.Add("@NewInternalPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewInternalPartNo").Value = NewInternalPartNo

            If NewDesignLevel Is Nothing Then
                NewDesignLevel = ""
            End If

            myCommand.Parameters.Add("@NewDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@NewDesignLevel").Value = NewDesignLevel

            If NewDrawingNo Is Nothing Then
                NewDrawingNo = ""
            End If

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", CurrentCustomerPartNo: " & CurrentCustomerPartNo _
            & ", CurrentCustomerPartName: " & CurrentCustomerPartName _
            & ", CurrentInternalPartNo: " & CurrentInternalPartNo _
            & ", CurrentDesignLevel: " & CurrentDesignLevel _
            & ", CurrentDrawingNo: " & CurrentDrawingNo _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", NewCustomerPartName: " & NewCustomerPartName _
            & ", NewInternalPartNo: " & NewInternalPartNo _
            & ", NewDesignLevel: " & NewDesignLevel _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertTAChildPart(ByVal TANo As Integer, _
     ByVal CurrentPartNo As String, ByVal CurrentPartName As String, ByVal CurrentDrawingNo As String, ByVal NewPartNo As String, ByVal NewPartName As String, ByVal NewDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@CurrentPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartNo").Value = IIf(CurrentPartNo Is Nothing, "", CurrentPartNo)

            myCommand.Parameters.Add("@CurrentPartName", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentPartName").Value = commonFunctions.convertSpecialChar(IIf(CurrentPartName Is Nothing, "", CurrentPartName), False)
            myCommand.Parameters.Add("@CurrentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentDrawingNo").Value = CurrentDrawingNo

            myCommand.Parameters.Add("@NewPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartNo").Value = NewPartNo

            myCommand.Parameters.Add("@NewPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewPartName").Value = commonFunctions.convertSpecialChar(IIf(NewPartName Is Nothing, "", NewPartName), False)

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", CurrentBPCSPartNo: " & CurrentPartNo _
            & ", CurrentBPCSPartName: " & CurrentPartName _
            & ", CurrentDrawingNo: " & CurrentDrawingNo _
            & ", NewBPCSPartNo: " & NewPartNo _
            & ", NewBPCSPartName: " & NewPartName _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Function GetToolingAuthorizationApproval(ByVal TANo As Integer, ByVal SubscriptionID As Integer, ByVal TeamMemberID As Integer, _
    ' ByVal filterNotified As Boolean, ByVal isNotified As Boolean, ByVal isHistorical As Boolean, ByVal filterWorking As Boolean, _
    ' ByVal isWorking As Boolean) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Tooling_Authorization_Approval"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
    '        myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

    '        myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
    '        myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

    '        myCommand.Parameters.Add("@filterNotified", SqlDbType.Bit)
    '        myCommand.Parameters("@filterNotified").Value = filterNotified

    '        myCommand.Parameters.Add("@isNotified", SqlDbType.Bit)
    '        myCommand.Parameters("@isNotified").Value = isNotified

    '        myCommand.Parameters.Add("@isHistorical", SqlDbType.Bit)
    '        myCommand.Parameters("@isHistorical").Value = isHistorical

    '        myCommand.Parameters.Add("@filterWorking", SqlDbType.Bit)
    '        myCommand.Parameters("@filterWorking").Value = filterWorking

    '        myCommand.Parameters.Add("@isWorking", SqlDbType.Bit)
    '        myCommand.Parameters("@isWorking").Value = isWorking

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ToolingAuthorizationApproval")
    '        GetToolingAuthorizationApproval = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", SubscriptionID: " & SubscriptionID _
    '        & ", TeamMemberID: " & TeamMemberID _
    '        & ", filterNotified: " & filterNotified _
    '        & ", isNotified: " & isNotified _
    '        & ", isHistorical: " & isHistorical _
    '        & ", filterWorking: " & filterWorking _
    '        & ", isWorking: " & isWorking _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetToolingAuthorizationApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetToolingAuthorizationApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetToolingAuthorizationApproval = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function GetToolingAuthorizationRSS(ByVal TANo As Integer, ByVal RSSID As Integer) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Tooling_Authorization_RSS"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
    '        myCommand.Parameters("@RSSID").Value = RSSID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ToolingAuthorizationRSS")
    '        GetToolingAuthorizationRSS = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", RSSID: " & RSSID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetToolingAuthorizationRSS: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetToolingAuthorizationRSS: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetToolingAuthorizationRSS = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    'Public Shared Function GetToolingAuthorizationRSSReply(ByVal TANo As Integer, ByVal RSSID As Integer) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Tooling_Authorization_RSS_Reply"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
    '        myCommand.Parameters("@RSSID").Value = RSSID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "ToolingAuthorizationRSSReply")
    '        GetToolingAuthorizationRSSReply = GetData

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", RSSID: " & RSSID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetToolingAuthorizationRSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetToolingAuthorizationRSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetToolingAuthorizationRSSReply = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function InsertTARSS(ByVal TANo As Integer, ByVal TeamMemberID As Integer, _
   ByVal SubscriptionID As Integer, ByVal Comment As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            If Comment Is Nothing Then
                Comment = ""
            End If

            myCommand.Parameters.Add("@Comment", SqlDbType.VarChar)
            myCommand.Parameters("@Comment").Value = Replace(Comment, "'", "`") 'Comment

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertTARSS")
            InsertTARSS = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", Comment: " & Comment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTARSS: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTARSS: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTARSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertTARSSReply(ByVal TANo As Integer, ByVal RSSID As Integer, _
        ByVal TeamMemberID As Integer, ByVal Comment As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_TA_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            If Comment Is Nothing Then
                Comment = ""
            End If

            myCommand.Parameters.Add("@Comment", SqlDbType.VarChar)
            myCommand.Parameters("@Comment").Value = Replace(Comment, "'", "`") 'Comment

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertTARSSReply")
            InsertTARSSReply = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", RSSID: " & RSSID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", Comment: " & Comment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTARSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTARSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertTARSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTATaskMaint(ByVal TaskID As Integer, ByVal TaskName As String) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Task_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TaskID", SqlDbType.Int)
            myCommand.Parameters("@TaskID").Value = TaskID

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            myCommand.Parameters.Add("@TaskName", SqlDbType.VarChar)
            myCommand.Parameters("@TaskName").Value = TaskName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TATaskMaint")
            GetTATaskMaint = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID: " & TaskID _
            & ", TaskName: " & TaskName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTATaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTATaskMaint: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTATaskMaint = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTATask(ByVal TANo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Task"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TANo", SqlDbType.Int)
            myCommand.Parameters("@TANo").Value = TANo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TATask")
            GetTATask = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTATask: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTATask: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTATask = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Sub InsertToolingAuthorizationTeamMemberTask(ByVal TANo As Integer, ByVal TaskID As Integer, _
    '    ByVal TeamMemberID As Integer, ByVal TargetDate As String)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_Tooling_Authorization_Team_Member_Task"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@TaskID", SqlDbType.Int)
    '        myCommand.Parameters("@TaskID").Value = TaskID

    '        myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
    '        myCommand.Parameters("@TeamMemberID").Value = TaskID

    '        If TargetDate Is Nothing Then
    '            TargetDate = ""
    '        End If

    '        myCommand.Parameters.Add("@TargetDate", SqlDbType.VarChar)
    '        myCommand.Parameters("@TargetDate").Value = TargetDate

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", TaskID: " & TaskID _
    '        & ", TeamMemberID: " & TeamMemberID _
    '        & ", TargetDate: " & TargetDate _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertToolingAuthorizationTeamMemberTask: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertToolingAuthorizationTeamMemberTask : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Sub InsertToolingAuthorizationTask(ByVal TaskName As String)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_Tooling_Authorization_Task"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        If TaskName Is Nothing Then
    '            TaskName = ""
    '        End If

    '        myCommand.Parameters.Add("@TaskName", SqlDbType.VarChar)
    '        myCommand.Parameters("@TaskName").Value = TaskName

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TaskName: " & TaskName _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertToolingAuthorizationTask: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertToolingAuthorizationTask: " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Sub UpdateToolingAuthorizationTask(ByVal TaskID As Integer, ByVal TaskName As String, ByVal Obsolete As Boolean)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Update_Tooling_Authorization_Task"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TaskID", SqlDbType.Int)
    '        myCommand.Parameters("@TaskID").Value = TaskID

    '        If TaskName Is Nothing Then
    '            TaskName = ""
    '        End If

    '        myCommand.Parameters.Add("@TaskName", SqlDbType.VarChar)
    '        myCommand.Parameters("@TaskName").Value = TaskName

    '        myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
    '        myCommand.Parameters("@Obsolete").Value = Obsolete

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TaskID: " & TaskID _
    '        & ", TaskName: " & TaskName _
    '        & ", Obsolete: " & Obsolete _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateToolingAuthorizationTask: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateToolingAuthorizationTask: " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Sub InsertToolingAuthorizationChangeType(ByVal ChangeTypeName As String)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_Tooling_Authorization_Change_Type"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        If ChangeTypeName Is Nothing Then
    '            ChangeTypeName = ""
    '        End If

    '        myCommand.Parameters.Add("@ChangeTypeName", SqlDbType.VarChar)
    '        myCommand.Parameters("@ChangeTypeName").Value = ChangeTypeName

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "ChangeTypeName: " & ChangeTypeName _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertToolingAuthorizationChangeType: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertToolingAuthorizationChangeType: " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Sub UpdateToolingAuthorizationChangeType(ByVal RowID As Integer, _
    '    ByVal ChangeTypeName As String, ByVal Obsolete As Boolean)

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Update_Tooling_Authorization_Change_Type"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@RowID", SqlDbType.Int)
    '        myCommand.Parameters("@RowID").Value = RowID

    '        If ChangeTypeName Is Nothing Then
    '            ChangeTypeName = ""
    '        End If

    '        myCommand.Parameters.Add("@ChangeTypeName", SqlDbType.VarChar)
    '        myCommand.Parameters("@ChangeTypeName").Value = ChangeTypeName

    '        myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
    '        myCommand.Parameters("@Obsolete").Value = Obsolete

    '        myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowId: " & RowID _
    '        & ", ChangeTypeName: " & ChangeTypeName _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateToolingAuthorizationChangeType: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateToolingAuthorizationChangeType: " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub



    Public Shared Function GetTADSMaterialMaint(ByVal DSMaterialID As Integer, ByVal MaterialName As String) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Die_Shop_Material_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DSMaterialID", SqlDbType.Int)
            myCommand.Parameters("@DSMaterialID").Value = DSMaterialID

            If MaterialName Is Nothing Then
                MaterialName = ""
            End If

            myCommand.Parameters.Add("@MaterialName", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialName").Value = MaterialName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TADSMaterialMaint")
            GetTADSMaterialMaint = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DSMaterialID: " & DSMaterialID _
            & ", MaterialName: " & MaterialName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetToolingDSMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTADSMaterial: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTADSMaterialMaint = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetTADSLaborMaint(ByVal DSLaborID As Integer, ByVal LaborName As String) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TA_Die_Shop_Labor_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DSLaborID", SqlDbType.Int)
            myCommand.Parameters("@DSLaborID").Value = DSLaborID

            If LaborName Is Nothing Then
                LaborName = ""
            End If

            myCommand.Parameters.Add("@LaborName", SqlDbType.VarChar)
            myCommand.Parameters("@LaborName").Value = LaborName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TADSLabor")
            GetTADSLaborMaint = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DSLaborID: " & DSLaborID _
            & ", LaborName: " & LaborName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTADSLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTADSLaborMaint: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTADSLaborMaint = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub CopyTAChildPart(ByVal NewTANo As Integer, ByVal OldTANo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_TA_Child_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewTANo", SqlDbType.Int)
            myCommand.Parameters("@NewTANo").Value = NewTANo

            myCommand.Parameters.Add("@OldTANo", SqlDbType.Int)
            myCommand.Parameters("@OldTANo").Value = OldTANo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewTANo: " & NewTANo _
            & ", OldTANo: " & OldTANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyTAChildPart: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyTAFinishedPart(ByVal NewTANo As Integer, ByVal OldTANo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_TA_Finished_Part"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewTANo", SqlDbType.Int)
            myCommand.Parameters("@NewTANo").Value = NewTANo

            myCommand.Parameters.Add("@OldTANo", SqlDbType.Int)
            myCommand.Parameters("@OldTANo").Value = OldTANo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewTANo: " & NewTANo _
            & ", OldTANo: " & OldTANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyTAFinishedPart: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyTACustomerProgram(ByVal NewTANo As Integer, ByVal OldTANo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_TA_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewTANo", SqlDbType.Int)
            myCommand.Parameters("@NewTANo").Value = NewTANo

            myCommand.Parameters.Add("@OldTANo", SqlDbType.Int)
            myCommand.Parameters("@OldTANo").Value = OldTANo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewTANo: " & NewTANo _
            & ", OldTANo: " & OldTANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyTACustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyTACustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyTATask(ByVal NewTANo As Integer, ByVal OldTANo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_TA_Task"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewTANo", SqlDbType.Int)
            myCommand.Parameters("@NewTANo").Value = NewTANo

            myCommand.Parameters.Add("@OldTANo", SqlDbType.Int)
            myCommand.Parameters("@OldTANo").Value = OldTANo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewTANo: " & NewTANo _
            & ", OldTANo: " & OldTANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyTATask: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyTATask: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub InsertTADieShop(ByVal TANo As Integer)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_TA_Die_Shop"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try

    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo: " & TANo _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertTADieShop: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TAModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertTADieShop: " & commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    'Public Shared Function GetTADieShop(ByVal TANo As Integer) As DataSet

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_TA_Die_Shop"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@TANo", SqlDbType.Int)
    '        myCommand.Parameters("@TANo").Value = TANo

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "TADieShop")

    '        GetTADieShop = GetData

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TANo " & TANo _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetTADieShop: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> TAModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetTADieShop: " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        GetTADieShop = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function isTAComplete(ByVal TANo As Integer) As Boolean

        Dim bReturn As Boolean = True

        Try
            'it is complete if all tasks have a completion date

            Dim iRowCounter As Integer = 0
            Dim objTask As New ExpProjToolingAuthBLL
            Dim dtTask As DataTable

            'Dim iTaskID As Integer = 0

            dtTask = objTask.GetTATask(TANo)

            'only check if in-process if there are more tasks than just 1
            If commonFunctions.CheckDataTable(dtTask) = True Then

                For iRowCounter = 0 To dtTask.Rows.Count - 1
                    'If dtTask.Rows(iRowCounter).Item("TaskID") IsNot System.DBNull.Value Then
                    '    iTaskID = dtTask.Rows(iRowCounter).Item("TaskID")
                    'End If

                    If dtTask.Rows(iRowCounter).Item("CompletionDate").ToString = "" Then 'And iTaskID <> 10 'Complete Die Shop Cost Form
                        bReturn = False
                    End If
                Next
            Else
                bReturn = False
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "isTAComplete: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TAModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("isTAComplete: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

        Return bReturn

    End Function

End Class
