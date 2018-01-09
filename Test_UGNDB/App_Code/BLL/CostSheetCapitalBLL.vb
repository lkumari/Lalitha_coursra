''******************************************************************************************************
''* CostSheetCapitalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/04/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetCapitalBLL
    Private CostingCapitalAdapter As CostSheetCapitalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetCapitalTableAdapter
        Get
            If CostingCapitalAdapter Is Nothing Then
                CostingCapitalAdapter = New CostSheetCapitalTableAdapter()
            End If
            Return CostingCapitalAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetCapital returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetCapital(ByVal CostSheetID As Integer, ByVal CapitalID As Integer) As Costing.CostSheetCapital_MaintDataTable

        Try

            Return Adapter.GetCostSheetCapital(CostSheetID, CapitalID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", CapitalID: " & CapitalID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingCapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetCapital
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetCapital(ByVal CostSheetID As Integer, _
        ByVal CapitalID As Integer, ByVal TotalDollarAmount As Double, ByVal YearsOfDepreciation As Integer, _
        ByVal CapitalAnnualVolume As Integer, ByVal OverheadAmount As Double, _
        ByVal isOffline As Boolean, ByVal isInline As Boolean, ByVal Ordinal As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetCapital(CostSheetID, CapitalID, TotalDollarAmount, _
            YearsOfDepreciation, CapitalAnnualVolume, _
            OverheadAmount, isOffline, isInline, Ordinal, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & _
            ", CapitalID: " & CapitalID & ", TotalDollarAmount: " & TotalDollarAmount & _
            ", YearsOfDepreciation: " & YearsOfDepreciation & ", CapitalAnnualVolume: " & CapitalAnnualVolume & _
            ", CapitalAnnualVolume: " & CapitalAnnualVolume & _
            ", OverheadAmount: " & OverheadAmount & _
            ", isOffline: " & isOffline & ", isInline: " & isInline & ", Ordinal: " & Ordinal & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetCapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingCapitalBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetCapital(ByVal CapitalID As Integer, ByVal TotalDollarAmount As Double, _
        ByVal YearsOfDepreciation As Integer, ByVal CapitalAnnualVolume As Integer, ByVal OverheadAmount As Double, _
        ByVal StandardCostPerUnit As Double, ByVal isOffline As Boolean, ByVal isInline As Boolean, ByVal Ordinal As Integer, _
        ByVal original_RowID As Integer, ByVal CostSheetID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetCapital(original_RowID, CapitalID, TotalDollarAmount, YearsOfDepreciation, _
            CapitalAnnualVolume, OverheadAmount, StandardCostPerUnit, _
            isOffline, isInline, Ordinal, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & _
            ", CapitalID: " & CapitalID & ", TotalDollarAmount: " & TotalDollarAmount & _
            ", YearsOfDepreciation: " & YearsOfDepreciation & ", CapitalAnnualVolume: " & CapitalAnnualVolume & _
            ", CapitalAnnualVolume: " & CapitalAnnualVolume & _
            ", OverheadAmount: " & OverheadAmount & _
            ", StandardCostPerUnit: " & StandardCostPerUnit & _
            ", isOffline: " & isOffline & ", isInline: " & isInline & ", Ordinal: " & Ordinal & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetCapitalBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetCapital(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetCapital(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetCapital(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
