<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" CodeBehind="mheInput.aspx.cs" Inherits="SmartMIS.Input.mheInput" %>

<asp:Content ID="viContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= mheIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= mheIDLabel.ClientID %>').innerHTML = value;
        }       
    </script>

    <asp:ScriptManager ID="mheScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="mheUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">MHE Input</p>
                        </div>
                    </td>
                </tr>
                <asp:Label ID="mheIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <tr>
                    <td class="masterLabel">Work center Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="mheWCNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="82%" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="mheWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="mheWCNameFieldValidator" runat="server" 
                            ControlToValidate="mheWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td class="masterLabel">Product Code :</td>
                    <td>
                    
                        <asp:DropDownList ID="mheProductCodeDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" 
                            OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                        
                    <asp:Label ID="mheProductIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>                  
                    <td>   <asp:RequiredFieldValidator ID="mheProductCodeValidator" runat="server" 
                            ControlToValidate="mheProductCodeDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Product Code is Required">
                        </asp:RequiredFieldValidator>
                        </span></td>
                </tr>
                
                <tr>
                    <td class="masterLabel">Unit :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="mheUnitDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="mheUnitIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                
                <tr>
                    <td class="masterLabel">
                        Quantity:
                    </td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="mheQuantityTextBox" runat="server" Width="80%"></asp:TextBox>
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="mheQuantityTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Quantity is Required">
                        </asp:RequiredFieldValidator>            
            </td>
                     
                </tr>
                <tr>
                    <td class="masterLabel">MHE Code :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="mheCodeDropDownList" runat="server" 
                            AutoPostBack="true" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList" Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="mheCodeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="mheCodeFieldValidator" runat="server" 
                            ControlToValidate="mheCodeDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="MHE Code is Required">
                        </asp:RequiredFieldValidator>
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
                        <asp:Button ID="mheSaveButton" runat="server" CssClass="masterButton" 
                            Text="Save" onclick="Button_Click"  />&nbsp;
                        <asp:Button ID="mheCancelButton" runat="server" CssClass="masterButton" Text="Cancel" onclick="Button_Click" CausesValidation="false"  />&nbsp;                       
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
                                                     <asp:Label ID="mheMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete MHE Record." ></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="mheDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="mheDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false;" Text="Cancel" />
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
                        <asp:Timer ID="mheNotifyTimer" runat="server" OnTick="NotifyTimer_Tick" Interval="5000" Enabled="false" >
                        </asp:Timer>
                        <asp:HiddenField ID="mheIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="mheNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="mheNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="mheNotifyLabel" runat="server" Text="MHE record saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">MHE Detail: 
                                </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                           <tr>
                               <td class="gridViewHeader" style="width:15%; padding:5px">MHE Code</td> 
                               <td class="gridViewHeader" style="width:15%; padding:5px">Product Code</td>
                               <td class="gridViewHeader" style="width:15%; padding:5px">Unit</td>
                               <td class="gridViewHeader" style="width:15%; padding:5px">Quantity</td>                           
                                                        
                               <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                           </tr>  
                        </table>
                           <asp:Panel ID="mhePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="mheGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="mheGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="mheGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    
                                             <Columns>
                                         <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                             <ItemTemplate>
                                                 <asp:Label ID="mheGridMHEWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns> 
                                     
                                          <Columns>
                                         <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                             <ItemTemplate>
                                                 <asp:Label ID="mheGridMHENameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("mheName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>  
                               
                                     <Columns>
                                         <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                             <ItemTemplate>
                                                 <asp:Label ID="mheGridProductNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("productTypeName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                             <ItemTemplate>
                                                 <asp:Label ID="mheGridUnitNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("unitName") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>
                                     <Columns>
                                         <asp:TemplateField HeaderText="Fault Status Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                             <ItemTemplate>
                                                 <asp:Label ID="mheGridQuantityLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("quantity") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                     </Columns>                     
                                                 

                                     <Columns>
                                         <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                             <ItemTemplate>                                                
                                                <asp:ImageButton ID="mheGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click"  />
                                                 <asp:ImageButton ID="mheGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
</asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        