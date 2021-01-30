<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="TrimmingFileName.aspx.cs" Inherits="SmartMIS.Master.TrimmingFileName" Title="Untitled Page" %>
<asp:Content ID="TrimLookUpContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
 <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Style/masterPage.css" />

    <script language="javascript" type="text/javascript">
        
        function setID(value)
        {
            document.getElementById('<%= TrimLookUpIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= TrimLookUpIDLabel.ClientID %>').innerHTML = value;
        }  
             
    </script>

    <asp:ScriptManager ID="TrimLookUpScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="TrimLookUpUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">TrimLookUp setting</p>
                        </div>
                    </td>
                </tr>
                <asp:Label ID="TrimLookUpIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <tr>
                    <td class="masterLabel">TBM RecipeName :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="TrimLookUpTbmRecipeNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="82%" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="TrimLookUpTbmRecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                         
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="TrimLookUpTbmRecipeNameFieldValidator" runat="server" 
                            ControlToValidate="TrimLookUpTbmRecipeNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="TBM Recipe Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td class="masterLabel">MouldName :</td>
                    <td>
                    <asp:Label ID="TrimLookUpGridMouldNameLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                       <asp:TextBox ID="txtmouldname" runat="server" MaxLength="4" Width="80%"></asp:TextBox>
                        
                    </td>
                   <td>
                        <span class="errorSpan">*</span></td>           
                     <td>
                        <asp:RequiredFieldValidator ID="txtmouldnameValidator" runat="server" 
                            ControlToValidate="txtmouldname" CssClass="reqFieldValidator"
                            ErrorMessage="Mould Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                
          <tr>
                    <td class="masterLabel">Trim No :</td>
                    <td>
                        <asp:TextBox ID="txtTrim" runat="server" Width="80%"></asp:TextBox>
                        <asp:Label ID="TrimLookUpGridTrimNOLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                  <td>
                        <span class="errorSpan">*</span></td>           
                     <td>
                        <asp:RequiredFieldValidator ID="txtTrimValidator" runat="server" 
                            ControlToValidate="txtTrim" CssClass="reqFieldValidator"
                            ErrorMessage="Trim No is Required.Enter number only">
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
                        <asp:Button ID="TrimLookUpSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click"  />&nbsp;
                        <asp:Button ID="TrimLookUpCancelButton" runat="server" CssClass="masterButton" Text="Cancel" onclick="Button_Click" CausesValidation="false"  />&nbsp;                       
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
                                                     <asp:Label ID="TrimLookUpMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete MHE Record." ></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="TrimLookUpDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
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
                        <asp:HiddenField ID="TrimLookUpIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="TrimLookUpNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="TrimLookUpNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="TrimLookUpNotifyLabel" runat="server" Text="Recipe Setting  saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Trim LookUP Detail: 
                                </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                           <tr> 
                              <td class="gridViewHeader" style="width:15%; padding:5px">TBM RecipeName</td>
                              <td class="gridViewHeader" style="width:15%; padding:5px">MouldName</td>
                              <td class="gridViewHeader" style="width:15%; padding:5px">TrimNo</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Action</td>
                           </tr>  
                        </table>
                           <asp:Panel ID="TrimLookUpPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="TrimLookUpGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                   <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="TrimLookUpGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                                         <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="TrimLookUptbmRecipeGridIDLabel" runat="server" Text='<%# Eval("tbmrecipeiD") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                          
                                          
                                                <Columns>         
                                              
                                        <asp:TemplateField HeaderText="tbm recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="TrimLookUpGridTBMRecipeNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("Name") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                                                <Columns>
                                        <asp:TemplateField HeaderText="MHE ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="TrimLookUpGridMouldNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("mouldName") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                              <Columns>
                                        <asp:TemplateField HeaderText="TrimNo" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="TrimLookUpGridTrimNOLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("TrimNo") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns>
                              <Columns>
                                         <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%">
                                             <ItemTemplate>                                                
                                                <asp:ImageButton ID="TrimLookUpGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click"  />
                                                 <asp:ImageButton ID="TrimLookUpGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage');javascript:setID(this.value); return false;" />
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
