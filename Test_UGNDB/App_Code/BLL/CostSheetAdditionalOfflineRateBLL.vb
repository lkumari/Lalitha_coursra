''******************************************************************************************************
''* CostSheetAdditionalOfflineRateBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/26/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetAdditionalOfflineRateBLL
    Private CostSheetMaterialsAdapter As CostSheetAdditionalOfflineRateTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetAdditionalOfflineRateTableAdapter
        Get
            If CostSheetMaterialsAdapter Is Nothing Then
                CostSheetMaterialsAdapter = New CostSheetAdditionalOfflineRateTableAdapter()
            End If
            Return CostSheetMaterialsAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetAdditionalOfflineRates returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetAdditionalOfflineRate(ByVal CostSheetID As Integer, ByVal LaborID As Integer) As Costing.CostSheetAdditionalOfflineRate_MaintDataTable

        Try

            Return Adapter.GetCostSheetAdditionalOfflineRate(CostSheetID, LaborID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & "LaborID: " & LaborID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAdditionalOfflineRateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAdditionalOfflineRatsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetAdditionalOfflineRates
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetAdditionalOfflineRate(ByVal CostSheetID As Integer, ByVal LaborID As Integer, ByVal PiecesPerHour As Double, ByVal Ordinal As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetAdditionalOfflineRate(CostSheetID, LaborID, PiecesPerHour, Ordinal, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & ", LaborID: " & LaborID & ", PiecesPerHour: " & PiecesPerHour & ", Ordinal: " & Ordinal & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAdditionalOfflineRateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAdditionalOfflineRateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostSheetAdditionalOfflineRates
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetAdditionalOfflineRate(ByVal PiecesPerHour As Double, ByVal Ordinal As Integer, ByVal original_RowID As Integer, ByVal original_LaborID As Integer, ByVal ddLongLaborDesc As String, ByVal LaborID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the  record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetAdditionalOfflineRate(original_RowID, LaborID, PiecesPerHour, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", LaborID: " & LaborID & ", PiecesPerHour: " & PiecesPerHour & ", Ordinal: " & Ordinal & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAdditionalOfflineRateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAdditionalOfflineRateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetAdditionalOfflineRates
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetAdditionalOfflineRates(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Obsolete the record
            ''*****
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetAdditionalOfflineRate(original_rowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetAdditionalOfflineRate(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID.ToString & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAdditionalOfflineRateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostingAdditionalOfflineRate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAdditionalOfflineRateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
