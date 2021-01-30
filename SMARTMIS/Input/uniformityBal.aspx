<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Input/uniformityBal.aspx"Inherits="SmartMIS.Input.uniformityBal" MasterPageFile="~/smartMISMaster.Master" %>

<asp:Content ID="viContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= uniBalIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= uniBalIDLabel.ClientID %>').innerHTML = value;
        }
                        
    </script>

    <asp:ScriptManager ID="uniBalScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uniBalUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">Uniformity &amp; Balancing Input</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Work center Name :</td>
                    <td class="masterTextBox">
                    
                        <asp:Label ID="uniBalWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                    
                        <asp:DropDownList ID="uniBalWCNameDropDownList" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" 
                            CssClass="masterDropDownList"  Width="82%" >
                        </asp:DropDownList>
                                                
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalWCNameFieldValidator" runat="server" 
                            ControlToValidate="uniBalWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">GTBarcode :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="uniBalGTBarcodeTextBox" runat="server" Width="80%"></asp:TextBox>
                        <asp:Label ID="uniBalIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalGTBarcodeFieldValidator" runat="server" 
                            ControlToValidate="uniBalGTBarcodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="GT Barcode is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Recipe Code:</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="uniBalRecipeCodeTextBox" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalGTBarcodeFieldValidator0" runat="server" 
                            ControlToValidate="uniBalRecipeCodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="RecipeCode is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Csv File Data:</td>
                    <td class="masterTextBox">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="masterLabel">Test Result:</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="uniBalTestResultDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true" 
                             Width="82%">
                        </asp:DropDownList>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalGTBarcodeFieldValidator1" runat="server" 
                            ControlToValidate="uniBalTestResultDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Test Result is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Reason Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="uniBalReasonNameDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true"  Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="uniBalReasonIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalReasonNameFieldValidator" runat="server" 
                            ControlToValidate="uniBalReasonNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Reason is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Status :</td>
                    <td>
                        <asp:DropDownList ID="uniBalStatusDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="30%" 
                            onselectedindexchanged="DropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="uniBalStatusFieldValidator" runat="server" 
                            ControlToValidate="uniBalStatusDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Status is Required"></asp:RequiredFieldValidator>
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
                        <asp:Button ID="uniBalSaveButton" runat="server" CssClass="masterButton" Text="Save"  />&nbsp;
                        <asp:Button ID="uniBalCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false"  />
                    </td>
                    <td></td>
                    <td>&nbsp;</td>
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
                                                     <asp:Label ID="uniBalMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Visualization Record."></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="uniBalDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" />&nbsp;
                                                     <asp:Button ID="uniBalDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                        <asp:Timer ID="uniBalNotifyTimer" runat="server" Interval="5000" Enabled="false" >
                        </asp:Timer>
                        <asp:HiddenField ID="uniBalIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="uniBalNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="uniBalNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="uniBalNotifyLabel" runat="server" 
                                            Text="Uniformity &amp; Balancing record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Visual Inspection Detail</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                           <tr>
                               <td class="gridViewHeader" style="width:20%; padding:5px">Work center</td>
                               <td class="gridViewHeader" style="width:20%; padding:5px">GT Barcode</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Recipe Code</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">CSV File Data</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Test Result</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Reason Name</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Status</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                           </tr>  
                        </table>
                           <asp:Panel ID="uniBalPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="uniBalGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="uniBalGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="uniBalGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                         <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridRecipeCodeIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("recipeCode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridFaultStatusCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("csvFileData") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" >
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridTestResultLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("testResult") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Reason Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridReasonNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonID") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Direction" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridReasonNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalGridStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("status") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                 
                                     
                                       <Columns>
                                         <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                             <ItemTemplate>
                                                 <asp:Label ID="uniBalStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("manningID") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     
                                     <Columns>
                                         <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                             <ItemTemplate>
                                                 <asp:ImageButton ID="uniBalGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif"  />
                                                 <asp:ImageButton ID="uniBalGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
</asp:Content>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       