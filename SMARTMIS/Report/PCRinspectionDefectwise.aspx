<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="PCRinspectionDefectwise.aspx.cs" Inherits="SmartMIS.Report.PCRinspectionDefectwise" Title="PCRinspectionDefectWise Report" %>
<asp:Content ID="PCRinspectionDefectwiseContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">


<style>
    
    
.VertiColumn th{

-webkit-transform: rotate(270deg);
	-moz-transform: rotate(270deg);
	-ms-transform: rotate(270deg);
	-o-transform: rotate(270deg);
	transform: rotate(270deg);
	white-space:nowrap;
	/*display:inline-block;*/
	/*height:30px;*/
	/*float:left;*/
	/*width:10px !important;*/

}
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
    border:5px solid #888888;
    : 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
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

//document.documentElement.style.height = "100%";
//document.body.style.height = "100%";
</script>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
<table width="100%"><tr><td width="95%" align="center"><h2>PCR Visual Inspection Defect Summary Report</h2></td>
<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
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
    <div id="RadiobuttonDiv" runat="server" style="text-align:center; border-width:thin; border-style:solid">  
     <asp:RadioButton ID="BarcodeFromTOReport" Text="BarcodeFromReport" 
          GroupName="BarcodeReport" Checked="true" runat="server" 
          oncheckedchanged="BarcodeFromReport_CheckedChanged" AutoPostBack="true" />
     <asp:RadioButton ID="DateFromToReport" Text=" DateFromToWiseReport" GroupName="barcodeReport"
         runat="server"  oncheckedchanged="BarcodeFromReport_CheckedChanged" AutoPostBack="true" /> </div>
    <div id="DatefromtoDiv" visible="false" runat="server">
    <tr>
       
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
        <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:5%"> Type &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
               <asp:DropDownList ID="TypeDropDownList" Width="100%" runat="server" AutoPostBack="true" >
                 <%--  <asp:ListItem>Select</asp:ListItem>--%>
                  <%-- <asp:ListItem>BuildingWise</asp:ListItem>--%>
                   <asp:ListItem>WcWise</asp:ListItem>
                   <asp:ListItem>RecipeWise</asp:ListItem>
               </asp:DropDownList>
              
          </td>    
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:11%"> &nbsp;&nbsp;&nbsp;select category &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">
           
               <asp:DropDownList ID="ddlCategory" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FillddlFault" >
                   <asp:ListItem Value="0">Select</asp:ListItem>
                   <asp:ListItem Value="1">Minor</asp:ListItem>
                   <asp:ListItem Value="2">Major</asp:ListItem>
               </asp:DropDownList>
              
          </td>
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%"> &nbsp;&nbsp;&nbsp;select Fault &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">
           
               <asp:DropDownList ID="ddlFault" Width="100%" runat="server" AutoPostBack="true" >
                   <%-- <asp:ListItem>Select</asp:ListItem>
                    <asp:ListItem Value="1">PASS-1</asp:ListItem>
                    <asp:ListItem Value="2">Buff</asp:ListItem>
                    <asp:ListItem Value="3">Scrap</asp:ListItem>--%>
                  
               </asp:DropDownList>
              
          </td>
            <td style="font-weight:bold; font-family:Arial; font-size:small; width:8%"> &nbsp;&nbsp;&nbsp;select Size &nbsp; ::</td>
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:12%">
           
               <asp:DropDownList ID="ddlRecipe" Width="100%" runat="server" AutoPostBack="true" >
                  
               </asp:DropDownList>
              
          </td> 
        <td  style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; &nbsp; </td>
         <td  style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp;  &nbsp; </td>
           <td  style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp;  &nbsp; </td>  
         <%--  <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; select FaultArea &nbsp; ::</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
               <asp:DropDownList ID="FaultAreaDropDownList" Width="100%" runat="server" AutoPostBack="true" >
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>All</asp:ListItem>
                   <asp:ListItem>Ok</asp:ListItem>
                   <asp:ListItem>Buff</asp:ListItem>
                   <asp:ListItem>Scrap</asp:ListItem>
                   
               </asp:DropDownList>
          </td>  --%>     
            
   </tr>
   </div>
   <div id="BarcodeFromToDiv" runat="server">
     <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
          <asp:TextBox ID="BarcodeFromTextBox" runat="server" MaxLength="11" onkeypress="return onlyNumbers(event);"></asp:TextBox>
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
 <asp:Label ID="Label2" runat="server" Text="" CssClass="alrtPopup" Visible="false"></asp:Label>
     


<asp:Panel ID="gvpanel" runat="server" >
<div id="grdCharges" runat="server" style="width:auto; overflow: auto; height: 360px;">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found" EmptyDataRowStyle-HorizontalAlign="Center" 
    HeaderStyle-HorizontalAlign="Left">
            <Columns>
            </Columns>
     </asp:GridView>
     </div>
</asp:Panel>   
<asp:Label ID="Label1" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>

    </ContentTemplate>
</asp:UpdatePanel>
    
</asp:Content>
