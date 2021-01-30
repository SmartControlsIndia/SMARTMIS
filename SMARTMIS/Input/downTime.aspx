<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="downTime.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.downTime" %>
<%@ Register src="../UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="asp" %>

<asp:Content ID="downTimeContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/downtime.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script language="javascript" type="text/javascript">

        function setID(authentication, value)
        {
            if (authentication == "True") {
                document.getElementById('<%= downTimeIDHidden.ClientID %>').value = value.toString();
                document.getElementById('<%= downTimeIDLabel.ClientID %>').innerHTML = value;
                revealModal('modalPage');
            }
            else {
                revealModal('modalPageForAuthentication');
            }
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

                document.getElementById('ctl00_masterContentPlaceHolder_downTimeFromDateTextBox_calenderUserControlTextBox').disabled = false;
                document.getElementById('ctl00_masterContentPlaceHolder_downTimeFromDateTextBox_calenderUserControlTextBox').value = currentTime;

                document.getElementById('ctl00_masterContentPlaceHolder_downTimeToDateTextBox_calenderUserControlTextBox').disabled = false;
                document.getElementById('ctl00_masterContentPlaceHolder_downTimeToDateTextBox_calenderUserControlTextBox').value = currentTime;

                document.getElementById('<%= this.downTimeAllPendingDropDownList.ClientID %>').disabled = false;
                document.getElementById('<%= this.downTimeAllPendingDropDownList.ClientID %>').value = 'All';
            }
            else if (value == "1")
            {
                document.getElementById('ctl00_masterContentPlaceHolder_downTimeFromDateTextBox_calenderUserControlTextBox').disabled = true;
                document.getElementById('ctl00_masterContentPlaceHolder_downTimeFromDateTextBox_calenderUserControlTextBox').value = "";

                document.getElementById('ctl00_masterContentPlaceHolder_downTimeToDateTextBox_calenderUserControlTextBox').disabled = true;
                document.getElementById('ctl00_masterContentPlaceHolder_downTimeToDateTextBox_calenderUserControlTextBox').value = "";

                document.getElementById('<%= this.downTimeAllPendingDropDownList.ClientID %>').disabled = true;
                document.getElementById('<%= this.downTimeAllPendingDropDownList.ClientID %>').value = '';
            }
        }

    </script>
    
    <script type="text/javascript" language="javascript">
        function getWCIDFromGridView()
        {
            document.getElementById('<%= this.downTimeWCIDLabel.ClientID %>').innerHTML = "";
            //get reference of GridView control
            var grid = document.getElementById('<%= this.downTimeWCGridView.ClientID %>');
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 0; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];
                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[j].checked == true) {
                            document.getElementById('<%= this.downTimeWCIDLabel.ClientID %>').innerHTML = document.getElementById('<%= this.downTimeWCIDLabel.ClientID %>').innerHTML + "#" + cell.childNodes[j].value;
                        }

                    }
                }
            }
        }
        function queryString()
        {
            var splitter = "?";

            var wcID = getWCID(document.getElementById('<%= this.downTimeWCIDLabel.ClientID %>').innerHTML);
            var reportChoice = getReportChoice();
            var fromDate = getDate('ctl00_masterContentPlaceHolder_downTimeFromDateTextBox_calenderUserControlTextBox');
            var toDate = getDate('ctl00_masterContentPlaceHolder_downTimeToDateTextBox_calenderUserControlTextBox');
            var reportType = getIndex('<%= this.downTimeAllPendingDropDownList.ClientID %>');

            showReport(wcID + splitter + reportChoice + splitter + fromDate + splitter + toDate + splitter + reportType);

        }
        function getWCID(controlID)
        {
            if (controlID == "") {
                return "0";
            }
            else {
                return controlID;
            }

        }
        function getReportChoice()
        {
            var choice;
            if (document.getElementById('<%= this.downTimeRangeRadioButton.ClientID %>').checked == true) {
                choice = "0";
            }
            else if (document.getElementById('<%= this.downTimePendingRadioButton.ClientID %>').checked == true) {
                choice = "1";
            }

            return choice;
        }
        function getDate(controlID)
        {
            var fromDate;
            if ((document.getElementById(controlID).value) == "") {
                fromDate = "0";
            }
            else {
                fromDate = (document.getElementById(controlID).value);
            }

            return fromDate;
        }
        function getIndex(controlID)
        {
            var objDropDownList = document.getElementById(controlID);
            return objDropDownList.selectedIndex;
        }
        function showReport(queryString)
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }    
    </script>
    
    <asp:ScriptManager ID="downTimeScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="downTimeUpdatePanel" runat="server">   
        <ContentTemplate>
            <table class="downTimeTable" align="center" cellpadding="0" cellspacing="0">
                <tr >
                    <td class="downTimeFirstCol"></td>           
                    <td class="downTimeSecondCol"></td>
                    <td class="downTimeThirdCol"></td>
                    <td class="downTimeForthCol"></td>
                    <td class="downTimeForthCol"></td>
                </tr>
                <tr >
                    <td colspan="5">
                        <div class="masterHeader" >
                            <p class="masterHeaderTagline" >Downtime Reason</p>
                        </div>
                    </td>
                   
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5" align="center">
                        <div class="downTimeDiv">
                                <table class="downTimeContentTable" align="center">
                                    <tr>
                                        <td style="width:5%"></td>
                                        <td style="width:15%"></td>
                                        <td style="width:10%"></td>
                                        <td style="width:20%"></td>
                                        <td style="width:8%"></td>
                                        <td style="width:20%"></td>
                                        <td style="width:22%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">&nbsp;
                                            <asp:Label id="downTimeWCIDLabel" runat="server" Text="0" />
                                        </td>
                                    </tr>                     
                                    <tr>
                                        <td colspan="7">
                                            <div id="downTimeWCDiv" runat="server" class="glossymenu">
                                                <a class="menuitem submenuheader" href="#" >Workcenter Name</a>
                                                    <div class="submenu">
                                                            <asp:Panel ID="downTimeWCPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                                                <asp:GridView ID="downTimeWCGridView" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="downTimeWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <input id="downTimeWCCheckBox" runat="server" Value='<%# Eval("iD") %>' onClick="javascript:getWCIDFromGridView()" CssClass="gridViewItems" type="checkbox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="95%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="downTimeWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
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
                                                    </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="masterHeader">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td><input ID="downTimeRangeRadioButton" runat="server" type="radio" onclick="javascript:enableSelection('0');" /> </td>
                                        <td class="masterLabel">Range :</td>
                                        <td class="masterLabel">From :</td>
                                        <td class="masterTextBox">
                                            <asp:calenderTextBox ID="downTimeFromDateTextBox" runat="server" Disabled="true" Width="80%" />
                                        </td>
                                        <td class="masterLabel">To :</td>
                                        <td class="masterTextBox">
                                            <asp:calenderTextBox ID="downTimeToDateTextBox" runat="server" Disabled="true" Width="80%" />
                                        </td>
                                        <td style="text-align:center">
                                            <asp:DropDownList ID="downTimeAllPendingDropDownList" runat="server" CssClass="masterDropDownList" Enabled="false" Width="60%">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Pending</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><input ID="downTimePendingRadioButton" runat="server" type="radio" onclick="javascript:enableSelection('1');" /></td>
                                        <td class="masterLabel">Pending :</td>
                                        <td class="masterLabel"></td>
                                        <td></td>
                                        <td class="masterLabel"></td>
                                        <td></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" class="masterHeader">
                                         &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" align="center">
                                            <input id="downTimeViewButton" type="button" class="masterButton" 
                                                value="view" onclick="return queryString();" />&nbsp;
                                            <asp:Button ID="downTimeResetButton" runat="server" class="masterButton" Text="reset" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="downTimeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                        <asp:HiddenField ID="downTimeIDHidden" runat="server" Value="0" />
                        <asp:HiddenField ID="magicHidden" runat="server" Value="0" />                
                        <asp:Button ID="magicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" OnClick="magicButton_Click" CausesValidation="false" />
                    </td>
                    <td>
                        <div id="modalPage">
                          <div class="modalBackground">
                          </div>
                           <div class="modalContainer">
                              <div class="modal">
                                  <div class="modalTop"><a href="javascript:hideModal('modalPage')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                  <div class="modalBody">
                                      <table class="innerTable" cellspacing="0">
                                          <tr>
                                              <td style="width: 20%"></td>
                                              <td style="width: 80%"></td>
                                          </tr>
                                          <tr>
                                              <td class="masterLabel">
                                                  <img alt="Close" src="../Images/question.png" />
                                              </td>
                                              <td>
                                                  <asp:Label ID="downTimeMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Reason."></asp:Label>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td colspan="2">
                                                  <asp:Button ID="downTimeDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                  <asp:Button ID="downTimeDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                              </td>
                                          </tr>
                                      </table>
                                  </div>
                              </div>
                          </div>
                      </div>
                    </td>
                    <td>
                        <div id="modalPageForAlert">
                          <div class="modalBackground">
                          </div>
                           <div class="modalContainer">
                              <div class="modal">
                                  <div class="modalTop"><a href="javascript:hideModal('modalPageForAlert')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                  <div class="modalBody">
                                      <table class="innerTable" cellspacing="0">
                                          <tr>
                                              <td style="width: 20%"></td>
                                              <td style="width: 80%"></td>
                                          </tr>
                                          <tr>
                                              <td class="masterLabel">
                                                  <img alt="Close" src="../Images/exclamation.png" />
                                              </td>
                                              <td>
                                                  <asp:Label ID="downTimeAlertLabel" runat="server" CssClass="masterWelcomeLabel" Text="Unable to add reason."></asp:Label>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td colspan="2">
                                                  <asp:Button ID="downTimeDialogAlertOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClientClick="javascript:hideModal('modalPageForAlert'); return false" />&nbsp;
                                                  <asp:Button ID="downTimeDialogAlertCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPageForAlert'); return false" Text="Cancel" />
                                              </td>
                                          </tr>
                                      </table>
                                  </div>
                              </div>
                          </div>
                      </div>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Downtime Entry</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" valign="top" align="center">
                            <table class="innerTable" cellspacing="0">
                                <tr>
                                    <td class="gridViewHeader" style="width:15%; padding:5px; text-align:left">Workcenter Name</td>
                                    <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Downtime Event</td>
                                    <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Uptime Event</td>
                                    <td class="gridViewHeader" style="width:5%; padding:5px; text-align:left">Duration</td>
                                    <td class="gridViewHeader" style="width:55%; padding:5px; text-align:center">Reasons</td>
                                </tr>
                            </table>
                            <asp:Panel ID="downTimePanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                                <asp:GridView ID="downTimeGridView" runat="server" AutoGenerateColumns="False" 
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="downTimeWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="downTimeWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85%">
                                            <ItemTemplate>
                                                
                                            <%--child Gridview--%>
                                            <div class="inputTable">
                                                <asp:GridView ID="downTimeChildGridView" runat="server" AutoGenerateColumns="False" 
                                                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView_RowDataBound"
                                                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="downTimeChildIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="downTimeChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Down Event" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="13%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="downTimeDownEventLabel" runat="server" Text='<%# Eval("downEvent") %>' CssClass="gridViewItems"></asp:Label><br />
                                                                    <asp:Label ID="downTimeDownEventDateTimeLabel" runat="server" Text='<%# Eval("downdtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Up Event" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="13%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="downTimeUpEventLabel" runat="server" Text='<%# Eval("upEvent") %>' CssClass="gridViewItems"></asp:Label><br />
                                                                    <asp:Label ID="downTimeUpEventDateTimeLabel" runat="server" Text='<%# Eval("updtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="downTimeDurationLabel" runat="server" Text='<%# Eval("duration") %>' CssClass="gridViewItems"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="67%">
                                                                <ItemTemplate>
                                                                
                                                                 <%--Reason child Gridview--%>
                                                                <div class="inputTable">
                                                                    <asp:GridView ID="downTimeChildReasonGridView" runat="server" AutoGenerateColumns="False" 
                                                                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                                                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true" >
                                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="downTimereasonIDLabel" runat="server" Text='<%# Eval("reasonName") %>' CssClass="gridViewItems"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="downTimeUpDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Reason Duration (in minutes)" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="downTimeReasonDurationLabel" runat="server" Text='<%# Eval("downDuration") %>' CssClass="gridViewItems"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="downTimeEditImageButton" runat="server" ImageUrl="~/Images/edit.gif" CssClass="gridViewImageButton" ToolTip="Edit" OnClick="ImageButton_Click" />
                                                                                    <asp:ImageButton ID="downTimeDeleteImageButton" runat="server" ImageUrl="~/Images/delete.gif" AlternateText='<%# isAuthenticate("108")%>' Value='<%# Eval("downTimeReasonID") %>' OnClientClick="javascript:setID(this.alt, this.value); return false;" CssClass="gridViewImageButton" ToolTip="Delete" />
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
                                                                <table class="inputTable">
                                                                    <tr>
                                                                        <td style="width:15%">
                                                                            <asp:DropDownList ID="downTimeReasonIDDropDownList" runat="server" Width="95%" AutoPostBack="true"
                                                                               OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList"></asp:DropDownList>
                                                                        </td>
                                                                        <td style="width:35%">
                                                                            <asp:Label ID="downTimeReasonIDLabel" runat="server" Text="" CssClass="masterHiddenLabel" ></asp:Label>
                                                                            <asp:Label ID="downTimeReasonDescLabel" runat="server" Text="" CssClass="gridViewItems" ></asp:Label>
                                                                            <asp:Label ID="downTimeReasonLeftDurationLabel" runat="server" Text='<%# displayLeftDuration(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"ID"))%>' CssClass="gridViewItems" ></asp:Label>
                                                                        </td>
                                                                        <td style="width:40%">
                                                                            <asp:TextBox ID="downTimeDurationTextBox" runat="server" CssClass="masterDropDownList" MaxLength="3" AutoComplete="off" onKeyPress="return keyRestrict(event,'1234567890.')" Width="30%"></asp:TextBox>
                                                                            <asp:DropDownList ID="downTimeDurationDropDownList" runat="server" Width="40%" CssClass="masterDropDownList"></asp:DropDownList>
                                                                        </td>
                                                                        <td style="width:10%">
                                                                            <asp:ImageButton ID="downTimeAddImageButton" runat="server" ImageUrl="~/Images/review.png" CssClass="gridViewImageButton" ToolTip="Add" AlternateText="108" OnClick="ImageButton_Click" />
                                                                            <asp:ImageButton ID="downTimeCancelImageButton" runat="server" ImageUrl="~/Images/cancel.png" CssClass="gridViewImageButton" ToolTip="Cancel" AlternateText="108" OnClick="ImageButton_Click" />
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
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>         
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="magicButton" />
        </Triggers>
    </asp:UpdatePanel>
   
</asp:Content>