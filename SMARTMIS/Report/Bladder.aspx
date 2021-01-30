<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bladder.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.Bladder" Title="Curing Bladder Report" %>

<%@ Register src="~/UserControl/BladderReport.ascx" tagname="bladder" tagprefix="asp" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>

<asp:Content ID="bladderReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
<style>
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
</style>
<asp:ScriptManager ID="curingOperatorPlanningScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="curingOperatorPlanningUpdatePanel">
        <ProgressTemplate>
        <div style="position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;">
             
             <div style="width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#1B1B1B;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;">
             <img src="../Images/loading.gif"/>

             <h2><font color="#888888">Loading, please wait............</font> </h2> 
             </div>
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
       <asp:LinkButton runat="server" ID="ExportExcel" onclick="exptoexcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>

     <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
<asp:Panel ID="bladderReportDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    <table style="width: 100%">
             <tr>
                 <td style="width: 10%">
                     <asp:DropDownList ID="curingDropDownList" runat="server"
                         OnSelectedIndexChanged="curingDropDownList_SelectedIndexChanged"  AutoPostBack="True">
                         <asp:ListItem>Select</asp:ListItem>
                         <asp:ListItem>Curing TBR</asp:ListItem>
                         <asp:ListItem>Curing PCR</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                 <td style="width: 26%">
                     <b>Min Heat :</b> 
                     <asp:TextBox ID="mininumHeat" runat="server" ToolTip="Enter numeric value"></asp:TextBox>
                     
                 </td>
                 <td style="width: 26%">
                     <b>Max Heat : </b>
                    <asp:TextBox ID="maximumHeat" runat="server" ToolTip="Enter numeric value"></asp:TextBox>
                     
                 </td>
                 <td style="width: 34%">
                     <b>Bladder Size : </b>
                    <asp:DropDownList ID="mSize" runat="server"
                         OnSelectedIndexChanged="mSizeDropDownList_SelectedIndexChanged"  AppendDataBoundItems="True" AutoPostBack="True">
                    </asp:DropDownList> 
                 </td>
                 <td style="width: 2%">                 
                     <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="View"  CssClass="button"
                         Width="88px" Height="26px" />
                 </td>
             </tr>
         </table>
       <asp:reportHeader ID="reportHeader" runat="server" />

    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:11%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
            <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Side</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Current Bladder Code</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Current Bladder Size</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Current Bladder Heat</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Bladder TargetLife</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Old Bladder Code</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Old Bladder Size</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" colspan="2">Old Bladder Heat </td>

        </tr>
     </table>
     
     <asp:Panel ID="bladderReportDateWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
&nbsp;<asp:GridView ID="bladderReportDateWiseGridView" runat="server" AutoGenerateColumns="False"
        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                    <ItemTemplate>
                            <asp:Label ID="bladderReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>                                 
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11%">
                    <ItemTemplate>
                            <asp:Label ID="bladderReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>  
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90%">
                    <ItemTemplate>
                            <table class=" innerTable" style="border: solid 1px #C3D9FF;" cellspacing="1">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 95%"></td
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="width:10%;text-align: center; padding-top: 2px; padding-bottom: 2px;">
                                        LH
                                    </td>
                                    <td>
                                        <asp:GridView ID="bladderReportDateWiseChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />

                                             <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldCodeLabelLH" runat="server" Text='<%#Eval("CurrentBladderCodeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldSizeLabelLH" runat="server" Text='<%#Eval("CurrentBladderSizeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldHeatLabelLH" runat="server" Text='<%#Eval("CurrentBladderHeatLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                             <Columns>
                                                <asp:TemplateField HeaderText="Targetlife" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseTargetLifeLabelLH" runat="server" Text='<%#Eval("BladderTargetLifeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                               <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldCodeLabelLH" runat="server" Text='<%#Eval("OldBladderCodeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldSizeLabelLH" runat="server" Text='<%#Eval("OldBladderSizeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                             <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldHeatLabelLH" runat="server" Text='<%#Eval("OldBladderHeatLH")%>' CssClass="gridViewItems"></asp:Label>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="text-align: center; padding-top: 2px; padding-bottom: 2px;">
                                        RH
                                    </td>
                                    <td>
                                        <asp:GridView ID="bladderReportDateWiseChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                           <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldCodeLabelRH" runat="server" Text='<%#Eval("CurrentBladderCodeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldSizeLabelRH" runat="server" Text='<%#Eval("CurrentBladderSizeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldHeatLabelRH" runat="server" Text='<%#Eval("CurrentBladderHeatRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                              <Columns>
                                                <asp:TemplateField HeaderText="TargetlifeRH" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseTargetLifeLabelRH" runat="server" Text='<%#Eval("BladderTargetLifeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                   <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldCodeLabelRH" runat="server" Text='<%#Eval("OldBladderCodeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                   <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldSizeLabelRH" runat="server" Text='<%#Eval("OldBladderSizeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns> 
                                            <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldHeatLabelRH" runat="server" Text='<%#Eval("OldBladderHeatRH")%>' CssClass="gridViewItems"></asp:Label>
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
                                    </td>
                                </tr>
                             </table>
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
    
</asp:Panel>   
   </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>