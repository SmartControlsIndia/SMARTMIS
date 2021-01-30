<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tyreGeneaology.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.tyreGenealogy" Title="Tyre Genealogy Report" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>

<asp:Content ID="tgContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/report.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

 
    <table class="reportTable">
        

        <tr>
            <td class="reportFirstCol"></td>
            <td class="reportSecondCol"></td>
            <td class="reportThirdCol"></td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:reportHeader ID="reportHeader" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div class="reportMainHeader">Enter Barcode : &nbsp;<asp:TextBox ID= "tgBarcodeTextBox" runat="server" AutoComplete="off"></asp:TextBox>&nbsp;<asp:Button 
                        ID="tgBarcodeButton" runat="server" CssClass="masterButton" Text="Show" 
                        onclick="Button_Click" /> 
                </div>
            </td>
        </tr>
        
        <tr>
            <td colspan="3">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Green Tyre Barcode Number : <asp:Label ID="tgGTBarcodeNumber" runat="server"></asp:Label></p></div>
            </td>
        </tr>
        
     <tr>
            <td colspan="3">
            <div id="VISecondlinediv" runat ="server" visible="false">
                <div class="reportHeader">VI Second Line</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Operator Code</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Name</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                          <td class="reportHeading" style="width:10%; padding:5px">DefectName</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="SecondlineGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                    <Columns>
                        <asp:TemplateField HeaderText="Workcenter" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringRecipeCodeLabel" runat="server" Text='<%# Eval("curingRecpeName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="20%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                <asp:Label ID="vi2CuringLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                   
                                        <Columns>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringtyreserialNoLabel" runat="server" Text='<%# Eval("defectstatusName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="DefectName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="vi2CuringtyreserialNoLabel" runat="server" Text='<%# Eval("defectName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>            
            </td>
        </tr>
      <tr>
            <td colspan="3">
            <div id="PCRDiv1" runat ="server" visible="false">
                <div class="reportHeader">VI PCR Second Line</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Operator Code</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Name</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                          <td class="reportHeading" style="width:10%; padding:5px">DefectName</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="PCRGridView1" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                    <Columns>
                        <asp:TemplateField HeaderText="Workcenter" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                            <ItemTemplate>
                                <asp:Label ID="vPCRCuringWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="PCRCuringRecipeCodeLabel" runat="server" Text='<%# Eval("curingRecpeName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="PCRCuringInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="20%">
                            <ItemTemplate>
                                <asp:Label ID="vPCRCuringFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                <asp:Label ID="vPCRCuringLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                   
                                        <Columns>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="PCRCuringtyreserialNoLabel" runat="server" Text='<%# Eval("statusName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="DefectName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="PCRCuringtyreserialNoLabel" runat="server" Text='<%# Eval("defectName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="PCRCuringDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                <%--<asp:GridView  ID="PCR2ndGridview" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    
                </asp:GridView>--%>
                    
            </div>            
            </td>
        </tr>
       <tr>
            <td colspan="3">
            <div id="shreographyDiv1" runat ="server" visible="false">
                <div class="reportHeader">Shereography</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Recipe Code</td>
                        
                        <td class="reportHeading" style="width:10%; padding:5px">Grade</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Shift</td>
                          
                         <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="shreographyGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                    <Columns>
                        <asp:TemplateField HeaderText="Workcenter" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                            <ItemTemplate>
                                <asp:Label ID="shCuringWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="20%">
                            <ItemTemplate>
                                <asp:Label ID="shCuringNAmeLabel" runat="server" Text='<%# Eval("NAme") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                                        <Columns>
                        <asp:TemplateField HeaderText="Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="shCuringGradeLabel" runat="server" Text='<%# Eval("Grade") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                   
                                        <Columns>
                        <asp:TemplateField HeaderText="Shift" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="shShiftLabel" runat="server" Text='<%# Eval("Shift") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="shCuringDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>            
            </td>
        </tr>
    
        
        <tr>
            <td colspan="3">
            <div id="ClassificationDiv1" runat ="server" visible="false">
                <div class="reportHeader">Classification TUO</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Defectname</td>
                        <td class="reportHeading" style="width:10%; padding:5px">DefectArea</td>
                           <td class="reportHeading" style="width:15%; padding:5px">ParameterName</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="classificationGridView1" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="14.5%">
                                <ItemTemplate>
                                    <asp:Label ID="cTUOWCNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="TUORecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                        
                        <Columns>
                                                       <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUOFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                      <%--  <asp:Label ID="cTUOLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<Columns>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUOActionLabel" runat="server" Text='<%# Eval("action") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUOStatusLabel" runat="server" Text='<%#Eval("statusname")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                              <Columns>
                                                <asp:TemplateField HeaderText="DefectName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUODefectNameLabel" runat="server" Text='<%#Eval("defect_name")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                             <Columns>
                                                <asp:TemplateField HeaderText="DefectArea" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUOAreaLabel" runat="server" Text='<%#Eval("defectArea")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                             <Columns>
                                                <asp:TemplateField HeaderText="ParameterName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUOparameterNameLabel" runat="server" Text='<%#Eval("parameterName")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="cTUODateTimeLabel" runat="server" Text='<%# Eval("dtandtime") %>' CssClass="gridViewItems"></asp:Label>
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
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                
                <%--<div class="reportHeader">CSV Record TUO</div>
                <asp:Panel ID="Panel5" runat="server" ScrollBars="Horizontal" Width="1200PX" >
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="true" 
                Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true"
                onrowdatabound="GridView_RowDataBound"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" CssClass="gridViewItems" />    
                    
                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle CssClass="reportHeading" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                </asp:Panel>
                --%>
                
                
            </div>
            </td>
        </tr>
           <%-- //adding by sarita--%>
           <tr>
            <td colspan="3">
            <div id="exitBayDiv1" runat ="server" visible="false">
                <div class="reportHeader">Dispatch</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                       <td class="reportHeading" style="width:16%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Barcode</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Plan No</td>
                         <td class="reportHeading" style="width:10%; padding:5px">InvoiceNo</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Consignee_Name</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Destination_Name</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="exitBayGridView1" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                <ItemTemplate>
                                    <asp:Label ID="tgexitWCNameLabel" runat="server" Text='<%# Eval("WcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgexitecipeCodeLabel" runat="server" Text='<%# Eval("recipecode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                        <Columns>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="16%">
                                <ItemTemplate>
                                    <asp:Label ID="tgexitFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                    <%--<asp:Label ID="tgTBMLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="11%">
                                <ItemTemplate>
                                    <asp:Label ID="tgbayGTWeightLabel" runat="server" Text='<%# Eval("gtbarCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="Plan No" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="9%">
                                <ItemTemplate>
                                    <asp:Label ID="tgbayGTWeightLabel" runat="server" Text='<%# Eval("bayPlanNo") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMDateTimeLabel" runat="server" Text='<%# Eval("InvoiceNo") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Consignee_Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMDateTimeLabel" runat="server" Text='<%# Eval("Consignee_Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="InvoiceNo" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMDateTimeLabel" runat="server" Text='<%# Eval("Destination_Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="Destination_Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>
            </td>
        </tr>
           <tr>
            <td colspan="3">
            <div id="TUODiv1" runat ="server" visible="false">
                <div class="reportHeader">TUO</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:30%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:15%; padding:5px">Action</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="TUOGrid" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                <ItemTemplate>
                                    <asp:Label ID="TUOWCNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="TUORecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                        
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    
                                    <%--Inner UniBalancing Gridview--%>
                                    
                                    <asp:GridView ID="TUOInnerGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewChildItems" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TUOInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TUOFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                        <asp:Label ID="TUOLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TUOActionLabel" runat="server" Text='<%# Eval("action") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TUOStatusLabel" runat="server" Text='<%#Eval("status")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TUODateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                
                <div class="reportHeader">CSV Record TUO</div>
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Horizontal" Width="1200PX" >
                <asp:GridView ID="TUOcsvGrid" runat="server" AutoGenerateColumns="true" 
                Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true"
                onrowdatabound="GridView_RowDataBound"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" CssClass="gridViewItems" />    
                    
                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle CssClass="reportHeading" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                </asp:Panel>
                
                
                
            </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            <div id="tgUniBalDiv" runat ="server" visible="false">
                <div class="reportHeader">Uniformity and Balancing</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:30%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:15%; padding:5px">Action</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="tgUniBalGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                <ItemTemplate>
                                    <asp:Label ID="tgUniBalWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgUniBalRecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                        
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    
                                    <%--Inner UniBalancing Gridview--%>
                                    
                                    <asp:GridView ID="tgUniBalInnerGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewChildItems" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgUniBalInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgUniBalFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                        <asp:Label ID="tgUniBalLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgUniBalActionLabel" runat="server" Text='<%# Eval("action") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgUniBalStatusLabel" runat="server" Text='<%#Eval("status")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgUniBalDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                
                <div class="reportHeader">CSV Record Uniformity</div>
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="1200PX" >
                <asp:GridView ID="tgUniBalCSVGridView" runat="server" AutoGenerateColumns="true" 
                Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true"
                onrowdatabound="GridView_RowDataBound"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" CssClass="gridViewItems" />    
                    
                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle CssClass="reportHeading" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                </asp:Panel>
                
                
                 <div class="reportHeader">CSV Record Dynamic balancing</div>
                <asp:Panel ID="Panel3"  runat="server" ScrollBars="Horizontal" Width="1200PX" >
                <asp:GridView ID="DBGridView" runat="server" AutoGenerateColumns="true" 
                Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true"
                onrowdatabound="GridView_RowDataBound"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" CssClass="gridViewItems" />    
                    
                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle CssClass="reportHeading" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                </asp:Panel>

            </div>
            </td>
            </tr>
         
         <tr>
            <td colspan="3">
            <div id="RunOUTDiv" runat ="server" visible="false">
                <div class="reportHeader">RunOUT</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:30%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:15%; padding:5px">Action</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="RunOUTGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                <ItemTemplate>
                                    <asp:Label ID="RunOUTWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="RunOUTRecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                        
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    
                                    <%--Inner UniBalancing Gridview--%>
                                    
                                    <asp:GridView ID="RunOUTInnerGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewChildItems" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RunOUTInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RunOUTFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                        <asp:Label ID="RunOUTLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RunOUTActionLabel" runat="server" Text='<%# Eval("action") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RunOUTStatusLabel" runat="server" Text='<%#Eval("status")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RunOUTDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                
                <div class="reportHeader">CSV Record RunOutData</div>
                <asp:Panel ID="Panel4" runat="server" ScrollBars="Horizontal" Width="1200PX" >
                <asp:GridView ID="tgRunOUTCSVGridView" runat="server" AutoGenerateColumns="true" 
                Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true"
                onrowdatabound="GridView_RowDataBound"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" CssClass="gridViewItems" />    
                    
                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <HeaderStyle CssClass="reportHeading" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
                </asp:Panel>
                
                
            </div>
            </td>
            </tr>
          <tr>
            <td colspan="3">
            <div id="tgXrayDiv" runat ="server" visible="false">
                <div class="reportHeader">X-Ray1 Report</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:25%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:15%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:29%; padding:5px">Defect Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:11%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="tgXRayGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="25%">
                                <ItemTemplate>
                                    <asp:Label ID="tgXRayWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="75%">
                                <ItemTemplate>
                                    
                                    <%-- X-Ray Inner Grid view--%>
                                    
                                    <asp:GridView ID="tgXRayInnerGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewChildItems" />
			                            <Columns>
                                            <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                    <asp:Label ID="tgXRayLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Defect Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDefectNameLabel" runat="server" Text="None" CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                           <Columns>
                                            <asp:TemplateField HeaderText="Defect Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDefectNameLabel" runat="server" Text="N/A" CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                        <Columns>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>
            </td>
        </tr>
        
        <tr>
            <td colspan="3">
            <div id="Xra2Div2" runat ="server" visible="false">
                <div class="reportHeader">X-Ray2 Report</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:25%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Inspector Code</td>
                        <td class="reportHeading" style="width:15%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:29%; padding:5px">Defect Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:11%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="tgXRay2GridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
                    onrowdatabound="GridView_RowDataBound"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="25%">
                                <ItemTemplate>
                                    <asp:Label ID="tgXRay2WCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="75%">
                                <ItemTemplate>
                                    
                                    <%-- X-Ray Inner Grid view--%>
                                    
                                    <asp:GridView ID="tgXRayInnerGridView2" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewChildItems" />
			                            <Columns>
                                            <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                    <asp:Label ID="tgXRayLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Defect Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDefectNameLabel" runat="server" Text="None" CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                           <Columns>
                                            <asp:TemplateField HeaderText="Defect Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDefectNameLabel" runat="server" Text="N/A" CssClass="gridViewItems"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                        <Columns>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="tgXRayDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>
            </td>
        </tr>
              
        <tr>
             <td colspan="3">
                <div id="tgVIDiv" runat ="server" visible="false">
                <div class="reportHeader">Visual Inspection</div>
                
                 <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:10%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:9%; padding:5px">Sap Code</td>
                        <td class="reportHeading" style="width:9%; padding:5px">Inspector Name</td>
          
                        <td class="reportHeading" style="width:9%; padding:5px">Defect Status Name</td>
                        <td class="reportHeading" style="width:9%; padding:5px">Fault Side Name</td>
                        <td class="reportHeading" style="width:9%; padding:5px">Fault Area Name</td>
                         <td class="reportHeading" style="width:9%; padding:5px">FaultName</td>
                        <td class="reportHeading" style="width:9%; padding:5px">ReasonName</td>
                        <td class="reportHeading" style="width:9%; padding:5px">SerialNo</td>
                        <td class="reportHeading" style="width:9%; padding:5px">Remark</td>
                        <td class="reportHeading" style="width:9%; padding:5px">DateTime</td>
                        
                    </tr>
                </table>
           
              <asp:GridView ID="tgVIGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="2" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgVIWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="90%">
                                <ItemTemplate>
                                        <asp:GridView ID="tgVIInnerGridView" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                                        
                                            <Columns>
                                                <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                                        <asp:Label ID="tgVILNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Defect Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIDefectCodeLabel" runat="server" Text='<%# Eval("defectStatusName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Reason" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIReasonLabel" runat="server" Text='<%# Eval("faultSideName") %>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIStatusLabel" runat="server" Text='<%# Eval("faultAreaName")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIStatusLabel" runat="server" Text='<%# Eval("faultName")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIStatusLabel" runat="server" Text='<%# Eval("reasonName")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIStatusLabel" runat="server" Text='<%# Eval("SerialNo")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="9%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIStatusLabel" runat="server" Text='<%# Eval("Remark")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tgVIDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                    <PagerStyle BackColor="#507CD1" ForeColor="White" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
            
            </div>
            </td>
        </tr>
        <tr>
             <td colspan="3">
                <div id="TrimmingDetailDiv" runat ="server" visible="False">
                <div class="reportHeader">Trimming Detail</div>
                
                 <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:30%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:30%; padding:5px">TrimNo</td>
                       <td class="reportHeading" style="width:30%; padding:5px">DateTime</td>
                        
                    </tr>
                </table>
           
              <asp:GridView ID="TrimmimgGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="2" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTrimWcNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTrimTrimNoLabel" runat="server" Text='<%# Eval("TrimNo") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="DateTime" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTrimDtandtimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            
            </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            <div id="tgCuringDiv" runat ="server" visible="false">
                <div class="reportHeader">Curing</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Operator Code</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Name</td>
                         <td class="reportHeading" style="width:10%; padding:5px">Tyre SerialNo</td>
                         <td class="reportHeading" style="width:10%; padding:5px">CavityNo</td>
                        <td class="reportHeading" style="width:10%; padding:5px">MouldNO</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="tgCuringGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                    <Columns>
                        <asp:TemplateField HeaderText="Workcenter" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringRecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="20%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                <asp:Label ID="tgCuringLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Press Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringPressBarcodeLabel" runat="server" Text='<%# Eval("pressbarcode") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                                        <Columns>
                        <asp:TemplateField HeaderText="Tyre SerialNo" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringtyreserialNoLabel" runat="server" Text='<%# Eval("TyreSerialNo") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="CavityNo" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringPressCavityNoLabel" runat="server" Text='<%# Eval("CavityNo") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <Columns>
                        <asp:TemplateField HeaderText="Press Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringPressMouldNumberLabel" runat="server" Text='<%# Eval("mouldNo") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                            <ItemTemplate>
                                <asp:Label ID="tgCuringDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
                <asp:Button ID="tgCuringChartButton" runat="server" Text="Show Curing Cycle" 
                    CssClass="masterButton" Width="140px" OnClick="ChartButton_Click" 
                    Visible="true" />
                <asp:Chart ID="tgCuringChart" runat="server" Height="900" Width="1000" Visible="true" >
                    <Titles>
                    </Titles>
                    <Legends>
                        <asp:Legend>
                        </asp:Legend>
                    </Legends>
                    <Series>
                    </Series>
                    <ChartAreas>
                    </ChartAreas>
                </asp:Chart>
            </div>            
            </td>
        </tr>
        
        
        <tr>
            <td colspan="3">
            <div id="tgTBMDiv" runat ="server" visible="false">
                <div class="reportHeader">TBM</div>
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:15%; padding:5px">Workcenter Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Operator Code</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Name</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Cured Tyre Weight</td>
                        <td class="reportHeading" style="width:15%; padding:5px"></td>
                        <td class="reportHeading" style="width:10%; padding:5px">Status</td>
                        <td class="reportHeading" style="width:10%; padding:5px">Date Time</td>
                    </tr>
                </table>
                <asp:GridView ID="tgTBMGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" CssClass="gridViewItems" />
                        <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="15%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMRecipeCodeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMInspectorCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="25%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMFNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>&nbsp;
                                    <asp:Label ID="tgTBMLNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="GT Weight" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="20%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMGTWeightLabel" runat="server" Text='<%# Eval("gtWeight") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                          <Columns>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMGTWeightLabel" runat="server" Text='<%# Eval("status") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="10%">
                                <ItemTemplate>
                                    <asp:Label ID="tgTBMDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
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
            </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            <div id="tgMHEBarcodeDiv" runat ="server" visible="false">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="reportHeading" style="width:20%; padding:5px"></td>
                        <td class="reportHeading" style="width:20%; padding:5px">Bead</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Inner Liner</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Ply</td>
                        <td class="reportHeading" style="width:20%; padding:5px">Side Wall</td>
                    </tr>
                    <tr>
                        <td class="reportHeading" style="width:20%; padding:5px">MHE Barcode</td>
                        <td style="width:20%;  padding:5px; text-align:left;"><asp:Label ID="tgTBMBeadBarcodeLabel" CssClass="masterLabel" runat="server" Text=""></asp:Label></td>
                        <td style="width:20%;  padding:5px; text-align:left;"><asp:Label ID="tgTBMILBarcodeLabel" CssClass="masterLabel" runat="server" Text=""></asp:Label></td>
                        <td style="width:20%;  padding:5px; text-align:left;"><asp:Label ID="tgTBMPlyBarcodeLabel" CssClass="masterLabel" runat="server" Text=""></asp:Label></td>
                        <td style="width:20%;  padding:5px; text-align:left;"><asp:Label ID="tgTBMSideWallBarcodeLabel" CssClass="masterLabel" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
            </td>
        </tr>
        
        
    </table>
</asp:Content>
