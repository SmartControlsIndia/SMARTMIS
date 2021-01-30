<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SmartMIS._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<link rel="stylesheet" href="Style/masterPage.css" type="text/css" charset="utf-8" />
<link rel="stylesheet" href="Style/default.css" type="text/css" charset="utf-8" />
<link rel="SHORTCUT ICON" href="Images/favicon.ico" />

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="defaultHead" runat="server">
    <title>Smart MIS - Login</title>
    
</head>
<body>
    <form id="logInForm" runat="server">
        <table align="center" class="loginTable">
        <tr>
            <td class="loginFirstCol" rowspan="2">
                <img alt="Smart MIS" src="Images/logo.png" class="logoImage" />
            </td>
            <td class="loginSecondCol">
                <div style="text-align: right">                    
                <img alt="JK" class="custImage" src="Images/logo_jk.jpg" /></div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="loginHeader">
                    
                <p class="loginHeaderTagline">&nbsp;</p></div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table align="center" class="loginInnerTable">
                    <tr>
                        <td class="loginInnerTableFirstCol"></td>
                        <td class="loginInnerTableSecondCol"></td>
                        <td class="loginInnerTableThirdCol"></td>
                        <td class="loginInnerTableForthCol"></td>                        
                    </tr>
                    <tr>
                        <td rowspan="3">
                            <div class="loginInnerDiv">
                                <table align="center" class="loginInnerDivTable">
                                <tr>
                                    <td class="loginInnerDivFirstCol"></td>
                                    <td class="loginInnerDivSecondCol"></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="loginInnerDivHeaderTagline">Sign in with <b>Smart MIS</b> account</div>
                                     </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        User Name:</td>
                                    <td>
                                        <asp:TextBox ID="loginUserNameTextBox" runat="server" CssClass="loginTextBox" AutoComplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="loginPasswordTextBox" runat="server" TextMode="Password" CssClass="loginTextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                        
                                        
                                    </td>
                                    <td>
                                        <asp:Button ID="loginButton" runat="server" CssClass="signInButton"  Text="Sign in" 
                                        onclick="Button_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">&nbsp;
                                        <asp:Label ID="loginErrorLabel" runat="server" CssClass="invalidMessageLabel" Visible="False"></asp:Label>                            
                                    </td>                              
                                </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td>
                            <img alt="Manage Production Planning" src="Images/Planning.png" class="image"  />
                        </td>
                        <td>
                            <div class="logInHeaderDiv">
                                Implements Tyre Genealogy</div>
                            <div class="logInContentDiv">
                                Track the complete history of Tyres manufactured.</div>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <img alt="Get usable reports" src="Images/reports.png" class="image" />
                        </td>
                        <td>
                            <div class="logInHeaderDiv">
                                Gets the usable reports
                            </div>
                            <div class="logInContentDiv">
                                                                View Reports for better planning and 
                                decision making.</div>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                         <img alt="Users" src="Images/Users.png" class="image" /></td>
                        <td>
                            <div class="logInHeaderDiv">
                                Ease of creating users
                            </div>
                            <div class="logInContentDiv">
                                Create users, manage configuration and general settings.
                            </div>
                            </td>                        
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="loginFooter">
                    <p class="loginFooterTagline">© 2012 Developed By SmartControls India Ltd.</p>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
