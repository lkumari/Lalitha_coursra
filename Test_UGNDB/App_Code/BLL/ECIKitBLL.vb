''******************************************************************************************************
''* ECIKitBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/01/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIKitBLL
    Private ECIKitAdapter As ECIKitTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIKitTableAdapter
        Get
            If ECIKitAdapter Is Nothing Then
                ECIKitAdapter = New ECIKitTableAdapter()
            End If
            Return ECIKitAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIKit returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIKit(ByVal ECINo As Integer) As ECI.ECIKit_MaintDataTable

        Try

            Return Adapter.GetECIKit(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECIKit
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECIKit(ByVal ECINo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
    ByVal FinishedGoodBPCSPartNo As String, ByVal FinishedGoodBPCSPartRevision As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If KitBPCSPartNo Is Nothing Then
                KitBPCSPartNo = ""
            End If

            If KitBPCSPartRevision Is Nothing Then
                KitBPCSPartRevision = ""
            End If

            If FinishedGoodBPCSPartNo Is Nothing Then
                FinishedGoodBPCSPartNo = ""
            End If

            If FinishedGoodBPCSPartRevision Is Nothing Then
                FinishedGoodBPCSPartRevision = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertECIKit(ECINo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo:" & ECINo _
            & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
            & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIKit : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIKitBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertECIKit : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "ECIKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update ECIKit
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateECIKit(ByVal RowID As Integer, ByVal ECINo As Integer, ByVal original_RowID As Integer, _
    '    ByVal original_ECINo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
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


    '        Dim rowsAffected As Integer = Adapter.UpdateECIKit(original_RowID, ECINo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & RowID & ", ECINo:" & ECINo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateECIKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIKitBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateECIKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIKitBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete ECIKit
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteECIKit(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECIKit(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECIKit: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECIKit: " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
