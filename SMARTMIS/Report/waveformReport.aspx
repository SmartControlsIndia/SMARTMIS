<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="waveformReport.aspx.cs" Inherits="SmartMIS.Report.waveformReport" Title="WaveForm Report" %>


<asp:Content ID="waveformcontent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<link rel="stylesheet" href="../Style/reportMaster.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
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

 

 <style type="text/css">
    
   #headerdive
   {
       background-color:#C3D9FF;
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       
   }
      .tablecolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:10%;
       
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:100%;
       
      
   }
   
   tr
   {
      border:1pt solid black;
      
     
   }
   
   .tableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
       background-color:#C3D9FF;
       
   }
  .bder
  {
      background-color: inherit; 
   font-size:20px; 
   font-family:Arial Narrow;
   border:1pt solid black;
    border-top-color:black;
    border-width:thin;
    border-bottom-color:black;
    height:30px;
}
 
   .button
{
    cursor: pointer;    
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
 endColorstr='#579AEB'); /* for IE */
    -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
    endColorstr='#579AEB'); /* for IE 8 and above */
    background: -webkit-gradient(linear, left top, left bottom, from(#85B6F0),
    to(#579AEB)); /* for webkit browsers */
    background: -moz-linear-gradient(top, #85B6F0, #579AEB); /* for firefox 3.6+ */
    background: -o-linear-gradient(top, #85B6F0, #579AEB); /* for Opera */
    width:100PX;
}
   
     .style1
     {
         width: 17%;
     }
   
 </style>
 <asp:TextBox ID="txtwcin" style="display:none" runat="server"></asp:TextBox>
<%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
  <asp:reportHeader ID="reportHeader" runat="server" />
<table width="100%" class="bder">
<tr>
<td class="tablecolumn">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date From :</td>
<td class="tablecolumn"><myControl:calendertextbox ID="fromdatecalendertextbox" runat="server" Width="80%" Disabled="false" /> </td>

<td class="tablecolumn"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date To :</td>
<td class="tablecolumn"><myControl:calendertextbox ID="TodateCalendertextbox" runat="server" Width="80%" Disabled="false"/></td>
                             
                             
                             
<td class="tablecolumn">
    <asp:Button ID="ViewButton" CssClass="button"  runat="server" Text="View " OnDataBound = "OnDataBound" 
        onclick="viewReport_Click"/>
    </td>
    &nbsp;<%--<td visible=false>
 
    <asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
    </td>--%>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>

</tr>

   </table>
   
   </br>
  <asp:Label ID="ShowWarning" runat="server" Text="" CssClass="alrtPopup" Visible="false"></asp:Label>
  <div align="center" style="width:1039px;; " visible="false">
<asp:Label ID="lblText" runat="server" Visible="False" BackColor="Gray"></asp:Label>
</div>
<asp:Panel ID="QualityPanel" runat="server" ScrollBars="Both" Height="100%"  CssClass="panel" >
 <div style="height:400px;width:1244px; overflow: scroll;">
           <asp:GridView ID="MainGridView" runat="server"  CssClass="TBMTable " HeaderStyle-HorizontalAlign="Left" EmptyDataRowStyle-BackColor="Gray" 
            ShowHeader="true"  EmptyDataRowStyle-Width="70%" 
             EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound = "GridView_RowDataBound" Width="1244px"
             EmptyDataText="No Records Found" 
            ShowFooter="false" >
            <Columns>
             <%--<asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
            </Columns>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
           </asp:GridView>
           </div>
  </asp:Panel>
</asp:Content>
