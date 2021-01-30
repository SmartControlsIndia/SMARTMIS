<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReportOAY.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.PerformanceReportOAY" Title="Performance OAY Report" %>

<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

<%@ Register src="~/UserControl/OAYGRAFReport.ascx" TagName="OAYFilter" tagprefix="asp" %>


<asp:Content ID="plantPerformanceReportContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript" language="javascript">
        function showReport(queryString) {
        
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_reportHeader_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_OAYFilter_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = '';
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_OAYFilter_viewQueryHidden').value = "True";
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
        function setTyreType() 
        {

            var tyreType = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_OAYFilter_tyreTypeHidden').value;
            var result = document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_OAYFilter_tuoFilterResultDropDownList').value;
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_tuoReportMasterContentPalceHolder_tyreTypeHidden').value = tyreType + '?' + result;
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        } 

    </script>
  
    <asp:ScriptManager ID="TuoRejectionDetailScriptManager" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    
    <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
    
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:reportHeader ID="reportHeader" runat="server" />
    
    <asp:OAYFilter ID="OAYFilter" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>