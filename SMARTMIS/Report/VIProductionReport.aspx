<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIProductionReport.aspx.cs" Inherits="SmartMIS.Report.VIProductionReport" MasterPageFile="~/smartMISMaster.Master"%>
 <%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>

<asp:Content ID="VIProductionreportContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server" Visible="true">
  <script type = "text/javascript">
              function downloadFile(filename)
              {
	            window.location.href = filename;
	            document.getElementById("maindiv").remove();
	            document.getElementById("smalldiv").remove();	
              }
              function closebox()
               {
                   document.getElementById("maindiv").remove();
                   document.getElementById("smalldiv").remove();
               }
               function divshoworhide(MinthWiseDive) 
               { 
               
               
               }

</script>

<style type="text/css">
#headerdive
   {
       background-color:#C3D9FF;
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
   }
      .tablecolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       
   }
     .showdivselect
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:5px;
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:450px;
   }
   tr
   {
      border:1pt solid black;
   }
   .tableheadercolumn
   {   font-family :Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       background-color:#C3D9FF;
       }
       .griddateHeader
       {
           font-family : @Gulim;
       font-size:small;
       font-weight:normal;
       text-align:left;
       background-color:#FCF978;
           }
   .innertableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:4px;
       
       width:100%;
       background-color:#C3D9FF;
   }
   #datePicker
        {
            position:absolute;
            border:solid 1px black;
            background-color:white;
        }
  .bder
  {
      background-color: inherit; 
   font-size:20px; 
   font-family:Arial Narrow;
   border:1pt solid black;
    border-top-color:black;
    border-width:thin;
    border-bottom-color:black;
    height:30px;
}
   .button
{
    cursor: pointer;    
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
 endColorstr='#579AEB'); /* for IE */
    -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
    endColorstr='#579AEB'); /* for IE 8 and above */
    background: -webkit-gradient(linear, left top, left bottom, from(#85B6F0),
    to(#579AEB)); /* for webkit browsers */
    background: -moz-linear-gradient(top, #85B6F0, #579AEB); /* for firefox 3.6+ */
    background: -o-linear-gradient(top, #85B6F0, #579AEB); /* for Opera */
    width:100PX;
}
    .style1
    {
        height: 19px;
    }
</style>

<div id="headerdive">Visual Inspection Production Report :</div>&nbsp;&nbsp;

<div style="width:100%;">
<table width="100%" class="bder">
<tr><td class="tablecolumn"> Select Process :
<asp:DropDownList ID="ddlProcess" runat="server" AutoPostBack="true" Width="90px" 
        onselectedindexchanged="ddlProcess_SelectedIndexChanged">
       <asp:ListItem Value="PCR">PCR</asp:ListItem>
       <asp:ListItem Value="TBR" >TBR</asp:ListItem>
       
    </asp:DropDownList></td>
<td class="tablecolumn"> Select Duration:
   <asp:DropDownList ID="ddlmonthselection" runat="server" AutoPostBack="true" onselectedindexchanged="ddlmonthselection_SelectedIndexChanged"
    Width="90px">
       <asp:ListItem Value="Daily">Daily</asp:ListItem>
       <asp:ListItem Value="Monthly" >Monthly</asp:ListItem>
       
    </asp:DropDownList>
</td>
<td class="tablecolumn"><asp:Panel id="pnlshift" runat="server">  Select Shift:
   <asp:DropDownList ID="ddlshift" runat="server" AutoPostBack="true"  
    Width="90px">
     <asp:ListItem Value="0">ALL</asp:ListItem>
       <asp:ListItem Value="1">A</asp:ListItem>
       <asp:ListItem Value="2" >B</asp:ListItem>
        <asp:ListItem Value="3" >C</asp:ListItem>
    </asp:DropDownList>
    </asp:Panel>
</td>
<td class="tablecolumn"><asp:Panel id="DailyselectionPanel" runat="server"> Date:
 <asp:TextBox ID="cadatetextbox" ReadOnly="true" runat="server" Width="80px"></asp:TextBox>
<asp:ImageButton ID="ImageButton1" runat="server" OnClick="calbutton_Click" Height="17px"  ImageUrl="~/Images/calendar.png"/>
 <div id="datePicker" >
<asp:Calendar ID="Calnder1" runat="server" 
        onselectionchanged="Calnder11_SelectionChanged" Visible="False" 
        Height="200px" BackColor="White" BorderColor="#3366CC" 
        DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" 
        Width="220px" BorderWidth="1px" CellPadding="1">
    <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
    <WeekendDayStyle BackColor="#CCCCFF" />
    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
    <OtherMonthDayStyle ForeColor="#999999" />
    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
    <DayHeaderStyle BackColor="#99CCCC" Height="1px" ForeColor="#336666" />
    <TitleStyle BackColor="#003399" Font-Bold="True" Font-Size="10pt" 
        ForeColor="#CCCCFF" BorderColor="#3366CC" BorderWidth="1px" 
        Height="25px" />
        </asp:Calendar>
        <%--<asp:CustomValidator ID="calenderCustom" runat="server" Display="Dynamic" OnServerValidate="calenderCustom_ServerValidate" ErrorMessage="Select date">
        </asp:CustomValidator>--%>
        </div></asp:Panel>

<div><asp:HiddenField  id="manningHidden" runat="server"/>
<asp:Label ID="ManningLabel" runat="server" Text=""></asp:Label>
    <div>
    </div>
<td class="tablecolumn"><asp:Panel id="MonthlyselectionPanel" runat="server">
 <asp:DropDownList ID="ddlMonth" runat="server" Width="80px">
      <asp:ListItem Value="0">--Select--</asp:ListItem>
 </asp:DropDownList>
 <asp:RangeValidator ID="RangeValidator1" runat="server"
ControlToValidate="ddlMonth" ErrorMessage="Select Month"
MaximumValue="100" MinimumValue="1" Type="Integer"></asp:RangeValidator>
<asp:DropDownList ID="ddlYear" runat="server" Width="80px" 
        onselectedindexchanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

<asp:Label ID="Label1" runat="server"></asp:Label> 
</asp:Panel></td><td class="tablecolumn"> Select User:<asp:DropDownList ID="MachineDropdown" runat="server" Width="110px"
        onselectedindexchanged="MachineDropdown_SelectedIndexChanged" AppendDataBoundItems="True">
<%--<asp:ListItem Value="0" >--Select User--</asp:ListItem>--%>
</asp:DropDownList>
</td> 
<td class="tablecolumn">
<asp:Button ID="Vbutton" CssClass="button" Text="View" runat="server" OnClick="ViewMachineButton_Click" />
<asp:Button ID="ExptoExcelButton" CssClass="button" Text="Excel" runat="server" OnClick="ExcelMachineButton_Click"/>
<asp:Label ID="MessageLabel" runat="server"></asp:Label>
</td>
</tr> 
</table>
</div>
<div id="LabelDive" runat="server" style ="text-align:center; font-size:20px; font-family:Arial Narrow; padding-top:10px; padding-bottom:10px;"> 
<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="False" 
        BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="250px"></asp:Label>
</div>
<div id="gridDive" style="height:400px; overflow:scroll; width:100%;" runat="server">

    <asp:GridView ID="GridViewproduction" runat="server" 
        AutoGenerateColumns="false" Width="100%">
    <Columns>
    <asp:BoundField HeaderText="Date" DataField="Date" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="Machine Name" DataField="wcName" HeaderStyle-CssClass="tableheadercolumn"/> 
   <asp:BoundField HeaderText="Shift" DataField="shift" HeaderStyle-CssClass="tableheadercolumn"/> 
   <asp:BoundField HeaderText="Inspector Name." DataField="InspectorName" HeaderStyle-CssClass="tableheadercolumn"/> 
   <asp:BoundField HeaderText="Employee No." DataField="sapcode" HeaderStyle-CssClass="tableheadercolumn"/> 
      <asp:BoundField HeaderText="Total Checked Tyre" DataField="tcheckedtyre" HeaderStyle-CssClass="tableheadercolumn" />
    </Columns>
        </asp:GridView>
          <asp:PlaceHolder ID="VIproductiondailyplaceholder" EnableViewState="true" runat="server"> </asp:PlaceHolder>

     </div>
<asp:Panel ID="MonthWisePanel" runat="server" Width="100%" Visible="false">
<div id="MinthWiseDive" style="overflow:scroll; height:400px;" >
 <table class="innertableheadercolumn" cellspacing="1">
        <tr>
            <td style="text-align:center; padding:5px" >
            <asp:Label ID="MonthNameLabelId" runat="server" Font-Bold="true" ></asp:Label></td>
        </tr>
    </table>
  <asp:GridView ID="MonthwiseGridView" runat="server" AutoGenerateColumns="true" onrowdatabound="MonthwiseGridView_RowDataBound"
        Width="100%" HeaderStyle-BackColor="#C3D9FF">
         
  </asp:GridView>
  <asp:PlaceHolder ID="VIproductiondataplaceholder" EnableViewState="true" runat="server"> </asp:PlaceHolder>

  <div id="Labeldive2" runat="server" style ="text-align:center; font-size:20px; font-family:Arial Narrow; padding-top:10px; padding-bottom:10px;"> 
<asp:Label runat="server" ID="InnerLabel" Text="No Records Found!" Visible="False" 
        BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="250px"></asp:Label>
        
</div>
</div>

  </asp:Panel>
</asp:Content>

