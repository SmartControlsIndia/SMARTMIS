<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="ShearographyReport.aspx.cs" Inherits="SmartMIS.Report.ShearographyReport" Title="SHEAROGRAPHY REPORT" %>
<%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<link rel="stylesheet" href="../Style/reportMaster.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
<script type="text/javascript" language="javascript">
        function ExportToExcel()
         {
            var html = $("#exportArea").html();
            html = $.trim(html);
            html = html.replace(/>/g, '&gt;');
            html = html.replace(/</g, '&lt;');
            $("input[id$='HdnValue']").val(html);
        }
        function showDuration(duration,hideWCID) {
            
            switch (duration) {
                case "Day":
                case "day":
                    $("#setdate").slideDown();
                    break;
                case "Month":
                case "month":
                    $("#setMonth").slideDown();
                    break;
                case "Year":
                case "year":
                    $("#setYear").slideDown();
                    break;
            }
            if (hideWCID == "hideWCIDDiv")
            $("#dropdownarea").slideUp();
            
        }
        function toggleprocess() {
            $("#dropdownarea:visible").slideUp(700);
            $("#dropdownarea:hidden").slideDown(700);
        }
        function toggleduration() {
            if ($('select[id=ctl00_masterContentPlaceHolder_DropDownListDuration]').val() == "Date") {
                $("#setdate:visible").slideUp(700);
                $("#setdate:hidden").slideDown(700);
            }
            else if ($('select[id=ctl00_masterContentPlaceHolder_DropDownListDuration]').val() == "Month") {
                $("#setMonth:visible").slideUp(700);
                $("#setMonth:hidden").slideDown(700);
            }
            else if ($('select[id=ctl00_masterContentPlaceHolder_DropDownListDuration]').val() == "Year") {
                $("#setYear:visible").slideUp(700);
                $("#setYear:hidden").slideDown(700);
            }
        }
        function toggletype() {
            $("#ctl00_ctl00_masterContentPlaceHolder_operatorPanel:visible").slideUp(700);
            $("#ctl00_ctl00_masterContentPlaceHolder_operatorPanel:hidden").slideDown(700);
        }
        function setDuration() {

            var element = ["setdate", "setMonth", "setYear"];
            for (var i = 0; i <= element.length - 1; i++)
                $("#"+element[i]).slideUp(500);

            if (ctl00_masterContentPlaceHolder_DropDownListDuration.value == "Date")
                $("#setdate").slideDown(500);
            else if (ctl00_masterContentPlaceHolder_DropDownListDuration.value == "Month") {
            $("#setMonth").slideDown(500);
            } else if (ctl00_masterContentPlaceHolder_DropDownListDuration.value == "Year")
                $("#setYear").slideDown(500);
            
            //alert(ctl00_ctl00_masterContentPlaceHolder_DropDownListDuration.value);
             
            document.getElementById("ctl00_ctl00_masterContentPlaceHolder_DropDownListDuration").value = ctl00_ctl00_masterContentPlaceHolder_DropDownListDuration.value;
        }
       
        function hidewcDiv() {
            $("#dropdownarea:visible").slideUp(500);
        }
    </script>
    <script type="text/javascript" language="javascript">

        function enableSelection(value) 
        {
            var currentTime = new Date()

            var day = currentTime.getDate();
            var month = currentTime.getMonth() + 1;
            var year = currentTime.getFullYear();

            if (day.toString().length < 2) 
            {
                day = "0" + day;
            }
            if (month.toString().length < 2) 
            {
                month = "0" + month;
            }
            
            currentTime = (day + "-" + month + "-" + year);
            
            if (value == "0")
            {

                document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterFromDateTextBox_calenderUserControlTextBox').disabled = false;
                document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterFromDateTextBox_calenderUserControlTextBox').value = currentTime;

                document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterToDateTextBox_calenderUserControlTextBox').disabled = false;
                document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterToDateTextBox_calenderUserControlTextBox').value = currentTime;

            }           
            
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
       width:10%;
       
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:100%;
       
      
   }
   
   tr
   {
      border:1pt solid black;
      
     
   }
   
   .tableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
       background-color:#C3D9FF;
       
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
         width: 17%;
     }
   
 </style>
<style>
    #ctl00_ctl00_masterContentPlaceHolder_reportMasterWCPanel
    {
        position:relative;
        overflow:auto;
    }
    .operatorclass
    {
        border: 1px solid #129FEA;
        padding: 6px 0;
        text-align: center;
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
        -webkit-box-shadow: #666 0px 2px 3px;
        -moz-box-shadow: #666 0px 2px 3px;
        box-shadow: #666 0px 2px 3px;
        position:absolute;
        width:20%;
        background-color: #15497C;
        background: -webkit-linear-gradient(top, #C3D9FF, #8DB3E1); /* For Safari 5.1 to 6.0 */
        background: -o-linear-gradient(bottom, #C3D9FF, #8DB3E1); /* For Opera 11.1 to 12.0 */
        background: -moz-linear-gradient(top, #C3D9FF, #8DB3E1); /* For Firefox 3.6 to 15 */
        background: linear-gradient(top, #C3D9FF, #8DB3E1); /* Standard syntax (must be last) */
    }
    .durationclass
    {
        display:none;
        border: 1px solid #129FEA;
        padding: 6px 0;
        text-align: center;
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
        -webkit-box-shadow: #666 0px 2px 3px;
        -moz-box-shadow: #666 0px 2px 3px;
        box-shadow: #666 0px 2px 3px;
        position:absolute;
        width:21%;
        background-color: #15497C;
        background: -webkit-linear-gradient(top, #C3D9FF, #8DB3E1); /* For Safari 5.1 to 6.0 */
        background: -o-linear-gradient(bottom, #C3D9FF, #8DB3E1); /* For Opera 11.1 to 12.0 */
        background: -moz-linear-gradient(top, #C3D9FF, #8DB3E1); /* For Firefox 3.6 to 15 */
        background: linear-gradient(top, #C3D9FF, #8DB3E1); /* Standard syntax (must be last) */
    }
    #dropdownarea
    { position:absolute;}
    </style>
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
       width:9%;
       
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:100%;
       
      
   }
   
   tr
   {
      border:1pt solid black;
      
     
   }
   
   .tableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
       background-color:#C3D9FF;
       
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
   
</style>
 <asp:TextBox ID="txtwcin" style="display:none" runat="server"></asp:TextBox>

 <asp:reportHeader ID="reportHeader" runat="server" />
<table width="100%" class="bder">
<tr>
<td width="15%" ><span class="masterLabel" style="padding-left: 5px; cursor:pointer;" onclick="toggleduration();">Duration :</span>
                   <asp:DropDownList ID="DropDownListDuration"  
                   Width="40%" runat="server" 
                   CausesValidation="false" 
                   style="margin-bottom: 0px" onchange="setDuration();">
                   <asp:ListItem Value="Select">--Select--</asp:ListItem>
                   <asp:ListItem Value="Date">Date</asp:ListItem>
                   <asp:ListItem Value="Month">Month</asp:ListItem>
                  
             </asp:DropDownList>
                                 
                             
                             <div id="setdate" class="durationclass">
                             <table width="100%"><tr>
                             <td class="masterLabel" width="24%">Date :</td>
                             <td class="masterLabel" width="6%"></td>
                             <td class="masterTextBox" width="60%">
                                
                                 <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Width="45%" />
                                 
                             </td>
                             
                             </tr>
                             </table>
                             </div>
                             <div id="setMonth" class="durationclass">
                             <table width="100%"><tr>
                             
                             <td class="masterLabel" width="24%">Month :</td>
                             <td class="masterLabel" width="6%"></td>
                             <td class="masterTextBox" width="60%">
                                
                                 <asp:DropDownList ID="DropDownListMonth"  
                   Width="55%" runat="server" 
                   CausesValidation="false" 
                   style="margin-bottom: 0px">
                   <asp:ListItem Value="01">January</asp:ListItem>
                   <asp:ListItem Value="02">February</asp:ListItem>
                   <asp:ListItem Value="03">March</asp:ListItem>
                   <asp:ListItem Value="04">April</asp:ListItem>
                   <asp:ListItem Value="05">May</asp:ListItem>
                   <asp:ListItem Value="06">June</asp:ListItem>
                   <asp:ListItem Value="07">July</asp:ListItem>
                   <asp:ListItem Value="08">August</asp:ListItem>
                   <asp:ListItem Value="09">September</asp:ListItem>
                   <asp:ListItem Value="10">October</asp:ListItem>
                   <asp:ListItem Value="11">November</asp:ListItem>
                   <asp:ListItem Value="12">December</asp:ListItem>
             </asp:DropDownList>
                                <asp:DropDownList ID="DropDownListYear"  
                   Width="35%" runat="server" 
                   CausesValidation="false" 
                   style="margin-bottom: 0px">
                   <asp:ListItem Text="2010">2010</asp:ListItem>
                   <asp:ListItem Text="2011">2011</asp:ListItem>
                   <asp:ListItem Text="2012">2012</asp:ListItem>
                   <asp:ListItem Text="2013">2013</asp:ListItem>
                   <asp:ListItem Text="2014">2014</asp:ListItem>
                   <asp:ListItem Text="2015">2015</asp:ListItem>
                   <asp:ListItem Text="2016">2016</asp:ListItem>
                   <asp:ListItem Text="2017">2017</asp:ListItem>
                   <asp:ListItem Text="2018">2018</asp:ListItem>
                   <asp:ListItem Text="2019">2019</asp:ListItem>
                   <asp:ListItem Text="2020">2020</asp:ListItem>
                   <asp:ListItem Text="2021">2021</asp:ListItem>
             </asp:DropDownList> 
                             </td>
                             
                             
                             </tr>
                             </table>
                             </div>
                             
             </td>

 
            

<td class="tablecolumn">
    <asp:Button ID="ViewButton" CssClass="button"  runat="server" Text="View " OnDataBound = "OnDataBound" 
        onclick="viewReport_Click"/>
    </td>
    &nbsp;<td>
    <asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
 <%--<asp:Button ID="ExcelButton" CssClass="button"  runat="server" Text="Excel" OnClick="expToExcel_Click"/>--%>
    </td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>
 <td class="tablecolumn"></td> 
 <td class="tablecolumn"></td>
</tr>

   </table>
   
   </br>
   
  <div align="center" style="width:100%; " visible="false">
<asp:Label ID="lblText" runat="server" Visible="False" BackColor="Gray"></asp:Label></div>
 <div style="height:400px; overflow: scroll;">
           <asp:GridView ID="MainGridView" runat="server"  CssClass="TBMTable " HeaderStyle-HorizontalAlign="Left" EmptyDataRowStyle-BackColor="Gray" 
            ShowHeader="true"  EmptyDataRowStyle-Width="100%" 
             EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound = "GridView_RowDataBound" Width="100%"
             EmptyDataText="No Records Found" 
            ShowFooter="false" >
            <Columns>
             <%--<asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
            </Columns>
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
           </asp:GridView>
           </div>
  
      
      
    
</asp:Content>
