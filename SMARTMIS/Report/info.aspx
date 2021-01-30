<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="info.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.info" %>

<asp:Content ID="SmartfinalfinishContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">

    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
     <asp:Label ID="MessageViewLabel" runat="server"></asp:Label>
    <asp:Label ID="summarylabel" runat="server" Text="Label"></asp:Label>
     <asp:Panel runat="server" ScrollBars="Vertical" Height="300">
    <asp:Label ID="InfoDataViewLabel" runat="server"></asp:Label></asp:Panel>
    
</asp:Content>