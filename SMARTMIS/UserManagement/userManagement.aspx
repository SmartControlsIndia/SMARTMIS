<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userManagement.aspx.cs" Inherits="SmartMIS.userManagement" MasterPageFile="~/smartMISMaster.Master" %>


<asp:Content ID="userManagementContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
    <ContentTemplate>
    <link rel="stylesheet" href="../Style/userManagement.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
  
  
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= umIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= umIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    <table class="managementTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="managementFirstCol"></td>
            <td class="managementSecondCol"></td>
            <td class="managementThirdCol"></td>
            <td class="managementForthCol"></td>
        </tr>
        <tr>               
            <td colspan="4">            
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Pick a Task...</p>
                </div>
            </td>           
        </tr>
        <tr>
            <td>
                <div id="passChangeNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                    <table>
                        <tr>
                            <td>
                                <img id="passChangeNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                            </td>
                            <td>
                                <asp:Label id="passChangeNotifyLabel" runat="server" Text="Password changed successfully."></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        
        </tr>
        <tr>
            <td colspan="2">
                <ul class ="list">
                    <li><a href="userRegistration.aspx" class="task">Create an account</a></li>
                    <li><a href="javascript:void();" onclick="javascript:revealModal('modalPage')" class="task">Change my Password</a></li>
                </ul>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel" align="left" colspan="4">
                 <div id="modalPage">
                    <div class="modalBackground">
                        </div>
                        <div class="modalContainer">
                            <div class="modal">
                                <div class="modalTop"><a href="javascript:hideModal('modalPage')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                <div class="modalBody">
                                    <table class="innerTable" cellspacing="0">
                                        <tr>
                                            <td class="masterLabel">Old Password :</td>
                                            <td class="masterTextBox">
                                                <asp:TextBox ID="userManagemetOldPasswordTextBox" TextMode="Password" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                                <td><span class="errorSpan">*</span></td>
                                                 <td>
                                             <asp:RequiredFieldValidator ID="oldPassReqFieldValidator" runat="server" CssClass="reqFieldValidator"
                                            ControlToValidate="userManagemetOldPasswordTextBox" ErrorMessage="Required"> </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="masterLabel">New Password :</td>
                                            <td class="masterTextBox">
                                                <asp:TextBox ID="userManagemetNewPasswordTextBox" TextMode="Password" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                                <td>
                                            <span class="errorSpan">*</span></td>
                                             <td>
                                             <asp:RequiredFieldValidator ID="newPassReqFieldValidator" runat="server" CssClass="reqFieldValidator"
                                            ControlToValidate="userManagemetNewPasswordTextBox" ErrorMessage="Required"> </asp:RequiredFieldValidator>                                
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="masterLabel">Re New Password :</td>
                                            <td class="masterTextBox">
                                                <asp:TextBox ID="userManagemetReNewPasswordTextBox" TextMode="Password" runat="server" Width="80%"></asp:TextBox>
                                            </td>
                                                <td>
                                            <span class="errorSpan">*</span></td>
                                            <td>
                                             <asp:RequiredFieldValidator ID="confirmPassReqFieldValidator" runat="server" CssClass="reqFieldValidator"
                                            ControlToValidate="userManagemetReNewPasswordTextBox" ErrorMessage="Required"> </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                          <tr>
                                            <td colspan="2">
                                                <asp:Button ID="userManagemetSavePasswordButton" runat="server" CssClass="masterButton" Text="Save" CausesValidation="true" OnClick="Button_Click" />&nbsp;
                                                <asp:Button ID="userManagemetCancelPasswordButton" runat="server" CssClass="masterButton" Text="Cancel" OnClientClick="javascript:hideModal('modalPage'); return false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>            
                            
            </td>
            <td colspan="4">
                <div id="modalPageForDelete">
                    <div class="modalBackground">
                    </div>
                    <div class="modalContainer">
                        <div class="modal">
                            <div class="modalTop"><a href="javascript:hideModal('modalPageForDelete')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
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
                                            <asp:Label ID="areaMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete this User?"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="umDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                            <asp:Button ID="umDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPageForDelete'); return false" Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </td>
            <td align="center">
                <td>
                  <asp:HiddenField ID="mannIDHidden" runat="server" Value="0" />
                    <asp:Timer ID="passChangeNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                          ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="umIDHidden" runat="server" Value="0" Visible="true"  />
                     <asp:HiddenField ID="userIDHiddenField" runat="server" Value="0" Visible="true"  />
                </td> 
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                 <asp:Label ID="umIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="true"></asp:Label>
                    <p class="masterHeaderTagline">Pick an account to change...</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Panel ID="usermgmtPanel" runat="server" ScrollBars="Vertical" Height="200px" CssClass="panel" >
                    <asp:GridView ID="umGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="Gridview_RowBound"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />                                   
                    <Columns>
                      <asp:TemplateField HeaderText="" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <img alt="users" src="../Images/user.png" class="userImg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="umIDGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Users" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="umUserIDGridNameLabel" runat="server" Text='<%# Eval("userID") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="ManningID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="umUserMannIDGridNameLabel" runat="server" Text='<%# Eval("manningID") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>                    
                    <Columns>
                        <asp:TemplateField HeaderText="Roles Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                               <asp:BulletedList ID="bulletedListRoles" runat="server" DataTextField="roleID" DataValueField="roleID" CssClass="gridViewItems"></asp:BulletedList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>            
                      
                 <Columns>
                        <asp:TemplateField HeaderText="" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                            <ItemTemplate>
                             <asp:ImageButton ID="usermgmtEditButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />                                                                    
                             <asp:ImageButton ID="usermgmtGridDeleteButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPageForDelete'); javascript:setID(this.value); return false;" />
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
   </asp:UpdatePanel>
</asp:Content>
