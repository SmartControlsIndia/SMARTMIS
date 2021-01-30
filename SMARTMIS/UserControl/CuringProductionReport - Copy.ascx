<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuringProductionReport.ascx.cs" Inherits="SmartMIS.UserControl.CuringProductionReport" %>

 <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
<style>
.ErrorMsgCSS
{
    color :#B5B3B5; padding:7px;width:35%;max-width:35%;height:auto;
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;
    position:fixed;z-index: 1080;top:75px;left: 450px;
    -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background: #1B1B1B;
	background: -moz-linear-gradient(top, #1B1B1B, #0033CC);
	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#1B1B1B), to(#0033CC));
	text-align:center;
}
.popupBut
{
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;color:#5B5B5B;border: 1px solid #666;
}
    </style>
<script type="text/javascript">
    function closePopup() {
        document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_CuringproductionReportWCWise_ErrorMsg').style.display = "none";
    }
</script>  
    <asp:HiddenField id="magicHidden" runat="server" value="" />
 <asp:Label ID="queryStringSave" runat="server" Text="" Visible="false"></asp:Label>
 
    <asp:ScriptManager ID="curingOperatorPlanningScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="ErrorMsgCSS" Visible="false"></asp:Label>
 <%--<table cellpadding="2" cellspacing="0">
    <tr>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:6%">Size::</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:20%">
           
             <asp:DropDownList ID="CuringProductionReportSizeDropdownlist"  
                   Width="90%" runat="server" 
                   CausesValidation="false" AutoPostBack="True" 
                   style="margin-bottom: 0px" AppendDataBoundItems="True"
                   onselectedindexchanged="DropDownList_SelectedIndexChanged">
             </asp:DropDownList>
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:6%">Operator:</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:20%">
           
             <asp:DropDownList ID="CuringProductionReportOperatorDropdownlist"  
                   Width="90%" runat="server" CausesValidation="false"  AutoPostBack="true" AppendDataBoundItems="True"
                   onselectedindexchanged="DropDownList_SelectedIndexChanged">
             </asp:DropDownList>
        </td> 
        <td style="width:48%"></td>       
    </tr>
</table>--%>
<asp:Panel ID="curingProductionDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel">
  
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Product Name</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Recipe</td>
            <td class="gridViewHeader" style="width:42%; text-align:center; padding:5px" colspan="3">Shift</td>
           <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px"></td>

        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="text-align:center; width:14%; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="text-align:center; width:14%; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="text-align:center; width:14%; padding:5px">C</td>
           <td class="gridViewAlternateHeader" style= "text-align:center; width:14%; padding:5px">Total</td>

        </tr>
     </table>
     
    <asp:Panel ID="curingProductionReportDateWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel">
     
        <asp:GridView ID="curingProductionReportDateWiseGridView" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
           AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                            <asp:Label ID="curingProductionReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>                                 
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                    <ItemTemplate>
                            <asp:Label ID="curingProductionReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>  
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="86%">
                    <ItemTemplate>
                           
                                    
                                    <asp:GridView ID="curingProductionReportDateWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                        
                                        
                                        <Columns>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="14%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="curingProductionReportDateWiseDescription" runat="server" Text='<%#Eval("description")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        
                                            <Columns>
                                                <asp:TemplateField HeaderText="RecipeName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="14%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="curingProductionReportDateWiseRecipeCode" runat="server" Text='<%#Eval("recipecode")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                                    <asp:TemplateField HeaderText="Shift A Production" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="14%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="curingProductionReportDateWiseChildAshiftLabel" runat="server" Text='<%#Eval("AshiftCount")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Shift B Production" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="14%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="curingProductionReportDateWiseChildBshiftLabel" runat="server" Text='<%#Eval("BshiftCount")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>   
                                                                 
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="shift C Production" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="14%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="curingProductionReportDateWiseChildCshiftLabel" runat="server" Text='<%#Eval("CshiftCount")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Total Production" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="14%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="curingProductionReportDateWiseChildTotalshiftLabel" runat="server" Text='<%#Eval("Totalcount")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                     
                                                                <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditRowStyle BackColor="#999999" />
                                                                <AlternatingRowStyle BackColor="#FFFFFF" />
                                                            </asp:GridView>
                                                    
        
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>         
        <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
        </asp:GridView>
        
   <table width="100%">
        <tr>
            <td  style="width:14%" class="gridViewItems2">GrandTotal</td>
            <td style="width:14%" class="gridViewItems2"></td>
            <td  style="width:13%" class="gridViewItems2"></td>
            <td  style="width:14%" class="gridViewItems2"><%= shiftcountA %></td>
            <td  style="width:14%" class="gridViewItems2"><%= shiftcountB %></td>
            <td  style="width:14%" class="gridViewItems2"><%= shiftcountC %></td>
            <td  style="width:14%" class="gridViewItems2"><%= (shiftcountA + shiftcountB + shiftcountC) %></td>
        </tr>
   </table>

        
     </asp:Panel>
    
</asp:Panel>   