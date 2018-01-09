''******************************************************************************************************
''* DrawingToleranceBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 07/30/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingToleranceBLL
    Private DrawingToleranceAdapter As DrawingToleranceTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingToleranceTableAdapter
        Get
            If DrawingToleranceAdapter Is Nothing Then
                DrawingToleranceAdapter = New DrawingToleranceTableAdapter()
            End If
            Return DrawingToleranceAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingTolerance returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingTolerance(ByVal ToleranceID As Integer, ByVal ToleranceName As String) As Drawings.DrawingTolerance_MaintDataTable

        Try
            If ToleranceName Is Nothing Then
                ToleranceName = ""
            End If

            Return Adapter.GetDrawingTolerance(ToleranceID, ToleranceName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ToleranceID: " & ToleranceID & "ToleranceName: " & ToleranceName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingToleranceBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingToleranceMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingToleranceBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert DrawingTolerance
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingTolerance(ByVal ToleranceName As String, ByVal DensityValue As Double, ByVal DensityTolerance As String, ByVal DensityUnits As String, ByVal ThicknessValue As Double, ByVal ThicknessTolerance As String, ByVal ThicknessUnits As String, ByVal WMDValue As Double, ByVal WMDTolerance As String, ByVal WMDUnits As String, ByVal AMDValue As Double, ByVal AMDTolerance As String, ByVal AMDUnits As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ToleranceName Is Nothing Then
                ToleranceName = ""
            End If

            If DensityTolerance Is Nothing Then
                DensityTolerance = ""
            End If

            If DensityUnits Is Nothing Then
                DensityUnits = ""
            End If

            If ThicknessTolerance Is Nothing Then
                ThicknessTolerance = ""
            End If

            If ThicknessUnits Is Nothing Then
                ThicknessUnits = ""
            End If

            If WMDTolerance Is Nothing Then
                WMDTolerance = ""
            End If

            If WMDUnits Is Nothing Then
                WMDUnits = ""
            End If

            If AMDTolerance Is Nothing Then
                AMDTolerance = ""
            End If

            If AMDUnits Is Nothing Then
                AMDUnits = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertDrawingTolerance(ToleranceName, DensityValue, DensityTolerance, DensityUnits, ThicknessValue, ThicknessTolerance, ThicknessUnits, WMDValue, WMDTolerance, WMDUnits, AMDValue, AMDTolerance, AMDUnits, CreatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ToleranceName: " & ToleranceName & ", DensityValue:" & DensityValue & ", DensityTolerance:" & DensityTolerance & ", DensityUnits:" & DensityUnits & ", ThicknessValue:" & ThicknessValue & ", ThicknessTolerance:" & ThicknessTolerance & ", ThicknessUnits:" & ThicknessUnits & ", WMDValue:" & WMDValue & ", WMDTolerance:" & WMDTolerance & ", WMDUnits:" & WMDUnits & ", AMDValue:" & AMDValue & ", AMDTolerance:" & AMDTolerance & ", AMDUnits:" & AMDUnits & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingToleranceBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingToleranceMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingToleranceBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update DrawingTolerance
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateDrawingTolerance(ByVal ToleranceID As Integer, ByVal ToleranceName As String, ByVal DensityValue As Double, ByVal DensityTolerance As String, ByVal DensityUnits As String, ByVal ThicknessValue As Double, ByVal ThicknessTolerance As String, ByVal ThicknessUnits As String, ByVal WMDValue As Double, ByVal WMDTolerance As String, ByVal WMDUnits As String, ByVal AMDValue As Double, ByVal AMDTolerance As String, ByVal AMDUnits As String, ByVal Obsolete As Boolean, ByVal original_toleranceID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ToleranceName Is Nothing Then
                ToleranceName = ""
            End If

            If DensityTolerance Is Nothing Then
                DensityTolerance = ""
            End If

            If DensityUnits Is Nothing Then
                DensityUnits = ""
            End If

            If ThicknessTolerance Is Nothing Then
                ThicknessTolerance = ""
            End If

            If ThicknessUnits Is Nothing Then
                ThicknessUnits = ""
            End If

            If WMDTolerance Is Nothing Then
                WMDTolerance = ""
            End If

            If WMDUnits Is Nothing Then
                WMDUnits = ""
            End If

            If AMDTolerance Is Nothing Then
                AMDTolerance = ""
            End If

            If AMDUnits Is Nothing Then
                AMDUnits = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateDrawingTolerance(original_toleranceID, ToleranceName, DensityValue, DensityTolerance, DensityUnits, ThicknessValue, ThicknessTolerance, ThicknessUnits, WMDValue, WMDTolerance, WMDUnits, AMDValue, AMDTolerance, AMDUnits, UpdatedBy, Obsolete, 0)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_toleranceID: " & original_toleranceID & "ToleranceName: " & ToleranceName & ", DensityValue:" & DensityValue & ", DensityTolerance:" & DensityTolerance & ", DensityUnits:" & DensityUnits & ", ThicknessValue:" & ThicknessValue & ", ThicknessTolerance:" & ThicknessTolerance & ", ThicknessUnits:" & ThicknessUnits & ", WMDValue:" & WMDValue & ", WMDTolerance:" & WMDTolerance & ", WMDUnits:" & WMDUnits & ", AMDValue:" & AMDValue & ", AMDTolerance:" & AMDTolerance & ", AMDUnits:" & AMDUnits & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingToleranceBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingToleranceMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingToleranceBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
   End Class
