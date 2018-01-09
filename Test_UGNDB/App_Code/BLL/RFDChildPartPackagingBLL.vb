''******************************************************************************************************
''* RFDChildPartPackagingBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 10/07/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDChildPartPackagingBLL
    Private RFDChildPartPackagingAdapter As RFDChildPartPackagingTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDChildPartPackagingTableAdapter
        Get
            If RFDChildPartPackagingAdapter Is Nothing Then
                RFDChildPartPackagingAdapter = New RFDChildPartPackagingTableAdapter()
            End If
            Return RFDChildPartPackagingAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDPackaging returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDChildPartPackaging(ByVal RFDNo As Integer) As RFD.RFDChildPartPackaging_MaintDataTable

        Try

            Return Adapter.GetRFDChildPartPackaging(0, RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDChildPartPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartPackagingBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDChildPartPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    '* Update RFDPackaging
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateRFDChildPartPackaging(ByVal original_RowID As Integer, ByVal RFDNo As Integer, _
        ByVal ContainerCount As Double, ByVal ContainerHeight As Double, ByVal ContainerHeightUnitID As Integer, _
        ByVal ContainerWidth As Double, ByVal ContainerWidthUnitID As Integer, _
        ByVal ContainerDepth As Double, ByVal ContainerDepthUnitID As Integer, _
        ByVal PackagingAnnualVolume As Double, ByVal SystemDayCount As Double, _
        ByVal PackagingComments As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.UpdateRFDChildPartPackaging(original_RowID, RFDNo, ContainerCount, _
            ContainerHeight, ContainerHeightUnitID, ContainerWidth, ContainerWidthUnitID, _
            ContainerDepth, ContainerDepthUnitID, PackagingAnnualVolume, SystemDayCount, PackagingComments, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo _
            & ", ContainerCount:" & ContainerCount _
            & ", ContainerHeight:" & ContainerHeight _
            & ", ContainerHeightUnitID:" & ContainerHeightUnitID _
            & ", ContainerWidth:" & ContainerWidth _
            & ", ContainerWidthUnitID:" & ContainerWidthUnitID _
            & ", ContainerDepth:" & ContainerDepth _
            & ", ContainerDepthUnitID:" & ContainerDepthUnitID _
            & ", PackagingAnnualVolume:" & PackagingAnnualVolume _
            & ", SystemDayCount:" & SystemDayCount _
            & ", PackagingComments:" & PackagingComments _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDChildPartPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDChildPartPackagingBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDChildPartPackaging : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDChildPartPackagingBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function


End Class
