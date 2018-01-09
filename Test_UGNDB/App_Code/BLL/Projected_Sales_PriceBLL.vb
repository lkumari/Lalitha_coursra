''******************************************************************************************************
''* Projected_Sales_PriceBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Sales_Projection.aspx - gvPrice
''* Author  : LRey 04/01/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports Projected_SalesTableAdapters
<System.ComponentModel.DataObject()> _
Public Class Projected_Sales_PriceBLL
    Private psAdapter As Projected_Sales_Price_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As Projected_SalesTableAdapters.Projected_Sales_Price_TableAdapter
        Get
            If psAdapter Is Nothing Then
                psAdapter = New Projected_Sales_Price_TableAdapter()
            End If
            Return psAdapter
        End Get
    End Property

    ''*****
    ''* Select Projected_Sales_Price returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetProjectedSalesPrice(ByVal PartNo As String) As Projected_Sales.Projected_Sales_PriceDataTable
        Return Adapter.Get_Price(PartNo)
    End Function

    ''*****
    ''* Insert a New row to Projected_Sales_Price table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProjectedSalesPrice(ByVal PartNo As String, ByVal Price As Decimal, ByVal EffDate As String, ByVal CostDown As Decimal) As Boolean

        ' Create a new pscpRow instance
        Dim psTable As New Projected_Sales.Projected_Sales_PriceDataTable
        Dim psRow As Projected_Sales.Projected_Sales_PriceRow = psTable.NewProjected_Sales_PriceRow
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without null columns
        If PartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
            Throw New ApplicationException("Insert Cancelled: PartNo is a required field.")
        End If
        'If Price = Nothing And (Price <> 0) Then 
        '    Throw New ApplicationException("Insert Cancelled: Price is a required field.")
        'End If
        If EffDate = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Effective Date is a required field.")
        End If

        ' Insert the new Projected_Sales_Price row
        Dim rowsAffected As Integer = Adapter.sp_Insert_Projected_Sales_Price(HttpContext.Current.Request.QueryString("sPartNo"), Price, EffDate, CostDown, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function

    ''*****
    ''* Update Projected_Sales_Price
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateProjectedSalesPrice(ByVal PartNo As String, ByVal Price As Decimal, ByVal EffDate As String, ByVal CostDown As Decimal, ByVal original_EffDate As String) As Boolean

        Dim psTable As Projected_Sales.Projected_Sales_PriceDataTable = Adapter.Get_Price(PartNo)
        Dim psRow As Projected_Sales.Projected_Sales_PriceRow = psTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        If psTable.Count = 0 Then
            ' no matching record found, return false
            Return False
        End If

        ' Logical Rule - Cannot update a record without null columns
        If PartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
            Throw New ApplicationException("Update Cancelled: PartNo is a required field.")
        End If
        'If Price = Nothing Then
        '    Throw New ApplicationException("Update Cancelled: Price is a required field.")
        'End If
        If EffDate = Nothing Then
            Throw New ApplicationException("Update Cancelled: Effective Date is a required field.")
        End If

        ' Update the Projected_Sales_Price record
        Dim rowsAffected As Integer = Adapter.sp_Update_Projected_Sales_Price(HttpContext.Current.Request.QueryString("sPartNo"), Price, EffDate, CostDown, original_EffDate, User)

        ' Return true if precisely one row was updated, otherwise false
        Return rowsAffected = 1
    End Function

    ''*****
    ''* Delete Projected_Sales_Price
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteProjectedSalesPrice(ByVal PartNo As String, ByVal EffDate As String, ByVal original_EffDate As String) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Projected_Sales_Price(PartNo, original_EffDate)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class
