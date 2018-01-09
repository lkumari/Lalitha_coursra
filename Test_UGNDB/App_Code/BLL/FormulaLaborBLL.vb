''******************************************************************************************************
''* FormulaLaborBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaLaborBLL
    Private FormulaLaborAdapter As FormulaLaborTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaLaborTableAdapter
        Get
            If FormulaLaborAdapter Is Nothing Then
                FormulaLaborAdapter = New FormulaLaborTableAdapter()
            End If
            Return FormulaLaborAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaLaborFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaLabor(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal LaborID As Integer) As Costing.FormulaLabor_MaintDataTable

        Try

            Return Adapter.GetFormulaLabor(RowID, FormulaID, LaborID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", LaborID: " & LaborID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaLabor: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaLaborFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaLabor: " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaLaborFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaLabor(ByVal FormulaID As Integer, ByVal LaborID As Integer, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaLabor(FormulaID, LaborID, Ordinal, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", LaborID: " & LaborID & _
            ", Ordinal: " & Ordinal & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaLaborBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaLabor(ByVal original_RowID As Integer, ByVal Ordinal As Integer, ByVal Obsolete As Boolean, ByVal ddLaborDesc As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaLabor(original_RowID, Ordinal, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", Ordinal: " & Ordinal & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaLaborrBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
