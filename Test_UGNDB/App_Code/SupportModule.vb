''************************************************************************************************
''Name:		SupportModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Accounts Receivable Module
''
''Date		     Author	    
'' 12/06/2011    Roderick Carlson			Created .Net application
'' 01/28/2013    Roderick Carlson           Modified: Do actual delete of supporting documents and add stored procedure references for approvals
'' 02/13/2013    Roderick Carlson           Modified: Replaced most direct select statements with stored procedures
'' 04/17/2013    Roderick Carlson           Modified: Set Default Status to be open instead of in-process
Imports Microsoft.VisualBasic

Public Class SupportModule

    Public Shared Sub DeleteSupportCookies()

        Try
            HttpContext.Current.Session("BLLerror") = Nothing

            HttpContext.Current.Response.Cookies("SupportModule_SaveJobNumberSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveJobNumberSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveStatusSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveStatusSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveCategoryIDSearch").Value = 0
            HttpContext.Current.Response.Cookies("SupportModule_SaveCategoryIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveRelatedToSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveRelatedToSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveRequestBySearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveRequestBySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveModuleIDSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveModuleIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveJobDescriptionSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveJobDescriptionSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("SupportModule_SaveAssignedToSearch").Value = ""
            HttpContext.Current.Response.Cookies("SupportModule_SaveAssignedToSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteSupportCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSupportCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GetAssignedTo() As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Get_JRF_AssignedTo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString

            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            ''get list of team members with Admin rights to the UGNDB Team Member Maint page
            'Dim strSQL As String = "SELECT distinct tm.Working, tm.LastName, tm.FirstName, r.TeamMemberID, (CASE tm.Working WHEN 0 THEN '** ' ELSE '' END) +  (tm.LastName + ', ' + tm.FirstName) AS ddTeamMemberName  FROM TeamMember_RoleForm r LEFT OUTER JOIN TeamMember_Maint tm ON r.TeamMemberID = tm.TeamMemberID WHERE(r.FormID = 70 And r.RoleID = 11) ORDER BY tm.Working DESC, tm.LastName, tm.FirstName"

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssignedToList")
            GetAssignedTo = GetData

        Catch ex As Exception
            GetAssignedTo = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssignedTo: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssignedTo: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function GetModule(ByVal DBMID As String, ByVal Desc As String) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Get_JRF_Module"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString            
            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            'Dim strSQL As String = "Select DBMID, CASE WHEN Obsolete = 1 THEN '** ' + Description ELSE Description END AS Description FROM DB_Modules WHERE DBMID <> '' ORDER BY Obsolete, Description"

            'If DBMID <> "" Then
            '    strSQL &= " AND DBMID = '" + DBMID + "'"
            'End If

            'If Desc <> "" Then
            '    strSQL = " AND Description = '" + Desc + "'"
            'End If

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DBMID", SqlDbType.VarChar)
            myCommand.Parameters("@DBMID").Value = DBMID

            If Desc Is Nothing Then
                Desc = ""
            End If

            myCommand.Parameters.Add("@Desc", SqlDbType.VarChar)
            myCommand.Parameters("@Desc").Value = Desc

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ModuleList")
            GetModule = GetData

        Catch ex As Exception
            GetModule = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetModule: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetModule: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function GetCategory(ByVal DBCID As String, ByVal Category As String) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Get_JRF_Category"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            'Dim strSQL As String = "Select DBCID, Category FROM DB_Category WHERE DBCID <> '' "

            'If DBCID <> "" Then
            '    strSQL &= " AND DBCID = '" + DBCID + "'"
            'End If

            'If Category <> "" Then
            '    strSQL = " AND Category = '" + Category + "'"
            'End If

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DBCID Is Nothing Then
                DBCID = ""
            End If

            myCommand.Parameters.Add("@DBCID", SqlDbType.VarChar)
            myCommand.Parameters("@DBCID").Value = DBCID

            If Category Is Nothing Then
                Category = ""
            End If

            myCommand.Parameters.Add("@Category", SqlDbType.VarChar)
            myCommand.Parameters("@Category").Value = Category

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CategoryList")
            GetCategory = GetData
        Catch ex As Exception
            GetCategory = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCategory: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCategory: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function GetSupportSearch(ByVal JobNumber As String, ByVal DBCID As Integer, ByVal DBMID As String, _
        ByVal Status As String, ByVal RelatedTo As String, ByVal RequestBy As String, _
        ByVal JobDescription As String, ByVal AssignedTo As String) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Get_JRF_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            'Dim strSQL As String = "SELECT jrf.jnId,jrf.JobNumber,jrf.DBCID,dbc.Category,jrf.DBMID,dbm.[Description] As Module,CONVERT(VARCHAR,jrf.RequestDate,101) As RequestDate,jrf.RequestBy,jrf.AssignedTo,ISNULL(jrf.EstimatedHours,0) As EstimatedHours,CONVERT(VARCHAR,jrf.StartDate,101) As StartDate,ISNULL(jrf.ActualHours,0) AS ActualHours,CONVERT(VARCHAR,jrf.DateCompleted,101) As DateCompleted,CONVERT(VARCHAR(50),jrf.JobDescription) + '...' AS JobDescription,jrf.Notes,jrf.[Status] ,jrf.Email ,jrf.PgmHlpDesk AS RelatedTo FROM JRF jrf  LEFT OUTER JOIN DB_Category dbc ON jrf.DBCID=dbc.DBCID  LEFT OUTER JOIN DB_Modules dbm ON jrf.DBMID=dbm.DBMID WHERE jrf.JnId > 0 "

            'If DBCID > 0 Then
            '    strSQL &= " AND jrf.DBCID = " & DBCID & " "
            '    'strSQL &= " AND jrf.DBCID = 1 "
            'End If

            'If JobNumber <> "" Then
            '    strSQL &= " AND jrf.JobNumber LIKE '" & JobNumber & "' "
            'End If

            'If DBMID <> "" Then
            '    strSQL &= " AND jrf.DBMID = '" & DBMID & "' "
            'End If

            'If Status <> "" Then
            '    strSQL &= " AND jrf.Status = '" & Status & "' "
            'End If

            'If RelatedTo <> "" Then
            '    strSQL &= " AND jrf.PgmHlpDesk = '" & RelatedTo & "' "
            'End If

            'If RequestBy <> "" Then
            '    strSQL &= " AND jrf.RequestBy LIKE '" & RequestBy & "' "
            'End If

            'If JobDescription <> "" Then
            '    strSQL &= " AND jrf.JobDescription LIKE '" & JobDescription & "' "
            'End If

            'If AssignedTo <> "" Then
            '    strSQL &= " AND jrf.AssignedTo LIKE '" & AssignedTo & "' "
            'End If


            'strSQL &= " ORDER BY jrf.jnId DESC "

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If JobNumber Is Nothing Then
                JobNumber = ""
            End If

            myCommand.Parameters.Add("@JobNumber", SqlDbType.VarChar)
            myCommand.Parameters("@JobNumber").Value = JobNumber

            myCommand.Parameters.Add("@DBCID", SqlDbType.Int)
            myCommand.Parameters("@DBCID").Value = DBCID

            If DBMID Is Nothing Then
                DBMID = ""
            End If

            myCommand.Parameters.Add("@DBMID", SqlDbType.VarChar)
            myCommand.Parameters("@DBMID").Value = DBMID

            If Status Is Nothing Then
                Status = ""
            End If

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar)
            myCommand.Parameters("@Status").Value = Status

            If RelatedTo Is Nothing Then
                RelatedTo = ""
            End If

            myCommand.Parameters.Add("@PgmHlpDesk", SqlDbType.VarChar)
            myCommand.Parameters("@PgmHlpDesk").Value = RelatedTo

            If RequestBy Is Nothing Then
                RequestBy = ""
            End If

            myCommand.Parameters.Add("@RequestBy", SqlDbType.VarChar)
            myCommand.Parameters("@RequestBy").Value = RequestBy

            If JobDescription Is Nothing Then
                JobDescription = ""
            End If

            myCommand.Parameters.Add("@JobDescription", SqlDbType.VarChar)
            myCommand.Parameters("@JobDescription").Value = JobDescription

            If AssignedTo Is Nothing Then
                AssignedTo = ""
            End If

            myCommand.Parameters.Add("@AssignedTo", SqlDbType.VarChar)
            myCommand.Parameters("@AssignedTo").Value = AssignedTo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "JobList")
            GetSupportSearch = GetData
        Catch ex As Exception
            GetSupportSearch = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "JobNumber: " & JobNumber _
            & ", DBCID: " & DBCID _
            & ", DBMID: " & DBMID _
            & ", Status: " & Status _
            & ", RelatedTo: " & RelatedTo _
            & ", RequestBy: " & RequestBy _
            & ", JobDescription: " & JobDescription _
            & ", AssignedTo: " & AssignedTo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSupportSearch: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportSearch: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function GetSupportRequest(ByVal JobNumber As String) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Get_JRF"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            '' get specific support requestor
            'Dim strSQL As String = "SELECT jrf.jnId, jrf.JobNumber, jrf.DBCID, dbc.Category, jrf.DBMID, dbm.[Description] As Module, CONVERT(VARCHAR, jrf.RequestDate,101) As RequestDate, jrf.RequestBy, jrf.AssignedTo, ISNULL(jrf.EstimatedHours,0) As EstimatedHours, CONVERT(VARCHAR,jrf.StartDate,101) As StartDate, ISNULL(jrf.ActualHours,0) AS ActualHours, CONVERT(VARCHAR, jrf.DateCompleted,101) As DateCompleted, jrf.JobDescription, jrf.Notes,jrf.[Status] , jrf.Email , jrf.PgmHlpDesk AS RelatedTo FROM JRF jrf  LEFT OUTER JOIN DB_Category dbc ON jrf.DBCID=dbc.DBCID  LEFT OUTER JOIN DB_Modules dbm ON jrf.DBMID=dbm.DBMID WHERE JnId > 0 "

            '' must always have a JobNumber
            'strSQL &= " AND JobNumber = '" & JobNumber & "'"

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If JobNumber Is Nothing Then
                JobNumber = ""
            End If

            myCommand.Parameters.Add("@JobNumber", SqlDbType.VarChar)
            myCommand.Parameters("@JobNumber").Value = JobNumber

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "JobRequest")
            GetSupportRequest = GetData

        Catch ex As Exception
            GetSupportRequest = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "JobNumber: " & JobNumber _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupportRequest : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportRequest : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function GetSupportingDoc(ByVal RowID As Integer, ByVal jnId As Integer) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        '' get specific support requestor
        'Dim strSQL As String = "SELECT RowID, jnId, SupportingDocBinary, SupportingDocName, SupportingDocDesc, SupportingDocEncodeType, SupportingDocBinarySizeInBytes, Obsolete, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn FROM JRF_Supporting_Doc WHERE Obsolete = 0 AND JnId > 0 "

        '' must always have a JobNumber
        'strSQL &= "AND RowID = " & RowID.ToString & " AND jnId = " & jnId.ToString & ""

        'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
        Dim strStoredProcName As String = "sp_Get_JRF_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "JobSupportingDoc")
            GetSupportingDoc = GetData

        Catch ex As Exception
            GetSupportingDoc = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID.ToString _
            & "jnId: " & jnId.ToString _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    'Public Shared Function GetSupportingDocList(ByVal jnId As Integer) As DataSet

    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter

    '    Try
    '        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

    '        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

    '        ' get specific support requestor
    '        Dim strSQL As String = "SELECT RowID, jnId, SupportingDocName, SupportingDocDesc, SupportingDocEncodeType, SupportingDocBinarySizeInBytes, ISNULL(isSignatureReq,0) AS isSignatureReq, Obsolete, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn FROM JRF_Supporting_Doc WHERE Obsolete = 0 AND JnId > 0 "

    '        ' must always have a JobNumber
    '        strSQL &= " AND jnId = " & jnId.ToString & ""

    '        Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "JobSupportingDocList")
    '        GetSupportingDocList = GetData

    '    Catch ex As Exception
    '        GetSupportingDocList = Nothing

    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "jnId: " & jnId.ToString _
    '        & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetSupportingDocList : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '    End Try

    'End Function

    Public Shared Sub InsertSupportingDoc(ByVal jnId As Integer, ByVal SupportingDocName As String, _
        ByVal SupportingDocDesc As String, ByVal SupportingDocBinary As Byte(), _
        ByVal SupportingDocBinarySizeInBytes As Integer, ByVal SupportingDocEncodeType As String, _
        ByVal isSignatureReq As Boolean)


        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Insert_JRF_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            If SupportingDocName Is Nothing Then
                SupportingDocName = ""
            End If

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = commonFunctions.replaceSpecialChar(SupportingDocName, False)

            If SupportingDocDesc Is Nothing Then
                SupportingDocDesc = ""
            End If

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = commonFunctions.replaceSpecialChar(SupportingDocDesc, False)

            myCommand.Parameters.Add("@SupportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@SupportingDocBinary").Value = SupportingDocBinary

            myCommand.Parameters.Add("@SupportingDocBinarySizeInBytes", SqlDbType.Int)
            myCommand.Parameters("@SupportingDocBinarySizeInBytes").Value = SupportingDocBinarySizeInBytes

            If SupportingDocEncodeType Is Nothing Then
                SupportingDocEncodeType = ""
            End If

            myCommand.Parameters.Add("@SupportingDocEncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocEncodeType").Value = commonFunctions.replaceSpecialChar(SupportingDocEncodeType, False)

            myCommand.Parameters.Add("@isSignatureReq", SqlDbType.Bit)
            myCommand.Parameters("@isSignatureReq").Value = isSignatureReq

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId _
            & ", SupportingDocName:" & SupportingDocName _
            & ", SupportingDocDesc:" & SupportingDocDesc _
            & ", SupportingDocBinarySizeInBytes:" & SupportingDocBinarySizeInBytes _
            & ", SupportingDocEncodeType:" & SupportingDocEncodeType _
            & ", isSignatureReq:" & isSignatureReq _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupportingDocument : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSupportingDocument : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteSupportingDoc(ByVal jnId As Integer, ByVal RowID As Integer, ByVal original_RowID As Integer)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            '' get specific support requestor
            ''Dim strSQL As String = "UPDATE JRF_Supporting_Doc SET OBSOLETE = 1 WHERE JnId = " & jnId.ToString & " AND RowID = " & original_RowID.ToString
            'Dim strSQL As String = "DELETE FROM JRF_Supporting_Doc WHERE JnId = " & jnId.ToString & " AND RowID = " & original_RowID.ToString

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
            Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            Dim strStoredProcName As String = "sp_Delete_JRF_Supporting_Doc"
            Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId.ToString _
            & ", RowID: " & original_RowID.ToString _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupportDocument : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSupportDocument : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Sub UpdateLastUsedRequestID(ByVal LastUsedSupportRequestID As Integer, ByVal DBMID As String)

        Try
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
            Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            Dim strSQL As String = ""

            strSQL = "UPDATE DB_Sequence_M Set LastUsedNo = " & LastUsedSupportRequestID & " WHERE Prefix = '" & DBMID & "' or SeqDesc = 'DB Requests'"
            Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetNewSupportRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetNewSupportRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GetNewSupportRequestID() As DataSet

        Dim iReturnVal As Integer = 0

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
            Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            Dim strSQL As String = "SELECT LastUsedNo FROM DB_Sequence_M Where SeqDesc = 'DB Requests'"

            Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewSupportRequestID")
            GetNewSupportRequestID = GetData
        Catch ex As Exception
            GetNewSupportRequestID = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetNewSupportRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetNewSupportRequestID : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Function InsertSupportDetail(ByVal DBCID As Integer, ByVal DBMID As String, _
        ByVal RequestBy As String, ByVal JobDescription As String, _
        ByVal PgmHlpDesk As String) As String

        Dim retVal As String = ""

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString

            Dim dsNewSupportRequest As DataSet
            Dim iNewSupportRequestID As Integer = 0

            dsNewSupportRequest = GetNewSupportRequestID()

            If commonFunctions.CheckDataSet(dsNewSupportRequest) Then
                If dsNewSupportRequest.Tables(0).Rows(0).Item("LastUsedNo") IsNot System.DBNull.Value Then
                    If dsNewSupportRequest.Tables(0).Rows(0).Item("LastUsedNo") > 0 Then
                        iNewSupportRequestID = dsNewSupportRequest.Tables(0).Rows(0).Item("LastUsedNo")

                        'increment ID
                        iNewSupportRequestID += 1

                        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

                        Dim strSQL As String = "insert into JRF (jnId, JobNumber, DBCID, DBMID, RequestDate, RequestBy, JobDescription, [Status], PgmHlpDesk) values "
                        strSQL &= " ( " & iNewSupportRequestID.ToString _
                        & ", '" & DBMID & "-" & iNewSupportRequestID.ToString() & " ' " _
                        & ", " & DBCID & " " _
                        & ", '" & DBMID & " ' " _
                        & ", '" & Today.Date.ToString & "' " _
                        & ", '" & RequestBy & " ' " _
                        & ", '" & commonFunctions.convertSpecialChar(JobDescription, False) & " ' " _
                        & ", 'Open' " _
                        & ", '" & PgmHlpDesk & " ') "

                        Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
                        myConnection.Open()
                        myCommand.ExecuteNonQuery()

                        'need to update LastUsedID
                        UpdateLastUsedRequestID(iNewSupportRequestID, DBMID)

                        retVal = DBMID & "-" & iNewSupportRequestID.ToString()

                    End If
                End If
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DBMID: " & DBMID _
            & ", DBCID: " & DBCID _
            & ", RequestBy: " & RequestBy _
            & ", JobDescription: " & JobDescription _
            & ", PgmHlpDesk: " & PgmHlpDesk _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupportDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSupportDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        Return retVal

    End Function

    Public Shared Function UpdateSupportDetail(ByVal jnId As Integer, ByVal DBCID As Integer, ByVal DBMID As String, _
     ByVal RequestBy As String, ByVal AssignedTo As String, ByVal JobDescription As String, _
     ByVal Notes As String, ByVal Status As String, ByVal PgmHlpDesk As String, _
     ByVal ActualHours As Double, ByVal EstimatedHours As Double) As String

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim retVal As String = ""

        Try
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
            Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            Dim strSQL As String = "UPDATE JRF SET JobNumber = '" & DBMID & "-" & jnId.ToString & "' " _
            & ", DBCID = " & DBCID.ToString _
            & ", DBMID = '" & DBMID & "' " _
            & ", RequestBy = '" & RequestBy & "'"

            If AssignedTo <> "" Then
                strSQL &= ", AssignedTo = '" & AssignedTo & "'"
            End If

            strSQL &= ", JobDescription = '" & commonFunctions.convertSpecialChar(JobDescription, False) & "'" _
            & ", Notes = '" & commonFunctions.convertSpecialChar(Notes, False) & "'" _
            & ", [Status] = '" & Status & "'" _
            & ", PgmHlpDesk = '" & PgmHlpDesk & "'"

            If Status = "Closed" Or Status = "Completed" Then
                strSQL &= ", DateCompleted = '" & Today.Date.ToString & "'"
            End If

            strSQL &= ", ActualHours = " & ActualHours.ToString
            strSQL &= ", EstimatedHours = " & EstimatedHours.ToString

            strSQL &= " WHERE jnId = " & jnId

            Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            myConnection.Open()
            myCommand.ExecuteNonQuery()

            retVal = DBMID & "-" & jnId.ToString

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId _
            & ", DBCID: " & DBCID _
            & ", DBMID: " & DBMID _
            & ", RequestBy: " & RequestBy _
            & ", AssignedTo: " & AssignedTo _
            & ", JobDescription: " & JobDescription _
            & ", Notes: " & Notes _
            & ", Status: " & Status _
            & ", PgmHlpDesk: " & PgmHlpDesk _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSupportDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSupportDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        Return retVal

    End Function

    Public Shared Sub DeleteSupportRequest(ByVal jnId As Integer)

        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

        Dim strStoredProcName As String = "sp_Delete_JRF"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            'Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
            'Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            ''remove request
            'Dim strSQL As String = "DELETE FROM JRF WHERE jnId = " & jnId & " "

            'Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)
            'myConnection.Open()
            'myCommand.ExecuteNonQuery()

            ''remove supporting docs
            'strSQL = "DELETE FROM JRF_Supporting_Doc WHERE jnId = " & jnId & " "

            'Dim myCommand2 As SqlCommand = New SqlCommand(strSQL, myConnection)
            ''myConnection.Open()
            'myCommand2.ExecuteNonQuery()
          
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupportRequest : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSupportRequest : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

    End Sub

    Public Shared Function GetTeamMemberByString(ByVal SearchString As String) As DataSet

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString

            Dim myConnection As SqlConnection = New SqlConnection(strConnectionString)

            Dim strSQL As String = "SELECT DISTINCT TeamMemberID FROM UGNDB.dbo.TeamMember_Maint  WHERE LastName + ', ' + FirstName = '" & SearchString & "'"

            Dim myCommand As SqlCommand = New SqlCommand(strSQL, myConnection)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TeamMemberInfo")
            GetTeamMemberByString = GetData
        Catch ex As Exception
            GetTeamMemberByString = Nothing

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTeamMemberByString : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMemberByString : " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function

    Public Shared Sub DeleteSupportRequestApproval(ByVal RowID As Integer, ByVal original_RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Delete_JRF_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupportRequestApproval: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSupportRequestApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetSupportRequestApproval(ByVal jnId As Integer, ByVal RoutingLevel As Integer, ByVal TeamMemberID As Integer, ByVal StatusID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Get_JRF_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            myCommand.Parameters.Add("@RoutingLevel", SqlDbType.Int)
            myCommand.Parameters("@RoutingLevel").Value = RoutingLevel

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupportApprovals")
            GetSupportRequestApproval = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId _
            & ", RoutingLevel:" & RoutingLevel _
            & ", TeamMemberID:" & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupportRequestApproval: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportRequestApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupportRequestApproval = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub InsertSupportRequestApproval(ByVal jnId As Integer, ByVal TeamMemberID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Insert_JRF_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@jnId", SqlDbType.Int)
            myCommand.Parameters("@jnId").Value = jnId

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "jnId: " & jnId _
            & ", TeamMemberID:" & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupportRequestApproval: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportRequestApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub UpdateSupportRequestApproval(ByVal TeamMemberID As Integer, ByVal RoutingLevel As Integer, _
        ByVal Comments As String, ByVal StatusID As String, ByVal original_RowID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Update_JRF_Approval"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = original_RowID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@RoutingLevel", SqlDbType.Int)
            myCommand.Parameters("@RoutingLevel").Value = RoutingLevel

            If Comments Is Nothing Then
                Comments = ""
            End If

            myCommand.Parameters.Add("@Comments", SqlDbType.VarChar)
            myCommand.Parameters("@Comments").Value = Comments

            myCommand.Parameters.Add("@StatusID", SqlDbType.Int)
            myCommand.Parameters("@StatusID").Value = StatusID

            'If StatusDate Is Nothing Then
            '    StatusDate = ""
            'End If

            'myCommand.Parameters.Add("@StatusDate", SqlDbType.VarChar)
            'myCommand.Parameters("@StatusDate").Value = StatusDate

            'If NotificationDate Is Nothing Then
            '    NotificationDate = ""
            'End If

            'myCommand.Parameters.Add("@NotificationDate", SqlDbType.VarChar)
            'myCommand.Parameters("@NotificationDate").Value = NotificationDate

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", RoutingLevel:" & RoutingLevel _
            & ", Comments:" & Comments _
            & ", StatusID:" & StatusID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSupportRequestApproval: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSupportRequestApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function GetSupportRequestApprovalStatus() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnRequestor").ToString
        Dim strStoredProcName As String = "sp_Get_JRF_Approval_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SupportApprovalStatus")
            GetSupportRequestApprovalStatus = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupportRequestApprovalStatus: " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SupportModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSupportRequestApprovalStatus: " & commonFunctions.convertSpecialChar(ex.Message, False), "SupportModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSupportRequestApprovalStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
End Class
