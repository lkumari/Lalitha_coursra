''******************************************************************************************************
''* CostSheetLaborBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/26/2009
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 05/18/2010 - Added StandardCostPerUnitWOScrap and StandardCostFactor columns
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetLaborBLL
    Private CostSheetMaterialsAdapter As CostSheetLaborTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetLaborTableAdapter
        Get
            If CostSheetMaterialsAdapter Is Nothing Then
                CostSheetMaterialsAdapter = New CostSheetLaborTableAdapter()
            End If
            Return CostSheetMaterialsAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetLabor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetLabor(ByVal CostSheetID As Integer, ByVal LaborID As Integer, _
    ByVal filterOffline As Boolean, ByVal isOffline As Boolean) As Costing.CostSheetLabor_MaintDataTable

        Try

            Return Adapter.GetCostSheetLabor(CostSheetID, LaborID, filterOffline, isOffline)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & "LaborID: " & LaborID _
            & ", filterOffline: " & filterOffline & ", isOffline: " & isOffline _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetLabor(ByVal CostSheetID As Integer, ByVal LaborID As Integer, _
    ByVal Rate As Double, ByVal CrewSize As Double, ByVal StandardCostFactor As Double, _
    ByVal Ordinal As Integer, ByVal isOffline As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetLabor(CostSheetID, LaborID, Rate, CrewSize, StandardCostFactor, isOffline, Ordinal, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID _
            & ", LaborID: " & LaborID _
            & ", Rate: " & Rate _
            & ", CrewSize: " & CrewSize _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", isOffline: " & isOffline & ", Ordinal: " & Ordinal _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostingLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetLaborBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostingLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update CostSheetLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetLabor(ByVal Rate As Double, ByVal CrewSize As Double, ByVal Ordinal As Integer, _
        ByVal isOffline As Boolean, ByVal StandardCostFactor As Double, ByVal StandardCostPerUnitWOScrap As Double, _
        ByVal StandardCostPerUnit As Double, ByVal original_RowID As Integer, ByVal LaborID As Integer, _
        ByVal ddLaborDesc As String, ByVal CostSheetID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetLabor(original_RowID, LaborID, Rate, CrewSize, _
            StandardCostFactor, StandardCostPerUnitWOScrap, StandardCostPerUnit, isOffline, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", LaborID: " & LaborID _
            & ", Rate: " & Rate & ", CrewSize: " & CrewSize & ", isOffline: " & isOffline _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", Ordinal: " & Ordinal _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostingLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetLaborBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostingLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetLabor(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Obsolete the record
            ''*****
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetLabor(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetLabor(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
