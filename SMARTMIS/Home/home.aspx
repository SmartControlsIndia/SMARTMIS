<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="SmartMIS.Home.home" Title="Production Summary" Codebehind="home.aspx.cs" %>
<%@ Register Src="~/UserControl/dailySummaryReportControl.ascx" TagName="WebUserControl" TagPrefix="ucl" %>


  
<asp:Content ID="homeContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
<link rel="Stylesheet" href="../Style/home.css" type="text/css" charset="utf-8" />

    <style type="text/css" >
.homemenu {
    float: left;
    width: 100%;
    padding: 0;
    margin: 0;
    list-style-type: none;
}

.homemenulink {
    float: left;
    width: 6em;
    text-decoration: none;
    color: white;
    background-color: #165188;
    padding: 0.2em 0.6em;
    border-right: 1px solid white;
}

a.homemenulink:hover {
    background-color: #15497C;
}

li.homemenulist {
    display: inline;
}
.charttable
{
    border-collapse:collapse;
    width:100%;
}

<%--#primary_nav_wrap ul ul li {
    float: none;
    width: 220px;
    z-index: 999;
}
#primary_nav_wrap ul ul {
    display: none;
    position: absolute;
    top: 100%;
    left: 0;
    background: ##15497C;
    padding: 0;
    z-index: 1000px;
    overflow-y: scroll;
    height: 500px;
}--%>
</style>



  <%--  <script language="javascript" type="text/javascript">
        function setDescription(description) {
            document.getElementById('<%= this.descriptionLabel.ClientID %>').innerHTML = description.toString();
        }
    </script>--%>
    
    <script type="text/javascript"> 
            $(document).ready(function(){

            //Larger thumbnail preview 

$("ul.thumb li").hover(function() {
	$(this).css({'z-index' : '10'});
	$(this).find('img').addClass("hover").stop()
		.animate({
			marginTop: '-110px', 
			marginLeft: '-110px', 
			top: '50%', 
			left: '50%', 
			width: '174px', 
			height: '174px',
			padding: '20px' 
		}, 200);
	
	} , function() {
	$(this).css({'z-index' : '0'});
	$(this).find('img').removeClass("hover").stop()
		.animate({
			marginTop: '0', 
			marginLeft: '0',
			top: '0', 
			left: '0', 
			width: '100px', 
			height: '100px', 
			padding: '5px'
		}, 400);
});

//Swap Image on Click
	$("ul.thumb li a").click(function() {
		
		var mainImage = $(this).attr("href"); //Find Image Name
		$("#main_view img").attr({ src: mainImage });
		return false;		
	});

});

</script>
<asp:ScriptManager ID="HomeScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="HomeUpdatePanel">
        <ProgressTemplate>
        <div class="backDiv">             
             <div align="center" class="waitBox">
             
<div id="bookG">
<div id="blockG_1" class="blockG">
</div>
<div id="blockG_2" class="blockG">
</div>
<div id="blockG_3" class="blockG">
</div>
</div>

<br />

<h2><font color="#888888">Loading, please wait............</font> </h2> 
</div>
</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>

<asp:UpdatePanel ID="HomeUpdatePanel" runat="server">
<ContentTemplate>
    <table class="homeTable" align="center" cellpadding="0" cellspacing="0" border="1">
        <tr>
            <td class="homeFirstCol"></td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                <tr align="left">
                <td width="50%">
                <ul class="homemenu">
                    <li class="homemenulist">
                        <asp:LinkButton ID="TBR" runat="server" class="homemenulink" OnClick="TBR_Click">TBR</asp:LinkButton></li>
                    <li class="homemenulist">
                        <asp:LinkButton ID="PCR" runat="server" class="homemenulink" OnClick="PCR_Click">PCR</asp:LinkButton></li>
                </ul></td>
                <td width="20%"><strong><font face="Verdana" size="2">Today's Production</font></strong></td>
                <td width="30%" align="right"><strong><font face="Verdana" size="2">
                    <asp:Label ID="TBMProductionLabel" runat="server" Text=""></asp:Label> | 
                    <asp:Label ID="CURProductionLabel" runat="server" Text=""></asp:Label>
                </font></strong></td>
                </tr>
                </table>
                <asp:Label ID="descriptionLabel" Visible="false" runat="server" Text="Click on option" CssClass="descriptionLabel"></asp:Label>
                
            </td>
        </tr>        
    </table>
        <asp:Panel ID="TBRPanel" runat="server" Visible="false">
            <table border="1" class="charttable">
                <tr align="center">
                    <td>
                <font face="Verdana" size="2"><strong>TBM TBR WorkCenterwise</strong></font>
                <div>
                   
                    <asp:Chart ID="TBMTBRChart" runat="server" Width="400px">
                        <Series>
                            <asp:Series Name="TBMTBRSeries">
                            </asp:Series>
                        </Series>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="true" Name="Default" LegendStyle="Row" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="TBMTBRChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                </td>
                <td>
                <font face="Verdana" size="2"><strong>TBM TBR Recipewise</strong></font>
                <div>
                   
                    <asp:Chart ID="TBMTBRRecipeChart" runat="server" Width="400px">
                        <Series>
                            <asp:Series Name="TBMTBRRecipeSeries">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="TBMTBRRecipeChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                </td>
                </tr>
                <tr>
            <td colspan="2" align="center">
                <font face="Verdana" size="2"><strong>TBR Curing</strong></font>
                <div>
                   
                    <asp:Chart ID="TBRCURChart" runat="server" Width="1000px">
                        <Series>
                            <asp:Series Name="TBRCURSeries">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="TBRCURChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </td>
           
        </tr>
                </table>
                </asp:Panel>
           <asp:Panel ID="PCRPanel" runat="server" Visible="false">
           <table class="charttable" border="1">
                <tr align="center">
                <td>
                    <font face="Verdana" size="2"><strong>TBM PCR WorkCenterwise</strong></font>
                <div>
                   
                    <asp:Chart ID="TBMPCRChart" runat="server" Width="400px">
                        <Series>
                            <asp:Series Name="TBMPCRSeries">
                            </asp:Series>
                        </Series>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="TBMPCRChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </td>
            <td>
                <font face="Verdana" size="2"><strong>TBM PCR Recipewise</strong></font>
                <div>
                   
                    <asp:Chart ID="TBMPCRRecipeChart" runat="server" Width="600px">
                        <Series>
                            <asp:Series Name="TBMPCRRecipeSeries">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="TBMPCRRecipeChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <font face="Verdana" size="2"><strong>PCR Curing</strong></font>
                <div>
                   
                    <asp:Chart ID="PCRCURChart" runat="server" Width="1200px">
                        <Series>
                            <asp:Series Name="PCRCURSeries">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="PCRCURChartArea">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </td>
           
        </tr>
        </table>
        </asp:Panel>
        <table>
        
        <tr>
            <td></td>
            
        </tr>
        <tr>
            <%--<td colspan="3">
                <ucl:WebUserControl ID="dailySummaryReport" runat="server" />
            </td>--%>
        </tr>        
        <tr>
            <td>&nbsp;</td>
            </tr>        
    </table>
        
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

