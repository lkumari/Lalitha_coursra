''******************************************************************************************************
''* RFDCustomerProgramBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDCustomerProgramBLL
    Private RFDCustomerProgramAdapter As RFDCustomerProgramTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDCustomerProgramTableAdapter
        Get
            If RFDCustomerProgramAdapter Is Nothing Then
                RFDCustomerProgramAdapter = New RFDCustomerProgramTableAdapter()
            End If
            Return RFDCustomerProgramAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDCustomerProgram returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDCustomerProgram(ByVal RFDNo As Integer) As RFD.RFDCustomerProgram_MaintDataTable

        Try

            Return Adapter.GetRFDCustomerProgram(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New RFDCustomerProgram
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertRFDCustomerProgram(ByVal RFDNo As Integer, ByVal KitBPCSPartNo As String, ByVal KitBPCSPartRevision As String, _
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

    '        Dim rowsAffected As Integer = Adapter.InsertRFDCustomerProgram(RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertRFDCustomerProgram : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDCustomerProgramBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

    '        UGNErrorTrapping.InsertErrorLog("InsertRFDCustomerProgram : " _
    '        & commonFunctions.convertSpecialChar(ex.Message, False), "RFDCustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ' ''*****
    ''* Update RFDCustomerProgram
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateRFDCustomerProgram(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer, _
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


    '        Dim rowsAffected As Integer = Adapter.UpdateRFDCustomerProgram(original_RowID, RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & RowID & ", RFDNo:" & RFDNo _
    '        & ", KitBPCSPartNo:" & KitBPCSPartNo & ", KitBPCSPartRevision:" & KitBPCSPartRevision _
    '        & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDCustomerProgramBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDCustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete RFDCustomerProgram
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDCustomerProgram(ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDCustomerProgram(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDCustomerProgram(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
