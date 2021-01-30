<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workCenterLogIn.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.manningInput" %>

<asp:Content ID="manningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <table class="inputTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="inputFirstCol"></td>
            <td class="inputSecondCol"></td>
            <td class="inputThirdCol"></td>
            <td class="inputForthCol"></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Work center Login</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">SAP Code:</td>
            <td >
                <asp:Label ID="wcLogInsapCodeLabel" runat="server"></asp:Label></td>
            <td>
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">First Name:</td>
            <td class="masterTextBox">
                <asp:Label ID="wcLogInFirstNameLabel" runat="server"></asp:Label>
             </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Last Name:</td>
            <td>
                <asp:Label ID="wcLogInLastNameLabel" runat="server"></asp:Label>
            </td>
            <td></td>
            <td></td>
        </tr>
         <tr>
            <td class="masterLabel">Gender:</td>
            <td>
                <asp:Label ID="wcLogInGenderLabel" runat="server"></asp:Label>
            </td>
            <td></td>
            <td></td>
        </tr>
         <tr>
            <td class="masterLabel">Department:</td>
            <td>
                <asp:Label ID="wcLogInDepartmentLabel" runat="server"></asp:Label>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Workcenter:</td>
            <td>
                <asp:DropDownList ID="wcLogInWCDropDownList" runat="server" Width="80%">
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="wcLogInAddButton" runat="server" CssClass="masterButton" Text="Log in" />&nbsp;
                <asp:Button ID="wcLogInCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
                </td>
            <td></td>
            <td></td>
        </tr>  
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
         
        </table>
</asp:Content>
