''******************************************************************************************************
''* FormulaCoatingFactorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FormulaCoatingFactorBLL
    Private FormulaCoatingFactorAdapter As FormulaCoatingFactorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.FormulaCoatingFactorTableAdapter
        Get
            If FormulaCoatingFactorAdapter Is Nothing Then
                FormulaCoatingFactorAdapter = New FormulaCoatingFactorTableAdapter()
            End If
            Return FormulaCoatingFactorAdapter
        End Get
    End Property
    ''*****
    ''* Select FormulaCoatingFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFormulaCoatingFactor(ByVal FactorID As Integer, ByVal FormulaID As Integer) As Costing.FormulaCoatingFactor_MaintDataTable

        Try

            Return Adapter.GetFormulaCoatingFactor(FactorID, FormulaID, 0)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ",FormulaID: " & FormulaID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaCoatingFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFormulaCoatingFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "FormulaCoatingFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New FormulaCoatingFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFormulaCoatingFactor(ByVal FormulaID As Integer, ByVal MinimumFactor As Double, ByVal MaximumFactor As Double, _
        ByVal CoatingFactor As Double, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertFormulaCoatingFactor(FormulaID, MinimumFactor, MaximumFactor, CoatingFactor, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormulaID:" & FormulaID & ", MinimumFactor: " & MinimumFactor & _
            ", MaximumFactor: " & MaximumFactor & ", CoatingFactor: " & CoatingFactor & ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFormulaCoatingFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaCoatingFactorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertFormulaCoatingFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaCoatingFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update FormulaCoatingFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFormulaCoatingFactor(ByVal original_FactorID As Integer, ByVal FormulaID As Integer, _
        ByVal MinimumFactor As Double, ByVal MaximumFactor As Double, ByVal CoatingFactor As Double, _
        ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFormulaCoatingFactor(original_FactorID, FormulaID, MinimumFactor, MaximumFactor, CoatingFactor, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID:" & original_FactorID & ", FormulaID: " & FormulaID & _
            ", MinimumFactor: " & MinimumFactor & ", MaximumFactor: " & MaximumFactor & _
            ", CoatingFactor: " & CoatingFactor & ", Obsolete: " & Obsolete & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFormulaCoatingFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FormulaCoatingFactorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateFormulaCoatingFactor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "FormulaCoatingFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
   
End Class
