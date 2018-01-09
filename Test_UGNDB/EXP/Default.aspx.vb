Partial Class EXP_Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            If ViewState("pProjNo") = Nothing Then
                m.ContentLabel = "Example of Drill Downs"
            Else
                m.ContentLabel = "Example of Drill Downs"
            End If

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pProjNo") = Nothing Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > New Packaging Expenditure"
                Else
                    If ViewState("pAprv") = 0 Then
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > Packaging Expenditure"
                    Else 'Go Back To approval Screen
                        lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > <a href='PackagingExpProjList.aspx'><b>Packaging Expenditure Search</b></a> > <a href='crExpProjPackagingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a> > Packaging Expenditure"
                    End If
                End If
                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("SPRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            'If Not Page.IsPostBack Then
            '    BindCriteria()
            'End If

            'System.Threading.Thread.Sleep(3000)
            'Label33.Text = DateTime.Now()

        Catch ex As Exception
            'update error on web page


            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Protected Sub BindCriteria()

        Try
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            Dim ds As DataSet = New DataSet


            ' ''bind existing data to drop down Customer control for selection criteria for search
            ''ds = commonFunctions.GetMake(Nothing)
            ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ''    ddMakes.DataSource = ds
            ''    ddMakes.DataTextField = ds.Tables(0).Columns("MakeName").ColumnName.ToString()
            ''    ddMakes.DataValueField = ds.Tables(0).Columns("MakeName").ColumnName.ToString()
            ''    ddMakes.DataBind()
            ''    ddMakes.Items.Insert(0, "")
            ''End If

            ''bind existing data to drop down Customer control for selection criteria for search
            'ds = commonFunctions.GetPlatformOEMMfgByMake(ddMake.SelectedValue)
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddOEMMfg.DataSource = ds
            '    ddOEMMfg.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            '    ddOEMMfg.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            '    ddOEMMfg.DataBind()
            '    ddOEMMfg.Items.Insert(0, "")
            'End If



        Catch ex As Exception
            'update error on web page

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindCriteria

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("Default.aspx", False)

    End Sub
End Class
