<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="RUNOUTTESTREPORT.aspx.cs" Inherits="SmartMIS.Report.RUNOUTTESTREPORT" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
 <style>
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

    function onlyNumbers(event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }


</script>


 
<script>
    function hideDialog() {
        $('#ctl00_masterContentPlaceHolder_dialogPanel').fadeOut(1500);
        $('#ctl00_masterContentPlaceHolder_backDiv').fadeOut(1500);
    }
    setTimeout(function() {
    $('.alrtPopup').fadeOut(1500);
}, 4000);

//document.documentElement.style.height = "100%";
//document.body.style.height = "100%";
</script>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
<table width="100%"><tr><td width="95%" align="center"><h2>TUO BarCode Wise Report</h2></td>
<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" 
        onclick="tbExportToexcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
</td></tr></table>
<asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
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
 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
 <div id="DatefromtoDiv" visible="false" runat="server">
 <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
      <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%"/></td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
      <td style="width: 8%">
      <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
      <asp:Button ID="ViewButton" runat="server" CausesValidation="false" Text="View Report" onclick="ViewButton_Click"/>&nbsp;</td>             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%"> select Machine &nbsp; ::</td>      
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">         
       <asp:DropDownList ID="MachineDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>All</asp:ListItem>
                   <asp:ListItem>TRO1</asp:ListItem>
                   
               </asp:DropDownList>            
       </td>   
       
   </tr>
  </div>
    <div id="BarcodeFromToDiv" runat="server">
     <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
          <asp:TextBox ID="BarcodeFromTextBox" runat="server" MaxLength="10" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
      <td style="width: 8%">
          <asp:TextBox ID="barcodeToTextBox" runat="server" MaxLength="3" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
      <asp:Button ID="BarcodeWiseButton" runat="server" CausesValidation="false" Text="View Report" onclick="ViewButton_Click"/>&nbsp;</td>             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">  &nbsp; </td>      
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">         
       </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; &nbsp; </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
        </td>       
   </tr>
    
   </div>

   
</table>
<asp:Panel ID="Panel1" runat="server" Width=1250PX ScrollBars="Both" Height=500PX >

<%--<table style="width:240%">
        <tr>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">MachineName</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">DateTime</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RecipeCode</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">BarCode </td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">Total_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_Deg</td>        
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_N</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_Deg</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_Rank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_Rank</td>  
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CON_N </td>         
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CON_Rank </td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">PLY_N</td>      
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">PLY_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">TotalRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RORank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OARank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOARank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeAmount</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeAngle</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentAmount</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentAngle</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentAmount</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentAngle</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticAmount</td>               
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticAngle</td>               
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticRank</td>               
        </tr>
</table>--%>
<asp:Panel ID="gvpanel" runat="server">
<asp:GridView Width="100%" CssClass="TBMTable" ID="RunoutMainGridView" runat="server" AutoGenerateColumns="False">
 <Columns>           
 </Columns>
</asp:GridView>
</asp:Panel>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>


<asp:Panel ID="ExcelPanel" runat="server">
<asp:GridView Width="100%" ID="ExcelGridView" runat="server" AutoGenerateColumns="False">
<Columns>
</Columns>
</asp:GridView>
</asp:Panel>
</asp:Content>
