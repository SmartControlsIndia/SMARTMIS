<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="classificationReport2.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.classificationReport2" Title="Smart MIS - TBR Visual Inspection Classifiation Report" %>

<asp:Content ID="TBRVISummaryReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
<script type="text/javascript" language="javascript" src="../Script/download.js"></script>
<style>
.button:hover
{
    background-color: #15497C;
    background: -moz-linear-gradient(top, #15497C, #2384D3);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#15497C), to(#2384D3));
}
.close {
	background: #606061;
	color: #FFFFFF;
	line-height: 25px;
	position: absolute;
	right: -10px;
	text-align: center;
	top: -12px;
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
</style>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />

<table width="100%"><tr><td width="95%" align="center"><h2>TBR Visual Inspection Classifiation Report</h2></td>
<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
</td></tr></table>
<asp:ScriptManager ID="TBRVI2ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="curingOperatorPlanningUpdatePanel">
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
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
       
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:4%">&nbsp;&nbsp;Date::</td>
        <td style="width: 8%">
        <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" CssClass="button" onclick="ViewButton_Click" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:12%"> 
          Display Type: 
          <asp:DropDownList ID="displayType" runat="server" AutoPostBack="True" 
              onselectedindexchanged="displayType_SelectedIndexChanged" Width="50%">
              <asp:ListItem Selected="True">Numbers</asp:ListItem>
              <asp:ListItem>Percent</asp:ListItem>
          </asp:DropDownList>
        </td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  
 </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
               &nbsp;</td>    
   </tr>
</table>
<asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound">
            <Columns>
            </Columns>
        </asp:GridView>
</asp:Panel> 

<asp:Panel ID="ExcelPanel" runat="server">
    <asp:GridView Width="100%" ID="ExcelGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound">
            <Columns>
            </Columns>
    </asp:GridView>
</asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>
    
</asp:Content>