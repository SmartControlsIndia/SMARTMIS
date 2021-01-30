<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="matarialConsumption.aspx.cs" MasterPageFile="../smartMISReportMaster.Master" Inherits="SmartMIS.matarialConsumptionReport" Title="Material Consumption Report" %>
<%@ Register Src="../UserControl/matConsumptionControl.ascx" TagName="matConsump" TagPrefix="uc2" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>


<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/matarialconsumptionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    
      <script type="text/javascript" language="javascript">
        function showReport(queryString)
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_matConsumpUC_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
    </script>
    
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    <asp:reportHeader ID="reportHeader" runat="server" />
    <uc2:matConsump ID="matConsumpUC" runat="server" />
     
</asp:Content>


