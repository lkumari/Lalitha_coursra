''******************************************************************************************************
''* RFDFinishedGoodBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDFinishedGoodBLL
    Private RFDFinishedGoodAdapter As RFDFinishedGoodTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDFinishedGoodTableAdapter
        Get
            If RFDFinishedGoodAdapter Is Nothing Then
                RFDFinishedGoodAdapter = New RFDFinishedGoodTableAdapter()
            End If
            Return RFDFinishedGoodAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDFinishedGood returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDFinishedGood(ByVal RFDNo As Integer) As RFD.RFDFinishedGood_MaintDataTable

        Try

            Return Adapter.GetRFDFinishedGood(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFinishedGoodBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFinishedGoodBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New RFDFinishedGood
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertRFDFinishedGood(ByVal RFDNo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
    'ByVal FinishedGoodBPCSPartNo As String, ByVal FinishedGoodBPCSPartRevision As String) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If KitBPCSPartNo Is Nothing Then
    '            KitBPCSPartNo = ""
    '        End If

    '        If KitBPCSPartRevision Is Nothing Then
    '            KitBPCSPartRevision = ""
    '        End If

    '        If FinishedGoodBPCSPartNo Is Nothing Then
    '            FinishedGoodBPCSPartNo = ""
    '        End If

    '        If FinishedGoodBPCSPartRevision Is Nothing Then
    '            FinishedGoodBPCSPartRevision = ""
    '        End If

    '        Dim rowsAffected As Integer = Adapter.InsertRFDFinishedGood(RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDFinishedGood : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFinishedGoodBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

    '        UGNErrorTrapping.InsertErrorLog("InsertRFDFinishedGood : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFinishedGoodBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ' ''*****
    ''* Update RFDFinishedGood
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateRFDFinishedGood(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer, _
    '    ByVal original_RFDNo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
    '    ByVal FinishedGoodBPCSPartNo As String, ByVal FinishedGoodBPCSPartRevision As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If KitBPCSPartNo Is Nothing Then
    '            KitBPCSPartNo = ""
    '        End If

    '        If KitBPCSPartRevision Is Nothing Then
    '            KitBPCSPartRevision = ""
    '        End If

    '        If FinishedGoodBPCSPartNo Is Nothing Then
    '            FinishedGoodBPCSPartNo = ""
    '        End If

    '        If FinishedGoodBPCSPartRevision Is Nothing Then
    '            FinishedGoodBPCSPartRevision = ""
    '        End If


    '        Dim rowsAffected As Integer = Adapter.UpdateRFDFinishedGood(original_RowID, RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & RowID & ", RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFinishedGoodBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFinishedGoodBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete RFDFinishedGood
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDFinishedGood(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDFinishedGood(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDFinishedGood(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo & ", RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFinishedGoodBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFinishedGoodBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
