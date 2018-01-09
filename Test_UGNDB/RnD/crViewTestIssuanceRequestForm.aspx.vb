' ************************************************************************************************
' Name:	TestIssuanceRequestPreview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 02/13/2009    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class RnD_crViewTestIssuanceRequestForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "Test Issuance Request Form Preview"
        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Research and Development</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > Test Issuance Request Form Preview"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*****
        ''Expand menu item
        ''*****
        Dim testMasterPanel As CollapsiblePanelExtender
        testMasterPanel = CType(Master.FindControl("RnDExtender"), CollapsiblePanelExtender)
        testMasterPanel.Collapsed = False

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pReqID") = HttpContext.Current.Request.QueryString("pReqID")

        If ViewState("pReqID") <> "" Then
            Try
                Dim oRpt = New ReportDocument()
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crTestIssuanceRequestForm.rpt")
                    Session("TempCrystalRptFiles") = oRpt


                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)

                    oRpt.SetParameterValue("@RequestID", ViewState("pReqID"))
                    'oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)
                    Session("TempCrystalRptFiles") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=R&D Test Issuance-" & ViewState("pReqID").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                Else
                    oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)
                    CrystalReportViewer1.ReportSource = oRpt
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=R&D Test Issuance-" & ViewState("pReqID").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                End If
            Catch ex As Exception
                lblErrors.Text = "Error found in report view. " & ex.Message
                lblErrors.Visible = "True"
            End Try
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'in order to clear crystal reports
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If
    End Sub
End Class
