''************************************************************************************************
''Name:		SafetyModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Safety Module
''
''Date		    Author	    
''01/08/2010    Roderick Carlson			Created  
''02/28/2011    Roderick Carlson            Modified : Added isActive Column to UpdateChemicalReviewForm
''************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports Microsoft.VisualBasic

Public Class SafetyModule
    Public Shared Sub CleanChemicalReviewFormCrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("ChemRevFormPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("ChemRevFormPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ChemRevFormPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanChemicalReviewFormCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanChemicalReviewFormCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub DeleteChemicalReviewFormCookies()

        Try
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Value = ""
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormFilterActiveSearch").Value = 0
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormFilterActiveIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormisActiveSearch").Value = 0
            HttpContext.Current.Response.Cookies("SafetyModule_SaveChemRevFormisActiveSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteChemicalReviewFormCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteChemicalReviewFormCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GetChemicalReviewForm(ByVal ChemRevFormID As String, ByVal StatusID As Integer, _
        ByVal UGNFacility As String, ByVal RequestedByTeamMemberID As Integer, _
        ByVal RequestDateStart As String, ByVal RequestDateEnd As String, _
        ByVal ApprovingTeamMemberID As Integer, ByVal ProductName As String, _
        ByVal ProductManufacturer As String, ByVal PurchaseFrom As String, _
        ByVal DeptArea As String, ByVal ChemicalDesc As String, _
        ByVal filterActive As Boolean, ByVal isActive As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If ChemRevFormID Is Nothing Then
                ChemRevFormID = ""
            End If

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.VarChar)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RequestedByTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTeamMemberID").Value = RequestedByTeamMemberID

            If RequestDateStart Is Nothing Then
                RequestDateStart = ""
            End If

            myCommand.Parameters.Add("@RequestDateStart", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDateStart").Value = RequestDateStart

            If RequestDateEnd Is Nothing Then
                RequestDateEnd = ""
            End If

            myCommand.Parameters.Add("@RequestDateEnd", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDateEnd").Value = RequestDateEnd

            myCommand.Parameters.Add("@ApprovingTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@ApprovingTeamMemberID").Value = ApprovingTeamMemberID

            If ProductName Is Nothing Then
                ProductName = ""
            End If

            myCommand.Parameters.Add("@ProductName", SqlDbType.VarChar)
            myCommand.Parameters("@ProductName").Value = ProductName

            If ProductManufacturer Is Nothing Then
                ProductManufacturer = ""
            End If

            myCommand.Parameters.Add("@ProductManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@ProductManufacturer").Value = ProductManufacturer

            If PurchaseFrom Is Nothing Then
                PurchaseFrom = ""
            End If

            myCommand.Parameters.Add("@PurchaseFrom", SqlDbType.VarChar)
            myCommand.Parameters("@PurchaseFrom").Value = PurchaseFrom

            If DeptArea Is Nothing Then
                DeptArea = ""
            End If

            myCommand.Parameters.Add("@DeptArea", SqlDbType.VarChar)
            myCommand.Parameters("@DeptArea").Value = DeptArea

            If ChemicalDesc Is Nothing Then
                ChemicalDesc = ""
            End If

            myCommand.Parameters.Add("@ChemicalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ChemicalDesc").Value = ChemicalDesc

            myCommand.Parameters.Add("@filterActive", SqlDbType.Bit)
            myCommand.Parameters("@filterActive").Value = filterActive

            myCommand.Parameters.Add("@isActive", SqlDbType.Bit)
            myCommand.Parameters("@isActive").Value = isActive

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChemicalReviewForm")
            GetChemicalReviewForm = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID & ", UGNFacility: " & UGNFacility _
            & ", RequestedByTeamMemberID: " & RequestedByTeamMemberID & ", RequestDateStart: " & RequestDateStart _
            & ", RequestDateEnd: " & RequestDateEnd & ", ApprovingTeamMemberID: " & ApprovingTeamMemberID _
            & ", ProductName : " & ProductName & ", ProductManufacturer: " & ProductManufacturer _
            & ", PurchaseFrom: " & PurchaseFrom & ", DeptArea: " & DeptArea & ", ChemicalDesc: " & ChemicalDesc _
            & ", filterActive: " & filterActive & ", isActive: " & isActive _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewForm = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetChemicalReviewFormStatus(ByVal StatusID As Integer, ByVal isEditable As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@isEditable", SqlDbType.Bit)
            myCommand.Parameters("@isEditable").Value = isEditable

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChemicalReviewFormStatus")
            GetChemicalReviewFormStatus = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID & ", isEditable: " & isEditable _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormStatus : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormStatus : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewFormStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetChemicalReviewFormRequestedByTeamMembers() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form_Requested_By"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChemicalReviewFormRequested_By")
            GetChemicalReviewFormRequestedByTeamMembers = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormRequestedByTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormRequestedByTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewFormRequestedByTeamMembers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetChemicalReviewFormApprovers() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form_Approvers"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ChemicalReviewFormApprovers")
            GetChemicalReviewFormApprovers = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormApprovers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormApprovers : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewFormApprovers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateChemicalReviewForm(ByVal ChemRevFormID As Integer, ByVal UGNFacility As String, ByVal RequestedByTeamMemberID As Integer, _
        ByVal RequestDate As String, ByVal ProductName As String, ByVal ProductManufacturer As String, ByVal PurchaseFrom As String, _
        ByVal DeptArea As String, ByVal ChemicalDesc As String, ByVal isProductionUsage As Boolean, ByVal isLabUsage As Boolean, _
        ByVal isMaintenanceUsage As Boolean, ByVal isOtherUsage As Boolean, ByVal OtherUsageDesc As String, ByVal HealthLevel As Integer, _
        ByVal FlammabilityLevel As Integer, ByVal ReactivityLevel As Integer, ByVal ProtectiveEquipmentLevel As Integer, _
        ByVal isPhysicalHazard As Boolean, ByVal isHealthHazard As Boolean, ByVal isEnvironmentalHazard As Boolean, ByVal isOtherHazard As Boolean, _
        ByVal OtherHazardDesc As String, ByVal isGlovesEquip As Boolean, ByVal isGogglesEquip As Boolean, ByVal isRespiratoryEquip As Boolean, _
        ByVal isProtectiveClothingEquip As Boolean, ByVal isOtherEquip As Boolean, ByVal OtherEquipDesc As String, ByVal isVentilationEng As Boolean, _
        ByVal isContainmentEng As Boolean, ByVal isOtherEng As Boolean, ByVal OtherEngDesc As String, ByVal IncompatibleWith As String, _
        ByVal StorageDesc As String, ByVal DisposalDesc As String, ByVal isMSDSEnv As Boolean, ByVal isAspectListEnv As Boolean, _
        ByVal isEMPEnv As Boolean, ByVal AspectType As String, ByVal RnDTeamMemberID As Integer, ByVal HRSafetyTeamMemberID As Integer, _
        ByVal CorpEnvTeamMemberID As Integer, ByVal PlantEnvTeamMemberID As Integer, ByVal PurchasingTeamMemberID As Integer, _
        ByVal isActive As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Chemical_Review_Form"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RequestedByTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTeamMemberID").Value = RequestedByTeamMemberID

            If RequestDate Is Nothing Then
                RequestDate = ""
            End If

            myCommand.Parameters.Add("@RequestDate", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDate").Value = commonFunctions.convertSpecialChar(RequestDate, False)

            If ProductName Is Nothing Then
                ProductName = ""
            End If

            myCommand.Parameters.Add("@ProductName", SqlDbType.VarChar)
            myCommand.Parameters("@ProductName").Value = commonFunctions.convertSpecialChar(ProductName, False)

            If ProductManufacturer Is Nothing Then
                ProductManufacturer = ""
            End If

            myCommand.Parameters.Add("@ProductManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@ProductManufacturer").Value = commonFunctions.convertSpecialChar(ProductManufacturer, False)

            If PurchaseFrom Is Nothing Then
                PurchaseFrom = ""
            End If

            myCommand.Parameters.Add("@PurchaseFrom", SqlDbType.VarChar)
            myCommand.Parameters("@PurchaseFrom").Value = commonFunctions.convertSpecialChar(PurchaseFrom, False)

            If DeptArea Is Nothing Then
                DeptArea = ""
            End If

            myCommand.Parameters.Add("@DeptArea", SqlDbType.VarChar)
            myCommand.Parameters("@DeptArea").Value = commonFunctions.convertSpecialChar(DeptArea, False)

            If ChemicalDesc Is Nothing Then
                ChemicalDesc = ""
            End If

            myCommand.Parameters.Add("@ChemicalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ChemicalDesc").Value = commonFunctions.convertSpecialChar(ChemicalDesc, False)

            myCommand.Parameters.Add("@isProductionUsage", SqlDbType.Bit)
            myCommand.Parameters("@isProductionUsage").Value = isProductionUsage

            myCommand.Parameters.Add("@isLabUsage", SqlDbType.Bit)
            myCommand.Parameters("@isLabUsage").Value = isLabUsage

            myCommand.Parameters.Add("@isMaintenanceUsage", SqlDbType.Bit)
            myCommand.Parameters("@isMaintenanceUsage").Value = isMaintenanceUsage

            myCommand.Parameters.Add("@isOtherUsage", SqlDbType.Bit)
            myCommand.Parameters("@isOtherUsage").Value = isOtherUsage

            If OtherUsageDesc Is Nothing Then
                OtherUsageDesc = ""
            End If

            myCommand.Parameters.Add("@OtherUsageDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherUsageDesc").Value = commonFunctions.convertSpecialChar(OtherUsageDesc, False)

            myCommand.Parameters.Add("@HealthLevel", SqlDbType.Int)
            myCommand.Parameters("@HealthLevel").Value = HealthLevel

            myCommand.Parameters.Add("@FlammabilityLevel", SqlDbType.Int)
            myCommand.Parameters("@FlammabilityLevel").Value = FlammabilityLevel

            myCommand.Parameters.Add("@ReactivityLevel", SqlDbType.Int)
            myCommand.Parameters("@ReactivityLevel").Value = ReactivityLevel

            myCommand.Parameters.Add("@ProtectiveEquipmentLevel", SqlDbType.Int)
            myCommand.Parameters("@ProtectiveEquipmentLevel").Value = ReactivityLevel

            myCommand.Parameters.Add("@isPhysicalHazard", SqlDbType.Bit)
            myCommand.Parameters("@isPhysicalHazard").Value = isPhysicalHazard

            myCommand.Parameters.Add("@isHealthHazard", SqlDbType.Bit)
            myCommand.Parameters("@isHealthHazard").Value = isHealthHazard

            myCommand.Parameters.Add("@isEnvironmentalHazard", SqlDbType.Bit)
            myCommand.Parameters("@isEnvironmentalHazard").Value = isEnvironmentalHazard

            myCommand.Parameters.Add("@isOtherHazard", SqlDbType.Bit)
            myCommand.Parameters("@isOtherHazard").Value = isOtherHazard

            If OtherHazardDesc Is Nothing Then
                OtherHazardDesc = ""
            End If

            myCommand.Parameters.Add("@OtherHazardDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherHazardDesc").Value = commonFunctions.convertSpecialChar(OtherHazardDesc, False)

            myCommand.Parameters.Add("@isGlovesEquip", SqlDbType.Bit)
            myCommand.Parameters("@isGlovesEquip").Value = isGlovesEquip

            myCommand.Parameters.Add("@isGogglesEquip", SqlDbType.Bit)
            myCommand.Parameters("@isGogglesEquip").Value = isGogglesEquip

            myCommand.Parameters.Add("@isRespiratoryEquip", SqlDbType.Bit)
            myCommand.Parameters("@isRespiratoryEquip").Value = isRespiratoryEquip

            myCommand.Parameters.Add("@isProtectiveClothingEquip", SqlDbType.Bit)
            myCommand.Parameters("@isProtectiveClothingEquip").Value = isProtectiveClothingEquip

            myCommand.Parameters.Add("@isOtherEquip", SqlDbType.Bit)
            myCommand.Parameters("@isOtherEquip").Value = isOtherEquip

            If OtherEquipDesc Is Nothing Then
                OtherEquipDesc = ""
            End If

            myCommand.Parameters.Add("@OtherEquipDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherEquipDesc").Value = OtherEquipDesc

            myCommand.Parameters.Add("@isVentilationEng", SqlDbType.Bit)
            myCommand.Parameters("@isVentilationEng").Value = isVentilationEng

            myCommand.Parameters.Add("@isContainmentEng", SqlDbType.Bit)
            myCommand.Parameters("@isContainmentEng").Value = isContainmentEng

            myCommand.Parameters.Add("isOtherEng", SqlDbType.Bit)
            myCommand.Parameters("isOtherEng").Value = isOtherEng

            If OtherEngDesc Is Nothing Then
                OtherEngDesc = ""
            End If

            myCommand.Parameters.Add("@OtherEngDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherEngDesc").Value = commonFunctions.convertSpecialChar(OtherEngDesc, False)

            If IncompatibleWith Is Nothing Then
                IncompatibleWith = ""
            End If

            myCommand.Parameters.Add("@IncompatibleWith", SqlDbType.VarChar)
            myCommand.Parameters("@IncompatibleWith").Value = commonFunctions.convertSpecialChar(IncompatibleWith, False)

            If StorageDesc Is Nothing Then
                StorageDesc = ""
            End If

            myCommand.Parameters.Add("@StorageDesc", SqlDbType.VarChar)
            myCommand.Parameters("@StorageDesc").Value = commonFunctions.convertSpecialChar(StorageDesc, False)

            If DisposalDesc Is Nothing Then
                DisposalDesc = ""
            End If

            myCommand.Parameters.Add("@DisposalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@DisposalDesc").Value = commonFunctions.convertSpecialChar(DisposalDesc, False)

            myCommand.Parameters.Add("@isMSDSEnv", SqlDbType.Bit)
            myCommand.Parameters("@isMSDSEnv").Value = isMSDSEnv

            myCommand.Parameters.Add("@isAspectListEnv", SqlDbType.Bit)
            myCommand.Parameters("@isAspectListEnv").Value = isAspectListEnv

            myCommand.Parameters.Add("@isEMPEnv", SqlDbType.Bit)
            myCommand.Parameters("@isEMPEnv").Value = isEMPEnv

            If AspectType Is Nothing Then
                AspectType = ""
            End If

            myCommand.Parameters.Add("@AspectType", SqlDbType.VarChar)
            myCommand.Parameters("@AspectType").Value = AspectType

            myCommand.Parameters.Add("@RnDTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@RnDTeamMemberID").Value = RnDTeamMemberID

            myCommand.Parameters.Add("@HRSafetyTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@HRSafetyTeamMemberID").Value = HRSafetyTeamMemberID

            myCommand.Parameters.Add("@CorpEnvTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@CorpEnvTeamMemberID").Value = CorpEnvTeamMemberID

            myCommand.Parameters.Add("@PlantEnvTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PlantEnvTeamMemberID").Value = PlantEnvTeamMemberID

            myCommand.Parameters.Add("@PurchasingTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingTeamMemberID").Value = PurchasingTeamMemberID

            myCommand.Parameters.Add("@isActive", SqlDbType.Bit)
            myCommand.Parameters("@isActive").Value = isActive

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID & ", UGNFacility: " & UGNFacility _
            & ", RequestedByTeamMemberID: " & RequestedByTeamMemberID & ", RequestDate: " & RequestDate _
            & ", ProductName: " & ProductName & ", ProductManufacturer: " & ProductManufacturer _
            & ", PurchaseFrom: " & PurchaseFrom & ", DeptArea: " & DeptArea _
            & ", ChemicalDesc: " & ChemicalDesc _
            & ", isProductionUsage: " & isProductionUsage & ", isLabUsage: " & isLabUsage _
            & ", isMaintenanceUsage: " & isMaintenanceUsage & ", isOtherUsage: " & isOtherUsage _
            & ", OtherUsageDesc: " & OtherUsageDesc & ", HealthLevel: " & HealthLevel _
            & ", FlammabilityLevel: " & FlammabilityLevel & ", ReactivityLevel: " & ReactivityLevel _
            & ", ProtectiveEquipmentLevel: " & ProtectiveEquipmentLevel _
            & ", isPhysicalHazard: " & isPhysicalHazard & ", isHealthHazard: " & isHealthHazard _
            & ", isEnvironmentalHazard: " & isEnvironmentalHazard & ", isOtherHazard: " & isOtherHazard _
            & ", OtherHazardDesc: " & OtherHazardDesc _
            & ", isGlovesEquip: " & isGlovesEquip & ", isGogglesEquip: " & isGogglesEquip _
            & ", isRespiratoryEquip: " & isRespiratoryEquip & ", isProtectiveClothingEquip: " & isProtectiveClothingEquip _
            & ", isOtherEquip: " & isOtherEquip & ", OtherEquipDesc: " & OtherEquipDesc _
            & ", isVentilationEng: " & isVentilationEng _
            & ", isContainmentEng: " & isContainmentEng & ", isOtherEng: " & isOtherEng _
            & ", OtherEngDesc: " & OtherEngDesc & ", IncompatibleWith: " & IncompatibleWith _
            & ", StorageDesc: " & StorageDesc & ", DisposalDesc: " & DisposalDesc _
            & ", isMSDSEnv: " & isMSDSEnv & ", isAspectListEnv: " & isAspectListEnv & ", isEMPEnv: " & isEMPEnv _
            & ", AspectType: " & AspectType _
            & ", RnDTeamMemberID: " & RnDTeamMemberID & ", HRSafetyTeamMemberID: " & HRSafetyTeamMemberID _
            & ", CorpEnvTeamMemberID: " & CorpEnvTeamMemberID & ", PlantEnvTeamMemberID: " & PlantEnvTeamMemberID _
            & ", PurchasingTeamMemberID: " & PurchasingTeamMemberID _
            & ", isActive: " & isActive _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function InsertChemicalReviewForm(ByVal UGNFacility As String, ByVal RequestedByTeamMemberID As Integer, _
        ByVal RequestDate As String, ByVal ProductName As String, ByVal ProductManufacturer As String, ByVal PurchaseFrom As String, _
        ByVal DeptArea As String, ByVal ChemicalDesc As String, ByVal isProductionUsage As Boolean, ByVal isLabUsage As Boolean, _
        ByVal isMaintenanceUsage As Boolean, ByVal isOtherUsage As Boolean, ByVal OtherUsageDesc As String, ByVal HealthLevel As Integer, _
        ByVal FlammabilityLevel As Integer, ByVal ReactivityLevel As Integer, ByVal ProtectiveEquipmentLevel As Integer, _
        ByVal isPhysicalHazard As Boolean, ByVal isHealthHazard As Boolean, ByVal isEnvironmentalHazard As Boolean, ByVal isOtherHazard As Boolean, _
        ByVal OtherHazardDesc As String, ByVal isGlovesEquip As Boolean, ByVal isGogglesEquip As Boolean, ByVal isRespiratoryEquip As Boolean, _
        ByVal isProtectiveClothingEquip As Boolean, ByVal isOtherEquip As Boolean, ByVal OtherEquipDesc As String, ByVal isVentilationEng As Boolean, _
        ByVal isContainmentEng As Boolean, ByVal isOtherEng As Boolean, ByVal OtherEngDesc As String, ByVal IncompatibleWith As String, _
        ByVal StorageDesc As String, ByVal DisposalDesc As String, ByVal isMSDSEnv As Boolean, ByVal isAspectListEnv As Boolean, _
        ByVal isEMPEnv As Boolean, ByVal AspectType As String, ByVal RnDTeamMemberID As Integer, ByVal HRSafetyTeamMemberID As Integer, _
        ByVal CorpEnvTeamMemberID As Integer, ByVal PlantEnvTeamMemberID As Integer, ByVal PurchasingTeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Chemical_Review_Form"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@RequestedByTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@RequestedByTeamMemberID").Value = RequestedByTeamMemberID

            If RequestDate Is Nothing Then
                RequestDate = ""
            End If

            myCommand.Parameters.Add("@RequestDate", SqlDbType.VarChar)
            myCommand.Parameters("@RequestDate").Value = commonFunctions.convertSpecialChar(RequestDate, False)

            If ProductName Is Nothing Then
                ProductName = ""
            End If

            myCommand.Parameters.Add("@ProductName", SqlDbType.VarChar)
            myCommand.Parameters("@ProductName").Value = commonFunctions.convertSpecialChar(ProductName, False)

            If ProductManufacturer Is Nothing Then
                ProductManufacturer = ""
            End If

            myCommand.Parameters.Add("@ProductManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@ProductManufacturer").Value = commonFunctions.convertSpecialChar(ProductManufacturer, False)

            If PurchaseFrom Is Nothing Then
                PurchaseFrom = ""
            End If

            myCommand.Parameters.Add("@PurchaseFrom", SqlDbType.VarChar)
            myCommand.Parameters("@PurchaseFrom").Value = commonFunctions.convertSpecialChar(PurchaseFrom, False)

            If DeptArea Is Nothing Then
                DeptArea = ""
            End If

            myCommand.Parameters.Add("@DeptArea", SqlDbType.VarChar)
            myCommand.Parameters("@DeptArea").Value = commonFunctions.convertSpecialChar(DeptArea, False)

            If ChemicalDesc Is Nothing Then
                ChemicalDesc = ""
            End If

            myCommand.Parameters.Add("@ChemicalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ChemicalDesc").Value = commonFunctions.convertSpecialChar(ChemicalDesc, False)

            myCommand.Parameters.Add("@isProductionUsage", SqlDbType.Bit)
            myCommand.Parameters("@isProductionUsage").Value = isProductionUsage

            myCommand.Parameters.Add("@isLabUsage", SqlDbType.Bit)
            myCommand.Parameters("@isLabUsage").Value = isLabUsage

            myCommand.Parameters.Add("@isMaintenanceUsage", SqlDbType.Bit)
            myCommand.Parameters("@isMaintenanceUsage").Value = isMaintenanceUsage

            myCommand.Parameters.Add("@isOtherUsage", SqlDbType.Bit)
            myCommand.Parameters("@isOtherUsage").Value = isOtherUsage

            If OtherUsageDesc Is Nothing Then
                OtherUsageDesc = ""
            End If

            myCommand.Parameters.Add("@OtherUsageDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherUsageDesc").Value = commonFunctions.convertSpecialChar(OtherUsageDesc, False)

            myCommand.Parameters.Add("@HealthLevel", SqlDbType.Int)
            myCommand.Parameters("@HealthLevel").Value = HealthLevel

            myCommand.Parameters.Add("@FlammabilityLevel", SqlDbType.Int)
            myCommand.Parameters("@FlammabilityLevel").Value = FlammabilityLevel

            myCommand.Parameters.Add("@ReactivityLevel", SqlDbType.Int)
            myCommand.Parameters("@ReactivityLevel").Value = ReactivityLevel

            myCommand.Parameters.Add("@ProtectiveEquipmentLevel", SqlDbType.Int)
            myCommand.Parameters("@ProtectiveEquipmentLevel").Value = ReactivityLevel

            myCommand.Parameters.Add("@isPhysicalHazard", SqlDbType.Bit)
            myCommand.Parameters("@isPhysicalHazard").Value = isPhysicalHazard

            myCommand.Parameters.Add("@isHealthHazard", SqlDbType.Bit)
            myCommand.Parameters("@isHealthHazard").Value = isHealthHazard

            myCommand.Parameters.Add("@isEnvironmentalHazard", SqlDbType.Bit)
            myCommand.Parameters("@isEnvironmentalHazard").Value = isEnvironmentalHazard

            myCommand.Parameters.Add("@isOtherHazard", SqlDbType.Bit)
            myCommand.Parameters("@isOtherHazard").Value = isOtherHazard

            If OtherHazardDesc Is Nothing Then
                OtherHazardDesc = ""
            End If

            myCommand.Parameters.Add("@OtherHazardDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherHazardDesc").Value = commonFunctions.convertSpecialChar(OtherHazardDesc, False)

            myCommand.Parameters.Add("@isGlovesEquip", SqlDbType.Bit)
            myCommand.Parameters("@isGlovesEquip").Value = isGlovesEquip

            myCommand.Parameters.Add("@isGogglesEquip", SqlDbType.Bit)
            myCommand.Parameters("@isGogglesEquip").Value = isGogglesEquip

            myCommand.Parameters.Add("@isRespiratoryEquip", SqlDbType.Bit)
            myCommand.Parameters("@isRespiratoryEquip").Value = isRespiratoryEquip

            myCommand.Parameters.Add("@isProtectiveClothingEquip", SqlDbType.Bit)
            myCommand.Parameters("@isProtectiveClothingEquip").Value = isProtectiveClothingEquip

            myCommand.Parameters.Add("@isOtherEquip", SqlDbType.Bit)
            myCommand.Parameters("@isOtherEquip").Value = isOtherEquip

            If OtherEquipDesc Is Nothing Then
                OtherEquipDesc = ""
            End If

            myCommand.Parameters.Add("@OtherEquipDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherEquipDesc").Value = OtherEquipDesc

            myCommand.Parameters.Add("@isVentilationEng", SqlDbType.Bit)
            myCommand.Parameters("@isVentilationEng").Value = isVentilationEng

            myCommand.Parameters.Add("@isContainmentEng", SqlDbType.Bit)
            myCommand.Parameters("@isContainmentEng").Value = isContainmentEng

            If OtherEngDesc Is Nothing Then
                OtherEngDesc = ""
            End If

            myCommand.Parameters.Add("@OtherEngDesc", SqlDbType.VarChar)
            myCommand.Parameters("@OtherEngDesc").Value = commonFunctions.convertSpecialChar(OtherEngDesc, False)

            If IncompatibleWith Is Nothing Then
                IncompatibleWith = ""
            End If

            myCommand.Parameters.Add("@IncompatibleWith", SqlDbType.VarChar)
            myCommand.Parameters("@IncompatibleWith").Value = commonFunctions.convertSpecialChar(IncompatibleWith, False)

            If StorageDesc Is Nothing Then
                StorageDesc = ""
            End If

            myCommand.Parameters.Add("@StorageDesc", SqlDbType.VarChar)
            myCommand.Parameters("@StorageDesc").Value = commonFunctions.convertSpecialChar(StorageDesc, False)

            If DisposalDesc Is Nothing Then
                DisposalDesc = ""
            End If

            myCommand.Parameters.Add("@DisposalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@DisposalDesc").Value = commonFunctions.convertSpecialChar(DisposalDesc, False)

            myCommand.Parameters.Add("@isMSDSEnv", SqlDbType.Bit)
            myCommand.Parameters("@isMSDSEnv").Value = isMSDSEnv

            myCommand.Parameters.Add("@isAspectListEnv", SqlDbType.Bit)
            myCommand.Parameters("@isAspectListEnv").Value = isAspectListEnv

            myCommand.Parameters.Add("@isEMPEnv", SqlDbType.Bit)
            myCommand.Parameters("@isEMPEnv").Value = isEMPEnv

            If AspectType Is Nothing Then
                AspectType = ""
            End If

            myCommand.Parameters.Add("@AspectType", SqlDbType.VarChar)
            myCommand.Parameters("@AspectType").Value = AspectType

            myCommand.Parameters.Add("@RnDTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@RnDTeamMemberID").Value = RnDTeamMemberID

            myCommand.Parameters.Add("@HRSafetyTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@HRSafetyTeamMemberID").Value = HRSafetyTeamMemberID

            myCommand.Parameters.Add("@CorpEnvTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@CorpEnvTeamMemberID").Value = CorpEnvTeamMemberID

            myCommand.Parameters.Add("@PlantEnvTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PlantEnvTeamMemberID").Value = PlantEnvTeamMemberID

            myCommand.Parameters.Add("@PurchasingTeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@PurchasingTeamMemberID").Value = PurchasingTeamMemberID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewChemicalReviewForm")
            InsertChemicalReviewForm = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", RequestedByTeamMemberID: " & RequestedByTeamMemberID & ", RequestDate: " & RequestDate _
            & ", ProductName: " & ProductName & ", ProductManufacturer: " & ProductManufacturer _
            & ", PurchaseFrom: " & PurchaseFrom & ", DeptArea: " & DeptArea _
            & ", ChemicalDesc: " & ChemicalDesc _
            & ", isProductionUsage: " & isProductionUsage & ", isLabUsage: " & isLabUsage _
            & ", isMaintenanceUsage: " & isMaintenanceUsage & ", isOtherUsage: " & isOtherUsage _
            & ", OtherUsageDesc: " & OtherUsageDesc & ", HealthLevel: " & HealthLevel _
            & ", FlammabilityLevel: " & FlammabilityLevel & ", ReactivityLevel: " & ReactivityLevel _
            & ", ProtectiveEquipmentLevel: " & ProtectiveEquipmentLevel _
            & ", isPhysicalHazard: " & isPhysicalHazard & ", isHealthHazard: " & isHealthHazard _
            & ", isEnvironmentalHazard: " & isEnvironmentalHazard & ", isOtherHazard: " & isOtherHazard _
            & ", OtherHazardDesc: " & OtherHazardDesc _
            & ", isGlovesEquip: " & isGlovesEquip & ", isGogglesEquip: " & isGogglesEquip _
            & ", isRespiratoryEquip: " & isRespiratoryEquip & ", isProtectiveClothingEquip: " & isProtectiveClothingEquip _
            & ", isOtherEquip: " & isOtherEquip & ", OtherEquipDesc: " & OtherEquipDesc _
            & ", isVentilationEng: " & isVentilationEng _
            & ", isContainmentEng: " & isContainmentEng & ", isOtherEng: " & isOtherEng _
            & ", OtherEngDesc: " & OtherEngDesc & ", IncompatibleWith: " & IncompatibleWith _
            & ", StorageDesc: " & StorageDesc & ", DisposalDesc: " & DisposalDesc _
            & ", isMSDSEnv: " & isMSDSEnv & ", isAspectListEnv: " & isAspectListEnv & ", isEMPEnv: " & isEMPEnv _
            & ", AspectType: " & AspectType _
            & ", RnDTeamMemberID: " & RnDTeamMemberID & ", HRSafetyTeamMemberID: " & HRSafetyTeamMemberID _
            & ", CorpEnvTeamMemberID: " & CorpEnvTeamMemberID & ", PlantEnvTeamMemberID: " & PlantEnvTeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertChemicalReviewForm = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateChemicalReviewFormOverallStatus(ByVal ChemRevFormID As Integer, ByVal StatusID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Chemical_Review_Form_Overall_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID _
            & ", StatusID: " & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChemicalReviewFormOverallStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChemicalReviewFormOverallStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateChemicalReviewFormApprovalStatus(ByVal ChemRevFormID As Integer, ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Integer, ByVal StatusID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Chemical_Review_Form_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            If Comments Is Nothing Then
                Comments = ""
            End If

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.convertSpecialChar(Comments, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID & ", StatusID: " & StatusID _
            & ", Comments : " & Comments _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChemicalReviewFormApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChemicalReviewFormApprovalStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateChemicalReviewFormNotification(ByVal ChemRevFormID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Checmical_Review_Form_Notification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChemicalReviewFormNotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChemicalReviewFormNotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteChemicalReviewForm(ByVal ChemRevFormID As Integer, ByVal VoidComment As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Chemical_Review_Form"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@VoidComment", SqlDbType.VarChar)
            myCommand.Parameters("@VoidComment").Value = VoidComment

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID _
            & ", VoidComment: " & VoidComment _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteChemicalReviewForm : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetChemicalReviewFormSupportingDocList(ByVal ChemRevFormID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form_Supporting_Doc_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupportingDocList")
            GetChemicalReviewFormSupportingDocList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewFormSupportingDocList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetChemicalReviewFormSupportingDoc(ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Chemical_Review_Form_Supporting_Doc"
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
            myAdapter.Fill(GetData, "SupportingDoc")
            GetChemicalReviewFormSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetChemicalReviewFormSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertChemicalReviewFormSupportingDoc(ByVal ChemRevFormID As Integer, ByVal SupportingDocName As String, ByVal DocBytes As Byte()) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Chemical_Review_Form_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ChemRevFormID", SqlDbType.Int)
            myCommand.Parameters("@ChemRevFormID").Value = ChemRevFormID

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = SupportingDocName

            myCommand.Parameters.Add("@supportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@supportingDocBinary").Value = DocBytes

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewSupportingDoc")
            InsertChemicalReviewFormSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID & ", SupportingDocName: " & SupportingDocName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SafetyModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "SafetyModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertChemicalReviewFormSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

End Class
