''******************************************************************************************************
''* CostSheetMiscCostBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/30/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetMiscCostBLL
    Private CostingMiscCostAdapter As CostSheetMiscCostTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetMiscCostTableAdapter
        Get
            If CostingMiscCostAdapter Is Nothing Then
                CostingMiscCostAdapter = New CostSheetMiscCostTableAdapter()
            End If
            Return CostingMiscCostAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetMiscCost returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetMiscCost(ByVal CostSheetID As Integer, ByVal MiscCostID As Integer) As Costing.CostSheetMiscCost_MaintDataTable

        Try

            Return Adapter.GetCostSheetMiscCost(CostSheetID, MiscCostID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", MiscCostID: " & MiscCostID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetMiscCost
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetMiscCost(ByVal CostSheetID As Integer, ByVal MiscCostID As Integer, ByVal Rate As Double, _
        ByVal QuoteRate As Double, ByVal Cost As Double, ByVal AmortVolume As Integer, ByVal isPiecesPerHour As Boolean, _
        ByVal isPiecesPerYear As Boolean, ByVal isPiecesPerContainer As Boolean, ByVal Ordinal As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetMiscCost(CostSheetID, MiscCostID, Rate, QuoteRate, _
            Cost, AmortVolume, isPiecesPerHour, isPiecesPerYear, isPiecesPerContainer, Ordinal, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & ", MiscCostID: " & MiscCostID & _
            ", Rate: " & Rate & ", QuoteRate: " & QuoteRate & ", Cost: " & Cost & ", AmortVolume: " & AmortVolume _
            & ", isPiecesPerHour: " & isPiecesPerHour & ", isPiecesPerYear: " & isPiecesPerYear _
            & ", isPiecesPerContainer: " & isPiecesPerContainer _
            & ", Ordinal: " & Ordinal & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingMiscCostBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetMiscCost(ByVal MiscCostID As Integer, _
        ByVal Rate As Double, ByVal QuoteRate As Double, ByVal Cost As Double, ByVal AmortVolume As Integer, _
        ByVal StandardCostPerUnit As Double, ByVal isPiecesPerHour As Boolean, _
        ByVal isPiecesPerYear As Boolean, ByVal isPiecesPerContainer As Boolean, _
        ByVal Ordinal As Integer, ByVal original_RowID As Integer, _
        ByVal ddMiscCostDesc As String, ByVal CostSheetID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetMiscCost(original_RowID, MiscCostID, Rate, QuoteRate, Cost, AmortVolume, _
            StandardCostPerUnit, isPiecesPerHour, isPiecesPerYear, isPiecesPerContainer, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", MiscCostID: " & _
            MiscCostID & ", Rate: " & Rate & ", QuoteRate: " & QuoteRate & ", Cost: " & Cost _
            & ", AmortVolume: " & AmortVolume & ", isPiecesPerHour: " & isPiecesPerHour _
            & ", isPiecesPerYear: " & isPiecesPerYear & ", isPiecesPerContainer: " & isPiecesPerContainer _
            & ", Ordinal: " & Ordinal _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetMiscCost : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingMiscCostBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetMiscCost : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetMiscCostBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetMiscCost(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Obsolete the record
            ''*****
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetMiscCost(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetMiscCost(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetMiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
