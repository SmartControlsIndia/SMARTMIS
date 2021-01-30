<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="performanceReport2SizeWise.aspx.cs" MasterPageFile="~/NewTUOReportMaster.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.TUO.performanceReport2SizeWise" Title="Performance Report2 Size wise" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<%@ Register Src="~/UserControl/QualityperformancereportTBMWise.ascx" TagName="performanceReport2" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/QualityPerformanceReportTBMRecipeWise.ascx" TagName="performanceReport3" TagPrefix="asp" %>

<asp:Content ID="plantPerformanceReport2Content" runat="server" ContentPlaceHolderID="NewtuoReportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
     <script type="text/javascript" language="javascript">
        function showReport(queryString) {
                
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_reportHeader_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport3_magicHidden').value = queryString.toString();

            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = '';
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_viewQueryHidden').value = "True";
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport3_viewQueryHidden').value = "True";
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
        function setTyreType() 
        {
            var tyreType = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_tyreTypeHidden').value;
            var result = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_tuoFilterRep2ResultDropDownList').value;
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = tyreType + '?' + result;
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        } 

    </script>
    
     <script type="text/javascript" language="javascript">
         function Show() 
         {
             document.getElementById("performanceReport2").style.display = "none";
         }
         function Hide() 
         {
             document.getElementById("performanceReport2").style.display = "none";
         }
    </script>
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:reportHeader ID="reportHeader"  runat="server" />
     <table cellpadding="0" cellspacing="0" style="width: 60%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
       <td style="width: 8%"></td>
        <td style="width: 12%"></td>
         <td style="width: 8%"></td>
        <td style="width: 10%"></td>
        <td style="width: 10%"></td>
    </tr>
    <tr>
     <td class="masterLabel">
    
        Select Date</td>
     <td  class="masterLabel" style="border-width:medium">
      <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="80%" />     
    </td>
     <td  class="masterLabel">
         <asp:Button ID="ViewButton" runat="server" Text="View Report" 
             onclick="ViewButton_Click" />
    </td>
     <td  class="masterLabel">
            <asp:RadioButton ID="PerformanceReportMachineWiseRadioButton" runat="server" 
                AutoPostBack="True" Checked="True" GroupName="reportType" 
                oncheckedchanged="PerformanceReportMachineWiseRadioButton_CheckedChanged" 
                Text="MachineWise" />
       </td>
     <td style="font-weight:bold; font-family:Arial; font-size:small"> 
            <asp:RadioButton ID="PerformanceReportRecipeWiseRadioButton" runat="server" 
                Text="RecipeWise" AutoPostBack="True" GroupName="reportType" 
                oncheckedchanged="PerformanceReportRecipeWiseRadioButton_CheckedChanged" />
        </td>    
    </tr>
</table>
     
     
     <asp:performanceReport2 ID="performanceReport2"  runat="server"/>
         
     <asp:performanceReport3 ID="performanceReport3"  runat="server"/>
     

   
</asp:Content>