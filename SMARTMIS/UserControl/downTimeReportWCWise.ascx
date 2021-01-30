<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="downTimeReportWCWise.ascx.cs" Inherits="SmartMIS.UserControl.downTimeReportWCWise" %>

    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/downtime.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />

    <asp:HiddenField id="magicHidden" runat="server" value="" />
    
    <table class="innerTable" cellspacing="0">
        <tr>
            <td class="gridViewHeader" style="width:15%; padding:5px; text-align:left">Workcenter Name</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Downtime Event</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Uptime Event</td>
            <td class="gridViewHeader" style="width:5%; padding:5px; text-align:left">Duration</td>
            <td class="gridViewHeader" style="width:55%; padding:5px; text-align:center">Reasons</td>
        </tr>
    </table>
    
    <asp:Panel ID="downTimeReportWCWisePanel" runat="server" ScrollBars="None" CssClass="panel" >
        <asp:GridView ID="downTimeReportWCWiseGridView" runat="server" AutoGenerateColumns="False" 
        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="downTimeReportWCWiseWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <asp:Label ID="downTimeReportWCWiseWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85%">
                    <ItemTemplate>
                        
                    <%--child Gridview--%>
                    <div class="inputTable">
                        <asp:GridView ID="downTimeReportWCWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="downTimeReportWCWiseChildIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="downTimeReportWCWiseChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Down Event" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                                        <ItemTemplate>
                                            <asp:Label ID="downTimeReportWCWiseDownEventLabel" runat="server" Text='<%# Eval("downEvent") %>' CssClass="gridViewItems"></asp:Label><br />
                                            <asp:Label ID="downTimeReportWCWiseDownEventDateTimeLabel" runat="server" Text='<%# Eval("downdtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="UP Event" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                                        <ItemTemplate>
                                            <asp:Label ID="downTimeReportWCWiseUpEventLabel" runat="server" Text='<%# Eval("upEvent") %>' CssClass="gridViewItems"></asp:Label><br />
                                            <asp:Label ID="downTimeReportWCWiseUpEventDateTimeLabel" runat="server" Text='<%# Eval("updtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="downTimeReportWCWiseDurationLabel" runat="server" Text='<%# Eval("duration") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="66%">
                                        <ItemTemplate>
                                        
                                         <%--Reason child Gridview--%>
                                         
                                            <asp:GridView ID="downTimeReportWCWiseChildReasonGridView" runat="server" AutoGenerateColumns="False" 
                                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true" >
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="downTimeReportWCWisereasonIDLabel" runat="server" Text='<%# Eval("reasonName") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="downTimeUpReportWCWiseDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Reason Duration" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="downTimeReportWCWiseReasonDurationLabel" runat="server" Text='<%# Eval("downDuration") %>' CssClass="gridViewItems"></asp:Label>
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
                        <table class="innerTable">
                            <tr>
                                <td style="width: 50%">
                                    <asp:Chart ID="downTimeReportByDurationChart" runat="server" AlternateText="Daily Production Chart" IsSoftShadows="true"
                                        RenderType="ImageTag" Height="150px" Width="250px" >
                                        <Titles>
                                            <asp:Title Alignment="TopCenter" Text="Faults by Duration"></asp:Title>
                                        </Titles>
                                        <Series>
                                            <asp:Series Name="" ChartType="Column" Palette="EarthTones" ShadowOffset="0">
                                            
                                            </asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="downTimeReportByDurationChartArea" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                                                BorderDashStyle="Solid" BorderWidth="1">
                                                <AxisX>
                                                    <MajorGrid Enabled="false"/>
                                                </AxisX>
                                                <AxisY IntervalOffsetType="Number" >
                                                </AxisY>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart> 
                                </td>
                                <td style="width: 50%">
                                    <asp:Chart ID="downTimeReportByOccuranceChart" runat="server" AlternateText="Daily Production Chart" IsSoftShadows="true"
                                        RenderType="ImageTag" Height="150px" Width="250px" >
                                        <Titles>
                                            <asp:Title Alignment="TopCenter" Text="Faults by Occurance"></asp:Title>
                                        </Titles>
                                        <Series>
                                            <asp:Series Name="" ChartType="Column" Palette="EarthTones" ShadowOffset="0">
                                                
                                            </asp:Series>
                                        </Series>                                        
                                        <ChartAreas>
                                            <asp:ChartArea Name="downTimeReportByOccuranceChartArea" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                                                BorderDashStyle="Solid" BorderWidth="1">
                                                <AxisX>
                                                    <MajorGrid Enabled="false" />
                                                </AxisX>
                                                <AxisY Interval="5" IntervalOffsetType="Number" >
                                                </AxisY>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </td>
                            </tr>
                        </table>
                    </div>
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