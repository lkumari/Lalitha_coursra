''******************************************************************************************************
''* DrawingImagesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 07/30/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************


Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingImagesBLL
    Private DrawingImageAdapter As DrawingsTableAdapters.DrawingImagesTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingImagesTableAdapter
        Get
            If DrawingImageAdapter Is Nothing Then
                DrawingImageAdapter = New DrawingImagesTableAdapter()
            End If
            Return DrawingImageAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingImages returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingImages(ByVal DrawingNo As String) As Drawings.DrawingImages_MaintDataTable

        Try
            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            Return Adapter.GetDrawingImages(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingImagesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingImagesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ' ''*****
    ' ''* Update DrawingImages
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    'Public Function UpdateDrawingImages(ByVal Obsolete As Boolean, ByVal original_DrawingImage As Integer) As Boolean

    '    Try
    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.UpdateDrawingImage(original_DrawingImage, Obsolete, UpdatedBy)
    '        'MsgBox("Rows Affected: " & rowsAffected)
    '        '' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "Obsolete: " & Obsolete & ", original_DrawingImage:" & original_DrawingImage & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "UpdateDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingImagesBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/DrawingImageMaintenance.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingImagesBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

End Class
