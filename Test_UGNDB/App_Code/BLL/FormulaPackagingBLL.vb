''******************************************************************************************************
''* FormulaPackagingBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaPackagingBLL
    Private FormulaPackagingAdapter As FormulaPackagingTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaPackagingTableAdapter
        Get
            If FormulaPackagingAdapter Is Nothing Then
                FormulaPackagingAdapter = New FormulaPackagingTableAdapter()
            End If
            Return FormulaPackagingAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaPackagingFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaPackaging(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal MaterialID As Integer) As Costing.FormulaPackaging_MaintDataTable

        Try

            Return Adapter.GetFormulaPackaging(RowID, FormulaID, MaterialID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", MaterialID: " & MaterialID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaPackaging: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaPackagingFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaPackaging: " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaPackagingFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaPackaging
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaPackaging(ByVal FormulaID As Integer, ByVal MaterialID As Integer, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaPackaging(FormulaID, MaterialID, Ordinal, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", MaterialID: " & MaterialID & _
            ", Ordinal: " & Ordinal & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaPackaging : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaPackagingBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaPackaging : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaPackaging
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaPackaging(ByVal original_RowID As Integer, ByVal Ordinal As Integer, _
        ByVal Obsolete As Boolean, ByVal ddMaterialName As String, ByVal ddMaterialNameCombo As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaPackaging(original_RowID, Ordinal, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", Ordinal: " & Ordinal & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaPackaging : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaPackagingrBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaPackaging : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
