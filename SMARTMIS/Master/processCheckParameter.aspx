<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="processCheckParameter.aspx.cs" Inherits="SmartMIS.processCheckParameters" %>


<asp:Content ID="processCheckParameterContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
    
      <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= processCheckParameterIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= processCheckParameterIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
     <asp:ScriptManager ID="processCheckParameterScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="processCheckParameterUpdatePanel" runat="server">
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
                    <p class="masterHeaderTagline">Process Check Parameters</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Work Center Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="processCheckParameterWCNameDropDownList" runat="server" Width="82%" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" >
                </asp:DropDownList>
                 <asp:Label ID="processCheckParameterWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                  <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="processCheckParameterWorkCenterNameReqFieldValidator" runat="server" 
                    ErrorMessage="WorkCenter Name is required!" 
                    ControlToValidate="processCheckParameterWCNameDropDownList"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="masterLabel">Product Type :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="processCheckParameterProductTypeDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="processCheckParameterProductIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="processCheckParameterProductTyeNameReqFieldValidator" runat="server" 
                    ErrorMessage="ProductType is required!" 
                    ControlToValidate="processCheckParameterProductTypeDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        
         <tr>
            <td class="masterLabel">Recipe Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="processCheckParameterRecipeNameDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="processCheckParameterRecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="processCheckParameterRecipeNameReqFieldValidator" runat="server" 
                    ErrorMessage="ProductType is required!" 
                    ControlToValidate="processCheckParameterRecipeNameDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        
        
        <tr>
            <td class="masterLabel">Parameter Name:</td>
            <td>
                <asp:TextBox ID="processCheckParameterNameTextBox" runat="server" Width="80%"></asp:TextBox>
                <asp:Label ID="processCheckParameterIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="processCheckParameterNameTextBoxNameReqFieldValidator" runat="server" 
                    ErrorMessage="Raw Matarial Name is required!" 
                    ControlToValidate="processCheckParameterNameTextBox"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
           <td class="masterLabel">description :</td>
            <td>
                <asp:TextBox ID="processCheckParameterDescriptionTextBox" TextMode="MultiLine" Rows="3" Width="80%" runat="server"></asp:TextBox>
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
                        <asp:Button ID="processCheckParameterSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="processCheckParameterCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="processCheckParameterMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Category."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="processCheckParameterDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="processCheckParameterDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="processCheckParameterNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="processCheckParameterIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="processCheckParameterNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="processCheckParameterNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="processCheckParameterNotifyLabel" runat="server" Text="ProcessCheckParameter saved successfully."></asp:Label>
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
                    <p class="masterHeaderTagline">Parameters List</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="gridViewHeader" style="width:18%; padding:5px">Work Center Name</td>
                        <td class="gridViewHeader" style="width:18%; padding:5px">Product Type</td>
                          <td class="gridViewHeader" style="width:18%; padding:5px">Recipe Name</td>
                        <td class="gridViewHeader" style="width:18%; padding:5px">Parameter Name</td>
                        <td class="gridViewHeader" style="width:18%; padding:5px">Description</td>
                          <td class="gridViewHeader" style="width:8%; padding:5px"></td>
                    </tr>
                </table>
             <asp:Panel ID="processCheckParameterPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="processCheckParameterGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="wcID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="productID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridProductTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         <Columns>
                            <asp:TemplateField HeaderText="recipeID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridRecipeIDLabel" runat="server" Text='<%# Eval("recipeID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                                          
                       
                        <Columns>
                            <asp:TemplateField HeaderText="wcName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="productType" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridProductTypeLabel" runat="server" Text='<%# Eval("productTypeName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="recipeName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridRecipeNameLabel" runat="server" Text='<%# Eval("recipeName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                         <Columns>
                            <asp:TemplateField HeaderText="processCheckParameterName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label ID="processCheckParameterGridDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>         
                                
                        
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="processCheckParameterGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="processCheckParameterGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    