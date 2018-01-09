''******************************************************************************************************
''* OverheadBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/04/2009
''* Modified: {Name} {Date} - {Notes}
''*           Roderick Carlson 05/18/2010 - added Variable Rate
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class OverheadBLL
    Private OverheadAdapter As OverheadTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.OverheadTableAdapter
        Get
            If OverheadAdapter Is Nothing Then
                OverheadAdapter = New OverheadTableAdapter()
            End If
            Return OverheadAdapter
        End Get
    End Property
    ''*****
    ''* Select Overhead returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetOverhead(ByVal LaborID As Integer, ByVal LaborDesc As String) As Costing.Overhead_MaintDataTable

        Try

            If LaborDesc Is Nothing Then
                LaborDesc = ""
            End If

            Return Adapter.GetOverhead(LaborID, LaborDesc)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID: " & LaborID & ",LaborDesc: " & LaborDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> OverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "OverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Overhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertOverhead(ByVal LaborID As Integer, ByVal Rate As Double, ByVal VariableRate As Double, ByVal CrewSize As Double, ByVal isOffline As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertOverhead(LaborID, Rate, VariableRate, CrewSize, isOffline, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID: " & LaborID & _
            ", Rate: " & Rate & ", VariableRate: " & VariableRate & _
            ", CrewSize: " & CrewSize & ", isOffline: " & isOffline & _
            ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> OverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "OverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Overhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateOverhead(ByVal LaborID As Integer, ByVal original_LaborID As Integer, ByVal Rate As Double, _
        ByVal VariableRate As Double, ByVal CrewSize As Double, ByVal isOffline As Boolean, ByVal Obsolete As Boolean, ByVal RowID As Integer, _
        ByVal ddLaborDesc As String, ByVal original_RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateOverhead(original_RowID, LaborID, Rate, VariableRate, CrewSize, isOffline, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", LaborID:" & original_LaborID _
            & ", Rate: " & Rate & ", VariableRate: " & VariableRate _
            & ", CrewSize: " & CrewSize & ", isOffline: " & isOffline _
            & ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> OverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "OverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
   
End Class
