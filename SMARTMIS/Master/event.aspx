<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="event.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS._event" %>

<asp:Content ID="eventContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
     <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= eventIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= eventIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
    <asp:ScriptManager ID="eventScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="eventUpdatePanel" runat="server">
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
                    <p class="masterHeaderTagline">Event</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Event ID :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="eventIDTextBox" runat="server" Width="80%"></asp:TextBox>
                 <asp:Label ID="eventIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="eventIDReqFieldValidator" runat="server" 
                    ControlToValidate="eventIDTextBox" ErrorMessage="Event ID is Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Category :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="eventCategoryNameDropDownList" runat="server" Width="82%" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                </asp:DropDownList>
                <asp:Label ID="eventCategoryIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="eventDescReqFieldValidator" runat="server" 
                    ControlToValidate="eventCategoryNameDropDownList" 
                    ErrorMessage="Catagory is Required">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Event Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="eventNameTextBox" runat="server" Width="80%"></asp:TextBox>
                 
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="eventNameReqFieldValidator" runat="server" 
                    ControlToValidate="eventNameTextBox" ErrorMessage="Event Name Required">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Description :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="eventDescriptionTextBox" runat="server" Width="80%" 
                    TextMode="MultiLine" Rows="3">
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
                        <asp:Button ID="eventSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="eventCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="eventMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Event."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="eventDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="eventDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="eventNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="eventIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="eventNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="eventNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="eventNotifyLabel" runat="server" Text="Event saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Event List</p>
                        </div>
                    </td>
                </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:30%; padding:5px">Event Name</td>
                    <td class="gridViewHeader" style="width:70%; padding:5px">Description</td>
                </tr>
                </table>
                <asp:Panel ID="eventPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="eventGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="eventGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
 
                        <Columns>
                            <asp:TemplateField HeaderText="eventID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="eventIDGridIDLabel" runat="server" Text='<%# Eval("eventID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="eventID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="eventCategoryGridNameLabel" runat="server" Text='<%# Eval("categoryName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns><Columns>
                            <asp:TemplateField HeaderText="categoryID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="eventCategoryIDGridIDLabel" runat="server" Text='<%# Eval("categoryID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Event Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="eventGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                                <ItemTemplate>
                                    <asp:Label ID="eventGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="eventGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="eventGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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