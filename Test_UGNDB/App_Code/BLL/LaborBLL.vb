''******************************************************************************************************
''* LaborBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/09/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class LaborBLL
    Private LaborAdapter As LaborTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.LaborTableAdapter
        Get
            If LaborAdapter Is Nothing Then
                LaborAdapter = New LaborTableAdapter()
            End If
            Return LaborAdapter
        End Get
    End Property
    ''*****
    ''* Select Labor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetLabor(ByVal LaborID As Integer, ByVal LaborDesc As String, ByVal filterOffline As Boolean, ByVal isOffline As Boolean) As Costing.Labor_MaintDataTable

        Try

            If LaborDesc Is Nothing Then
                LaborDesc = ""
            End If

            Return Adapter.GetLabor(LaborID, LaborDesc, filterOffline, isOffline)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID: " & LaborID & ", LaborDesc: " & LaborDesc _
            & ", filterOffline: " & filterOffline & ", isOffline: " & isOffline _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetLabor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> LaborBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "LaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Labor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertLabor(ByVal LaborDesc As String, ByVal Rate As Double, ByVal CrewSize As Double, ByVal isOffline As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If LaborDesc Is Nothing Then
                LaborDesc = "unknown"
            End If

            ''*****
            ' Insert the CostingMaterialsBLL record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertLabor(LaborDesc, Rate, CrewSize, isOffline, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborDesc: " & LaborDesc & _
            ", Rate: " & Rate & ", CrewSize: " & CrewSize & ", isOffline: " & isOffline & _
            ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> LaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "LaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Labor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateLabor(ByVal LaborID As Integer, ByVal original_LaborID As Integer, ByVal LaborDesc As String, _
        ByVal Rate As Double, ByVal CrewSize As Double, ByVal isOffline As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If LaborDesc Is Nothing Then
                LaborDesc = "unknown"
            End If

            ''*****
            ' Update the CostingMaterialsBLL record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateLabor(original_LaborID, LaborDesc, Rate, CrewSize, isOffline, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "LaborID:" & original_LaborID & ", LaborDesc: " & LaborDesc & _
            ", Rate: " & Rate & ", CrewSize: " & CrewSize & ", isOffline: " & isOffline & _
            ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> LaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "LaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ' ''* Delete Labor
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    '    Public Function DeleteLabor(ByVal LaborID As Integer, ByVal original_LaborID As Integer) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        ''*****
    '        ' Update the Labor record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.DeleteLabor(original_LaborID, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "LaborID:" & original_LaborID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "DeleteLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> LaborBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("DeleteLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "LaborBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
End Class
