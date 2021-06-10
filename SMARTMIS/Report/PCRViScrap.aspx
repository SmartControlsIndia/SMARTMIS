<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" CodeBehind="PCRViScrap.aspx.cs" EnableEventValidation="false" Inherits="SmartMIS.Report.PCRViScrap" Title="PCRVISCRAP" %>

<asp:Content ID="PCRVIScrapReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <style type="text/css">
.links
{
    text-decoration:none;
    color:#0000EE;
    font-family:Verdana;
	font-weight: bold;
	font-size:12px;
	text-align:left;
	padding:2px;
}
.links:hover
{
      text-decoration:underline;      
}
    .close {
	background-color: #4C4C4C;
    background: -moz-linear-gradient(top, #272727, #4C4C4C);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#272727), to(#4C4C4C));
	color: #FFFFFF;
	line-height: 25px;
	position: absolute;
	right: -12px;
	text-align: center;
	top: -10px;
	width: 24px;
	text-decoration: none;
	font-weight: bold;
	-webkit-border-radius: 12px;
	-moz-border-radius: 12px;
	border-radius: 12px;
  .box-shadow;
  .transition;
  .transition-delay(.2s);
  &:hover { background: #00d9ff; .transition; }
}
    .close:hover
    {
        background-color: #272727;
    background: -moz-linear-gradient(top, #4C4C4C, #272727);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#4C4C4C), to(#272727));
        }
.dialogPanelCSS
{
    padding:12px;
    left:10%;
    top:50px;
    z-index:2000;
    position:fixed;
    background-color: #FF9933;
    background: -moz-linear-gradient(top, #C5DEE1, #E8EDFF);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C5DEE1), to(#E8EDFF));
    -webkit-border-radius: 5px;
	-moz-border-radius: 5px;
	border-radius: 5px;
    }
    .saveLink {
  padding:5px;
  background-color: #FF9933;
  background: -moz-linear-gradient(top, #FCAE41, #FF9933);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#FCAE41), to(#FF9933));
  border: 1px solid #666;
  color:#000;
  text-decoration:none;
   font-weight:bold;
}
.alrtPopup
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
<script>
    function hideDialog() {
        $('#ctl00_masterContentPlaceHolder_dialogPanel').fadeOut(1500);
        $('#ctl00_masterContentPlaceHolder_backDiv').fadeOut(1500);
    }
    setTimeout(function() {
    $('.alrtPopup').fadeOut(1500);
}, 4000);

</script>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />


<table width="100%"><tr><td width="95%" align="center"><h2 style="color:#376d9c;">PCR VI Scrap Report</h2></td>
<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click" ToolTip="Download Scrap Report"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
</td></tr></table>

<asp:ScriptManager ID= "PCRVIScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
<div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="PopupUpdatePanel">
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
    
 <asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
 <asp:UpdatePanel ID="PopupUpdatePanel" runat="server">
    <ContentTemplate>
<asp:Label ID="ShowWarning" runat="server" Text="" CssClass="alrtPopup" Visible="false"></asp:Label>
   <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #6ba5d6;" align="center">
    <tr>
       
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
        <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">  &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
              
          </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp;  &nbsp; ::</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
          </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:11%">          
              Status Type:
               <asp:DropDownList ID="displayType" runat="server" AutoPostBack="True" Width="100">
                   <asp:ListItem Selected="True">SCRAP</asp:ListItem>
                   
               </asp:DropDownList>
        </td>    
   </tr>
</table>

    <asp:GridView ID="VIRecipeWiseGridView" runat="server" CssClass="TBMTable" AutoGenerateColumns="true"
    Width="100%" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="False" >   
          
    </asp:GridView> 

<asp:Label ID="backDiv" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>

    
    <asp:Label ID="Label1" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
    <asp:Panel ID="dialogPanel" Visible="false" runat="server" Width="80%" Height="480" CssClass="dialogPanelCSS">
    <a href="javascript:void();" class="close" OnClick="hideDialog()">X</a>
    <asp:Label ID="emptyMsg" runat="server" Visible="false" Text="<center><h2>No Data To Display</h2></center>"></asp:Label>

   
    </asp:Panel>
   
    </ContentTemplate>
</asp:UpdatePanel>
    

<asp:Panel ID="ExcelPanel" runat="server">
    <asp:GridView Width="100%" ID="ExcelGridView" runat="server" AutoGenerateColumns="false" onrowdatabound="GridView_RowDataBound">
           
    </asp:GridView>
</asp:Panel>


<%--</asp:UpdatePanel>
    
                <asp:Panel ID="ExcelPanel" runat="server">
                <asp:Label ID="ExcelLabel" runat="server" Text=""></asp:Label>
            </asp:Panel>--%>

</asp:Content>