<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xRayHMI.aspx.cs" MasterPageFile="~/smartMISHMIMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.xRayHMI" %>

<asp:Content ID="xRayHMIContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="Stylesheet" href="../Style/xRay.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script type="text/javascript" language="javascript">
        function enableDefect(value) {
            if (value == "OK") {
                document.getElementById('xRayHMIDefectTable').style.visibility = "hidden";
                document.getElementById('xRayHMIButtonTable').style.visibility = "hidden";
            }
            else if (value == "Not OK") {
                document.getElementById('xRayHMIDefectTable').style.visibility = "visible";
                document.getElementById('xRayHMIButtonTable').style.visibility = "visible";
                document.getElementById('<%= xRayHMIStatusOKButton.ClientID %>').disabled = true;

                document.getElementById('<%= xRayHMIGTBarcodeCodeHidden.ClientID %>').value = document.getElementById('<%= xRayHMIGTBarcodeTextBox.ClientID %>').value
                document.getElementById('<%= xRayHMIStatusHidden.ClientID %>').value = value;
                document.getElementById('<%= xRayHMIGTBarcodeTextBox.ClientID %>').disabled = true;
                document.getElementById('<%= xRayHMITimerStatusHidden.ClientID %>').value = '1';
            }
        }

        function setFocus() {
            document.getElementById('<%= this.xRayHMIGTBarcodeTextBox.ClientID %>').focus();
            setTimeout(function() { document.getElementById('<%= this.xRayHMIGTBarcodeTextBox.ClientID %>').focus() }, 5);
        }

        function callModal() {
            var defectID = document.getElementById('<%= xRayHMIDefectCodeHidden.ClientID %>').value;
            if (defectID.toString() == '1') {
                revealModal('modalPageForBodyPly');
            }
            else if (defectID.toString() == '2') {
                revealModal('modalPageForBelt');
            }
            else if (defectID.toString() == '3') {
                revealModal('modalPageForBead');
            }
            else if (defectID.toString() == '4') {
                revealModal('modalPageForSteelChipper');
            }
            else if (defectID.toString() == '5') {
                revealModal('modalPageForOther');
            }
        }

        function revealModal(divID) {
            window.onscroll = function() { document.getElementById(divID).style.top = document.body.scrollTop; };
            document.getElementById(divID).style.display = "block";
            document.getElementById(divID).style.top = document.body.scrollTop;
        }
        function hideModal(divID) {
            document.getElementById(divID).style.display = "none";
        }
        function setGTBarcodeandStatus(gtBarcode, status) {
            document.getElementById('<%= xRayHMIGTBarcodeCodeHidden.ClientID %>').value = gtBarcode.toString();
            document.getElementById('<%= xRayHMIStatusHidden.ClientID %>').value = status.toString();
        }
        function setDefectCode(defectCode, iD) {
            document.getElementById('xRayHMIDefectCodeButton').value = defectCode.toString();
            document.getElementById('<%= xRayHMIDefectCodeHidden.ClientID %>').value = iD.toString();
        }
        function setDefectLocation(defectLocation, iD) {
            document.getElementById('xRayHMIDefectLocationButton').value = defectLocation.toString();
            document.getElementById('<%= xRayHMIDefectLocationHidden.ClientID %>').value = iD.toString();
        }
        function validate() {

            if (document.getElementById('<%= xRayHMIGTBarcodeCodeHidden.ClientID %>').value == '') {
                document.getElementById('<%= this.xRayHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else if (document.getElementById('xRayHMIDefectCodeButton').value == '') {
                document.getElementById('<%= this.xRayHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else if (document.getElementById('xRayHMIDefectLocationButton').value == '') {
                document.getElementById('<%= this.xRayHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else {
                document.getElementById('<%= this.xRayHMISaveButton.ClientID %>').click();
            }
        }
    </script>
    
    <script type="text/javascript" src="..\Script\autoComplete.js"></script>
    
        <asp:ScriptManager ID="xRayHMIScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="xRayHMIUpdatePanel" runat="server">
            <ContentTemplate>
            <asp:Button ID="xRayMagicButton" runat="server" CssClass="xRayButton" Text="Save" style="width: 30%; visibility: hidden" OnClick="Button_Click" />
            <table class="xRayTable" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 20%"></td>
                </tr>
                <tr>
                    <td class="masterLabel" rowspan="2" style="text-align: center">
                        <asp:Label ID="xRayHMIWCNameLabel" runat="server" class="xRayTextBox" 
                            style="width :100%" Text="X-Ray-01"></asp:Label>
                    </td>
                    <td class="notifyMessageBoxDiv" style="text-align: center" colspan="2">
                        Last GT Barcdode :
                        <asp:Label ID="xRayLastGTBarcodeLabel" runat="server" Text=""></asp:Label>
                    </td>
                    <td rowspan="2">
                    </td>
                </tr>
                <tr>
                    <td class="notifyMessageBoxDiv" style="text-align: center">
                        TBM Workcenter :
                        <asp:Label ID="xRayTBMWCNameLabel" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="notifyMessageBoxDiv" style="text-align: center">
                        Curing Workcenter :
                        <asp:Label ID="xRayCuringWCNameLabel" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="xRayHMITimerStatusHidden" runat="server" Value="" />
                    </td>
                    <td align="center" colspan="2" style="height:20px">
                        <div ID="xRayNotifyMessageDiv" runat="server" class="notifyMessageDiv" 
                            visible="false">
                            <table width="98%">
                                <tr>
                                    <td>
                                        <img ID="xRayNotifyImage" runat="server" alt="notify" class="notifyImg" 
                                            src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="xRayNotifyLabel" runat="server" 
                                            Text="X-Ray record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td class="masterLabel">
                       <%-- <asp:Timer ID="xRayTimer" runat="server" Enabled="false" Interval="10000" 
                            ontick="NotifyTimer_Tick">
                        </asp:Timer>--%>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        GTBarcode :
                    </td>
                    <td class="masterLabel" colspan="2" style="text-align: center">
                        <input ID="xRayHMIGTBarcodeTextBox" runat="server" AutoComplete="off"
                            class="xRayTextBox" disabled="true" maxlength="10" 
                            style="width :100%" type="text" />
                    </td>
                    <td>
                    </td>
                </tr>
                 <tr>
                     <td class="masterLabel">
                         Status :
                     </td>
                     <td style="text-align: center">
                         <asp:Button ID="xRayHMIStatusOKButton" runat="server" CssClass="xRayButton"
                             onclick="Button_Click" style="width: 90%;" Text="OK" />
                     </td>
                     <td style="text-align: center">
                        <input id="xRayHMIStatusNotOKButton" class="xRayButton" 
                             onclick="javascript:enableDefect('Not OK'); return false;" style="width: 90%" type="button" 
                             value="Not OK" />
                     </td>
                     <td class="masterLabel">
                        <asp:HiddenField ID="xRayHMIGTBarcodeCodeHidden" runat="server" Value="" />
                        <asp:HiddenField ID="xRayHMIStatusHidden" runat="server" Value="" />
                     </td>
                 </tr>
             
                </table>
                <table id="xRayHMIDefectTable" class="xRayTable" align="center" cellpadding="2" cellspacing="0" style="visibility: hidden" >
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                    <tr>
                        <td class="masterLabel">
                            Defect :
                            <asp:HiddenField ID="xRayHMIDefectCodeHidden" runat="server" Value="0" />
                        </td>
                        <td style="text-align: center">
                            <input id="xRayHMIDefectCodeButton" type="button" class="xRayButton" value="" style="width: 90%"
                                onclick="return revealModal('modalPageForDefectCode');" />
                        </td>
                        <td style="text-align: center">
                            <input id="xRayHMIDefectLocationButton" type="button" class="xRayButton" value="" style="width: 90%"
                                onclick="return callModal();" />
                        </td>
                        <td class="masterLabel">
                            <asp:HiddenField ID="xRayHMIDefectLocationHidden" runat="server" Value="0" />
                        </td>
                     </tr>
                     <tr>
                        <td colspan="4">
                            <div id="modalPageForStatus">
                                <div class="modalBackground">
                                </div>
                                <div class="modalContainer">
                                    <div class="modal">
                                        <div class="modalBody">
                                            <table class="innerTable" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 100%"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="xRayHMIDialogOKButton" runat="server" CssClass="xRayButton" Text="OK" Width="100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="xRayHMIDialogNotOKButton" runat="server" CssClass="xRayButton" Text="Not OK" Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="modalPageForDefectCode">
                                <div class="modalBackground">
                                </div>
                                <div class="xRayModalContainer">
                                    <div class="xRayModal">
                                        <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForDefectCode')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                        <div class="modalBody">
                                        <asp:Panel ID="xRayHMIDefectCodePanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                            <asp:ListView ID="xRayHMIDefectCodeListView" runat="server" GroupItemCount="2">
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
                                                                <asp:Button ID="xRayHMIDefectCodeDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectCode(this.value, this.title); javascript:hideModal('modalPageForDefectCode'); return false;" Width="100%" ></asp:Button>
                                                            </td>
                                                        </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                  </div>
                              </div>
                            <div id="modalPageForBodyPly">
                                  <div class="modalBackground">
                                  </div>
                                   <div class="xRayModalContainer">
                                      <div class="xRayModal">
                                         <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForBodyPly')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                          <div class="modalBody">
                                              <asp:Panel ID="xRayHMIBodyPlyPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                              <asp:ListView ID="xRayHMIBodyPlyListView" runat="server" GroupItemCount="3">
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
                                                   
                                                    <asp:Button ID="xRayHMIBodyPlyDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForBodyPly'); return false;" Width="80%" ></asp:Button>
                                                   
                                                  </td>
                                                  </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                            <div id="modalPageForBelt">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="xRayModalContainer">
                                        <div class="xRayModal">
                                           <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForBelt')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="xRayHMIBeltPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="xRayHMIBeltListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="xRayHMIBeltDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForBelt'); return false;" Width="80%" ></asp:Button>
                                                     
                                                    </td>
                                                    </ItemTemplate>
                                                  </asp:ListView>
                                              </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="modalPageForBead">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="xRayModalContainer">
                                        <div class="xRayModal">
                                           <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForBead')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="xRayHMIBeadPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="xRayHMIBeadListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="xRayHMIBeadDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForBead'); return false;" Width="80%" ></asp:Button>
                                                     
                                                    </td>
                                                    </ItemTemplate>
                                                  </asp:ListView>
                                              </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="modalPageForSteelChipper">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="xRayModalContainer">
                                        <div class="xRayModal">
                                           <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForSteelChipper')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="xRayHMISteelChipperPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="xRayHMISteelChipperListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="xRayHMISteelChipperDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForSteelChipper'); return false;" Width="80%" ></asp:Button>
                                                     
                                                    </td>
                                                    </ItemTemplate>
                                                  </asp:ListView>
                                              </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="modalPageForOther">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="xRayModalContainer">
                                        <div class="xRayModal">
                                           <div class="xRayModalTop"><a href="javascript:hideModal('modalPageForOther')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="xRayHMIOtherPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="xRayHMIOtherListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="xRayHMIOtherDialogButton" runat="server" CssClass="xRayDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForOther'); return false;" Width="80%" ></asp:Button>
                                                     
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
                 </table>
                <table id="xRayHMIButtonTable" class="xRayTable" align="center" cellpadding="2" cellspacing="0" style="visibility: hidden" >
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" align="center">
                            <input id="magicButton" type="button" class="xRayButton" value="Save" style="width: 30%" onclick="javascript:validate(); return false;" />
                            <asp:Button ID="xRayHMICancelButton" runat="server" CssClass="xRayButton" Text="Cancel" style="width: 30%" OnClick="Button_Click"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="xRayHMISaveButton" runat="server" CssClass="xRayButton" Text="Save" style="width: 30%; visibility: hidden" OnClick="Button_Click" ></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
