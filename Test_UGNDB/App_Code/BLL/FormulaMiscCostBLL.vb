''******************************************************************************************************
''* FormulaMiscCostBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/11/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaMiscCostBLL
    Private FormulaMiscCostAdapter As FormulaMiscCostTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaMiscCostTableAdapter
        Get
            If FormulaMiscCostAdapter Is Nothing Then
                FormulaMiscCostAdapter = New FormulaMiscCostTableAdapter()
            End If
            Return FormulaMiscCostAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaMiscCost returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaMiscCost(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal MiscCostID As Integer) As Costing.FormulaMiscCost_MaintDataTable

        Try

            Return Adapter.GetFormulaMiscCost(RowID, FormulaID, MiscCostID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", MiscCostID: " & MiscCostID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaCostType: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMiscCostBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaMiscCost: " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaMiscCost
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaMiscCost(ByVal FormulaID As Integer, ByVal MiscCostID As Integer, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaMiscCost(FormulaID, MiscCostID, Ordinal, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", MiscCostID: " & MiscCostID & _
            ", Ordinal: " & Ordinal & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaMiscCost : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMiscCostBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaCostType : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaMiscCost
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaMiscCost(ByVal original_RowID As Integer, ByVal Ordinal As Integer, ByVal Obsolete As Boolean, ByVal ddMiscCostDesc As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaMiscCost(original_RowID, Ordinal, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", Ordinal: " & Ordinal & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaMiscCost : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMiscCostBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaCostType : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMiscCostBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
