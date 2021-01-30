<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityPerformanceSpecReport.aspx.cs" MasterPageFile="~/NewTUOReportMaster.Master" Inherits="SmartMIS.TUO.QualityPerformanceSpecReport" Title="Performance Report Spec Wise" %>
 

<%@ Register Src="~/UserControl/QualityperformanceSpacReport.ascx" TagName="performanceReport2" TagPrefix="asp" %>

<asp:Content ID="plantPerformanceReport2Content" runat="server" ContentPlaceHolderID="NewtuoReportMasterContentPalceHolder">
    
    &nbsp;<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <script type="text/javascript" language="javascript">
        function showReport(queryString) {
        
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_reportHeader_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = '';
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_viewQueryHidden').value = "True";
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
        function setTyreType() 
        {

            var tyreType = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_tyreTypeHidden').value;
            var result = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_performanceReport2_tuoFilterRep2ResultDropDownList').value;
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = tyreType + '?' + result;
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        } 

    </script><asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:performanceReport2 ID="performanceReport2" runat="server"/>
   
</asp:Content>