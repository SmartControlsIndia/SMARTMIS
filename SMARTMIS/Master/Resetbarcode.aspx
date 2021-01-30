<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="Resetbarcode.aspx.cs" Inherits="SmartMIS.Report.Resetbarcode" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">

  <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
     <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    
</script>
    <%--<script language="javascript" type="text/javascript">
        function setID(value)
        {
            document.getElementById('<%= oemIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= bIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>--%>
     <asp:ScriptManager ID="oemScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="oemUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="masterFirstCol"></td>
                    <td class="masterSecondCol" style="width: 413px"></td>
                    <td class="masterThirdCol"></td>
                    <td class="masterForthCol"></td>
                     <td class="masterfifthCol"></td>
                      <td class="mastersixthCol"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Reset Barcode</p>
                        </div>
                    </td>
                </tr>
                <tr>
                 
                    <td class="masterLabel">Barcode :</td>
                    <td class="masterTextBox" style="width: 203px">
                        <asp:TextBox ID="barcodeTextBox" runat="server" Width="80%" maxlength="10"></asp:TextBox>
                       <asp:Label ID="bIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td class="masterTextBox" style="width: 203px">
                        <span class="errorSpan">*</span>
                    <asp:RequiredFieldValidator ID="areaNameReqFieldValidator" runat="server" CssClass="reqFieldValidator"
                            ControlToValidate="barcodeTextBox" ErrorMessage="Barcode is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Button ID="resetbarcodeSaveButton" runat="server" CssClass="masterButton" Text="Reset" onclick="Button_Click" />&nbsp;
                       
                    </td> 
                   <td class="masterTextBox" style="width: 203px"></td>
                    <td class="masterLabel" style="width: 203px"></td>
                    <td class="masterLabel" style="width: 203px"></td>
                </tr>
                <tr>
                <td ></td>
                <td colspan="2">
                 
                </td> 
                <td>
                    &nbsp;</td>
                <td rowspan=4> 
                    
                    &nbsp;</td>
                </tr>
              
                <tr>
                    <td></td>
                    <td style="width: 413px">
                       <%-- <asp:Button ID="resetbarcodeSaveButton" runat="server" CssClass="masterButton" Text="Reset" onclick="Button_Click" />&nbsp;
                       --%>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
               <tr>
                    <td>
                        <asp:Timer ID="oemNotifyTimer" runat="server" Interval="5000" Enabled="false" ontick="NotifyTimer_Tick">
                            
                        </asp:Timer>
                        <asp:HiddenField ID="rIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center" style="width: 413px">
                        <div id="oemNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="rNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="rNotifyLabel" runat="server" Text="Record Not Found." ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    </tr>
                
               
            </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="resetbarcodeSaveButton" />
        
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
