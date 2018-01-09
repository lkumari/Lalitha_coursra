''******************************************************************************************************
''* MaterialBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/13/2009
''* Modified: {Name} {Date} - {Notes}
''            Roderick Carlson 12/12/2012 - In GetMaterial, made sure MaterialID was NOT nothing
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class MaterialBLL
    Private MaterialAdapter As MaterialTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.MaterialTableAdapter
        Get
            If MaterialAdapter Is Nothing Then
                MaterialAdapter = New MaterialTableAdapter()
            End If
            Return MaterialAdapter
        End Get
    End Property
    ''*****
    ''* Select Material returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetMaterial(ByVal MaterialID As String, ByVal PartName As String, ByVal PartNo As String, _
    ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal PurchasedGoodID As Integer, ByVal UGNFacilityCode As String, ByVal OldMaterialGroup As String, _
    ByVal isPackaging As Integer, ByVal filterPackaging As Integer, ByVal isCoating As Integer, ByVal filterCoating As Integer, _
    ByVal Obsolete As Integer, ByVal filterObsolete As Integer) As Costing.Material_MaintDataTable

        Try
            If MaterialID Is Nothing Then MaterialID = ""

            If PartName Is Nothing Then PartName = ""

            If PartNo Is Nothing Then PartNo = ""

            If DrawingNo Is Nothing Then DrawingNo = ""

            If OldMaterialGroup Is Nothing Then OldMaterialGroup = ""

            Return Adapter.GetMaterial(MaterialID, PartName, PartNo, DrawingNo, UGNDBVendorID, _
                                       PurchasedGoodID, UGNFacilityCode, OldMaterialGroup, isPackaging, filterPackaging, _
                                       isCoating, filterCoating, Obsolete, filterObsolete)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialID: " & MaterialID _
            & ",PartName: " & PartName & ",PartNo: " & PartNo _
            & ",DrawingNo: " & DrawingNo & ", UGNDBVendorID: " & UGNDBVendorID _
            & ",PurchasedGoodID: " & PurchasedGoodID _
            & ",UGNFacilityCode: " & UGNFacilityCode _
            & ",OldMaterialGroup: " & OldMaterialGroup _
            & ",isPackaging: " & isPackaging _
            & ",filterPackaging: " & filterPackaging _
            & ",isCoating: " & isCoating _
            & ",filterCoating: " & filterCoating _
            & ",Obsolete: " & Obsolete _
            & ",filterObsolete: " & filterObsolete _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MaterialBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "MaterialBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New Material
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertMaterial(ByVal MaterialName As String, ByVal MaterialDesc As String, ByVal PartNo As String, _
    '    ByVal PartRevision As String, ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal purchasedGoodID As Integer, _
    '    ByVal PriceChangeDate As String, ByVal StandardCost As Decimal, ByVal BPCSPurchasedCost As Decimal, ByVal QuoteCost As Decimal, _
    '    ByVal FreightCost As Decimal, ByVal UnitID As Integer, ByVal isCoating As Boolean, ByVal isPackaging As Boolean, _
    '    ByVal Obsolete As Boolean) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If MaterialName Is Nothing Then
    '            MaterialName = ""
    '        End If

    '        If MaterialDesc Is Nothing Then
    '            MaterialDesc = ""
    '        End If

    '        If PartNo Is Nothing Then
    '            PartNo = ""
    '        End If

    '        If PartRevision Is Nothing Then
    '            PartRevision = ""
    '        End If

    '        If DrawingNo Is Nothing Then
    '            DrawingNo = ""
    '        End If

    '        If PriceChangeDate Is Nothing Then
    '            PriceChangeDate = ""
    '        End If

    '        ''*****
    '        ' Insert the record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.InsertMaterial(MaterialName, MaterialDesc, PartNo, PartRevision, DrawingNo, _
    '        UGNDBVendorID, purchasedGoodID, PriceChangeDate, StandardCost, BPCSPurchasedCost, QuoteCost, FreightCost, _
    '        UnitID, isCoating, isPackaging, Obsolete, createdBy)

    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "MaterialName: " & MaterialName & _
    '         ", MaterialDesc: " & MaterialDesc & ", PartNo: " & PartNo & _
    '         ", PartRevision: " & PartRevision & ", DrawingNo: " & DrawingNo & ", UGNDBVendorID: " & UGNDBVendorID & _
    '         ", purchasedGoodID: " & purchasedGoodID & ", PriceChangeDate: " & PriceChangeDate & ", StandardCost: " & StandardCost & _
    '         ", BPCSPurchasedGost: " & BPCSPurchasedCost & ", QuoteCost: " & QuoteCost & ", FreightCost: " & FreightCost & _
    '         ", UnitID: " & UnitID & ", isCoating: " & isCoating & ", isPackaging: " & isPackaging & _
    '         ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertMaterial : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MaterialBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

    '        UGNErrorTrapping.InsertErrorLog("InsertMaterial : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "MaterialBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    '' ''*****
    ' ''* Update Material
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateMaterial(ByVal original_MaterialID As Integer, ByVal MaterialName As String, _
    '    ByVal MaterialDesc As String, ByVal PartNo As String, ByVal PartRevision As String, ByVal DrawingNo As String, _
    '    ByVal CostingVendorID As Integer, ByVal purchasedGoodID As Integer, ByVal PriceChangeDate As String, ByVal StandardCost As Decimal, _
    '    ByVal BPCSPurchasedCost As Decimal, ByVal QuoteCost As Decimal, ByVal FreightCost As Decimal, ByVal UnitID As Integer, _
    '    ByVal isCoating As Boolean, ByVal isPackaging As Boolean, ByVal Obsolete As Boolean, ByVal ddCostingVendorName As String, _
    '    ByVal ddPurchasedGoodName As String, ByVal OldMaterialGroup As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If MaterialName Is Nothing Then
    '            MaterialName = ""
    '        End If

    '        If MaterialDesc Is Nothing Then
    '            MaterialDesc = ""
    '        End If

    '        If PartNo Is Nothing Then
    '            PartNo = ""
    '        End If

    '        If PartRevision Is Nothing Then
    '            PartRevision = ""
    '        End If

    '        If DrawingNo Is Nothing Then
    '            DrawingNo = ""
    '        End If

    '        If PriceChangeDate Is Nothing Then
    '            PriceChangeDate = ""
    '        End If

    '        ''*****
    '        ' Update the record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.UpdateMaterial(original_MaterialID, MaterialName, MaterialDesc, PartNo, PartRevision, _
    '        DrawingNo, CostingVendorID, purchasedGoodID, PriceChangeDate, StandardCost, BPCSPurchasedCost, QuoteCost, FreightCost, _
    '        UnitID, isCoating, isPackaging, Obsolete, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "MaterialID:" & original_MaterialID & ", MaterialName: " & MaterialName & _
    '        ", MaterialDesc: " & MaterialDesc & ", PartNo: " & PartNo & _
    '        ", PartRevision: " & PartRevision & ", DrawingNo: " & DrawingNo & ", CostingVendorID: " & CostingVendorID & _
    '        ", purchasedGoodID: " & purchasedGoodID & ", PriceChangeDate: " & PriceChangeDate & ", StandardCost: " & StandardCost & _
    '        ", BPCSPurchasedGost: " & BPCSPurchasedCost & ", QuoteCost: " & QuoteCost & ", FreightCost: " & FreightCost & _
    '        ", UnitID: " & UnitID & ", isCoating: " & isCoating & ", isPackaging: " & isPackaging & _
    '        ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> MaterialBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

    '        UGNErrorTrapping.InsertErrorLog("UpdateMaterial : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "MaterialBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

End Class
