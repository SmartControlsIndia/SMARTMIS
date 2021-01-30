<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productionPlanningTest.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.productionPlanningTest" %>

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
            <td  colspan="4">
                <asp:MultiView ID="productionPlanningMultiVIew" runat="server" ActiveViewIndex="0">
                    <asp:View ID="productionPlanningView1" runat="server">
                        <table class="inputTable" align="center" cellpadding="0" cellspacing="0">
                            <tr >
                                <td class="inputFirstCol"></td>           
                                <td class="inputSecondCol"></td>
                                <td class="inputThirdCol"></td>
                                <td class="inputForthCol"></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Calendar ID="productionPlanningCalender" runat="server" BackColor="White" 
                                        BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" 
                                        DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" 
                                        ForeColor="#003399" Height="200px" Width="220px">
                                        <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                        <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                        <WeekendDayStyle BackColor="#CCCCFF" />
                                        <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                        <OtherMonthDayStyle ForeColor="#999999" />
                                        <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                        <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                        <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" 
                                            Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                                    </asp:Calendar> 
                                </td>
                                <td colspan="3" rowspan="2">
                                    <asp:ListView ID="productionPlanningWCListView" runat="server" GroupItemCount="4">
                                        <LayoutTemplate>
                                            <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                                        </LayoutTemplate>
                                        <GroupTemplate>
                                            <table>
                                                <tr>
                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                                </tr>
                                            </table>
                                        </GroupTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <div>
                                                    <asp:Button ID="productionPlanningWCButton" runat="server" CssClass="inputWCListButton" Text="abc" />
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="productionPlanningProcessListBox" runat="server" SelectionMode="Single">
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </td>
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
    </table>
    </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
