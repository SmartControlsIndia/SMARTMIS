

<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="UniformityReport.aspx.cs" Inherits="SmartMIS.Report.UniformityReport" Title="Uniformity Classification Report" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<link rel="stylesheet" href="../Style/reportMaster.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />


 

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
    &nbsp;<td>
 
    <asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
    </td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>

</tr>

   </table>
   
   </br>
   
  <div align="center" style="width:100%; " visible="false">
<asp:Label ID="lblText" runat="server" Visible="False" BackColor="Gray"></asp:Label>
</div>
 <div style="height:400px; overflow: scroll;">
           <asp:GridView ID="MainGridView" runat="server"  CssClass="TBMTable " HeaderStyle-HorizontalAlign="Left" EmptyDataRowStyle-BackColor="Gray" 
            ShowHeader="true"  EmptyDataRowStyle-Width="100%" 
             EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound = "GridView_RowDataBound" Width="100%"
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
  
</asp:Content>
