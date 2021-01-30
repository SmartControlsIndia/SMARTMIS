<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oldProductionPlanning.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.productionPlanning" %>

<asp:Content ID="productionPlanningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />

     <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= prodPlanIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= prodPlanIDLabel.ClientID %>').innerHTML = value;
        }       
    </script>
    

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
   
<table class="inputTable" align="center" cellpadding="0" cellspacing="0">
        <tr >
            <td class="inputFirstCol"></td>           
            <td class="inputSecondCol"></td>
            <td class="inputThirdCol"></td>
            <td class="inputForthCol"></td>
        </tr>
        <tr >
            <td colspan="4">
                <div class="masterHeader" >
                    <p class="masterHeaderTagline" >Production Planning</p>
                </div>
            </td>           
        </tr>
        <tr>
            <td>
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
                                                     <asp:Label ID="prodPlanMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Planning Record." ></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="prodPlanDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="prodPlanDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false;" Text="Cancel" />
                                                 </td>
                                             </tr>
                                         </table>
                                     </div>
                                 </div>
                             </div>
                         </div>   
            &nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Workcenter Name :</td>
            <td class="masterTextBox"">
                <asp:DropDownList ID="productionPlanningWCNameDropDownList" runat="server" 
                    Width="81%" onselectedindexchanged="DropDownList_SelectedIndexChanged" 
                    AutoPostBack="True"></asp:DropDownList>
                    <asp:Label ID="ProdPlanningWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                    <asp:Label ID="ProdPlanningProcessID" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
               <asp:RequiredFieldValidator ID="ppWCNameFieldValidator" runat="server" 
                            ControlToValidate="productionPlanningWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required"></asp:RequiredFieldValidator> 
                       
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Product Type :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="productionPlanningProductTypeDropDownList" onselectedindexchanged="DropDownList_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="81%" >
                </asp:DropDownList>
                <asp:Label ID="productTypeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                
            </td>
            <td>
                <span class="errorSpan">*</span>               
            </td>
            <td>
               <asp:RequiredFieldValidator ID="ppProdTypeFieldValidator0" runat="server" 
                            ControlToValidate="productionPlanningProductTypeDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Product Type  is Required"></asp:RequiredFieldValidator> 
                       
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Recipe Code :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="productionPlanningRecipeCodeTextBox" runat="server" 
                    Width="40%" Enabled="true"></asp:TextBox>
               No.<asp:TextBox ID="productionPlanningRecipeNoText" runat="server" Width="25%" 
                    Enabled="False">
                    </asp:TextBox><asp:Label ID="recipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
            
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="ppRecipeCodeFieldValidator1" runat="server" 
                            ControlToValidate="productionPlanningRecipeCodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="RecipeCode  is Required"></asp:RequiredFieldValidator> 
                <asp:Label ID="ppRecipeNoRequiredFieldLabel" runat="server" CssClass="reqFieldValidator"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Quantity :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="productionPlanningQuantityTextBox" runat="server" Width="40%"></asp:TextBox>
                 <asp:Label ID="productionPlanningUnitOfMeasureLabel" runat="server" Width="20%" Text=""></asp:Label>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
               <asp:RequiredFieldValidator ID="ppQuantityFieldValidator1" runat="server" 
                            ControlToValidate="productionPlanningQuantityTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Product Quantity  is Required"></asp:RequiredFieldValidator> 
                       
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Date & Shift :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="productionPlanningDateTextBox" runat="server" Width="40%" 
                    Enabled="False"></asp:TextBox>
                <img alt="Calender" src="../Images/calender.png" class="calenderImg" />
                <asp:DropDownList ID="productionPlanningShiftDropDownList" runat="server" Width="28%">
                </asp:DropDownList>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
        </tr>
        
        <tr>
            <td>
                    <asp:HiddenField ID="prodPlanIDHidden" runat="server" Value="0" />
                    </td>
            <td> <asp:Label ID="prodPlanIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>&nbsp;
               <div id="prodPlanNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="prodPlanNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="prodPlanNotifyLabel" runat="server" Text="Production Planning record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                        <asp:Timer ID="prodPlanNotifyTimer" runat="server"  OnTick="NotifyTimer_Tick"
                            Interval="5000" Enabled="false" >
                        </asp:Timer>
                        </td>
            <td>
                <asp:Button ID="productionPlanningSaveButton" runat="server" 
                    CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                <asp:Button ID="productionPlanningCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Planning Details</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                    <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Workcenter</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Product Type</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Recipe Code</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Quantity</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Shift</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Date</td>     
                    
                    
                    
                    
                   
                    <td class="gridViewHeader" style="width:15%; padding:5px"></td>
                </tr>
                </table>
            <asp:Panel ID="prodPlanRolePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="prodPlanRoleGridView" runat="server" AutoGenerateColumns="False" 
                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridWCIDLabel" runat="server" Text='<%# Eval("wciD") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                        
                      <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                
                    
                       <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridProdTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                      <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridProdTypeNameLabel" runat="server" Text='<%# Eval("productType") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                      <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false"  ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridRecipeIDLabel" runat="server" Text='<%# Eval("recipeID") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                      
                      <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridRecipeNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                   <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridQuantityLabel" runat="server" Text='<%# Eval("quantity") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridShiftLabel" runat="server" Text='<%# Eval("shift") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="PRODSEQ" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="productionPlanningGridDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
             
                 <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                     <ItemTemplate>                                                
                      <asp:ImageButton ID="prodPlanGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click"  />
                      <asp:ImageButton ID="prodPlanGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>            
        </tr>
        <tr>
            <td class="masterLabel">
                &nbsp;</td>
            <td align="center">
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
    </table>
    </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        