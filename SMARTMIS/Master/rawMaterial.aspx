<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rawMaterial.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.rawMaterialMaster1" %>

<asp:Content ID="rawMaterialContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
        <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= rawMaterialIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= rawMaterialIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
    <asp:ScriptManager ID="rawMaterialScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="rawMaterialUpdatePanel" runat="server">
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
                    <p class="masterHeaderTagline">Raw Material</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Work Center Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="rawMaterialWCNameDropDownList" runat="server" Width="82%" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" >
                </asp:DropDownList>
                 <asp:Label ID="rawMaterialWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                  <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rawMaterialWorkCenterNameReqFieldValidator" runat="server" 
                    ErrorMessage="WorkCenter Name is required!" 
                    ControlToValidate="rawMaterialWCNameDropDownList"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="masterLabel">Product Type :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="rawMaterialProductCodeDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="rawMaterialProductIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rawMaterialProductTyeNameReqFieldValidator" runat="server" 
                    ErrorMessage="ProductType is required!" 
                    ControlToValidate="rawMaterialProductCodeDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        <tr>
            <td class="masterLabel">Raw Material Name:</td>
            <td>
                <asp:TextBox ID="rawMaterialNameTextBox" runat="server" Width="80%"></asp:TextBox>
                <asp:Label ID="rawMaterialIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="rawMaterialNameTextBoxNameReqFieldValidator" runat="server" 
                    ErrorMessage="Raw Matarial Name is required!" 
                    ControlToValidate="rawMaterialNameTextBox"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Description:</td>
            <td>
                <asp:TextBox ID="rawMaterialDescriptionTextBox" runat="server" Width="80%" Rows="3" TextMode="MultiLine"></asp:TextBox>
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
                        <asp:Button ID="rawMaterialSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="rawMaterialCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="rawMaterialMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete RawMaterial."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="rawMaterialDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="rawMaterialDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="rawMaterialNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="rawMaterialIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="rawMaterialNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="rawMaterialNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="rawMaterialNotifyLabel" runat="server" Text="raw aterial saved successfully."></asp:Label>
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
                    <p class="masterHeaderTagline">Raw Material List</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Work Center Name</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Product Type</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Raw Material Name</td>
                        <td class="gridViewHeader" style="width:22%; padding:5px">Description</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                    </tr>
                </table>
                <asp:Panel ID="rawMaterialPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="rawMaterialGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="wcID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="productID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridProductTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                         
                       
                        <Columns>
                            <asp:TemplateField HeaderText="wcName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="productCODE" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridProductLabel" runat="server" Text='<%# Eval("productTypeName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="RawMaterialName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="rawMaterialGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>         
                                
                        
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="rawMaterialGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="rawMaterialGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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