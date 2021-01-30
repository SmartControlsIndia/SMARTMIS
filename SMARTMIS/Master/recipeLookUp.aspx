<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" CodeBehind="RecipeLookUp.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Master.WebForm1" %>
<asp:Content ID="recipeLookUpContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Style/masterPage.css" />

    <script language="javascript" type="text/javascript">
        
        function setID(value)
        {
            document.getElementById('<%= recipeLookUpIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= recipeLookUpIDLabel.ClientID %>').innerHTML = value;
        }  
             
    </script>

    <asp:ScriptManager ID="recipeLookUpScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="recipeLookUpUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">RecipeLookUp setting</p>
                        </div>
                    </td>
                </tr>
                <asp:Label ID="recipeLookUpIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <tr>
                    <td class="masterLabel">TBM RecipeName :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="recipeLookUpTbmRecipeNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="82%" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="recipeLookUpTbmRecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                         
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeLookUpTbmRecipeNameFieldValidator" runat="server" 
                            ControlToValidate="recipeLookUpTbmRecipeNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="TBM Recipe Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td class="masterLabel">Curing RecipeName :</td>
                    <td>
                   
                        <asp:DropDownList ID="recipeLookUpCuringRecipeNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" 
                            OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                        
                        <asp:Label ID="recipeLookUpCuringRecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>           
                    <td>   <asp:RequiredFieldValidator ID="curingRecipeNameValidator" runat="server" 
                            ControlToValidate="recipeLookUpCuringRecipeNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Curing Recipe Name is Required">
                        </asp:RequiredFieldValidator>
                        </span></td>
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
                        <asp:Button ID="recipeLookUpSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click"  />&nbsp;
                        <asp:Button ID="recipeLookUpCancelButton" runat="server" CssClass="masterButton" Text="Cancel" onclick="Button_Click" CausesValidation="false"  />&nbsp;                       
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
                                                     <asp:Label ID="recipeLookUpMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete MHE Record." ></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="recipeLookUpDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="recipelookUpDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false;" Text="Cancel" />
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
                        <asp:Timer ID="recipelookUpNotifyTimer" runat="server" OnTick="NotifyTimer_Tick" Interval="5000" Enabled="false" >
                        </asp:Timer>
                        <asp:HiddenField ID="recipeLookUpIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="recipeLookUpNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="recipeLookUpNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="recipeLookUpNotifyLabel" runat="server" Text="Recipe Setting  saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Recipe LookUP Detail: 
                                </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                           <tr> 
                              <td class="gridViewHeader" style="width:20%; padding:5px">TBM RecipeName</td><td class="gridViewHeader" style="width:20%; padding:5px">Curing RecipeName</td><td class="gridViewHeader" style="width:10%; padding:5px"></td>
                           </tr>  
                        </table>
                           <asp:Panel ID="recipeLookUpPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="recipeLookUpGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="recipeLookUpGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                                         <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="recipeLookUptbmRecipeGridIDLabel" runat="server" Text='<%# Eval("tbmrecipeiD") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                          
                                          <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="recipeLookUpCuringRecipeGridIDLabel" runat="server" Text='<%# Eval("curingRecipeiD") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                                <Columns>         
                                              
                                        <asp:TemplateField HeaderText="tbm recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="recipeLookUpGridTBMRecipeNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("tbmrecName") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                                                <Columns>
                                        <asp:TemplateField HeaderText="MHE ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="recipeLookUpGridCuringRecipeNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("curingrecName") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
                                         <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%">
                                             <ItemTemplate>                                                
                                                <asp:ImageButton ID="recipeLookUpGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click"  />
                                                 <asp:ImageButton ID="recipeLookUpGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage');javascript:setID(this.value); return false;" />
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