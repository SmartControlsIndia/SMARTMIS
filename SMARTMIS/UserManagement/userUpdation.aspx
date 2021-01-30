<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userUpdation.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.userUpdation" %>

<asp:Content ID="userupdationContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="Style/userupdation.css" type="text/css" charset="utf-8" />
<link rel="SHORTCUT ICON" href="Images/favicon.ico" />

    <table class="updationTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="updationFirstCol">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td class="updationSecondCol"></td>
            <td class="updationThirdCol"></td>
            <td class="updationForthCol"></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="updationHeader">
                    <p class="updationHeaderTagline">What do you change about this account</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="updationLabel">First Name :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="firstNameTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator Display="None" ID="FNRequiredFieldValidator" runat="server" 
                    ControlToValidate="firstNameTextBox" ErrorMessage="First Name is required."></asp:RequiredFieldValidator>
           
           
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Last Name :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="lastNameTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="LNRequiredFieldValidator" runat="server" 
                    ControlToValidate="lastNameTextBox" Display="None" ErrorMessage="Last Name is required."></asp:RequiredFieldValidator>
              
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Gender :</td>
            <td class="updationTextBox">
                <asp:DropDownList ID="genderDropDownList" Width="82%" runat="server">
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">DOB :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="DOBTextBox" runat="server" Width="80%"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="DOBRequiredFieldValidator" runat="server" 
                    ControlToValidate="DOBTextBox" Display="None" 
                    ErrorMessage="DOBRequiredFieldValidator">Date of Birth is required.</asp:RequiredFieldValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Address :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="addressTextBox" TextMode="MultiLine" Rows="3" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Contact No. :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="contactNoTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Email ID : </td>
            <td class="updationTextBox">
                <asp:TextBox ID="emailTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
             <asp:RequiredFieldValidator ID="EmailRequiredFieldValidator3" Display="None" 
                    runat="server" ControlToValidate="emailTextBox" 
                   ErrorMessage="Email is required!" Font-Size="XX-Small"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="emailTextBox" ErrorMessage="RegularExpressionValidator" 
                    Font-Size="XX-Small" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
           
            
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">Department :</td>
            <td class="updationTextBox">
                <asp:DropDownList ID="departmentDropDownList" Width="82%" runat="server">
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">SAP Code :</td>
            <td class="updationTextBox">
                <asp:TextBox ID="sapCodeTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="SAPRequiredFieldValidator" runat="server" 
                    ControlToValidate="sapCodeTextBox" Display="None" 
                    ErrorMessage="Last Name is required."></asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="updationLabel">
                &nbsp;</td>
            <td>
                <asp:Button ID="signInButton" runat="server" CssClass="updationButton" 
                    Text="Save" />&nbsp;
                <asp:Button ID="cancelButton" runat="server" CssClass="updationButton" Text="Cancel" />
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