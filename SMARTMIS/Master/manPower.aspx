<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manPower.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Master.manPower1" %>

<asp:Content ID="manPowerContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(authentication, value)
        {
            if (authentication == "True") {
                document.getElementById('<%= manPowerIDHidden.ClientID %>').value = value.toString();
                revealModal('modalPage');
            }
            else {
                revealModal('modalPageForAuthentication');
            }
        }

        function removeDropDownItem(id)
        {
            var objDropDownList = id;
            for (i = objDropDownList.length - 1; i >= 0; i--) {
                if (objDropDownList.options[i].value == "".trim()) {
                    objDropDownList.options[i] = null;
                }
            }
        }
    </script>
 <asp:ScriptManager ID="manPowerScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="manPowerUpdatePanel" runat="server">
        <ContentTemplate>   
            <table class="innerTable" cellspacing="1">
                <tr>
                    <td class="gridViewHeader" style="width:40%; padding:5px; text-align:center">SAP Code</td>
                    <td class="gridViewHeader" style="width:40%; padding:5px; text-align:center">Workcenter Name</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px; text-align:center">Shift</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px; text-align:center"></td>
                </tr>
                <tr>
                    <td colspan="1">
                        <asp:Button ID="manPowerMagicButton" runat="server" CssClass="masterHiddenButton" Text="magicButton" />
                    </td>
                    <td colspan="5">
                        <div id="modalPageForReview">
                            <div class="modalBackground">
                            </div>
                            <div class="modalContainer">
                            <div class="modal">
                                <div class="modalTop"><a href="javascript:hideModal('modalPageForReview')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                <div class="modalBody">
                                     <table class="innerTable" cellspacing="0">
                                        <tr>
                                            <td style="width: 30%"></td>
                                            <td style="width: 50%"></td>
                                            <td style="width: 20%"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="masterLabel">Workcenter :</td>
                                            <td>
                                                <asp:DropDownList ID="manPowerWCNameDropDownList" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList"
                                                    onChange="javascript:removeDropDownItem(this)" Width="98%"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="manPowerWCIDLabel" runat="server" CssClass="masterHiddenLabel" Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="masterLabel">Shift :</td>
                                            <td>
                                                <asp:DropDownList ID="manPowerShiftDropDownList" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList"
                                                    onChange="javascript:removeDropDownItem(this)" Width="98%"></asp:DropDownList>
                                             </td>
                                             <td>
                                                
                                             </td>
                                         </tr>
                                         <tr>
                                            <td colspan="3">&nbsp;</td>
                                         </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:HiddenField ID="manPowerManningIDHidden" runat="server" Value="0"></asp:HiddenField>
                                                <asp:Button ID="manPowerReviewDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                <asp:Button ID="manPowerReviewDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPageForReview'); return false" Text="Cancel" />
                                            </td>
                                        </tr>
                                     </table>
                                </div>
                             </div>
                         </div>
                     </div>
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
                                                    <asp:Label ID="manPowerMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete workcenter."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:HiddenField ID="manPowerIDHidden" runat="server" Value="0"></asp:HiddenField>
                                                    <asp:Button ID="manPowerDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="manPowerDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <td colspan="6">
                        <asp:Panel ID="manPowerSapCodePanel" runat="server" >
                            <asp:GridView ID="manPowerSapCodeGridView" runat="server" AutoGenerateColumns="False" 
                                Width="100%" CellPadding="2" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField  HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="manPowerSapCodeIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <table class="innerTable" cellpadding="5">
                                                <tr>
                                                    <td style="width: 10%; text-align:right">
                                                        <asp:Image id="manPowerGridStatusImage" runat="server" AlternateText="" ImageUrl="../Images/present.png" />
                                                    </td>
                                                    <td style="width: 40%">
                                                        <asp:Label ID="manPowerGridSapCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                    </td>
                                                    <td style="width: 40%; text-align:left">
                                                        <asp:Label ID="manPowerGridFirstNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>
                                                        <asp:Label ID="manPowerGridLastNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:Label ID="manPowerGridManningIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="masterHiddenLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField  HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                                        <ItemTemplate>
                                            
                                            <%--Child gridview--%>
                                            <asp:GridView ID="manPowerInnerGridView" runat="server" AutoGenerateColumns="False" 
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="manPowerInnerGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" >
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="manPowerInnerGridCheckBox" runat="server" ></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="manPowerInnerGridWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="manPowerInnerGridShiftLabel" runat="server" Text='<%# Eval("shiftName") %>' CssClass="gridViewItems"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>                                    
                                                <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="#FFFFFF" />
                                                </asp:GridView>
                                                <table class="masterTable" cellpadding="3">
                                                    <tr>
                                                        <td  style="width: 85%">
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="manPowerGridAddImageButton" runat="server" Text="Add" AlternateText="104#102" CausesValidation="false" ToolTip="Add" CssClass="gridImageButton" ImageUrl="~/Images/plus.png" Value='<%# Eval("iD") %>' OnClick="ImageButton_Click" />
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="manPowerInnerGridEditImageButton" runat="server" Text="Edit" AlternateText="104#102" ToolTip="Edit" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                                        </td>
                                                        <td  style="width: 5%">
                                                            <asp:ImageButton ID="manPowerInnerGridDeleteImageButton" runat="server" Text="Delete" AlternateText="104#102" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" OnClick="ImageButton_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-CssClass="masterTable" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <div id="hostNameMessageDiv" runat="server" class="notifyDiv" visible="false">
                                                <img alt="Close" src="../Images/tick.png" class="notifyImage" />
                                                <asp:Label ID="manPowerSapCodeNotifyLabel" runat="server" CssClass="notifyLabel" Text="No errors found!"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
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


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  