<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productionPlanning.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.productionPlanning" %>
<%@ Register Src="~/UserControl/calenderTextBox.ascx" TagName="calanderTextBox" TagPrefix="asp" %>
<asp:Content ID="productionPlanningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
        
    <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
        <asp:MultiView ID="productionPlanningMultiView" runat="server" ActiveViewIndex="0">
            <asp:View ID="productionPlanningView" runat="server">
                <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 70%"></td>
                        <td style="width: 10%"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="masterHeader">
                                <p class="masterHeaderTagline">Production Planning</p>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <span class="masterLabel">Select Date :</span>
                            <asp:calanderTextBox id="productionPlanningCalenderTextBox" runat="server" width="69%"/>
                            <br />
                            <br />
                            <span class="masterLabel">Select Workcenter :</span>
                            <asp:DropDownList ID="productionPlanningWCNameDropDownList" runat="server"
                                CssClass="masterDropDownList" Width="82%" AutoPostBack="true"
                                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="productionPlanningWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                        </td>
                        <td style="vertical-align: top">
                            <table class="innerTable" cellspacing="1">
                                <tr>
                                    <td class="gridViewHeader" style="width:20%; padding:5px" rowspan="2">Component</td>
                                    <td class="gridViewHeader" style="width:30%; padding:5px" rowspan="2">Recipe Name</td>
                                    <td class="gridViewHeader" style="width:16%; text-align: center; padding:5px" colspan="2">A</td>
                                    <td class="gridViewHeader" style="width:16%; text-align: center; padding:5px" colspan="2">B</td>
                                    <td class="gridViewHeader" style="width:16%; text-align: center; padding:5px" colspan="2">C</td>
                                    <td class="gridViewHeader" style="width:2%; text-align: center; padding:5px" rowspan="2"></td>
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Plan</td>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Actual</td>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Plan</td>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Actual</td>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Plan</td>
                                    <td class="gridViewAlternateHeader" style="width: 8%; text-align: center; padding:5px">Actual</td>
                                </tr>
                            </table>
                            <asp:Panel ID="productionPlanningPanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                                <asp:GridView ID="productionPlanningGridView" runat="server" AutoGenerateColumns="False" 
                                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="productionPlanningGridRecipeIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="productionPlanningGridComponentIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Product Type" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="productionPlanningProductTypeLabel" runat="server" Text='<%# Eval("productTypeName") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="productionPlanningRecipeNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="A" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productionPlanningGridShiftAPlanTextBox" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A")%>'
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%" >
                                                </asp:TextBox>
                                                <asp:TextBox ID="productionPlanningGridShiftAActualTextBox" runat="server" Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A")%>' Enabled=false
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%" >
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="B" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productionPlanningGridShiftBPlanTextBox" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B")%>'
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%">
                                                </asp:TextBox>
                                                <asp:TextBox ID="productionPlanningGridShiftBActualTextBox" runat="server" Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B")%>' Enabled=false
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns> 
                                    <Columns>
                                        <asp:TemplateField HeaderText="C" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productionPlanningGridShiftCPlanTextBox" runat="server"  Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C")%>'
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%">
                                                </asp:TextBox>
                                                <asp:TextBox ID="productionPlanningGridShiftCActualTextBox" runat="server"  Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"iD"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C")%>' Enabled=false
                                                    AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" onblur="javascript:validateTextBox(this);" CssClass="gridViewTextBox" Width="40%">
                                                </asp:TextBox>
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
                        <td align="center" valign="top">
                            <asp:Button ID="productionPlanningPriorityViewButton" runat="server" Text="Priority"
                                CssClass="masterButton" OnClick="MultiViewButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="text-align: center">
                            <asp:Button ID="productionPlanningSaveButton" runat="server" 
                                CssClass="masterButton" Text="Save" 
                                onclick="Button_Click" />&nbsp;
                            <asp:Button ID="productionPlanningCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
                        </td>
                        <td></td>
                    </tr>
                </table>            
            </asp:View>
            <asp:View ID="productionPlanningPriorityView" runat="server">
                <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 70%"></td>
                        <td style="width: 10%"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="masterHeader">
                                <p class="masterHeaderTagline">Production Priority</p>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <span class="masterLabel">Select Date :</span>
                            <asp:calanderTextBox id="productionPlanningPriorityCalenderTextBox" runat="server" width="69%"/>
                            <br />
                            <br />
                            <span class="masterLabel">Select Workcenter :</span>
                            <asp:DropDownList ID="productionPlanningPriorityWCNameDropDownList" runat="server"
                                CssClass="masterDropDownList" Width="82%" AutoPostBack="true"
                                OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="productionPlanningPriorityWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                        </td>
                        <td style="vertical-align: top">
                            <table class="innerTable" cellspacing="1">
                                <tr>
                                    <td class="gridViewHeader" style="width:33%; text-align: center; padding:5px" colspan="3">A</td>
                                    <td class="gridViewHeader" style="width:33%; text-align: center; padding:5px" colspan="3">B</td>
                                    <td class="gridViewHeader" style="width:32%; text-align: center; padding:5px" colspan="3">C</td>
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Component</td>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Recipe</td>
                                    <td class="gridViewAlternateHeader" style="width:3%; text-align: center; padding:5px"></td>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Component</td>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Recipe</td>
                                    <td class="gridViewAlternateHeader" style="width:3%; text-align: center; padding:5px"></td>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Component</td>
                                    <td class="gridViewAlternateHeader" style="width:15%; text-align: center; padding:5px">Recipe</td>
                                    <td class="gridViewAlternateHeader" style="width:2%; text-align: center; padding:5px"></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    
                                        <asp:Panel ID="productionPlanningPriorityShiftAPanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                                        
                                            <%--Gridview for Shift A--%>
                                            
                                            <asp:GridView ID="productionPlanningPriorityShiftAGridView" runat="server" AutoGenerateColumns="False" 
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="productionPlanningPriorityShiftARadioButton" runat="server" AutoPostBack="true" OnCheckedChanged="ShiftARadioButton_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftAPriorityIDLabel" runat="server" Text='<%# Container.DataItemIndex + 1 %>' ></asp:Label>
                                                            <asp:Label ID="productionPlanningPriorityShiftAIDLabel" runat="server" Text='<%# Eval("iD") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftARecipeLabel" runat="server"  Text='<%# Eval("productType") %>' CssClass="gridViewItems">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftAComponentLabel" runat="server"  Text='<%# Eval("name") %>' CssClass="gridViewItems">
                                                            </asp:Label>
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
                                    <td>
                                        <asp:ImageButton ID="productionPlanningPriorityShiftAUPImageButton" runat="server" ImageUrl="~/Images/control up.png" CssClass="masterUPDownButton" OnClick="UpDownButton_Click" />
                                        <br />
                                        <asp:ImageButton ID="productionPlanningPriorityShiftADownImageButton" runat="server" ImageUrl="~/Images/control down.png" CssClass="masterUPDownButton" OnClick="UpDownButton_Click" />
                                    </td>
                                    <td colspan="2">
                                    
                                        <asp:Panel ID="productionPlanningPriorityShiftBPanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                                        
                                            <%--Gridview for Shift B--%>
                                            
                                            <asp:GridView ID="productionPlanningPriorityShiftBGridView" runat="server" AutoGenerateColumns="False" 
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="productionPlanningPriorityShiftBRadioButton" runat="server" AutoPostBack="true" OnCheckedChanged="ShiftBRadioButton_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftBPriorityIDLabel" runat="server" Text='<%# Container.DataItemIndex + 1 %>' ></asp:Label>
                                                            <asp:Label ID="productionPlanningPriorityShiftBIDLabel" runat="server" Text='<%# Eval("iD") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftBRecipeLabel" runat="server"  Text='<%# Eval("productType") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftBComponentLabel" runat="server"  Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
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
                                    <td>
                                        <asp:ImageButton ID="productionPlanningPriorityShiftBUPImageButton" runat="server" ImageUrl="~/Images/control up.png" CssClass="masterUPDownButton" OnClick="UpDownButton_Click" />
                                        <br />
                                        <asp:ImageButton ID="productionPlanningPriorityShiftBDownImageButton" runat="server" ImageUrl="~/Images/control down.png" CssClass="masterUPDownButton" OnClick="UpDownButton_Click" />
                                    </td>
                                    <td colspan="2">
                                    
                                        <asp:Panel ID="productionPlanningPriorityShiftCPanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                                        
                                            <%--Gridview for Shift C--%>
                                            
                                            <asp:GridView ID="productionPlanningPriorityShiftCGridView" runat="server" AutoGenerateColumns="False" 
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="productionPlanningPriorityShiftCRadioButton" runat="server" AutoPostBack="true" OnCheckedChanged="ShiftCRadioButton_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftCPriorityIDLabel" runat="server" Text='<%# Container.DataItemIndex + 1 %>' ></asp:Label>
                                                            <asp:Label ID="productionPlanningPriorityShiftCIDLabel" runat="server" Text='<%# Eval("iD") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>    
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Component" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftCComponentLabel" runat="server"  Text='<%# Eval("productType") %>' CssClass="gridViewItems">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionPlanningPriorityShiftCRecipeLabel" runat="server"  Text='<%# Eval("name") %>' CssClass="gridViewItems">
                                                            </asp:Label>
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
                                    <td>
                                        <asp:ImageButton ID="productionPlanningPriorityShiftCUPImageButton" runat="server" ImageUrl="~/Images/control up.png"  CssClass="masterUPDownButton" OnClick="UpDownButton_Click"/>
                                        <br />
                                        <asp:ImageButton ID="productionPlanningPriorityShiftCDownImageButton" runat="server" ImageUrl="~/Images/control down.png" CssClass="masterUPDownButton" OnClick="UpDownButton_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" valign="top">
                            <asp:Button ID="productionPlanningPlanningViewButton" runat="server" Text="Planning"
                                CssClass="masterButton" OnClick="MultiViewButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="text-align: center">
                            <asp:Button ID="productionPlanningPrioritySaveButton" runat="server" 
                                CssClass="masterButton" Text="Save" 
                                onclick="Button_Click" />&nbsp;
                            <asp:Button ID="productionPlanningPriorityCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </table>        

</asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           