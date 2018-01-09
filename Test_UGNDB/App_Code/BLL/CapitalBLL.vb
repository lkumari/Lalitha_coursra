''******************************************************************************************************
''* CapitalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/04/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CapitalBLL
    Private CapitalAdapter As CapitalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CapitalTableAdapter
        Get
            If CapitalAdapter Is Nothing Then
                CapitalAdapter = New CapitalTableAdapter()
            End If
            Return CapitalAdapter
        End Get
    End Property
    ''*****
    ''* Select Capital returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCapital(ByVal CapitalID As Integer, ByVal CapitalDesc As String) As Costing.Capital_MaintDataTable

        Try

            If CapitalDesc Is Nothing Then
                CapitalDesc = ""
            End If

            Return Adapter.GetCapital(CapitalID, CapitalDesc)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalID: " & CapitalID & ",CapitalDesc: " & CapitalDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Capital
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCapital(ByVal CapitalDesc As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCapital(CapitalDesc, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalDesc:" & CapitalDesc & ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Capital
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCapital(ByVal CapitalDesc As String, ByVal original_CapitalID As Integer, ByVal CapitalID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If CapitalDesc Is Nothing Then
                CapitalDesc = "unknown"
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCapital(original_CapitalID, CapitalDesc, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalID:" & original_CapitalID & ", CapitalDesc: " & CapitalDesc & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CapitalBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CapitalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ' ''* Delete Capital
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    '    Public Function DeleteCapital(ByVal CapitalID As Integer, ByVal original_CapitalID As Integer) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        ''*****
    '        ' Update the Capital record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.DeleteCapital(original_CapitalID, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "CapitalID:" & original_CapitalID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "DeleteCapital : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CapitalBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("DeleteCapital : " & commonFunctions.convertSpecialChar(ex.Message, False), "CapitalBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
End Class
