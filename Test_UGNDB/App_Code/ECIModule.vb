''************************************************************************************************
''Name:		ECIModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the ECI Module
''
''Date		    Author	    
''06/25/2009    Roderick Carlson			Created  
''10/15/2009    Roderick Carlson            Modified    Added Copy_ECI_Notification_Group function
''11/17/2009    Roderick Carlson            Modified    ECI-2771 - Added Insert_ECI_History Function
''11/23/2009    Roderick Carlson            Modified    ECI-2776 - Added PPAP Level to Insert and Update ECI
''07/09/2010    Roderick Carlson            Modified    PE-2909 - added sp_Update_Drawing_ECI for ReleaseTypeID
''07/20/2010    Roderick Carlson            Modified    Added sp_Insert_ECI_CAR
''11/22/2010    Roderick Carlson            Modified    Change search to be in sp_Get_ECI_Search instead of sp_Get_ECI, made sp_Get_ECI more specific
''12/06/2011    Roderick Carlson            Modified    Allow more supporting document types
''11/19/2012    Roderick Carlson            Modified    Remove GetECITrackingSummary and UpdateDrawingFromECI
' 02/18/2013    Roderick Carlson            Modified    Aded ECI Initiator List
''************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports Microsoft.VisualBasic

Public Class ECIModule
    Public Shared Sub CleanECICrystalReports()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Preview
            If HttpContext.Current.Session("ECIPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("ECIPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ECIPreview") = Nothing
                HttpContext.Current.Session("ECIPreviewECINo") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("UgnIppPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("UgnIppPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("UgnIppPreviewECINo") = Nothing
                HttpContext.Current.Session("UgnIppPreview") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("CustomerIppPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("CustomerIppPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("CustomerIppPreviewECINo") = Nothing
                HttpContext.Current.Session("CustomerIppPreview") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("ECITrackingDetailPreview") IsNot Nothing Then                
                tempRpt = CType(HttpContext.Current.Session("ECITrackingDetailPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ECITrackingDetailPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanECICrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanECICrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeleteECICookies()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            HttpContext.Current.Response.Cookies("ECIModule_SaveECINoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveECINoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveECIDescSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveECIDescSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveECITypeSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveECITypeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveIssueDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveIssueDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveImplementationDateSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveImplementationDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveRFDNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveRFDNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveCostSheetIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveCostSheetIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveDrawingNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveBPCSPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveBPCSPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveCustomerSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveCustomerPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveCustomerPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveDesignLevelSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveDesignLevelSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveDesignationTypeSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveProgramIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveCommodityIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SavePurchasedGoodIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveProductTechnologyIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveSubFamilyIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveSubFamilyIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveAccountManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveQualityEngineerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterPPAPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterPPAPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveIsPPAPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveIsPPAPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterUgnIPPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterUgnIPPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveIsUgnIPPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveIsUgnIPPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveIsCustomerIPPSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveIsCustomerIPPSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("ECIModule_SaveIncludeArchiveSearch").Value = 0
            HttpContext.Current.Response.Cookies("ECIModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteECICookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECICookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Function GetECI(ByVal ECINo As Integer) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIDetails")
            GetECI = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECI: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> ECIModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECI : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetECI = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECIInitiator() As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Initiator"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIInitiator")
            GetECIInitiator = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIInitiator: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> ECIModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIInitiator: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetECIInitiator = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
  
    Public Shared Function GetECISearch(ByVal ECINo As String, ByVal ECIDesc As String, ByVal ECIType As String, ByVal StatusID As Integer, _
      ByVal IssueDate As String, ByVal ImplementationDate As String, ByVal RFDNo As String, ByVal CostSheetID As String, _
      ByVal InitiatorTeamMemberID As Integer, ByVal DrawingNo As String, ByVal PartNo As String, _
      ByVal PartName As String, ByVal Customer As String, ByVal CustomerPartNo As String, _
      ByVal DesignLevel As String, ByVal DesignationType As String, ByVal BusinessProcessTypeID As Integer, _
      ByVal ProgramID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, _
      ByVal ProductTechnologyID As Integer, ByVal SubFamilyID As Integer, ByVal UGNFacility As String, ByVal UGNDBVendorID As Integer, _
      ByVal AccountManagerID As Integer, ByVal QualityEngineerID As Integer, _
      ByVal filterPPAP As Boolean, ByVal isPPAP As Boolean, ByVal filterUgnIPP As Boolean, ByVal isUgnIPP As Boolean, _
      ByVal filterCustomerIPP As Boolean, ByVal isCustomerIPP As Boolean, _
      ByVal LastUpdatedOnStartDate As String, ByVal LastUpdatedOnEndDate As String, ByVal includeArchive As Boolean) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.VarChar)
            myCommand.Parameters("@ECINo").Value = ECINo

            If ECIDesc Is Nothing Then
                ECIDesc = ""
            End If

            myCommand.Parameters.Add("@ECIDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ECIDesc").Value = ECIDesc

            If ECIType Is Nothing Then
                ECIType = ""
            End If

            myCommand.Parameters.Add("@ECIType", SqlDbType.VarChar)
            myCommand.Parameters("@ECIType").Value = ECIType

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

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

            myCommand.Parameters.Add("@RFDNo", SqlDbType.VarChar)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = commonFunctions.convertSpecialChar(PartNo, False)

            If PartName Is Nothing Then
                PartName = ""
            End If

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = commonFunctions.convertSpecialChar(PartName, False)

            If Customer Is Nothing Then
                Customer = ""
            End If

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = commonFunctions.convertSpecialChar(CustomerPartNo, False)

            If DesignLevel Is Nothing Then
                DesignLevel = ""
            End If

            myCommand.Parameters.Add("@DesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@DesignLevel").Value = commonFunctions.convertSpecialChar(DesignLevel, False)

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@UGNDBVendorID", SqlDbType.Int)
            myCommand.Parameters("@UGNDBVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@filterPPAP", SqlDbType.Bit)
            myCommand.Parameters("@filterPPAP").Value = filterPPAP

            myCommand.Parameters.Add("@isPPAP", SqlDbType.Bit)
            myCommand.Parameters("@isPPAP").Value = isPPAP

            myCommand.Parameters.Add("@isUgnIPP", SqlDbType.Bit)
            myCommand.Parameters("@isUgnIPP").Value = isUgnIPP

            myCommand.Parameters.Add("@filterUgnIPP", SqlDbType.Bit)
            myCommand.Parameters("@filterUgnIPP").Value = filterUgnIPP

            myCommand.Parameters.Add("@isCustomerIPP", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerIPP").Value = isCustomerIPP

            myCommand.Parameters.Add("@filterCustomerIPP", SqlDbType.Bit)
            myCommand.Parameters("@filterCustomerIPP").Value = filterCustomerIPP

            If LastUpdatedOnStartDate Is Nothing Then
                LastUpdatedOnStartDate = ""
            End If

            myCommand.Parameters.Add("@LastUpdatedOnStartDate", SqlDbType.VarChar)
            myCommand.Parameters("@LastUpdatedOnStartDate").Value = LastUpdatedOnStartDate

            If LastUpdatedOnEndDate Is Nothing Then
                LastUpdatedOnEndDate = ""
            End If

            myCommand.Parameters.Add("@LastUpdatedOnEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@LastUpdatedOnEndDate").Value = LastUpdatedOnEndDate

            myCommand.Parameters.Add("@includeArchive", SqlDbType.Bit)
            myCommand.Parameters("@includeArchive").Value = includeArchive

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECISearchList")
            GetECISearch = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo " & ECINo _
            & ", ECIDesc " & ECIDesc _
            & ", ECIType " & ECIType _
            & ", StatusID " & StatusID _
            & ", IssueDate " & IssueDate _
            & ", ImplementationDate " & ImplementationDate _
            & ", RFDNo " & RFDNo _
            & ", CostSheetID " & CostSheetID _
            & ", InitiatorTeamMemberID " & InitiatorTeamMemberID _
            & ", DrawingNo " & DrawingNo _
            & ", PartNo " & PartNo _
            & ", PartName " & PartName _
            & ", Customer " & Customer _
            & ", CustomerPartNo " & CustomerPartNo _
            & ", DesignLevel " & DesignLevel _
            & ", DesignationType " & DesignationType _
            & ", BusinessProcessTypeID " & BusinessProcessTypeID _
            & ", ProgramID " & ProgramID _
            & ", CommodityID " & CommodityID _
            & ", PurchasedGoodID " & PurchasedGoodID _
            & ", ProductTechnologyID " & ProductTechnologyID _
            & ", SubFamilyID " & SubFamilyID _
            & ", UGNDBVendorID " & UGNDBVendorID _
            & ", AccountManagerID " & AccountManagerID _
            & ", QualityEngineerID " & QualityEngineerID _
            & ", filterPPAP " & filterPPAP & ", isPPAP " & isPPAP _
            & ", filterUgnIPP " & filterUgnIPP & ", isUgnIPP " & isUgnIPP _
            & ", filterCustomerIPP " & filterCustomerIPP & ", isCustomerIPP " & isCustomerIPP _
            & ", lastUpdatedOnStartDate " & LastUpdatedOnStartDate _
            & ", lastUpdatedOnEndDate " & LastUpdatedOnEndDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECISearch: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> ECIModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECISearch : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetECISearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetECIGroup(ByVal GroupID As Integer, ByVal GroupName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            If GroupName Is Nothing Then
                GroupName = ""
            End If

            myCommand.Parameters.Add("@groupName", SqlDbType.VarChar)
            myCommand.Parameters("@groupName").Value = GroupName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIGroup")
            GetECIGroup = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID _
            & ", GroupName: " & GroupName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIGroup = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetECINotificationGroup(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Notification_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECINotificationGroup")
            GetECINotificationGroup = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECINotificationGroup = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetECIBPCSParentPartsAffected(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_BPCS_Parent_Parts_Affected"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIBPCSParentPartsAffected")
            GetECIBPCSParentPartsAffected = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIBPCSParentPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIBPCSParentPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIBPCSParentPartsAffected = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetECICustomerProgram(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetECICustomerProgram")
            GetECICustomerProgram = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECICustomerProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECIGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Group_Team_Member"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIGroupTeamMember")
            GetECIGroupTeamMember = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIGroupTeamMember = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECITask(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Task"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@eciNo", SqlDbType.Int)
            myCommand.Parameters("@eciNo").Value = ECINo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECITask")
            GetECITask = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECITask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECITask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECITask = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECIStatus(ByVal StatusID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@statusID", SqlDbType.Int)
            myCommand.Parameters("@statusID").Value = StatusID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIStatus")
            GetECIStatus = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function CopyECIGroup(ByVal GroupID As Integer) As Boolean

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Dim bResult As Boolean = False

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

            bResult = True

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECIGroup : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

        CopyECIGroup = bResult

    End Function

    Public Shared Function GetECISupportingDoc(ByVal RowID As Integer) As DataSet

        'ByVal ECINo As Integer, ByVal SupportingDocName As String

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            'If SupportingDocName Is Nothing Then
            '    SupportingDocName = ""
            'End If

            'myCommand.Parameters.Add("@supportingDocName", SqlDbType.VarChar)
            'myCommand.Parameters("@supportingDocName").Value = SupportingDocName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECISupportingDoc")
            GetECISupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECISupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECISupportingDocList(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Supporting_Doc_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECISupportingDocList")
            GetECISupportingDocList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECISupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECISupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECISupportingDocList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetECITaskDesc(ByVal TaskID As Integer, ByVal TaskName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Task_Desc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@taskID", SqlDbType.Int)
            myCommand.Parameters("@taskID").Value = TaskID

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            myCommand.Parameters.Add("@taskName", SqlDbType.VarChar)
            myCommand.Parameters("@taskName").Value = TaskName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetECITaskDesc")
            GetECITaskDesc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID: " & TaskID _
            & ", TaskName: " & TaskName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECITaskDesc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetECIVendor(ByVal ECINo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIVendor")
            GetECIVendor = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIVendor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub InsertECIHistory(ByVal ECINo As Integer, ByVal HistoryDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            If HistoryDesc Is Nothing Then
                HistoryDesc = ""
            End If

            myCommand.Parameters.Add("@HistoryDesc", SqlDbType.VarChar)
            myCommand.Parameters("@HistoryDesc").Value = Replace(HistoryDesc, "<BR>", "", 1, -1, CompareMethod.Text)

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", HistoryDesc: " & HistoryDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function InsertECI(ByVal PreviousECINo As Integer, ByVal ECIType As String, ByVal StatusID As Integer, _
    ByVal ECIDesc As String, ByVal ImplementationDate As String, _
    ByVal RFDNo As Integer, ByVal CostSheetID As Integer, ByVal InitiatorTeamMemberID As Integer, ByVal CurrentDrawingNo As String, _
    ByVal NewDrawingNo As String, ByVal CurrentBPCSPartNo As String, ByVal NewBPCSPartNo As String, ByVal CurrentBPCSPartRevision As String, _
    ByVal NewBPCSPartRevision As String, ByVal NewBPCSPartName As String, ByVal CurrentCustomerPartNo As String, ByVal NewCustomerPartNo As String, _
    ByVal CurrentDesignLevel As String, ByVal NewDesignLevel As String, _
    ByVal CurrentCustomerDrawingNo As String, ByVal NewCustomerDrawingNo As String, _
    ByVal DesignationType As String, ByVal BusinessProcessTypeID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, _
    ByVal ProductTechnologyID As Integer, ByVal SubFamilyID As Integer, ByVal AccountManagerID As Integer, ByVal QualityEngineerID As Integer, _
    ByVal isPPAP As Boolean, ByVal PPAPLevel As Integer, ByVal ProductionStatus As String, _
    ByVal isUgnIPP As Boolean, ByVal isCustomerIPP As Boolean, ByVal IPPDesc As String, ByVal IPPDate As String, _
    ByVal DesignDesc As String, ByVal InternalRequirement As String, ByVal PurchasingComment As String, ByVal VendorRequirement As String, _
    ByVal ExistingMaterialActionID As Integer, ByVal NotificationGroupID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PreviousECINo", SqlDbType.Int)
            myCommand.Parameters("@PreviousECINo").Value = PreviousECINo

            If ECIType Is Nothing Then
                ECIType = "Internal"
            End If

            myCommand.Parameters.Add("@ECIType", SqlDbType.VarChar)
            myCommand.Parameters("@ECIType").Value = ECIType

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            If ECIDesc Is Nothing Then
                ECIDesc = ""
            End If

            myCommand.Parameters.Add("@ECIDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ECIDesc").Value = commonFunctions.convertSpecialChar(ECIDesc, False)

            If ImplementationDate Is Nothing Then
                ImplementationDate = ""
            End If

            myCommand.Parameters.Add("@ImplementationDate", SqlDbType.VarChar)
            myCommand.Parameters("@ImplementationDate").Value = ImplementationDate

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

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

            If CurrentBPCSPartNo Is Nothing Then
                CurrentBPCSPartNo = ""
            End If

            myCommand.Parameters.Add("@CurrentBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentBPCSPartNo").Value = commonFunctions.convertSpecialChar(CurrentBPCSPartNo, False)

            If NewBPCSPartNo Is Nothing Then
                NewBPCSPartNo = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartNo").Value = commonFunctions.convertSpecialChar(NewBPCSPartNo, False)

            If CurrentBPCSPartRevision Is Nothing Then
                CurrentBPCSPartRevision = ""
            End If

            myCommand.Parameters.Add("@CurrentBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentBPCSPartRevision").Value = commonFunctions.convertSpecialChar(CurrentBPCSPartRevision, False)

            If NewBPCSPartRevision Is Nothing Then
                NewBPCSPartRevision = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartRevision").Value = commonFunctions.convertSpecialChar(NewBPCSPartRevision, False)

            If NewBPCSPartName Is Nothing Then
                NewBPCSPartName = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartName").Value = commonFunctions.convertSpecialChar(NewBPCSPartName, False)

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

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            'myCommand.Parameters.Add("@isCustomerApproval", SqlDbType.Bit)
            'myCommand.Parameters("@isCustomerApproval").Value = isCustomerApproval

            'If CustomerApprovalDate Is Nothing Then
            '    CustomerApprovalDate = ""
            'End If

            'myCommand.Parameters.Add("@CustomerApprovalDate", SqlDbType.VarChar)
            'myCommand.Parameters("@CustomerApprovalDate").Value = CustomerApprovalDate

            myCommand.Parameters.Add("@isPPAP", SqlDbType.Bit)
            myCommand.Parameters("@isPPAP").Value = isPPAP

            myCommand.Parameters.Add("@PPAPLevel", SqlDbType.Int)
            myCommand.Parameters("@PPAPLevel").Value = PPAPLevel

            If ProductionStatus Is Nothing Then
                ProductionStatus = ""
            End If

            myCommand.Parameters.Add("@ProductionStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProductionStatus").Value = ProductionStatus

            myCommand.Parameters.Add("@isUgnIPP", SqlDbType.Bit)
            myCommand.Parameters("@isUgnIPP").Value = isUgnIPP

            myCommand.Parameters.Add("@isCustomerIPP", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerIPP").Value = isCustomerIPP

            If IPPDesc Is Nothing Then
                IPPDesc = ""
            End If

            myCommand.Parameters.Add("@IPPDesc", SqlDbType.VarChar)
            myCommand.Parameters("@IPPDesc").Value = commonFunctions.convertSpecialChar(IPPDesc, False)

            If IPPDate Is Nothing Then
                IPPDate = ""
            End If

            myCommand.Parameters.Add("@IPPDate", SqlDbType.VarChar)
            myCommand.Parameters("@IPPDate").Value = IPPDate

            If DesignDesc Is Nothing Then
                DesignDesc = ""
            End If

            myCommand.Parameters.Add("@DesignDesc", SqlDbType.VarChar)
            myCommand.Parameters("@DesignDesc").Value = commonFunctions.convertSpecialChar(DesignDesc, False)

            If InternalRequirement Is Nothing Then
                InternalRequirement = ""
            End If

            myCommand.Parameters.Add("@InternalRequirement", SqlDbType.VarChar)
            myCommand.Parameters("@InternalRequirement").Value = commonFunctions.convertSpecialChar(InternalRequirement, False)

            If PurchasingComment Is Nothing Then
                PurchasingComment = ""
            End If

            myCommand.Parameters.Add("@PurchasingComment", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingComment").Value = commonFunctions.convertSpecialChar(PurchasingComment, False)

            If VendorRequirement Is Nothing Then
                VendorRequirement = ""
            End If

            myCommand.Parameters.Add("@VendorRequirement", SqlDbType.VarChar)
            myCommand.Parameters("@VendorRequirement").Value = commonFunctions.convertSpecialChar(VendorRequirement, False)

            myCommand.Parameters.Add("@ExistingMaterialActionID", SqlDbType.Int)
            myCommand.Parameters("@ExistingMaterialActionID").Value = ExistingMaterialActionID

            myCommand.Parameters.Add("@NotificationGroupID", SqlDbType.Int)
            myCommand.Parameters("@NotificationGroupID").Value = NotificationGroupID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewECI")
            InsertECI = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PreviousECINo: " & PreviousECINo & ", ECIType: " & ECIType _
            & ", StatusID: " & StatusID & ", ECIDesc: " & ECIDesc _
            & ", ImplementationDate: " & ImplementationDate _
            & ", RFDNo: " & RFDNo & ", CostSheetID: " & CostSheetID _
            & ", InitiatorTeamMemberID: " & InitiatorTeamMemberID _
            & ", CurrentDrawingNo: " & CurrentDrawingNo _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", CurrentBPCSPartNo: " & CurrentBPCSPartNo _
            & ", NewBPCSPartNo: " & NewBPCSPartNo _
            & ", CurrentBPCSPartRevision: " & CurrentBPCSPartRevision _
            & ", NewBPCSPartRevision: " & NewBPCSPartRevision _
            & ", NewBPCSPartName: " & NewBPCSPartName _
            & ", CurrentCustomerPartNo: " & CurrentCustomerPartNo _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", CurrentDesignLevel: " & CurrentDesignLevel _
            & ", NewDesignLevel: " & NewDesignLevel _
            & ", CurrentCustomerDrawingNo: " & CurrentCustomerDrawingNo _
            & ", NewCustomerDrawingNo: " & NewCustomerDrawingNo _
            & ", DesignationType: " & DesignationType _
            & ", BusinessProcessTypeID: " & BusinessProcessTypeID & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID & ", ProductTechnologyID: " & ProductTechnologyID _
            & ", SubFamilyID: " & SubFamilyID & ", AccountManagerID: " & AccountManagerID _
            & ", QualityEngineerID: " & QualityEngineerID _
            & ", isPPAP: " & isPPAP & ", PPAPLevel: " & PPAPLevel _
            & ", ProductionStatus: " & ProductionStatus & ", isUgnIPP: " & isUgnIPP _
            & ", isCustomerIPP: " & isCustomerIPP & ", IPPDesc: " & IPPDesc _
            & ", IPPDate: " & IPPDate & ", DesignDesc: " & DesignDesc & ", InternalRequirement: " & InternalRequirement _
            & ", VendorRequirement: " & VendorRequirement _
            & ", PurchasingComment: " & PurchasingComment & ", ExistingMaterialActionID: " & ExistingMaterialActionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertECI = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateECI(ByVal ECINo As Integer, ByVal ECIType As String, _
    ByVal ECIDesc As String, ByVal ImplementationDate As String, _
    ByVal RFDNo As Integer, ByVal CostSheetID As Integer, ByVal InitiatorTeamMemberID As Integer, ByVal CurrentDrawingNo As String, _
    ByVal NewDrawingNo As String, ByVal CurrentBPCSPartNo As String, ByVal NewBPCSPartNo As String, ByVal CurrentBPCSPartRevision As String, _
    ByVal NewBPCSPartRevision As String, ByVal NewBPCSPartName As String, _
    ByVal CurrentCustomerPartNo As String, ByVal NewCustomerPartNo As String, _
    ByVal CurrentDesignLevel As String, ByVal NewDesignLevel As String, _
    ByVal CurrentCustomerDrawingNo As String, ByVal NewCustomerDrawingNo As String, _
    ByVal DesignationType As String, ByVal BusinessProcessTypeID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, _
    ByVal ProductTechnologyID As Integer, ByVal SubFamilyID As Integer, ByVal AccountManagerID As Integer, ByVal QualityEngineerID As Integer, _
    ByVal isPPAP As Boolean, ByVal PPAPLevel As Integer, ByVal ProductionStatus As String, _
    ByVal isUgnIPP As Boolean, ByVal isCustomerIPP As Boolean, ByVal IPPDesc As String, ByVal IPPDate As String, _
    ByVal DesignDesc As String, ByVal InternalRequirement As String, ByVal PurchasingComment As String, ByVal VendorRequirement As String, _
    ByVal ExistingMaterialActionID As Integer, ByVal NotificationGroupID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ECI"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            If ECIType Is Nothing Then
                ECIType = "Internal"
            End If

            myCommand.Parameters.Add("@ECIType", SqlDbType.VarChar)
            myCommand.Parameters("@ECIType").Value = ECIType

            If ECIDesc Is Nothing Then
                ECIDesc = ""
            End If

            myCommand.Parameters.Add("@ECIDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ECIDesc").Value = commonFunctions.convertSpecialChar(ECIDesc, False)

            If ImplementationDate Is Nothing Then
                ImplementationDate = ""
            End If

            myCommand.Parameters.Add("@ImplementationDate", SqlDbType.VarChar)
            myCommand.Parameters("@ImplementationDate").Value = ImplementationDate

            myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
            myCommand.Parameters("@RFDNo").Value = RFDNo

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@InitiatorTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@InitiatorTeamMemberID").Value = InitiatorTeamMemberID

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

            If CurrentBPCSPartNo Is Nothing Then
                CurrentBPCSPartNo = ""
            End If

            myCommand.Parameters.Add("@CurrentBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentBPCSPartNo").Value = commonFunctions.convertSpecialChar(CurrentBPCSPartNo, False)

            If NewBPCSPartNo Is Nothing Then
                NewBPCSPartNo = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartNo").Value = commonFunctions.convertSpecialChar(NewBPCSPartNo, False)

            If CurrentBPCSPartRevision Is Nothing Then
                CurrentBPCSPartRevision = ""
            End If

            myCommand.Parameters.Add("@CurrentBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@CurrentBPCSPartRevision").Value = commonFunctions.convertSpecialChar(CurrentBPCSPartRevision, False)

            If NewBPCSPartRevision Is Nothing Then
                NewBPCSPartRevision = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartRevision").Value = commonFunctions.convertSpecialChar(NewBPCSPartRevision, False)

            If NewBPCSPartName Is Nothing Then
                NewBPCSPartName = ""
            End If

            myCommand.Parameters.Add("@NewBPCSPartName", SqlDbType.VarChar)
            myCommand.Parameters("@NewBPCSPartName").Value = commonFunctions.convertSpecialChar(NewBPCSPartName, False)

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

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = commonFunctions.convertSpecialChar(DesignationType, False)

            myCommand.Parameters.Add("@BusinessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@BusinessProcessTypeID").Value = BusinessProcessTypeID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@AccountManagerID", SqlDbType.Int)
            myCommand.Parameters("@AccountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@QualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngineerID").Value = QualityEngineerID

            'myCommand.Parameters.Add("@isCustomerApproval", SqlDbType.Bit)
            'myCommand.Parameters("@isCustomerApproval").Value = isCustomerApproval

            'If CustomerApprovalDate Is Nothing Then
            '    CustomerApprovalDate = ""
            'End If

            'myCommand.Parameters.Add("@CustomerApprovalDate", SqlDbType.VarChar)
            'myCommand.Parameters("@CustomerApprovalDate").Value = CustomerApprovalDate

            myCommand.Parameters.Add("@isPPAP", SqlDbType.Bit)
            myCommand.Parameters("@isPPAP").Value = isPPAP

            myCommand.Parameters.Add("@PPAPLevel", SqlDbType.Int)
            myCommand.Parameters("@PPAPLevel").Value = PPAPLevel

            If ProductionStatus Is Nothing Then
                ProductionStatus = ""
            End If

            myCommand.Parameters.Add("@ProductionStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ProductionStatus").Value = ProductionStatus

            myCommand.Parameters.Add("@isUgnIPP", SqlDbType.Bit)
            myCommand.Parameters("@isUgnIPP").Value = isUgnIPP

            myCommand.Parameters.Add("@isCustomerIPP", SqlDbType.Bit)
            myCommand.Parameters("@isCustomerIPP").Value = isCustomerIPP

            If IPPDesc Is Nothing Then
                IPPDesc = ""
            End If

            myCommand.Parameters.Add("@IPPDesc", SqlDbType.VarChar)
            myCommand.Parameters("@IPPDesc").Value = commonFunctions.convertSpecialChar(IPPDesc, False)

            If IPPDate Is Nothing Then
                IPPDate = ""
            End If

            myCommand.Parameters.Add("@IPPDate", SqlDbType.VarChar)
            myCommand.Parameters("@IPPDate").Value = IPPDate

            If DesignDesc Is Nothing Then
                DesignDesc = ""
            End If

            myCommand.Parameters.Add("@DesignDesc", SqlDbType.VarChar)
            myCommand.Parameters("@DesignDesc").Value = commonFunctions.convertSpecialChar(DesignDesc, False)

            If InternalRequirement Is Nothing Then
                InternalRequirement = ""
            End If

            myCommand.Parameters.Add("@InternalRequirement", SqlDbType.VarChar)
            myCommand.Parameters("@InternalRequirement").Value = commonFunctions.convertSpecialChar(InternalRequirement, False)

            If PurchasingComment Is Nothing Then
                PurchasingComment = ""
            End If

            myCommand.Parameters.Add("@PurchasingComment", SqlDbType.VarChar)
            myCommand.Parameters("@PurchasingComment").Value = commonFunctions.convertSpecialChar(PurchasingComment, False)

            If VendorRequirement Is Nothing Then
                VendorRequirement = ""
            End If

            myCommand.Parameters.Add("@VendorRequirement", SqlDbType.VarChar)
            myCommand.Parameters("@VendorRequirement").Value = commonFunctions.convertSpecialChar(VendorRequirement, False)


            myCommand.Parameters.Add("@ExistingMaterialActionID", SqlDbType.Int)
            myCommand.Parameters("@ExistingMaterialActionID").Value = ExistingMaterialActionID

            myCommand.Parameters.Add("@NotificationGroupID", SqlDbType.Int)
            myCommand.Parameters("@NotificationGroupID").Value = NotificationGroupID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", ECIType: " & ECIType _
            & ", ECIDesc: " & ECIDesc & ", ImplementationDate: " & ImplementationDate _
            & ", RFDNo: " & RFDNo & ", CostSheetID: " & CostSheetID _
            & ", InitiatorTeamMemberID: " & InitiatorTeamMemberID & ", CurrentDrawingNo: " & CurrentDrawingNo _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", NewDesignLevel: " & NewDesignLevel & ", NewDrawingNo: " & NewDrawingNo _
            & ", CurrentBPCSPartNo: " & CurrentBPCSPartNo & ", NewBPCSPartNo: " & NewBPCSPartNo _
            & ", CurrentBPCSPartRevision: " & CurrentBPCSPartRevision & ", NewBPCSPartRevision: " & NewBPCSPartRevision _
            & ", NewBPCSPartName: " & NewBPCSPartName & ", CurrentCustomerPartNo: " & CurrentCustomerPartNo _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", CurrentDesignLevel: " & CurrentDesignLevel & ", NewDesignLevel: " & NewDesignLevel _
            & ", CurrentCustomerDrawingNo: " & CurrentCustomerDrawingNo & ", NewCustomerDrawingNo: " & NewCustomerDrawingNo _
            & ", DesignationType: " & DesignationType _
            & ", BusinessProcessTypeID: " & BusinessProcessTypeID & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID & ", ProductTechnologyID: " & ProductTechnologyID _
            & ", SubFamilyID: " & SubFamilyID & ", AccountManagerID: " & AccountManagerID _
            & ", QualityEngineerID: " & QualityEngineerID _
            & ", isPPAP: " & isPPAP & ", PPAPLevel: " & PPAPLevel _
            & ", ProductionStatus: " & ProductionStatus & ", isUgnIPP: " & isUgnIPP _
            & ", isCustomerIPP: " & isCustomerIPP & ", IPPDesc: " & IPPDesc _
            & ", IPPDate: " & IPPDate & ", DesignDesc: " & DesignDesc & ", InternalRequirement: " & InternalRequirement _
            & ", VendorRequirement: " & VendorRequirement _
            & ", PurchasingComment: " & PurchasingComment & ", ExistingMaterialActionID: " & ExistingMaterialActionID _
            & ", NotificationGroupID: " & NotificationGroupID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    'Public Shared Sub UpdateDrawingFromECI(ByVal DrawingNo As String, ByVal CostSheetID As Integer, ByVal RFDNo As Integer, _
    'ByVal ECINo As Integer, ByVal DesignationType As String, ByVal CustomerPartNo As String, _
    'ByVal SubFamilyID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, ByVal ProductTechnologyID As Integer)

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Update_Drawing_From_ECI"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    HttpContext.Current.Session("BLLerror") = Nothing

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        If DrawingNo Is Nothing Then
    '            DrawingNo = ""
    '        End If

    '        myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
    '        myCommand.Parameters("@DrawingNo").Value = DrawingNo

    '        myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
    '        myCommand.Parameters("@CostSheetID").Value = CostSheetID

    '        myCommand.Parameters.Add("@RFDNo", SqlDbType.Int)
    '        myCommand.Parameters("@RFDNo").Value = RFDNo

    '        myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
    '        myCommand.Parameters("@ECINo").Value = ECINo

    '        If DesignationType Is Nothing Then
    '            DesignationType = ""
    '        End If

    '        myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
    '        myCommand.Parameters("@DesignationType").Value = DesignationType

    '        If CustomerPartNo Is Nothing Then
    '            CustomerPartNo = ""
    '        End If

    '        myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
    '        myCommand.Parameters("@CustomerPartNo").Value = CustomerPartNo

    '        myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
    '        myCommand.Parameters("@SubFamilyID").Value = SubFamilyID

    '        myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
    '        myCommand.Parameters("@CommodityID").Value = CommodityID

    '        myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
    '        myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGoodID

    '        myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
    '        myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

    '        myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", RFDNo: " & RFDNo _
    '        & ", RFDNo: " & RFDNo _
    '        & ", DesignationType: " & DesignationType _
    '        & ", CommodityID: " & CommodityID _
    '        & ", PurchasedGoodID: " & PurchasedGoodID & ", ProductTechnologyID: " & ProductTechnologyID _
    '        & ", SubFamilyID: " & SubFamilyID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateDrawingFromECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateDrawingFromECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub

    Public Shared Sub UpdateDrawingECI(ByVal DrawingNo As String, ByVal ECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_ECI"
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

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateECIRelease(ByVal ECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ECI_Release"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetECIExistingMaterialAction(ByVal ActionID As Integer, ByVal ActionName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ECI_Existing_Material_Action"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@actionID", SqlDbType.Int)
            myCommand.Parameters("@actionID").Value = ActionID

            myCommand.Parameters.Add("@actionName", SqlDbType.VarChar)
            myCommand.Parameters("@actionName").Value = ActionName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ECIExistingMaterialAction")

            GetECIExistingMaterialAction = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ActionID: " & ActionID & ", ActionName: " & ActionName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIExistingMaterialAction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIExistingMaterialAction : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetECIExistingMaterialAction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertECISupportingDoc(ByVal ECINo As Integer, ByVal SupportingDocName As String, _
        ByVal SupportingDocDesc As String, ByVal SupportingDocBinary As Byte(), _
        ByVal SupportingDocBinarySizeInBytes As Integer, ByVal SupportingDocEncodeType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = commonFunctions.convertSpecialChar(SupportingDocName, False)

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = commonFunctions.convertSpecialChar(SupportingDocDesc, False)

            myCommand.Parameters.Add("@SupportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@SupportingDocBinary").Value = SupportingDocBinary

            myCommand.Parameters.Add("@SupportingDocBinarySizeInBytes", SqlDbType.Int)
            myCommand.Parameters("@SupportingDocBinarySizeInBytes").Value = SupportingDocBinarySizeInBytes

            myCommand.Parameters.Add("@SupportingDocEncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocEncodeType").Value = SupportingDocEncodeType

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewECISupportingDoc")
            InsertECISupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", SupportingDocName: " & SupportingDocName _
            & ", SupportingDocDesc: " & SupportingDocDesc _
            & ", SupportingDocBinarySizeInBytes: " & SupportingDocBinarySizeInBytes _
            & ", SupportingDocEncodeType: " & SupportingDocEncodeType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertECISupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertECICustomerProgram(ByVal ECINo As Integer, ByVal isCustomerApprovalRequired As Boolean, ByVal CustomerApprovalDate As String, _
    ByVal CustomerApprovalNo As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer, ByVal SOPDate As String, _
    ByVal EOPDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

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
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", CustomerApprovalDate: " & CustomerApprovalDate & ", ProgramID: " & ProgramID _
            & ", SOPDate: " & SOPDate & ", EOPDate: " & EOPDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateECICustomerProgram(ByVal RowID As Integer, ByVal isCustomerApprovalRequired As Boolean, ByVal CustomerApprovalDate As String, _
  ByVal CustomerApprovalNo As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer, ByVal SOPDate As String, _
  ByVal EOPDate As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_ECI_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

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
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", isCustomerApprovalRequired: " & isCustomerApprovalRequired _
            & ", CustomerApprovalDate: " & CustomerApprovalDate & ", ProgramID: " & ProgramID _
            & ", SOPDate: " & SOPDate & ", EOPDate: " & EOPDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub


    Public Shared Sub InsertECICAR(ByVal ECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_CAR"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECICAR : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECICAR : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertECIFacilityDept(ByVal ECINo As Integer, ByVal UGNFacility As String, ByVal DepartmentID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_Facility_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

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
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", UGNFacility: " & UGNFacility _
            & ", DepartmentID: " & DepartmentID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertECINotification(ByVal ECINo As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_Notification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECINotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECINotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyECICustomerProgram(ByVal NewECINo As Integer, ByVal OldECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewECINo", SqlDbType.Int)
            myCommand.Parameters("@NewECINo").Value = NewECINo

            myCommand.Parameters.Add("@OldECINo", SqlDbType.Int)
            myCommand.Parameters("@OldECINo").Value = OldECINo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewECINo: " & NewECINo & ", OldECINo: " & OldECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyECIFacilityDept(ByVal NewECINo As Integer, ByVal OldECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Facility_Dept"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewECINo", SqlDbType.Int)
            myCommand.Parameters("@NewECINo").Value = NewECINo

            myCommand.Parameters.Add("@OldECINo", SqlDbType.Int)
            myCommand.Parameters("@OldECINo").Value = OldECINo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewECINo: " & NewECINo & ", OldECINo: " & OldECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyECINotificationGroup(ByVal NewECINo As Integer, ByVal OldECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Notification_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewECINo", SqlDbType.Int)
            myCommand.Parameters("@NewECINo").Value = NewECINo

            myCommand.Parameters.Add("@OldECINo", SqlDbType.Int)
            myCommand.Parameters("@OldECINo").Value = OldECINo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewECINo: " & NewECINo & ", OldECINo: " & OldECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyECIVendor(ByVal NewECINo As Integer, ByVal OldECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewECINo", SqlDbType.Int)
            myCommand.Parameters("@NewECINo").Value = NewECINo

            myCommand.Parameters.Add("@OldECINo", SqlDbType.Int)
            myCommand.Parameters("@OldECINo").Value = OldECINo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewECINo: " & NewECINo & ", OldECINo: " & OldECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyECITask(ByVal NewECINo As Integer, ByVal OldECINo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_ECI_Task"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewECINo", SqlDbType.Int)
            myCommand.Parameters("@NewECINo").Value = NewECINo

            myCommand.Parameters.Add("@OldECINo", SqlDbType.Int)
            myCommand.Parameters("@OldECINo").Value = OldECINo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewECINo: " & NewECINo & ", OldECINo: " & OldECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyECITask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyECITask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteECI(ByVal ECINo As Integer, ByVal VoidComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_ECI"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@VoidComment", SqlDbType.VarChar)
            myCommand.Parameters("@VoidComment").Value = commonFunctions.convertSpecialChar(VoidComment, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", VoidComment: " & VoidComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECI : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECI : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertECIBPCSPartsAffected(ByVal ECINo As Integer, ByVal ChildPartNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_ECI_BPCS_Parent_Parts_Affected"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ECINo", SqlDbType.Int)
            myCommand.Parameters("@ECINo").Value = ECINo

            myCommand.Parameters.Add("@ChildPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@ChildPartNo").Value = ChildPartNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo & ", ChildPartNo: " & ChildPartNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIBPCSPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIBPCSPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

End Class
