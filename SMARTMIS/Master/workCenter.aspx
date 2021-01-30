<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workCenter.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.workCenter" %>

<asp:Content ID="workCenterContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
       <script language="javascript" type="text/javascript">

           function setID(authentication, value) {
               if (authentication == "True") {
                   document.getElementById('<%= wcIDHidden.ClientID %>').value = value.toString();
                   document.getElementById('<%=wcIDLabel.ClientID %>').innerHTML = value;
                   revealModal('modalPage');
               }
               else {
                   revealModal('modalPageForAuthentication');
               }
           }
        
    </script>
  
        
  
    
    <asp:ScriptManager ID="wcScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="wcUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">Work Center</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Work center SAP Code :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="wcNameTextBox" runat="server" Width="80%" AutoComplete="off"></asp:TextBox>
                        <asp:Label ID="wcIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="wcNameFieldValidator" runat="server" 
                            ControlToValidate="wcNameTextBox" CssClass="reqFieldValidator"
                            ErrorMessage="Work Center Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                   <td class="masterLabel"> Process Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="wcProcessNameDropDownList" runat="server" 
                            CssClass="masterDropDownList" Width="82%" AutoPostBack="true"
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="wcProcessIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="wcProcessNameFieldValidator" runat="server" 
                            ControlToValidate="wcProcessNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="Process Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Description :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="wcDescriptionTextBox" runat="server" TextMode="MultiLine" 
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
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="wcSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="wcCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="wcMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Workcenter."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="wcDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="wcDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="wcNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="wcIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="wcNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="wcNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="wcNotifyLabel" runat="server" Text="Work center saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Work Center List</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                        <tr>
                            <td class="gridViewHeader" style="width:25%; padding:5px">Workcenter Name</td>
                            <td class="gridViewHeader" style="width:25%; padding:5px">Process Name</td>
                            <td class="gridViewHeader" style="width:40%; padding:5px">Description</td>
                            <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="wcPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                        <asp:GridView ID="wcGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="wcGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                        <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <asp:Label ID="wcGridWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Process ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="wcGridProcessIDLabel" runat="server" Text='<%# Eval("processID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Process Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="wcGridProcessNameLabel" runat="server" Text='<%# Eval("processName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="wcGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="wcGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" AlternateText='<%# isAuthenticate(3)%>' OnClick="ImageButton_Click"/>
                                    <asp:ImageButton ID="wcGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" AlternateText='<%# isAuthenticate(4)%>' Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage');javascript:setID(this.alt, this.value); return false;" />
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
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                </table>
        </ContentTemplate>
        <Triggers>
         <asp:PostBackTrigger ControlID="wcCancelButton" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
