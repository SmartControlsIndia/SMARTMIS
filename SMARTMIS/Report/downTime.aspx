<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="downTime.aspx.cs" EnableEventValidation="false" MasterPageFile="~/smartMISReportMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.downTimeReport" Title="Downtime Report" %>

<%@ Register src="../UserControl/downTimeReportWCWise.ascx" tagname="downTimeReport" tagprefix="asp" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>

<asp:Content ID="dtReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/downtimeReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript" language="javascript">
        function showReport(queryString)
        {
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_downTimeReportWCWise_magicHidden').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_reportHeader_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
    </script>
    
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:reportHeader ID="reportHeader" runat="server" />
    <asp:downTimeReport ID="downTimeReportWCWise" runat="server" />
</asp:Content>
