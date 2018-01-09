''******************************************************************************************************
''* DrawingBPCSBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 08/20/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingBPCSBLL
    Private DrawingBPCSAdapter As DrawingBPCSTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingBPCSTableAdapter
        Get
            If DrawingBPCSAdapter Is Nothing Then
                DrawingBPCSAdapter = New DrawingBPCSTableAdapter()
            End If
            Return DrawingBPCSAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingBPCS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingBPCS(ByVal DrawingNo As String) As Drawings.DrawingBPCS_MaintDataTable

        Try
            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            Return Adapter.GetDrawingBPCS(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingBPCSBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingBPCSBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert DrawingBPCS
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingBPCS(ByVal DrawingNo As String, ByVal PartNo As String, ByVal PartRevision As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If PartRevision Is Nothing Then
                PartRevision = ""
            End If


            Dim rowsAffected As Integer = Adapter.InsertDrawingBPCS(DrawingNo, PartNo, PartRevision, CreatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", PartNo:" & PartNo & ", PartRevision:" & PartRevision & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingBPCSBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingBPCSBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update DrawingBPCS
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateDrawingBPCS(ByVal PartNo As String, ByVal PartRevision As String, ByVal original_RowID As Integer, ByVal PartName As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If PartRevision Is Nothing Then
                PartRevision = ""
            End If


            Dim rowsAffected As Integer = Adapter.UpdateDrawingBPCS(original_RowID, PartNo, PartRevision, UpdatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID & ", PartNo: " & PartNo & ", PartRevision:" & PartRevision & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingBPCSBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingBPCSBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete DrawingBPCS
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingBPCS(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingBPCS(original_RowID)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingBPCSBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingBPCSBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
