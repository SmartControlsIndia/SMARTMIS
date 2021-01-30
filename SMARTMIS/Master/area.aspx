<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="area.aspx.cs" Inherits="SmartMIS.AreaCenter" %>


<asp:Content ID="areaContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
     <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= areaIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= areaIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>


    <asp:ScriptManager ID="areaScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="areaUpdatePanel" runat="server">
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
                            <p class="masterHeaderTagline">Area</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Area Name :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="areaNameTextBox" runat="server" Width="80%"></asp:TextBox>
                        <asp:Label ID="areaIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="areaNameReqFieldValidator" runat="server" CssClass="reqFieldValidator"
                            ControlToValidate="areaNameTextBox" ErrorMessage="Area Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Description :</td>
                    <td>
                        <asp:TextBox ID="areaDescriptionTextBox" runat="server" TextMode="MultiLine" 
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
                        <asp:Button ID="areaSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="areaCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="areaMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Unit."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="areaDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="areaDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                        <asp:Timer ID="areaNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                            ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        <asp:HiddenField ID="areaIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="areaNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="areaNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="areaNotifyLabel" runat="server" Text="Area saved successfully."></asp:Label>
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
                            <p class="masterHeaderTagline">Area List</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                        <tr>
                            <td class="gridViewHeader" style="width:30%; padding:5px">Area Name</td>
                            <td class="gridViewHeader" style="width:60%; padding:5px">Description</td>
                            <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="areaPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                            <asp:GridView ID="areaGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="areaGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="areaGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:Label ID="areaGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="areaGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                            <asp:ImageButton ID="areaGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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