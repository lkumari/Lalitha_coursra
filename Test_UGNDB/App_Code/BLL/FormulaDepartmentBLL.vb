''******************************************************************************************************
''* FormulaDepartmentBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/02/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaDepartmentBLL
    Private CostingDepartmentAdapter As FormulaDepartmentTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaDepartmentTableAdapter
        Get
            If CostingDepartmentAdapter Is Nothing Then
                CostingDepartmentAdapter = New FormulaDepartmentTableAdapter()
            End If
            Return CostingDepartmentAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaDepartment returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaDepartment(ByVal FormulaID As Integer) As Costing.FormulaDepartment_MaintDataTable

        Try

            Return Adapter.GetFormulaDepartment(FormulaID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingDepartmentBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDepartmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaDepartment
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaDepartment(ByVal FormulaID As Integer, ByVal DepartmentID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaDepartment(FormulaID, DepartmentID, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", DepartmentID: " & DepartmentID _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaDepartment : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingDepartmentBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDepartmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingDepartmentBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaDepartment(ByVal RowID As Integer, ByVal FormulaID As Integer, _
        ByVal DepartmentID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaDepartment(original_RowID, FormulaID, DepartmentID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", FormulaID: " & FormulaID _
            & ", DepartmentID: " & DepartmentID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaDepartment : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingDepartmentBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateFormulaDepartment : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDepartmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete FormulaDepartmentBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteFormulaDepartment(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteFormulaDepartment(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteFormulaDepartment(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaDepartmentBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteFormulaDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaDepartmentBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
