<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="PCR2ndLiveVI.aspx.cs" Inherits="SmartMIS.Report.PCR2ndLiveVI" Title="PCR2ndLineVI" %>
<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<style>
.ErrorMsgCSS
{
    padding:7px;width:30%;max-width:30%;height:auto;
    position:fixed;
    z-index: 1080;top:75px;left: 35%;
    -moz-border-radius: 15px;-webkit-border-radius: 15px;border-radius:15px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background:#fff8c4 10px 50%;
    border:1px solid #f2c779;
    color:#555;
    font: bold 12px verdana;
}
</style>
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
 
 <link rel="stylesheet" href="../Style/reportMaster.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <style type="text/css">

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
    <script type="text/javascript" language="javascript">
        $(function() {
        $("select").selectbox();
        });
        function showPrintPreview(id)
        {

            var el = document.getElementById('printArea');
            var backEl = document.getElementById('BackImage');
            var printPreviewEl = document.getElementById('PrintPreviewImage');
            var printEl = document.getElementById('PrintImage');

            if (id == 'PrintPreviewImage') 
            {
                document.body.style.visibility = "hidden";

                el.style.visibility = "visible";
                el.style.position = "absolute";
                el.style.top = "20px";
                el.style.left = "30px";

                backEl.style.display = "inline";
                printPreviewEl.style.display = "none";
                printEl.style.display = "inline";
            }
            else if (id == 'PrintImage') 
            {
                window.print();
            }
            else if (id == 'BackImage')
             {
                 location.reload(true);
            }
            
        }
    </script> 
    <script type="text/javascript">
    function closePopup() {
        setTimeout(function() {
            $('.ErrorMsgCSS').fadeOut(1500);
        }, 4000);

    }
    setTimeout(function() {
        $('.ErrorMsgCSS').fadeOut(1500);
    }, 4000);
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
    
    <script type="text/javascript" language="javascript">
        
        function getDate(controlID)
        {
            var fromDate;
            if((document.getElementById(controlID).value) == "")
            {
                fromDate = "0";
            }
            else
            {
                fromDate = (document.getElementById(controlID).value);
            }
            
            return fromDate;
        }

    </script>
    
    <script type="text/javascript" language="javascript">
        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
                return true;
            return false;
        }
    </script>
    
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
            
            document.getElementById("ctl00_masterContentPlaceHolder_DropDownListDuration").value = ctl00_ctl00_masterContentPlaceHolder_DropDownListDuration.value;
        }

    </script>

      <center><H3>PCR SECOND LINE REPORT</H3></center> 
      
      
      <table align="center" class="reportMasterTable" width="100%" border="1" style="border-collapse:collapse;">
         <tr>
         
           
             <td width="10%"><span class="masterLabel" style="padding-left: 5px; cursor:pointer;" onclick="toggleduration();">Duration :</span>
                   <asp:DropDownList ID="DropDownListDuration"  
                   Width="40%" runat="server" 
                   CausesValidation="false" 
                   style="margin-bottom: 0px" onchange="setDuration();">
                   <asp:ListItem Value="Select">--Select--</asp:ListItem>
                   <asp:ListItem Value="Date">Date</asp:ListItem>
                   <asp:ListItem Value="Month">Month</asp:ListItem>
                   <asp:ListItem Value="Year">Year</asp:ListItem>
                   
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
                             <div id="setYear" class="durationclass">
                             <table width="100%"><tr>
                             
                             <td class="masterLabel" width="24%">Year :</td>
                             <td width="6%"></td>
                             <td class="masterTextBox" width="60%">
                             <asp:DropDownList ID="DropDownListYearWise"  
                   Width="45%" runat="server" 
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
             <td width="10%"><span class="masterLabel" style="padding-left: 5px; cursor:pointer;" onclick="toggleduration();">TYPE :</span>
                   <asp:DropDownList ID="ddldecision"  
                   Width="40%" runat="server" 
                   CausesValidation="false">
                   <asp:ListItem Value="ALL">ALL</asp:ListItem>
                   <asp:ListItem Value="1">OK</asp:ListItem>
                   <asp:ListItem Value="2">MINOR</asp:ListItem>
                   <asp:ListItem Value="3">MAJOR</asp:ListItem>
                   
             </asp:DropDownList>
             </td>
               <td width="10%">
                   <asp:Button ID="reportMasterViewButton" runat="server" Text="View" OnClick="viewReport_Click" />
             </td>
            
            <td width="10%">
                <div id="printArea">
                    <div style="text-align:right;">
                         <asp:LinkButton runat="server" ID="btnExport" onclick="Export_click"><img src="../Images/Excel.jpg" class="imag" /></asp:LinkButton>
                    </div>
                 </div>
             </td>
         </tr>
         </table>
      
<asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="ErrorMsgCSS" Visible="false"></asp:Label>
    
   <asp:Label ID="HeaderText" runat="server" Text=""></asp:Label>
    <asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" OnDataBound = "OnDataBound" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound">
     <Columns>
            
     </Columns>
    </asp:GridView>
    </asp:Panel>

    <center>
    
    </center>
</asp:Content>
