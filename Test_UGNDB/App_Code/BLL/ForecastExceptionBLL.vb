''******************************************************************************************************
''* ForecastExceptionBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: ForecastException.aspx - gvCommodity
''* Author  : LRey 01/14/2011
''******************************************************************************************************
Imports FinancialsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ForecastExceptionBLL
    Private vAdapter As Forecast_Exception_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As FinancialsTableAdapters.Forecast_Exception_TableAdapter
        Get
            If vAdapter Is Nothing Then
                vAdapter = New Forecast_Exception_TableAdapter
            End If
            Return vAdapter
        End Get
    End Property

    Private vAdapter2 As OEM_Model_Conv_TableAdapter = Nothing
    Protected ReadOnly Property Adapter2() As FinancialsTableAdapters.OEM_Model_Conv_TableAdapter
        Get
            If vAdapter2 Is Nothing Then
                vAdapter2 = New OEM_Model_Conv_TableAdapter
            End If
            Return vAdapter2
        End Get
    End Property

    ''*****
    ''* Select Forecast_Exception returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetForecastException(ByVal RowID As Integer) As Financials.Forecast_ExceptionDataTable
        Try
            Return Adapter.Get_Forecast_Exception(RowID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ForecastExceptionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False), "ForecastExceptionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Delete row to Forecast_Exception table this will replace QTYRQ field with OQTYRQ in Forecast table
    ''*****

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteForecastException(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean
        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Forecast_Exception(original_RowID)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ForecastExceptionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/FIN/ForecastExceptionMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteForecastException : " & commonFunctions.convertSpecialChar(ex.Message, False), "ForecastExceptionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

    ''*****
    ''* Select OEM_Model_Conv returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetOEMModelConv(ByVal RowID As Integer, ByVal OEM As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal DABBV As String, ByVal PartField As String, ByVal OEMMfg As String) As Financials.OEM_Model_ConvDataTable
        Try
            If OEM = Nothing Then OEM = ""
            If CABBV = Nothing Then CABBV = ""
            If DABBV = Nothing Then DABBV = ""
            If PartField = Nothing Then PartField = ""
            If OEMMfg = Nothing Then OEMMfg = ""

            Return Adapter2.Get_OEM_Model_Conv(RowID, OEM, CABBV, SoldTo, DABBV, PartField, OEMMfg)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ForecastExceptionBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/FIN/OEMModelConvMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetOEMModelConv : " & commonFunctions.convertSpecialChar(ex.Message, False), "ForecastExceptionBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetOEMModelConv
End Class


