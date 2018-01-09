''************************************************************************************************
''Name:		PGMModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the Purchasing Module
''
''Date		    Author	    
''02/05/2013    LRey			Created .Net application
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

Public Class PGMModule

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

#Region "Sample Trial Event"
    Public Shared Function GetSampleTrialEvent(ByVal TrialEvent As String, ByVal OEMMfg As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleTrialEvent"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TrialEvent", SqlDbType.VarChar)
            myCommand.Parameters("@TrialEvent").Value = TrialEvent

            myCommand.Parameters.Add("@OEMMfg", SqlDbType.VarChar)
            myCommand.Parameters("@OEMMfg").Value = OEMMfg

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetPkgContainer")

            GetSampleTrialEvent = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", TrialEvent: " & TrialEvent _
            & ", OEMMfg: " & OEMMfg

            HttpContext.Current.Session("BLLerror") = "GetSampleTrialEvent : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSampleTrialEvent") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleTrialEvent : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleTrialEvent = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleTrialEvent

#End Region 'EOF "Sample Trial Event"

#Region "Sample Material Request"
    Public Shared Sub DeleteSampleMtrlReqCookies()

        Try
            HttpContext.Current.Response.Cookies("SMR_SMRNO").Value = ""
            HttpContext.Current.Response.Cookies("SMR_SMRNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_SDESC").Value = ""
            HttpContext.Current.Response.Cookies("SMR_SDESC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_RTMID").Value = ""
            HttpContext.Current.Response.Cookies("SMR_RTMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_ATMID").Value = ""
            HttpContext.Current.Response.Cookies("SMR_ATMID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_UFAC").Value = ""
            HttpContext.Current.Response.Cookies("SMR_UFAC").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_CUST").Value = ""
            HttpContext.Current.Response.Cookies("SMR_CUST").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_PNO").Value = ""
            HttpContext.Current.Response.Cookies("SMR_PNO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_IE").Value = ""
            HttpContext.Current.Response.Cookies("SMR_IE").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_PONO").Value = ""
            HttpContext.Current.Response.Cookies("SMR_PONO").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SMR_RSTAT").Value = ""
            HttpContext.Current.Response.Cookies("SMR_RSTAT").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteSampleMtrlReqCookies") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteSampleMtrlReqCookies

    Public Shared Sub DeleteSampleMtrlReq(ByVal SMRNo As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_SampleMtrlReq"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("DeleteSampleMtrlReq") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteSampleMtrlReq

    Public Shared Sub DeleteSampleMtrlReqApproval(ByVal SMRNo As Integer, ByVal TMID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_SampleMtrlReq_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@TMID ", SqlDbType.Int)
            myCommand.Parameters("@TMID ").Value = TMID

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("DeleteSampleMtrlReqApproval") = "~/PGM/SampleMaterialRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteSampleMtrlReqApproval    

    Public Shared Function GetSampleMtrlReq(ByVal SMRNo As String, ByVal SampleDesc As String, ByVal RequestorTMID As Integer, ByVal AccountMgrTMID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal PartNo As String, ByVal IntExt As String, ByVal PONo As String, ByVal RecStatus As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = SampleDesc

            myCommand.Parameters.Add("@RequestorTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestorTMID").Value = RequestorTMID

            myCommand.Parameters.Add("@AccountMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AccountMgrTMID").Value = AccountMgrTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@IntExt", SqlDbType.VarChar)
            myCommand.Parameters("@IntExt").Value = IntExt

            myCommand.Parameters.Add("@PONo", SqlDbType.VarChar)
            myCommand.Parameters("@PONo").Value = PONo

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReq")

            GetSampleMtrlReq = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo _
            & ", SampleDesc: " & SampleDesc _
            & ", RequestorTMID: " & RequestorTMID _
            & ", AccountMgrTMID: " & AccountMgrTMID _
            & ", UGNFacility: " & UGNFacility _
            & ", Customer: " & Customer _
            & ", PartNo: " & PartNo _
            & ", IntExt: " & IntExt _
            & ", PONo: " & PONo _
            & ", RecStatus: " & RecStatus

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSampleMtrlReq") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReq = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleMtrlReq

    Public Shared Function GetSampleMtrlReqRec(ByVal SMRNo As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_Preview"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReqRec")

            GetSampleMtrlReqRec = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqRec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSampleMtrlReqRec") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqRec : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqRec = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleMtrlReqRec

    Public Shared Function GetLastSampleMtrlReq(ByVal SampleDesc As String, ByVal RequestorTMID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal TEID As Integer, ByVal Formula As String, ByVal RecStatus As String, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Last_SampleMtrlReq"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RequestorTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestorTMID").Value = RequestorTMID

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@TEID", SqlDbType.Int)
            myCommand.Parameters("@TEID").Value = TEID

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = commonFunctions.replaceSpecialChar(Formula, False)

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetLastSampleMtrlReq")

            GetLastSampleMtrlReq = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SampleDesc: " & SampleDesc _
            & ", RequestorTMID: " & RequestorTMID _
            & ", UGNFacility: " & UGNFacility _
            & ", Customer: " & Customer _
            & ", TEID: " & TEID _
            & ", Formula: " & Formula _
            & ", RecStatus: " & RecStatus

            HttpContext.Current.Session("BLLerror") = "GetLastSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetLastSampleMtrlReq") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetLastSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetLastSampleMtrlReq = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetLastSampleMtrlReq

    Public Shared Function GetSampleMtrlReqDocuments(ByVal SMRNo As Integer, ByVal DocID As Integer, ByVal Section As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@DocID", SqlDbType.Int)
            myCommand.Parameters("@DocID").Value = DocID

            myCommand.Parameters.Add("@Section", SqlDbType.VarChar)
            myCommand.Parameters("@Section").Value = Section

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReqDocuments")

            GetSampleMtrlReqDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo _
            & ", DocID: " & DocID _
            & ", Section: " & Section

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSampleMtrlReqDocuments") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleMtrlReqDocuments
    Public Shared Function GetSampleMtrlReqPartNo(ByVal SMRNo As Integer, ByVal RowID As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_PartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReqDocuments")

            GetSampleMtrlReqPartNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo _
            & ", RowID: " & RowID

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("GetSampleMtrlReqPartNo") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqPartNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleMtrlReqPartNo


    Public Shared Function GetSampleMtrlReqRSS(ByVal SMRNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo


            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SampleMtrlReqRSS")

            GetSampleMtrlReqRSS = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqRSS") = "~/PGM/SampleMtrlReq.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqRSS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSampleMtrlReqRSS

    Public Shared Function GetSampleMtrlReqRSSReply(ByVal SMRNo As String, ByVal RSSID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ExpProj_Assets_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.VarChar)
            myCommand.Parameters("@RSSID").Value = RSSID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SampleMtrlReqRSSReply")

            GetSampleMtrlReqRSSReply = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqRSSReply") = "~/PGM/SampleMtrlReq.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqRSSReply = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSampleMtrlReqRSSReply

    Public Shared Function GetSampleMtrlReqApproval(ByVal SMRNo As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID ", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID ").Value = ResponsibleTMID

            myCommand.Parameters.Add("@PendingApprovals ", SqlDbType.Bit)
            myCommand.Parameters("@PendingApprovals ").Value = PendingApprovals

            myCommand.Parameters.Add("@RejectedTM ", SqlDbType.Bit)
            myCommand.Parameters("@RejectedTM ").Value = RejectedTM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReqApproval")

            GetSampleMtrlReqApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqApproval") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSampleMtrlReqApproval

    Public Shared Function GetSampleMtrlReqLead(ByVal SMRNo As Integer) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_Lead"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, " SampleMtrlReqLead")

            GetSampleMtrlReqLead = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value & ", SMRNo: " & SMRNo
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqLead : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqLead") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqLead : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqLead = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSampleMtrlReqLead

    Public Shared Function GetSampleMtrlReqHistory(ByVal SMRNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SampleMtrlReqHistory")

            GetSampleMtrlReqHistory = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> EXPModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqHistory") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqHistory = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSampleMtrlReqHistory

    Public Shared Function GetSampleMtrlReqShipping(ByVal SMRNo As Integer, ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SampleMtrlReq_Shipping"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetSampleMtrlReqShipping")

            GetSampleMtrlReqShipping = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqShipping : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("GetSampleMtrlReqShipping") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqShipping : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSampleMtrlReqShipping = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSampleMtrlReqShipping

    Public Shared Function InsertSampleMtrlReq(ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal SampleDesc As String, ByVal RequestorTMID As Integer, ByVal AccountMgrTMID As Integer, ByVal QualityEngrTMID As Integer, ByVal PackagingTMID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal TEID As Integer, ByVal Formula As String, ByVal IntExt As String, ByVal ProjectNo As String, ByVal DueDate As String, ByVal RecoveryType As String, ByVal ProdLevel As String, ByVal NotifyActMgr As Boolean, ByVal NotifyPkgCoord As Boolean, ByVal CreatedBy As String, ByVal CreatedOn As String) As DataSet


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@RequestorTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestorTMID").Value = RequestorTMID

            myCommand.Parameters.Add("@AccountMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AccountMgrTMID").Value = AccountMgrTMID

            myCommand.Parameters.Add("@QualityEngrTMID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngrTMID").Value = QualityEngrTMID

            myCommand.Parameters.Add("@PackagingTMID", SqlDbType.Int)
            myCommand.Parameters("@PackagingTMID").Value = PackagingTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@TEID", SqlDbType.Int)
            myCommand.Parameters("@TEID").Value = TEID

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = commonFunctions.replaceSpecialChar(Formula, False)

            myCommand.Parameters.Add("@IntExt", SqlDbType.VarChar)
            myCommand.Parameters("@IntExt").Value = IntExt

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            myCommand.Parameters.Add("@RecoveryType", SqlDbType.VarChar)
            myCommand.Parameters("@RecoveryType").Value = RecoveryType

            myCommand.Parameters.Add("@ProdLevel", SqlDbType.VarChar)
            myCommand.Parameters("@ProdLevel").Value = ProdLevel

            myCommand.Parameters.Add("@NotifyActMgr", SqlDbType.Bit)
            myCommand.Parameters("@NotifyActMgr").Value = NotifyActMgr

            myCommand.Parameters.Add("@NotifyPkgCoord", SqlDbType.Bit)
            myCommand.Parameters("@NotifyPkgCoord").Value = NotifyPkgCoord

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSampleMtrlReq")
            InsertSampleMtrlReq = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SampleDesc: " & SampleDesc

            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSampleMtrlReq") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSampleMtrlReq = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF InsertSampleMtrlReq

    Public Shared Function InsertSampleMtrlReqDocuments(ByVal SMRNo As Integer, ByVal TeamMemberID As Integer, ByVal Section As String, ByVal Description As String, ByVal BinaryFile As Byte(), ByVal FileName As String, ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_Documents"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Section", SqlDbType.VarChar)
            myCommand.Parameters("@Section").Value = Section

            myCommand.Parameters.Add("@Description", SqlDbType.VarChar)
            myCommand.Parameters("@Description").Value = commonFunctions.replaceSpecialChar(Description, False)

            myCommand.Parameters.Add("@BinaryFile", SqlDbType.VarBinary)
            myCommand.Parameters("@BinaryFile").Value = BinaryFile

            myCommand.Parameters.Add("@FileName", SqlDbType.VarChar)
            myCommand.Parameters("@FileName").Value = commonFunctions.replaceSpecialChar(FileName, True)

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.replaceSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertSampleMtrlReqDocuments")
            InsertSampleMtrlReqDocuments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo

            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("InsertSampleMtrlReqDocuments") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSampleMtrlReqDocuments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF InsertSampleMtrlReqDocuments

    Public Shared Sub InsertSampleMtrlReqRSS(ByVal SMRNo As String, ByVal SampleDesc As String, ByVal TeamMemberID As Integer, ByVal ApprovalLevel As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_RSS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@ApprovalLevel", SqlDbType.Int)
            myCommand.Parameters("@ApprovalLevel").Value = ApprovalLevel

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", SampleDesc: " & commonFunctions.replaceSpecialChar(SampleDesc, False) & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertSampleMtrlReqRSS") = "~/PGM/SampleMaterialRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertSampleMtrlReqRSS

    Public Shared Sub InsertSampleMtrlReqRSSReply(ByVal SMRNo As String, ByVal RSSID As Integer, ByVal SampleDesc As String, ByVal TeamMemberID As Integer, ByVal Comments As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_RSS_Reply"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RSSID", SqlDbType.Int)
            myCommand.Parameters("@RSSID").Value = RSSID

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", SampleDesc: " & commonFunctions.replaceSpecialChar(SampleDesc, False) & ", RSSID: " & RSSID & ", TeamMember: " & TeamMemberID & ", Comments: " & Comments & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertSampleMtrlReqRSSReply") = "~/PGM/SampleMaterialRequest.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertSampleMtrlReqRSSReply

    Public Shared Sub InsertSampleMtrlReqApproval(ByVal SMRNo As Integer, ByVal UGNFacility As String, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertSampleMtrlReqApproval") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertSampleMtrlReqApproval

    Public Shared Sub InsertSampleMtrlReqAddLvl1Aprvl(ByVal SMRNo As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal OriginalTMID As Integer, ByVal CreatedBy As String, ByVal CreatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_AddLvl1Aprvl"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@Sequence", SqlDbType.Int)
            myCommand.Parameters("@Sequence").Value = Sequence

            myCommand.Parameters.Add("@ResponsibleTMID", SqlDbType.Int)
            myCommand.Parameters("@ResponsibleTMID").Value = ResponsibleTMID

            myCommand.Parameters.Add("@OriginalTMID", SqlDbType.Int)
            myCommand.Parameters("@OriginalTMID").Value = OriginalTMID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = CreatedBy

            myCommand.Parameters.Add("@CreatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedOn").Value = CreatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertSampleMtrlReqAddLvl1Aprvl") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqAddLvl1Aprvl : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertSampleMtrlReqAddLvl1Aprvl

    Public Shared Sub InsertSampleMtrlReqHistory(ByVal SMRNo As Integer, ByVal SampleDesc As String, ByVal ActionTakenTMID As Integer, ByVal ActionDesc As String) '', ByVal FieldChange As String, ByVal PreviousValue As String, ByVal NewValue As String, ByVal ChangeReason As String

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_SampleMtrlReq_History"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@ActionTakenTMID", SqlDbType.Int)
            myCommand.Parameters("@ActionTakenTMID").Value = ActionTakenTMID

            myCommand.Parameters.Add("@ActionDesc", SqlDbType.VarChar)
            myCommand.Parameters("@ActionDesc").Value = commonFunctions.replaceSpecialChar(ActionDesc, False)

            'myCommand.Parameters.Add("@FieldChange", SqlDbType.VarChar)
            'myCommand.Parameters("@FieldChange").Value = commonFunctions.replaceSpecialChar(FieldChange, False)

            'myCommand.Parameters.Add("@PreviousValue", SqlDbType.VarChar)
            'myCommand.Parameters("@PreviousValue").Value = commonFunctions.replaceSpecialChar(PreviousValue, False)

            'myCommand.Parameters.Add("@NewValue", SqlDbType.VarChar)
            'myCommand.Parameters("@NewValue").Value = commonFunctions.replaceSpecialChar(NewValue, False)

            'myCommand.Parameters.Add("@ChangeReason", SqlDbType.VarChar)
            'myCommand.Parameters("@ChangeReason").Value = commonFunctions.replaceSpecialChar(ChangeReason, False)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", SampleDesc: " & commonFunctions.replaceSpecialChar(SampleDesc, False) & ", ActionTakenTMID: " & ActionTakenTMID & ", ActionDesc: " & ActionDesc & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("InsertSampleMtrlReqHistory") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqHistory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertSampleMtrlReqHistory

    Public Shared Function UpdateSampleMtrlReq(ByVal SMRNo As Integer, ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal SampleDesc As String, ByVal RequestorTMID As Integer, ByVal AccountMgrTMID As Integer, ByVal QualityEngrTMID As Integer, ByVal PackagingTMID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal TEID As Integer, ByVal Formula As String, ByVal IntExt As String, ByVal ProjectNo As String, ByVal DueDate As String, ByVal RecoveryType As String, ByVal ProdLevel As String, ByVal IssueDate As String, ByVal PackagingReq As String, ByVal ShipMethod As String, ByVal SpecialInstructions As String, ByVal LblReqComments As String, ByVal InvPONo As String, ByVal VoidReason As String, ByVal NotifyActMgr As Boolean, ByVal NotifyPkgCoord As Boolean, ByVal UpdatedBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_SampleMtrlReq"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@SampleDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SampleDesc").Value = commonFunctions.replaceSpecialChar(SampleDesc, False)

            myCommand.Parameters.Add("@RequestorTMID", SqlDbType.Int)
            myCommand.Parameters("@RequestorTMID").Value = RequestorTMID

            myCommand.Parameters.Add("@AccountMgrTMID", SqlDbType.Int)
            myCommand.Parameters("@AccountMgrTMID").Value = AccountMgrTMID

            myCommand.Parameters.Add("@QualityEngrTMID", SqlDbType.Int)
            myCommand.Parameters("@QualityEngrTMID").Value = QualityEngrTMID

            myCommand.Parameters.Add("@PackagingTMID", SqlDbType.Int)
            myCommand.Parameters("@PackagingTMID").Value = PackagingTMID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@TEID", SqlDbType.Int)
            myCommand.Parameters("@TEID").Value = TEID

            myCommand.Parameters.Add("@Formula", SqlDbType.VarChar)
            myCommand.Parameters("@Formula").Value = commonFunctions.replaceSpecialChar(Formula, False)

            myCommand.Parameters.Add("@IntExt", SqlDbType.VarChar)
            myCommand.Parameters("@IntExt").Value = IntExt

            myCommand.Parameters.Add("@ProjectNo", SqlDbType.VarChar)
            myCommand.Parameters("@ProjectNo").Value = ProjectNo

            myCommand.Parameters.Add("@DueDate", SqlDbType.VarChar)
            myCommand.Parameters("@DueDate").Value = DueDate

            myCommand.Parameters.Add("@RecoveryType", SqlDbType.VarChar)
            myCommand.Parameters("@RecoveryType").Value = RecoveryType

            myCommand.Parameters.Add("@ProdLevel", SqlDbType.VarChar)
            myCommand.Parameters("@ProdLevel").Value = ProdLevel

            myCommand.Parameters.Add("@IssueDate", SqlDbType.VarChar)
            myCommand.Parameters("@IssueDate").Value = IssueDate

            myCommand.Parameters.Add("@PackagingReq", SqlDbType.VarChar)
            myCommand.Parameters("@PackagingReq").Value = commonFunctions.replaceSpecialChar(PackagingReq, False)

            myCommand.Parameters.Add("@ShipMethod", SqlDbType.VarChar)
            myCommand.Parameters("@ShipMethod").Value = ShipMethod

            myCommand.Parameters.Add("@SpecialInstructions", SqlDbType.VarChar)
            myCommand.Parameters("@SpecialInstructions").Value = commonFunctions.replaceSpecialChar(SpecialInstructions, False)

            myCommand.Parameters.Add("@LblReqComments", SqlDbType.VarChar)
            myCommand.Parameters("@LblReqComments").Value = commonFunctions.replaceSpecialChar(LblReqComments, False)

            myCommand.Parameters.Add("@InvPONo", SqlDbType.VarChar)
            myCommand.Parameters("@InvPONo").Value = InvPONo

            myCommand.Parameters.Add("@VoidReason", SqlDbType.VarChar)
            myCommand.Parameters("@VoidReason").Value = commonFunctions.replaceSpecialChar(VoidReason, False)

            myCommand.Parameters.Add("@NotifyActMgr", SqlDbType.Bit)
            myCommand.Parameters("@NotifyActMgr").Value = NotifyActMgr

            myCommand.Parameters.Add("@NotifyPkgCoord", SqlDbType.Bit)
            myCommand.Parameters("@NotifyPkgCoord").Value = NotifyPkgCoord

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateSampleMtrlReq")
            UpdateSampleMtrlReq = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", SMRNo: " & SMRNo & ", SampleDesc: " & SampleDesc

            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PKGModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UpdateSampleMtrlReq") = "~/PGM/SamleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReq : " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateSampleMtrlReq = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function ' EOF UpdateSampleMtrlReq

    Public Shared Sub UpdateSampleMtrlReqApproval(ByVal SMRNo As Integer, ByVal TMID As Integer, ByVal TMSigned As Boolean, ByVal Status As String, ByVal Comments As String, ByVal SeqNo As Integer, ByVal UpdatedBy As String, ByVal UpdatedOn As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_SampleMtrlReq_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.VarChar)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@TMID", SqlDbType.Int)
            myCommand.Parameters("@TMID").Value = TMID

            myCommand.Parameters.Add("@TMSigned", SqlDbType.Bit)
            myCommand.Parameters("@TMSigned").Value = TMSigned

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = commonFunctions.replaceSpecialChar(Comments, False)

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myCommand.Parameters.Add("@UpdatedOn", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedOn").Value = UpdatedOn

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateSampleMtrlReqApproval") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReqApproval : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateSampleMtrlReqApproval

    Public Shared Sub UpdateSampleMtrlReqStatus(ByVal SMRNo As Integer, ByVal RecStatus As String, ByVal RoutingStatus As String, ByVal ShipEDICoordTMID As Integer, ByVal ShipComments As String, ByVal UpdatedBy As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_SampleMtrlReq_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SMRNo", SqlDbType.Int)
            myCommand.Parameters("@SMRNo").Value = SMRNo

            myCommand.Parameters.Add("@RecStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RecStatus").Value = RecStatus

            myCommand.Parameters.Add("@RoutingStatus", SqlDbType.VarChar)
            myCommand.Parameters("@RoutingStatus").Value = RoutingStatus

            myCommand.Parameters.Add("@ShipEDICoordTMID", SqlDbType.Int)
            myCommand.Parameters("@ShipEDICoordTMID").Value = ShipEDICoordTMID

            myCommand.Parameters.Add("@ShipComments", SqlDbType.VarChar)
            myCommand.Parameters("@ShipComments").Value = commonFunctions.replaceSpecialChar(ShipComments, False)

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = UpdatedBy

            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReqStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UpdateSampleMtrlReqStatus") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReqStatus : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateSampleMtrlReqStatus

#End Region 'EOF "Sample Material Request"

End Class
