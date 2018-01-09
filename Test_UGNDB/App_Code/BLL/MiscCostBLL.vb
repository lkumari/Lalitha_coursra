''******************************************************************************************************
''* MiscCostBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/05/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class MiscCostBLL
    Private MiscCostAdapter As MiscCostTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.MiscCostTableAdapter
        Get
            If MiscCostAdapter Is Nothing Then
                MiscCostAdapter = New MiscCostTableAdapter()
            End If
            Return MiscCostAdapter
        End Get
    End Property
    ''*****
    ''* Select MiscCost returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetMiscCost(ByVal MiscCostID As Integer, ByVal MiscCostDesc As String) As Costing.MiscCost_MaintDataTable

        Try

            If MiscCostDesc Is Nothing Then
                MiscCostDesc = ""
            End If

            Return Adapter.GetMiscCost(MiscCostID, MiscCostDesc)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MiscCostID: " & MiscCostID & ", MiscCostDesc: " & MiscCostDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "MiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New MiscCost
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertMiscCost(ByVal MiscCostDesc As String, ByVal Rate As Double, ByVal QuoteRate As Double, ByVal isRatePercentage As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertMiscCost(MiscCostDesc, Rate, QuoteRate, isRatePercentage, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MiscCostDesc:" & MiscCostDesc & ", Rate:" & Rate & _
            ", Quote Rate:" & QuoteRate & ", isRatePercentage: " & isRatePercentage & ", Obsolete: " & Obsolete _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "MiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update MiscCost
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateMiscCost(ByVal MiscCostDesc As String, ByVal original_MiscCostID As Integer, ByVal MiscCostID As Integer, ByVal Rate As Double, ByVal QuoteRate As Double, ByVal isRatePercentage As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If MiscCostDesc Is Nothing Then
                MiscCostDesc = "unknown"
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateMiscCost(original_MiscCostID, MiscCostDesc, Rate, QuoteRate, isRatePercentage, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MiscCostID:" & original_MiscCostID & ", MiscCostDesc: " & MiscCostDesc _
            & ", Rate: " & Rate & ", QuoteRate: " & QuoteRate & ", isRatePercentage: " & isRatePercentage & ", Obsolete: " & Obsolete _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateMiscCost : " & commonFunctions.convertSpecialChar(ex.Message, False), "MiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
