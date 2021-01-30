<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="visualInspectionHMI.aspx.cs" MasterPageFile="~/smartMISHMIMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.visualInspectionHMI" %>

<asp:Content ID="viHMIContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="Stylesheet" href="../Style/vi.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script type="text/javascript" language="javascript">
        function enableDefect(value) {
            if (value == "OK") {
                document.getElementById('viHMIDefectTable').style.visibility = "hidden";
                document.getElementById('viHMIButtonTable').style.visibility = "hidden";
            }
            else if (value == "Not OK") {
                document.getElementById('viHMIDefectTable').style.visibility = "visible";
                document.getElementById('viHMIButtonTable').style.visibility = "visible";
                document.getElementById('<%= viHMIStatusOKButton.ClientID %>').disabled = true;

                document.getElementById('<%= viHMIGTBarcodeCodeHidden.ClientID %>').value = document.getElementById('<%= viHMIGTBarcodeTextBox.ClientID %>').value
                document.getElementById('<%= viHMIStatusHidden.ClientID %>').value = value;
                document.getElementById('<%= viHMIGTBarcodeTextBox.ClientID %>').disabled = true;
                document.getElementById('<%= viHMITimerStatusHidden.ClientID %>').value = '1';

                var timer = $get('<%= viTimer.ClientID %>').control;
                timer._stopTimer();
            }
        }

        function setFocus() {
            document.getElementById('<%= this.viHMIGTBarcodeTextBox.ClientID %>').focus();
            setTimeout(function() { document.getElementById('<%= this.viHMIGTBarcodeTextBox.ClientID %>').focus() }, 5);
        }
        function showHistory() {
            document.getElementById('<%= viHMIGTBarcodeCodeHidden.ClientID %>').value = document.getElementById('<%= viHMIGTBarcodeTextBox.ClientID %>').value
            document.getElementById('<%= this.viHMIGTHistoryImageButton.ClientID %>').click();
            
        }

        function callModal() {
            var defectID = document.getElementById('<%= viHMIDefectCodeHidden.ClientID %>').value;
            if (defectID.toString() == '1') {
                revealModal('modalPageForRepair');
            }
            else if (defectID.toString() == '2') {
                revealModal('modalPageForBuff');
            }
            else if (defectID.toString() == '3') {
                revealModal('modalPageForHold');
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
            document.getElementById('<%= viHMIGTBarcodeCodeHidden.ClientID %>').value = gtBarcode.toString();
            document.getElementById('<%= viHMIStatusHidden.ClientID %>').value = status.toString();
        }
        function setDefectCode(defectCode, iD) {
            document.getElementById('viHMIDefectCodeButton').value = defectCode.toString();
            document.getElementById('<%= viHMIDefectCodeHidden.ClientID %>').value = iD.toString();
        }
        function setDefectLocation(defectLocation, iD) {
            document.getElementById('viHMIDefectLocationButton').value = defectLocation.toString();
            document.getElementById('<%= viHMIDefectLocationHidden.ClientID %>').value = iD.toString();
        }
        function validate() {

            if (document.getElementById('<%= viHMIGTBarcodeCodeHidden.ClientID %>').value == '') {
                document.getElementById('<%= this.viHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else if (document.getElementById('viHMIDefectCodeButton').value == '') {
                document.getElementById('<%= this.viHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else if (document.getElementById('viHMIDefectLocationButton').value == '') {
                document.getElementById('<%= this.viHMIGTBarcodeTextBox.ClientID %>').focus()
            }
            else {
                document.getElementById('<%= this.viHMISaveButton.ClientID %>').click();
            }
        }
    </script>
    
    <script type="text/javascript" src="..\Script\autoComplete.js"></script>
    
        <asp:ScriptManager ID="viHMIScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="viHMIUpdatePanel" runat="server">
            <ContentTemplate>
            <asp:Button ID="viMagicButton" runat="server" CssClass="viButton" Text="Save" style="width: 30%; visibility: hidden" OnClick="Button_Click" />
            <table class="viTable" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 20%"></td>
                </tr>
                <tr>
                    <td class="masterLabel" rowspan="2" style="text-align: center">
                        <asp:Label ID="viHMIWCNameLabel" runat="server" class="viTextBox" 
                            style="width :100%" Text="VI-01"></asp:Label>
                    </td>
                    <td class="notifyMessageBoxDiv" style="text-align: center" colspan="2">
                        Last GT Barcdode :
                        <asp:Label ID="viLastGTBarcodeLabel" runat="server" Text=""></asp:Label>
                    </td>
                    <td rowspan="2">
                        <div id="modalPageForGTHistory">
                             <div class="viModalContainer">
                                <div class="viGTHistoryModal">
                                   <div class="viModalTop"><a href="javascript:hideModal('modalPageForGTHistory')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                    <div class="viModalBody">
                                        <asp:Panel ID="viHMIGTHistoryPanel" runat="server" ScrollBars="Vertical" Height="80px" CssClass="panel" >
                                            <asp:GridView ID="viHMIGTHistoryGridView" runat="server" AutoGenerateColumns="False"
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Vertical" PageSize="5" ShowHeader="False" >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                                            </ItemTemplate>
                                                      </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viHMIGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viHMIGridInspectorCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("sapCode") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Defect Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viHMIGridDefectStatusIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("defectStatusID") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Defect Status Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridDefectStatusCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("defectStatusName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                     </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Defect Location" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridDefectLocationLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("defectLocation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viHMIGridStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# displayStatus(DataBinder.Eval(Container.DataItem,"status"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Date Time" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="33%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viHMIGridDateTimeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("dtandTime") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns> 
                                                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                                                HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="#FFFFFF" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="notifyMessageBoxDiv" style="text-align: center">
                        TBM Workcenter :
                        <asp:Label ID="viTBMWCNameLabel" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="notifyMessageBoxDiv" style="text-align: center">
                        Curing Workcenter :
                        <asp:Label ID="viCuringWCNameLabel" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="viHMITimerStatusHidden" runat="server" Value="" />
                    </td>
                    <td align="center" colspan="2" style="height:20px">
                        <div ID="viNotifyMessageDiv" runat="server" class="notifyMessageDiv" 
                            visible="false">
                            <table width="98%">
                                <tr>
                                    <td>
                                        <img ID="viNotifyImage" runat="server" alt="notify" class="notifyImg" 
                                            src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="viNotifyLabel" runat="server" 
                                            Text="Visualization record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td class="masterLabel">
                        <asp:Timer ID="viTimer" runat="server" Enabled="false" Interval="10000" 
                            ontick="NotifyTimer_Tick">
                        </asp:Timer>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        GTBarcode :
                    </td>
                    <td class="masterLabel" colspan="2" style="text-align: center">
                        <input ID="viHMIGTBarcodeTextBox" runat="server" AutoComplete="off"
                            class="viTextBox" disabled="false" maxlength="10" onblur="javascript:showHistory(); return false;" 
                            style="width :100%" type="text" />
                    </td>
                    <td>
                        <asp:Button ID="viHMIGTHistoryImageButton" runat="server" CssClass="viButton" Text="Save" style="width: 30%; visibility: hidden" OnClick="Button_Click" ></asp:Button>
                    </td>
                </tr>
                 <tr>
                     <td class="masterLabel">
                         Status :
                     </td>
                     <td style="text-align: center">
                         <asp:Button ID="viHMIStatusOKButton" runat="server" CssClass="viButton"
                             onclick="Button_Click" style="width: 90%;" Text="OK" />
                     </td>
                     <td style="text-align: center">
                        <input id="viHMIStatusNotOKButton" class="viButton" 
                             onclick="javascript:enableDefect('Not OK'); return false;" style="width: 90%" type="button" 
                             value="Not OK" />
                     </td>
                     <td class="masterLabel">
                        <asp:HiddenField ID="viHMIGTBarcodeCodeHidden" runat="server" Value="" />
                        <asp:HiddenField ID="viHMIStatusHidden" runat="server" Value="" />
                     </td>
                 </tr>
             
                </table>
                <table id="viHMIDefectTable" class="viTable" align="center" cellpadding="2" cellspacing="0" style="visibility: hidden" >
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                    <tr>
                        <td class="masterLabel">
                            Defect :
                            <asp:HiddenField ID="viHMIDefectCodeHidden" runat="server" Value="0" />
                        </td>
                        <td style="text-align: center">
                            <input id="viHMIDefectCodeButton" type="button" class="viButton" value="" style="width: 90%"
                                onclick="return revealModal('modalPageForDefectCode');" />
                        </td>
                        <td style="text-align: center">
                            <input id="viHMIDefectLocationButton" type="button" class="viButton" value="" style="width: 90%"
                                onclick="return callModal();" />
                        </td>
                        <td class="masterLabel">
                            <asp:HiddenField ID="viHMIDefectLocationHidden" runat="server" Value="0" />
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
                                                        <asp:Button ID="viHMIDialogOKButton" runat="server" CssClass="viButton" Text="OK" Width="100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="viHMIDialogNotOKButton" runat="server" CssClass="viButton" Text="Not OK" Width="100%" />
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
                                <div class="viModalContainer">
                                    <div class="viModal">
                                        <div class="viModalTop"><a href="javascript:hideModal('modalPageForDefectCode')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                        <div class="modalBody">
                                        <asp:Panel ID="viHMIDefectCodePanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                            <asp:ListView ID="viHMIDefectCodeListView" runat="server" GroupItemCount="2">
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
                                                                <asp:Button ID="viHMIDefectCodeDialogButton" runat="server" CssClass="viDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectCode(this.value, this.title); javascript:hideModal('modalPageForDefectCode'); return false;" Width="100%" ></asp:Button>
                                                            </td>
                                                        </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                  </div>
                              </div>
                            <div id="modalPageForRepair">
                                  <div class="modalBackground">
                                  </div>
                                   <div class="viModalContainer">
                                      <div class="viModal">
                                         <div class="viModalTop"><a href="javascript:hideModal('modalPageForRepair')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                          <div class="modalBody">
                                              <asp:Panel ID="viHMIRepairPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                              <asp:ListView ID="viHMIRepairListView" runat="server" GroupItemCount="3">
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
                                                   
                                                    <asp:Button ID="viHMIRepairDialogButton" runat="server" CssClass="viDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForRepair'); return false;" Width="80%" ></asp:Button>
                                                   
                                                  </td>
                                                  </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                            <div id="modalPageForBuff">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="viModalContainer">
                                        <div class="viModal">
                                           <div class="viModalTop"><a href="javascript:hideModal('modalPageForBuff')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="viHMIBuffPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="viHMIBuffListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="viHMIBuffDialogButton" runat="server" CssClass="viDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForBuff'); return false;" Width="80%" ></asp:Button>
                                                     
                                                    </td>
                                                    </ItemTemplate>
                                                  </asp:ListView>
                                              </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="modalPageForHold">
                                    <div class="modalBackground">
                                    </div>
                                     <div class="viModalContainer">
                                        <div class="viModal">
                                           <div class="viModalTop"><a href="javascript:hideModal('modalPageForHold')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                            <div class="modalBody">
                                                <asp:Panel ID="viHMIHoldPanel" runat="server" ScrollBars="Vertical" Height="340px" CssClass="panel" >
                                                <asp:ListView ID="viHMIHoldListView" runat="server" GroupItemCount="3">
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
                                                     
                                                      <asp:Button ID="viHMIHoldDialogButton" runat="server" CssClass="viDefectButton" Text='<%# Eval("name") %>' title='<%# Eval("iD") %>' OnClientClick="javascript:setDefectLocation(this.value, this.title); javascript:hideModal('modalPageForHold'); return false;" Width="80%" ></asp:Button>
                                                     
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
                <table id="viHMIButtonTable" class="viTable" align="center" cellpadding="2" cellspacing="0" style="visibility: hidden" >
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 20%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" align="center">
                            <input id="magicButton" type="button" class="viButton" value="Save" style="width: 30%" onclick="javascript:validate(); return false;" />
                            <asp:Button ID="viHMICancelButton" runat="server" CssClass="viButton" Text="Cancel" style="width: 30%" OnClick="Button_Click"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="viHMISaveButton" runat="server" CssClass="viButton" Text="Save" style="width: 30%; visibility: hidden" OnClick="Button_Click" ></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
