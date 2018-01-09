''******************************************************************************************************
''* CostSheetOverheadBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/30/2009
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 05/18/2010 - added Variable Rate
''* Modified: Roderick Carlson 05/18/2010 - Added StandardCostPerUnitWOScrap, StandardCostPerUnitWOScrapFixedRate, StandardCostPerUnitWOScrapVariableRate, StandardCostPerUnitFixedRate, StandardCostPerUnitVariableRate and StandardCostFactor columns
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetOverheadBLL
    Private CostingOverheadAdapter As CostSheetOverheadTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetOverheadTableAdapter
        Get
            If CostingOverheadAdapter Is Nothing Then
                CostingOverheadAdapter = New CostSheetOverheadTableAdapter()
            End If
            Return CostingOverheadAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetOverhead returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetOverhead(ByVal CostSheetID As Integer, ByVal LaborID As Integer) As Costing.CostSheetOverhead_MaintDataTable

        Try

            Return Adapter.GetCostSheetOverhead(CostSheetID, LaborID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", LaborID: " & LaborID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingOverheadBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetOverhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetOverhead(ByVal CostSheetID As Integer, ByVal LaborID As Integer, _
    ByVal Rate As Double, ByVal VariableRate As Double, ByVal CrewSize As Double, ByVal StandardCostFactor As Double, _
    ByVal Ordinal As Integer, ByVal isOffline As Boolean, ByVal isProportion As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetOverhead(CostSheetID, LaborID, Rate, VariableRate, CrewSize, StandardCostFactor, isOffline, isProportion, Ordinal, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID _
            & ", LaborID: " & LaborID _
            & ", Rate: " & Rate _
            & ", CrewSize: " & CrewSize _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", Ordinal: " & Ordinal _
            & ", isOffline: " & isOffline _
            & ", isProportion: " & isProportion _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingOverheadBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingOverheadBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetOverhead(ByVal LaborID As Integer, ByVal Rate As Double, ByVal VariableRate As Double, _
        ByVal CrewSize As Double, ByVal NumberofCarriers As Double, ByVal Ordinal As Integer, ByVal isOffline As Boolean, _
        ByVal isProportion As Boolean, ByVal StandardCostFactor As Double, ByVal StandardCostPerUnitWOScrapFixedRate As Double, _
        ByVal StandardCostPerUnitWOScrapVariableRate As Double, ByVal StandardCostPerUnitWOScrap As Double, _
        ByVal StandardCostPerUnitFixedRate As Double, ByVal StandardCostPerUnitVariableRate As Double, ByVal StandardCostPerUnit As Double, _
        ByVal original_RowID As Integer, ByVal ddLaborDesc As String, ByVal CostSheetID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetOverhead(original_RowID, LaborID, Rate, VariableRate, CrewSize, _
            NumberofCarriers, StandardCostFactor, StandardCostPerUnitWOScrapFixedRate, StandardCostPerUnitWOScrapVariableRate, _
            StandardCostPerUnitWOScrap, StandardCostPerUnitFixedRate, StandardCostPerUnitVariableRate, _
            StandardCostPerUnit, isOffline, isProportion, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", LaborID: " & LaborID _
            & ", Rate: " & Rate _
            & ", VariableRate: " & VariableRate _
            & ", CrewSize: " & CrewSize _
            & ", NumberofCarriers: " & NumberofCarriers _
            & ", Ordinal: " & Ordinal _
            & ", isOffline: " & isOffline _
            & ", isProportion: " & isProportion _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrapFixedRate: " & StandardCostPerUnitWOScrapFixedRate _
            & ", StandardCostPerUnitWOScrapVariableRate: " & StandardCostPerUnitWOScrapVariableRate _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnitFixedRate: " & StandardCostPerUnitFixedRate _
            & ", StandardCostPerUnitVariableRate: " & StandardCostPerUnitVariableRate _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingOverheadBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetOverhead : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetOverheadBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetOverhead(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetOverhead(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetOverhead(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
