' ************************************************************************************************
'
' Name:		crViewExpProjToolingAuth
' Purpose:	This Code Behind is for the Tooling Authorization Previews in Crystal Reports
'
' Date		    Author	    
' 09/21/2012    Roderick Carlson    Created

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class crViewExpProjToolingAuth
    Inherits System.Web.UI.Page
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim ds As DataSet
            Dim oRpt As ReportDocument = New ReportDocument()

            Dim iTANo As Integer = 0
            Dim iArchiveData As Integer = 0

            Dim strFormType As String = ""

            Dim bContinue As Boolean = True

            If HttpContext.Current.Request.QueryString("TAProjectNo") <> "" Then

                ViewState("TAProjectNo") = HttpContext.Current.Request.QueryString("TAProjectNo")

                ViewState("TANo") = 0
                If InStr(ViewState("TAProjectNo"), "U2", CompareMethod.Binary) > 0 Then
                    ViewState("TANo") = CType(Replace(ViewState("TAProjectNo"), "U", ""), Integer)
                End If

                ViewState("FormType") = "TA"
                If HttpContext.Current.Request.QueryString("FormType") IsNot Nothing Then
                    ViewState("FormType") = HttpContext.Current.Request.QueryString("FormType").ToString
                End If

                ViewState("ArchiveData") = 0
                If HttpContext.Current.Request.QueryString("ArchiveData") IsNot Nothing Then
                    If HttpContext.Current.Request.QueryString("ArchiveData").ToString <> "" Then
                        ViewState("ArchiveData") = CType(HttpContext.Current.Request.QueryString("ArchiveData"), Integer)
                    End If
                End If


                'check to see if TA exists and is NOT void
                If ViewState("TANo") > 0 Then
                    ds = TAModule.GetTA(ViewState("TANo"))
                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("StatusID") = 4 Then
                                bContinue = False
                            End If
                        End If
                    End If
                End If
                
                'should be open, in-process, complete, or from the old system
                If bContinue = True Then
                    If Session("TAPreviewTANo") <> ViewState("TANo") Then
                        Session("TAPreview") = Nothing
                        Session("TAPreviewTANo") = Nothing
                    End If


                    If (Session("TAPreview") Is Nothing) Then
                        ' new report document object 
                        If ViewState("FormType") = "TA" Then

                            If ViewState("TANo") >= 200000 Then
                                oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjToolingAuth.rpt")
                            Else
                                oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjToolingAuthArchive.rpt")
                            End If
                        Else

                            If ViewState("TANo") >= 200000 Then
                                oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjToolingAuthDieshop.rpt")
                            Else
                                oRpt.Load(Server.MapPath(".\Forms\") & "crExpProjToolingAuthDieshopArchive.rpt")
                            End If

                        End If

                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)

                        'TANo is used for the new module, TAProjectNo must be used for the old module since there could be any text
                        'TANo is used as the true unique ID in the new module and the function call is shared between the PDF preview and the Details
                        If ViewState("TANo") > 0 Then
                            oRpt.SetParameterValue("@TANo", ViewState("TANo"))
                        Else
                            oRpt.SetParameterValue("@TANo", ViewState("TAProjectNo"))
                        End If

                        Session("TAPreviewTANo") = ViewState("TANo")
                        Session("TAPreview") = oRpt

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=" & ViewState("FormType") & "-" & ViewState("TAProjectNo").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    Else
                        oRpt = CType(Session("TAPreview"), ReportDocument)
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("content-disposition", "inline;filename=" & ViewState("FormType") & "-" & ViewState("TAProjectNo").ToString & "preview.pdf")
                        Response.BinaryWrite(oStream.ToArray())
                    End If
                Else
                    lblMessage.Text = "<br><br><br>The Tooling Authorization has been voided or no longer exists."
                End If

            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("TAPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TAPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TAPreview") = Nothing
            HttpContext.Current.Session("TAPreviewTANo") = Nothing
            GC.Collect()
        End If

    End Sub


End Class
