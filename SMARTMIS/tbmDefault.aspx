<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tbmDefault.aspx.cs" Inherits="SmartMIS._tbmDefault" %>

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
                        <td style="width : 33%"></td>
                        <td style="width : 33%"></td>
                        <td style="width : 33%"></td>
                    </tr>
                    <tr>
                        <td>
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
                                        <asp:TextBox ID="loginUserNameOP1TextBox" runat="server" CssClass="loginTextBox" AutoComplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="loginPasswordOP1TextBox" runat="server" TextMode="Password" CssClass="loginTextBox"></asp:TextBox>
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
                                        <asp:Button ID="loginOP1Button" runat="server" CssClass="signInButton"  Text="Sign in" 
                                        onclick="Button_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">&nbsp;
                                        <asp:Label ID="loginErrorOP1Label" runat="server" CssClass="invalidMessageLabel" Visible="False"></asp:Label>                            
                                    </td>                              
                                </tr>
                                </table>
                            </div>
                        </td>
                       <td>
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
                                        <asp:TextBox ID="loginUserNameOP2TextBox" runat="server" CssClass="loginTextBox" AutoComplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="loginPasswordOP2TextBox" runat="server" TextMode="Password" CssClass="loginTextBox"></asp:TextBox>
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
                                        <asp:Button ID="loginOP2Button" runat="server" CssClass="signInButton"  Text="Sign in" 
                                        onclick="Button_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">&nbsp;
                                        <asp:Label ID="loginErrorOP2Label" runat="server" CssClass="invalidMessageLabel" Visible="False"></asp:Label>                            
                                    </td>                              
                                </tr>
                                </table>
                            </div>
                        </td>
                        <td>
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
                                        <asp:TextBox ID="loginUserNameOP3TextBox" runat="server" CssClass="loginTextBox" AutoComplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="loginPasswordOP3TextBox" runat="server" TextMode="Password" CssClass="loginTextBox"></asp:TextBox>
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
                                        <asp:Button ID="loginOP3Button" runat="server" CssClass="signInButton"  Text="Sign in" 
                                        onclick="Button_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">&nbsp;
                                        <asp:Label ID="loginErrorOP3Label" runat="server" CssClass="invalidMessageLabel" Visible="False"></asp:Label>                            
                                    </td>                              
                                </tr>
                                </table>
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
