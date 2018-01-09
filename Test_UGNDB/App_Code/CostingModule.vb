''************************************************************************************************
''Name:		CodingModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Costing Module
''
''Date		    Author	    
''01/19/2009    Roderick Carlson			Created  
''04/14/2009    Roderick Carlson            Modified : Added Department, Process, and Template to GetFormula function
''10/29/2009    Roderick Carlson            Modified : Changed getCostSheet parameter from int to string for RFDNo
''10/30/2009    Roderick Carlson            Modified : Added function sp_Update_Cost_Sheet_Approved
''11/02/2009    Roderick Carlson            Modified : Adjusted function CalculateCostSheetCapital
''11/10/2009    Roderick Carlson            Modified : Adjusted CalculateCostSheetMaterial
''11/10/2009    Roderick Carlson            Modified : DCADE - round most numbers to 4 decimals during calculations
''11/17/2009    Roderick Carlson            Modified : Added Get CostSheet Customer Program function
''11/18/2009    Roderick Carlson            Modified : Added Get CostSheetSearch Function - to allow searching BOM too for BPCS PartNo
''11/23/2009    Roderick Carlson            Modified : Added Function - sp_Copy_Cost_Sheet_Material_Replace_Obsolete, sp_Copy_Cost_Sheet_Packaging_Replace_Obsolete, sp_Copy_Formula_Material_Replace_Obsolete, and sp_Copy_Formula_Packaging_Replace_Obsolete
''12/03/2009    Roderick Carlson            Modified : Adjusted Calculate Overhead and Capital
''01/12/2010    Roderick Carlson            Modified : CO-2822 - Freight can be saver per cost sheet
''04/27/2010    Roderick Carlson            Modified : CO-2884 Added paramter MaterialID to cost sheet search and delete cookie
''05/18/2010    Roderick Carlson            Modified : Added StandardCostPerUnitWOScrap, and Multiple Departments
''06/22/2010    Roderick Carlson            Modified : Added Costing-Department-List, added GetCostSheetReplicatedTo,GetCostSheetReplicatedFrom
''08/17/2010    Roderick Carlson            Modified : Adjusted Rounding in Calculate Functions to be to the nearest 0.00001
''09/19/2010    Roderick Carlson            Modified : Fixed bug where 0 totals were not saved
''01/27/2011    Roderick Carlson            Modified : Calculate Overhead - adjustment for LaborID=86 to calculate like LaborID=15
' 07/21/2011    Roderick Carlson            Modified : Added Barrier Run Rate Calculations, function sp_Get_Formula_Barrier_Run_Rate
' 01/10/2012    Roderick Carlson            Modified : Added new fields to Formula Maint - insert and update formula are affected
' 02/13/2012    Roderick Carlson            Modified : On all Overhead calculations for all cost sheets, for all formulas, do not use the crew size in the calculation
' 09/10/2012    Roderick Carlson            Modified : DB Cleanup
' 09/14/2012    Roderick Carlson            Modified : Add Notes field
' 01/03/2014    LREY                        Replaced "PartNo" to PartNo, SoldTo/CABBV to Customer, Vendor to Supplier wherever used.
' 04/30/2014    LRey                        Added QuickQuote to GetCostSheetSearch
''************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports Microsoft.VisualBasic

Public Class CostingModule
    Public Shared Sub CleanCostingCrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Costing Preview
            If HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("CostSheetPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("CostSheetPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("CostSheetPreview") = Nothing
                HttpContext.Current.Session("CostSheetPreviewID") = Nothing
                GC.Collect()
            End If

            If HttpContext.Current.Session("DieLayoutPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("DieLayoutPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DieLayoutPreview") = Nothing
                HttpContext.Current.Session("DieLayoutPreviewID") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanCostingCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanCostingCrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub DeleteCostingCookies()

        Try
            HttpContext.Current.Response.Cookies("CostingModule_SaveCostSheetIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveCostSheetIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveAccountManagerIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveDepartmentIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveDepartmentIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveDrawingNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SavePartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SavePartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCustomerPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveCustomerPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveDesignLevelSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveDesignLevelSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveRFDNoSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveRFDNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveProgramIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCommodityIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveYearSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveYearSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCustomerSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveUGNFacilitySearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveIsApprovedSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveIsApprovedSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFilterApprovedSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveFilterApprovedSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCheckBOMSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveCheckBOMSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveListMaterialIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveListMaterialIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostingCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostingCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub DeleteFormulaCookies()

        Try
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaPartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaPartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaProcessIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteFormulaCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteFormulaCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub DeleteMaterialCookies()

        Try
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPartNameSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Value = ""
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIsCoatingSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIsCoatingSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveCommodityIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterCoatingSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterCoatingSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIsPackagingSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialIsPackagingSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterPackagingSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterPackagingSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialObsoleteSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialObsoleteSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterObsoleteSearch").Value = 0
            HttpContext.Current.Response.Cookies("CostingModule_SaveMaterialFilterObsoleteSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteMaterialCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteMaterialCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Function GetCostSheet(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheet")
            GetCostSheet = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheet = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetSearch(ByVal CostSheetID As String, ByVal CostSheetStatus As String, _
  ByVal AccountManagerID As Integer, ByVal DepartmentID As Integer, ByVal FormulaID As Integer, _
  ByVal DrawingNo As String, ByVal PartNo As String, ByVal CustomerPartNo As String, ByVal DesignLevel As String, _
  ByVal PartName As String, ByVal RFDNo As String, ByVal ProgramID As Integer, ByVal CommodityID As Integer, _
  ByVal VehicleYear As Integer, ByVal Customer As String, ByVal UGNFacility As String, _
  ByVal WaitingForTeamMemberApproval As Integer, ByVal ApprovedByTeamMember As Integer, ByVal isAdmin As Boolean, _
  ByVal SubscriptionID As Integer, ByVal filterApproved As Boolean, ByVal isApproved As Boolean, _
  ByVal checkBOM As Boolean, ByVal MaterialID As String, ByVal QuickQuote As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetID").Value = If(CostSheetID Is Nothing, "", CostSheetID)

            myCommand.Parameters.Add("@costSheetStatus", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetStatus").Value = CostSheetStatus

            myCommand.Parameters.Add("@accountManagerID", SqlDbType.Int)
            myCommand.Parameters("@accountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = If(DrawingNo Is Nothing, "", DrawingNo)

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = If(PartNo Is Nothing, "", PartNo)

            myCommand.Parameters.Add("@customerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@customerPartNo").Value = If(CustomerPartNo Is Nothing, "", CustomerPartNo)

            myCommand.Parameters.Add("@designLevel", SqlDbType.VarChar)
            myCommand.Parameters("@designLevel").Value = If(DesignLevel Is Nothing, "", DesignLevel)

            myCommand.Parameters.Add("@partName", SqlDbType.VarChar)
            myCommand.Parameters("@partName").Value = If(PartName Is Nothing, "", PartName)

            myCommand.Parameters.Add("@rfdNo", SqlDbType.VarChar)
            myCommand.Parameters("@rfdNo").Value = If(RFDNo Is Nothing, "", RFDNo)

            myCommand.Parameters.Add("@programID", SqlDbType.Int)
            myCommand.Parameters("@programID").Value = ProgramID

            myCommand.Parameters.Add("@commodityID", SqlDbType.Int)
            myCommand.Parameters("@commodityID").Value = CommodityID

            myCommand.Parameters.Add("@vehicleYear", SqlDbType.Int)
            myCommand.Parameters("@vehicleYear").Value = VehicleYear

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = If(Customer Is Nothing, "", Customer)

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = If(UGNFacility Is Nothing, "", UGNFacility)

            myCommand.Parameters.Add("@waitingForTeamMemberApproval", SqlDbType.Int)
            myCommand.Parameters("@waitingForTeamMemberApproval").Value = WaitingForTeamMemberApproval

            myCommand.Parameters.Add("@approvedByTeamMember", SqlDbType.Int)
            myCommand.Parameters("@approvedByTeamMember").Value = ApprovedByTeamMember

            myCommand.Parameters.Add("@isAdmin", SqlDbType.Bit)
            myCommand.Parameters("@isAdmin").Value = isAdmin

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@filterApproved", SqlDbType.Bit)
            myCommand.Parameters("@filterApproved").Value = filterApproved

            myCommand.Parameters.Add("@isApproved", SqlDbType.Bit)
            myCommand.Parameters("@isApproved").Value = isApproved

            myCommand.Parameters.Add("@checkBOM", SqlDbType.Bit)
            myCommand.Parameters("@checkBOM").Value = checkBOM

            myCommand.Parameters.Add("@materialID", SqlDbType.VarChar)
            myCommand.Parameters("@materialID").Value = If(MaterialID Is Nothing, "", MaterialID)

            myCommand.Parameters.Add("@QuickQuote", SqlDbType.Bit)
            myCommand.Parameters("@QuickQuote").Value = QuickQuote

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetSearch")
            GetCostSheetSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", CostSheetStatus: " & CostSheetStatus _
            & ", AccountManagerID: " & AccountManagerID & ", DepartmentID: " & DepartmentID _
            & ", FormulaID: " & FormulaID & ", DrawingNo: " & DrawingNo & ", PartNo : " & PartNo _
            & ", CustomerPartNo: " & CustomerPartNo & ", DesignLevel: " & DesignLevel & ", PartName: " & PartName _
            & ", RFDNo: " & RFDNo & ", ProgramID: " & ProgramID & ", VehicleYear: " & VehicleYear _
            & ", Customer: " & Customer & ", UGNFacility: " & UGNFacility _
            & ", WaitingForTeamMemberApproval: " & WaitingForTeamMemberApproval _
            & ", ApprovedByTeamMember: " & ApprovedByTeamMember _
            & ", isAdmin: " & isAdmin _
            & ", SubscriptionID: " & SubscriptionID _
            & ", filterApproved: " & filterApproved _
            & ", isApproved: " & isApproved _
            & ", checkBOM: " & checkBOM _
            & ", MaterialID: " & MaterialID _
            & ", QuickQuote: " & QuickQuote _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetSearch : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetPriceMargin(ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Price_Margin"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPriceMargin")
            GetCostSheetPriceMargin = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPriceMargin : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPriceMargin : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPriceMargin = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetPartSpecification(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPartSpecifications")
            GetCostSheetPartSpecification = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPartSpecification = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetAccountManagers() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Account_Managers"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetAccountManagers")
            GetCostSheetAccountManagers = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetAccountManagers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetAccountManagers : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetAccountManagers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetFormula(ByVal FormulaID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Formula")
            GetFormula = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page           
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormula : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormula : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormula = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetFormulaRevisions(ByVal FormulaName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Revisions"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaName", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaName").Value = FormulaName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FormulaRevisions")
            GetFormulaRevisions = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page           
            Dim strUserEditedData As String = "FormulaName: " & FormulaName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaRevisions : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaRevisions : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaRevisions = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetFormulaBarrierRunRate(ByVal Travel As Decimal) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Barrier_Run_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@Travel", SqlDbType.Decimal)
            myCommand.Parameters("@Travel").Value = Travel

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FormulaBarrierRunRate")
            GetFormulaBarrierRunRate = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page           
            Dim strUserEditedData As String = "Travel: " & Travel & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaBarrierRunRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaBarrierRunRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaBarrierRunRate = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetFormulaSearch(ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal DrawingNo As String, _
      ByVal PartNo As String, ByVal PartName As String, ByVal DepartmentID As Integer, ByVal ProcessID As Integer, _
      ByVal TemplateID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@formulaName", SqlDbType.VarChar)
            myCommand.Parameters("@formulaName").Value = FormulaName

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = PartName

            myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@processID", SqlDbType.Int)
            myCommand.Parameters("@processID").Value = ProcessID

            myCommand.Parameters.Add("@templateID", SqlDbType.Int)
            myCommand.Parameters("@templateID").Value = TemplateID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Formula")
            GetFormulaSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ",FormulaName: " & FormulaName & _
            ",DrawingNo: " & DrawingNo & ",PartNo: " & PartNo & ",PartName: " & PartName & _
            ",DepartmentID: " & DepartmentID & ",ProcessID: " & ProcessID & ",TemplateID: " & TemplateID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormula : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormula : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetFormulaHistory(ByVal FormulaID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FormulaHistory")
            GetFormulaHistory = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertFormulaHistory(ByVal FormulaID As Integer, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Formula_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

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
            Dim strUserEditedData As String = "FormulaID: " & FormulaID _
            & ", ActionTakenTMID:" & ActionTakenTMID _
            & ", ActionDesc:" & ActionDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaHistory : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertFormulaHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyFormulaToCostSheetDepartment(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Department"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyFormulaToCostSheetLabor(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaToCostSheetOverhead(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaToCostSheetMaterial(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaToCostSheetPackaging(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaToCostSheetMiscCost(ByVal FormulaID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_To_Cost_Sheet_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaToCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaToCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetWaitingForTeamMemberApprovals() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Waiting_For_Team_Member_Approvals"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetUnapprovedTeamMembers")
            GetCostSheetWaitingForTeamMemberApprovals = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetWaitingForTeamMemberApprovals : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetWaitingForTeamMemberApprovals : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetWaitingForTeamMemberApprovals = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetApprovedByTeamMembers() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Approved_By_Team_Members"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetApprovedTeamMembers")
            GetCostSheetApprovedByTeamMembers = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetApprovedByTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetApprovedByTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetApprovedByTeamMembers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetProcess(ByVal ProcessID As Integer, ByVal ProcessName As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Process"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@processID", SqlDbType.Int)
            myCommand.Parameters("@processID").Value = ProcessID

            If ProcessName Is Nothing Then
                ProcessName = ""
            End If

            myCommand.Parameters.Add("@processName", SqlDbType.VarChar)
            myCommand.Parameters("@processName").Value = ProcessName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProcessList")
            GetProcess = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetProcess = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetLabor(ByVal LaborID As Integer, ByVal LaborDesc As String, ByVal filterOffline As Boolean, ByVal isOffline As Boolean) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            If LaborDesc Is Nothing Then
                LaborDesc = ""
            End If

            myCommand.Parameters.Add("@laborDesc", SqlDbType.VarChar)
            myCommand.Parameters("@laborDesc").Value = LaborDesc

            myCommand.Parameters.Add("@filterOffline", SqlDbType.Bit)
            myCommand.Parameters("@filterOffline").Value = filterOffline

            myCommand.Parameters.Add("@isOffline", SqlDbType.Bit)
            myCommand.Parameters("@isOffline").Value = isOffline

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "LaborList")
            GetLabor = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID: " & LaborID & ", LaborDesc: " & LaborDesc _
            & ", filterOffline: " & filterOffline & ", isOffline: " & isOffline _
            & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLabor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetMaterial(ByVal MaterialID As String, ByVal PartName As String, ByVal PartNo As String, _
    ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal PurchasedGoodID As Integer, _
    ByVal UGNFacilityCode As String, ByVal OldMaterialGroup As String, ByVal isPackaging As Boolean, ByVal filterPackaging As Boolean, _
    ByVal isCoating As Boolean, ByVal filterCoating As Boolean, _
    ByVal Obsolete As Boolean, ByVal filterObsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialID Is Nothing Then MaterialID = ""

            myCommand.Parameters.Add("@materialID", SqlDbType.VarChar)
            myCommand.Parameters("@materialID").Value = MaterialID

            If PartName Is Nothing Then PartName = ""

            myCommand.Parameters.Add("@partName", SqlDbType.VarChar)
            myCommand.Parameters("@partName").Value = PartName

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If DrawingNo Is Nothing Then DrawingNo = ""

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@ugndbVendorID", SqlDbType.Int)
            myCommand.Parameters("@ugndbVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@UGNFacilityCode", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacilityCode").Value = UGNFacilityCode

            If OldMaterialGroup Is Nothing Then OldMaterialGroup = ""

            myCommand.Parameters.Add("@oldMaterialGroup", SqlDbType.VarChar)
            myCommand.Parameters("@oldMaterialGroup").Value = OldMaterialGroup

            myCommand.Parameters.Add("@isPackaging", SqlDbType.Bit)
            myCommand.Parameters("@isPackaging").Value = isPackaging

            myCommand.Parameters.Add("@filterPackaging", SqlDbType.Bit)
            myCommand.Parameters("@filterPackaging").Value = filterPackaging

            myCommand.Parameters.Add("@isCoating", SqlDbType.Bit)
            myCommand.Parameters("@isCoating").Value = isCoating

            myCommand.Parameters.Add("@filterCoating", SqlDbType.Bit)
            myCommand.Parameters("@filterCoating").Value = filterCoating

            myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            myCommand.Parameters("@obsolete").Value = Obsolete

            myCommand.Parameters.Add("@filterObsolete", SqlDbType.Bit)
            myCommand.Parameters("@filterObsolete").Value = filterObsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialList")
            GetMaterial = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialID: " & MaterialID & ", PartName: " & PartName & _
            ", PartNo: " & PartNo & ", DrawingNo: " & DrawingNo & ", UGNDBVendorID: " & UGNDBVendorID & _
            ", PurchasedGoodID: " & PurchasedGoodID & ", OldMaterialGroup: " & OldMaterialGroup & _
            ", isPackaging: " & isPackaging & ", filterPackaging: " & filterPackaging & _
            ", isCoating: " & isCoating & ", filterCoating: " & filterCoating & _
            ", Obsolete: " & Obsolete & ", filterObsolete: " & filterObsolete & _
            "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMaterial = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetProductionRate(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Production_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetProductionRates")
            GetCostSheetProductionRate = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetProductionRates : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetProductionRates : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetProductionRate = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetProductionLimit(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Production_Limit"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetProductionLimit")
            GetCostSheetProductionLimit = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetProductionLimit = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetQuotedInfo(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Quoted_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetQuotedInfo")
            GetCostSheetQuotedInfo = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetQuotedInfo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetOverhead(ByVal LaborID As Integer, ByVal LaborDesc As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            If LaborDesc Is Nothing Then
                LaborDesc = ""
            End If

            myCommand.Parameters.Add("@laborDesc", SqlDbType.VarChar)
            myCommand.Parameters("@laborDesc").Value = LaborDesc

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OverheadList")
            GetOverhead = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID: " & LaborID & ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetOverhead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetMiscCost(ByVal MiscCostID As Integer, ByVal MiscCostDesc As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@miscCostID", SqlDbType.Int)
            myCommand.Parameters("@miscCostID").Value = MiscCostID

            If MiscCostDesc Is Nothing Then
                MiscCostDesc = ""
            End If

            myCommand.Parameters.Add("@miscCostDesc", SqlDbType.VarChar)
            myCommand.Parameters("@miscCostDesc").Value = MiscCostDesc

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MiscCostList")
            GetMiscCost = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MiscCostID: " & MiscCostID & ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMiscCost = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetSketchInfo(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Sketch_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetSketchInfo")
            GetCostSheetSketchInfo = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetSketchInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetSketchInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetSketchInfo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetCompositePartSpecification(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Composite_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetCompositePartSpecification")
            GetCostSheetCompositePartSpecification = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetCompositePartSpecification = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetMoldedBarrier(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Molded_Barrier"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure


            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetMoldedBarrier")
            GetCostSheetMoldedBarrier = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetMoldedBarrier : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetMoldedBarrier : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetMoldedBarrier = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCapital(ByVal CapitalID As Integer, ByVal CapitalDesc As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@capitalID", SqlDbType.Int)
            myCommand.Parameters("@capitalID").Value = CapitalID

            If CapitalDesc Is Nothing Then
                CapitalDesc = ""
            End If

            myCommand.Parameters.Add("@capitalDesc", SqlDbType.VarChar)
            myCommand.Parameters("@capitalDesc").Value = CapitalDesc

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CapitalList")
            GetCapital = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalID: " & CapitalID & "CapitalDesc: " & CapitalDesc & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCapital = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetTemplate(ByVal TemplateID As Integer, ByVal TemplateName As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Template"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@templateID", SqlDbType.Int)
            myCommand.Parameters("@templateID").Value = TemplateID

            If TemplateName Is Nothing Then
                TemplateName = ""
            End If

            myCommand.Parameters.Add("@templateName", SqlDbType.VarChar)
            myCommand.Parameters("@templateName").Value = TemplateName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TemplateList")
            GetTemplate = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TemplateID: " & TemplateID & ", TemplateName: " & TemplateName & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTemplate = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetGroup(ByVal GroupID As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetGroupList")
            GetCostSheetGroup = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetGroup = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Group_Team_Member"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetGroupTeamMember")
            GetCostSheetGroupTeamMember = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID & _
            ", TeamMemberID: " & TeamMemberID & _
            ", SubscriptionID: " & SubscriptionID & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetGroupTeamMember : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetGroupTeamMember = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function CopyCostSheetGroup(ByVal GroupID As Integer) As Boolean

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Group"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Dim bResult As Boolean = False

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

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetGroup : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

        CopyCostSheetGroup = bResult

    End Function
   
    Public Shared Function InsertCostSheet(ByVal PreviousCostSheetID As Integer, ByVal CostSheetStatus As String, _
   ByVal QuoteDate As String, ByVal RFDNo As Integer, ByVal UGNFacility As String, _
   ByVal DesignationType As String, ByVal NewCustomerPartNo As String, ByVal NewPartName As String, _
   ByVal NewDesignLevel As String, ByVal NewDrawingNo As String, _
   ByVal OriginalCustomerPartNo As String, ByVal OriginalDesignLevel As String, ByVal CommodityID As Integer, _
   ByVal PurchasedGoodID As Integer, ByVal NewPartNo As String, ByVal NewPartRevision As String, _
   ByVal OriginalPartNo As String, ByVal OriginalPartRevision As String, _
   ByVal OldOriginalPartNo As String, ByVal Notes As String, ByVal QuickQuote As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@previousCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@previousCostSheetID").Value = PreviousCostSheetID

            myCommand.Parameters.Add("@costSheetStatus", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetStatus").Value = CostSheetStatus

            If QuoteDate Is Nothing Then QuoteDate = ""

            myCommand.Parameters.Add("@quoteDate", SqlDbType.VarChar)
            myCommand.Parameters("@quoteDate").Value = QuoteDate

            myCommand.Parameters.Add("@rfdNo", SqlDbType.Int)
            myCommand.Parameters("@rfdNo").Value = RFDNo

            If UGNFacility Is Nothing Then UGNFacility = ""

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            If DesignationType Is Nothing Then DesignationType = ""

            myCommand.Parameters.Add("@designationType", SqlDbType.VarChar)
            myCommand.Parameters("@designationType").Value = DesignationType

            If NewCustomerPartNo Is Nothing Then NewCustomerPartNo = ""

            myCommand.Parameters.Add("@newCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@newCustomerPartNo").Value = NewCustomerPartNo

            If NewPartName Is Nothing Then NewPartName = ""

            myCommand.Parameters.Add("@newPartName", SqlDbType.VarChar)
            myCommand.Parameters("@newPartName").Value = NewPartName

            If NewDesignLevel Is Nothing Then NewDesignLevel = ""

            myCommand.Parameters.Add("@newDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@newDesignLevel").Value = NewDesignLevel

            If NewDrawingNo Is Nothing Then NewDrawingNo = ""

            myCommand.Parameters.Add("@newDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@newDrawingNo").Value = NewDrawingNo

            If OriginalCustomerPartNo Is Nothing Then OriginalCustomerPartNo = ""

            myCommand.Parameters.Add("@originalCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@originalCustomerPartNo").Value = OriginalCustomerPartNo

            If OriginalDesignLevel Is Nothing Then OriginalDesignLevel = ""

            myCommand.Parameters.Add("@originalDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@originalDesignLevel").Value = OriginalDesignLevel

            myCommand.Parameters.Add("@commodityID", SqlDbType.Int)
            myCommand.Parameters("@commodityID").Value = CommodityID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            If NewPartNo Is Nothing Then NewPartNo = ""

            myCommand.Parameters.Add("@newBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@newBPCSPartNo").Value = NewPartNo

            If NewPartRevision Is Nothing Then NewPartRevision = ""

            myCommand.Parameters.Add("@newBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@newBPCSPartRevision").Value = NewPartRevision

            If OriginalPartNo Is Nothing Then OriginalPartNo = ""

            myCommand.Parameters.Add("@originalBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@originalBPCSPartNo").Value = OriginalPartNo

            If OriginalPartRevision Is Nothing Then OriginalPartRevision = ""

            myCommand.Parameters.Add("@originalBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@originalBPCSPartRevision").Value = OriginalPartRevision

            If OldOriginalPartNo Is Nothing Then OldOriginalPartNo = ""

            myCommand.Parameters.Add("@oldOriginalPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@oldOriginalPartNo").Value = OldOriginalPartNo

            If Notes Is Nothing Then Notes = ""

            myCommand.Parameters.Add("@notes", SqlDbType.VarChar)
            myCommand.Parameters("@notes").Value = Notes

            myCommand.Parameters.Add("@QuickQuote", SqlDbType.Bit)
            myCommand.Parameters("@QuickQuote").Value = QuickQuote

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewCostSheet")
            InsertCostSheet = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PreviousCostSheetID: " & PreviousCostSheetID _
            & ", CostSheetStatus: " & CostSheetStatus _
            & ", QuoteDate: " & QuoteDate _
            & ", RFDNo: " & RFDNo _
            & ", UGNFacility : " & UGNFacility _
            & ", DesignationType: " & DesignationType _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", NewPartName: " & NewPartName _
            & ", NewDesignLevel: " & NewDesignLevel _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", OriginalCustomerPartNo: " & OriginalCustomerPartNo _
            & ", OriginalDesignLevel: " & OriginalDesignLevel _
            & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", NewPartNo: " & NewPartNo _
            & ", NewPartRevision: " & NewPartRevision _
            & ", OriginalPartNo: " & OriginalPartNo _
            & ", OriginalPartRevision: " & OriginalPartRevision _
            & ", Notes: " & Notes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertCostSheet = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub UpdateCostSheet(ByVal CostSheetID As Integer, ByVal CostSheetStatus As String, ByVal QuoteDate As String, _
   ByVal RFDNo As Integer, ByVal ECINo As Integer, ByVal UGNFacility As String, ByVal DesignationType As String, _
   ByVal NewCustomerPartNo As String, ByVal NewPartName As String, ByVal NewDesignLevel As String, _
   ByVal NewDrawingNo As String, ByVal OriginalCustomerPartNo As String, ByVal OriginalDesignLevel As String, _
   ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, ByVal NewPartNo As String, ByVal NewPartRevision As String, _
   ByVal OriginalPartNo As String, ByVal OriginalPartRevision As String, ByVal Notes As String, ByVal QuickQuote As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@costSheetStatus", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetStatus").Value = CostSheetStatus

            If QuoteDate Is Nothing Then QuoteDate = ""

            myCommand.Parameters.Add("@quoteDate", SqlDbType.VarChar)
            myCommand.Parameters("@quoteDate").Value = QuoteDate

            myCommand.Parameters.Add("@rfdNo", SqlDbType.Int)
            myCommand.Parameters("@rfdNo").Value = RFDNo

            myCommand.Parameters.Add("@eciNo", SqlDbType.Int)
            myCommand.Parameters("@eciNo").Value = ECINo

            If UGNFacility Is Nothing Then UGNFacility = ""

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            If DesignationType Is Nothing Then DesignationType = ""

            myCommand.Parameters.Add("@designationType", SqlDbType.VarChar)
            myCommand.Parameters("@designationType").Value = DesignationType

            If NewCustomerPartNo Is Nothing Then NewCustomerPartNo = ""

            myCommand.Parameters.Add("@newCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@newCustomerPartNo").Value = NewCustomerPartNo

            If NewPartName Is Nothing Then NewPartName = ""

            myCommand.Parameters.Add("@newPartName", SqlDbType.VarChar)
            myCommand.Parameters("@newPartName").Value = NewPartName

            If NewDesignLevel Is Nothing Then NewDesignLevel = ""

            myCommand.Parameters.Add("@newDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@newDesignLevel").Value = NewDesignLevel

            If NewDrawingNo Is Nothing Then NewDrawingNo = ""

            myCommand.Parameters.Add("@newDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@newDrawingNo").Value = NewDrawingNo

            If OriginalCustomerPartNo Is Nothing Then OriginalCustomerPartNo = ""

            myCommand.Parameters.Add("@originalCustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@originalCustomerPartNo").Value = OriginalCustomerPartNo

            If OriginalDesignLevel Is Nothing Then OriginalDesignLevel = ""

            myCommand.Parameters.Add("@originalDesignLevel", SqlDbType.VarChar)
            myCommand.Parameters("@originalDesignLevel").Value = OriginalDesignLevel

            myCommand.Parameters.Add("@commodityID", SqlDbType.Int)
            myCommand.Parameters("@commodityID").Value = CommodityID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            If NewPartNo Is Nothing Then NewPartNo = ""

            myCommand.Parameters.Add("@newBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@newBPCSPartNo").Value = NewPartNo

            If NewPartRevision Is Nothing Then NewPartRevision = ""


            myCommand.Parameters.Add("@newBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@newBPCSPartRevision").Value = NewPartRevision

            If OriginalPartNo Is Nothing Then OriginalPartNo = ""

            myCommand.Parameters.Add("@originalBPCSPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@originalBPCSPartNo").Value = OriginalPartNo

            If OriginalPartRevision Is Nothing Then OriginalPartRevision = ""

            myCommand.Parameters.Add("@originalBPCSPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@originalBPCSPartRevision").Value = OriginalPartRevision

            If Notes Is Nothing Then Notes = ""

            myCommand.Parameters.Add("@notes", SqlDbType.VarChar)
            myCommand.Parameters("@notes").Value = Notes

            myCommand.Parameters.Add("@QuickQuote", SqlDbType.Bit)
            myCommand.Parameters("@QuickQuote").Value = QuickQuote

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", CostSheetStatus: " & CostSheetStatus _
            & ", QuoteDate: " & QuoteDate _
            & ", RFDNo: " & RFDNo _
            & ", UGNFacility : " & UGNFacility _
            & ", DesignationType: " & DesignationType _
            & ", NewCustomerPartNo: " & NewCustomerPartNo _
            & ", NewPartName: " & NewPartName _
            & ", NewDesignLevel: " & NewDesignLevel _
            & ", NewDrawingNo: " & NewDrawingNo _
            & ", OriginalCustomerPartNo: " & OriginalCustomerPartNo _
            & ", OriginalDesignLevel: " & OriginalDesignLevel _
            & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", NewPartNo: " & NewPartNo _
            & ", NewPartRevision: " & NewPartRevision _
            & ", OriginalPartNo: " & OriginalPartNo _
            & ", OriginalPartRevision: " & OriginalPartRevision _
            & ", Notes: " & Notes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheet: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheet: " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetStatus(ByVal CostSheetID As Integer, ByVal CostSheetStatus As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@costSheetStatus", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetStatus").Value = CostSheetStatus

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", CostSheetStatus: " & CostSheetStatus _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheet(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID


            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetPartSpecification(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, _
    ByVal isDiecut As Boolean, ByVal PartThickness As Double, ByVal PartThicknessUnitID As Integer, ByVal isCompletedOffline As Boolean, _
    ByVal OffLineRate As Integer, ByVal NumberOfHoles As Integer, ByVal PartWidth As Double, ByVal PartWidthUnitID As Integer, _
    ByVal PartLength As Double, ByVal PartLengthUnitID As Integer, ByVal ConfigurationFactor As Double, ByVal RepackMaterial As String, _
    ByVal ApproxWeight As Double, ByVal ApproxWeightUnitID As Integer, ByVal ProductionRate As Double, ByVal DepartmentID As Integer, _
    ByVal NumberOfCarriers As Double, ByVal Foam As Double, ByVal FoamUnitID As Integer, ByVal PiecesPerCycle As Integer, _
    ByVal PiecesCaughtTogether As Integer, ByVal isSideBySide As Boolean, ByVal CalculatedArea As Double, ByVal CalculatedAreaUnitID As Integer, _
    ByVal ChangedArea As Double, ByVal ChangedAreaUnitID As Integer, ByVal DieLayoutWidth As Double, ByVal DieLayoutWidthUnitID As Integer, _
    ByVal DieLayoutTravel As Double, ByVal DieLayoutTravelUnitID As Integer, ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, _
    ByVal SpecificGravity As Double, ByVal SpecificGravityUnitID As Integer, ByVal ProcessID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.VarChar)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@isDiecut", SqlDbType.Bit)
            myCommand.Parameters("@isDiecut").Value = isDiecut

            myCommand.Parameters.Add("@partThickness", SqlDbType.Decimal)
            myCommand.Parameters("@partThickness").Value = PartThickness

            myCommand.Parameters.Add("@partThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@partThicknessUnitID").Value = PartThicknessUnitID

            myCommand.Parameters.Add("@isCompletedOffline", SqlDbType.Bit)
            myCommand.Parameters("@isCompletedOffline").Value = isCompletedOffline

            myCommand.Parameters.Add("@offLineRate", SqlDbType.Int)
            myCommand.Parameters("@offLineRate").Value = OffLineRate

            myCommand.Parameters.Add("@numberOfHoles", SqlDbType.Int)
            myCommand.Parameters("@numberOfHoles").Value = NumberOfHoles

            myCommand.Parameters.Add("@partWidth", SqlDbType.Decimal)
            myCommand.Parameters("@partWidth").Value = PartWidth

            myCommand.Parameters.Add("@partWidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@partWidthUnitID").Value = PartWidthUnitID

            myCommand.Parameters.Add("@partLength", SqlDbType.Decimal)
            myCommand.Parameters("@partLength").Value = PartLength

            myCommand.Parameters.Add("@partLengthUnitID", SqlDbType.Int)
            myCommand.Parameters("@partLengthUnitID").Value = PartLengthUnitID

            myCommand.Parameters.Add("@configurationFactor", SqlDbType.Decimal)
            myCommand.Parameters("@configurationFactor").Value = ConfigurationFactor

            If RepackMaterial Is Nothing Then
                RepackMaterial = ""
            End If

            myCommand.Parameters.Add("@repackMaterial", SqlDbType.VarChar)
            myCommand.Parameters("@repackMaterial").Value = RepackMaterial

            myCommand.Parameters.Add("@approxWeight", SqlDbType.Decimal)
            myCommand.Parameters("@approxWeight").Value = ApproxWeight

            myCommand.Parameters.Add("@approxWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@approxWeightUnitID").Value = ApproxWeightUnitID

            myCommand.Parameters.Add("@productionRate", SqlDbType.Decimal)
            myCommand.Parameters("@productionRate").Value = ProductionRate

            myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@numberOfCarriers", SqlDbType.Decimal)
            myCommand.Parameters("@numberOfCarriers").Value = NumberOfCarriers

            myCommand.Parameters.Add("@foam", SqlDbType.Decimal)
            myCommand.Parameters("@foam").Value = Foam

            myCommand.Parameters.Add("@foamUnitID", SqlDbType.Int)
            myCommand.Parameters("@foamUnitID").Value = FoamUnitID

            myCommand.Parameters.Add("@piecesPerCycle", SqlDbType.Int)
            myCommand.Parameters("@piecesPerCycle").Value = PiecesPerCycle

            myCommand.Parameters.Add("@piecesCaughtTogether", SqlDbType.Int)
            myCommand.Parameters("@piecesCaughtTogether").Value = PiecesCaughtTogether

            myCommand.Parameters.Add("@isSideBySide", SqlDbType.Bit)
            myCommand.Parameters("@isSideBySide").Value = isSideBySide

            myCommand.Parameters.Add("@calculatedArea", SqlDbType.Decimal)
            myCommand.Parameters("@calculatedArea").Value = CalculatedArea

            myCommand.Parameters.Add("@calculatedAreaUnitID", SqlDbType.VarChar)
            myCommand.Parameters("@calculatedAreaUnitID").Value = CalculatedAreaUnitID

            myCommand.Parameters.Add("@changedArea", SqlDbType.Decimal)
            myCommand.Parameters("@changedArea").Value = ChangedArea

            myCommand.Parameters.Add("@changedAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@changedAreaUnitID").Value = ChangedAreaUnitID

            myCommand.Parameters.Add("@dieLayoutWidth", SqlDbType.Decimal)
            myCommand.Parameters("@dieLayoutWidth").Value = DieLayoutWidth

            myCommand.Parameters.Add("@dieLayoutWidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@dieLayoutWidthUnitID").Value = DieLayoutWidthUnitID

            myCommand.Parameters.Add("@dieLayoutTravel", SqlDbType.Decimal)
            myCommand.Parameters("@dieLayoutTravel").Value = DieLayoutTravel

            myCommand.Parameters.Add("@dieLayoutTravelUnitID", SqlDbType.Int)
            myCommand.Parameters("@dieLayoutTravelUnitID").Value = DieLayoutTravelUnitID

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@specificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@specificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@specificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@specificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@processID", SqlDbType.Int)
            myCommand.Parameters("@processID").Value = ProcessID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", FormulaID: " & FormulaID _
            & ", isDiecut: " & isDiecut & ", PartThickness: " & PartThickness & ", PartThicknessUnitID : " & PartThicknessUnitID _
            & ", isCompletedOffline: " & isCompletedOffline & ", OffLineRate: " & OffLineRate & ", NumberOfHoles: " & NumberOfHoles _
            & ", PartWidth: " & PartWidth & ", PartWidthUnitID: " & PartWidthUnitID _
            & ", PartLength: " & PartLength & ", PartLengthUnitID: " & PartLengthUnitID _
            & ", ConfigurationFactor: " & ConfigurationFactor & ", RepackMaterial: " & RepackMaterial _
            & ", ApproxWeight: " & ApproxWeight & ", ApproxWeightUnitID: " & ApproxWeightUnitID _
            & ", ProductionRate: " & ProductionRate & ", DepartmentID: " & DepartmentID _
            & ", NumberOfCarriers: " & NumberOfCarriers & ", Foam: " & Foam _
            & ", FoamUnitID: " & FoamUnitID & ", PiecesPerCycle: " & PiecesPerCycle _
            & ", PiecesCaughtTogether: " & PiecesCaughtTogether & ", isSideBySide: " & isSideBySide _
            & ", CalculatedArea: " & CalculatedArea & ", CalculatedAreaUnitID: " & CalculatedAreaUnitID _
            & ", ChangedArea: " & ChangedArea & ", ChangedAreaUnitID: " & ChangedAreaUnitID _
            & ", DieLayoutWidth: " & DieLayoutWidth & ", DieLayoutWidthUnitID: " & DieLayoutWidthUnitID _
            & ", DieLayoutTravel: " & DieLayoutTravel & ", DieLayoutTravelUnitID: " & DieLayoutTravelUnitID _
            & ", WeightPerArea: " & WeightPerArea & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID _
            & ", SpecificGravity: " & SpecificGravity & ", SpecificGravityUnitID: " & SpecificGravityUnitID _
            & ", ProcessID: " & ProcessID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetPartSpecification(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, _
   ByVal isDiecut As Boolean, ByVal PartThickness As Double, ByVal PartThicknessUnitID As Integer, ByVal isCompletedOffline As Boolean, _
   ByVal OffLineRate As Integer, ByVal NumberOfHoles As Integer, ByVal PartWidth As Double, ByVal PartWidthUnitID As Integer, _
   ByVal PartLength As Double, ByVal PartLengthUnitID As Integer, ByVal ConfigurationFactor As Double, ByVal RepackMaterial As String, _
   ByVal ApproxWeight As Double, ByVal ApproxWeightUnitID As Integer, ByVal ProductionRate As Double, ByVal DepartmentID As Integer, _
   ByVal NumberOfCarriers As Double, ByVal Foam As Double, ByVal FoamUnitID As Integer, ByVal PiecesPerCycle As Integer, _
   ByVal PiecesCaughtTogether As Integer, ByVal isSideBySide As Boolean, ByVal CalculatedArea As Double, ByVal CalculatedAreaUnitID As Integer, _
   ByVal ChangedArea As Double, ByVal ChangedAreaUnitID As Integer, ByVal DieLayoutWidth As Double, ByVal DieLayoutWidthUnitID As Integer, _
   ByVal DieLayoutTravel As Double, ByVal DieLayoutTravelUnitID As Integer, ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, _
   ByVal SpecificGravity As Double, ByVal SpecificGravityUnitID As Integer, ByVal ProcessID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.VarChar)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@isDiecut", SqlDbType.Bit)
            myCommand.Parameters("@isDiecut").Value = isDiecut

            myCommand.Parameters.Add("@partThickness", SqlDbType.Decimal)
            myCommand.Parameters("@partThickness").Value = PartThickness

            myCommand.Parameters.Add("@partThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@partThicknessUnitID").Value = PartThicknessUnitID

            myCommand.Parameters.Add("@isCompletedOffline", SqlDbType.Bit)
            myCommand.Parameters("@isCompletedOffline").Value = isCompletedOffline

            myCommand.Parameters.Add("@offLineRate", SqlDbType.Int)
            myCommand.Parameters("@offLineRate").Value = OffLineRate

            myCommand.Parameters.Add("@numberOfHoles", SqlDbType.Int)
            myCommand.Parameters("@numberOfHoles").Value = NumberOfHoles

            myCommand.Parameters.Add("@partWidth", SqlDbType.Decimal)
            myCommand.Parameters("@partWidth").Value = PartWidth

            myCommand.Parameters.Add("@partWidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@partWidthUnitID").Value = PartWidthUnitID

            myCommand.Parameters.Add("@partLength", SqlDbType.Decimal)
            myCommand.Parameters("@partLength").Value = PartLength

            myCommand.Parameters.Add("@partLengthUnitID", SqlDbType.Int)
            myCommand.Parameters("@partLengthUnitID").Value = PartLengthUnitID

            myCommand.Parameters.Add("@configurationFactor", SqlDbType.Decimal)
            myCommand.Parameters("@configurationFactor").Value = ConfigurationFactor

            If RepackMaterial Is Nothing Then
                RepackMaterial = ""
            End If

            myCommand.Parameters.Add("@repackMaterial", SqlDbType.VarChar)
            myCommand.Parameters("@repackMaterial").Value = RepackMaterial

            myCommand.Parameters.Add("@approxWeight", SqlDbType.Decimal)
            myCommand.Parameters("@approxWeight").Value = ApproxWeight

            myCommand.Parameters.Add("@approxWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@approxWeightUnitID").Value = ApproxWeightUnitID

            myCommand.Parameters.Add("@productionRate", SqlDbType.Decimal)
            myCommand.Parameters("@productionRate").Value = ProductionRate

            myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@numberOfCarriers", SqlDbType.Decimal)
            myCommand.Parameters("@numberOfCarriers").Value = NumberOfCarriers

            myCommand.Parameters.Add("@foam", SqlDbType.Decimal)
            myCommand.Parameters("@foam").Value = Foam

            myCommand.Parameters.Add("@foamUnitID", SqlDbType.Int)
            myCommand.Parameters("@foamUnitID").Value = FoamUnitID

            myCommand.Parameters.Add("@piecesPerCycle", SqlDbType.Int)
            myCommand.Parameters("@piecesPerCycle").Value = PiecesPerCycle

            myCommand.Parameters.Add("@piecesCaughtTogether", SqlDbType.Int)
            myCommand.Parameters("@piecesCaughtTogether").Value = PiecesCaughtTogether

            myCommand.Parameters.Add("@isSideBySide", SqlDbType.Bit)
            myCommand.Parameters("@isSideBySide").Value = isSideBySide

            myCommand.Parameters.Add("@calculatedArea", SqlDbType.Decimal)
            myCommand.Parameters("@calculatedArea").Value = CalculatedArea

            myCommand.Parameters.Add("@calculatedAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@calculatedAreaUnitID").Value = CalculatedAreaUnitID

            myCommand.Parameters.Add("@changedArea", SqlDbType.Decimal)
            myCommand.Parameters("@changedArea").Value = ChangedArea

            myCommand.Parameters.Add("@changedAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@changedAreaUnitID").Value = ChangedAreaUnitID

            myCommand.Parameters.Add("@dieLayoutWidth", SqlDbType.Decimal)
            myCommand.Parameters("@dieLayoutWidth").Value = DieLayoutWidth

            myCommand.Parameters.Add("@dieLayoutWidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@dieLayoutWidthUnitID").Value = DieLayoutWidthUnitID

            myCommand.Parameters.Add("@dieLayoutTravel", SqlDbType.Decimal)
            myCommand.Parameters("@dieLayoutTravel").Value = DieLayoutTravel

            myCommand.Parameters.Add("@dieLayoutTravelUnitID", SqlDbType.Int)
            myCommand.Parameters("@dieLayoutTravelUnitID").Value = DieLayoutTravelUnitID

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@specificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@specificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@specificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@specificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@processID", SqlDbType.Int)
            myCommand.Parameters("@processID").Value = ProcessID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", FormulaID: " & FormulaID _
            & ", isDiecut: " & isDiecut & ", PartThickness: " & PartThickness & ", PartThicknessUnitID : " & PartThicknessUnitID _
            & ", isCompletedOffline: " & isCompletedOffline & ", OffLineRate: " & OffLineRate & ", NumberOfHoles: " & NumberOfHoles _
            & ", PartWidth: " & PartWidth & ", PartWidthUnitID: " & PartWidthUnitID _
            & ", PartLength: " & PartLength & ", PartLengthUnits: " & PartLengthUnitID _
            & ", ConfigurationFactor: " & ConfigurationFactor & ", RepackMaterial: " & RepackMaterial _
            & ", ApproxWeight: " & ApproxWeight & ", ApproxWeightUnitID: " & ApproxWeightUnitID _
            & ", ProductionRate: " & ProductionRate & ", DepartmentID: " & DepartmentID _
            & ", NumberOfCarriers: " & NumberOfCarriers & ", Foam: " & Foam _
            & ", FoamUnitID: " & FoamUnitID & ", PiecesPerCycle: " & PiecesPerCycle _
            & ", PiecesCaughtTogether: " & PiecesCaughtTogether & ", isSideBySide: " & isSideBySide _
            & ", CalculatedArea: " & CalculatedArea & ", CalculatedAreaUnitID: " & CalculatedAreaUnitID _
            & ", ChangedArea: " & ChangedArea & ", ChangedAreaUnitID: " & ChangedAreaUnitID _
            & ", DieLayoutWidth: " & DieLayoutWidth & ", DieLayoutWidthUnitID: " & DieLayoutWidthUnitID _
            & ", DieLayoutTravel: " & DieLayoutTravel & ", DieLayoutTravelUnitID: " & DieLayoutTravelUnitID _
            & ", WeightPerArea: " & WeightPerArea & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID _
            & ", SpecificGravity: " & SpecificGravity & ", SpecificGravityUnitID: " & SpecificGravityUnitID _
            & ", ProcessID: " & ProcessID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetPartSpecification(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetPartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetProductionRate(ByVal CostSheetID As Integer, ByVal MaxMixCapacity As Integer, ByVal MaxMixCapacityUnitID As Integer, _
   ByVal MaxFormingRate As Integer, ByVal MaxFormingRateUnitID As Integer, ByVal CatchingAbility As Double, ByVal LineSpeedLimitation As Integer, ByVal CatchPercent As Double, _
   ByVal CoatingFactor As Double, ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, ByVal Offline_SheetsUp As Integer, _
   ByVal Offline_BlankCode As String, ByVal Offline_QuotedPressCycles As Integer, ByVal Offline_QuotedOfflineRates As Integer, _
   ByVal Offline_PiecesPerManHour As Double, ByVal Offline_PercentRecycle As Double, ByVal Quoted_MaxPieces As Integer, _
   ByVal Quoted_MaxPiecesUnitID As Integer, ByVal Max_MaxPieces As Integer, ByVal Max_MaxPiecesUnitID As Integer, _
   ByVal Quoted_PressCycles As Integer, ByVal Quoted_PressCyclesUnitID As Integer, ByVal Max_PressCycles As Integer, _
   ByVal Max_PressCyclesUnitID As Integer, ByVal Quoted_LineSpeed As Double, ByVal Quoted_LineSpeedUnitID As Integer, _
   ByVal Max_LineSpeed As Double, ByVal Max_LineSpeedUnitID As Integer, ByVal Quoted_NetFormingRate As Double, _
   ByVal Quoted_NetFormingRateUnitID As Integer, ByVal Max_NetFormingRate As Double, ByVal Max_NetFormingRateUnitID As Integer, _
   ByVal Quoted_MixCapacity As Double, ByVal Quoted_MixCapacityUnitID As Integer, ByVal Max_MixCapacity As Double, _
   ByVal Max_MixCapacityUnitID As Integer, ByVal Quoted_RecycleRate As Double, ByVal Quoted_RecycleRateUnitID As Integer, _
   ByVal Max_RecycleRate As Double, ByVal Max_RecycleRateUnitID As Integer, ByVal Quoted_PartWeight As Double, _
   ByVal Quoted_PartWeightUnitID As Integer, ByVal Max_PartWeight As Double, ByVal Max_PartWeightUnitID As Integer, _
   ByVal Quoted_CoatingWeight As Double, ByVal Quoted_CoatingWeightUnitID As Integer, ByVal Quoted_TotalWeight As Double, _
   ByVal Quoted_TotalWeightUnitID As Integer)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Production_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@maxMixCapacity", SqlDbType.Int)
            myCommand.Parameters("@maxMixCapacity").Value = MaxMixCapacity

            myCommand.Parameters.Add("@maxMixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@maxMixCapacityUnitID").Value = MaxMixCapacityUnitID

            myCommand.Parameters.Add("@maxFormingRate", SqlDbType.Int)
            myCommand.Parameters("@maxFormingRate").Value = MaxFormingRate

            myCommand.Parameters.Add("@maxFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@maxFormingRateUnitID").Value = MaxFormingRateUnitID

            myCommand.Parameters.Add("@catchingAbility", SqlDbType.Decimal)
            myCommand.Parameters("@catchingAbility").Value = CatchingAbility

            myCommand.Parameters.Add("@lineSpeedLimitation", SqlDbType.Int)
            myCommand.Parameters("@lineSpeedLimitation").Value = LineSpeedLimitation

            myCommand.Parameters.Add("@catchPercent", SqlDbType.Decimal)
            myCommand.Parameters("@catchPercent").Value = CatchPercent

            myCommand.Parameters.Add("@coatingFactor", SqlDbType.Decimal)
            myCommand.Parameters("@coatingFactor").Value = CoatingFactor

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@offline_SheetsUp", SqlDbType.Int)
            myCommand.Parameters("@offline_SheetsUp").Value = Offline_SheetsUp

            If Offline_BlankCode Is Nothing Then
                Offline_BlankCode = ""
            End If

            myCommand.Parameters.Add("@offline_BlankCode", SqlDbType.VarChar)
            myCommand.Parameters("@offline_BlankCode").Value = Offline_BlankCode

            myCommand.Parameters.Add("@offline_QuotedPressCycles", SqlDbType.Int)
            myCommand.Parameters("@offline_QuotedPressCycles").Value = Offline_QuotedPressCycles

            myCommand.Parameters.Add("@offline_QuotedOfflineRates", SqlDbType.Int)
            myCommand.Parameters("@offline_QuotedOfflineRates").Value = Offline_QuotedOfflineRates

            myCommand.Parameters.Add("@offline_PiecesPerManHour", SqlDbType.Decimal)
            myCommand.Parameters("@offline_PiecesPerManHour").Value = Offline_PiecesPerManHour

            myCommand.Parameters.Add("@offline_PercentRecycle", SqlDbType.Decimal)
            myCommand.Parameters("@offline_PercentRecycle").Value = Offline_PercentRecycle

            myCommand.Parameters.Add("@quoted_MaxPieces", SqlDbType.Int)
            myCommand.Parameters("@quoted_MaxPieces").Value = Quoted_MaxPieces

            myCommand.Parameters.Add("@quoted_MaxPiecesUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_MaxPiecesUnitID").Value = Quoted_MaxPiecesUnitID

            myCommand.Parameters.Add("@max_MaxPieces", SqlDbType.Int)
            myCommand.Parameters("@max_MaxPieces").Value = Max_MaxPieces

            myCommand.Parameters.Add("@max_MaxPiecesUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_MaxPiecesUnitID").Value = Max_MaxPiecesUnitID

            myCommand.Parameters.Add("@quoted_PressCycles", SqlDbType.Int)
            myCommand.Parameters("@quoted_PressCycles").Value = Quoted_PressCycles

            myCommand.Parameters.Add("@quoted_PressCyclesUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_PressCyclesUnitID").Value = Quoted_PressCyclesUnitID

            myCommand.Parameters.Add("@max_PressCycles", SqlDbType.Int)
            myCommand.Parameters("@max_PressCycles").Value = Max_PressCycles

            myCommand.Parameters.Add("@max_PressCyclesUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_PressCyclesUnitID").Value = Max_PressCyclesUnitID

            myCommand.Parameters.Add("@quoted_LineSpeed", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_LineSpeed").Value = Quoted_LineSpeed

            myCommand.Parameters.Add("@quoted_LineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_LineSpeedUnitID").Value = Quoted_LineSpeedUnitID

            myCommand.Parameters.Add("@max_LineSpeed", SqlDbType.Decimal)
            myCommand.Parameters("@max_LineSpeed").Value = Max_LineSpeed

            myCommand.Parameters.Add("@max_LineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_LineSpeedUnitID").Value = Max_LineSpeedUnitID

            myCommand.Parameters.Add("@quoted_NetFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_NetFormingRate").Value = Quoted_NetFormingRate

            myCommand.Parameters.Add("@quoted_NetFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_NetFormingRateUnitID").Value = Quoted_NetFormingRateUnitID

            myCommand.Parameters.Add("@max_NetFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@max_NetFormingRate").Value = Max_NetFormingRate

            myCommand.Parameters.Add("@max_NetFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_NetFormingRateUnitID").Value = Max_NetFormingRateUnitID

            myCommand.Parameters.Add("@quoted_MixCapacity", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_MixCapacity").Value = Quoted_MixCapacity

            myCommand.Parameters.Add("@quoted_MixCapacityUnitID", SqlDbType.VarChar)
            myCommand.Parameters("@quoted_MixCapacityUnitID").Value = Quoted_MixCapacityUnitID

            myCommand.Parameters.Add("@max_MixCapacity", SqlDbType.Decimal)
            myCommand.Parameters("@max_MixCapacity").Value = Max_MixCapacity

            myCommand.Parameters.Add("@max_MixCapacityUnitID", SqlDbType.VarChar)
            myCommand.Parameters("@max_MixCapacityUnitID").Value = Max_MixCapacityUnitID

            myCommand.Parameters.Add("@quoted_RecycleRate", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_RecycleRate").Value = Quoted_RecycleRate

            myCommand.Parameters.Add("@quoted_RecycleRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_RecycleRateUnitID").Value = Quoted_RecycleRateUnitID

            myCommand.Parameters.Add("@max_RecycleRate", SqlDbType.Decimal)
            myCommand.Parameters("@max_RecycleRate").Value = Max_RecycleRate

            myCommand.Parameters.Add("@max_RecycleRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_RecycleRateUnitID").Value = Max_RecycleRateUnitID

            myCommand.Parameters.Add("@quoted_PartWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_PartWeight").Value = Quoted_PartWeight

            myCommand.Parameters.Add("@quoted_PartWeightUnitID", SqlDbType.VarChar)
            myCommand.Parameters("@quoted_PartWeightUnitID").Value = Quoted_PartWeightUnitID

            myCommand.Parameters.Add("@max_PartWeight", SqlDbType.Decimal)
            myCommand.Parameters("@max_PartWeight").Value = Max_PartWeight

            myCommand.Parameters.Add("@max_PartWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_PartWeightUnitID").Value = Max_PartWeightUnitID

            myCommand.Parameters.Add("@quoted_CoatingWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_CoatingWeight").Value = Quoted_CoatingWeight

            myCommand.Parameters.Add("@quoted_CoatingWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_CoatingWeightUnitID").Value = Quoted_CoatingWeightUnitID

            myCommand.Parameters.Add("@quoted_TotalWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_TotalWeight").Value = Quoted_TotalWeight

            myCommand.Parameters.Add("@quoted_TotalWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_TotalWeightUnitID").Value = Quoted_TotalWeightUnitID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", MaxMixCapacity: " & MaxMixCapacity _
            & ", MaxMixCapacityUnitID: " & MaxMixCapacityUnitID & ", MaxFormingRate: " & MaxFormingRate _
            & ", MaxFormingRateUnitID: " & MaxFormingRateUnitID & ", CatchingAbility: " & CatchingAbility _
            & ", LineSpeedLimitation : " & LineSpeedLimitation & ", CatchPercent: " & CatchPercent _
            & ", CoatingFactor: " & CoatingFactor & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID & ", Offline_SheetsUp: " & Offline_SheetsUp _
            & ", Offline_BlankCode: " & Offline_BlankCode _
            & ", Offline_QuotedPressCycles: " & Offline_QuotedPressCycles & ", Offline_QuotedOfflineRates: " & Offline_QuotedOfflineRates _
            & ", Offline_PiecesPerManHour: " & Offline_PiecesPerManHour & ", Offline_PercentRecycle: " & Offline_PercentRecycle _
            & ", Quoted_MaxPieces: " & Quoted_MaxPieces & ", Quoted_MaxPiecesUnitID: " & Quoted_MaxPiecesUnitID _
            & ", Max_MaxPieces: " & Max_MaxPieces & ", Max_MaxPiecesUnitID: " & Max_MaxPiecesUnitID _
            & ", Quoted_PressCycles: " & Quoted_PressCycles & ", Quoted_PressCyclesUnitID: " & Quoted_PressCyclesUnitID _
            & ", Max_PressCycles: " & Max_PressCycles & ", Max_PressCyclesUnitID: " & Max_PressCyclesUnitID _
            & ", Quoted_LineSpeed: " & Quoted_LineSpeed & ", Quoted_LineSpeedUnitID: " & Quoted_LineSpeedUnitID _
            & ", Max_LineSpeed: " & Max_LineSpeed & ", Max_LineSpeedUnitID: " & Max_LineSpeedUnitID _
            & ", Quoted_MixCapacity: " & Quoted_MixCapacity & ", Quoted_MixCapacityUnitID: " & Quoted_MixCapacityUnitID _
            & ", Max_MixCapacity: " & Max_MixCapacity & ", Max_MixCapacityUnitID: " & Max_MixCapacityUnitID _
            & ", Quoted_RecycleRate: " & Quoted_RecycleRate & ", Quoted_RecycleRateUnitID: " & Quoted_RecycleRateUnitID _
            & ", Max_RecycleRate: " & Max_RecycleRate & ", Max_RecycleRateUnitID: " & Max_RecycleRateUnitID _
            & ", Quoted_PartWeight: " & Quoted_PartWeight & ", Quoted_PartWeightUnitID: " & Quoted_PartWeightUnitID _
            & ", Max_PartWeight: " & Max_PartWeight & ", Max_PartWeightUnitID: " & Max_PartWeightUnitID _
            & ", Quoted_CoatingWeight: " & Quoted_CoatingWeight & ", Quoted_CoatingWeightUnitID: " & Quoted_CoatingWeightUnitID _
            & ", Quoted_TotalWeight: " & Quoted_TotalWeight & ", Quoted_TotalWeightUnitID: " & Quoted_TotalWeightUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetProductionRate(ByVal CostSheetID As Integer, ByVal MaxMixCapacity As Integer, ByVal MaxMixCapacityUnitID As Integer, _
  ByVal MaxFormingRate As Integer, ByVal MaxFormingRateUnitID As Integer, ByVal CatchingAbility As Double, ByVal LineSpeedLimitation As Integer, ByVal CatchPercent As Double, _
  ByVal CoatingFactor As Double, ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, ByVal Offline_SheetsUp As Integer, _
  ByVal Offline_BlankCode As String, ByVal Offline_QuotedPressCycles As Integer, ByVal Offline_QuotedOfflineRates As Integer, _
  ByVal Offline_PiecesPerManHour As Double, ByVal Offline_PercentRecycle As Double, ByVal Quoted_MaxPieces As Integer, _
  ByVal Quoted_MaxPiecesUnitID As Integer, ByVal Max_MaxPieces As Integer, ByVal Max_MaxPiecesUnitID As Integer, _
  ByVal Quoted_PressCycles As Integer, ByVal Quoted_PressCyclesUnitID As Integer, ByVal Max_PressCycles As Integer, _
  ByVal Max_PressCyclesUnitID As Integer, ByVal Quoted_LineSpeed As Double, ByVal Quoted_LineSpeedUnitID As Integer, _
  ByVal Max_LineSpeed As Double, ByVal Max_LineSpeedUnitID As String, ByVal Quoted_NetFormingRate As Double, _
  ByVal Quoted_NetFormingRateUnitID As Integer, ByVal Max_NetFormingRate As Double, ByVal Max_NetFormingRateUnitID As Integer, _
  ByVal Quoted_MixCapacity As Double, ByVal Quoted_MixCapacityUnitID As Integer, ByVal Max_MixCapacity As Double, _
  ByVal Max_MixCapacityUnitID As Integer, ByVal Quoted_RecycleRate As Double, ByVal Quoted_RecycleRateUnitID As Integer, _
  ByVal Max_RecycleRate As Double, ByVal Max_RecycleRateUnitID As Integer, ByVal Quoted_PartWeight As Double, _
  ByVal Quoted_PartWeightUnitID As Integer, ByVal Max_PartWeight As Double, ByVal Max_PartWeightUnitID As Integer, _
  ByVal Quoted_CoatingWeight As Double, ByVal Quoted_CoatingWeightUnitID As Integer, ByVal Quoted_TotalWeight As Double, _
  ByVal Quoted_TotalWeightUnitID As Integer)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Production_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@maxMixCapacity", SqlDbType.Int)
            myCommand.Parameters("@maxMixCapacity").Value = MaxMixCapacity

            myCommand.Parameters.Add("@maxMixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@maxMixCapacityUnitID").Value = MaxMixCapacityUnitID

            myCommand.Parameters.Add("@maxFormingRate", SqlDbType.Int)
            myCommand.Parameters("@maxFormingRate").Value = MaxFormingRate

            myCommand.Parameters.Add("@maxFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@maxFormingRateUnitID").Value = MaxFormingRateUnitID

            myCommand.Parameters.Add("@catchingAbility", SqlDbType.Decimal)
            myCommand.Parameters("@catchingAbility").Value = CatchingAbility

            myCommand.Parameters.Add("@lineSpeedLimitation", SqlDbType.Int)
            myCommand.Parameters("@lineSpeedLimitation").Value = LineSpeedLimitation

            myCommand.Parameters.Add("@catchPercent", SqlDbType.Decimal)
            myCommand.Parameters("@catchPercent").Value = CatchPercent

            myCommand.Parameters.Add("@coatingFactor", SqlDbType.Decimal)
            myCommand.Parameters("@coatingFactor").Value = CoatingFactor

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@offline_SheetsUp", SqlDbType.Int)
            myCommand.Parameters("@offline_SheetsUp").Value = Offline_SheetsUp

            If Offline_BlankCode Is Nothing Then
                Offline_BlankCode = ""
            End If

            myCommand.Parameters.Add("@offline_BlankCode", SqlDbType.VarChar)
            myCommand.Parameters("@offline_BlankCode").Value = Offline_BlankCode

            myCommand.Parameters.Add("@offline_QuotedPressCycles", SqlDbType.Int)
            myCommand.Parameters("@offline_QuotedPressCycles").Value = Offline_QuotedPressCycles

            myCommand.Parameters.Add("@offline_QuotedOfflineRates", SqlDbType.Int)
            myCommand.Parameters("@offline_QuotedOfflineRates").Value = Offline_QuotedOfflineRates

            myCommand.Parameters.Add("@offline_PiecesPerManHour", SqlDbType.Decimal)
            myCommand.Parameters("@offline_PiecesPerManHour").Value = Offline_PiecesPerManHour

            myCommand.Parameters.Add("@offline_PercentRecycle", SqlDbType.Decimal)
            myCommand.Parameters("@offline_PercentRecycle").Value = Offline_PercentRecycle

            myCommand.Parameters.Add("@quoted_MaxPieces", SqlDbType.Int)
            myCommand.Parameters("@quoted_MaxPieces").Value = Quoted_MaxPieces

            myCommand.Parameters.Add("@quoted_MaxPiecesUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_MaxPiecesUnitID").Value = Quoted_MaxPiecesUnitID

            myCommand.Parameters.Add("@max_MaxPieces", SqlDbType.Int)
            myCommand.Parameters("@max_MaxPieces").Value = Max_MaxPieces

            myCommand.Parameters.Add("@max_MaxPiecesUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_MaxPiecesUnitID").Value = Max_MaxPiecesUnitID

            myCommand.Parameters.Add("@quoted_PressCycles", SqlDbType.Int)
            myCommand.Parameters("@quoted_PressCycles").Value = Quoted_PressCycles

            myCommand.Parameters.Add("@quoted_PressCyclesUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_PressCyclesUnitID").Value = Quoted_PressCyclesUnitID

            myCommand.Parameters.Add("@max_PressCycles", SqlDbType.Int)
            myCommand.Parameters("@max_PressCycles").Value = Max_PressCycles

            myCommand.Parameters.Add("@max_PressCyclesUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_PressCyclesUnitID").Value = Max_PressCyclesUnitID

            myCommand.Parameters.Add("@quoted_LineSpeed", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_LineSpeed").Value = Quoted_LineSpeed

            myCommand.Parameters.Add("@quoted_LineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_LineSpeedUnitID").Value = Quoted_LineSpeedUnitID

            myCommand.Parameters.Add("@max_LineSpeed", SqlDbType.Decimal)
            myCommand.Parameters("@max_LineSpeed").Value = Max_LineSpeed

            myCommand.Parameters.Add("@max_LineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_LineSpeedUnitID").Value = Max_LineSpeedUnitID

            myCommand.Parameters.Add("@quoted_NetFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_NetFormingRate").Value = Quoted_NetFormingRate

            myCommand.Parameters.Add("@quoted_NetFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_NetFormingRateUnitID").Value = Quoted_NetFormingRateUnitID

            myCommand.Parameters.Add("@max_NetFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@max_NetFormingRate").Value = Max_NetFormingRate

            myCommand.Parameters.Add("@max_NetFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_NetFormingRateUnitID").Value = Max_NetFormingRateUnitID

            myCommand.Parameters.Add("@quoted_MixCapacity", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_MixCapacity").Value = Quoted_MixCapacity

            myCommand.Parameters.Add("@quoted_MixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_MixCapacityUnitID").Value = Quoted_MixCapacityUnitID

            myCommand.Parameters.Add("@max_MixCapacity", SqlDbType.Decimal)
            myCommand.Parameters("@max_MixCapacity").Value = Max_MixCapacity

            myCommand.Parameters.Add("@max_MixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_MixCapacityUnitID").Value = Max_MixCapacityUnitID

            myCommand.Parameters.Add("@quoted_RecycleRate", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_RecycleRate").Value = Quoted_RecycleRate

            myCommand.Parameters.Add("@quoted_RecycleRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_RecycleRateUnitID").Value = Quoted_RecycleRateUnitID

            myCommand.Parameters.Add("@max_RecycleRate", SqlDbType.Decimal)
            myCommand.Parameters("@max_RecycleRate").Value = Max_RecycleRate

            myCommand.Parameters.Add("@max_RecycleRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_RecycleRateUnitID").Value = Max_RecycleRateUnitID

            myCommand.Parameters.Add("@quoted_PartWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_PartWeight").Value = Quoted_PartWeight

            myCommand.Parameters.Add("@quoted_PartWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_PartWeightUnitID").Value = Quoted_PartWeightUnitID

            myCommand.Parameters.Add("@max_PartWeight", SqlDbType.Decimal)
            myCommand.Parameters("@max_PartWeight").Value = Max_PartWeight

            myCommand.Parameters.Add("@max_PartWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@max_PartWeightUnitID").Value = Max_PartWeightUnitID

            myCommand.Parameters.Add("@quoted_CoatingWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_CoatingWeight").Value = Quoted_CoatingWeight

            myCommand.Parameters.Add("@quoted_CoatingWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_CoatingWeightUnitID").Value = Quoted_CoatingWeightUnitID

            myCommand.Parameters.Add("@quoted_TotalWeight", SqlDbType.Decimal)
            myCommand.Parameters("@quoted_TotalWeight").Value = Quoted_TotalWeight

            myCommand.Parameters.Add("@quoted_TotalWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@quoted_TotalWeightUnitID").Value = Quoted_TotalWeightUnitID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", MaxMixCapacity: " & MaxMixCapacity & ", MaxMixCapacityUnitID: " & MaxMixCapacityUnitID _
            & ", MaxFormingRate: " & MaxFormingRate & ", MaxFormingRateUnitID: " & MaxFormingRateUnitID & ", CatchingAbility: " & CatchingAbility & ", LineSpeedLimitation : " & LineSpeedLimitation _
            & ", CatchPercent: " & CatchPercent & ", CoatingFactor: " & CoatingFactor & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID & ", Offline_SheetsUp: " & Offline_SheetsUp _
            & ", Offline_BlankCode: " & Offline_BlankCode _
            & ", Offline_QuotedPressCycles: " & Offline_QuotedPressCycles & ", Offline_QuotedOfflineRates: " & Offline_QuotedOfflineRates _
            & ", Offline_PiecesPerManHour: " & Offline_PiecesPerManHour & ", Offline_PercentRecycle: " & Offline_PercentRecycle _
            & ", Quoted_MaxPieces: " & Quoted_MaxPieces & ", Quoted_MaxPiecesUnitID: " & Quoted_MaxPiecesUnitID _
            & ", Max_MaxPieces: " & Max_MaxPieces & ", Max_MaxPiecesUnitID: " & Max_MaxPiecesUnitID _
            & ", Quoted_PressCycles: " & Quoted_PressCycles & ", Quoted_PressCyclesUnitID: " & Quoted_PressCyclesUnitID _
            & ", Max_PressCycles: " & Max_PressCycles & ", Max_PressCyclesUnitID: " & Max_PressCyclesUnitID _
            & ", Quoted_LineSpeed: " & Quoted_LineSpeed & ", Quoted_LineSpeedUnitID: " & Quoted_LineSpeedUnitID _
            & ", Max_LineSpeed: " & Max_LineSpeed & ", Max_LineSpeedUnitID: " & Max_LineSpeedUnitID _
            & ", Quoted_MixCapacity: " & Quoted_MixCapacity & ", Quoted_MixCapacityUnitID: " & Quoted_MixCapacityUnitID _
            & ", Max_MixCapacity: " & Max_MixCapacity & ", Max_MixCapacityUnitID: " & Max_MixCapacityUnitID _
            & ", Quoted_RecycleRate: " & Quoted_RecycleRate & ", Quoted_RecycleRateUnitID: " & Quoted_RecycleRateUnitID _
            & ", Max_RecycleRate: " & Max_RecycleRate & ", Max_RecycleRateUnitID: " & Max_RecycleRateUnitID _
            & ", Quoted_PartWeight: " & Quoted_PartWeight & ", Quoted_PartWeightUnitID: " & Quoted_PartWeightUnitID _
            & ", Max_PartWeight: " & Max_PartWeight & ", Max_PartWeightUnitID: " & Max_PartWeightUnitID _
            & ", Quoted_CoatingWeight: " & Quoted_CoatingWeight & ", Quoted_CoatingWeightUnitID: " & Quoted_CoatingWeightUnitID _
            & ", Quoted_TotalWeight: " & Quoted_TotalWeight & ", Quoted_TotalWeightUnitID: " & Quoted_TotalWeightUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetProductionRate(ByVal CostSheetID As Integer)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Production_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetProductionRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetProductionLimit(ByVal CostSheetID As Integer, ByVal ProductionLimitID As Integer, _
    ByVal ProductionLimit As Integer, ByVal ProductionLimitUnitID As Integer, ByVal Ordinal As Integer)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Production_Limit"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@productionLimitID", SqlDbType.Int)
            myCommand.Parameters("@productionLimitID").Value = ProductionLimitID

            myCommand.Parameters.Add("@productionLimit", SqlDbType.Int)
            myCommand.Parameters("@productionLimit").Value = ProductionLimit

            myCommand.Parameters.Add("@productionLimitUnitID", SqlDbType.Int)
            myCommand.Parameters("@productionLimitUnitID").Value = ProductionLimitUnitID

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", ProductionLimitID: " & ProductionLimitID _
            & ", ProductionLimit: " & ProductionLimit & ", ProductionLimitUnitID: " & ProductionLimitUnitID _
            & ", Ordinal: " & Ordinal & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetQuotedInfo(ByVal CostSheetID As Integer, ByVal AccountManagerID As Integer, _
  ByVal StandardCostFactor As Double, ByVal PiecesPerYear As Integer, ByVal Comments As String)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Quoted_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@accountManagerID", SqlDbType.Int)
            myCommand.Parameters("@accountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@standardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@piecesPerYear", SqlDbType.Int)
            myCommand.Parameters("@piecesPerYear").Value = PiecesPerYear

            If Comments Is Nothing Then
                Comments = ""
            End If

            myCommand.Parameters.Add("@comments", SqlDbType.VarChar)
            myCommand.Parameters("@comments").Value = Comments

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", AccountManagerID: " & AccountManagerID _
            & ", StandardCostFactor: " & StandardCostFactor & ", PiecesPerYear: " & PiecesPerYear _
            & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetQuotedInfo(ByVal CostSheetID As Integer, ByVal AccountManagerID As Integer, _
  ByVal StandardCostFactor As Double, ByVal PiecesPerYear As Integer, ByVal Comments As String)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Quoted_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@accountManagerID", SqlDbType.Int)
            myCommand.Parameters("@accountManagerID").Value = AccountManagerID

            myCommand.Parameters.Add("@standardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@piecesPerYear", SqlDbType.Int)
            myCommand.Parameters("@piecesPerYear").Value = PiecesPerYear

            If Comments Is Nothing Then
                Comments = ""
            End If

            myCommand.Parameters.Add("@comments", SqlDbType.VarChar)
            myCommand.Parameters("@comments").Value = Comments

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", AccountManagerID: " & AccountManagerID _
            & ", StandardCostFactor: " & StandardCostFactor & ", PiecesPerYear: " & PiecesPerYear _
            & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetQuotedInfo(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Quoted_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetQuotedInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetMaterial(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetPackaging(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetLabor(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetOverhead(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetMiscCost(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetAdditionalOfflineRate(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Additional_Offline_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetCapital(ByVal RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetSketchMemo(ByVal CostSheetID As Integer, ByVal SketchMemo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Sketch_Memo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            If SketchMemo Is Nothing Then
                SketchMemo = ""
            End If

            myCommand.Parameters.Add("@sketchMemo", SqlDbType.VarChar)
            myCommand.Parameters("@sketchMemo").Value = SketchMemo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", SketchMemo: " & SketchMemo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    'Public Shared Sub InsertCostSheetSketchImage(ByVal CostSheetID As Integer, ByVal SketchImage As Byte())

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Sketch_Image"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
    '        myCommand.Parameters("@costSheetID").Value = CostSheetID

    '        myCommand.Parameters.Add("@sketchImage", SqlDbType.Image)
    '        myCommand.Parameters("@sketchImage").Value = SketchImage

    '        myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertCostSheetSketchImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertCostSheetSketchImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub
    Public Shared Sub UpdateCostSheetSketchImage(ByVal CostSheetID As Integer, ByVal SketchImage As Byte())

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Sketch_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@sketchImage", SqlDbType.Image)
            myCommand.Parameters("@sketchImage").Value = SketchImage

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetSketchImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetSketchImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetSketchMemo(ByVal CostSheetID As Integer, ByVal SketchMemo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Sketch_Memo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@sketchMemo", SqlDbType.VarChar)
            myCommand.Parameters("@sketchMemo").Value = SketchMemo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", SketchMemo: " & SketchMemo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    'Public Shared Sub DeleteCostSheetSketchMemo(ByVal CostSheetID As Integer)


    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Sketch_Memo"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
    '        myCommand.Parameters("@costSheetID").Value = CostSheetID

    '        myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
    '        myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        myConnection.Open()
    '        myCommand.ExecuteNonQuery()

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "DeleteCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("DeleteCostSheetSketchMemo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Sub
    Public Shared Sub DeleteCostSheetSketchImage(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Sketch_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetSketchImage: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetSketchImage: " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetCompositePartSpecification(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, _
    ByVal PartThickness As Double, ByVal PartThicknessUnitID As Integer, ByVal PartSpecificGravity As Double, _
    ByVal PartSpecificGravityUnitID As Integer, ByVal PartArea As Double, ByVal PartAreaUnitID As Integer, _
    ByVal RSSWeight As Double, ByVal RSSWeightUnitID As Integer, ByVal AntiBlockCoating As Double, _
    ByVal AntiBlockCoatingUnitID As String, ByVal HotMeldAdhesive As Double, ByVal HotMeldAdhesiveUnitID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Composite_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@partThickness", SqlDbType.Decimal)
            myCommand.Parameters("@partThickness").Value = PartThickness

            myCommand.Parameters.Add("@partThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@partThicknessUnitID").Value = PartThicknessUnitID

            myCommand.Parameters.Add("@partSpecificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@partSpecificGravity").Value = PartSpecificGravity

            myCommand.Parameters.Add("@partSpecificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@partSpecificGravityUnitID").Value = PartSpecificGravityUnitID

            myCommand.Parameters.Add("@partArea", SqlDbType.Decimal)
            myCommand.Parameters("@partArea").Value = PartArea

            myCommand.Parameters.Add("@partAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@partAreaUnitID").Value = PartAreaUnitID

            myCommand.Parameters.Add("@rssWeight", SqlDbType.Decimal)
            myCommand.Parameters("@rssWeight").Value = RSSWeight

            myCommand.Parameters.Add("@rssWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@rssWeightUnitID").Value = RSSWeightUnitID

            myCommand.Parameters.Add("@antiBlockCoating", SqlDbType.Decimal)
            myCommand.Parameters("@antiBlockCoating").Value = AntiBlockCoating

            myCommand.Parameters.Add("@antiBlockCoatingUnitID", SqlDbType.Int)
            myCommand.Parameters("@antiBlockCoatingUnitID").Value = AntiBlockCoatingUnitID

            myCommand.Parameters.Add("@hotMeldAdhesive", SqlDbType.Decimal)
            myCommand.Parameters("@hotMeldAdhesive").Value = HotMeldAdhesive

            myCommand.Parameters.Add("@hotMeldAdhesiveUnitID", SqlDbType.Int)
            myCommand.Parameters("@hotMeldAdhesiveUnitID").Value = HotMeldAdhesiveUnitID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", FormulaID: " & FormulaID & ", PartThickness: " & PartThickness _
            & ", PartThicknessUnitID: " & PartThicknessUnitID & ", PartSpecificGravity: " & PartSpecificGravity _
            & ", PartSpecificGravityUnitID: " & PartSpecificGravityUnitID & ", PartArea: " & PartArea _
            & ", PartAreaUnitID: " & PartAreaUnitID & ", RSSWeight: " & RSSWeight _
            & ", RSSWeightUnitID: " & RSSWeightUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", HotMeldAdhesive: " & HotMeldAdhesive _
            & ", HotMeldAdhesiveUnitID: " & HotMeldAdhesiveUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetCompositePartSpecification(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, _
    ByVal PartThickness As Double, ByVal PartThicknessUnitID As Integer, ByVal PartSpecificGravity As Double, _
   ByVal PartSpecificGravityUnitID As Integer, ByVal PartArea As Double, ByVal PartAreaUnitID As Integer, _
   ByVal RSSWeight As Double, ByVal RSSWeightUnitID As Integer, ByVal AntiBlockCoating As Double, _
   ByVal AntiBlockCoatingUnitID As Integer, ByVal HotMeldAdhesive As Double, ByVal HotMeldAdhesiveUnitID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Composite_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@partThickness", SqlDbType.Decimal)
            myCommand.Parameters("@partThickness").Value = PartThickness

            myCommand.Parameters.Add("@partThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@partThicknessUnitID").Value = PartThicknessUnitID

            myCommand.Parameters.Add("@partSpecificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@partSpecificGravity").Value = PartSpecificGravity

            myCommand.Parameters.Add("@partSpecificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@partSpecificGravityUnitID").Value = PartSpecificGravityUnitID

            myCommand.Parameters.Add("@partArea", SqlDbType.Decimal)
            myCommand.Parameters("@partArea").Value = PartArea

            myCommand.Parameters.Add("@partAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@partAreaUnitID").Value = PartAreaUnitID

            myCommand.Parameters.Add("@rssWeight", SqlDbType.Decimal)
            myCommand.Parameters("@rssWeight").Value = RSSWeight

            myCommand.Parameters.Add("@rssWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@rssWeightUnitID").Value = RSSWeightUnitID

            myCommand.Parameters.Add("@antiBlockCoating", SqlDbType.Decimal)
            myCommand.Parameters("@antiBlockCoating").Value = AntiBlockCoating

            myCommand.Parameters.Add("@antiBlockCoatingUnitID", SqlDbType.Int)
            myCommand.Parameters("@antiBlockCoatingUnitID").Value = AntiBlockCoatingUnitID

            myCommand.Parameters.Add("@hotMeldAdhesive", SqlDbType.Decimal)
            myCommand.Parameters("@hotMeldAdhesive").Value = HotMeldAdhesive

            myCommand.Parameters.Add("@hotMeldAdhesiveUnitID", SqlDbType.Int)
            myCommand.Parameters("@hotMeldAdhesiveUnitID").Value = HotMeldAdhesiveUnitID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", FormulaID: " & FormulaID & ", PartThickness: " & PartThickness _
            & ", PartThicknessUnitID: " & PartThicknessUnitID & ", PartSpecificGravity: " & PartSpecificGravity _
            & ", PartSpecificGravityUnitID: " & PartSpecificGravityUnitID & ", PartAreaValue: " & PartArea _
            & ", PartAreaUnitID: " & PartAreaUnitID & ", RSSWeight: " & RSSWeight _
            & ", RSSWeightUnitID: " & RSSWeightUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", HotMeldAdhesive: " & HotMeldAdhesive _
            & ", HotMeldAdhesiveUnitID: " & HotMeldAdhesiveUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetCompositePartSpecification(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Composite_Part_Specification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetMoldedBarrier(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, ByVal BarrierLength As Double, _
    ByVal BarrierLengthUnitID As Integer, ByVal BarrierWidth As Double, ByVal BarrierWidthUnitID As Integer, ByVal BarrierThickness As Double, ByVal BarrierThicknessUnitID As Integer, _
    ByVal BarrierBlankArea As Double, ByVal BarrierBlankAreaUnitID As Integer, ByVal SpecificGravity As Double, ByVal SpecificGravityUnitID As Integer, _
    ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, ByVal BlankWeight As Double, ByVal BlankWeightUnitID As Integer, _
    ByVal AntiBlockCoating As Double, ByVal AntiBlockCoatingUnitID As Integer, ByVal TotalBarrierWeight As Double, _
    ByVal TotalBarrierWeightUnitID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Molded_Barrier"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@barrierLength", SqlDbType.Decimal)
            myCommand.Parameters("@barrierLength").Value = BarrierLength

            myCommand.Parameters.Add("@barrierLengthUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierLengthUnitID").Value = BarrierLengthUnitID

            myCommand.Parameters.Add("@barrierWidth", SqlDbType.Decimal)
            myCommand.Parameters("@barrierWidth").Value = BarrierWidth

            myCommand.Parameters.Add("@barrierwidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierwidthUnitID").Value = BarrierWidthUnitID

            myCommand.Parameters.Add("@barrierThickness", SqlDbType.Decimal)
            myCommand.Parameters("@barrierThickness").Value = BarrierThickness

            myCommand.Parameters.Add("@barrierThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierThicknessUnitID").Value = BarrierThicknessUnitID

            myCommand.Parameters.Add("@barrierBlankArea", SqlDbType.Decimal)
            myCommand.Parameters("@barrierBlankArea").Value = BarrierBlankArea

            myCommand.Parameters.Add("@barrierBlankAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierBlankAreaUnitID").Value = BarrierBlankAreaUnitID

            myCommand.Parameters.Add("@specificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@specificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@specificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@specificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@blankWeight", SqlDbType.Decimal)
            myCommand.Parameters("@blankWeight").Value = BlankWeight

            myCommand.Parameters.Add("@blankWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@blankWeightUnitID").Value = BlankWeightUnitID

            myCommand.Parameters.Add("@antiBlockCoating", SqlDbType.Decimal)
            myCommand.Parameters("@antiBlockCoating").Value = AntiBlockCoating

            myCommand.Parameters.Add("@antiBlockCoatingUnitID", SqlDbType.Int)
            myCommand.Parameters("@antiBlockCoatingUnitID").Value = AntiBlockCoatingUnitID

            myCommand.Parameters.Add("@totalBarrierWeight", SqlDbType.Decimal)
            myCommand.Parameters("@totalBarrierWeight").Value = TotalBarrierWeight

            myCommand.Parameters.Add("@totalBarrierWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@totalBarrierWeightUnitID").Value = TotalBarrierWeightUnitID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", FormulaID: " & FormulaID _
            & ", BarrierLength: " & BarrierLength & ", BarrierLengthUnitID: " & BarrierLengthUnitID _
            & ", BarrierWidth: " & BarrierWidth & ", BarrierWidthUnitID: " & BarrierWidthUnitID _
            & ", BarrierThickness: " & BarrierThickness & ", BarrierThicknessUnitID: " & BarrierThicknessUnitID _
            & ", BarrierBlankArea: " & BarrierBlankArea & ", SpecificGravity: " & SpecificGravity _
            & ", SpecificGravityUnitID: " & SpecificGravityUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID & ", BlankWeight: " & BlankWeight _
            & ", BlankWeightUnitID: " & BlankWeightUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", TotalBarrierWeight: " & TotalBarrierWeight _
            & ", TotalBarrierWeightUnitID: " & TotalBarrierWeightUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetMoldedBarrier(ByVal CostSheetID As Integer, ByVal FormulaID As Integer, ByVal BarrierLength As Double, _
 ByVal BarrierLengthUnitID As Integer, ByVal BarrierWidth As Double, ByVal BarrierWidthUnitID As Integer, ByVal BarrierThickness As Double, _
 ByVal BarrierThicknessUnitID As Integer, ByVal BarrierBlankArea As Double, ByVal BarrierBlankAreaUnitID As Integer, _
 ByVal SpecificGravity As Double, ByVal SpecificGravityUnitID As Integer, ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, _
 ByVal BlankWeight As Double, ByVal BlankWeightUnitID As Integer, ByVal AntiBlockCoating As Double, ByVal AntiBlockCoatingUnitID As Integer, _
 ByVal TotalBarrierWeight As Double, ByVal TotalBarrierWeightUnitID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Molded_Barrier"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@barrierLength", SqlDbType.Decimal)
            myCommand.Parameters("@barrierLength").Value = BarrierLength

            myCommand.Parameters.Add("@barrierLengthUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierlengthUnitID").Value = BarrierLengthUnitID

            myCommand.Parameters.Add("@barrierWidth", SqlDbType.Decimal)
            myCommand.Parameters("@barrierWidth").Value = BarrierWidth

            myCommand.Parameters.Add("@barrierWidthUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierWidthUnitID").Value = BarrierWidthUnitID

            myCommand.Parameters.Add("@barrierThickness", SqlDbType.Decimal)
            myCommand.Parameters("@barrierThickness").Value = BarrierThickness

            myCommand.Parameters.Add("@barrierThicknessUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierThicknessUnitID").Value = BarrierThicknessUnitID

            myCommand.Parameters.Add("@barrierBlankArea", SqlDbType.Decimal)
            myCommand.Parameters("@barrierBlankArea").Value = BarrierBlankArea

            myCommand.Parameters.Add("@barrierBlankAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@barrierBlankAreaUnitID").Value = BarrierBlankAreaUnitID

            myCommand.Parameters.Add("@specificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@specificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@specificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@specificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@weightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@weightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@weightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@weightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@blankWeight", SqlDbType.Decimal)
            myCommand.Parameters("@blankWeight").Value = BlankWeight

            myCommand.Parameters.Add("@blankWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@blankWeightUnitID").Value = BlankWeightUnitID

            myCommand.Parameters.Add("@antiBlockCoating", SqlDbType.Decimal)
            myCommand.Parameters("@antiBlockCoating").Value = AntiBlockCoating

            myCommand.Parameters.Add("@antiBlockCoatingUnitID", SqlDbType.Int)
            myCommand.Parameters("@antiBlockCoatingUnitID").Value = AntiBlockCoatingUnitID

            myCommand.Parameters.Add("@totalBarrierWeight", SqlDbType.Decimal)
            myCommand.Parameters("@totalBarrierWeight").Value = TotalBarrierWeight

            myCommand.Parameters.Add("@totalBarrierWeightUnitID", SqlDbType.Int)
            myCommand.Parameters("@totalBarrierWeightUnitID").Value = TotalBarrierWeightUnitID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", FormulaID: " & FormulaID _
            & ", BarrierLength: " & BarrierLength & ", BarrierLengthUnitID: " & BarrierLengthUnitID _
            & ", BarrierWidth: " & BarrierWidth & ", BarrierWidthUnitID: " & BarrierWidthUnitID _
            & ", BarrierThickness: " & BarrierThickness & ", BarrierThicknessUnitID: " & BarrierThicknessUnitID _
            & ", BarrierBlankArea: " & BarrierBlankArea & ", SpecificGravity: " & SpecificGravity _
            & ", SpecificGravityUnitID: " & SpecificGravityUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID & ", BlankWeight: " & BlankWeight _
            & ", BlankWeightUnitID: " & BlankWeightUnitID & ", AntiBlockCoating: " & AntiBlockCoating _
            & ", AntiBlockCoatingUnitID: " & AntiBlockCoatingUnitID & ", TotalBarrierWeight: " & TotalBarrierWeight _
            & ", TotalBarrierWeightUnitsID: " & TotalBarrierWeightUnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub DeleteCostSheetMoldedBarrier(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Molded_Barrier"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetCompositePartSpecification : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetCustomerProgram(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetDepartment(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Department"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetAdditionalOfflineRate(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Additional_Offline_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetMaterial(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetMaterialReplaceObsolete(ByVal NewCostSheetID As Integer, _
    ByVal OldCostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Material_Replace_Obsolete"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetMaterialReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetMaterialReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetPackaging(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & "OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetPackagingReplaceObsolete(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Packaging_Replace_Obsolete"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@NewCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@OldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@OldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & "OldCostSheetID: " & OldCostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetPackagingReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetPackagingReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyCostSheetLabor(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetOverhead(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetMiscCost(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, _
    ByVal CopyType As String, ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & "OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType & ", FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetMiscCostType : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetMiscCostType : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetCapital(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer, ByVal CopyType As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = CopyType

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & ", OldCostSheetID: " & OldCostSheetID _
            & ", CopyType: " & CopyType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetCatchingAbilityFactor(ByVal FactorID As Integer, ByVal PartLength As Double, _
   ByVal isSideBySide As Boolean, ByVal isAll As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Catching_Ability_Factor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@factorID", SqlDbType.Int)
            myCommand.Parameters("@factorID").Value = FactorID

            myCommand.Parameters.Add("@partLength", SqlDbType.Decimal)
            myCommand.Parameters("@partLength").Value = PartLength

            myCommand.Parameters.Add("@isSideBySide", SqlDbType.Bit)
            myCommand.Parameters("@isSideBySide").Value = isSideBySide

            myCommand.Parameters.Add("@isAll", SqlDbType.Bit)
            myCommand.Parameters("@isAll").Value = isAll

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CatchingAbilityFactor")
            GetCostSheetCatchingAbilityFactor = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ", PartLength: " & PartLength _
            & ", isSideBySide: " & isSideBySide & ", isAll: " & isAll _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetCatchingAbilityFactor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetFormulaCoatingFactor(ByVal FactorID As Integer, ByVal FormulaID As Integer, ByVal Thickness As Double) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Coating_Factor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@factorID", SqlDbType.Int)
            myCommand.Parameters("@factorID").Value = FactorID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@thickness", SqlDbType.Decimal)
            myCommand.Parameters("@thickness").Value = Thickness

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CatchingAbilityFactor")
            GetFormulaCoatingFactor = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ", FormulaID: " & FormulaID _
            & ", Thickness: " & Thickness & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaCoatingFactor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetFormulaDeplugFactor(ByVal FactorID As Integer, ByVal FormulaID As Integer, ByVal Thickness As Double) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Deplug_Factor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@factorID", SqlDbType.Int)
            myCommand.Parameters("@factorID").Value = FactorID

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myCommand.Parameters.Add("@thickness", SqlDbType.Decimal)
            myCommand.Parameters("@thickness").Value = Thickness

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FormulaDeplugFactor")
            GetFormulaDeplugFactor = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ", FormulaID: " & FormulaID _
            & ", Thickness: " & Thickness & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaDeplugFactor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetMinimumProductionLimit(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Minumum_Production_Limit"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCostMinimumProductionLimit")
            GetCostSheetMinimumProductionLimit = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetMinimumProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetMinimumProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetMinimumProductionLimit = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetFormulaDeplugFactorCount(ByVal FormulaID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Formula_Deplug_Factor_Count"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@formulaID", SqlDbType.Int)
            myCommand.Parameters("@formulaID").Value = FormulaID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FormulaDeplugFactorCount")
            GetFormulaDeplugFactorCount = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFormulaDeplugFactorCount : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaDeplugFactorCount : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetFormulaDeplugFactorCount = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetMaterial(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@materialID", SqlDbType.Int)
            myCommand.Parameters("@materialID").Value = 0


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetMaterial")
            GetCostSheetMaterial = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetMaterial = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function CalculateCostSheetMaterial(ByVal CostSheetID As Integer, ByVal bFormulaFleeceType As Boolean, _
    ByVal PartWeight As Double, ByVal CoatingWeight As Double, ByVal QuotedStandardCostFactor As Double) As Double

        Dim iRowID As Integer = 0
        Dim iMaterialID As Integer = 0
        Dim dQuantity As Double = 0
        Dim dUsageFactor As Double = 0
        Dim dCostPerUnit As Double = 0
        Dim dFreight As Double = 0
        Dim dStandardCostFactor As Double = 0
        Dim dTempStandardCostFactor As Double = 0
        Dim dQuoteCostFactor As Double = 0
        Dim iOrdinal As Integer = 0

        Dim dMaterialWeight As Double = 0

        Dim dStandardCostPerUnitWOScrap As Double = 0
        Dim dStandardCostPerUnit As Double = 0

        Dim dTotalStandardCostPerUnitWOScrap As Double = 0
        Dim dTotalStandardCostPerUnit As Double = 0

        Dim ds As DataSet
        Dim dsMaterial As DataSet

        Dim isCoating As Boolean = False

        Dim iRowCounter As Integer = 0

        Try
            ds = CostingModule.GetCostSheetMaterial(CostSheetID)

            If commonFunctions.CheckDataset(ds) = True Then

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iMaterialID = 0
                    dQuantity = 0
                    dUsageFactor = 0
                    dCostPerUnit = 0
                    dFreight = 0
                    dStandardCostFactor = 0
                    dQuoteCostFactor = 0
                    iOrdinal = 0
                    isCoating = False
                    dMaterialWeight = 0

                    dTempStandardCostFactor = 0
                    dStandardCostPerUnitWOScrap = 0
                    dStandardCostPerUnit = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("MaterialID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("MaterialID") > 0 Then
                            iMaterialID = ds.Tables(0).Rows(iRowCounter).Item("MaterialID")
                        End If
                    End If

                    If iMaterialID > 0 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("Quantity") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Quantity") <> 0 Then
                                dQuantity = ds.Tables(0).Rows(iRowCounter).Item("Quantity")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("UsageFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("UsageFactor") <> 0 Then
                                dUsageFactor = ds.Tables(0).Rows(iRowCounter).Item("UsageFactor")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit") <> 0 Then
                                dCostPerUnit = ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("FreightCost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("FreightCost") <> 0 Then
                                dFreight = ds.Tables(0).Rows(iRowCounter).Item("FreightCost")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") <> 0 Then
                                dStandardCostFactor = ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor")
                            End If
                        End If

                        If dStandardCostFactor = 0 Then
                            dTempStandardCostFactor = QuotedStandardCostFactor
                        Else
                            dTempStandardCostFactor = dStandardCostFactor
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("QuoteCostFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("QuoteCostFactor") <> 0 Then
                                dQuoteCostFactor = ds.Tables(0).Rows(iRowCounter).Item("QuoteCostFactor")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        dsMaterial = CostingModule.GetMaterial(iMaterialID, "", "", "", 0, 0, "", "", False, False, False, False, False, False)
                        If commonFunctions.CheckDataset(dsMaterial) = True Then
                            If dsMaterial.Tables(0).Rows(0).Item("isCoating") IsNot System.DBNull.Value Then
                                isCoating = dsMaterial.Tables(0).Rows(0).Item("isCoating")
                            End If
                        End If

                        'dMaterialWeight = dUsageFactor * PartWeight
                        'DCADE 2009-Nov-10 - round most numbers to 4 decimals during calculations
                        dMaterialWeight = Format(dUsageFactor * Format(PartWeight, "####.0000"), "####.0000")
                        If isCoating = True Then
                            If dMaterialWeight = 0 Then
                                'DCADE 2009-Nov-10 - orginally Coating Weight was set to MaterialWeight only. But sometimes Coating Weight is 0
                                If CoatingWeight <> 0 Then
                                    dMaterialWeight = CoatingWeight
                                Else
                                    dMaterialWeight = dQuantity
                                End If
                            End If
                        Else
                            If dMaterialWeight = 0 Then
                                dMaterialWeight = dQuantity
                            End If
                        End If

                        If bFormulaFleeceType = True Then 'if fleece, convert g to lbs
                            If dUsageFactor * PartWeight <> 0 Then
                                dMaterialWeight = dMaterialWeight / 454
                            End If
                        End If

                        dStandardCostPerUnitWOScrap = dMaterialWeight * (dCostPerUnit + dFreight)

                        'force 0.00005 to round up to 0.00001
                        dStandardCostPerUnitWOScrap += 0.000001

                        dStandardCostPerUnitWOScrap = Round(dStandardCostPerUnitWOScrap, 4)
                        dStandardCostPerUnit = Round(dStandardCostPerUnitWOScrap * dTempStandardCostFactor, 4)

                        'round
                        CostingModule.UpdateCostSheetMaterial(iRowID, iMaterialID, dMaterialWeight, dUsageFactor, dCostPerUnit, dFreight, dStandardCostFactor, dQuoteCostFactor, dStandardCostPerUnitWOScrap, dStandardCostPerUnit, iOrdinal)

                        'add to total
                        dTotalStandardCostPerUnitWOScrap += dStandardCostPerUnitWOScrap
                        dTotalStandardCostPerUnit += dStandardCostPerUnit

                    End If
                Next
            End If

            'updata totals table
            CostingModule.UpdateCostSheetTotalMaterial(CostSheetID, dTotalStandardCostPerUnitWOScrap, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", QuotedStandardCostFactor: " & QuotedStandardCostFactor _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetMaterial = dTotalStandardCostPerUnit

    End Function
    Public Shared Sub UpdateCostSheetMaterial(ByVal RowID As Integer, ByVal MaterialID As Integer, _
    ByVal Quantity As Double, ByVal UsageFactor As Double, ByVal CostPerUnit As Double, ByVal FreightCost As Double, _
    ByVal StandardCostFactor As Double, ByVal QuoteCostFactor As Double, _
    ByVal StandardCostPerUnitWOScrap As Double, ByVal StandardCostPerUnit As Double, ByVal Ordinal As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@materialID", SqlDbType.Int)
            myCommand.Parameters("@materialID").Value = MaterialID

            myCommand.Parameters.Add("@quantity", SqlDbType.Decimal)
            myCommand.Parameters("@quantity").Value = Quantity

            myCommand.Parameters.Add("@usageFactor", SqlDbType.Decimal)
            myCommand.Parameters("@usageFactor").Value = UsageFactor

            myCommand.Parameters.Add("@costPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@costPerUnit").Value = CostPerUnit

            myCommand.Parameters.Add("@freightCost", SqlDbType.Decimal)
            myCommand.Parameters("@freightCost").Value = FreightCost

            myCommand.Parameters.Add("@standardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@quoteCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@quoteCostFactor").Value = QuoteCostFactor

            myCommand.Parameters.Add("@standardCostPerUnitWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitWOScrap").Value = StandardCostPerUnitWOScrap

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", MaterialID: " & MaterialID & ", Quantity: " & Quantity _
            & ", UsageFactor: " & UsageFactor & ", CostPerUnit: " & CostPerUnit _
            & ", StandardCostFactor: " & StandardCostFactor & ", QuoteCostFactor: " & QuoteCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", Ordinal: " & Ordinal & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetPackaging(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@materialID", SqlDbType.Int)
            myCommand.Parameters("@materialID").Value = 0


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPackaging")
            GetCostSheetPackaging = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPackaging = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function CalculateCostSheetPackaging(ByVal CostSheetID As Integer, ByVal QuotedStandardCostFactor As Double) As Double

        Dim iRowID As Integer = 0
        Dim iMaterialID As Integer = 0
        Dim dCostPerUnit As Double = 0
        Dim dUnitsNeeded As Double = 0
        Dim iPartsPerContainer As Integer = 0
        Dim dUnitsNeededDivPartsPerContainer As Double = 0
        Dim isUsed As Boolean = False
        Dim iOrdinal As Integer = 0

        Dim dStandardCostFactor As Double = 0
        Dim dTempStandardCostFactor As Double = 0
        Dim dStandardCostPerUnitWOScrap As Double = 0
        Dim dStandardCostPerUnit As Double = 0

        Dim dTotalStandardCostPerUnitWOScrap As Double = 0
        Dim dTotalStandardCostPerUnit As Double = 0

        Dim iRowCounter As Integer = 0

        Try
            Dim ds As DataSet = CostingModule.GetCostSheetPackaging(CostSheetID)

            If commonFunctions.CheckDataset(ds) = True Then

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iMaterialID = 0
                    dCostPerUnit = 0
                    dUnitsNeeded = 0
                    iPartsPerContainer = 0
                    dUnitsNeededDivPartsPerContainer = 0
                   
                    dStandardCostFactor = 0
                    dTempStandardCostFactor = 0
                    dStandardCostPerUnitWOScrap = 0
                    dStandardCostPerUnit = 0
                    isUsed = False
                    iOrdinal = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("MaterialID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("MaterialID") > 0 Then
                            iMaterialID = ds.Tables(0).Rows(iRowCounter).Item("MaterialID")
                        End If
                    End If

                    If iMaterialID > 0 Then

                        If ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit") <> 0 Then
                                dCostPerUnit = ds.Tables(0).Rows(iRowCounter).Item("CostPerUnit")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("UnitsNeeded") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("UnitsNeeded") <> 0 Then
                                dUnitsNeeded = ds.Tables(0).Rows(iRowCounter).Item("UnitsNeeded")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("PartsPerContainer") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("PartsPerContainer") <> 0 Then
                                iPartsPerContainer = ds.Tables(0).Rows(iRowCounter).Item("PartsPerContainer")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("UnitsNeededDIVPartsPerContainer") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("UnitsNeededDIVPartsPerContainer") <> 0 Then
                                dUnitsNeededDivPartsPerContainer = ds.Tables(0).Rows(iRowCounter).Item("UnitsNeededDIVPartsPerContainer")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isUsed") IsNot System.DBNull.Value Then
                            isUsed = ds.Tables(0).Rows(iRowCounter).Item("isUsed")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") <> 0 Then
                                dStandardCostFactor = ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor")
                            End If
                        End If

                        If dStandardCostFactor = 0 Then
                            dTempStandardCostFactor = QuotedStandardCostFactor
                        Else
                            dTempStandardCostFactor = dStandardCostFactor
                        End If

                        'start putting scrap factor on StandardCostPerUnit                      
                        dStandardCostPerUnitWOScrap = Round(dUnitsNeededDivPartsPerContainer * dCostPerUnit, 4)

                        'force 0.00005 to round up to 0.00001
                        dStandardCostPerUnitWOScrap += 0.000001

                        dStandardCostPerUnitWOScrap = Round(dStandardCostPerUnitWOScrap, 4)
                        dStandardCostPerUnit = Round(dStandardCostPerUnitWOScrap * dTempStandardCostFactor, 4)

                        CostingModule.UpdateCostSheetPackaging(iRowID, iMaterialID, dCostPerUnit, dUnitsNeeded, iPartsPerContainer, dStandardCostFactor, dStandardCostPerUnitWOScrap, dStandardCostPerUnit, isUsed, iOrdinal)

                        dTotalStandardCostPerUnitWOScrap += dStandardCostPerUnitWOScrap
                        dTotalStandardCostPerUnit += dStandardCostPerUnit

                    End If
                Next

            End If

            'updata totals table
            CostingModule.UpdateCostSheetTotalPackaging(CostSheetID, dTotalStandardCostPerUnitWOScrap, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", QuotedStandardCostFactor: " & QuotedStandardCostFactor _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetPackaging = dTotalStandardCostPerUnit

    End Function

    Public Shared Sub UpdateCostSheetPackaging(ByVal RowID As Integer, ByVal MaterialID As Integer, ByVal CostPerUnit As Double, _
    ByVal UnitsNeeded As Double, ByVal PartsPerContainer As Integer, ByVal StandardCostFactor As Double, _
    ByVal StandardCostPerUnitWOScrap As Double, ByVal StandardCostPerUnit As Double, _
    ByVal isUsed As Boolean, ByVal Ordinal As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@materialID", SqlDbType.Int)
            myCommand.Parameters("@materialID").Value = MaterialID

            myCommand.Parameters.Add("@costPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@costPerUnit").Value = CostPerUnit

            myCommand.Parameters.Add("@unitsNeeded", SqlDbType.Decimal)
            myCommand.Parameters("@unitsNeeded").Value = UnitsNeeded

            myCommand.Parameters.Add("@partsPerContainer", SqlDbType.Int)
            myCommand.Parameters("@partsPerContainer").Value = PartsPerContainer

            myCommand.Parameters.Add("@StandardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@StandardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@StandardCostPerUnitWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@StandardCostPerUnitWOScrap").Value = StandardCostPerUnitWOScrap

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@isUsed ", SqlDbType.Bit)
            myCommand.Parameters("@isUsed ").Value = isUsed

            myCommand.Parameters.Add("@ordinal", SqlDbType.VarChar)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", MaterialID: " & MaterialID _
            & ", CostPerUnit: " & CostPerUnit _
            & ", UnitsNeeded: " & UnitsNeeded _
            & ", PartsPerContainer: " & PartsPerContainer _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", isUsed: " & isUsed & ", Ordinal: " & Ordinal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPackaging : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetLabor(ByVal CostSheetID As Integer, ByVal LaborID As Integer, ByVal filterOffline As Boolean, ByVal isOffline As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            myCommand.Parameters.Add("@filterOffline", SqlDbType.Bit)
            myCommand.Parameters("@filterOffline").Value = filterOffline

            myCommand.Parameters.Add("@isOffline", SqlDbType.Bit)
            myCommand.Parameters("@isOffline").Value = isOffline

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCostSheetLabor")
            GetCostSheetLabor = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", LaborID: " & LaborID _
            & ", filterOffline: " & filterOffline & ", isOffline: " & isOffline _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetLabor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function CalculateCostSheetLabor(ByVal CostSheetID As Integer, ByVal TemplateID As Integer, _
    ByVal QuotedMaxPieces As Integer, ByVal ProductionRate As Integer, ByVal QuotedStandardCostFactor As Double, _
    ByVal OfflineRate As Integer) As Double

        Dim iRowID As Integer = 0
        Dim iLaborID As Integer = 0
        Dim dRate As Double = 0
        Dim dCrewSize As Double = 0
        Dim dStandardCostFactor As Double = 0
        Dim isOffline As Boolean = False
        Dim iOrdinal As Integer = 0

        Dim dsAdditionalOfflineRate As DataSet

        Dim dTempAdditionalOfflineRate As Double = 0

        Dim dTempStandardCostFactor As Double = 0
        Dim dStandardCostPerUnitWOScrap As Double = 0
        Dim dStandardCostPerUnit As Double = 0

        Dim dTotalStandardCostPerUnitWOScrap As Double = 0
        Dim dTotalStandardCostPerUnit As Double = 0

        Dim iRowCounter As Integer = 0
        Dim iRowCounterAdditionalOfflineRate As Integer = 0

        Try
            Dim ds As DataSet = CostingModule.GetCostSheetLabor(CostSheetID, 0, False, False)

            If commonFunctions.CheckDataset(ds) = True Then

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iLaborID = 0
                    dRate = 0
                    dCrewSize = 0
                    isOffline = False
                    iOrdinal = 0

                    dTempAdditionalOfflineRate = 0
                    dStandardCostFactor = 0
                    dTempStandardCostFactor = 0
                    dStandardCostPerUnitWOScrap = 0
                    dStandardCostPerUnit = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("LaborID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("LaborID") > 0 Then
                            iLaborID = ds.Tables(0).Rows(iRowCounter).Item("LaborID")
                        End If
                    End If

                    If iLaborID > 0 Then

                        If ds.Tables(0).Rows(iRowCounter).Item("Rate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Rate") > 0 Then
                                dRate = ds.Tables(0).Rows(iRowCounter).Item("Rate")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") > 0 Then
                                dCrewSize = ds.Tables(0).Rows(iRowCounter).Item("CrewSize")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isOffline") IsNot System.DBNull.Value Then
                            isOffline = ds.Tables(0).Rows(iRowCounter).Item("isOffline")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") > 0 Then
                                dStandardCostFactor = ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor")
                            End If
                        End If

                        If dStandardCostFactor = 0 Then
                            dTempStandardCostFactor = QuotedStandardCostFactor
                        Else
                            dTempStandardCostFactor = dStandardCostFactor
                        End If

                        'need to get additional offline rate if match exists
                        dsAdditionalOfflineRate = CostingModule.GetCostSheetAdditionalOfflineRate(CostSheetID, 0)
                        If commonFunctions.CheckDataset(dsAdditionalOfflineRate) = True Then
                            For iRowCounterAdditionalOfflineRate = 0 To dsAdditionalOfflineRate.Tables(0).Rows.Count - 1
                                If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") IsNot System.DBNull.Value Then
                                    If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") > 0 Then
                                        If iLaborID = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") Then
                                            If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour") IsNot System.DBNull.Value Then
                                                dTempAdditionalOfflineRate = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour")
                                            End If 'if pieces per hour is not null
                                        End If ' iLabor is same as Additional Offline Labor ID
                                    End If 'if additional offline labor > 0
                                End If 'if additional offline labor is not null
                            Next 'end loop                        
                        End If ' if dsAdditionalOfflineRate is not empty recordset

                        
                        If TemplateID <> 12 And TemplateID <> 13 Then 'if not Molding Chicago and not inj-mold-valpo
                            If isOffline = False Then
                                If QuotedMaxPieces > 0 Then
                                    'dStandardCostPerUnit = (dCrewSize * dRate) / QuotedMaxPieces
                                    dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / QuotedMaxPieces
                                End If
                            Else
                                If dTempAdditionalOfflineRate = 0 Then
                                    If OfflineRate > 0 Then
                                        'dStandardCostPerUnit = (dCrewSize * dRate) / OfflineRate
                                        dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / OfflineRate
                                    End If 'if OfflineRate > 0
                                Else
                                    If dTempAdditionalOfflineRate > 0 Then 'If OfflineRate > 0 Then
                                        'dStandardCostPerUnit = (dCrewSize * dRate) / dTempAdditionalOfflineRate
                                        dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / dTempAdditionalOfflineRate
                                    End If 'if OfflineRate > 0
                                End If 'dTempAdditionalOfflineRate = 0
                            End If 'If isOffline = False
                        Else
                            If isOffline = False Then
                                If ProductionRate > 0 Then
                                    'dStandardCostPerUnit = (dCrewSize * dRate) / ProductionRate
                                    dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / ProductionRate
                                End If
                            Else
                                If dTempAdditionalOfflineRate = 0 Then
                                    If OfflineRate > 0 Then
                                        'dStandardCostPerUnit = (dCrewSize * dRate) / OfflineRate
                                        dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / OfflineRate
                                    Else 'maybe should be 0 
                                        If ProductionRate > 0 Then
                                            'dStandardCostPerUnit = (dCrewSize * dRate) / ProductionRate
                                            dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / ProductionRate
                                        End If

                                    End If 'if OfflineRate > 0
                                Else
                                    If dTempAdditionalOfflineRate > 0 Then 'If OfflineRate > 0 Then
                                        'dStandardCostPerUnit = (dCrewSize * dRate) / dTempAdditionalOfflineRate
                                        dStandardCostPerUnitWOScrap = (dCrewSize * dRate) / dTempAdditionalOfflineRate
                                    End If 'if OfflineRate > 0
                                End If 'dTempAdditionalOfflineRate = 0
                            End If ' If isOffline = False

                        End If 'If TemplateID <> 12 And TemplateID <> 13 

                        'start putting scrap factor on StandardCostPerUnit
                        'force 0.00005 to round up to 0.00001
                        dStandardCostPerUnitWOScrap += 0.000001

                        dStandardCostPerUnitWOScrap = Round(dStandardCostPerUnitWOScrap, 4)
                        dStandardCostPerUnit = Round(dStandardCostPerUnitWOScrap * dTempStandardCostFactor, 4)

                        'round
                        'dStandardCostPerUnitWOScrap = Format(dStandardCostPerUnitWOScrap, "####.0000")
                        'dStandardCostPerUnit = Format(dStandardCostPerUnit, "####.0000")

                        CostingModule.UpdateCostSheetLabor(iRowID, iLaborID, dRate, dCrewSize, dStandardCostFactor, iOrdinal, isOffline, dStandardCostPerUnitWOScrap, dStandardCostPerUnit)

                        dTotalStandardCostPerUnitWOScrap += dStandardCostPerUnitWOScrap
                        dTotalStandardCostPerUnit += dStandardCostPerUnit

                        ''round
                        'dTotalStandardCostPerUnitWOScrap = Format(dTotalStandardCostPerUnitWOScrap, "####.0000")
                        'dTotalStandardCostPerUnit = Format(dTotalStandardCostPerUnit, "####.0000")

                    End If
                Next

                ''updata totals table
                'CostingModule.UpdateCostSheetTotalLabor(CostSheetID, dTotalStandardCostPerUnitWOScrap, dTotalStandardCostPerUnit)

            End If

            'updata totals table
            CostingModule.UpdateCostSheetTotalLabor(CostSheetID, dTotalStandardCostPerUnitWOScrap, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", TemplateID: " & TemplateID _
            & ", QuotedMaxPieces: " & QuotedMaxPieces _
            & ", ProductionRate: " & ProductionRate _
            & ", LaborID: " & iLaborID _
            & ", dRate: " & dRate _
            & ", dCrewSize: " & dCrewSize _
            & ", isOffline: " & isOffline _
            & ", Ordinal: " & iOrdinal _
            & ", StandardCostPerUnit: " & dStandardCostPerUnit _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetLabor = dTotalStandardCostPerUnit

    End Function
    Public Shared Sub UpdateCostSheetLabor(ByVal RowID As Integer, ByVal LaborID As Integer, ByVal Rate As Double, _
    ByVal CrewSize As Double, ByVal StandardCostFactor As Double, ByVal Ordinal As Integer, ByVal isOffline As Boolean, _
    ByVal StandardCostPerUnitWOScrap As Double, ByVal StandardCostPerUnit As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            myCommand.Parameters.Add("@rate", SqlDbType.Decimal)
            myCommand.Parameters("@rate").Value = Rate

            myCommand.Parameters.Add("@crewSize", SqlDbType.Decimal)
            myCommand.Parameters("@crewSize").Value = CrewSize

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@isOffline ", SqlDbType.Bit)
            myCommand.Parameters("@isOffline ").Value = isOffline

            myCommand.Parameters.Add("@standardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@standardCostPerUnitWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitWOScrap").Value = StandardCostPerUnitWOScrap

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", LaborID: " & LaborID & ", Rate: " & Rate _
            & ", CrewSize: " & CrewSize _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", isOffline: " & isOffline _
            & ", Ordinal: " & Ordinal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetOverhead(ByVal CostSheetID As Integer, ByVal LaborID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCostSheetOverhead")
            GetCostSheetOverhead = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", LaborID: " & LaborID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetOverhead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetAdditionalOfflineRate(ByVal CostSheetID As Integer, ByVal LaborID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Additional_Offline_Rate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCostSheetAdditionalOfflineRate")
            GetCostSheetAdditionalOfflineRate = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", LaborID: " & LaborID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetAdditionalOfflineRate : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetAdditionalOfflineRate = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    '2/15/2012 - this was the old way of calculating overhead - Randy Khalaf and Dan Cade requested that crew size be ignored
    'Public Shared Function CalculateCostSheetOverhead(ByVal CostSheetID As Integer, ByVal TemplateID As Integer, _
    'ByVal QuotedMaxPieces As Integer, ByVal ProductionRate As Integer, ByVal NumberOfCarriers As Double, _
    'ByVal QuotedStandardCostFactor As Double, ByVal OfflineRate As Integer) As Double

    '    Dim iRowID As Integer = 0
    '    Dim iLaborID As Integer = 0

    '    Dim dFixedRate As Double = 0
    '    Dim dVariableRate As Double = 0

    '    Dim dCrewSize As Double = 0
    '    Dim dStandardCostFactor As Double = 0
    '    Dim iOrdinal As Integer = 0
    '    Dim isOffline As Boolean = False
    '    Dim isProportion As Boolean = False

    '    Dim dTempStandardCostFactor As Double = 0

    '    Dim dStandardCostPerUnitWOScrapFixedRate As Double = 0
    '    Dim dStandardCostPerUnitWOScrapVariableRate As Double = 0
    '    Dim dStandardCostPerUnitWOScrap As Double = 0

    '    Dim dStandardCostPerUnitFixedRate As Double = 0
    '    Dim dStandardCostPerUnitVariableRate As Double = 0
    '    Dim dStandardCostPerUnit As Double = 0

    '    Dim dTotalStandardCostPerUnitWOScrapFixedRate As Double = 0
    '    Dim dTotalStandardCostPerUnitWOScrapVariableRate As Double = 0
    '    Dim dTotalStandardCostPerUnitWOScrap As Double = 0

    '    Dim dTotalStandardCostPerUnitFixedRate As Double = 0
    '    Dim dTotalStandardCostPerUnitVariableRate As Double = 0
    '    Dim dTotalStandardCostPerUnit As Double = 0

    '    Dim iRowCounter As Integer = 0

    '    Dim ds As DataSet
    '    Dim dsAdditionalOfflineRate As DataSet

    '    Dim iRowCounterAdditionalOfflineRate As Integer = 0
    '    Dim dTempAdditionalOfflineRate As Double = 0

    '    Try
    '        ds = CostingModule.GetCostSheetOverhead(CostSheetID, 0)

    '        If commonFunctions.CheckDataset(ds) = True Then

    '            For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

    '                iRowID = 0
    '                iLaborID = 0

    '                dFixedRate = 0
    '                dVariableRate = 0

    '                dCrewSize = 0
    '                iOrdinal = 0
    '                isOffline = False
    '                isProportion = False
    '                dTempAdditionalOfflineRate = 0

    '                dStandardCostFactor = 0
    '                dTempStandardCostFactor = 0

    '                dStandardCostPerUnitWOScrapFixedRate = 0
    '                dStandardCostPerUnitWOScrapVariableRate = 0
    '                dStandardCostPerUnitWOScrap = 0

    '                dStandardCostPerUnitFixedRate = 0
    '                dStandardCostPerUnitVariableRate = 0
    '                dStandardCostPerUnit = 0

    '                If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
    '                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
    '                        iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
    '                    End If
    '                End If

    '                If ds.Tables(0).Rows(iRowCounter).Item("LaborID") IsNot System.DBNull.Value Then
    '                    If ds.Tables(0).Rows(iRowCounter).Item("LaborID") > 0 Then
    '                        iLaborID = ds.Tables(0).Rows(iRowCounter).Item("LaborID")
    '                    End If
    '                End If

    '                If iLaborID > 0 Then
    '                    If ds.Tables(0).Rows(iRowCounter).Item("Rate") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(iRowCounter).Item("Rate") > 0 Then
    '                            dFixedRate = ds.Tables(0).Rows(iRowCounter).Item("Rate")
    '                        End If
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("VariableRate") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(iRowCounter).Item("VariableRate") > 0 Then
    '                            dVariableRate = ds.Tables(0).Rows(iRowCounter).Item("VariableRate")
    '                        End If
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") > 0 Then
    '                            dCrewSize = ds.Tables(0).Rows(iRowCounter).Item("CrewSize")
    '                        End If
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
    '                            iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
    '                        End If
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("isOffline") IsNot System.DBNull.Value Then
    '                        isOffline = ds.Tables(0).Rows(iRowCounter).Item("isOffline")
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("isProportion") IsNot System.DBNull.Value Then
    '                        isProportion = ds.Tables(0).Rows(iRowCounter).Item("isProportion")
    '                    End If

    '                    If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") IsNot System.DBNull.Value Then
    '                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") > 0 Then
    '                            dStandardCostFactor = ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor")
    '                        End If
    '                    End If

    '                    If dStandardCostFactor = 0 Then
    '                        dTempStandardCostFactor = QuotedStandardCostFactor
    '                    Else
    '                        dTempStandardCostFactor = dStandardCostFactor
    '                    End If

    '                    'If iLaborID = 15 Then
    '                    '    dTempNumberOfCarriers = NumberOfCarriers
    '                    'End If

    '                    'need to check additional offline rates
    '                    dsAdditionalOfflineRate = CostingModule.GetCostSheetAdditionalOfflineRate(CostSheetID, 0)
    '                    If commonFunctions.CheckDataset(dsAdditionalOfflineRate) = True Then
    '                        For iRowCounterAdditionalOfflineRate = 0 To dsAdditionalOfflineRate.Tables(0).Rows.Count - 1
    '                            If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") IsNot System.DBNull.Value Then
    '                                If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") > 0 Then
    '                                    If iLaborID = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") Then
    '                                        If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour") IsNot System.DBNull.Value Then
    '                                            dTempAdditionalOfflineRate = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour")
    '                                        End If 'if pieces per hour is not null
    '                                    End If ' iLabor is same as Additional Offline Labor ID
    '                                End If 'if additional offline labor > 0
    '                            End If 'if additional offline labor is not null
    '                        Next 'end loop                        
    '                    End If 'if additional offline is not null result set


    '                    If TemplateID <> 12 And TemplateID <> 13 Then 'NOT Molding Chicago and NOT Molding Valpo
    '                        If isOffline = False Then
    '                            If QuotedMaxPieces > 0 Then
    '                                ''dStandardCostPerUnit = dRate / QuotedMaxPieces
    '                                ''dStandardCostPerUnitWOScrap = dRate / QuotedMaxPieces
    '                                dStandardCostPerUnitWOScrapFixedRate = dFixedRate / QuotedMaxPieces
    '                                dStandardCostPerUnitWOScrapVariableRate = dVariableRate / QuotedMaxPieces
    '                                ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / QuotedMaxPieces
    '                                'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                            End If 'If QuotedMaxPieces > 0
    '                        Else                                
    '                            If dTempAdditionalOfflineRate = 0 Then
    '                                If OfflineRate > 0 Then
    '                                    If dCrewSize = 0 Then
    '                                        ''dStandardCostPerUnit = dRate / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = dRate / OfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / OfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    Else
    '                                        ''dStandardCostPerUnit = (dRate * dCrewSize) / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / OfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / OfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / OfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If ' if dCrewSize = 0
    '                                End If 'If OfflineRate > 0 
    '                            Else
    '                                If dTempAdditionalOfflineRate > 0 Then
    '                                    If dCrewSize = 0 Then
    '                                        ''dStandardCostPerUnit = dRate / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = dRate / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / dTempAdditionalOfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    Else
    '                                        ''dStandardCostPerUnit = (dRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / dTempAdditionalOfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If ' if dCrewSize = 0
    '                                End If

    '                            End If ' if dTempAdditionalOfflineRate = 0

    '                        End If 'If isOffline = False
    '                    Else
    '                        If isOffline = False Then
    '                            If TemplateID = 12 Then 'Molding Chicago
    '                                If dCrewSize = 0 Then
    '                                    If ProductionRate > 0 Then
    '                                        ''dStandardCostPerUnit = dRate / ProductionRate
    '                                        ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
    '                                        ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / ProductionRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If 'ProductionRate > 0
    '                                Else
    '                                    If ProductionRate > 0 Then
    '                                        ''dStandardCostPerUnit = (dRate * dCrewSize) / ProductionRate
    '                                        ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / ProductionRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / ProductionRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / ProductionRate
    '                                        ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / ProductionRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If 'ProductionRate > 0
    '                                End If 'If dCrewSize = 0
    '                            End If 'If TemplateID = 12 

    '                            If TemplateID = 13 Then 'Molding Valpo
    '                                If ProductionRate > 0 Then
    '                                    ''dStandardCostPerUnit = dRate / ProductionRate
    '                                    ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
    '                                    dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
    '                                    dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
    '                                    ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / ProductionRate
    '                                    'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                End If 'ProductionRate > 0
    '                            End If 'If TemplateID = 13
    '                        Else
    '                            If dTempAdditionalOfflineRate = 0 Then
    '                                If OfflineRate > 0 Then
    '                                    If dCrewSize = 0 Then
    '                                        ''dStandardCostPerUnit = dRate / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = dRate / OfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / OfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    Else
    '                                        ''dStandardCostPerUnit = (dRate * dCrewSize) / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / OfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / OfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / OfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / OfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If ' if dCrewSize = 0
    '                                Else 'dcade 12/03/2009 if offline = 0 then use production rate
    '                                    If dCrewSize = 0 Then
    '                                        If ProductionRate > 0 Then
    '                                            ''dStandardCostPerUnit = dRate / ProductionRate
    '                                            ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
    '                                            dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
    '                                            dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
    '                                            ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / ProductionRate
    '                                            'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                        End If
    '                                    Else
    '                                        If ProductionRate > 0 Then
    '                                            ''dStandardCostPerUnit = (dRate * dCrewSize) / ProductionRate
    '                                            ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / ProductionRate
    '                                            dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / ProductionRate
    '                                            dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / ProductionRate
    '                                            ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / ProductionRate
    '                                            'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                        End If 'ProductionRate > 0
    '                                    End If
    '                                End If 'If OfflineRate > 0 
    '                            Else
    '                                If dTempAdditionalOfflineRate > 0 Then
    '                                    If dCrewSize = 0 Then
    '                                        ''dStandardCostPerUnit = dRate / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = dRate / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dFixedRate + dVariableRate) / dTempAdditionalOfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    Else
    '                                        ''dStandardCostPerUnit = (dRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / dTempAdditionalOfflineRate
    '                                        ''dStandardCostPerUnitWOScrap = ((dFixedRate + dVariableRate) * dCrewSize) / dTempAdditionalOfflineRate
    '                                        'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                                    End If ' if dCrewSize = 0
    '                                End If

    '                            End If ' if dTempAdditionalOfflineRate = 0
    '                        End If 'If isOffline = False

    '                    End If 'If TemplateID <> 12 And TemplateID <> 13

    '                    '2011-Jan-27 - DCADE - originally allowed for ID 15, also now allowed for ID=86
    '                    'if Valpo Injection Mold or VALPO Seatbelt
    '                    If iLaborID = 15 Or iLaborID = 86 Then
    '                        If ProductionRate > 0 Then
    '                            ''dStandardCostPerUnit = (NumberOfCarriers * dRate) / ProductionRate
    '                            ''dStandardCostPerUnitWOScrap = (NumberOfCarriers * dRate) / ProductionRate
    '                            dStandardCostPerUnitWOScrapFixedRate = (NumberOfCarriers * dFixedRate) / ProductionRate
    '                            dStandardCostPerUnitWOScrapVariableRate = (NumberOfCarriers * dVariableRate) / ProductionRate
    '                            ''dStandardCostPerUnitWOScrap = (NumberOfCarriers * (dFixedRate + dVariableRate)) / ProductionRate
    '                            'dStandardCostPerUnitWOScrap = dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate
    '                        End If 'ProductionRate > 0
    '                    End If 'If iLaborID = 15

    '                    'round
    '                    'force 0.00005 to round up to 0.00001
    '                    dStandardCostPerUnitWOScrapFixedRate += 0.000001
    '                    dStandardCostPerUnitWOScrapVariableRate += 0.000001

    '                    dStandardCostPerUnitWOScrapFixedRate = Round(dStandardCostPerUnitWOScrapFixedRate, 4)
    '                    dStandardCostPerUnitWOScrapVariableRate = Round(dStandardCostPerUnitWOScrapVariableRate, 4)
    '                    dStandardCostPerUnitWOScrap = Round(dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate, 4)

    '                    'start putting scrap factor on StandardCostPerUnit
    '                    dStandardCostPerUnitFixedRate = Round(dStandardCostPerUnitWOScrapFixedRate * dTempStandardCostFactor, 4)
    '                    dStandardCostPerUnitVariableRate = Round(dStandardCostPerUnitWOScrapVariableRate * dTempStandardCostFactor, 4)
    '                    'dStandardCostPerUnit = dStandardCostPerUnitWOScrap * dTempStandardCostFactor
    '                    dStandardCostPerUnit = dStandardCostPerUnitFixedRate + dStandardCostPerUnitVariableRate

    '                    ''round
    '                    'dStandardCostPerUnitWOScrapFixedRate = Format(dStandardCostPerUnitWOScrapFixedRate, "####.0000")
    '                    'dStandardCostPerUnitWOScrapVariableRate = Format(dStandardCostPerUnitWOScrapVariableRate, "####.0000")
    '                    'dStandardCostPerUnitWOScrap = Format(dStandardCostPerUnitWOScrap, "####.0000")

    '                    'dStandardCostPerUnitFixedRate = Format(dStandardCostPerUnitFixedRate, "####.0000")
    '                    'dStandardCostPerUnitVariableRate = Format(dStandardCostPerUnitVariableRate, "####.0000")
    '                    'dStandardCostPerUnit = Format(dStandardCostPerUnit, "####.0000")

    '                    CostingModule.UpdateCostSheetOverhead(iRowID, iLaborID, dFixedRate, dVariableRate, dCrewSize, _
    '                    NumberOfCarriers, iOrdinal, isOffline, isProportion, dStandardCostFactor, _
    '                    dStandardCostPerUnitWOScrapFixedRate, dStandardCostPerUnitWOScrapVariableRate, _
    '                    dStandardCostPerUnitWOScrap, dStandardCostPerUnit, dStandardCostPerUnitFixedRate, dStandardCostPerUnitVariableRate)

    '                    dTotalStandardCostPerUnitWOScrapFixedRate += dStandardCostPerUnitWOScrapFixedRate
    '                    dTotalStandardCostPerUnitWOScrapVariableRate += dStandardCostPerUnitWOScrapVariableRate
    '                    dTotalStandardCostPerUnitWOScrap += dStandardCostPerUnitWOScrap

    '                    dTotalStandardCostPerUnitFixedRate += dStandardCostPerUnitFixedRate
    '                    dTotalStandardCostPerUnitVariableRate += dStandardCostPerUnitVariableRate
    '                    dTotalStandardCostPerUnit += dStandardCostPerUnit

    '                    ''round
    '                    'dTotalStandardCostPerUnitWOScrapFixedRate = Format(dTotalStandardCostPerUnitWOScrapFixedRate, "####.0000")
    '                    'dTotalStandardCostPerUnitWOScrapVariableRate = Format(dTotalStandardCostPerUnitWOScrapVariableRate, "####.0000")
    '                    'dTotalStandardCostPerUnitWOScrap = Format(dTotalStandardCostPerUnitWOScrap, "####.0000")

    '                    'dTotalStandardCostPerUnitFixedRate = Format(dTotalStandardCostPerUnitFixedRate, "####.0000")
    '                    'dTotalStandardCostPerUnitVariableRate = Format(dTotalStandardCostPerUnitVariableRate, "####.0000")
    '                    'dTotalStandardCostPerUnit = Format(dTotalStandardCostPerUnit, "####.0000")
    '                End If 'If iLaborID > 0

    '            Next

    '            ''updata totals table
    '            'CostingModule.UpdateCostSheetTotalOverhead(CostSheetID, dTotalStandardCostPerUnitWOScrapFixedRate, _
    '            'dTotalStandardCostPerUnitWOScrapVariableRate, dTotalStandardCostPerUnitWOScrap, _
    '            'dTotalStandardCostPerUnitFixedRate, dTotalStandardCostPerUnitVariableRate, dTotalStandardCostPerUnit)

    '        End If 'ds has values

    '        'updata totals table
    '        CostingModule.UpdateCostSheetTotalOverhead(CostSheetID, dTotalStandardCostPerUnitWOScrapFixedRate, _
    '        dTotalStandardCostPerUnitWOScrapVariableRate, dTotalStandardCostPerUnitWOScrap, _
    '        dTotalStandardCostPerUnitFixedRate, dTotalStandardCostPerUnitVariableRate, dTotalStandardCostPerUnit)

    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
    '        & ", TemplateID: " & TemplateID _
    '        & ", NumberOfCarriers: " & NumberOfCarriers _
    '        & ", QuotedMaxPieces: " & QuotedMaxPieces _
    '        & ", OfflineRate: " & OfflineRate _
    '        & ", dTempAdditionalOfflineRate: " & dTempAdditionalOfflineRate _
    '        & ", iLaborID: " & iLaborID _
    '        & ", dFixedRate: " & dFixedRate _
    '        & ", dVariableRate: " & dVariableRate _
    '        & ", dCrewSize: " & dCrewSize _
    '        & ", isOffline: " & isOffline _
    '        & ", isProportion: " & isProportion _
    '        & ", Ordinal: " & iOrdinal _
    '        & ", dTempStandardCostFactor: " & dTempStandardCostFactor _
    '        & ", StandardCostPerUnitWOScrapFixedRate: " & dStandardCostPerUnitWOScrapFixedRate _
    '        & ", StandardCostPerUnitWOScrapVariableRate: " & dStandardCostPerUnitWOScrapVariableRate _
    '        & ", StandardCostPerUnitWOScrap: " & dStandardCostPerUnitWOScrap _
    '        & ", StandardCostPerUnitFixedRate: " & dStandardCostPerUnitFixedRate _
    '        & ", StandardCostPerUnitVariableRate: " & dStandardCostPerUnitVariableRate _
    '        & ", StandardCostPerUnit: " & dStandardCostPerUnit _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "CalculateCostSheetOverhead : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("CalculateCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '    End Try

    '    CalculateCostSheetOverhead = dTotalStandardCostPerUnit

    'End Function

    '2/15/2012 - the commented function above was the old way of calculating overhead - Randy Khalaf and Dan Cade requested that crew size be ignored
    Public Shared Function CalculateCostSheetOverhead(ByVal CostSheetID As Integer, ByVal TemplateID As Integer, _
    ByVal QuotedMaxPieces As Integer, ByVal ProductionRate As Integer, ByVal NumberOfCarriers As Double, _
    ByVal QuotedStandardCostFactor As Double, ByVal OfflineRate As Integer) As Double

        Dim iRowID As Integer = 0
        Dim iLaborID As Integer = 0

        Dim dFixedRate As Double = 0
        Dim dVariableRate As Double = 0

        Dim dCrewSize As Double = 0
        Dim dStandardCostFactor As Double = 0
        Dim iOrdinal As Integer = 0
        Dim isOffline As Boolean = False
        Dim isProportion As Boolean = False

        Dim dTempStandardCostFactor As Double = 0

        Dim dStandardCostPerUnitWOScrapFixedRate As Double = 0
        Dim dStandardCostPerUnitWOScrapVariableRate As Double = 0
        Dim dStandardCostPerUnitWOScrap As Double = 0

        Dim dStandardCostPerUnitFixedRate As Double = 0
        Dim dStandardCostPerUnitVariableRate As Double = 0
        Dim dStandardCostPerUnit As Double = 0

        Dim dTotalStandardCostPerUnitWOScrapFixedRate As Double = 0
        Dim dTotalStandardCostPerUnitWOScrapVariableRate As Double = 0
        Dim dTotalStandardCostPerUnitWOScrap As Double = 0

        Dim dTotalStandardCostPerUnitFixedRate As Double = 0
        Dim dTotalStandardCostPerUnitVariableRate As Double = 0
        Dim dTotalStandardCostPerUnit As Double = 0

        Dim iRowCounter As Integer = 0

        Dim ds As DataSet
        Dim dsAdditionalOfflineRate As DataSet

        Dim iRowCounterAdditionalOfflineRate As Integer = 0
        Dim dTempAdditionalOfflineRate As Double = 0

        Try
            ds = CostingModule.GetCostSheetOverhead(CostSheetID, 0)

            If commonFunctions.CheckDataset(ds) = True Then

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iLaborID = 0

                    dFixedRate = 0
                    dVariableRate = 0
                   
                    dCrewSize = 0
                    iOrdinal = 0
                    isOffline = False
                    isProportion = False
                    dTempAdditionalOfflineRate = 0

                    dStandardCostFactor = 0
                    dTempStandardCostFactor = 0

                    dStandardCostPerUnitWOScrapFixedRate = 0
                    dStandardCostPerUnitWOScrapVariableRate = 0
                    dStandardCostPerUnitWOScrap = 0

                    dStandardCostPerUnitFixedRate = 0
                    dStandardCostPerUnitVariableRate = 0
                    dStandardCostPerUnit = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("LaborID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("LaborID") > 0 Then
                            iLaborID = ds.Tables(0).Rows(iRowCounter).Item("LaborID")
                        End If
                    End If

                    If iLaborID > 0 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("Rate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Rate") > 0 Then
                                dFixedRate = ds.Tables(0).Rows(iRowCounter).Item("Rate")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("VariableRate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("VariableRate") > 0 Then
                                dVariableRate = ds.Tables(0).Rows(iRowCounter).Item("VariableRate")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CrewSize") > 0 Then
                                dCrewSize = ds.Tables(0).Rows(iRowCounter).Item("CrewSize")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isOffline") IsNot System.DBNull.Value Then
                            isOffline = ds.Tables(0).Rows(iRowCounter).Item("isOffline")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isProportion") IsNot System.DBNull.Value Then
                            isProportion = ds.Tables(0).Rows(iRowCounter).Item("isProportion")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor") > 0 Then
                                dStandardCostFactor = ds.Tables(0).Rows(iRowCounter).Item("StandardCostFactor")
                            End If
                        End If

                        If dStandardCostFactor = 0 Then
                            dTempStandardCostFactor = QuotedStandardCostFactor
                        Else
                            dTempStandardCostFactor = dStandardCostFactor
                        End If

                        'If iLaborID = 15 Then
                        '    dTempNumberOfCarriers = NumberOfCarriers
                        'End If

                        'need to check additional offline rates
                        dsAdditionalOfflineRate = CostingModule.GetCostSheetAdditionalOfflineRate(CostSheetID, 0)
                        If commonFunctions.CheckDataset(dsAdditionalOfflineRate) = True Then
                            For iRowCounterAdditionalOfflineRate = 0 To dsAdditionalOfflineRate.Tables(0).Rows.Count - 1
                                If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") IsNot System.DBNull.Value Then
                                    If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") > 0 Then
                                        If iLaborID = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("LaborID") Then
                                            If dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour") IsNot System.DBNull.Value Then
                                                dTempAdditionalOfflineRate = dsAdditionalOfflineRate.Tables(0).Rows(iRowCounterAdditionalOfflineRate).Item("PiecesPerHour")
                                            End If 'if pieces per hour is not null
                                        End If ' iLabor is same as Additional Offline Labor ID
                                    End If 'if additional offline labor > 0
                                End If 'if additional offline labor is not null
                            Next 'end loop                        
                        End If 'if additional offline is not null result set


                        If TemplateID <> 12 And TemplateID <> 13 Then 'NOT Molding Chicago and NOT Molding Valpo
                            If isOffline = False Then
                                If QuotedMaxPieces > 0 Then
                                    ''dStandardCostPerUnit = dRate / QuotedMaxPieces
                                    ''dStandardCostPerUnitWOScrap = dRate / QuotedMaxPieces
                                    dStandardCostPerUnitWOScrapFixedRate = dFixedRate / QuotedMaxPieces
                                    dStandardCostPerUnitWOScrapVariableRate = dVariableRate / QuotedMaxPieces                                   
                                End If 'If QuotedMaxPieces > 0
                            Else                                
                                If dTempAdditionalOfflineRate = 0 Then
                                    If OfflineRate > 0 Then
                                        '02/13/2012 stop using crewsize
                                        'If dCrewSize = 0 Then
                                        '    ''dStandardCostPerUnit = dRate / OfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = dRate / OfflineRate
                                        '    dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate                                            
                                        'Else
                                        '    ''dStandardCostPerUnit = (dRate * dCrewSize) / OfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / OfflineRate

                                        '    dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / OfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / OfflineRate
                                        'End If ' if dCrewSize = 0

                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate
                                    End If 'If OfflineRate > 0 
                                Else
                                    If dTempAdditionalOfflineRate > 0 Then
                                        '02/13/2012 stop using crewsize
                                        'If dCrewSize = 0 Then
                                        '    ''dStandardCostPerUnit = dRate / dTempAdditionalOfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = dRate / dTempAdditionalOfflineRate
                                        '    dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate
                                        'Else
                                        '    ''dStandardCostPerUnit = (dRate * dCrewSize) / dTempAdditionalOfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / dTempAdditionalOfflineRate
                                        '    dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / dTempAdditionalOfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / dTempAdditionalOfflineRate
                                        'End If ' if dCrewSize = 0

                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate
                                    End If

                                End If ' if dTempAdditionalOfflineRate = 0

                            End If 'If isOffline = False
                        Else
                            If isOffline = False Then
                                If TemplateID = 12 Then 'Molding Chicago
                                    If dCrewSize = 0 Then
                                        If ProductionRate > 0 Then
                                            ''dStandardCostPerUnit = dRate / ProductionRate
                                            ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
                                            dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
                                            dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate                                            
                                        End If 'ProductionRate > 0
                                    Else
                                        If ProductionRate > 0 Then
                                            ''dStandardCostPerUnit = (dRate * dCrewSize) / ProductionRate
                                            ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / ProductionRate

                                            '02/13/2012 stop using crewsize
                                            'dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / ProductionRate
                                            'dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / ProductionRate  
                                            dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
                                            dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
                                        End If 'ProductionRate > 0
                                    End If 'If dCrewSize = 0
                                End If 'If TemplateID = 12 

                                If TemplateID = 13 Then 'Molding Valpo
                                    If ProductionRate > 0 Then
                                        ''dStandardCostPerUnit = dRate / ProductionRate
                                        ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate                                      
                                    End If 'ProductionRate > 0
                                End If 'If TemplateID = 13
                            Else
                                If dTempAdditionalOfflineRate = 0 Then
                                    If OfflineRate > 0 Then
                                        '02/13/2012 stop using crewsize
                                        'If dCrewSize = 0 Then
                                        '    ''dStandardCostPerUnit = dRate / OfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = dRate / OfflineRate
                                        '    dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate
                                        'Else
                                        '    ''dStandardCostPerUnit = (dRate * dCrewSize) / OfflineRate
                                        '    ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / OfflineRate
                                        '    dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / OfflineRate
                                        '    dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / OfflineRate
                                        'End If ' if dCrewSize = 0
                                        dStandardCostPerUnitWOScrapFixedRate = dFixedRate / OfflineRate
                                        dStandardCostPerUnitWOScrapVariableRate = dVariableRate / OfflineRate
                                    Else 'dcade 12/03/2009 if offline = 0 then use production rate
                                        If dCrewSize = 0 Then
                                            If ProductionRate > 0 Then
                                                ''dStandardCostPerUnit = dRate / ProductionRate
                                                ''dStandardCostPerUnitWOScrap = dRate / ProductionRate
                                                dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
                                                dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
                                            End If
                                        Else
                                            If ProductionRate > 0 Then
                                                ''dStandardCostPerUnit = (dRate * dCrewSize) / ProductionRate
                                                ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / ProductionRate

                                                '02/13/2012 stop using crewsize
                                                'dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / ProductionRate
                                                'dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / ProductionRate
                                                dStandardCostPerUnitWOScrapFixedRate = dFixedRate / ProductionRate
                                                dStandardCostPerUnitWOScrapVariableRate = dVariableRate / ProductionRate
                                            End If 'ProductionRate > 0
                                        End If
                                    End If 'If OfflineRate > 0 
                                Else
                                    If dTempAdditionalOfflineRate > 0 Then
                                        If dCrewSize = 0 Then
                                            ''dStandardCostPerUnit = dRate / dTempAdditionalOfflineRate
                                            ''dStandardCostPerUnitWOScrap = dRate / dTempAdditionalOfflineRate
                                            dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
                                            dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate                                           
                                        Else
                                            ''dStandardCostPerUnit = (dRate * dCrewSize) / dTempAdditionalOfflineRate
                                            ''dStandardCostPerUnitWOScrap = (dRate * dCrewSize) / dTempAdditionalOfflineRate

                                            '02/13/2012 stop using crewsize
                                            'dStandardCostPerUnitWOScrapFixedRate = (dFixedRate * dCrewSize) / dTempAdditionalOfflineRate
                                            'dStandardCostPerUnitWOScrapVariableRate = (dVariableRate * dCrewSize) / dTempAdditionalOfflineRate
                                            dStandardCostPerUnitWOScrapFixedRate = dFixedRate / dTempAdditionalOfflineRate
                                            dStandardCostPerUnitWOScrapVariableRate = dVariableRate / dTempAdditionalOfflineRate
                                        End If ' if dCrewSize = 0
                                    End If

                                End If ' if dTempAdditionalOfflineRate = 0
                            End If 'If isOffline = False

                        End If 'If TemplateID <> 12 And TemplateID <> 13

                        '2011-Jan-27 - DCADE - originally allowed for ID 15, also now allowed for ID=86
                        'if Valpo Injection Mold or VALPO Seatbelt
                        If iLaborID = 15 Or iLaborID = 86 Then
                            If ProductionRate > 0 Then
                                ''dStandardCostPerUnit = (NumberOfCarriers * dRate) / ProductionRate
                                ''dStandardCostPerUnitWOScrap = (NumberOfCarriers * dRate) / ProductionRate
                                dStandardCostPerUnitWOScrapFixedRate = (NumberOfCarriers * dFixedRate) / ProductionRate
                                dStandardCostPerUnitWOScrapVariableRate = (NumberOfCarriers * dVariableRate) / ProductionRate                              
                            End If 'ProductionRate > 0
                        End If 'If iLaborID = 15

                        'round
                        'force 0.00005 to round up to 0.00001
                        dStandardCostPerUnitWOScrapFixedRate += 0.000001
                        dStandardCostPerUnitWOScrapVariableRate += 0.000001

                        dStandardCostPerUnitWOScrapFixedRate = Round(dStandardCostPerUnitWOScrapFixedRate, 4)
                        dStandardCostPerUnitWOScrapVariableRate = Round(dStandardCostPerUnitWOScrapVariableRate, 4)
                        dStandardCostPerUnitWOScrap = Round(dStandardCostPerUnitWOScrapFixedRate + dStandardCostPerUnitWOScrapVariableRate, 4)

                        'start putting scrap factor on StandardCostPerUnit
                        dStandardCostPerUnitFixedRate = Round(dStandardCostPerUnitWOScrapFixedRate * dTempStandardCostFactor, 4)
                        dStandardCostPerUnitVariableRate = Round(dStandardCostPerUnitWOScrapVariableRate * dTempStandardCostFactor, 4)                  
                        dStandardCostPerUnit = dStandardCostPerUnitFixedRate + dStandardCostPerUnitVariableRate

                        CostingModule.UpdateCostSheetOverhead(iRowID, iLaborID, dFixedRate, dVariableRate, dCrewSize, _
                        NumberOfCarriers, iOrdinal, isOffline, isProportion, dStandardCostFactor, _
                        dStandardCostPerUnitWOScrapFixedRate, dStandardCostPerUnitWOScrapVariableRate, _
                        dStandardCostPerUnitWOScrap, dStandardCostPerUnit, dStandardCostPerUnitFixedRate, dStandardCostPerUnitVariableRate)

                        dTotalStandardCostPerUnitWOScrapFixedRate += dStandardCostPerUnitWOScrapFixedRate
                        dTotalStandardCostPerUnitWOScrapVariableRate += dStandardCostPerUnitWOScrapVariableRate
                        dTotalStandardCostPerUnitWOScrap += dStandardCostPerUnitWOScrap

                        dTotalStandardCostPerUnitFixedRate += dStandardCostPerUnitFixedRate
                        dTotalStandardCostPerUnitVariableRate += dStandardCostPerUnitVariableRate
                        dTotalStandardCostPerUnit += dStandardCostPerUnit

                    End If 'If iLaborID > 0

                Next

            End If 'ds has values

            'updata totals table
            CostingModule.UpdateCostSheetTotalOverhead(CostSheetID, dTotalStandardCostPerUnitWOScrapFixedRate, _
            dTotalStandardCostPerUnitWOScrapVariableRate, dTotalStandardCostPerUnitWOScrap, _
            dTotalStandardCostPerUnitFixedRate, dTotalStandardCostPerUnitVariableRate, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", TemplateID: " & TemplateID _
            & ", NumberOfCarriers: " & NumberOfCarriers _
            & ", QuotedMaxPieces: " & QuotedMaxPieces _
            & ", OfflineRate: " & OfflineRate _
            & ", dTempAdditionalOfflineRate: " & dTempAdditionalOfflineRate _
            & ", iLaborID: " & iLaborID _
            & ", dFixedRate: " & dFixedRate _
            & ", dVariableRate: " & dVariableRate _
            & ", dCrewSize: " & dCrewSize _
            & ", isOffline: " & isOffline _
            & ", isProportion: " & isProportion _
            & ", Ordinal: " & iOrdinal _
            & ", dTempStandardCostFactor: " & dTempStandardCostFactor _
            & ", StandardCostPerUnitWOScrapFixedRate: " & dStandardCostPerUnitWOScrapFixedRate _
            & ", StandardCostPerUnitWOScrapVariableRate: " & dStandardCostPerUnitWOScrapVariableRate _
            & ", StandardCostPerUnitWOScrap: " & dStandardCostPerUnitWOScrap _
            & ", StandardCostPerUnitFixedRate: " & dStandardCostPerUnitFixedRate _
            & ", StandardCostPerUnitVariableRate: " & dStandardCostPerUnitVariableRate _
            & ", StandardCostPerUnit: " & dStandardCostPerUnit _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetOverhead = dTotalStandardCostPerUnit

    End Function
    Public Shared Sub UpdateCostSheetOverhead(ByVal RowID As Integer, ByVal LaborID As Integer, _
    ByVal Rate As Double, ByVal VariableRate As Double, _
    ByVal CrewSize As Double, ByVal NumberOfCarriers As Double, _
    ByVal ordinal As Integer, ByVal isOffline As Boolean, _
    ByVal isProportion As Boolean, ByVal StandardCostFactor As Double, _
    ByVal StandardCostPerUnitWOScrapFixedRate As Double, ByVal StandardCostPerUnitWOScrapVariableRate As Double, _
    ByVal StandardCostPerUnitWOScrap As Double, _
    ByVal StandardCostPerUnit As Double, ByVal StandardCostPerUnitFixedRate As Double, _
    ByVal StandardCostPerUnitVariableRate As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@laborID", SqlDbType.Int)
            myCommand.Parameters("@laborID").Value = LaborID

            myCommand.Parameters.Add("@rate", SqlDbType.Decimal)
            myCommand.Parameters("@rate").Value = Rate

            myCommand.Parameters.Add("@variableRate", SqlDbType.Decimal)
            myCommand.Parameters("@variableRate").Value = VariableRate

            myCommand.Parameters.Add("@crewSize", SqlDbType.Decimal)
            myCommand.Parameters("@crewSize").Value = CrewSize

            myCommand.Parameters.Add("@numberOfCarriers", SqlDbType.Decimal)
            myCommand.Parameters("@numberOfCarriers").Value = NumberOfCarriers

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = ordinal

            myCommand.Parameters.Add("@isOffline ", SqlDbType.Bit)
            myCommand.Parameters("@isOffline ").Value = isOffline

            myCommand.Parameters.Add("@isProportion ", SqlDbType.Bit)
            myCommand.Parameters("@isProportion ").Value = isProportion

            myCommand.Parameters.Add("@standardCostFactor", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostFactor").Value = StandardCostFactor

            myCommand.Parameters.Add("@standardCostPerUnitWOScrapFixedRate", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitWOScrapFixedRate").Value = StandardCostPerUnitWOScrapFixedRate

            myCommand.Parameters.Add("@standardCostPerUnitWOScrapVariableRate", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitWOScrapVariableRate").Value = StandardCostPerUnitWOScrapVariableRate

            myCommand.Parameters.Add("@standardCostPerUnitWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitWOScrap").Value = StandardCostPerUnitWOScrap

            myCommand.Parameters.Add("@standardCostPerUnitFixedRate", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitFixedRate").Value = StandardCostPerUnitFixedRate

            myCommand.Parameters.Add("@standardCostPerUnitVariableRate", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnitVariableRate").Value = StandardCostPerUnitVariableRate

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", LaborID: " & LaborID & ", Rate: " & Rate _
            & ", CrewSize: " & CrewSize _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", isOffline: " & isOffline _
            & ", isProportion: " & isProportion _
            & ", Ordinal: " & ordinal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetCapital(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@CapitalID", SqlDbType.Int)
            myCommand.Parameters("@CapitalID").Value = 0


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetCapital")
            GetCostSheetCapital = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetCapital = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function CalculateCostSheetCapital(ByVal CostSheetID As Integer, ByVal TemplateID As Integer, _
    ByVal QuotedMaxPieces As Integer, ByVal ProductionRate As Double, ByVal OfflineRate As Integer) As Double

        Dim iRowID As Integer = 0
        Dim iCapitalID As Integer = 0
        Dim dTotalDollarAmount As Double = 0
        Dim iYearsOfDepreciation As Integer = 0
        Dim iCapitalAnnualVolume As Integer = 0
        'Dim dPerPiece As Double = 0
        'Dim dHoldMoldAmount As Double = 0
        Dim dOverheadAmount As Double = 0
        'Dim dHourlyCapitalRate As Double = 0
        'Dim dOverheadRate As Double = 0
        Dim isOffline As Boolean = False
        Dim isInline As Boolean = False
        Dim iOrdinal As Integer = 0
        Dim dAmountPerPiece As Double = 0

        Dim dStandardCostPerUnit As Double = 0

        Dim dTotalStandardCostPerUnit As Double = 0

        Dim iRowCounter As Integer = 0

        Try
            Dim ds As DataSet = CostingModule.GetCostSheetCapital(CostSheetID)

            If commonFunctions.CheckDataset(ds) = True Then

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iCapitalID = 0
                    dTotalDollarAmount = 0
                    iYearsOfDepreciation = 0
                    iCapitalAnnualVolume = 0
                    'dPerPiece = 0
                    'dHoldMoldAmount = 0
                    dOverheadAmount = 0
                    'dHourlyCapitalRate = 0
                    'dOverheadRate = 0
                    isOffline = False
                    isInline = False
                    iOrdinal = 0
                    dStandardCostPerUnit = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("CapitalID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("CapitalID") > 0 Then
                            iCapitalID = ds.Tables(0).Rows(iRowCounter).Item("CapitalID")
                        End If
                    End If

                    If iCapitalID > 0 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TotalDollarAmount") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("TotalDollarAmount") > 0 Then
                                dTotalDollarAmount = ds.Tables(0).Rows(iRowCounter).Item("TotalDollarAmount")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("YearsOfDepreciation") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("YearsOfDepreciation") > 0 Then
                                iYearsOfDepreciation = ds.Tables(0).Rows(iRowCounter).Item("YearsOfDepreciation")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("CapitalAnnualVolume") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CapitalAnnualVolume") > 0 Then
                                iCapitalAnnualVolume = ds.Tables(0).Rows(iRowCounter).Item("CapitalAnnualVolume")
                            End If
                        End If

                        'If ds.Tables(0).Rows(iRowCounter).Item("PerPiece") IsNot System.DBNull.Value Then
                        '    dPerPiece = ds.Tables(0).Rows(iRowCounter).Item("PerPiece")
                        'End If

                        'If ds.Tables(0).Rows(iRowCounter).Item("HoldMoldAmount") IsNot System.DBNull.Value Then
                        '    dHoldMoldAmount = ds.Tables(0).Rows(iRowCounter).Item("HoldMoldAmount")
                        'End If

                        If ds.Tables(0).Rows(iRowCounter).Item("OverheadAmount") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("OverheadAmount") > 0 Then
                                dOverheadAmount = ds.Tables(0).Rows(iRowCounter).Item("OverheadAmount")
                            End If
                        End If

                        'If ds.Tables(0).Rows(iRowCounter).Item("HourlyCapitalRate") IsNot System.DBNull.Value Then
                        '    dHourlyCapitalRate = ds.Tables(0).Rows(iRowCounter).Item("HourlyCapitalRate")
                        'End If

                        'If ds.Tables(0).Rows(iRowCounter).Item("OverheadRate") IsNot System.DBNull.Value Then
                        '    dOverheadRate = ds.Tables(0).Rows(iRowCounter).Item("OverheadRate")
                        'End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isOffline") IsNot System.DBNull.Value Then
                            isOffline = ds.Tables(0).Rows(iRowCounter).Item("isOffline")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isInline") IsNot System.DBNull.Value Then
                            isInline = ds.Tables(0).Rows(iRowCounter).Item("isInline")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        If iCapitalAnnualVolume > 0 And iYearsOfDepreciation > 0 Then
                            dAmountPerPiece = (dTotalDollarAmount / iYearsOfDepreciation) / iCapitalAnnualVolume
                        End If

                        'If isInline = True And ProductionRate > 0 Then
                        If isInline = True Then
                            dOverheadAmount = dAmountPerPiece * QuotedMaxPieces
                            'ElseIf isOffline = True And ProductionRate > 0 Then
                        ElseIf isOffline = True Then
                            dOverheadAmount = dAmountPerPiece * OfflineRate
                        ElseIf isInline = False Or isOffline = False Then
                            dOverheadAmount = dAmountPerPiece * ProductionRate
                        End If

                        If TemplateID <> 12 And TemplateID <> 13 Then 'NOT Molding Chicago and NOT Molding Valpo
                            If OfflineRate > 0 And isInline = False Then
                                dStandardCostPerUnit = dOverheadAmount / OfflineRate
                            Else
                                If isInline = True Then
                                    If QuotedMaxPieces > 0 Then
                                        dStandardCostPerUnit = dOverheadAmount / QuotedMaxPieces
                                    End If
                                End If
                            End If
                        Else
                            If ProductionRate > 0 Then
                                dStandardCostPerUnit = dOverheadAmount / ProductionRate
                            End If
                        End If

                        'round
                        'dStandardCostPerUnit = Format(dStandardCostPerUnit, "####.0000")

                        'force 0.00005 to round up to 0.00001
                        dStandardCostPerUnit += 0.000001

                        dStandardCostPerUnit = Round(dStandardCostPerUnit, 4)

                        'CostingModule.UpdateCostSheetCapital(iRowID, iCapitalID, dTotalDollarAmount, iYearsOfDepreciation, iCapitalAnnualVolume, dPerPiece, dHoldMoldAmount, dOverheadAmount, dHourlyCapitalRate, dOverheadRate, dStandardCostPerUnit, isOffline, isInline, iOrdinal)
                        CostingModule.UpdateCostSheetCapital(iRowID, iCapitalID, dTotalDollarAmount, iYearsOfDepreciation, iCapitalAnnualVolume, dOverheadAmount, dStandardCostPerUnit, isOffline, isInline, iOrdinal)

                        dTotalStandardCostPerUnit += dStandardCostPerUnit

                        'round
                        'dTotalStandardCostPerUnit = Format(dTotalStandardCostPerUnit, "####.0000")

                    End If
                Next

                ''updata totals table
                'CostingModule.UpdateCostSheetTotalCapital(CostSheetID, dTotalStandardCostPerUnit)

            End If

            'updata totals table
            CostingModule.UpdateCostSheetTotalCapital(CostSheetID, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", ProductionRate: " & ProductionRate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetCapital = dTotalStandardCostPerUnit

    End Function
    Public Shared Sub UpdateCostSheetCapital(ByVal RowID As Integer, ByVal CapitalID As Integer, _
    ByVal TotalDollarAmount As Double, ByVal YearsOfDepreciation As Integer, ByVal CapitalAnnualVolume As Integer, _
    ByVal OverheadAmount As Double, ByVal StandardCostPerUnit As Double, ByVal isOffline As Boolean, ByVal isInline As Boolean, ByVal Ordinal As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@CapitalID", SqlDbType.Int)
            myCommand.Parameters("@CapitalID").Value = CapitalID

            myCommand.Parameters.Add("@totalDollarAmount", SqlDbType.Decimal)
            myCommand.Parameters("@totalDollarAmount").Value = TotalDollarAmount

            myCommand.Parameters.Add("@yearsOfDepreciation", SqlDbType.Int)
            myCommand.Parameters("@yearsOfDepreciation").Value = YearsOfDepreciation

            myCommand.Parameters.Add("@capitalAnnualVolume", SqlDbType.Int)
            myCommand.Parameters("@capitalAnnualVolume").Value = CapitalAnnualVolume

            'myCommand.Parameters.Add("@perPiece", SqlDbType.Decimal)
            'myCommand.Parameters("@perPiece").Value = PerPiece

            'myCommand.Parameters.Add("@holdMoldAmount", SqlDbType.Decimal)
            'myCommand.Parameters("@holdMoldAmount").Value = HoldMoldAmount

            myCommand.Parameters.Add("@overheadAmount", SqlDbType.Decimal)
            myCommand.Parameters("@overheadAmount").Value = OverheadAmount

            'myCommand.Parameters.Add("@hourlyCapitalRate", SqlDbType.Decimal)
            'myCommand.Parameters("@hourlyCapitalRate").Value = HourlyCapitalRate

            'myCommand.Parameters.Add("@overheadRate", SqlDbType.Decimal)
            'myCommand.Parameters("@overheadRate").Value = OverheadRate

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@isOffline", SqlDbType.Bit)
            myCommand.Parameters("@isOffline").Value = isOffline

            myCommand.Parameters.Add("@isInline", SqlDbType.Bit)
            myCommand.Parameters("@isInline").Value = isInline

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            '& ", PerPiece: " & PerPiece 
            '& ", HoldMoldAmount: " & HoldMoldAmount _
            '& ", OverheadRate: " & OverheadRate
            '& ", HourlyCapitalRate: " & HourlyCapitalRate

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", CapitalID: " & CapitalID & ", TotalDollarAmount: " & TotalDollarAmount _
            & ", yearsOfDepreciation: " & YearsOfDepreciation & ", CapitalAnnualVolume: " & CapitalAnnualVolume _
            & ", OverheadAmount: " & OverheadAmount _
             & ", isOffline: " & isOffline & ", isInline: " & isInline _
            & ", Ordinal: " & Ordinal & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetMiscCost(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@miscCostID", SqlDbType.Int)
            myCommand.Parameters("@miscCostID").Value = 0

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetMiscCost")
            GetCostSheetMiscCost = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheet : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetMiscCost = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function CalculateCostSheetMiscCost(ByVal CostSheetID As Integer, ByVal TemplateID As Integer, ByVal QuoteDate As String, ByVal CostSheetSubTotal As Double, ByVal PiecesPerYear As Integer) As Double

        Dim dStandardCostPerUnit As Double = 0
        Dim dTotalStandardCostPerUnit As Double = 0

        Dim ds As DataSet
        Dim iRowCounter As Integer = 0
        Dim iRowID As Integer = 0
        Dim iMiscCostID As Integer = 0
        Dim dRate As Double = 0
        Dim dQuoteRate As Double = 0
        Dim dCost As Double = 0
        Dim iAmortVolume As Integer = 0
        'Dim iPieces As Integer = 0
        Dim isPiecesPerHour As Boolean = 0
        Dim isPiecesPerYear As Boolean = 0
        Dim isPiecesPerContainer As Boolean = 0
        Dim iOrdinal As Integer = 0

        Dim dsMiscCost As DataSet
        Dim isRatePercentage As Boolean = False

        Dim dsCostSheetArchive As DataSet
        Dim iYrsOfToolingAmmort As Integer = 0
        Dim iToolingYrlyVolume As Integer = 0
        Dim dDesignCostAmmort As Double = 0

        Try
            ds = CostingModule.GetCostSheetMiscCost(CostSheetID)
            If commonFunctions.CheckDataset(ds) = True Then

                dsCostSheetArchive = CostingModule.GetCostSheetArchive(CostSheetID)

                If commonFunctions.CheckDataset(dsCostSheetArchive) = True Then

                    If dsCostSheetArchive.Tables(0).Rows(0).Item("YrsOfToolingAmmort") IsNot System.DBNull.Value Then
                        If dsCostSheetArchive.Tables(0).Rows(0).Item("YrsOfToolingAmmort") > 0 Then
                            iYrsOfToolingAmmort = dsCostSheetArchive.Tables(0).Rows(0).Item("YrsOfToolingAmmort")
                        End If
                    End If

                    If dsCostSheetArchive.Tables(0).Rows(0).Item("ToolingYrlyVolume") IsNot System.DBNull.Value Then
                        If dsCostSheetArchive.Tables(0).Rows(0).Item("ToolingYrlyVolume") > 0 Then
                            iToolingYrlyVolume = dsCostSheetArchive.Tables(0).Rows(0).Item("ToolingYrlyVolume")
                        End If
                    End If

                    If dsCostSheetArchive.Tables(0).Rows(0).Item("DesignCostAmmort") IsNot System.DBNull.Value Then
                        If dsCostSheetArchive.Tables(0).Rows(0).Item("DesignCostAmmort") > 0 Then
                            dDesignCostAmmort = dsCostSheetArchive.Tables(0).Rows(0).Item("DesignCostAmmort")
                        End If
                    End If

                End If

                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iRowID = 0
                    iMiscCostID = 0
                    dRate = 0
                    dQuoteRate = 0
                    dCost = 0
                    iAmortVolume = 0
                    'iPieces = 0
                    isPiecesPerHour = False
                    isPiecesPerYear = False
                    isPiecesPerContainer = False
                    iOrdinal = 0
                    isRatePercentage = False

                    dStandardCostPerUnit = 0

                    If ds.Tables(0).Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = ds.Tables(0).Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If ds.Tables(0).Rows(iRowCounter).Item("MiscCostID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("MiscCostID") Then
                            iMiscCostID = ds.Tables(0).Rows(iRowCounter).Item("MiscCostID")
                        End If
                    End If

                    If iMiscCostID > 0 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("Rate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Rate") > 0 Then
                                dRate = ds.Tables(0).Rows(iRowCounter).Item("Rate")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("QuoteRate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("QuoteRate") > 0 Then
                                dQuoteRate = ds.Tables(0).Rows(iRowCounter).Item("QuoteRate")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Cost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Cost") > 0 Then
                                dCost = ds.Tables(0).Rows(iRowCounter).Item("Cost")
                            End If
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("AmortVolume") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("AmortVolume") > 0 Then
                                iAmortVolume = ds.Tables(0).Rows(iRowCounter).Item("AmortVolume")
                            End If
                        End If

                        'If ds.Tables(0).Rows(iRowCounter).Item("Pieces") IsNot System.DBNull.Value Then
                        '    iPieces = ds.Tables(0).Rows(iRowCounter).Item("Pieces")
                        'End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerHour") IsNot System.DBNull.Value Then
                            isPiecesPerHour = ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerHour")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerYear") IsNot System.DBNull.Value Then
                            isPiecesPerYear = ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerYear")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerContainer") IsNot System.DBNull.Value Then
                            isPiecesPerContainer = ds.Tables(0).Rows(iRowCounter).Item("isPiecesPerContainer")
                        End If

                        If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("Ordinal") > 0 Then
                                iOrdinal = ds.Tables(0).Rows(iRowCounter).Item("Ordinal")
                            End If
                        End If

                        'see if Rate or Amort Value should be shown on Cost Form Preview
                        dsMiscCost = CostingModule.GetMiscCost(iMiscCostID, "")
                        If commonFunctions.CheckDataset(dsMiscCost) = True Then                           
                            If dsMiscCost.Tables(0).Rows(0).Item("isRatePercentage") IsNot System.DBNull.Value Then
                                isRatePercentage = dsMiscCost.Tables(0).Rows(0).Item("isRatePercentage")
                            End If                       
                        End If

                        If QuoteDate = "" Then
                            QuoteDate = "01/01/1900"
                        End If

                        If QuoteDate <> "" And (CType(QuoteDate, Date) > CType("03/28/2005", Date)) Then
                            If isRatePercentage = True Then
                                dStandardCostPerUnit = dRate * CostSheetSubTotal
                            Else
                                If iAmortVolume > 0 Then
                                    dStandardCostPerUnit = dCost / iAmortVolume
                                End If 'If iAmortVolume > 0
                            End If 'If isRatePercentage = True 
                        Else
                            If TemplateID <> 12 And TemplateID <> 13 Then 'NOT Molding Chicago and NOT Molding Valpo
                                If isRatePercentage = True Then
                                    dStandardCostPerUnit = dRate * CostSheetSubTotal
                                Else
                                    If iMiscCostID = 11 Then 'Tooling
                                        If iYrsOfToolingAmmort > 0 Then
                                            'update iToolingYrlyVolume
                                            If iToolingYrlyVolume > 0 Then
                                                iToolingYrlyVolume = iYrsOfToolingAmmort * iToolingYrlyVolume
                                            Else
                                                iToolingYrlyVolume = iYrsOfToolingAmmort * PiecesPerYear
                                            End If 'If iToolingYrlyVolume > 0

                                            If iToolingYrlyVolume > 0 Then
                                                dStandardCostPerUnit = dCost / iToolingYrlyVolume
                                            End If
                                        End If 'If iYrsOfToolingAmmort > 0
                                    Else 'not tooling
                                        If dDesignCostAmmort > 0 And iAmortVolume > 0 Then
                                            dStandardCostPerUnit = dCost / iAmortVolume
                                        End If 'If dDesignCostAmmort > 0
                                    End If 'If iMiscCostID = 11 
                                End If 'If isRatePercentage = True
                            Else 'other templates
                                If dCost > 0 Then
                                    If iYrsOfToolingAmmort > 0 Then
                                        'update iToolingYrlyVolume
                                        If iToolingYrlyVolume > 0 Then
                                            iToolingYrlyVolume = iYrsOfToolingAmmort * iToolingYrlyVolume
                                        Else
                                            iToolingYrlyVolume = iYrsOfToolingAmmort * PiecesPerYear
                                        End If 'If iToolingYrlyVolume > 0

                                        If iToolingYrlyVolume > 0 Then
                                            dStandardCostPerUnit = dCost / iToolingYrlyVolume
                                        End If
                                    End If 'If iYrsOfToolingAmmort > 0
                                End If 'If dCost > 0
                            End If 'If TemplateID <> 12 And TemplateID <> 13
                        End If 'if earlier than 3/28/2005

                        'round
                        'dStandardCostPerUnit = Format(dStandardCostPerUnit, "####.0000")

                        'force 0.00005 to round up to 0.00001                       
                        dStandardCostPerUnit += 0.000001
                        dStandardCostPerUnit = Round(dStandardCostPerUnit, 4)

                        CostingModule.UpdateCostSheetMiscCost(iRowID, iMiscCostID, dRate, dQuoteRate, dCost, iAmortVolume, dStandardCostPerUnit, isPiecesPerHour, isPiecesPerYear, isPiecesPerContainer, iOrdinal)

                        'split SGA out
                        If iMiscCostID = 1 Then
                            CostingModule.UpdateCostSheetTotalSGA(CostSheetID, dStandardCostPerUnit)
                        Else
                            dTotalStandardCostPerUnit += dStandardCostPerUnit

                            'round
                            'dTotalStandardCostPerUnit = Format(dTotalStandardCostPerUnit, "####.0000")
                        End If

                    End If
                Next

                ''updata totals table
                'CostingModule.UpdateCostSheetTotalMiscCost(CostSheetID, dTotalStandardCostPerUnit)

            End If

            'updata totals table
            CostingModule.UpdateCostSheetTotalMiscCost(CostSheetID, dTotalStandardCostPerUnit)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", dRate : " & dRate _
            & ", dQuoteRate  : " & dQuoteRate _
            & ", dCost  : " & dCost _
            & ", iAmortVolume  : " & iAmortVolume _
            & ", isPiecesPerHour  : " & isPiecesPerHour _
            & ", isPiecesPerYear  : " & isPiecesPerYear _
            & ", isPiecesPerContainer  : " & isPiecesPerContainer _
            & ", iOrdinal  : " & iOrdinal _
            & ", isRatePercentage  : " & isRatePercentage _
            & ", QuoteDate: " & QuoteDate _
            & ", CostSheetSubTotal: " & CostSheetSubTotal _
            & ", PiecesPerYear: " & PiecesPerYear _
            & ", QuoteDate: " & QuoteDate _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CalculateCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CalculateCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        CalculateCostSheetMiscCost = dTotalStandardCostPerUnit
    End Function
    Public Shared Sub UpdateCostSheetMiscCost(ByVal RowID As Integer, ByVal MiscCostID As Integer, ByVal Rate As Double, _
    ByVal QuoteRate As Double, ByVal Cost As Double, ByVal AmortVolume As Integer, _
    ByVal StandardCostPerUnit As Double, ByVal isPiecesPerHour As Boolean, ByVal isPiecesPerYear As Boolean, _
    ByVal isPiecesPerContainer As Boolean, ByVal Ordinal As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@rowID", SqlDbType.Int)
            myCommand.Parameters("@rowID").Value = RowID

            myCommand.Parameters.Add("@miscCostID", SqlDbType.Int)
            myCommand.Parameters("@miscCostID").Value = MiscCostID

            myCommand.Parameters.Add("@rate", SqlDbType.Decimal)
            myCommand.Parameters("@rate").Value = Rate

            myCommand.Parameters.Add("@quoteRate", SqlDbType.Decimal)
            myCommand.Parameters("@quoteRate").Value = QuoteRate

            myCommand.Parameters.Add("@cost", SqlDbType.Decimal)
            myCommand.Parameters("@cost").Value = Cost

            myCommand.Parameters.Add("@amortVolume", SqlDbType.Int)
            myCommand.Parameters("@amortVolume").Value = AmortVolume

            'myCommand.Parameters.Add("@pieces", SqlDbType.Int)
            'myCommand.Parameters("@pieces").Value = Pieces

            myCommand.Parameters.Add("@standardCostPerUnit", SqlDbType.Decimal)
            myCommand.Parameters("@standardCostPerUnit").Value = StandardCostPerUnit

            myCommand.Parameters.Add("@isPiecesPerHour", SqlDbType.Bit)
            myCommand.Parameters("@isPiecesPerHour").Value = isPiecesPerHour

            myCommand.Parameters.Add("@isPiecesPerYear", SqlDbType.Bit)
            myCommand.Parameters("@isPiecesPerYear").Value = isPiecesPerYear

            myCommand.Parameters.Add("@isPiecesPerContainer", SqlDbType.Bit)
            myCommand.Parameters("@isPiecesPerContainer").Value = isPiecesPerContainer

            myCommand.Parameters.Add("@ordinal", SqlDbType.Int)
            myCommand.Parameters("@ordinal").Value = Ordinal

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", MiscCostID: " & MiscCostID & ", Rate: " & Rate _
            & ", QuoteRate: " & QuoteRate & ", Cost: " & Cost _
            & ", AmortVolume: " & AmortVolume _
            & ", StandardCostPerUnit: " & StandardCostPerUnit & ", isPiecesPerHour: " & isPiecesPerHour _
            & ", isPiecesPerContainer: " & isPiecesPerContainer _
            & ", Ordinal: " & Ordinal & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetTotal(ByVal CostSheetID As Integer, ByVal MaterialCostTotalWOScrap As Double, _
    ByVal MaterialCostTotal As Double, ByVal PackagingCostTotal As Double, _
    ByVal LaborCostTotalWOScrap As Double, ByVal LaborCostTotal As Double, _
    ByVal OverheadCostTotalWOScrap As Double, ByVal OverheadCostTotal As Double, ByVal ScrapCostTotal As Double, _
    ByVal CapitalCostTotal As Double, ByVal MiscCostTotal As Double, _
    ByVal SGACostTotal As Double, ByVal OverallCostTotal As Double, ByVal FixedCostTotal As Double, _
    ByVal VariableCostTotal As Double, ByVal MinPriceMargin As Double, ByVal MinSellingPrice As Double, _
    ByVal PriceVariableMarginPercent As Double, ByVal PriceVariableMarginDollar As Double, _
    ByVal PriceVariableMarginInclDeprPercent As Double, ByVal PriceVariableMarginInclDeprDollar As Double, _
    ByVal PriceGrossMarginPercent As Double, ByVal PriceGrossMarginDollar As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@MaterialCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotalWOScrap").Value = MaterialCostTotalWOScrap

            myCommand.Parameters.Add("@MaterialCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotal").Value = MaterialCostTotal

            myCommand.Parameters.Add("@PackagingCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@PackagingCostTotal").Value = PackagingCostTotal

            myCommand.Parameters.Add("@LaborCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotalWOScrap").Value = LaborCostTotalWOScrap

            myCommand.Parameters.Add("@LaborCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotal").Value = LaborCostTotal

            myCommand.Parameters.Add("@OverheadCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalWOScrap").Value = OverheadCostTotalWOScrap

            myCommand.Parameters.Add("@OverheadCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotal").Value = OverheadCostTotal

            myCommand.Parameters.Add("@ScrapCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapCostTotal").Value = ScrapCostTotal

            myCommand.Parameters.Add("@CapitalCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@CapitalCostTotal").Value = CapitalCostTotal

            myCommand.Parameters.Add("@MiscCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MiscCostTotal").Value = MiscCostTotal

            myCommand.Parameters.Add("@SGACostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@SGACostTotal").Value = SGACostTotal

            myCommand.Parameters.Add("@OverallCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverallCostTotal").Value = OverallCostTotal

            myCommand.Parameters.Add("@FixedCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@FixedCostTotal").Value = FixedCostTotal

            myCommand.Parameters.Add("@VariableCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@VariableCostTotal").Value = VariableCostTotal

            myCommand.Parameters.Add("@MinPriceMargin", SqlDbType.Decimal)
            myCommand.Parameters("@MinPriceMargin").Value = MinPriceMargin

            myCommand.Parameters.Add("@MinSellingPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MinSellingPrice").Value = MinSellingPrice

            myCommand.Parameters.Add("@PriceVariableMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginPercent").Value = PriceVariableMarginPercent

            myCommand.Parameters.Add("@PriceVariableMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginDollar").Value = PriceVariableMarginDollar

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprPercent").Value = PriceVariableMarginInclDeprPercent

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprDollar").Value = PriceVariableMarginInclDeprDollar

            myCommand.Parameters.Add("@PriceGrossMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginPercent").Value = PriceGrossMarginPercent

            myCommand.Parameters.Add("@PriceGrossMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginDollar").Value = PriceGrossMarginDollar

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", MaterialCostTotalWOScrap: " & MaterialCostTotalWOScrap _
            & ", MaterialCostTotal: " & MaterialCostTotal _
            & ", PackagingCostTotal: " & PackagingCostTotal _
            & ", LaborCostTotalWOScrap: " & LaborCostTotalWOScrap _
            & ", LaborCostTotal: " & LaborCostTotal _
            & ", OverheadCostTotalWOScrap : " & OverheadCostTotalWOScrap _
            & ", OverheadCostTotal : " & OverheadCostTotal _
            & ", ScrapCostTotal : " & ScrapCostTotal _
            & ", CapitalCostTotal: " & CapitalCostTotal _
            & ", MiscCostTotal: " & MiscCostTotal _
            & ", SGACostTotal : " & SGACostTotal _
            & ", OverallCostTotal: " & OverallCostTotal _
            & ", FixedCostTotal: " & FixedCostTotal _
            & ", VariableCostTotal: " & VariableCostTotal _
            & ", MinPriceMargin: " & MinPriceMargin _
            & ", MinSellingPrice: " & MinSellingPrice _
            & ", PriceVariableMarginPercent: " & PriceVariableMarginPercent _
            & ", PriceVariableMarginDollar: " & PriceVariableMarginDollar _
            & ", PriceVariableMarginInclDeprPercent: " & PriceVariableMarginInclDeprPercent _
            & ", PriceVariableMarginInclDeprDollar: " & PriceVariableMarginInclDeprDollar _
            & ", PriceGrossMarginPercent: " & PriceGrossMarginPercent _
            & ", PriceGrossMarginDollar: " & PriceGrossMarginDollar _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheeTotalt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub InsertCostSheetCustomerProgram(ByVal CostSheetID As Integer, ByVal CABBV As String, _
    ByVal SoldTo As Integer, ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", CABBV: " & CABBV _
            & ", SoldTo: " & SoldTo & ", ProgramID: " & ProgramID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheeTotalt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotal(ByVal CostSheetID As Integer, ByVal MaterialCostTotalWOScrap As Double, _
    ByVal MaterialCostTotal As Double, ByVal PackagingCostTotal As Double, _
    ByVal LaborCostTotalWOScrap As Double, ByVal LaborCostTotal As Double, _
    ByVal OverheadCostTotalWOScrap As Double, ByVal OverheadCostTotal As Double, _
    ByVal ScrapCostTotal As Double, ByVal CapitalCostTotal As Double, _
    ByVal MiscCostTotal As Double, ByVal SGACostTotal As Double, _
    ByVal OverallCostTotal As Double, ByVal FixedCostTotal As Double, _
    ByVal VariableCostTotal As Double, ByVal MinPriceMargin As Double, ByVal MinSellingPrice As Double, _
    ByVal PriceVariableMarginPercent As Double, ByVal PriceVariableMarginDollar As Double, _
    ByVal PriceVariableMarginInclDeprPercent As Double, ByVal PriceVariableMarginInclDeprDollar As Double, _
    ByVal PriceGrossMarginPercent As Double, ByVal PriceGrossMarginDollar As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@MaterialCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotalWOScrap").Value = MaterialCostTotalWOScrap

            myCommand.Parameters.Add("@MaterialCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotal").Value = MaterialCostTotal

            myCommand.Parameters.Add("@PackagingCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@PackagingCostTotal").Value = PackagingCostTotal

            myCommand.Parameters.Add("@LaborCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotalWOScrap").Value = LaborCostTotalWOScrap

            myCommand.Parameters.Add("@LaborCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotal").Value = LaborCostTotal

            myCommand.Parameters.Add("@OverheadCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalWOScrap").Value = OverheadCostTotalWOScrap

            myCommand.Parameters.Add("@OverheadCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotal").Value = OverheadCostTotal

            myCommand.Parameters.Add("@ScrapCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapCostTotal").Value = ScrapCostTotal

            myCommand.Parameters.Add("@CapitalCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@CapitalCostTotal").Value = CapitalCostTotal

            myCommand.Parameters.Add("@MiscCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MiscCostTotal").Value = MiscCostTotal

            myCommand.Parameters.Add("@SGACostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@SGACostTotal").Value = SGACostTotal

            myCommand.Parameters.Add("@OverallCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverallCostTotal").Value = OverallCostTotal

            myCommand.Parameters.Add("@FixedCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@FixedCostTotal").Value = FixedCostTotal

            myCommand.Parameters.Add("@VariableCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@VariableCostTotal").Value = VariableCostTotal

            myCommand.Parameters.Add("@MinPriceMargin", SqlDbType.Decimal)
            myCommand.Parameters("@MinPriceMargin").Value = MinPriceMargin

            myCommand.Parameters.Add("@MinSellingPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MinSellingPrice").Value = MinSellingPrice

            myCommand.Parameters.Add("@PriceVariableMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginPercent").Value = PriceVariableMarginPercent

            myCommand.Parameters.Add("@PriceVariableMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginDollar").Value = PriceVariableMarginDollar

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprPercent").Value = PriceVariableMarginInclDeprPercent

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprDollar").Value = PriceVariableMarginInclDeprDollar

            myCommand.Parameters.Add("@PriceGrossMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginPercent").Value = PriceGrossMarginPercent

            myCommand.Parameters.Add("@PriceGrossMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginDollar").Value = PriceGrossMarginDollar

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", MaterialCostTotalWOScrap: " & MaterialCostTotalWOScrap _
            & ", MaterialCostTotal: " & MaterialCostTotal _
            & ", PackagingCostTotal: " & PackagingCostTotal _
            & ", LaborCostTotalWOScrap: " & LaborCostTotalWOScrap _
            & ", LaborCostTotal: " & LaborCostTotal _
            & ", OverheadCostTotalWOScrap : " & OverheadCostTotalWOScrap _
            & ", OverheadCostTotal : " & OverheadCostTotal _
            & ", ScrapCostTotal  : " & ScrapCostTotal _
            & ", CapitalCostTotal: " & CapitalCostTotal _
            & ", MiscCostTotal: " & MiscCostTotal _
            & ", SGACostTotal: " & SGACostTotal _
            & ", OverallCostTotal: " & OverallCostTotal _
            & ", FixedCostTotal: " & FixedCostTotal _
            & ", VariableCostTotal: " & VariableCostTotal _
            & ", MinPriceMargin: " & MinPriceMargin _
            & ", MinSellingPrice: " & MinSellingPrice _
            & ", PriceVariableMarginPercent: " & PriceVariableMarginPercent _
            & ", PriceVariableMarginDollar: " & PriceVariableMarginDollar _
            & ", PriceVariableMarginInclDeprPercent: " & PriceVariableMarginInclDeprPercent _
            & ", PriceVariableMarginInclDeprDollar: " & PriceVariableMarginInclDeprDollar _
            & ", PriceGrossMarginPercent: " & PriceGrossMarginPercent _
            & ", PriceGrossMarginDollar: " & PriceGrossMarginDollar _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalCapital(ByVal CostSheetID As Integer, ByVal CapitalCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Capital"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@CapitalCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@CapitalCostTotal").Value = CapitalCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", CapitalCostTotal: " & CapitalCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalLabor(ByVal CostSheetID As Integer, _
    ByVal LaborCostTotalWOScrap As Double, ByVal LaborCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@LaborCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotalWOScrap").Value = LaborCostTotalWOScrap

            myCommand.Parameters.Add("@LaborCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@LaborCostTotal").Value = LaborCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", LaborCostTotalWOScrap: " & LaborCostTotalWOScrap _
            & ", LaborCostTotal: " & LaborCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalMaterial(ByVal CostSheetID As Integer, ByVal MaterialCostTotalWOScrap As Double, _
    ByVal MaterialCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@MaterialCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotalWOScrap").Value = MaterialCostTotalWOScrap

            myCommand.Parameters.Add("@MaterialCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MaterialCostTotal").Value = MaterialCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", MaterialCostTotalWOScrap: " & MaterialCostTotalWOScrap _
            & ", MaterialCostTotal: " & MaterialCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalMiscCost(ByVal CostSheetID As Integer, ByVal MiscCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_MiscCost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@MiscCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@MiscCostTotal").Value = MiscCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", MiscCostTotal: " & MiscCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalOverall(ByVal CostSheetID As Integer, ByVal OverallCostTotal As Double, _
    ByVal FixedCostTotal As Double, ByVal VariableCostTotal As Double, _
    ByVal MinPriceMargin As Double, ByVal MinSellingPrice As Double, _
    ByVal PriceVariableMarginPercent As Double, ByVal PriceVariableMarginDollar As Double, _
    ByVal PriceVariableMarginInclDeprPercent As Double, ByVal PriceVariableMarginInclDeprDollar As Double, _
    ByVal PriceGrossMarginPercent As Double, ByVal PriceGrossMarginDollar As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Overall"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@OverallCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverallCostTotal").Value = OverallCostTotal

            myCommand.Parameters.Add("@FixedCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@FixedCostTotal").Value = FixedCostTotal

            myCommand.Parameters.Add("@VariableCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@VariableCostTotal").Value = VariableCostTotal

            myCommand.Parameters.Add("@MinPriceMargin", SqlDbType.Decimal)
            myCommand.Parameters("@MinPriceMargin").Value = MinPriceMargin

            myCommand.Parameters.Add("@MinSellingPrice", SqlDbType.Decimal)
            myCommand.Parameters("@MinSellingPrice").Value = MinSellingPrice

            myCommand.Parameters.Add("@PriceVariableMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginPercent").Value = PriceVariableMarginPercent

            myCommand.Parameters.Add("@PriceVariableMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginDollar").Value = PriceVariableMarginDollar

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprPercent").Value = PriceVariableMarginInclDeprPercent

            myCommand.Parameters.Add("@PriceVariableMarginInclDeprDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceVariableMarginInclDeprDollar").Value = PriceVariableMarginInclDeprDollar

            myCommand.Parameters.Add("@PriceGrossMarginPercent", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginPercent").Value = PriceGrossMarginPercent

            myCommand.Parameters.Add("@PriceGrossMarginDollar", SqlDbType.Decimal)
            myCommand.Parameters("@PriceGrossMarginDollar").Value = PriceGrossMarginDollar

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", OverallCostTotal: " & OverallCostTotal _
            & ", FixedCostTotal: " & FixedCostTotal _
            & ", VariableCostTotal: " & VariableCostTotal _
            & ", MinPriceMargin: " & MinPriceMargin _
            & ", MinSellingPrice: " & MinSellingPrice _
            & ", PriceVariableMarginPercent: " & PriceVariableMarginPercent _
            & ", PriceVariableMarginDollar: " & PriceVariableMarginDollar _
            & ", PriceVariableMarginInclDeprPercent: " & PriceVariableMarginInclDeprPercent _
            & ", PriceVariableMarginInclDeprDollar: " & PriceVariableMarginInclDeprDollar _
            & ", PriceGrossMarginPercent: " & PriceGrossMarginPercent _
            & ", PriceGrossMarginDollar: " & PriceGrossMarginDollar _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalOverall : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalOverall : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalOverhead(ByVal CostSheetID As Integer, _
    ByVal OverheadCostTotalWOScrapFixedRate As Double, _
    ByVal OverheadCostTotalWOScrapVariableRate As Double, _
    ByVal OverheadCostTotalWOScrap As Double, _
    ByVal OverheadCostTotalFixedRate As Double, _
    ByVal OverheadCostTotalVariableRate As Double, _
    ByVal OverheadCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@OverheadCostTotalWOScrapFixedRate", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalWOScrapFixedRate").Value = OverheadCostTotalWOScrapFixedRate

            myCommand.Parameters.Add("@OverheadCostTotalWOScrapVariableRate", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalWOScrapVariableRate").Value = OverheadCostTotalWOScrapVariableRate

            myCommand.Parameters.Add("@OverheadCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalWOScrap").Value = OverheadCostTotalWOScrap

            myCommand.Parameters.Add("@OverheadCostTotalFixedRate", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalFixedRate").Value = OverheadCostTotalFixedRate

            myCommand.Parameters.Add("@OverheadCostTotalVariableRate", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotalVariableRate").Value = OverheadCostTotalVariableRate

            myCommand.Parameters.Add("@OverheadCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@OverheadCostTotal").Value = OverheadCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", OverheadCostTotalWOScrapFixedRate : " & OverheadCostTotalWOScrapFixedRate _
            & ", OverheadCostTotalWOScrapVariableRate : " & OverheadCostTotalWOScrapVariableRate _
            & ", OverheadCostTotalWOScrap : " & OverheadCostTotalWOScrap _
            & ", OverheadCostTotalFixedRate : " & OverheadCostTotalFixedRate _
            & ", OverheadCostTotalVariableRate : " & OverheadCostTotalVariableRate _
            & ", OverheadCostTotal : " & OverheadCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalPackaging(ByVal CostSheetID As Integer, ByVal PackagingCostTotalWOScrap As Double, _
        ByVal PackagingCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@PackagingCostTotalWOScrap", SqlDbType.Decimal)
            myCommand.Parameters("@PackagingCostTotalWOScrap").Value = PackagingCostTotalWOScrap

            myCommand.Parameters.Add("@PackagingCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@PackagingCostTotal").Value = PackagingCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", PackagingCostTotalWOScrap: " & PackagingCostTotalWOScrap _
            & ", PackagingCostTotal: " & PackagingCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalScrap(ByVal CostSheetID As Integer, ByVal ScrapCostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_Scrap"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@ScrapCostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@ScrapCostTotal").Value = ScrapCostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", ScrapCostTotal  : " & ScrapCostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalScrap : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalScrap : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateCostSheetTotalSGA(ByVal CostSheetID As Integer, ByVal SGACostTotal As Double)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Total_SGA"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@SGACostTotal", SqlDbType.Decimal)
            myCommand.Parameters("@SGACostTotal").Value = SGACostTotal

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", SGACostTotal: " & SGACostTotal _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTotalSGA : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTotalSGA : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetCostSheetArchive(ByVal CostSheetID As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Archive"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetArchive")
            GetCostSheetArchive = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCosSheetArchive : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCosSheetArchive : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetArchive = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetTotal(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Total"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetTotal")
            GetCostSheetTotal = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetTotal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetTotal = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetLaborMinOrdinal(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Labor_Min_Ordinal"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCostSheetLaborMinOrdinal")
            GetCostSheetLaborMinOrdinal = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetLaborMinOrdinal : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetLaborMinOrdinal : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetLaborMinOrdinal = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub DeleteCostSheetProductionLimit(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Cost_Sheet_Production_Limit"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetProductionLimit : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertCostSheetPreApproval(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, _
    ByVal RoutingLevel As Integer, ByVal SignedStatus As String, ByVal SubscriptionID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Cost_Sheet_Pre_Approval_Item"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@routingLevel", SqlDbType.Int)
            myCommand.Parameters("@routingLevel").Value = RoutingLevel

            myCommand.Parameters.Add("@signedStatus", SqlDbType.VarChar)
            myCommand.Parameters("@signedStatus").Value = SignedStatus

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SignedStatus: " & SignedStatus _
            & ", RoutingLevel: " & RoutingLevel _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPreApproval : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPreApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateFormula(ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal DrawingNo As String, _
        ByVal PartNo As String, ByVal PartRevision As String, ByVal SpecificGravity As Double, _
        ByVal SpecificGravityUnitID As Integer, ByVal MaximumMixCapacity As Integer, _
        ByVal MaximumMixCapacityUnitID As Integer, ByVal MaximumLineSpeed As Integer, _
        ByVal MaximumLineSpeedUnitID As Integer, _
        ByVal MaximumPressCycles As Integer, ByVal CoatingSides As Integer, _
        ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, _
        ByVal MaximumFormingRate As Double, ByVal MaximumFormingRateUnitID As Integer, _
        ByVal isDiecut As Boolean, _
        ByVal ProcessID As Integer, ByVal isRecycleReturn As Boolean, _
        ByVal TemplateID As Integer, ByVal isFleeceType As Boolean, _
        ByVal FormulaRevision As String, ByVal FormulaStartDate As String, _
        ByVal FormulaEndDate As String, ByVal CopyReason As String, ByVal Obsolete As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Formula"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

            If FormulaName Is Nothing Then
                FormulaName = ""
            End If

            myCommand.Parameters.Add("@FormulaName", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaName").Value = FormulaName

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            myCommand.Parameters.Add("@SpecificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@SpecificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@SpecificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@SpecificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@MaxMixCapacity", SqlDbType.Int)
            myCommand.Parameters("@MaxMixCapacity").Value = MaximumMixCapacity

            myCommand.Parameters.Add("@MaxMixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxMixCapacityUnitID").Value = MaximumMixCapacityUnitID

            myCommand.Parameters.Add("@MaxLineSpeed", SqlDbType.Int)
            myCommand.Parameters("@MaxLineSpeed").Value = MaximumLineSpeed

            myCommand.Parameters.Add("@MaxLineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxLineSpeedUnitID").Value = MaximumLineSpeedUnitID

            myCommand.Parameters.Add("@MaxPressCycles", SqlDbType.Int)
            myCommand.Parameters("@MaxPressCycles").Value = MaximumPressCycles

            myCommand.Parameters.Add("@CoatingSides", SqlDbType.Int)
            myCommand.Parameters("@CoatingSides").Value = CoatingSides

            myCommand.Parameters.Add("@WeightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@WeightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@WeightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@WeightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@MaxFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@MaxFormingRate").Value = MaximumFormingRate

            myCommand.Parameters.Add("@MaxFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxFormingRateUnitID").Value = MaximumFormingRateUnitID

            'myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            'myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@isDiecut", SqlDbType.Bit)
            myCommand.Parameters("@isDiecut").Value = isDiecut

            myCommand.Parameters.Add("@ProcessID", SqlDbType.Int)
            myCommand.Parameters("@ProcessID").Value = ProcessID

            myCommand.Parameters.Add("@isRecycleReturn", SqlDbType.Bit)
            myCommand.Parameters("@isRecycleReturn").Value = isRecycleReturn

            myCommand.Parameters.Add("@TemplateID", SqlDbType.Int)
            myCommand.Parameters("@TemplateID").Value = TemplateID

            myCommand.Parameters.Add("@isFleeceType", SqlDbType.Bit)
            myCommand.Parameters("@isFleeceType").Value = isFleeceType

            If FormulaRevision Is Nothing Then
                FormulaRevision = ""
            End If

            myCommand.Parameters.Add("@FormulaRevision", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaRevision").Value = FormulaRevision

            If FormulaStartDate Is Nothing Then
                FormulaStartDate = ""
            End If

            myCommand.Parameters.Add("@FormulaStartDate", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaStartDate").Value = FormulaStartDate

            If FormulaEndDate Is Nothing Then
                FormulaEndDate = ""
            End If

            myCommand.Parameters.Add("@FormulaEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaEndDate").Value = FormulaEndDate

            If CopyReason Is Nothing Then
                CopyReason = ""
            End If

            myCommand.Parameters.Add("@CopyReason", SqlDbType.VarChar)
            myCommand.Parameters("@CopyReason").Value = CopyReason

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID _
            & ", FormulaName: " & FormulaName _
            & ", DrawingNo: " & DrawingNo _
            & ", PartNo: " & PartNo _
            & ", SpecificGravity : " & SpecificGravity _
            & ", SpecificGravityUnitID: " & SpecificGravityUnitID _
            & ", MaximumMixCapacity: " & MaximumMixCapacity _
            & ", MaximumLineSpeed: " & MaximumLineSpeed _
            & ", MaximumPressCycles: " & MaximumPressCycles _
            & ", CoatingSides: " & CoatingSides _
            & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID _
            & ", isDiecut: " & isDiecut _
            & ", ProcessID: " & ProcessID _
            & ", isRecycleReturn: " & isRecycleReturn _
            & ", TemplateID: " & TemplateID _
            & ", isFleeceType: " & isFleeceType _
            & ", FormulaRevision: " & FormulaRevision _
            & ", FormulaStartDate: " & FormulaStartDate _
            & ", FormulaEndDate: " & FormulaEndDate _
            & ", CopyReason: " & CopyReason _
            & ", Obsolete: " & Obsolete _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormula : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateFormula : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateFormulaStatus(ByVal FormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Formula_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FormulaID", SqlDbType.Int)
            myCommand.Parameters("@FormulaID").Value = FormulaID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateFormulaStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function InsertFormula(ByVal FormulaName As String, ByVal DrawingNo As String, _
        ByVal PartNo As String, ByVal PartRevision As String, ByVal SpecificGravity As Double, _
        ByVal SpecificGravityUnitID As Integer, ByVal MaximumMixCapacity As Integer, _
        ByVal MaximumMixCapacityUnitID As Integer, ByVal MaximumLineSpeed As Integer, _
        ByVal MaximumLineSpeedUnitID As Integer, _
        ByVal MaximumPressCycles As Integer, ByVal CoatingSides As Integer, _
        ByVal WeightPerArea As Double, ByVal WeightPerAreaUnitID As Integer, _
        ByVal MaximumFormingRate As Double, ByVal MaximumFormingRateUnitID As Integer, _
        ByVal isDiecut As Boolean, _
        ByVal ProcessID As Integer, ByVal isRecycleReturn As Boolean, _
        ByVal TemplateID As Integer, ByVal isFleeceType As Boolean, _
        ByVal FormulaRevision As String, ByVal FormulaStartDate As String, _
        ByVal FormulaEndDate As String, ByVal CopyReason As String, _
        ByVal PreviousFormulaID As Integer, ByVal OriginalFormulaID As Integer, ByVal InsertType As String) As DataSet

        'ByVal DepartmentID As Integer, 
        'ByVal Obsolete As Boolean

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Formula"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If FormulaName Is Nothing Then
                FormulaName = ""
            End If

            myCommand.Parameters.Add("@FormulaName", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaName").Value = FormulaName

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            myCommand.Parameters.Add("@SpecificGravity", SqlDbType.Decimal)
            myCommand.Parameters("@SpecificGravity").Value = SpecificGravity

            myCommand.Parameters.Add("@SpecificGravityUnitID", SqlDbType.Int)
            myCommand.Parameters("@SpecificGravityUnitID").Value = SpecificGravityUnitID

            myCommand.Parameters.Add("@MaxMixCapacity", SqlDbType.Int)
            myCommand.Parameters("@MaxMixCapacity").Value = MaximumMixCapacity

            myCommand.Parameters.Add("@MaxMixCapacityUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxMixCapacityUnitID").Value = MaximumMixCapacityUnitID

            myCommand.Parameters.Add("@MaxLineSpeed", SqlDbType.Int)
            myCommand.Parameters("@MaxLineSpeed").Value = MaximumLineSpeed

            myCommand.Parameters.Add("@MaxLineSpeedUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxLineSpeedUnitID").Value = MaximumLineSpeedUnitID

            myCommand.Parameters.Add("@MaxPressCycles", SqlDbType.Int)
            myCommand.Parameters("@MaxPressCycles").Value = MaximumPressCycles

            myCommand.Parameters.Add("@CoatingSides", SqlDbType.Int)
            myCommand.Parameters("@CoatingSides").Value = CoatingSides

            myCommand.Parameters.Add("@WeightPerArea", SqlDbType.Decimal)
            myCommand.Parameters("@WeightPerArea").Value = WeightPerArea

            myCommand.Parameters.Add("@WeightPerAreaUnitID", SqlDbType.Int)
            myCommand.Parameters("@WeightPerAreaUnitID").Value = WeightPerAreaUnitID

            myCommand.Parameters.Add("@MaxFormingRate", SqlDbType.Decimal)
            myCommand.Parameters("@MaxFormingRate").Value = MaximumFormingRate

            myCommand.Parameters.Add("@MaxFormingRateUnitID", SqlDbType.Int)
            myCommand.Parameters("@MaxFormingRateUnitID").Value = MaximumFormingRateUnitID

            'myCommand.Parameters.Add("@departmentID", SqlDbType.Int)
            'myCommand.Parameters("@departmentID").Value = DepartmentID

            myCommand.Parameters.Add("@isDiecut", SqlDbType.Bit)
            myCommand.Parameters("@isDiecut").Value = isDiecut

            myCommand.Parameters.Add("@ProcessID", SqlDbType.Int)
            myCommand.Parameters("@ProcessID").Value = ProcessID

            myCommand.Parameters.Add("@isRecycleReturn", SqlDbType.Bit)
            myCommand.Parameters("@isRecycleReturn").Value = isRecycleReturn

            myCommand.Parameters.Add("@TemplateID", SqlDbType.Int)
            myCommand.Parameters("@TemplateID").Value = TemplateID

            myCommand.Parameters.Add("@isFleeceType", SqlDbType.Bit)
            myCommand.Parameters("@isFleeceType").Value = isFleeceType

            'myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            'myCommand.Parameters("@obsolete").Value = Obsolete

            If FormulaRevision Is Nothing Then
                FormulaRevision = ""
            End If

            myCommand.Parameters.Add("@FormulaRevision", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaRevision").Value = FormulaRevision

            If FormulaStartDate Is Nothing Then
                FormulaStartDate = ""
            End If

            myCommand.Parameters.Add("@FormulaStartDate", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaStartDate").Value = FormulaStartDate

            If FormulaEndDate Is Nothing Then
                FormulaEndDate = ""
            End If

            myCommand.Parameters.Add("@FormulaEndDate", SqlDbType.VarChar)
            myCommand.Parameters("@FormulaEndDate").Value = FormulaEndDate

            If CopyReason Is Nothing Then
                CopyReason = ""
            End If

            myCommand.Parameters.Add("@CopyReason", SqlDbType.VarChar)
            myCommand.Parameters("@CopyReason").Value = CopyReason

            myCommand.Parameters.Add("@PreviousFormulaID", SqlDbType.Int)
            myCommand.Parameters("@PreviousFormulaID").Value = PreviousFormulaID

            myCommand.Parameters.Add("@OriginalFormulaID", SqlDbType.Int)
            myCommand.Parameters("@OriginalFormulaID").Value = OriginalFormulaID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myCommand.Parameters.Add("@InsertType", SqlDbType.VarChar)
            myCommand.Parameters("@InsertType").Value = InsertType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewFormulaID")
            InsertFormula = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaName: " & FormulaName _
            & ", DrawingNo: " & DrawingNo _
            & ", PartNo: " & PartNo _
            & ", SpecificGravity : " & SpecificGravity _
            & ", SpecificGravityUnitID: " & SpecificGravityUnitID _
            & ", MaximumMixCapacity: " & MaximumMixCapacity _
            & ", MaximumLineSpeed: " & MaximumLineSpeed _
            & ", MaximumPressCycles: " & MaximumPressCycles _
            & ", CoatingSides: " & CoatingSides _
            & ", WeightPerArea: " & WeightPerArea _
            & ", WeightPerAreaUnitID: " & WeightPerAreaUnitID _
            & ", isDiecut: " & isDiecut _
            & ", ProcessID: " & ProcessID _
            & ", isRecycleReturn: " & isRecycleReturn _
            & ", TemplateID: " & TemplateID _
            & ", isFleeceType: " & isFleeceType _
            & ", FormulaRevision: " & FormulaRevision _
            & ", FormulaStartDate: " & FormulaStartDate _
            & ", FormulaEndDate: " & FormulaEndDate _
            & ", CopyReason: " & CopyReason _
            & ", PreviousFormulaID: " & PreviousFormulaID _
            & ", OriginalFormulaID: " & OriginalFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormula : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertFormula : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            InsertFormula = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub CopyFormulaDepartment(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Department"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaDepartment: " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyFormulaCoatingFactor(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Coating_Factor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaDeplugFactor(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Deplug_Factor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaMaterial(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyFormulaMaterialReplaceObsolete(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Material_Replace_Obsolete"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewFormulaID", SqlDbType.Int)
            myCommand.Parameters("@NewFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@OldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@OldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaMaterialReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaMaterialReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaPackaging(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Packaging"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaPackagingReplaceObsolete(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Packaging_Replace_Obsolete"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewFormulaID", SqlDbType.Int)
            myCommand.Parameters("@NewFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@OldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@OldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaPackagingReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaPackagingReplaceObsolete : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaLabor(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Labor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaOverhead(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Overhead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyFormulaMiscCost(ByVal NewFormulaID As Integer, ByVal OldFormulaID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Formula_Misc_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newFormulaID", SqlDbType.Int)
            myCommand.Parameters("@newFormulaID").Value = NewFormulaID

            myCommand.Parameters.Add("@oldFormulaID", SqlDbType.Int)
            myCommand.Parameters("@oldFormulaID").Value = OldFormulaID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewFormulaID: " & NewFormulaID & "OldFormulaID: " & OldFormulaID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyFormulaMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyFormulaMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function InsertMaterial(ByVal MaterialName As String, ByVal MaterialDesc As String, ByVal PartNo As String, _
    ByVal PartRevision As String, ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal PurchasedGoodID As Integer, ByVal UGNFacilityCode As String, _
    ByVal QuoteCost As Double, ByVal QuoteCostDate As String, ByVal FreightCost As Double, ByVal FreightCostDate As String, _
    ByVal UnitID As Integer, ByVal isCoating As Boolean, ByVal isPackaging As Boolean, ByVal Obsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialName Is Nothing Then MaterialName = ""

            myCommand.Parameters.Add("@materialName", SqlDbType.VarChar)
            myCommand.Parameters("@materialName").Value = MaterialName

            If MaterialDesc Is Nothing Then MaterialDesc = ""

            myCommand.Parameters.Add("@materialDesc", SqlDbType.VarChar)
            myCommand.Parameters("@materialDesc").Value = MaterialDesc

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartRevision Is Nothing Then PartRevision = ""

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            If DrawingNo Is Nothing Then DrawingNo = ""

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@ugndbVendorID", SqlDbType.Int)
            myCommand.Parameters("@ugndbVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@UGNFacilityCode", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacilityCode").Value = UGNFacilityCode

            myCommand.Parameters.Add("@quoteCost", SqlDbType.Decimal)
            myCommand.Parameters("@quoteCost").Value = QuoteCost

            If QuoteCostDate Is Nothing Then QuoteCostDate = ""

            myCommand.Parameters.Add("@quoteCostDate", SqlDbType.VarChar)
            myCommand.Parameters("@quoteCostDate").Value = QuoteCostDate

            myCommand.Parameters.Add("@freightCost", SqlDbType.Decimal)
            myCommand.Parameters("@freightCost").Value = FreightCost

            If FreightCostDate Is Nothing Then FreightCostDate = ""

            myCommand.Parameters.Add("@freightCostDate", SqlDbType.VarChar)
            myCommand.Parameters("@freightCostDate").Value = FreightCostDate

            myCommand.Parameters.Add("@unitID", SqlDbType.Int)
            myCommand.Parameters("@unitID").Value = UnitID

            myCommand.Parameters.Add("@isCoating", SqlDbType.Bit)
            myCommand.Parameters("@isCoating").Value = isCoating

            myCommand.Parameters.Add("@isPackaging", SqlDbType.Bit)
            myCommand.Parameters("@isPackaging").Value = isPackaging

            myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            myCommand.Parameters("@obsolete").Value = Obsolete

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewMaterial")
            InsertMaterial = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialName: " & MaterialName _
            & ", MaterialDesc: " & MaterialDesc & ", PartNo: " & PartNo & ", PartRevision : " & PartRevision _
            & ", DrawingNo: " & DrawingNo & ", UGNDBVendorID: " & UGNDBVendorID & ", PurchasedGoodID: " & PurchasedGoodID & ", UGNFacilityCode: " & UGNFacilityCode _
            & ", QuoteCost: " & QuoteCost & ", QuoteCostDate: " & QuoteCostDate _
            & ", FreightCost: " & FreightCost & ", FreightCostDate: " & FreightCostDate _
            & ", UnitID: " & UnitID & ", isCoating: " & isCoating & ", isPackaging: " & isPackaging _
            & ", Obsolete: " & Obsolete _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            InsertMaterial = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub UpdateMaterial(ByVal MaterialID As Integer, ByVal MaterialName As String, ByVal MaterialDesc As String, ByVal PartNo As String, _
    ByVal PartRevision As String, ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal PurchasedGoodID As Integer, _
    ByVal UGNFacilityCode As String, ByVal QuoteCost As Double, ByVal QuoteCostDate As String, ByVal FreightCost As Double, ByVal FreightCostDate As String, _
    ByVal UnitID As Integer, ByVal isCoating As Boolean, ByVal isPackaging As Boolean, ByVal Obsolete As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Material"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@materialID", SqlDbType.Int)
            myCommand.Parameters("@materialID").Value = MaterialID

            If MaterialName Is Nothing Then MaterialName = ""

            myCommand.Parameters.Add("@materialName", SqlDbType.VarChar)
            myCommand.Parameters("@materialName").Value = MaterialName

            If MaterialDesc Is Nothing Then MaterialDesc = ""

            myCommand.Parameters.Add("@materialDesc", SqlDbType.VarChar)
            myCommand.Parameters("@materialDesc").Value = MaterialDesc

            If PartNo Is Nothing Then PartNo = ""

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartRevision Is Nothing Then PartRevision = ""

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            If DrawingNo Is Nothing Then DrawingNo = ""

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@ugndbVendorID", SqlDbType.Int)
            myCommand.Parameters("@ugndbVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@UGNFacilityCode", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacilityCode").Value = UGNFacilityCode


            myCommand.Parameters.Add("@quoteCost", SqlDbType.Decimal)
            myCommand.Parameters("@quoteCost").Value = QuoteCost

            If QuoteCostDate Is Nothing Then QuoteCostDate = ""

            myCommand.Parameters.Add("@quoteCostDate", SqlDbType.VarChar)
            myCommand.Parameters("@quoteCostDate").Value = QuoteCostDate

            myCommand.Parameters.Add("@freightCost", SqlDbType.Decimal)
            myCommand.Parameters("@freightCost").Value = FreightCost

            If FreightCostDate Is Nothing Then FreightCostDate = ""

            myCommand.Parameters.Add("@freightCostDate", SqlDbType.VarChar)
            myCommand.Parameters("@freightCostDate").Value = FreightCostDate

            myCommand.Parameters.Add("@unitID", SqlDbType.Int)
            myCommand.Parameters("@unitID").Value = UnitID

            myCommand.Parameters.Add("@isCoating", SqlDbType.Bit)
            myCommand.Parameters("@isCoating").Value = isCoating

            myCommand.Parameters.Add("@isPackaging", SqlDbType.Bit)
            myCommand.Parameters("@isPackaging").Value = isPackaging

            myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            myCommand.Parameters("@obsolete").Value = Obsolete

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialID: " & MaterialID & " ,MaterialName: " & MaterialName _
            & ", MaterialDesc: " & MaterialDesc & ", PartNo: " & PartNo & ", PartRevision : " & PartRevision _
            & ", DrawingNo: " & DrawingNo & ", UGNDBVendorID: " & UGNDBVendorID & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", QuoteCost: " & QuoteCost & ", QuoteCostDate: " & QuoteCostDate _
            & ", FreightCost: " & FreightCost & ", FreightCostDate: " & FreightCostDate _
            & ", UnitID: " & UnitID _
            & ", isCoating: " & isCoating & ", isPackaging: " & isPackaging _
            & ", Obsolete: " & Obsolete _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetMaterialStandardCost(ByVal PartNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Material_BPCS_Standard_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialStandardCost")
            GetMaterialStandardCost = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetMaterialStandardCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMaterialStandardCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMaterialStandardCost = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetMaterialPurchasedCost(ByVal PartNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Material_BPCS_Purchased_Cost"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialPurchasedCost")
            GetMaterialPurchasedCost = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetMaterialPurchasedCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMaterialPurchasedCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetMaterialPurchasedCost = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetPreApprovalList(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, _
    ByVal RoutingLevel As Integer, ByVal SignedStatus As String, ByVal SubscriptionID As Integer, ByVal filterNotified As Boolean, _
    ByVal isNotified As Boolean, ByVal isHistorical As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Pre_Approval_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@routingLevel", SqlDbType.Int)
            myCommand.Parameters("@routingLevel").Value = RoutingLevel

            If SignedStatus Is Nothing Then
                SignedStatus = ""
            End If

            myCommand.Parameters.Add("@signedStatus", SqlDbType.VarChar)
            myCommand.Parameters("@signedStatus").Value = SignedStatus

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@filterNotified", SqlDbType.Bit)
            myCommand.Parameters("@filterNotified").Value = filterNotified

            myCommand.Parameters.Add("@isNotified", SqlDbType.Bit)
            myCommand.Parameters("@isNotified").Value = isNotified

            myCommand.Parameters.Add("@isHistorical", SqlDbType.Bit)
            myCommand.Parameters("@isHistorical").Value = isHistorical

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPreApprovalList")
            GetCostSheetPreApprovalList = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & _
            ", TeamMemberID: " & TeamMemberID & ", RoutingLevel: " & RoutingLevel & _
            ", SignedStatus: " & SignedStatus & ", SubscriptionID: " & SubscriptionID & _
            ", filterNotified: " & filterNotified & ", isNotified: " & isNotified & _
            ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPreApprovalList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPreApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPreApprovalList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub CopyCostSheetPreApprovalList(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Pre_Approval_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & "OldCostSheetID: " & OldCostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetPreApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetPreApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub CopyCostSheetPostApprovalList(ByVal NewCostSheetID As Integer, ByVal OldCostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Cost_Sheet_Post_Approval_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@newCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@newCostSheetID").Value = NewCostSheetID

            myCommand.Parameters.Add("@oldCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@oldCostSheetID").Value = OldCostSheetID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewCostSheetID: " & NewCostSheetID & "OldCostSheetID: " & OldCostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetPostApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetPostApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetPreApprovalNotificationDate(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Pre_Approval_Notification_Date"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", TeamMemberID: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPreApprovalNotificationDate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPreApprovalNotificationDate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetCostSheetApproverBySubscription(ByVal SubscriptionID As Integer, ByVal RoutingLevel As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Approver_By_Subscription"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@routingLevel", SqlDbType.Int)
            myCommand.Parameters("@routingLevel").Value = RoutingLevel

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ApproverListBySubscription")
            GetCostSheetApproverBySubscription = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubscriptionID: " & SubscriptionID & _
            ",RoutingLevel: " & RoutingLevel & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetApproverBySubscription : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetApproverBySubscription : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetApproverBySubscription = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetCostSheetSubscriptionByApprover(ByVal TeamMemberID As Integer, ByVal RoutingLevel As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Subscription_By_Approver"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@routingLevel", SqlDbType.Int)
            myCommand.Parameters("@routingLevel").Value = RoutingLevel

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SubscriptionByApprover")
            GetCostSheetSubscriptionByApprover = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & _
            ",RoutingLevel: " & RoutingLevel & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetSubscriptionByApprover : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetSubscriptionByApprover : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetSubscriptionByApprover = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub CopyNotificationGroupToPreApprovalList(ByVal GroupID As Integer, ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Notification_Group_To_Pre_Approval_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@groupID", SqlDbType.Int)
            myCommand.Parameters("@groupID").Value = GroupID

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID & ", CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetCostSheetPostApprovalTeamMembers() As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Post_Approval_Team_Members"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPostApprovalTeamMembers")
            GetCostSheetPostApprovalTeamMembers = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetPostApprovalTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPostApprovalTeamMembers : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPostApprovalTeamMembers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetPostApprovalComments(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Post_Approval_Comments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPostApprovalComments")
            GetCostSheetPostApprovalComments = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPostApprovalComments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPostApprovalComments : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPostApprovalComments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateCostSheetPostApprovalComments(ByVal CostSheetID As Integer, ByVal PostApprovalComments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Post_Approval_Comments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            If PostApprovalComments Is Nothing Then
                PostApprovalComments = ""
            End If

            myCommand.Parameters.Add("@postApprovalComments", SqlDbType.VarChar)
            myCommand.Parameters("@postApprovalComments").Value = PostApprovalComments

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", PostApprovalComments: " & PostApprovalComments _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPostApprovalComments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPostApprovalComments : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateCostSheetApproved(ByVal CostSheetID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Approved"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetApproved : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetApproved : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetCostSheetCustomerProgram(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetCustomerPrograms")
            GetCostSheetCustomerProgram = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetCustomerProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetPostApprovalList(ByVal CostSheetID As String, ByVal filterNotified As Boolean, _
    ByVal isNotified As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Post_Approval_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.VarChar)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@filterNotified", SqlDbType.Bit)
            myCommand.Parameters("@filterNotified").Value = filterNotified

            myCommand.Parameters.Add("@isNotified", SqlDbType.Bit)
            myCommand.Parameters("@isNotified").Value = isNotified

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PostApprovalList")
            GetCostSheetPostApprovalList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", filterNotified: " & filterNotified & ", isNotified: " & isNotified _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPostApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPostApprovalList : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPostApprovalList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateCostSheetPostApprovalItem(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Cost_Sheet_Post_Approval_Item"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page            
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPostApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPostApprovalItem : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetCostSheetPreApproverNames() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Pre_Approver_Names"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetPreApproverNames")
            GetCostSheetPreApproverNames = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetPreApproverNames : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPreApproverNames : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetPreApproverNames = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetTopLevelPartInfo(ByVal CostSheetID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Top_Level_BPCSPart_Info"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@costSheetID", SqlDbType.Int)
            myCommand.Parameters("@costSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostSheetTopLevelPartInfo")

            GetCostSheetTopLevelPartInfo = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetTopLevelPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetTopLevelPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetTopLevelPartInfo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    'Public Shared Function GetCostSheetLastFiveReviewed(ByVal TeamMemberID As Integer) As DataSet
    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Last_Five_Reviewed"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
    '        myCommand.Parameters("@teamMemberID").Value = TeamMemberID

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "LastFiveCostSheets")
    '        GetCostSheetLastFiveReviewed = GetData
    '    Catch ex As Exception

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & _
    '        ",User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "GetCostSheetLastFiveReviewed : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetCostSheetLastFiveReviewed : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

    '        GetCostSheetLastFiveReviewed = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function

    Public Shared Function GetCostSheetTeamMemberRecentSubscription(ByVal TeamMemberID As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Team_Member_Recent_Subscription"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "RecentSubscription")
            GetCostSheetTeamMemberRecentSubscription = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID _
            & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetTeamMemberRecentSubscription : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetTeamMemberRecentSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostingModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetTeamMemberRecentSubscription = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostingDepartmentList(ByVal DepartmentName As String, ByVal UGNFacility As String, ByVal Filter As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Costing_Department_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@departmentName", SqlDbType.VarChar)
            myCommand.Parameters("@departmentName").Value = DepartmentName

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            myCommand.Parameters.Add("@filter", SqlDbType.Bit)
            myCommand.Parameters("@filter").Value = Filter

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostingDepartmentList")
            GetCostingDepartmentList = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DepartmentName: " & DepartmentName & ", UGNFacility: " & UGNFacility _
            & ", Filter: " & Filter & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostingDepartmentList : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostingDepartmentList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostingDepartmentList = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetReplicatedTo(ByVal CostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Replicated_To"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CostSheetID", SqlDbType.Int)
            myCommand.Parameters("@CostSheetID").Value = CostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CostingreplicatedTo")
            GetCostSheetReplicatedTo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetReplicatedTo : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetReplicatedTo : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetReplicatedTo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetCostSheetReplicatedFrom(ByVal PreviousCostSheetID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cost_Sheet_Replicated_From"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PreviousCostSheetID", SqlDbType.Int)
            myCommand.Parameters("@PreviousCostSheetID").Value = PreviousCostSheetID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PreviousCostingreplicatedFrom")
            GetCostSheetReplicatedFrom = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PreviousCostSheetID: " & PreviousCostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetReplicatedFrom : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetReplicatedFrom : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCostSheetReplicatedFrom = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
End Class
