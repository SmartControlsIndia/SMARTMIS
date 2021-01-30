<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="performanceReportSizeWise.aspx.cs" MasterPageFile="~/NewTUOReportMaster.Master" EnableEventValidation="false"  MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.TUO.performanceReportSizeWise" Title="Performance Report TBM MACHINE WISE" %>

<%@ Register src="../UserControl/tuoFilter.ascx" TagName="tuoFilter" tagprefix="asp" %>




<asp:Content ID="plantPerformanceReportContent" runat="server" ContentPlaceHolderID="NewtuoReportMasterContentPalceHolder">


    

    &nbsp;<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>

    <asp:tuoFilter ID="tuoFilter" runat="server" />
   

   
</asp:Content>