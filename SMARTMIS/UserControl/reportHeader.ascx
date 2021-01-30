<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="reportHeader.ascx.cs" Inherits="SmartMIS.UserControl.reportHeader" %>

<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<asp:HiddenField id="magicHidden" runat="server" value="" />

<table class="innerTable" cellpadding="0" cellspacing="0" style="border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;">
    <tr>
        <td style="width:20%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;">
            <div style="height: 60px; text-align: center;">                    
                <img id="reportHeaderLogo" alt="CEAT" class="ceatImg" src="../Images/logo_jk.jpg" />
            </div>
        </td>
        <td style="width:60%; font-weight:bold; font-size: xx-large; font-variant:small-caps; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" rowspan="2">
            <asp:Label ID="reportHeaderWCName" runat="server" Text="" ></asp:Label>
        </td>
        <td class="masterLabel" style="width:10%; text-align:center; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" >
            <asp:Label ID="reportHeaderDate" runat="server" Text="Date :" ></asp:Label>
        </td>
        <td class="masterLabel" style="width:10%; text-align:center; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" >
            <asp:Label ID="reportHeaderDateLabel" runat="server" Text="####" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="masterLabel" style="text-align:center; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;">
            Chennai
        </td>
        <td class="masterLabel" style="text-align:center; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;">
        </td>
        <td style="border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;">
        </td>
    </tr>
</table>
