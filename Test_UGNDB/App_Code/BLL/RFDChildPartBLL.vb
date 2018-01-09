''******************************************************************************************************
''* RFDChildPartBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/02/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDChildPartBLL
    Private RFDChildPartAdapter As RFDChildPartTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDChildPartTableAdapter
        Get
            If RFDChildPartAdapter Is Nothing Then
                RFDChildPartAdapter = New RFDChildPartTableAdapter()
            End If
            Return RFDChildPartAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDChildPart returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDChildPart(ByVal RowID As Integer, ByVal RFDNo As Integer) As RFD.RFDChildPart_MaintDataTable

        Try

            Return Adapter.GetRFDChildPart(RowID, RFDNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New RFDChildPart
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertRFDChildPart(ByVal RFDNo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
    'ByVal ChildPartBPCSPartNo As String, ByVal ChildPartBPCSPartRevision As String) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If KitBPCSPartNo Is Nothing Then
    '            KitBPCSPartNo = ""
    '        End If

    '        If KitBPCSPartRevision Is Nothing Then
    '            KitBPCSPartRevision = ""
    '        End If

    '        If ChildPartBPCSPartNo Is Nothing Then
    '            ChildPartBPCSPartNo = ""
    '        End If

    '        If ChildPartBPCSPartRevision Is Nothing Then
    '            ChildPartBPCSPartRevision = ""
    '        End If

    '        Dim rowsAffected As Integer = Adapter.InsertRFDChildPart(RFDNo, KitBPCSPartNo, KitBPCSPartRevision, ChildPartBPCSPartNo, ChildPartBPCSPartRevision, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", ChildPartBPCSPartNo:" & ChildPartBPCSPartNo & ", ChildPartBPCSPartRevision:" & ChildPartBPCSPartRevision _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDChildPart : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

    '        UGNErrorTrapping.InsertErrorLog("InsertRFDChildPart : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ' ''*****
    ''* Update RFDChildPart
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateRFDChildPart(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer, _
    '    ByVal original_RFDNo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
    '    ByVal ChildPartBPCSPartNo As String, ByVal ChildPartBPCSPartRevision As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If KitBPCSPartNo Is Nothing Then
    '            KitBPCSPartNo = ""
    '        End If

    '        If KitBPCSPartRevision Is Nothing Then
    '            KitBPCSPartRevision = ""
    '        End If

    '        If ChildPartBPCSPartNo Is Nothing Then
    '            ChildPartBPCSPartNo = ""
    '        End If

    '        If ChildPartBPCSPartRevision Is Nothing Then
    '            ChildPartBPCSPartRevision = ""
    '        End If


    '        Dim rowsAffected As Integer = Adapter.UpdateRFDChildPart(original_RowID, RFDNo, KitBPCSPartNo, KitBPCSPartRevision, ChildPartBPCSPartNo, ChildPartBPCSPartRevision, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & RowID & ", RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", ChildPartBPCSPartNo:" & ChildPartBPCSPartNo & ", ChildPartBPCSPartRevision:" & ChildPartBPCSPartRevision _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete RFDChildPart
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDChildPart(ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDChildPart(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDChildPart(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo & ", RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDChildPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
