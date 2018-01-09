''******************************************************************************************************
''* FormulaMaterialBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaMaterialBLL
    Private FormulaMaterialAdapter As FormulaMaterialTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaMaterialTableAdapter
        Get
            If FormulaMaterialAdapter Is Nothing Then
                FormulaMaterialAdapter = New FormulaMaterialTableAdapter()
            End If
            Return FormulaMaterialAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaMaterialProfileFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaMaterial(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal MaterialID As Integer) As Costing.FormulaMaterial_MaintDataTable

        Try

            Return Adapter.GetFormulaMaterial(RowID, FormulaID, MaterialID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", MaterialID: " & MaterialID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMaterialBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMaterialBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaMaterialProfile
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaMaterial(ByVal FormulaID As Integer, ByVal MaterialID As Integer, ByVal UsageFactor As Double, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaMaterial(FormulaID, MaterialID, UsageFactor, Ordinal, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", MaterialID: " & MaterialID & _
            ", UsageFactor: " & UsageFactor & ", Ordinal: " & Ordinal & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMaterialBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaMaterialProfile : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMaterialBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaMaterial
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaMaterial(ByVal original_RowID As Integer, ByVal UsageFactor As Double, _
        ByVal Ordinal As Integer, ByVal Obsolete As Boolean, ByVal ddMaterialName As String, ByVal ddMaterialNameCombo As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaMaterial(original_RowID, UsageFactor, Ordinal, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & _
            ", UsageFactor: " & UsageFactor & ", Ordinal: " & Ordinal & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaMaterialBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaMaterialProfile : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaMaterialBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
