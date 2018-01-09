''******************************************************************************************************
''* CostSheetPriceMarginBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 05/27/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetPriceMarginBLL
    Private CostSheetPriceMarginAdapter As CostSheetPriceMarginTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetPriceMarginTableAdapter
        Get
            If CostSheetPriceMarginAdapter Is Nothing Then
                CostSheetPriceMarginAdapter = New CostSheetPriceMarginTableAdapter()
            End If
            Return CostSheetPriceMarginAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetPriceMargin returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetPriceMarginList(ByVal UGNFacility As String) As Costing.CostSheetPriceMargin_MaintDataTable

        Try

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            Return Adapter.GetCostSheetPriceMarginList(UGNFacility)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPriceMargin : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPriceMarginBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetPriceMargin : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPriceMarginBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetPriceMargin
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetPriceMargin(ByVal UGNFacility As String, ByVal MinPriceMargin As Double) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetPriceMargin(UGNFacility, MinPriceMargin, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility:" & UGNFacility _
            & ", MinPriceMargin: " & MinPriceMargin _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPriceMargin : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPriceMarginBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPriceMargin : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPriceMarginBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update CostSheetPriceMargin
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetPriceMargin(ByVal RowID As Integer, ByVal UGNFacility As String, ByVal MinPriceMargin As Double, _
        ByVal EffectiveDate As String, ByVal original_RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            If EffectiveDate Is Nothing Then
                EffectiveDate = Today.Date
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetPriceMargin(original_RowID, UGNFacility, _
            MinPriceMargin, EffectiveDate, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UGNFacility: " & UGNFacility _
            & ", MinPriceMargin: " & MinPriceMargin _
            & ", EffectiveDate: " & EffectiveDate _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPriceMargin : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPriceMarginBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPriceMargin : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPriceMarginBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
