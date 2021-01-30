<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="home" Title="Production Summary" Codebehind="home.aspx.cs" %>
<%@ Register Src="../UserControl/dailySummaryReportControl.ascx" TagName="WebUserControl" TagPrefix="ucl" %>


<asp:Content ID="homeContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/home.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript" language="javascript" src="../Script/ddaccordion.js">
    
    </script> 
    
    <script language="javascript" type="text/javascript">
        function setDescription(description) {
            document.getElementById('<%= this.descriptionLabel.ClientID %>').innerHTML = description.toString();
        }
    </script>
    
    <script type="text/javascript" src="../Script/jquery-latest.js"></script> 
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

    <table class="homeTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="homeFirstCol"></td>
            <td class="homeSecondCol"></td>
            <td class="homeThirdCol"></td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="descriptionLabel" runat="server" Text="Click on option" CssClass="descriptionLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div class="homeDiv">
                    <ul class="thumb">
                        <li><a href="#" onmouseover="javascript:setDescription('View')" onmouseout="javascript:setDescription('Click on option')"><img src="../Images/schedule.png" alt="" /></a></li>
                        <li><a href="#" onmouseover="javascript:setDescription('Production Planning')" onmouseout="javascript:setDescription('Click on option')"><img src="../Images/downTime.png" alt="" /></a></li>
                        <li><a href="#" onmouseover="javascript:setDescription('OEE Report')" onmouseout="javascript:setDescription('Click on option')"><img src="../Images/oee.png" alt="" /></a></li>
                        <li><a href="#" onmouseover="javascript:setDescription('Tyre Genealogy')" onmouseout="javascript:setDescription('Click on option')"><img src="../Images/geonology.png" alt="" /></a></li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <div class="homeHeader">
                    <p class="homeHeaderTagline">Common Information</p>
                </div>
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <%--<td colspan="3">
                <ucl:WebUserControl ID="dailySummaryReport" runat="server" />
            </td>--%>
        </tr>        
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>        
    </table>
</asp:Content>

