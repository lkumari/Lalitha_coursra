''************************************************************************************************
''Name:		PKGModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Purchasing Module
''
''Date		    Author	    
''09/07/2012    LRey			Created .Net application
''09/20/2012    SHoward			Added Packaging Layout Get,Insert,Update,Delete
''05/15/2014    LRey            Replaced SoldTo/Cabbv to Customer
''************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class PKGModule

    Public Shared Sub CleanFormCrystalReports()

        Dim tempRpt As ReportDocument = New ReportDocument()
        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then

            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If

    End Sub 'EOF CleanFormCrystalReports

#Region "Container List/Entry"

    Public Shared Sub DeletePkgContainerCookies()

        Try
            HttpContext.Current.Response.Cookies("PCO_CNO").Value = ""
            HttpContext.Current.Response.Cookies("PCO_CNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PCO_CDesc").Value = ""
            HttpContext.Current.Response.Cookies("PCO_CDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PCO_Type").Value = ""
            HttpContext.Current.Response.Cookies("PCO_Type").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PCO_Customer").Value = ""
            HttpContext.Current.Response.Cookies("PCO_Customer").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PCO_Vendor").Value = ""
            HttpContext.Current.Response.Cookies("PCO_Vendor").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PCO_OEM").Value = ""
            HttpContext.Current.Response.Cookies("PCO_OEM").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteContainerCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PKG/ContainerList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteContainerCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeletePkgContainerCookies

    Public Shared Function DeletePkgContainer(ByVal CID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_PKG_Container"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeletePkgContainer")
            DeletePkgContainer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID: " & CID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeletePkgContainer") = "~/PKG/Container.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeletePkgContainer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF DeletePkgContainer

    Public Shared Function GetPkgContainer(ByVal CID As String, ByVal ContainerNo As String, ByVal Desc As String, ByVal Type As String, ByVal OEM As String, ByVal Customer As String, ByVal Vendor As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_PKG_Container"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.VarChar)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@ContainerNo", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerNo").Value = ContainerNo

            myCommand.Parameters.Add("@Desc", SqlDbType.VarChar)
            myCommand.Parameters("@Desc").Value = commonFunctions.replaceSpecialChar(Desc, False)

            myCommand.Parameters.Add("@Type", SqlDbType.VarChar)
            myCommand.Parameters("@Type").Value = commonFunctions.replaceSpecialChar(Type, False)

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = commonFunctions.replaceSpecialChar(Customer, False)

            myCommand.Parameters.Add("@Vendor", SqlDbType.VarChar)
            myCommand.Parameters("@Vendor").Value = Vendor

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPkgContainer")

            GetPkgContainer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CID: " & CID

            HttpContext.Current.Session("BLLerror") = "GetPkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPkgContainer") = "~/PKG/ContainerList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPkgContainer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetPkgContainer

    Public Shared Function GetColor(ByVal CCode As String, ByVal Color As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Color_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CCode", SqlDbType.VarChar)
            myCommand.Parameters("@CCode").Value = CCode

            myCommand.Parameters.Add("@Color ", SqlDbType.VarChar)
            myCommand.Parameters("@Color ").Value = Color

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetColor")

            GetColor = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetColor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetColor") = "~/PKG/ContainerList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetColor : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetColor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetColor

    Public Shared Function InsertPkgContainer(ByVal ContainerNo As String, ByVal Desc As String, ByVal Type As String, ByVal OEM As String, ByVal OEMManufacturer As String, ByVal CCode As String, ByVal InDimL As String, ByVal InDimW As String, ByVal InDimH As String, ByVal OutDimL As String, ByVal OutDimW As String, ByVal OutDimH As String, ByVal TareWeight As Decimal, ByVal Notes As String, ByVal CreatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_PKG_Container"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ContainerNo", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerNo").Value = commonFunctions.convertSpecialChar(ContainerNo, False)

            myCommand.Parameters.Add("@Desc", SqlDbType.VarChar)
            myCommand.Parameters("@Desc").Value = commonFunctions.convertSpecialChar(Desc, False)

            myCommand.Parameters.Add("@Type", SqlDbType.VarChar)
            myCommand.Parameters("@Type").Value = Type

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@CCode", SqlDbType.VarChar)
            myCommand.Parameters("@CCode").Value = CCode

            myCommand.Parameters.Add("@InDimL", SqlDbType.VarChar)
            myCommand.Parameters("@InDimL").Value = InDimL

            myCommand.Parameters.Add("@InDimW", SqlDbType.VarChar)
            myCommand.Parameters("@InDimW").Value = InDimW

            myCommand.Parameters.Add("@InDimH", SqlDbType.VarChar)
            myCommand.Parameters("@InDimH").Value = InDimH

            myCommand.Parameters.Add("@OutDimL", SqlDbType.VarChar)
            myCommand.Parameters("@OutDimL").Value = OutDimL

            myCommand.Parameters.Add("@OutDimW", SqlDbType.VarChar)
            myCommand.Parameters("@OutDimW").Value = OutDimW

            myCommand.Parameters.Add("@OutDimH", SqlDbType.VarChar)
            myCommand.Parameters("@OutDimH").Value = OutDimH

            myCommand.Parameters.Add("@TareWeight", SqlDbType.Decimal)
            myCommand.Parameters("@TareWeight").Value = TareWeight

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewPkgContainer")
            InsertPkgContainer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", ContainerNo: " & ContainerNo & ", Desc: " & Desc & ", Type: " & Type & ", OEM: " & OEM & ", OEMManufacturer: " & OEMManufacturer & ", CCode: " & CCode & ", InDimL: " & InDimL & ", InDimW: " & InDimW & ", InDimH: " & InDimH & ", OutDimL: " & OutDimL & ", OutDimW: " & OutDimW & ", OutDimH: " & OutDimH & ", TareWeight: " & TareWeight & ", Notes: " & Notes

            HttpContext.Current.Session("BLLerror") = "InsertPkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertPkgContainer") = "~/PKG/Container.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertPkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertPkgContainer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF InsertPkgContainer

    Public Shared Function UpdatePkgContainer(ByVal CID As Integer, ByVal ContainerNo As String, ByVal Desc As String, ByVal Type As String, ByVal Notes As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_PKG_Container"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@ContainerNo", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerNo").Value = ContainerNo

            myCommand.Parameters.Add("@Desc", SqlDbType.VarChar)
            myCommand.Parameters("@Desc").Value = commonFunctions.convertSpecialChar(Desc, False)

            myCommand.Parameters.Add("@Type", SqlDbType.VarChar)
            myCommand.Parameters("@Type").Value = Type

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdatePkgContainer")
            UpdatePkgContainer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & _
                ", ContainerNo: " & ContainerNo & ", Desc: " & Desc & ", Type: " & Type & ", Notes: " & Notes

            HttpContext.Current.Session("BLLerror") = "UpdatePkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdatePkgContainer") = "~/PKG/Container.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdatePkgContainer : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdatePkgContainer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF UpdatePkgContainer

#End Region

#Region "Packaging Layout List/Entry"

    Public Shared Sub DeletePKGLayoutCookies()

        Try

            HttpContext.Current.Response.Cookies("PL_LDESC").Value = ""
            HttpContext.Current.Response.Cookies("PL_LDESC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_CNO").Value = ""
            HttpContext.Current.Response.Cookies("PL_CNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_OEMMFG").Value = ""
            HttpContext.Current.Response.Cookies("PL_OEMMFG").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_MAKE").Value = ""
            HttpContext.Current.Response.Cookies("PL_MAKE").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_MODEL").Value = ""
            HttpContext.Current.Response.Cookies("PL_MODEL").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_Customer").Value = ""
            HttpContext.Current.Response.Cookies("PL_Customer").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_FAC").Value = ""
            HttpContext.Current.Response.Cookies("PL_FAC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_DPT").Value = ""
            HttpContext.Current.Response.Cookies("PL_DPT").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_WC").Value = ""
            HttpContext.Current.Response.Cookies("PL_WC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PL_PNO").Value = ""
            HttpContext.Current.Response.Cookies("PL_PNO").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteLayoutCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PKG/PackagingList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteLayoutCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub 'EOF DeletePKGLayoutCookies

    Public Shared Function GetPKGLayout(ByVal PKGID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_PKG_Layout"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter


        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PKGID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGID").Value = PKGID
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPKGLayout")

            GetPKGLayout = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PKGID: " & PKGID

            HttpContext.Current.Session("BLLerror") = "GetPkgLayout  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPkgLayout ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPkgLayout : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPKGLayout = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPKGLayout

    Public Shared Function GetPKGLayoutSearch(ByVal PKGID As String, ByVal LayoutDesc As String, ByVal ContainerNo As String, ByVal OEMManufacturer As String, ByVal Make As String, ByVal Model As String, ByVal UGNFacility As String, ByVal DepartmentID As Integer, ByVal WorkCenter As Integer, ByVal Customer As String, ByVal PartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_PKG_Layout_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter


        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PKGID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGID").Value = PKGID

            myCommand.Parameters.Add("@LayoutDesc", SqlDbType.VarChar)
            myCommand.Parameters("@LayoutDesc").Value = LayoutDesc

            myCommand.Parameters.Add("@ContainerNo", SqlDbType.VarChar)
            myCommand.Parameters("@ContainerNo").Value = ContainerNo

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@Model", SqlDbType.VarChar)
            myCommand.Parameters("@Model").Value = Model

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DepartmentID", SqlDbType.Int)
            myCommand.Parameters("@DepartmentID").Value = DepartmentID

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.Int)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPKGLayoutSearch")

            GetPKGLayoutSearch = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PKGID: " & PKGID

            HttpContext.Current.Session("BLLerror") = "GetPKGLayoutSearch  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPKGLayoutSearch ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPKGLayoutSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPKGLayoutSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPKGLayoutSearch

    Public Shared Function GetPKGLastLayoutID(ByVal LayoutDesc As String, ByVal CreatedBy As String, ByVal Createdon As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_PKG_Last_Layout_ID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter


        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@LayoutDesc", SqlDbType.VarChar)
            myCommand.Parameters("@LayoutDesc").Value = commonFunctions.convertSpecialChar(LayoutDesc, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@Createdon", SqlDbType.VarChar)
            myCommand.Parameters("@Createdon").Value = Createdon

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPKGLastLayoutID")

            GetPKGLastLayoutID = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", LayoutDesc: " & LayoutDesc

            HttpContext.Current.Session("BLLerror") = "GetPKGLastLayoutID  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPKGLastLayoutID ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPKGLastLayoutID : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPKGLastLayoutID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPKGLastLayoutID

    Public Shared Function InsertPKGLayout(ByVal LayoutDesc As String, ByVal PKGLeadTMID As Integer, ByVal IsPublish As Boolean, ByVal UGNFacility As String, ByVal WorkCenter As Integer, ByVal OEMManufacturer As String, ByVal CID As Integer, ByVal ModelYr As Double, ByVal ProgramID As Integer, ByVal GrossWeight As Decimal, ByVal Notes As String, ByVal CreatedBy As String, ByVal Createdon As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_PKG_Layout"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@LayoutDesc", SqlDbType.VarChar)
            myCommand.Parameters("@LayoutDesc").Value = commonFunctions.convertSpecialChar(LayoutDesc, False)

            myCommand.Parameters.Add("@PKGLeadTMID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGLeadTMID").Value = PKGLeadTMID

            myCommand.Parameters.Add("@IsPublish", SqlDbType.VarChar)
            myCommand.Parameters("@IsPublish").Value = IsPublish

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = commonFunctions.convertSpecialChar(UGNFacility, False)

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.VarChar)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = commonFunctions.convertSpecialChar(OEMManufacturer, False)

            myCommand.Parameters.Add("@CID", SqlDbType.VarChar)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@ModelYr", SqlDbType.VarChar)
            myCommand.Parameters("@ModelYr").Value = ModelYr

            myCommand.Parameters.Add("@ProgramID", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@GrossWeight", SqlDbType.Decimal)
            myCommand.Parameters("@GrossWeight").Value = GrossWeight

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = Createdon


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewPkgLayout")
            InsertPKGLayout = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CID: " & CID

            HttpContext.Current.Session("BLLerror") = "GetPkgLayout  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetPkgLayout ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("GetPkgLayout : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertPKGLayout = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF InsertPKGLayout

    Public Shared Function UpdatePKGLayout(ByVal PKGID As Integer, ByVal LayoutDesc As String, ByVal PKGLeadTMID As Integer, ByVal IsPublish As Boolean, ByVal UGNFacility As String, ByVal WorkCenter As Integer, ByVal CID As Integer, ByVal ModelYr As Double, ByVal ProgramID As Integer, ByVal GrossWeight As Decimal, ByVal Notes As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_PKG_Layout"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PKGID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGID").Value = PKGID

            myCommand.Parameters.Add("@LayoutDesc", SqlDbType.VarChar)
            myCommand.Parameters("@LayoutDesc").Value = commonFunctions.convertSpecialChar(LayoutDesc, False)

            myCommand.Parameters.Add("@PKGLeadTMID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGLeadTMID").Value = PKGLeadTMID

            myCommand.Parameters.Add("@IsPublish", SqlDbType.VarChar)
            myCommand.Parameters("@IsPublish").Value = IsPublish

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = commonFunctions.convertSpecialChar(UGNFacility, False)

            myCommand.Parameters.Add("@WorkCenter", SqlDbType.VarChar)
            myCommand.Parameters("@WorkCenter").Value = WorkCenter

            myCommand.Parameters.Add("@CID", SqlDbType.VarChar)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@ModelYr", SqlDbType.VarChar)
            myCommand.Parameters("@ModelYr").Value = ModelYr

            myCommand.Parameters.Add("@ProgramID", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@GrossWeight", SqlDbType.Decimal)
            myCommand.Parameters("@GrossWeight").Value = GrossWeight

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(Notes, False)

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdatePKGLayout")
            UpdatePKGLayout = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CID: " & CID

            HttpContext.Current.Session("BLLerror") = "UpdatePkgLayout  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdatePkgLayout ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdatePkgLayout : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdatePKGLayout = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF UpdatePKGLayout

    Public Shared Function UpdatePKGLayoutImage(ByVal PKGID As Integer, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_PKG_Layout_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PKGID", SqlDbType.VarChar)
            myCommand.Parameters("@PKGID").Value = PKGID

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.convertSpecialChar(FileName, False)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdatePKGLayoutImage")
            UpdatePKGLayoutImage = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", PKGID: " & PKGID

            HttpContext.Current.Session("BLLerror") = "UpdatePKGLayoutImage  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdatePKGLayoutImage ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdatePKGLayoutImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdatePKGLayoutImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF UpdatePKGLayoutImage

    Public Shared Function DeletePKGLayout(ByVal PKGID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_PKG_Layout"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PKGID", SqlDbType.Int)
            myCommand.Parameters("@PKGID").Value = PKGID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DeletePkgContainer")
            DeletePKGLayout = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGLayout : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeletePKGLayout") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("DeletePKGLayout : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeletePKGLayout = Nothing
        Finally

            myConnection.Close()
            myCommand.Dispose()

        End Try
    End Function 'EOF DeletePKGLayout

    Public Shared Function GetPKGContainerCustomer(ByVal CID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer) As DataSet


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_PKG_Container_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter


        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.VarChar)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = commonFunctions.convertSpecialChar(CABBV, False)

            myCommand.Parameters.Add("@SoldTo", SqlDbType.VarChar)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetContainerCustomer")

            GetPKGContainerCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", CID: " & CID

            HttpContext.Current.Session("BLLerror") = "GetContainerCustomer  : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetContainerCustomer ") = "~/PKG/Packaging.aspx"

            UGNErrorTrapping.InsertErrorLog("GetContainerCustomer : " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPKGContainerCustomer = Nothing

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Function 'EOF GetPKGContainerCustomer

#End Region

End Class
