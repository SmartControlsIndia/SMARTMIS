<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jobPost.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Master.jobPost" %>

<asp:Content ID="jobPostContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= jobPostIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= jobPostIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
    <asp:ScriptManager ID="jobPostScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="jobPostUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">Job Post</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Job Post Name :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="jobPostNameTextBox" runat="server" Width="80%" AutoComplete="off"></asp:TextBox>
                        <asp:Label ID="jobPostIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="jobPostNameFieldValidator" runat="server" 
                            ControlToValidate="jobPostNameTextBox" CssClass="reqFieldValidator"
                            ErrorMessage="Job Post Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                   <td class="masterLabel"> WorkCenter Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="jobPostWCNameDropDownList" runat="server" 
                            CssClass="masterDropDownList" Width="82%" AutoPostBack="true"
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="jobPostWockCenterIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="jobPostWCNameFieldValidator" runat="server" 
                            ControlToValidate="jobPostWCNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="WorkCenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Description :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="jobPostDescriptionTextBox" runat="server" TextMode="MultiLine" 
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
                        <asp:Button ID="jobPostSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="jobPostCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="jobPostMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Workcenter."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="jobPostDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="jobPostDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="jobPostNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="jobPostIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="jobPostNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="jobPostNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="jobPostNotifyLabel" runat="server" Text="Work center saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Job Post List</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                        <tr>
                            <td class="gridViewHeader" style="width:25%; padding:5px">Job Name</td>
                            <td class="gridViewHeader" style="width:25%; padding:5px">WorkCenter Name </td>
                            <td class="gridViewHeader" style="width:40%; padding:5px">Description</td>
                            <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="jobPostPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                        <asp:GridView ID="jobPostGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="jobPostGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                       
                        <Columns>
                            <asp:TemplateField HeaderText="workCenterID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="jobPostGridWorkCenterIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                        <asp:TemplateField HeaderText="job  Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <asp:Label ID="jobPostGridJobNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="jobPostGridWorkCenterNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="jobPostGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="jobPostGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="jobPostGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
