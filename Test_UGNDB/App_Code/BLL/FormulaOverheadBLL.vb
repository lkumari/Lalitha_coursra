''******************************************************************************************************
''* FormulaOverheadBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaOverheadBLL
    Private FormulaOverheadAdapter As FormulaOverheadTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaOverheadTableAdapter
        Get
            If FormulaOverheadAdapter Is Nothing Then
                FormulaOverheadAdapter = New FormulaOverheadTableAdapter()
            End If
            Return FormulaOverheadAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaOverheadFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaOverhead(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal LaborID As Integer) As Costing.FormulaOverhead_MaintDataTable

        Try

            Return Adapter.GetFormulaOverhead(RowID, FormulaID, LaborID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", LaborID: " & LaborID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaOverhead: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaOverhead: " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaOverhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaOverhead(ByVal FormulaID As Integer, ByVal LaborID As Integer, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaOverhead(FormulaID, LaborID, Ordinal, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", LaborID: " & LaborID & _
            ", Ordinal: " & Ordinal & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaOverhead : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaOverheadBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaOverhead : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaOverhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaOverhead(ByVal original_RowID As Integer, ByVal Ordinal As Integer, _
        ByVal Obsolete As Boolean, ByVal ddOverheadDesc As String, ByVal ddLaborDesc As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaOverhead(original_RowID, Ordinal, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", Ordinal: " & Ordinal & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaOverhead : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaOverheadBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaOverhead : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
