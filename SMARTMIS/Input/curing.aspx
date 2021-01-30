<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="curing.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Input.curing" %>


<asp:Content ID="viContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= curingIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= curingIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>

    <asp:ScriptManager ID="curingScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="curingUpdatePanel" runat="server">
        <ContentTemplate>
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
                            <p class="masterHeaderTagline">Curing Input</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Work Center Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="curingWCNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="82%" 
                            onselectedindexchanged="curingWCNameDropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="curingWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingWCNameFieldValidator" runat="server" 
                            ControlToValidate="curingWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">GTBarcode :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="curingGTBarcodeTextBox" runat="server" Width="80%"></asp:TextBox>
                        <asp:Label ID="curingIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingGTBarcodeFieldValidator" runat="server" 
                            ControlToValidate="curingGTBarcodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="GT Barcode is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Press BarCode :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="curingPressBarCodeTextBox" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingGTBarcodeFieldValidator0" runat="server" 
                            ControlToValidate="curingPressBarCodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Press BarCode is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Recipe Code&nbsp; :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="curingRecipecodeTextBox" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingGTBarcodeFieldValidator1" runat="server" 
                            ControlToValidate="curingRecipecodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Recipe Code is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Mould No :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="curingMouldNoTextBox" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingGTBarcodeFieldValidator2" runat="server" 
                            ControlToValidate="curingMouldNoTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Mould No is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Press Direction :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="curingPressDirDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true" Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="curingPressDirIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="curingFaultStatusFieldValidator" runat="server" 
                            ControlToValidate="curingPressDirDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Press Direction is Required"></asp:RequiredFieldValidator>
                    </td>
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
                        <asp:Button ID="curingSaveButton" runat="server" CssClass="masterButton" Text="Save" OnClick="Button_Click"  />&nbsp;
                        <asp:Button ID="curingCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                     <td colspan="4">
                         <div id="modalPage">
                             <div class="modalBackground">
                             </div>
                              <div class="modalContainer">
                                 <div class="modal">
                                     <div class="modalTop"><a href="javascript:hideModal('modalPage')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                     <div class="modalBody">
                                         <table class="innerTable" cellspacing="0">
                                             <tr>
                                                 <td style="width: 20%"></td>
                                                 <td style="width: 80%"></td>
                                             </tr>
                                             <tr>
                                                 <td class="masterLabel">
                                                     <img alt="Close" src="../Images/question.png" />
                                                 </td>
                                                 <td>
                                                     <asp:Label ID="curingMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Visualization Record."></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="curingDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="curingDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                                 </td>
                                             </tr>
                                         </table>
                                     </div>
                                 </div>
                             </div>
                         </div>
                     </td>
                </tr>
                <tr>
                    <td>
                        <asp:Timer ID="curingNotifyTimer" runat="server" Interval="5000" Enabled="false" >
                        </asp:Timer>
                        <asp:HiddenField ID="curingIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="curingNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="curingNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="curingNotifyLabel" runat="server" 
                                            Text="Curing record saved successfully."></asp:Label>
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
                 <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Curing Detail</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                           <tr>
                               <td class="gridViewHeader" style="width:5%; padding:5px">Work Center</td>
                               <td class="gridViewHeader" style="width:5%; padding:5px">GT Barcode</td>
                               <td class="gridViewHeader" style="width:5%; padding:5px">Press BarCode</td>
                               <td class="gridViewHeader" style="width:5%; padding:5px">Recipe Code</td>
                               <td class="gridViewHeader" style="width:5%; padding:5px">Mould No</td>
                               <td class="gridViewHeader" style="width:5%; padding:5px">Press Direction</td>                              
                               
                           </tr>  
                        </table>
                           <asp:Panel ID="curingPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="curingGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="curingGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="curingGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                         <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtbarCode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" >
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridPressBarCodeIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("pressbarCode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridRecipeCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("recipeCode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%" >
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridMouldNoLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("mouldNo") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Reason Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridPressDirectionLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("pressDirection") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                 
                                     <Columns>
                                         <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                             <ItemTemplate>
                                                 <asp:Label ID="curingGridManningIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("manningID") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                 
                                     <Columns>
                                         <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                             <ItemTemplate>
                                                 <asp:ImageButton ID="curingGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                                 <asp:ImageButton ID="curingGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     