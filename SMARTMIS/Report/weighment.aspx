<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weighment.aspx.cs" MasterPageFile="~/smartMISReportMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Report.weighment" Title="Weighment Report" %>

<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>
<%@ Register src="../UserControl/weighmentReport.ascx" tagname="weighmentReport" tagprefix="asp" %>

<asp:Content ID="weighmentReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/downtimeReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript" language="javascript">
        function showReport(queryString)
        {
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_weighmentReport_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
    </script>
    
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:reportHeader ID="reportHeader" runat="server" />
    <asp:weighmentReport ID="weighmentReport" runat="server" />
    
</asp:Content>
