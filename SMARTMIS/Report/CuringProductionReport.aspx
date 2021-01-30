<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuringProductionReport.aspx.cs" MasterPageFile="~/smartMISCuringReportMaster.Master" Inherits="SmartMIS.Report.CuringProductionReport" Title="Curing Production Report" %>

<%@ Register src="~/UserControl/CuringProductionReport.ascx" tagname="CuringproductionReportWCWise" tagprefix="asp" %>
<%@ Register src="~/UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>

<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">

    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />

    <script type="text/javascript" language="javascript">
    
        function showReport(queryString)
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_CuringproductionReportWCWise_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
        
    </script>
    
    <asp:HiddenField id="magicHidden" runat=server value="" />  
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    <asp:reportHeader ID="reportHeader" runat="server" />
    <asp:CuringproductionReportWCWise ID="CuringproductionReportWCWise" runat="server" />
    
 </asp:Content>