<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productType.aspx.cs" Inherits="SmartMIS.Master.productType" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="productTypeContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
     
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
        <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= productTypeIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= productTypeIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
    <asp:ScriptManager ID="productTypeScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="productTypeUpdatePanel" runat="server">
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
                    <p class="masterHeaderTagline">Product Type</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Work Center Code :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="productTypeWCNameDropDownList" runat="server" Width="82%" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" >
                </asp:DropDownList>
                 <asp:Label ID="productTypeWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                  <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="productTypeWorkCenterNameReqFieldValidator" runat="server" 
                    ErrorMessage="WorkCenter Name is required!" 
                    ControlToValidate="productTypeWCNameDropDownList"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="masterLabel">Unit Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="productTypeUnitNameDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="productTypeUnitIdLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="productTypeProductTyeNameReqFieldValidator" runat="server" 
                    ErrorMessage="Unit Name is required!" 
                    ControlToValidate="productTypeUnitNameDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        <tr>
            <td class="masterLabel">Component :</td>
            <td>
                <asp:TextBox ID="productTypeNameTextBox" runat="server" Width="80%"></asp:TextBox>
                <asp:Label ID="productTypeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="productTypeNameTextBoxNameReqFieldValidator" runat="server" 
                    ErrorMessage="Product Name is required!" 
                    ControlToValidate="productTypeNameTextBox"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Description:</td>
            <td>
                <asp:TextBox ID="productTypeDescriptionTextBox" runat="server" Width="80%" Rows="3" TextMode="MultiLine"></asp:TextBox>
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
                        <asp:Button ID="productTypeSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="productTypeCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="productTypeMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete productName."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="productTypeDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="productTypeDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="productTypeNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="productTypeIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="productTypeNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="productTypeNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="productTypeNotifyLabel" runat="server" Text="ProductName saved successfully."></asp:Label>
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
                    <p class="masterHeaderTagline">ProductType List</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Work Center Name</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Unit Name</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">ProductType Name</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Description</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                    </tr>
                </table>
                <asp:Panel ID="productTypePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="productTypeGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="wcID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="productID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGriUnitIDLabel" runat="server" Text='<%# Eval("unitID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                         
                       
                        <Columns>
                            <asp:TemplateField HeaderText="wcName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="productCODE" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridUnitNameLabel" runat="server" Text='<%# Eval("unitName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="productTypeName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="productTypeGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>         
                                
                        
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="productTypeGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="productTypeGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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