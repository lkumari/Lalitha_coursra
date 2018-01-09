''******************************************************************************************************
''* FormulaDeplugFactorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaDeplugFactorBLL
    Private FormulaDeplugFactorAdapter As FormulaDeplugFactorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaDeplugFactorTableAdapter
        Get
            If FormulaDeplugFactorAdapter Is Nothing Then
                FormulaDeplugFactorAdapter = New FormulaDeplugFactorTableAdapter()
            End If
            Return FormulaDeplugFactorAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaDeplugFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaDeplugFactor(ByVal FactorID As Integer, ByVal FormulaID As Integer) As Costing.FormulaDeplugFactor_MaintDataTable

        Try

            Return Adapter.GetFormulaDeplugFactor(FactorID, FormulaID, 0)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ",FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaDeplugFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaDeplugFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDeplugFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaDeplugFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaDeplugFactor(ByVal FormulaID As Integer, ByVal MinimumFactor As Double, ByVal MaximumFactor As Double, _
        ByVal DeplugFactor As Double, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaDeplugFactor(FormulaID, MinimumFactor, MaximumFactor, DeplugFactor, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", MinimumFactor: " & MinimumFactor & _
            ", MaximumFactor: " & MaximumFactor & ", DeplugFactor: " & DeplugFactor & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaDeplugFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaDeplugFactorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaDeplugFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDeplugFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaDeplugFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaDeplugFactor(ByVal original_FactorID As Integer, ByVal FormulaID As Integer, _
        ByVal MinimumFactor As Double, ByVal MaximumFactor As Double, ByVal DeplugFactor As Double, _
        ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaDeplugFactor(original_FactorID, FormulaID, MinimumFactor, MaximumFactor, DeplugFactor, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID:" & original_FactorID & ", FormulaID: " & FormulaID & _
            ", MinimumFactor: " & MinimumFactor & ", MaximumFactor: " & MaximumFactor & _
            ", DeplugFactor: " & DeplugFactor & ", Obsolete: " & Obsolete & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaDeplugFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaDeplugFactorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaDeplugFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDeplugFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
