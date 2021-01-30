<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tyreBuildingMachineHMI.aspx.cs" MasterPageFile="~/smartMISHMIMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.tyreBuildingMachineHMI" %>

<asp:Content ID="tbmHMIContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="Stylesheet" href="../Style/tbm.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript" language="javascript">

        function setFocus() {
            document.getElementById('<%= this.tbmHMIGTBarcodeTextBox.ClientID %>').value = '';
            document.getElementById('<%= this.tbmHMIGTBarcodeTextBox.ClientID %>').focus();
            setTimeout(function() { document.getElementById('<%= this.tbmHMIGTBarcodeTextBox.ClientID %>').focus() }, 5);
        }
        function revealModal(divID) {
            window.onscroll = function() { document.getElementById(divID).style.top = document.body.scrollTop; };
            document.getElementById(divID).style.display = "block";
            document.getElementById(divID).style.top = document.body.scrollTop;
        }
        function hideModal(divID) {
            document.getElementById(divID).style.display = "none";
        }
        function setRecipeCode(recipeCode, iD) {

            

            var a = ""
            a = iD;
            var i_d="";
            var imagepath="";
            
            var i;
            for (i = 0; i < a.length; i++) 
            {
             if(a[i]=="#") {
                 break; 
             }
            }

            i_d = a.substring(0, i);
            i = i+1;
            imagepath=a.substring(i+1,a.length)

           
            document.getElementById('<%= tbmHMIRecipeCodeButton.ClientID %>').value = recipeCode.toString();
            document.getElementById('<%= tbmHMIRecipeIDHidden.ClientID %>').value = i_d.toString();
            document.getElementById('<%= tbmHMIRecipeCodeHidden.ClientID %>').value = recipeCode.toString();
            document.getElementById('<%= Image1.ClientID %>').src = imagepath;             
              
        }
    
    </script>
    
        <asp:ScriptManager ID="tbmHMIScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="tbmHMIUpdatePanel" runat="server">
            <ContentTemplate>
            <table class="tbmTable" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 20%"></td>
                </tr>
                <tr>
                     <td class="masterLabel">Workcenter Name : </td>
                     <td class="masterLabel" colspan="2" style="text-align: center">
                         <asp:Label id="tbmHMIWCNameLabel" runat="server" class="tbmTextBox" Text="TB-101" style="width :100%" ></asp:Label>
                     </td>
                     <td>
                         &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="masterLabel">Recipe Code : </td>
                        <td class="masterLabel" colspan="2" style="text-align: center">
                            <input id="tbmHMIRecipeCodeButton" runat="server" type="button" 
                                class="tbmButton" value="" style="width: 69%"
                                onclick="return revealModal('modalPageForRecipeCode');" />
                            
                            <asp:Image ID="Image1" runat="server" Height="36px" 
                                style="height: 116px; width: 214px" Width="43px" 
                                ImageUrl="" />
                            
                        </td>
                        <td>
                            <asp:HiddenField ID="tbmHMIRecipeIDHidden" runat="server" Value="" />
                            <asp:HiddenField ID="tbmHMIRecipeCodeHidden" runat="server" Value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="masterLabel">GTBarcode : </td>
                        <td class="masterLabel" colspan="2" style="text-align: center">
                            <input id="tbmHMIGTBarcodeTextBox" runat="server" type="text" class="tbmTextBox" AutoComplete="off"  disabled="false" style="width :100%" onblur="javascript:setFocus(this);" />
                        </td>
                        <td>
                            <asp:button ID="tbmHMIOKButton" CssClass="tbmButton" runat="server"  Text="OK"
                                style="visibility:hidden" onclick="Button_Click" />
                        </td>
                    </tr>
                </table>
                <table id="tbmHMIDefectTable" class="tbmTable" align="center" cellpadding="2" cellspacing="0" >
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                     <tr>
                        <td colspan="4">
                            <div id="modalPageForRecipeCode">
                                <div class="modalBackground">
                                </div>
                                <div class="tbmHMIModalContainer">
                                    <div class="tbmHMIModal">
                                        <div class="tbmHMIModalTop"><a href="javascript:hideModal('modalPageForRecipeCode')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                        <div class="modalBody">
                                        <asp:Panel ID="tbmHMIDefectCodePanel" runat="server" ScrollBars="Vertical" Height="225px" CssClass="panel" >
                                            <asp:ListView ID="tbmHMIDefectCodeListView" runat="server" GroupItemCount="1">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder ID="groupPlaceholder" runat="server"></asp:PlaceHolder>
                                                        </LayoutTemplate>
                                                        <GroupTemplate>
                                                            <table cellspacing="1" cellpadding="1" width="100%">
                                                                <tr>
                                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                                                </tr>
                                                            </table>
                                                        </GroupTemplate>
                                                        <ItemTemplate>
                                                            <td> 
                                                                
                                                                <asp:Button ID="viHMIDefectCodeDialogButton" runat="server" CssClass="tbmButton" Text='<%# Eval("recipename") %>' title='<%# Eval("iD")+ "#" + Eval("imagepath") %>' OnClientClick="javascript:setRecipeCode(this.value, this.title); javascript:hideModal('modalPageForRecipeCode'); return false;" Width="100%" ></asp:Button>
                                                            </td>
                                                        </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                  </div>
                              </div>
                        </td>
                     </tr>
                     <tr>
                         <td>
                            <asp:Timer ID="tbmNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                                ontick="NotifyTimer_Tick">
                            </asp:Timer>
                         </td>
                         <td align="center" colspan="2" style="height:20px">
                             <div id="tbmNotifyMessageDiv" runat="server" visible="false" class="tbmNotifyMessageDiv">
                                 <table width="98%">
                                     <tr>
                                         <td style="width: 10%">
                                             <img id="tbmNotifyImage" runat="server" alt="notify" class="tbmNotifyImg" src="../Images/notifyCircle.png" />
                                         </td>
                                         <td style="width: 90%">
                                             <asp:Label id="tbmNotifyLabel" runat="server" Text="GT Barcode record saved successfully."></asp:Label>
                                         </td>
                                     </tr>
                                 </table>
                             </div>
                         </td>
                         
                         <td>
                            
                         </td>
                     </tr>
                 </table>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
