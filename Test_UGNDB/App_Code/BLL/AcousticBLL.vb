''******************************************************************************************************
''* AcousticBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Acoustic_Project_Detail.aspx - gvCommodity
''* Author  : LRey 05/05/2009
''******************************************************************************************************
Imports AcousticTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AcousticBLL
    Private vAdapter As Acoustic_Project_Commodities_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As AcousticTableAdapters.Acoustic_Project_Commodities_TableAdapter
        Get
            If vAdapter Is Nothing Then
                vAdapter = New Acoustic_Project_Commodities_TableAdapter()
            End If
            Return vAdapter
        End Get
    End Property

    ''*****
    ''* Select Acoustic_Project_Commodities returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetAcousticProjectCommodities(ByVal ProjectID As Integer) As Acoustic.Acoustic_Project_CommoditiesDataTable
        If ProjectID = 0 And HttpContext.Current.Request.QueryString("pProjID") <> Nothing Then
            ProjectID = HttpContext.Current.Request.QueryString("pProjID")
        End If


        Return Adapter.GetAcousticProjectCommodities(ProjectID)
    End Function

    ''*****
    ''* Insert a New row to Acoustic_Project_Commodities table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertAcousticProjectCommodities(ByVal ProjectID As Integer, ByVal CommodityID As Integer) As Boolean

        ' Create a new vRow instance
        Dim vTable As New Acoustic.Acoustic_Project_CommoditiesDataTable
        Dim vRow As Acoustic.Acoustic_Project_CommoditiesRow = vTable.NewAcoustic_Project_CommoditiesRow
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value


        If ProjectID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Project No is a required field.")
        End If

        If CommodityID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Commodity is a required field.")
        End If

        ' Insert the new Acoustic_Project_Commodities row
        Dim rowsAffected As Integer = Adapter.sp_Insert_Acoustic_Project_Commodities(ProjectID, CommodityID, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
   
    ''*****
    ''* Delete Acoustic_Project_Commodities
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAcousticProjectCommodities(ByVal ProjectID As Integer, ByVal CommodityID As Integer, ByVal original_ProjectID As Integer, ByVal original_CommodityID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Acoustic_Project_Commodities(original_ProjectID, original_CommodityID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class


