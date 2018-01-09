''******************************************************************************************************
''* DrawingsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/28/2008
''* Modified: {Name} {Date} - {Notes}
''            Roderick Carlson 10/20/2008 - added CABBV
''            Roderick Carlson 05/28/2009 - PDE # 2715 - added Vehcile Year   
''            Roderick Carlson 06/04/2009 - added SoldTo and DesignationType, removed includeBOM
''            Roderick Carlosn 02/22/2010 - DAL file called sp_Get_Drawing_Search instead of sp_Get_Drawing
''* 12/20/2013    LRey      Replaced "SoldTo|CABBV" to "PartNo" wherever used. 
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingsBLL
    Private DrawingsAdapter As DrawingTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingTableAdapter
        Get
            If DrawingsAdapter Is Nothing Then
                DrawingsAdapter = New DrawingTableAdapter()
            End If
            Return DrawingsAdapter
        End Get
    End Property
    ''*****
    ''* Select Drawings returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
       Public Function GetDrawings(ByVal DrawingNo As String, ByVal ReleaseTypeID As Integer, ByVal PartNo As String, _
ByVal PartName As String, ByVal CustomerPartNo As String, ByVal Customer As String, _
ByVal DesignationType As String, ByVal VehicleYear As Integer, ByVal ProgramID As Integer, _
ByVal SubFamilyID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, ByVal DensityValue As Double, ByVal Construction As String, ByVal ApprovalStatus As String, ByVal Notes As String, ByVal DrawingByEngineerID As Integer, ByVal Obsolete As Boolean, ByVal DrawingDateStart As String, ByVal DrawingDateEnd As String, ByVal Make As String, ByVal ProductTechnologyID As Integer) As Drawings.Drawing_MaintDataTable

        Try
            If DrawingNo Is Nothing Then DrawingNo = ""
            If PartNo Is Nothing Then PartNo = ""
            If PartName Is Nothing Then PartName = ""
            If Customer Is Nothing Then Customer = ""
            If DesignationType Is Nothing Then DesignationType = ""
            If CustomerPartNo Is Nothing Then CustomerPartNo = ""
            If Construction Is Nothing Then Construction = ""
            If ApprovalStatus Is Nothing Then ApprovalStatus = ""
            If Notes Is Nothing Then Notes = ""
            If DrawingDateStart Is Nothing Then DrawingDateStart = ""
            If DrawingDateEnd Is Nothing Then DrawingDateEnd = ""
            If Make Is Nothing Then Make = ""

            Return Adapter.GetDrawings(DrawingNo, ReleaseTypeID, PartNo, PartName, CustomerPartNo, Customer, _
            DesignationType, VehicleYear, ProgramID, SubFamilyID, CommodityID, PurchasedGoodID, DensityValue, Construction, ApprovalStatus, Notes, DrawingByEngineerID, Obsolete, DrawingDateStart, DrawingDateEnd, Make, ProductTechnologyID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", ReleaseTypeID:" & ReleaseTypeID _
            & ", PartNo: " & PartNo & ", PartName: " & PartName _
            & ", Customer: " & Customer & ", DesignationType: " & DesignationType _
            & ", CustomerPartNo: " & CustomerPartNo & ", VehicleYear: " & VehicleYear & ", ProgramID: " & ProgramID _
            & ", SubFamilyID: " & SubFamilyID & ", CommodityID: " & CommodityID & ", PurchasedGoodID: " & PurchasedGoodID _
            & ", DensityValue: " & DensityValue & ", Construction: " & Construction & ", ApprovalStatus: " & ApprovalStatus & ", Notes: " _
            & Notes & ", DrawingDateStart: " & DrawingDateStart _
            & ", DrawingDateEnd: " & DrawingDateEnd _
            & ", Make: " & Make _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawings : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

End Class
