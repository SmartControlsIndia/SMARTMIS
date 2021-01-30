<%@ Page Title="" Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="ReceipeExportReport.aspx.cs" Inherits="SmartMIS.Report.ReceipeExportReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
    <style>
.button:hover
{
    background-color: #15497C;
    background: -moz-linear-gradient(top, #15497C, #2384D3);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#15497C), to(#2384D3));
}
.button 
{
    font-style: normal;
    font-size: 15px;
    font-family: Calibri,"Trebuchet MS",Verdana,Geneva,Arial,Helvetica,sans-serif;
    color: #fff;
    background: linear-gradient(to bottom, #2384D3, #15497C);
    background-color: #2384D3;
    background: -moz-linear-gradient(top, #2384D3, #15497C);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#2384D3), to(#15497C));
    padding: 0px 6px;
    border-width: 1px;
    border-style: solid;
    border-right: 1px solid #DDDDEB;
    border-left: 1px solid #DDDDEB;
    -moz-border-top-colors: none;
    -moz-border-right-colors: none;
    -moz-border-bottom-colors: none;
    -moz-border-left-colors: none;
    border-image: none;
    border-color: #FFF #DDDDEB #B3B3BD;
    border-radius: 7px;
    text-align: center;
    box-shadow: 0px 1px 4px 0px #C8C8D2;
    outline: medium none;
    line-height: 21px;
    display: inline-block;
    cursor: pointer;
    box-sizing: border-box;
    height: 28px;
}
::-moz-selection
{
  background: #FF0;
  color:#000000;
}

::-webkit-selection
{
  background: #FF0;
  color:#000000;
}

::selection
{
  background: #FF0;
  color:#000000;
}
    </style>
 <div><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
                        <asp:Panel ID="recipePanel" runat="server" CssClass="panel" Height="600px" 
                            ScrollBars="Vertical">
                            <asp:GridView ID="recipeGridView" runat="server" AllowPaging="false" 
                                AllowSorting="false" AutoGenerateColumns="False" CellPadding="3" 
                                ForeColor="#333333" GridLines="Both" PageSize="5" ShowHeader="true" 
                                Width="100%">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" HeaderText="ID" 
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  >
                                        <ItemTemplate>
                                            <asp:Label ID="recipeGridIDLabel" runat="server" CssClass="gridViewItems" 
                                                Text='<%# Eval("iD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                        HeaderText=" Recipe Name" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="recipeGridRecipeNameLabel" runat="server" 
                                                CssClass="gridViewItems" Text='<%# Eval("recipename") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                 
                        <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                        HeaderText="SAP Material Code" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Width="15%">
                                        <ItemTemplate>
                                           <asp:Label ID="lblsapmatlcd" runat="server" CssClass="gridViewItems" 
                                                Text='<%# Eval("SAPMaterialCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                 
                                        
                        <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                        HeaderText="Description" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="recipeGridDescriptionLabel" runat="server" 
                                                CssClass="gridViewItems" Text='<%# Eval("description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                        
                               
                                <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="#FFFFFF" />
                            </asp:GridView>
                        </asp:Panel>
</asp:Content>
