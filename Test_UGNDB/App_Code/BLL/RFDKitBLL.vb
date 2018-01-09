''******************************************************************************************************
''* RFDKitBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDKitBLL
    Private RFDKitAdapter As RFDKitTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDKitTableAdapter
        Get
            If RFDKitAdapter Is Nothing Then
                RFDKitAdapter = New RFDKitTableAdapter()
            End If
            Return RFDKitAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDKit returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDKit(ByVal RFDNo As Integer) As RFD.RFDKit_MaintDataTable

        Try

            Return Adapter.GetRFDKit(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New RFDKit
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRFDKit(ByVal RFDNo As Integer, ByVal KitBPCSPartNo As String, _
            ByVal KitBPCSPartRevision As String, ByVal FinishedGoodBPCSPartNo As String, ByVal FinishedGoodBPCSPartRevision As String) As Boolean

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

            Dim rowsAffected As Integer = Adapter.InsertRFDKit(RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", KitBPCSPartNo:" & KitBPCSPartNo _
            & ", KitBPCSRevision:" & KitBPCSPartRevision _
            & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo _
            & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update RFDKit
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDKit(ByVal original_RowID As Integer, ByVal RFDNo As Integer, ByVal KitBPCSPartNo As String, _
            ByVal KitBPCSPartRevision As String, ByVal FinishedGoodBPCSPartNo As String, _
            ByVal FinishedGoodBPCSPartRevision As String, ByVal RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

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

            Dim rowsAffected As Integer = Adapter.UpdateRFDKit(original_RowID, RFDNo, KitBPCSPartNo, KitBPCSPartRevision, FinishedGoodBPCSPartNo, FinishedGoodBPCSPartRevision, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", KitBPCSPartNo:" & KitBPCSPartNo _
            & ", KitBPCSRevision:" & KitBPCSPartRevision _
            & ", FinishedGoodBPCSPartNo:" & FinishedGoodBPCSPartNo _
            & ", FinishedGoodBPCSPartRevision:" & FinishedGoodBPCSPartRevision _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete RFDKit
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDKit(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDKit(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDKit(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDKitBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDKit : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDKitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
