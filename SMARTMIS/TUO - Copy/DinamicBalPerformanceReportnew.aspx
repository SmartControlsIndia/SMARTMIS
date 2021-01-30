<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinamicBalPerformanceReportnew.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.TUO.DinamicBalPerformanceReportnew" Title="Dynamic Balancing Summary Report" %>
<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

<asp:Content ID="DinamicBalContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
<style>
.links
{
    text-decoration:none;
    color:#333333;
    font-family:Verdana;
	font-weight: bold;
	font-size:12px;
	text-align:left;
	padding:2px;
}
.links:hover
{
      text-decoration:underline;      
}
.masterButtonCss:hover
{
    background: linear-gradient(to bottom, #15497C, #2384D3);
}
.masterButtonCss
{
    font-style: normal;
    font-size: 15px;
    font-family: Calibri,"Trebuchet MS",Verdana,Geneva,Arial,Helvetica,sans-serif;
    color: #fff;
    background: linear-gradient(to bottom, #2384D3, #15497C);
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
    .saveLink {
  padding:3px;
  background-color: #FF9933;
  background: -moz-linear-gradient(top, #FCAE41, #FF9933);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#FCAE41), to(#FF9933));
  border: 1px solid #666;
  color:#000;
  text-decoration:none;
   font-weight:bold;
   cursor:pointer;
}
    .close {
	background-color: #4C4C4C;
    background: -moz-linear-gradient(top, #272727, #4C4C4C);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#272727), to(#4C4C4C));
	color: #FFFFFF;
	line-height: 25px;
	position: absolute;
	right: -12px;
	text-align: center;
	top: -10px;
	width: 24px;
	text-decoration: none;
	font-weight: bold;
	-webkit-border-radius: 12px;
	-moz-border-radius: 12px;
	border-radius: 12px;
  .box-shadow;
  .transition;
  .transition-delay(.2s);
  &:hover { background: #00d9ff; .transition; }
}
    .close:hover
    {
        background-color: #272727;
    background: -moz-linear-gradient(top, #4C4C4C, #272727);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#4C4C4C), to(#272727));
        }
.dialogPanelCSS
{
    padding:12px;
    left:10%;
    top:50px;
    z-index:2000;
    position:fixed;
    background-color: #FF9933;
    background: -moz-linear-gradient(top, #E05539, #FCAE41);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#E05539), to(#FCAE41));
    -webkit-border-radius: 5px;
	-moz-border-radius: 5px;
	border-radius: 5px;
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
#bookG{
width:39px}

.blockG{
background-color:#949494;
border:1px solid #000000;
float:left;
height:28px;
margin-left:2px;
width:7px;
opacity:0.1;
-moz-animation-name:bounceG;
-moz-animation-duration:1s;
-moz-animation-iteration-count:infinite;
-moz-animation-direction:linear;
-moz-transform:scale(0.7);
-webkit-animation-name:bounceG;
-webkit-animation-duration:1s;
-webkit-animation-iteration-count:infinite;
-webkit-animation-direction:linear;
-webkit-transform:scale(0.7);
-ms-animation-name:bounceG;
-ms-animation-duration:1s;
-ms-animation-iteration-count:infinite;
-ms-animation-direction:linear;
-ms-transform:scale(0.7);
-o-animation-name:bounceG;
-o-animation-duration:1s;
-o-animation-iteration-count:infinite;
-o-animation-direction:linear;
-o-transform:scale(0.7);
animation-name:bounceG;
animation-duration:1s;
animation-iteration-count:infinite;
animation-direction:linear;
transform:scale(0.7);
}

#blockG_1{
-moz-animation-delay:0.3s;
-webkit-animation-delay:0.3s;
-ms-animation-delay:0.3s;
-o-animation-delay:0.3s;
animation-delay:0.3s;
}

#blockG_2{
-moz-animation-delay:0.4s;
-webkit-animation-delay:0.4s;
-ms-animation-delay:0.4s;
-o-animation-delay:0.4s;
animation-delay:0.4s;
}

#blockG_3{
-moz-animation-delay:0.5s;
-webkit-animation-delay:0.5s;
-ms-animation-delay:0.5s;
-o-animation-delay:0.5s;
animation-delay:0.5s;
}

@-moz-keyframes bounceG{
0%{
-moz-transform:scale(1.2);
opacity:1}

100%{
-moz-transform:scale(0.7);
opacity:0.1}

}

@-webkit-keyframes bounceG{
0%{
-webkit-transform:scale(1.2);
opacity:1}

100%{
-webkit-transform:scale(0.7);
opacity:0.1}

}

@-ms-keyframes bounceG{
0%{
-ms-transform:scale(1.2);
opacity:1}

100%{
-ms-transform:scale(0.7);
opacity:0.1}

}

@-o-keyframes bounceG{
0%{
-o-transform:scale(1.2);
opacity:1}

100%{
-o-transform:scale(0.7);
opacity:0.1}

}

@keyframes bounceG{
0%{
transform:scale(1.2);
opacity:1}

100%{
transform:scale(0.7);
opacity:0.1}

}
    </style>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
  <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
   <asp:HiddenField id="viewQueryHidden" runat="server" value="" />
     <script type="text/javascript" language="javascript">
    function hideDialog() {
        document.getElementById('ctl00_masterContentPlaceHolder_DynamicBalReportPanel').style.display = 'none';
        document.getElementById('ctl00_masterContentPlaceHolder_backDiv').style.display = 'none';
    }
        
    </script>
    
    <asp:Label ID="fromDate" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="toDate" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="queryStringLabel" runat="server" Visible="false"></asp:Label>
   
    <asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="DynamicBalUpdatePanel">
        <ProgressTemplate>
        <div style="position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;">
             
             <div align="center" style="padding:5px;width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#1B1B1B;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;">
             
<div id="bookG">
<div id="blockG_1" class="blockG">
</div>
<div id="blockG_2" class="blockG">
</div>
<div id="blockG_3" class="blockG">
</div>
</div>

<br />

             <h2><font color="#888888">Loading, please wait............</font> </h2> 
             </div>
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="DynamicBalUpdatePanel" runat="server">
        <ContentTemplate>
    <asp:Label ID="backDiv" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
    <asp:Panel ID="DynamicBalReportPanel" runat="server" Width="80%" Height="480" CssClass="dialogPanelCSS" Visible="false" >
   <a href="javascript:void();" class="close" OnClick="hideDialog()">X</a>
    <asp:Panel ID="innerDialogPanel" runat="server" ScrollBars="Auto" Width="100%" Height="470">
    
    <asp:GridView ID="DynamicBalReportBarcodeDetailGridView" runat="server" AutoGenerateColumns="False" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound" AllowPaging="false" AllowSorting="true" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
    
    <Columns>
       <asp:TemplateField HeaderText="TBM Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("tbmWCName") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Curing Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="CuringMachineName" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# fillCuringWCName(DataBinder.Eval(Container.DataItem,"Barcode")) %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
     
    <Columns>
   <asp:TemplateField HeaderText="Size" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("size") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="barcode" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:HyperLink ID="hlDetails1" Text='<%# Eval("barcode") %>' CssClass="links" runat="server"  NavigateUrl='<%# "~/Report/tyreGeneaology.aspx?gtbarcode=" + Eval("barcode") %>' Target="_blank" /> 
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


 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
    <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">From::</td>
        <td style="width: 10%">
         <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Width="80%" />     
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">To::</td>
        <td style="width: 10%">
         <myControl:calenderTextBox ID="reportMasterToDateTextBox" runat="server" Width="80%" />     
        </td>
        <td style="width: 4%">
            <asp:Button ID="MasterViewButton" runat="server" Text="  View  " CssClass="masterButtonCss" onclick="viewData_Click" />
            </td>
  <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
         <asp:RadioButton ID="QualityReportTBMWise" runat="server" 
          Text="MachineWise" GroupName="aa" AutoPostBack="True" 
          Checked="True" oncheckedchanged="QualityReportTBMWise_CheckedChanged" />
        </td>
  <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
            <asp:RadioButton ID="QualityReportRecipeWise" runat="server" 
                Text="RecipeWise" GroupName="aa" AutoPostBack="True" 
                oncheckedchanged="QualityReportRecipeWise_CheckedChanged" />
        </td>
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Size::</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="FilterDynamicBalancingSizeDropdownlist"  Width="60%" runat="server" CausesValidation="false" AppendDataBoundItems="True"> </asp:DropDownList>
        
         
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="FilterOptionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="Total"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Average"></asp:ListItem>
             </asp:DropDownList>
        </td>   
       
    </tr>
</table>
<asp:reportHeader ID="reportHeader" runat="server" />

<asp:Panel ID="dinamicBalReportRecipeWisePanel" runat="server" ScrollBars="Horizontal" Height="100%" CssClass="panel" >
 <table class="innerTable" cellspacing="1" style="width:125%;">
        <tr>
            <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" 
                rowspan="3">Machine Name</td>
            <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" 
                rowspan="3">Tyre Type</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="3">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="6">OE</td>
            <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" colspan="3"></td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" colspan="3">Rep</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" colspan="3">Rejection</td>
            <td class="gridViewHeader" style="width:15%; text-align:center; padding:5px" rowspan="3">Rejection Details</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px" colspan="3">A</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px" colspan="3">B</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px" colspan="3">C</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px" colspan="3">D</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px" colspan="3">E</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:4.5%; text-align:center; padding:5px">Static</td>
        </tr>
    </table>
                       
 <asp:GridView ID="dinamicBalMainRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="125%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" ItemStyle-Wrap=true>
                <ItemTemplate>
                        <asp:Label ID="dinamicBalSizeWisewcNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="DinamicBalRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="WcName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5.2%" Visible="false">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalSizeWisewcNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5.4%" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems" Width="10"></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="4.4%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseUpperAGradeLabel" runat="server" Text='<%# Eval("UpperA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseLowerAGradeLabel" runat="server" Text='<%# Eval("LowerA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseStaticAGradeLabel" runat="server" Text='<%# Eval("StaticA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="1.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperBGradeLabel" runat="server" Text='<%# Eval("UpperB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="1.4%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerBGradeLabel" runat="server" Text='<%# Eval("LowerB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticBGradeLabel" runat="server" Text='<%# Eval("StaticB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperCGradeLabel" runat="server" Text='<%# Eval("UpperC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerCGradeLabel" runat="server" Text='<%# Eval("LowerC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.6%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticCGradeLabel" runat="server" Text='<%# Eval("StaticC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperDGradeLabel" runat="server" Text='<%# Eval("UpperD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerDGradeLabel" runat="server" Text='<%# Eval("LowerD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticDGradeLabel" runat="server" Text='<%# Eval("StaticD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperEGradeLabel" runat="server" Text='<%# Eval("UpperE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerEGradeLabel" runat="server" Text='<%# Eval("LowerE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticEGradeLabel" runat="server" Text='<%# Eval("StaticE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Rejection Details" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.3%">
                                    <ItemTemplate>
                                        <asp:Button ID="RejectionDetails" runat="server" Text="View" OnClick="viewDetails_Click" CssClass="saveLink" ToolTip="View Rejection Details" />
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
                </ItemTemplate>
                  <FooterTemplate>
  
</FooterTemplate>
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

<asp:Panel ID="RecipeWisePanel" runat="server" ScrollBars="Horizontal" Height="100%" CssClass="panel" Visible="false" >
 <table class="innerTable" cellspacing="1" style="width:125%;">
        <tr>
            <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" 
                rowspan="3">Tyre Type</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="3">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="6">OE</td>
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px" colspan="3"></td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" colspan="3">Rep</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" colspan="3">Rejection</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" rowspan="3">Rejection Details</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px" colspan="3">A</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px" colspan="3">B</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px" colspan="3">C</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px" colspan="3">D</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px" colspan="3">E</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Static</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Upper</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Lower</td>
            <td class="gridViewAlternateHeader" style="width:5%; text-align:center; padding:5px">Static</td>
        </tr>
    </table>
                       
 <asp:GridView ID="DinamicBalRecipeWiseGridView" runat="server" AutoGenerateColumns="False" RowStyle-CssClass="rowHover" 
                        Width="125%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="4.8%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.1%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseUpperAGradeLabel" runat="server" Text='<%# Eval("UpperA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseLowerAGradeLabel" runat="server" Text='<%# Eval("LowerA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseStaticAGradeLabel" runat="server" Text='<%# Eval("StaticA") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperBGradeLabel" runat="server" Text='<%# Eval("UpperB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerBGradeLabel" runat="server" Text='<%# Eval("LowerB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticBGradeLabel" runat="server" Text='<%# Eval("StaticB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.8%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperCGradeLabel" runat="server" Text='<%# Eval("UpperC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerCGradeLabel" runat="server" Text='<%# Eval("LowerC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticCGradeLabel" runat="server" Text='<%# Eval("StaticC") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperDGradeLabel" runat="server" Text='<%# Eval("UpperD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerDGradeLabel" runat="server" Text='<%# Eval("LowerD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticDGradeLabel" runat="server" Text='<%# Eval("StaticD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseUpperEGradeLabel" runat="server" Text='<%# Eval("UpperE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseLowerEGradeLabel" runat="server" Text='<%# Eval("LowerE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2.9%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseStaticEGradeLabel" runat="server" Text='<%# Eval("StaticE") %>' CssClass="gridViewItems"></asp:Label>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Rejection Details" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Button ID="RejectionDetailsRecipeWise" runat="server" Text="View" OnClick="viewDetails_Click" CssClass="saveLink" ToolTip="View Rejection Details" />
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

</ContentTemplate></asp:UpdatePanel>
</asp:Content>
