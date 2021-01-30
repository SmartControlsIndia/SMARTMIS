<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TyreWeighmentReport.aspx.cs" MasterPageFile="~/DMYOptionMaster.Master" Inherits="SmartMIS.Report.TyreWeighmentReport" %>
<%@ MasterType VirtualPath="~/DMYOptionMaster.Master"%>
<asp:Content ID="TyreWeighmentReportContentID" ContentPlaceHolderID="DMYMasterContentPlaceHolder" runat="server" Visible="true" >

<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />

<style>
.ErrorMsgCSS
{
    padding:7px;width:30%;max-width:30%;height:auto;
    position:fixed;
    z-index: 1080;top:75px;left: 35%;
    -moz-border-radius: 15px;-webkit-border-radius: 15px;border-radius:15px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background:#fff8c4 10px 50%;
    border:1px solid #f2c779;
    color:#555;
    font: bold 12px verdana;
}
</style>
<script type="text/javascript">
    function closePopup() {
        setTimeout(function() {
            $('.ErrorMsgCSS').fadeOut(1500);
        }, 4000);

    }
    setTimeout(function() {
        $('.ErrorMsgCSS').fadeOut(1500);
    }, 4000);
</script>
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
<asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="ErrorMsgCSS" Visible="false"></asp:Label>
    
   <asp:Label ID="HeaderText" runat="server" Text=""></asp:Label>



<table width="100%"><tr><td width="95%" align="center"><h2>Tyres Weighment Report</h2></td>
<%--<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
--%></td></tr></table>

<%--<asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
--%>
<asp:UpdatePanel ID="weighmentUpdatepanel" runat="server">
<ContentTemplate>

 <asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" OnDataBound = "OnDataBound" runat="server" AutoGenerateColumns="true" onrowdatabound="GridView_RowDataBound">
     <Columns>
            
     </Columns>
    </asp:GridView>
    </asp:Panel>

    <center>
    <asp:Chart ID="TBMChart" runat="server" Width="1100px">
        <Series>
            <asp:Series Name="TBMSeries">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="TBMChartArea">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>



</ContentTemplate>


</asp:UpdatePanel>



















</asp:Content>
