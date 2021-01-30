<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SmartMIS._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<link rel="stylesheet" href="Style/masterPage.css" type="text/css" charset="utf-8" />
<link rel="stylesheet" href="Style/default.css" type="text/css" charset="utf-8" />
<link rel="SHORTCUT ICON" href="Images/favicon.ico" />

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="defaultHead" runat="server">
    <title>Smart MIS - Login</title>
    <script>
//        browser = {};
//        if (/(chrome\/[0-9]{2})/i.test(navigator.userAgent)) {
//            browser.agent = navigator.userAgent.match(/(chrome\/[0-9]{2})/i)[0].split("/")[0];
//            browser.version = parseInt(navigator.userAgent.match(/(chrome\/[0-9]{2})/i)[0].split("/")[1]);
//        } else if (/(firefox\/[0-9]{2})/i.test(navigator.userAgent)) {
//            browser.agent = navigator.userAgent.match(/(firefox\/[0-9]{2})/i)[0].split("/")[0];
//            browser.version = parseInt(navigator.userAgent.match(/(firefox\/[0-9]{2})/i)[0].split("/")[1]);
//        } else if (/(MSIE\ [0-9]{1})/i.test(navigator.userAgent)) {
//            browser.agent = navigator.userAgent.match(/(MSIE\ [0-9]{1})/i)[0].split(" ")[0];
//            browser.version = parseInt(navigator.userAgent.match(/(MSIE\ [0-9]{1})/i)[0].split(" ")[1]);
//        } else if (/(Opera\/[0-9]{1})/i.test(navigator.userAgent)) {
//            browser.agent = navigator.userAgent.match(/(Opera\/[0-9]{1})/i)[0].split("/")[0];
//            browser.version = parseInt(navigator.userAgent.match(/(Opera\/[0-9]{1})/i)[0].split("/")[1]);
//        } else if (/(Trident\/[7]{1})/i.test(navigator.userAgent)) {
//            browser.agent = "MSIE";
//            browser.version = 11;
//        } else {
//            browser.agent = false;
//            browser.version = false;
//        }
//        document.write(browser.agent +
//         " " + browser.version);
    </script>
</head>
<style>
input[type=text]:focus, input[type=password]:focus {
  border: 1px solid #129FEA;
  padding: 2px;
  <%--border-radius: 4px;--%>
}
</style>
<body>
<form id="logInForm" runat="server">
 <asp:ScriptManager ID="LoginScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
        
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="LoginUpdatePanel">
        <ProgressTemplate>
        <div class="backDiv">             
             <div align="center" class="waitBox">
             
<div id="bookG">
<div id="blockG_1" class="blockG">
</div>
<div id="blockG_2" class="blockG">
</div>
<div id="blockG_3" class="blockG">
</div>
</div>

<br />

             <h2><font color="#888888">Loading, please wait............</font> </h2> 
             </div>
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="LoginUpdatePanel" runat="server">
        <ContentTemplate>
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
                                        <asp:TextBox ID="loginUserNameTextBox" runat="server" CssClass="loginTextBox" AutoComplete="off"  placeholder="Username" autofocus required></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="loginLabel">
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="loginPasswordTextBox" runat="server" TextMode="Password" CssClass="loginTextBox"  placeholder="Password" required></asp:TextBox>
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
                                        <asp:Label ID="loginErrorLabel" runat="server" CssClass="invalidMessageLabel shake" Visible="False"></asp:Label>                            
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
    </ContentTemplate>
</asp:UpdatePanel>
    </form>
</body>
</html>
