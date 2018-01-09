''******************************************************************************************************
''* CostSheetMaterialBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/26/2009
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 01/12/2010 - CO-2822 - Freight can be saver per cost sheet
''* Modified: Roderick Carlson 05/18/2010 - Added StandardCostPerUnitWOScrap column
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetMaterialBLL
    Private CostSheetMaterialAdapter As CostSheetMaterialTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetMaterialTableAdapter
        Get
            If CostSheetMaterialAdapter Is Nothing Then
                CostSheetMaterialAdapter = New CostSheetMaterialTableAdapter()
            End If
            Return CostSheetMaterialAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetMaterials returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetMaterial(ByVal CostSheetID As Integer, ByVal MaterialID As Integer) As Costing.CostSheetMaterial_MaintDataTable

        Try

            Return Adapter.GetCostSheetMaterial(CostSheetID, MaterialID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & "MaterialID: " & MaterialID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetMaterial : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMaterialBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMaterialsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetMaterials
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetMaterial(ByVal CostSheetID As Integer, ByVal MaterialID As Integer, ByVal Quantity As Double, _
    ByVal UsageFactor As Double, ByVal CostPerUnit As Double, ByVal FreightCost As Double, _
    ByVal StandardCostFactor As Double, ByVal Ordinal As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetMaterial(CostSheetID, MaterialID, Quantity, UsageFactor, CostPerUnit, _
            FreightCost, StandardCostFactor, Ordinal, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & ", MaterialID: " & MaterialID _
            & ", Quantity: " & Quantity & ", UsageFactor: " & UsageFactor & ", CostPerUnit: " & CostPerUnit & ", FreightCost: " & FreightCost _
            & ", StandardCostFactor: " & StandardCostFactor & ", Ordinal: " & Ordinal & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetMaterial : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMaterialBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetMaterial : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMaterialsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingMaterialsBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetMaterial(ByVal MaterialID As Integer, ByVal Quantity As Double, ByVal UsageFactor As Double, _
        ByVal CostPerUnit As Double, ByVal FreightCost As Double, ByVal StandardCostFactor As Double, ByVal QuoteCostFactor As Double, _
        ByVal StandardCostPerUnitWOScrap As Double, ByVal StandardCostPerUnit As Double, _
        ByVal Ordinal As Integer, ByVal original_RowID As Integer, ByVal ddMaterialDesc As String, _
        ByVal CostSheetID As Integer, ByVal ddMaterialName As String, ByVal ddMaterialNameCombo As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetMaterial(original_RowID, MaterialID, Quantity, _
            UsageFactor, CostPerUnit, FreightCost, StandardCostFactor, QuoteCostFactor, StandardCostPerUnitWOScrap, _
            StandardCostPerUnit, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", MaterialID: " & MaterialID _
            & ", Quantity: " & Quantity & ", UsageFactor: " & UsageFactor & ", CostPerUnit: " & CostPerUnit _
            & ", FreightCost: " & FreightCost _
            & ", StandardCostFactor: " & StandardCostFactor _
            & ", QuoteCostFactor: " & QuoteCostFactor _
            & ", StandardCostPerUnitWOScrap: " & StandardCostPerUnitWOScrap _
            & ", StandardCostPerUnit: " & StandardCostPerUnit _
            & ", Ordinal: " & Ordinal _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetMaterial : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMaterialBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetMaterial : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMaterialsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetMaterialsBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetMaterial(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetMaterial(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetMaterial(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID.ToString & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetMaterialBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMaterialBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
