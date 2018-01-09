''******************************************************************************************************
''* CostSheetPackagingBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/28/2009
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 05/18/2010 - Added StandardCostPerUnitWOScrap and StandardCostFactor columns
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetPackagingBLL
    Private CostingPackagingAdapter As CostSheetPackagingTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetPackagingTableAdapter
        Get
            If CostingPackagingAdapter Is Nothing Then
                CostingPackagingAdapter = New CostSheetPackagingTableAdapter()
            End If
            Return CostingPackagingAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetPackaging returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetPackaging(ByVal CostSheetID As Integer, ByVal MaterialID As Integer) As Costing.CostSheetPackaging_MaintDataTable

        Try

            Return Adapter.GetCostSheetPackaging(CostSheetID, MaterialID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & "MaterialID: " & MaterialID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingPackagingBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetPackaging
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetPackaging(ByVal CostSheetID As Integer, ByVal MaterialID As Integer, _
    ByVal CostPerUnit As Double, ByVal UnitsNeeded As Double, ByVal PartsPerContainer As Integer, ByVal StandardCostFactor As Double, _
    ByVal isUsed As Boolean, ByVal Ordinal As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetPackaging(CostSheetID, MaterialID, CostPerUnit, UnitsNeeded, _
            PartsPerContainer, StandardCostFactor, isUsed, Ordinal, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID _
            & ", MaterialID: " & MaterialID _
            & ", CostPerUnit: " & CostPerUnit _
            & ", UnitsNeeded: " & UnitsNeeded _
            & ", PartsPerContainer: " & PartsPerContainer _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", isUsed: " & isUsed & ", Ordinal: " & Ordinal _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingPackagingBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetPackaging(ByVal MaterialID As Integer, ByVal CostPerUnit As Double, _
        ByVal UnitsNeeded As Double, ByVal PartsPerContainer As Integer, _
        ByVal StandardCostFactor As Double, ByVal StandardCostPerUnitWOScrap As Double, _
        ByVal StandardCostPerUnit As Double, ByVal isUsed As Boolean, _
        ByVal Ordinal As Integer, ByVal original_RowID As Integer, ByVal ddMaterialDesc As String, _
        ByVal CostSheetID As Integer, ByVal ddMaterialName As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetPackaging(original_RowID, MaterialID, CostPerUnit, _
            UnitsNeeded, PartsPerContainer, StandardCostFactor, StandardCostPerUnitWOScrap, _
            StandardCostPerUnit, isUsed, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", MaterialID: " & MaterialID _
            & ", CostPerUnit: " & CostPerUnit _
            & ", UnitsNeeded: " & UnitsNeeded _
            & ", PartsPerContainer: " & PartsPerContainer _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", isUsed: " & isUsed _
            & ", Ordinal: " & Ordinal _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPackaging : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPackaging : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetPackagingBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetPackaging(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetPackaging(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetPackaging(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPackagingBLLBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
