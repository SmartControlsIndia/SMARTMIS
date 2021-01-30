<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="mhe.aspx.cs" Inherits="SmartMIS.MHE" %>

<asp:Content ID="mheContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
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
                    <p class="masterHeaderTagline">Machine Handling Equipment:</p>
                </div>
            </td>
        </tr>
        
        <tr>
           <td class="masterLabel">MHE Code:</td>
            <td class="masterTextBox">
                <asp:TextBox ID="mheCodeTextBox" runat="server" Width="80%"></asp:TextBox>
                <asp:Label ID="mheIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                 <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td> <asp:RequiredFieldValidator ID="mheNameReqFieldValidator" runat="server" 
                    ControlToValidate="mheCodeTextBox" ErrorMessage="MHE Name is Required">
                </asp:RequiredFieldValidator></td>
        </tr>
        <tr>
           <td class="masterLabel">Description:</td>
            <td class="masterTextBox">
                <asp:TextBox ID="mheDescriptionTextBox" runat="server" TextMode="MultiLine" Rows="3" Width="80%"></asp:TextBox>
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
                        <asp:Button ID="mheSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="mheCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="mheMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete MHE."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="mheDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="mheDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                        <asp:Timer ID="mheNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                            ontick="NotifyTimer_Tick">
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
                                        <asp:Label id="mheNotifyLabel" runat="server" Text="MHE saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">MHE List</p>
                        </div>
                    </td>
                </tr>
        
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="gridViewHeader" style="width:25%; padding:5px">MHE Code</td>
                        <td class="gridViewHeader" style="width:50%; padding:5px">Description</td>      
                        <td class="gridViewHeader" style="width:30%; padding:5px"></td>
                    </tr>
                </table>
                <asp:Panel ID="mhePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="mheGridView" runat="server" AutoGenerateColumns="False" 
                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="mheGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>         
                <Columns>
                    <asp:TemplateField HeaderText="MHE Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="mheGridNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="gridViewItems"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>           
                <Columns>
                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                    <ItemTemplate>
                        <asp:Label ID="mheGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns> 
                 <Columns>
                      <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                       <ItemTemplate>
                           <asp:ImageButton ID="mheGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
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
