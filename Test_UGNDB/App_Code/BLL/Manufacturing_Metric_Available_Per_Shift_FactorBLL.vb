''******************************************************************************************************
''* Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 07/15/2010
''* Modified: {Name} {Date} - {Notes}
''            Roderick Carlson 01/21/2011 - by department
''******************************************************************************************************

Imports Manufacturing_MetricTableAdapters

<System.ComponentModel.DataObject()> _
Public Class Manufacturing_Metric_Available_Per_Shift_FactorBLL
    Private ManufacturingMetricAvailablePerShiftFactorAdapter As ManufacturingMetricAvailablePerShiftFactorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As Manufacturing_MetricTableAdapters.ManufacturingMetricAvailablePerShiftFactorTableAdapter
        Get
            If ManufacturingMetricAvailablePerShiftFactorAdapter Is Nothing Then
                ManufacturingMetricAvailablePerShiftFactorAdapter = New ManufacturingMetricAvailablePerShiftFactorTableAdapter()
            End If
            Return ManufacturingMetricAvailablePerShiftFactorAdapter
        End Get
    End Property
    ''*****
    ''* Select Manufacturing_Metric_AvailablePerShiftFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetManufacturingMetricAvailablePerShiftFactorList(ByVal UGNFacility As String, ByVal DeptID As Integer) As Manufacturing_Metric.ManufacturingMetricAvailablePerShiftFactor_MaintDataTable

        Try
            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            Return Adapter.GetManufacturingMetricAvailablePerShiftFactorList(UGNFacility, DeptID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DeptID: " & DeptID _
            & ", UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert Manufacturing_Metric_AvailablePerShiftFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertManufacturingMetricAvailablePerShiftFactor(ByVal UGNFacility As String, _
    ByVal DeptID As Integer, ByVal AvailablePerShiftFactor As Double, ByVal EffectiveDate As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            If EffectiveDate Is Nothing Then
                EffectiveDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertManufacturingMetricAvailablePerShiftFactor(UGNFacility, DeptID, AvailablePerShiftFactor, EffectiveDate, CreatedBy)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", DeptID:" & DeptID _
            & ", AvailablePerShiftFactor:" & AvailablePerShiftFactor _
            & ", EffectiveDate:" & EffectiveDate _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update Manufacturing_MetricAvailablePerShiftFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateManufacturingMetricAvailablePerShiftFactor(ByVal UGNFacility As String, _
    ByVal DeptID As Integer, ByVal AvailablePerShiftFactor As Double, _
    ByVal EffectiveDate As String, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            If EffectiveDate Is Nothing Then
                EffectiveDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateManufacturingMetricAvailablePerShiftFactor(original_RowID, UGNFacility, DeptID, AvailablePerShiftFactor, EffectiveDate, UpdatedBy)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID _
            & ", UGNFacility: " & UGNFacility _
            & ", DeptID:" & DeptID _
            & ", AvailablePerShiftFactor:" & AvailablePerShiftFactor _
            & ", EffectiveDate:" & EffectiveDate _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete Manufacturing_MetricAvailablePerShiftFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteManufacturing_MetricCustomerProgram(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteManufacturingMetricAvailablePerShiftFactor(original_RowID, UpdatedBy)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteManufacturingMetricAvailablePerShiftFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "Manufacturing_Metric_Available_Per_Shift_FactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
