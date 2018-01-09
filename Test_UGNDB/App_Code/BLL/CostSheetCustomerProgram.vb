''******************************************************************************************************
''* CostSheetCustomerProgramBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 08/14/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetCustomerProgramBLL
    Private CostSheetMaterialsAdapter As CostSheetCustomerProgramTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetCustomerProgramTableAdapter
        Get
            If CostSheetMaterialsAdapter Is Nothing Then
                CostSheetMaterialsAdapter = New CostSheetCustomerProgramTableAdapter()
            End If
            Return CostSheetMaterialsAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetCustomerProgram returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetCustomerProgram(ByVal CostSheetID As Integer) As Costing.CostSheetCustomerProgram_MaintDataTable

        Try

            Return Adapter.GetCostSheetCustomerProgram(CostSheetID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetCustomerProgram : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCustomerProgramBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New CostSheetCustomerProgram
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertCostSheetCustomerProgram(ByVal CostSheetID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        ''*****
    '        ' Insert the record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.InsertCostSheetCustomerProgram(CostSheetID, CABBV, SoldTo, ProgramID, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & ", CABBV: " & CABBV & ", SoldTo: " & SoldTo _
    '        & ", ProgramID: " & ProgramID _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertCostingCustomerProgram : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCustomerProgramBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertCostingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    '' ''*****
    ' ''* Update CostSheetCustomerProgram
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateCostSheetCustomerProgram(ByVal Rate As Decimal, ByVal CrewSize As Decimal, ByVal Ordinal As Integer, _
    '    ByVal isOffline As Boolean, ByVal StandardCostPerUnit As Decimal, ByVal original_RowID As Integer, _
    '    ByVal CustomerProgramID As Integer, ByVal ddCustomerProgramDesc As String, ByVal CostSheetID As Integer) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        ''*****
    '        ' Update the record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.UpdateCostSheetCustomerProgram(original_RowID, CustomerProgramID, Rate, CrewSize, StandardCostPerUnit, isOffline, Ordinal, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & original_RowID & ", CustomerProgramID: " & CustomerProgramID _
    '        & ", Rate: " & Rate & ", CrewSize: " & CrewSize & ", isOffline: " & isOffline _
    '        & ", StandardCostPerUnit: " & StandardCostPerUnit & ", Ordinal: " & Ordinal _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateCostingCustomerProgram : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCustomerProgramBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateCostingCustomerProgram : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ''*****
    ''* Delete CostSheetCustomerProgram
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetCustomerProgram(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetCustomerProgram(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetCustomerProgram(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
