''******************************************************************************************************
''* Future_PartNoBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 05/13/2008
''* Modified: 10/14/2011    LREY    - Added UGNFacility, OEM, OEMManufacturer, DesignationType columns
''* Modified: 07/26/2012    LRey    - Modified the Insert statement to include RFDNo value added by RCarlson on 07/24/2012
''******************************************************************************************************

Imports Future_PartNoTableAdapters

<System.ComponentModel.DataObject()> _
Public Class Future_PartNoBLL
    Private FuturePartNoAdapter As Future_Part_Maint_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As Future_PartNoTableAdapters.Future_Part_Maint_TableAdapter
        Get
            If FuturePartNoAdapter Is Nothing Then
                FuturePartNoAdapter = New Future_Part_Maint_TableAdapter
            End If
            Return FuturePartNoAdapter
        End Get
    End Property
    ''*****
    ''* Select Future_Part_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFuturePartNo(ByVal PartNo As String, ByVal PartDesc As String, ByVal CreatedBy As String) As Future_PartNo.Future_Part_MaintDataTable
        Try
            If PartNo = Nothing Then
                PartNo = ""
            End If
            If PartDesc = Nothing Then
                PartDesc = ""
            End If
            If CreatedBy = Nothing Then
                CreatedBy = ""
            End If
            Return Adapter.GetFuturePartNo(PartNo, PartDesc, "", "", "", "", CreatedBy)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Future_PartNoBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Future_Part_Maint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "Future_PartNoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetFuturePartNo

    ''*****
    ''* Insert New Future_Part_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFuturePartNo(ByVal PartNo As String, ByVal PartDesc As String, ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal DesignationType As String) As Boolean
        Try
            ' Create a new pscpRow instance
            Dim psTable As New Future_PartNo.Future_Part_MaintDataTable
            Dim psRow As Future_PartNo.Future_Part_MaintRow = psTable.NewFuture_Part_MaintRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If PartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Future Part Number is a required field.")
            End If
            If PartDesc = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Future Part Description is a required field.")
            End If

            OEM = ""

            ' Insert the new Projected_Sales_Price row
            Dim rowsAffected As Integer = Adapter.sp_Insert_Future_PartNo(PartNo, PartDesc, UGNFacility, OEM, OEMManufacturer, DesignationType, 0, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Future_PartNoBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Future_Part_Maint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "Future_PartNoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF InsertFuturePartNo
    ''*****
    ''* Update Future_Part_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
      Public Function UpdateFuturePartNo(ByVal PartNo As String, ByVal PartDesc As String, ByVal Obsolete As Boolean, ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal DesignationType As String, ByVal original_PartNo As String, ByVal orignal_UGNFacility As String, ByVal original_OEM As String, ByVal original_OEMManufacturer As String, ByVal original_DesignationType As String) As Boolean
        Try
            Dim psTable As New Future_PartNo.Future_Part_MaintDataTable
            Dim psRow As Future_PartNo.Future_Part_MaintRow = psTable.NewFuture_Part_MaintRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If PartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Future Part Number is a required field.")
            End If
            If PartDesc = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Future Part Description is a required field.")
            End If

            Dim sUGNFacility = UGNFacility.Trim
            Dim iUGNFacilityLen As Integer = Len(sUGNFacility)
            Dim iUGNFacilityStartPos As Integer = InStr(UGNFacility, ":")
            sUGNFacility = sUGNFacility.Substring(0, iUGNFacilityStartPos - 1)
            UGNFacility = commonFunctions.convertSpecialChar(sUGNFacility, False)

            'Dim sOEM = OEM.Trim
            'Dim iOEMLen As Integer = Len(sOEM)
            'Dim iOEMStartPos As Integer = InStr(OEM, ":")
            'sOEM = sOEM.Substring(0, iOEMStartPos - 1)
            'OEM = commonFunctions.convertSpecialChar(sOEM, False)
            OEM = ""

            Dim sOEMManufacturer = OEMManufacturer.Trim
            Dim iOEMManufacturerLen As Integer = Len(sOEMManufacturer)
            Dim iOEMManufacturerStartPos As Integer = InStr(OEMManufacturer, ":")
            sOEMManufacturer = sOEMManufacturer.Substring(0, iOEMManufacturerStartPos - 1)
            OEMManufacturer = commonFunctions.convertSpecialChar(sOEMManufacturer, False)

            Dim sDesignationType = DesignationType.Trim
            Dim iDesignationTypeLen As Integer = Len(sDesignationType)
            Dim iDesignationTypeStartPos As Integer = InStr(DesignationType, ":")
            sDesignationType = sDesignationType.Substring(0, iDesignationTypeStartPos - 1)
            DesignationType = commonFunctions.convertSpecialChar(sDesignationType, False)


            ' Update the Projected_Sales_Price record
            Dim rowsAffected As Integer = Adapter.sp_Update_Future_PartNo(PartNo, PartDesc, UGNFacility, OEM, OEMManufacturer, DesignationType, Obsolete, original_PartNo, UGNFacility, OEM, OEMManufacturer, DesignationType, User)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = " User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Future_PartNoBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Future_Part_Maint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateFuturePartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "Future_PartNoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateFuturePartNo
End Class


