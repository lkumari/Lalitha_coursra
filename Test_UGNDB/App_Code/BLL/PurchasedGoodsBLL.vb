''******************************************************************************************************
''* PurchasedGoodsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 09/10/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports PurchasedGoodsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class PurchasedGoodsBLL
    Private PurchasedGoodsAdapter As PurchasedGoodTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As PurchasedGoodsTableAdapters.PurchasedGoodTableAdapter
        Get
            If PurchasedGoodsAdapter Is Nothing Then
                PurchasedGoodsAdapter = New PurchasedGoodTableAdapter()
            End If
            Return PurchasedGoodsAdapter
        End Get
    End Property
    ''*****
    ''* Select PurchasedGoods returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPurchasedGoods(ByVal PurchasedGoodName As String) As PurchasedGoods.PurchasedGood_MaintDataTable

        Try
            If PurchasedGoodName Is Nothing Then PurchasedGoodName = ""

            Return Adapter.GetPurchasedGoods(PurchasedGoodName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PurchasedGoodName: " & PurchasedGoodName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertPurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PurchasedGoodsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PurchasedGoodMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "PurchasedGoodsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New PurchasedGood
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPurchasedGood(ByVal PurchasedGoodName As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the PurchasedGood record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertPurchasedGood(PurchasedGoodName, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PurchasedGoodName: " & PurchasedGoodName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertPurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PurchasedGoodsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PurchasedGoodMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "PurchasedGoodsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update PurchasedGood
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdatePurchasedGood(ByVal PurchasedGoodName As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_PurchasedGoodID As Integer) As Boolean

        Try

            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the PurchasedGood record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdatePurchasedGood(original_PurchasedGoodID, PurchasedGoodName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PurchasedGoodName: " & PurchasedGoodName & ", Obsolete: " & Obsolete.ToString & ", Original_PurchasedGoodID:" & original_PurchasedGoodID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdatePurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PurchasedGoodsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PurchasedGoodMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "PurchasedGoodsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
