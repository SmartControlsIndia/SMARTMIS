<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reason.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.reason" %>


<asp:Content ID="reasonContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= reasonIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= reasonIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>


    <asp:ScriptManager ID="reasonScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="reasonUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">Reasons</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Reason Name :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="reasonNameTextBox" runat="server" Width="80%" AutoComplete="off"></asp:TextBox>
                        <asp:Label ID="reasonIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="reasonNameReqFieldValidator" runat="server" 
                            ControlToValidate="reasonNameTextBox" CssClass="reqFieldValidator"
                            ErrorMessage="Reason Name is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Workcenter Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="reasonWCNameDropDownList" runat="server" AutoPostBack="true"
                            CssClass="masterDropDownList" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="reasonWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="wcNameReqFieldValidator" runat="server" 
                            ControlToValidate="reasonWCNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="Workcenter Name is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Description :</td>
                    <td>
                        <asp:TextBox ID="reasonDescriptionTextBox" runat="server" TextMode="MultiLine" 
                            Rows="3" Width="80%"></asp:TextBox>
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
                    <td></td>
                    <td>
                        <asp:Button ID="reasonSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="reasonCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                     <asp:Label ID="reasonMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Reason."></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="reasonDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="reasonDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                        <asp:Timer ID="reasonNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                            ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        <asp:HiddenField ID="reasonIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="reasonNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="reasonNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="reasonNotifyLabel" runat="server" Text="Reason saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Reason List</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                        <tr>
                            <td class="gridViewHeader" style="width:20%; padding:5px">Reason Name</td>
                            <td class="gridViewHeader" style="width:20%; padding:5px">Workcenter Name</td>
                            <td class="gridViewHeader" style="width:50%; padding:5px">Description</td>
                            <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="reasonPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                            <asp:GridView ID="reasonGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="reasonGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Reason Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="reasonGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="reasonGridWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="reasonGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="reasonGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="reasonGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                            <asp:ImageButton ID="reasonGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
