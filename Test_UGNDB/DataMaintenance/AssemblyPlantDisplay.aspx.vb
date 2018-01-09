' ************************************************************************************************
' Name:		AssemblyPlantDisplay.aspx
' Purpose:	This Code Behind is for the Supplier Request Look Up page. This page will be called from
'           various modules to allow team members to search or request new suppliers and include unapproved
'           suppliers as (f) future vendors in the drop down lists.
'
' Date		    Author	    
' 05/26/2011    LRey			Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_AssemblyPlantDisplay
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.lookupmasterpage_master = Master
            ' ''check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "OEM Model Types by Assembly Plant"
                mpTextBox.Font.Size = 18
                mpTextBox.Visible = True
                mpTextBox.Font.Bold = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAPID") <> "" Then
                ViewState("pAPID") = HttpContext.Current.Request.QueryString("pAPID")
            Else
                ViewState("pAPID") = ""
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindData()
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ViewState("pAPID") <> Nothing Then
                'bind data
                ds = commonFunctions.GetAssemblyPlantLocation(ViewState("pAPID"), "", "", "", "")
                If commonFunctions.CheckDataSet(ds) = True Then
                    If (ds.Tables.Item(0).Rows.Count > 0) Then
                        lblAssembly.Text = ds.Tables(0).Rows(0).Item("Assembly_Plant_Location").ToString()
                        lblStateVal.Text = ds.Tables(0).Rows(0).Item("State").ToString()
                        lblCountryVal.Text = ds.Tables(0).Rows(0).Item("Country").ToString()
                        lblOEMManufacturerVal.Text = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                        lblUGNBiz.Text = IIf(ds.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
                    End If
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData

    Protected Sub gvAPLOEM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim OEMModelType As String
            Dim drOEMModelType As AssemblyPlantLocation.Assembly_Plant_OEMRow = CType(CType(e.Row.DataItem, DataRowView).Row, AssemblyPlantLocation.Assembly_Plant_OEMRow)

            If DataBinder.Eval(e.Row.DataItem, "OEMModelType") IsNot DBNull.Value Then
                OEMModelType = drOEMModelType.OEMModelType
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsAPLPartOEM"), ObjectDataSource)

                ' Set the CategoryID Parameter value
                rpCBRC.SelectParameters("APID").DefaultValue = drOEMModelType.APID.ToString()
                rpCBRC.SelectParameters("ModelName").DefaultValue = drOEMModelType.ModelName.ToString()
                rpCBRC.SelectParameters("OEMModelType").DefaultValue = drOEMModelType.OEMModelType.ToString()
                rpCBRC.SelectParameters("PARTNO").DefaultValue = Nothing
                rpCBRC.SelectParameters("CPART").DefaultValue = Nothing
                rpCBRC.SelectParameters("COMPNY").DefaultValue = Nothing
                rpCBRC.SelectParameters("PRCCDE").DefaultValue = Nothing
            End If
        End If
    End Sub 'EOF gvAPLOEM_RowDataBound
End Class
