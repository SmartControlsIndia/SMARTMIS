<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="processCheckParameter.aspx.cs" Inherits="SmartMIS.processCheckParametersInput" %>
<asp:Content ID="inputProcessCheckParameterContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
 
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
    
      <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= inputProcessCheckParameterIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= inputProcessCheckParameterIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
     <asp:ScriptManager ID="inputProcessCheckParameterScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="inputProcessCheckParameterUpdatePanel" runat="server">
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
                    <p class="masterHeaderTagline">Process Check Parameters Input</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Work Center Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="inputProcessCheckParameterWCNameDropDownList" runat="server" Width="82%" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" >
                </asp:DropDownList>
                 <asp:Label ID="inputProcessCheckParameterWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                  <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="inputProcessCheckParameterWorkCenterNameReqFieldValidator" runat="server" 
                    ErrorMessage="WorkCenter Name is required!" 
                    ControlToValidate="inputProcessCheckParameterWCNameDropDownList"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="masterLabel">Product Type :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="inputProcessCheckParameterProductTypeDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="inputProcessCheckParameterProductTypeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="inputProcessCheckParameterProductTyeNameReqFieldValidator" runat="server" 
                    ErrorMessage="ProductType is required!" 
                    ControlToValidate="inputProcessCheckParameterProductTypeDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        
         <tr>
            <td class="masterLabel">Recipe Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="inputProcessCheckParameterRecipeNameDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="inputProcessCheckParameterRecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td><asp:RequiredFieldValidator ID="inputProcessCheckParameterRecipeNameReqFieldValidator" runat="server" 
                    ErrorMessage="ProductType is required!" 
                    ControlToValidate="inputProcessCheckParameterRecipeNameDropDownList"></asp:RequiredFieldValidator>
             </td>
        </tr>
        
        
        <tr>
            <td class="masterLabel">Parameter Name:</td>
            <td>
                <asp:DropDownList ID="inputProcessCheckParameterNameDropDownList" runat="server" Width="82%"
                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="inputProcessCheckParameterMasterIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="inputProcessCheckParameterNameDropDownReqFieldValidator" runat="server" 
                    ErrorMessage="Parameter Name is required!" 
                    ControlToValidate="inputProcessCheckParameterNameDropDownList"></asp:RequiredFieldValidator>
            </td>
        </tr>
        
         <tr>
            <td class="masterLabel">Parameter Value:</td>
            <td>
                <asp:TextBox ID="inputProcessCheckParameterValueTextBox" runat="server" Width="80%"></asp:TextBox>
                <asp:Label ID="inputProcessCheckParameterIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="inputProcessCheckParameterValueTextBoxReqFieldValidator" runat="server" 
                    ErrorMessage="Parameter Value is required!" 
                    ControlToValidate="inputProcessCheckParameterValueTextBox"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">DateAndTime:</td>
            <td>
                <asp:TextBox ID="inputProcessCheckParameterDateTextBox" runat="server" Width="80%"></asp:TextBox>
                 <asp:Image ID="InputCalanderImage" runat="server" ImageUrl="~/Images/calender.png" CssClass="masterCalenderButton" />
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="parameterInput Date is required!" 
                    ControlToValidate="inputProcessCheckParameterDateTextBox"></asp:RequiredFieldValidator>
            </td>
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
                        <asp:Button ID="inputProcessCheckParameterSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="inputProcessCheckParameterCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
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
                                                    <asp:Label ID="inputProcessCheckParameterMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete ParameterValue."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="inputProcessCheckParameterDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="inputProcessCheckParameterDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                    <asp:Timer ID="inputProcessCheckParameterNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                        ontick="NotifyTimer_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="inputProcessCheckParameterIDHidden" runat="server" Value="0" />
                </td>
                    <td align="center">
                        <div id="inputProcessCheckParameterNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="inputProcessCheckParameterNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="inputProcessCheckParameterNotifyLabel" runat="server" Text="inputProcessCheckParameter saved successfully."></asp:Label>
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
                        <td class="gridViewHeader" style="width:15%; padding:5px">Work Center Name</td>
                        <td class="gridViewHeader" style="width:15%; padding:5px">Product Type</td>
                          <td class="gridViewHeader" style="width:15%; padding:5px">Recipe Name</td>
                        <td class="gridViewHeader" style="width:15%; padding:5px">Parameter Name</td>
                        <td class="gridViewHeader" style="width:15%; padding:5px">Parameter Value</td>
                         <td class="gridViewHeader" style="width:15%; padding:5px">DateAndTime</td>
                          <td class="gridViewHeader" style="width:8%; padding:5px"></td>
                    </tr>
                </table>
             <asp:Panel ID="inputProcessCheckParameterPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="inputProcessCheckParameterGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="wcID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                        <Columns>
                            <asp:TemplateField HeaderText="productID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridProductTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         <Columns>
                            <asp:TemplateField HeaderText="recipeID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridRecipeIDLabel" runat="server" Text='<%# Eval("recipeID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                                          
                        <Columns>
                            <asp:TemplateField HeaderText="recipeID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParametermMasterGridIDLabel" runat="server" Text='<%# Eval("processCheckParameterID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                              
                        <Columns>
                            <asp:TemplateField HeaderText="wcName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="productType" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridProductTypeLabel" runat="server" Text='<%# Eval("productTypeName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="recipeName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridRecipeNameLabel" runat="server" Text='<%# Eval("recipeName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="ParameterName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridNameLabel" runat="server" Text='<%# Eval("ParameterName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="inputProcessCheckParameterValue" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridValueLabel" runat="server" Text='<%# Eval("ParameterValue") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns> 
                         
                            <Columns>
                            <asp:TemplateField HeaderText="inputProcessCheckParameterDate" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="inputProcessCheckParameterGridDateLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>      
                        
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="inputProcessCheckParameterGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="inputProcessCheckParameterGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                