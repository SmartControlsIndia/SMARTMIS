<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.role" %>

<asp:Content ID="roleMasterContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
       <script language="javascript" type="text/javascript">

           function setID(authentication, value) 
           {
               if (authentication == "True")
                {
                   document.getElementById('<%=roleIDHidden.ClientID %>').value = value.toString();
                   document.getElementById('<%=roleIDHidden.ClientID %>').innerHTML = value;
                   revealModal('modalPage');
               }
               else 
               {
                   revealModal('modalPageForAuthentication');
               }
           }
        
    </script>
  
        
  
    
    <asp:ScriptManager ID="roleScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="roleUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="masterFirstCol"></td>
                    <td class="masterSecondCol"></td>
                    <td class="masterThirdCol"></td>
                    <td class="masterForthCol"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">User Roles Master</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Role Name :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="roleNameTextBox" runat="server" Width="80%" AutoComplete="off"></asp:TextBox>
                        <asp:Label ID="roleIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="roleNameFieldValidator" runat="server" 
                            ControlToValidate="roleNameTextBox" CssClass="reqFieldValidator"
                            ErrorMessage="Role Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                   <td class="masterLabel"> Modue Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="roleModuleNameDropDownList" runat="server" 
                            CssClass="masterDropDownList" Width="82%" AutoPostBack="true"
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="roleModuleIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="roleModuleFieldValidator" runat="server" 
                            ControlToValidate="roleModuleNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="Module Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Description :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="roleDescriptionTextBox" runat="server" TextMode="MultiLine" 
                            Rows="3" Width="80%">
                        </asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                
                  <td colspan="4"> 
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Select Permissions : &nbsp;&nbsp;View :&nbsp<asp:CheckBox ID="View" runat="server" /> &nbsp;&nbsp;Add :&nbsp;<asp:CheckBox ID="Add" runat="server" />&nbsp;&nbsp;Edit :&nbsp;<asp:CheckBox ID="Edit" runat="server" />&nbsp;&nbsp;Delete :&nbsp;<asp:CheckBox ID="Delete" runat="server" /></p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="roleSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="roleCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="roleMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Workcenter."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="roleDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="roleDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="roleNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="roleIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="roleNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="roleNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="roleNotifyLabel" runat="server" Text="Role  saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Role List</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                        <tr>
                            <td class="gridViewHeader" style="width:20%; padding:5px">Role Name</td>
                            <td class="gridViewHeader" style="width:20%; padding:5px">Module Name</td>
                             <td class="gridViewHeader" style="width:20%; padding:5px">Rights</td>

                            <td class="gridViewHeader" style="width:30%; padding:5px">Description</td>
                         <td class="gridViewHeader" style="width:10%; padding:5px">Edit/delete</td>

                        </tr>
                        </table>
                        <asp:Panel ID="rolePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                        <asp:GridView ID="roleGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                                onrowdatabound="Gridview_RowBound"  >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        
                        <Columns>
                        <asp:TemplateField HeaderText="role Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label ID="roleGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="module ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
                                <ItemTemplate>
                                 <asp:GridView ID="rolechildGridView" runat="server" AutoGenerateColumns="False" 
                             Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" onrowdatabound="Gridview_RowBound"  >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderText="module ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="roleGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                           <Columns>
                        <asp:TemplateField HeaderText="role Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label ID="rolechildGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="module ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="roleGridmoduleIDLabel" runat="server" Text='<%# Eval("moduleID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="module Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="roleGridModuleNameLabel" runat="server" Text='<%# Eval("moduleName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                <ItemTemplate>
                               <asp:BulletedList ID="bulletedListRoles" runat="server" DataTextField="name" DataValueField="name" CssClass="gridViewItems"></asp:BulletedList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="module Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="roleGridModuledescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                          <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="roleGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click"/>
                                    <asp:ImageButton ID="roleGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" AlternateText='<%# isAuthenticate(4)%>' Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage');javascript:setID(this.alt, this.value); return false;" />
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
         <asp:PostBackTrigger ControlID="roleCancelButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
