''******************************************************************************************************
''* SubDrawingsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 07/30/2008
''* Modified: {Name} {Date} - {Notes}
''*         : Roderick Carlson 01/05/2010 - changed to make RowID the key, allow Subdrawing to be udpated  
''* Modified: Roderick Carlson 02/22/2010 - PDE-2834 - added Process, Equipment, and ProcessParameters 
''* Modified: Roderick Carlson 06/29/2011 - If during the process of editing and issued drawing, do NOT delete a subdrawing unless the edit notes field has a reason

''******************************************************************************************************


Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class SubDrawingsBLL
    Private SubDrawingAdapter As SubDrawingTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.SubDrawingTableAdapter
        Get
            If SubDrawingAdapter Is Nothing Then
                SubDrawingAdapter = New SubDrawingTableAdapter()
            End If
            Return SubDrawingAdapter
        End Get
    End Property
    ''*****
    ''* Select SubDrawings returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSubDrawings(ByVal DrawingNo As String, ByVal SubDrawingNo As String, ByVal PartNo As String, _
    ByVal PartRevision As String, ByVal SubPartNo As String, ByVal SubPartRevision As String, _
    ByVal DrawingQuantity As Double, ByVal Notes As String) As Drawings.SubDrawing_MaintDataTable

        Try
            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If SubDrawingNo Is Nothing Then
                SubDrawingNo = ""
            End If

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If PartRevision Is Nothing Then
                PartRevision = ""
            End If

            If SubPartNo Is Nothing Then
                SubPartNo = ""
            End If

            If SubPartRevision Is Nothing Then
                SubPartRevision = ""
            End If

            If Notes Is Nothing Then
                Notes = ""
            End If

            Return Adapter.GetSubDrawings(DrawingNo, SubDrawingNo, PartNo, PartRevision, SubPartNo, _
            SubPartRevision, DrawingQuantity, Notes, False)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "SubDrawingNo: " & SubDrawingNo & ",PartNo:" & PartNo & "PartRevision: " & PartRevision & "SubPartNo: " & SubPartNo & "SubPartRevision: " & SubPartRevision & "DrawingQuantity: " & DrawingQuantity & "Notes: " & Notes & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSubDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubDrawingsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSubDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubDrawingsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert SubDrawings
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertSubDrawing(ByVal DrawingNo As String, ByVal SubDrawingNo As String, _
        ByVal DrawingQuantity As Double, ByVal Notes As String, _
        ByVal Process As String, ByVal Equipment As String, ByVal ProcessParameters As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If SubDrawingNo Is Nothing Then
                SubDrawingNo = ""
            End If

            If Notes Is Nothing Then
                Notes = ""
            End If

            If Process Is Nothing Then
                Process = ""
            End If

            If Equipment Is Nothing Then
                Equipment = ""
            End If

            If ProcessParameters Is Nothing Then
                ProcessParameters = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertSubDrawing(DrawingNo, SubDrawingNo, DrawingQuantity, Notes, _
            Process, Equipment, ProcessParameters, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", SubDrawing:" & SubDrawingNo _
            & ", DrawingQuantity: " & DrawingQuantity & "Notes: " & Notes _
            & ", Process: " & Process & "Equipment: " & Equipment & "ProcessParameters: " & ProcessParameters _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSubDrawing : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubDrawingsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubDrawingsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update SubDrawings
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSubDrawings(ByVal DrawingQuantity As Double, ByVal Notes As String, ByVal Process As String, _
        ByVal Equipment As String, ByVal ProcessParameters As String, _
        ByVal original_RowID As Integer, ByVal SubDrawingNo As String, ByVal RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SubDrawingNo Is Nothing Then
                SubDrawingNo = ""
            End If

            If Notes Is Nothing Then
                Notes = ""
            End If

            If Process Is Nothing Then
                Process = ""
            End If

            If Equipment Is Nothing Then
                Equipment = ""
            End If

            If ProcessParameters Is Nothing Then
                ProcessParameters = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateSubDrawing(original_RowID, SubDrawingNo, DrawingQuantity, Notes, _
                Process, Equipment, ProcessParameters, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = ", SubDrawingNo:" & SubDrawingNo & ", DrawingQuantity: " & DrawingQuantity _
            & ", Notes: " & Notes & ", Process: " & Process _
            & ", Equipment: " & Equipment & ", ProcessParameters: " & ProcessParameters _
            & ", RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSubDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubDrawingsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSubDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubDrawingsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete SubDrawings
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteSubDrawings(ByVal original_RowID As Integer, ByVal DrawingStatusID As String, ByVal AppendRevisionNotes As String) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If DrawingStatusID Is Nothing Then
                DrawingStatusID = ""
            End If

            If AppendRevisionNotes Is Nothing Then
                AppendRevisionNotes = ""
            End If

            Dim rowsAffected As Integer = 0
            'do not let issued drawings delete subdrawings in the BOM until a reason is saved
            If (DrawingStatusID = "I" And AppendRevisionNotes.Trim <> "") Or DrawingStatusID = "N" Then
                rowsAffected = Adapter.DeleteSubDrawing(original_RowID)
            End If

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", DrawingStatusID: " & original_RowID _
            & ", AppendRevisionNotes: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSubDrawing : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) _
            & " :<br/> SubDrawingsBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSubDrawing : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "SubDrawingsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
