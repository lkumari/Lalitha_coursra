''******************************************************************************************************
''* DrawingCustomerProgramBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 08/28/2009
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingCustomerProgramBLL
    Private DrawingCustomerProgramAdapter As DrawingCustomerProgramTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingCustomerProgramTableAdapter
        Get
            If DrawingCustomerProgramAdapter Is Nothing Then
                DrawingCustomerProgramAdapter = New DrawingCustomerProgramTableAdapter()
            End If
            Return DrawingCustomerProgramAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingCustomerProgram returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingCustomerProgram(ByVal DrawingNo As String) As Drawings.DrawingCustomerProgram_MaintDataTable

        Try
            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            Return Adapter.GetDrawingCustomerProgram(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingCustomerProgram(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingCustomerProgram(original_RowID)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
