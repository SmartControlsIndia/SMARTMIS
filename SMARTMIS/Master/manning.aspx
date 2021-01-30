<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manning.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.manning" %>
<%@ Register Src="~/UserControl/calenderTextBox.ascx" TagName="calanderTextBox" TagPrefix="asp" %>

<asp:Content ID="manningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
   
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= manningIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= manningIDLabel.ClientID %>').innerHTML = value;
        }
        
    </script>
    
    <asp:ScriptManager ID="manningScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="manningUpdatePanel" runat="server">
        <ContentTemplate>

    <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="masterFirstCol"></td>
            <td class="masterSecondCol"></td>
            <td class="masterThirdCol"></td>
            <td class="masterForthCol"></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Manning</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">First Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningFirstNameTextBox" Width="80%" runat="server"></asp:TextBox>
                 <asp:Label ID="manningIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="manningFNameReqFieldValidator" 
                    runat="server" ControlToValidate="manningFirstNameTextBox"
                    CssClass="reqFieldValidator" ErrorMessage="First Name is Required">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Last Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningLastNameTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="manningLNameReqFieldValidator" 
                    runat="server" ControlToValidate="manningLastNameTextBox"
                    CssClass="reqFieldValidator" ErrorMessage="Last Name is Required">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Gender :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="manningGenderDropDownList" Width="82%" runat="server">
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="manningGenderReqFieldValidator" runat="server" 
                    ControlToValidate="manningGenderDropDownList" CssClass="reqFieldValidator" ErrorMessage="Gender is Required">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">DOB :</td>
            <td class="masterTextBox">
                <asp:calanderTextBox id="manningDOBCalenderTextBox" runat="server" width="69%"/>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Address :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningAddressTextBox" runat="server" Width="80%" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Contact No. :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningContactNoTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Email ID : </td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningEmailTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
                <asp:RegularExpressionValidator ID="userRegEmailFieldValidator" runat="server" 
                    ControlToValidate="manningEmailTextBox" CssClass="reqFieldValidator"
                    ErrorMessage="Email is not in correct format." Font-Size="Small" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Department :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="manningDepartmentNameDropDownList" Width="82%" runat="server" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                </asp:DropDownList>
                <asp:Label ID="manningDeptIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="manningDeptReqFieldValidator" runat="server" 
                    ControlToValidate="manningDepartmentNameDropDownList" 
                    CssClass="reqFieldValidator" ErrorMessage="Department is RequiredS">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Process :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="processTypeDropDownList" Width="82%" runat="server" 
                    onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                     <asp:ListItem Value="TBMTBR">TBMTBR</asp:ListItem>
                    <asp:ListItem Value="TBMPCR">TBMPCR</asp:ListItem>
                    <asp:ListItem Value="PCRCuring">Curing PCR</asp:ListItem>
                    <asp:ListItem Value="TBRCuring">Curing TBR</asp:ListItem>
                   <asp:ListItem Value="TBRFF">TBR FF</asp:ListItem>
                   <asp:ListItem Value="PCRFF">PCR FF</asp:ListItem>
                    <asp:ListItem Value="TUOClassifications">TUOClassifications</asp:ListItem>
                    <asp:ListItem Value="XrayTBR">XrayTBR</asp:ListItem>
                    <asp:ListItem Value="VITBR">VITBR</asp:ListItem>
                    <asp:ListItem Value="VIPCR">VIPCR</asp:ListItem>
                       <asp:ListItem Value="SLVIPCR">SLVIPCR</asp:ListItem>
                        <asp:ListItem Value="TUOSettingPCR">TUOSettingPCR</asp:ListItem>
                    <asp:ListItem Value="">Others</asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">SAP Code :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="manningSAPCodeTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td>
                <span class="errorSpan">*</span>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="SAPCodeRequiredFieldValidator"
                    runat="server" ControlToValidate="manningSAPCodeTextBox"
                    CssClass="reqFieldValidator"  ErrorMessage="SAP Code is required!"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="manningSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
            <asp:Button ID="manningCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />
        </td>
        <td></td>
        <td></td>
            </tr>
            <tr>
        <td colspan="4">
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
                                         <asp:Label ID="manningMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete this record."></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td colspan="2">
                                         <asp:Button ID="manningDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                         <asp:Button ID="manningDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                     </td>
                                 </tr>
                             </table>
                         </div>
                     </div>
                 </div>
             </div>
        </td>
            </tr>
            <tr>
         <td>
             <asp:Timer ID="manningNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                 ontick="NotifyTimer_Tick">
             </asp:Timer>
             <asp:HiddenField ID="manningIDHidden" runat="server" Value="0" />
         </td>
         <td align="center">
             <div id="manningNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                 <table>
                     <tr>
                         <td>
                             <img id="manningNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                         </td>
                         <td>
                             <asp:Label id="manningNotifyLabel" runat="server" Text="This record saved successfully."></asp:Label>
                         </td>
                     </tr>
                 </table>
             </div>
         </td>
            <td></td>
            <td></td>
         </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Manning List</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:11%; padding:5px">Name</td>
                    <td class="gridViewHeader" style="width:4%; padding:5px">Gender</td>
                    <td class="gridViewHeader" style="width:5%; padding:5px">DOB</td>
                    <td class="gridViewHeader" style="width:8%; padding:5px">Address</td>
                    <td class="gridViewHeader" style="width:8%; padding:5px">Contact No</td>
                    <td class="gridViewHeader" style="width:16%; padding:5px">Email ID</td>
                    <td class="gridViewHeader" style="width:7%; padding:5px">Department</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Area Name</td>
                    <td class="gridViewHeader" style="width:7%; padding:5px">SAP Code</td>
                    <td class="gridViewHeader" style="width:2%; padding:5px"></td>
                </tr>
                </table>
                <asp:Panel ID="manningPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="manningGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="deptID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="manningDeptGridIDLabel" runat="server" Text='<%# Eval("deptID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridFirstNameLabel" runat="server" Text='<%# Eval("firstName") %>' CssClass="gridViewItems"></asp:Label>
                                    <asp:Label ID="manningGridLastNameLabel" runat="server" Text='<%# Eval("lastName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Gender" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridGenderLabel" runat="server" Text='<%# Eval("gender") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="DOB" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridDOBLabel" runat="server" Text='<%# Eval("dob") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Address" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridAddressLabel" runat="server" Text='<%# Eval("address") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Contact No" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridContactNoLabel" runat="server" Text='<%# Eval("contactNo") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Email ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridEmailLabel" runat="server" Text='<%# Eval("email") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Department" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridDepartmentLabel" runat="server" Text='<%# Eval("deptName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="areaNameLabel" runat="server" Text='<%# Eval("areaName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="SAP Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="manningGridSAPCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="manningGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                    <asp:ImageButton ID="manningGridDeleteImageButton" runat="server" Text="Delete" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;" />
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
        </table>
         </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
      
    
</asp:Content>