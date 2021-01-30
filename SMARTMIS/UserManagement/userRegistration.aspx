<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="userRegistration.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.userRegistration" %>
<%@ Register Src="~/UserControl/calenderTextBox.ascx" TagName="calanderTextBox" TagPrefix="ucDob" %>
<asp:Content ID="userRegistrationContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="SHORTCUT ICON" href="Images/favicon.ico" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/userRegistration.css" rel="stylesheet" type="text/css" />

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    &nbsp;<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table class="registrationTable" align="center" cellpadding="0" cellspacing="0">
        <tr >
            <td class="registrationFirstCol"></td>           
            <td class="registrationSecondCol"></td>
            <td class="registrationFirstCol" style="width: 31%"></td>
            <td class="registrationForthCol"></td>
        </tr>
        <tr >
            <td colspan="4">
                <div class="masterHeader" >
                    <p class="masterHeaderTagline" >Login Details</p>
                </div>
            </td>           
        </tr>
        <tr>
            <td class="masterLabel">User ID :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="userIDTextBox" runat="server" Width="80%" ></asp:TextBox>
                    <span class="errorSpan">&nbsp;*</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">                
                <asp:RequiredFieldValidator ID="userRegIDReqFieldValidator" runat="server" 
                    ControlToValidate="userIDTextBox" CssClass="reqFieldValidator" 
                    ErrorMessage="UserID is required!"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Password :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="passwordTextBox" runat="server" Width="80%" TextMode="Password"></asp:TextBox>
                <span class="errorSpan">&nbsp;*</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">                
                <asp:RequiredFieldValidator ID="userRegPasswordFieldValidator" runat="server" 
                    BorderStyle="None" ControlToValidate="passwordTextBox" 
                    CssClass="reqFieldValidator" ErrorMessage="Password is required!"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Re-type Password :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="rePasswordTextBox" runat="server" TextMode="Password" Width="80%"></asp:TextBox>
                <span class="errorSpan">&nbsp;*</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">            
                <asp:RequiredFieldValidator ID="userRegRePasswordFieldValidator" 
                    runat="server" BorderStyle="None" ControlToValidate="rePasswordTextBox" 
                    CssClass="reqFieldValidator" ErrorMessage="Retype password is required"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator" runat="server" 
                    ControlToCompare="passwordTextBox" ControlToValidate="rePasswordTextBox" 
                    CssClass="reqFieldValidator" ErrorMessage="Passwords do not match"></asp:CompareValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">SAP code :</td>
            <td class="masterTextBox">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
               <ContentTemplate>
                 <asp:DropDownList ID="userRegSAPCodeDropDown" CssClass="masterDropDownList" runat="server" Width="82%" AutoPostBack="true"
                       OnSelectedIndexChanged="DropDownList_SelectedIndexChanged"  ></asp:DropDownList>
               </ContentTemplate>                
                 </asp:UpdatePanel>
            
            </td>
            <td class="registrationFirstCol" style="width: 31%">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Personal Details</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">First Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="firstNameTextBox" Width="80%" runat="server"></asp:TextBox>
                <span class="errorSpan">&nbsp;*</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:RequiredFieldValidator ID="userRegFNameFieldValidator0" runat="server" 
                    ControlToValidate="firstNameTextBox" CssClass="reqFieldValidator" 
                    ErrorMessage="FirstName is required!"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Last Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="lastNameTextBox" Width="80%" runat="server"></asp:TextBox>
                <span class="errorSpan">&nbsp;*</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:RequiredFieldValidator ID="userRegLNameFieldValidator0" runat="server" 
                    ControlToValidate="lastNameTextBox" CssClass="reqFieldValidator" 
                    ErrorMessage="LastName is required!"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Gender :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="userRegGenderDropDownList" CssClass="masterDropDownList" Width="82%" runat="server">                   
                </asp:DropDownList>
                <span class="errorSpan">&nbsp;*</span></td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:RequiredFieldValidator ID="userRegDeptNameRequiredFieldValidator1" 
                    runat="server" ControlToValidate="userRegGenderDropDownList" 
                    CssClass="reqFieldValidator" ErrorMessage="Gender is required!"></asp:RequiredFieldValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Date of Birth :</td>
            <td class="masterTextBox">
            <ucDob:calanderTextBox id="txtDOB" runat="server" width="69%"/>
            <span class="errorSpan"> *</span>
            </td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:RequiredFieldValidator ID="userRegDOBRequiredFieldValidator" 
                    runat="server" ControlToValidate="txtDOB$calenderUserControlTextBox" 
                    CssClass="reqFieldValidator" ErrorMessage="DOB is required!">
                </asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Address :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="addressTextBox" Width="80%" TextMode="MultiLine" Rows="3" 
                    runat="server">
                </asp:TextBox>
            </td>
            <td class="registrationFirstCol" style="width: 31%"></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Contact No. :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="contactNoTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td class="registrationFirstCol" style="width: 31%"></td>
        </tr>
        <tr>
            <td class="masterLabel">Email ID : </td>
            <td class="masterTextBox">
                <asp:TextBox ID="emailTextBox" Width="80%" runat="server" ></asp:TextBox>
            </td>
            <td class="registrationFirstCol" style="width: 31%">
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="masterLabel">Department :</td>
            <td class="masterTextBox">
           
                    <asp:DropDownList ID="userRegDepartmentDropDownList" CssClass="masterDropDownList"  Width="82%" runat="server" 
                    AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="userRegDeptIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    <span class="errorSpan">*</span></td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:RequiredFieldValidator ID="userRegDeptNameRequiredFieldValidator0" 
                    runat="server" ControlToValidate="userRegDepartmentDropDownList" 
                    CssClass="reqFieldValidator" ErrorMessage="Department is required!"></asp:RequiredFieldValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                        <asp:Timer ID="userRegNotifyTimer" runat="server" Interval="5000" Enabled="false" OnTick="NotifyTimer_Tick" >
                        </asp:Timer>
                        </td>
            <td>
                <asp:HiddenField ID="passHidden" runat="server" Value="0" />
            </td>
            <td class="registrationFirstCol" style="width: 31%">
                <asp:HiddenField ID="mannIDHidden" runat="server" Value="0" />
            </td>
            <td>&nbsp;</td>
        </tr>
            <tr>
            <td>
           <div id="userRegNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td colspan="4">
                                        <img id="userRegNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td colspan="4">
                                        <asp:Label id="userRegNotifyLabel" runat="server" Text="User saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                  </td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Roles</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                    <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:20%; padding:5px">Role Name</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Checked</td>
                    <td class="gridViewHeader" style="width:60%; padding:5px"></td>


                </tr>
                </table>
            <asp:Panel ID="rolePanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="roleGridView" runat="server" AutoGenerateColumns="False" 
                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" DataSourceID="roleSqlDataSource"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                         
                          <Columns>
                    <asp:TemplateField HeaderText="Role Name" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="True" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="roleNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>        
                    <Columns>
                    <asp:TemplateField HeaderText="" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:CheckBox ID="roleCheckBox" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                  <Columns>
                    <asp:TemplateField HeaderText="" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70%">
                        <ItemTemplate>
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
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="registrationFirstCol" style="width: 31%">&nbsp;</td>
            <td>&nbsp;</td>            
        </tr>
        <tr>
            <td class="registrationLabel">
                <asp:SqlDataSource ID="roleSqlDataSource" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:mySQLConnection %>" 
                    SelectCommand="SELECT distinct name FROM dbo.roleMaster">
                </asp:SqlDataSource>
            </td>
            <td align="center">
                <asp:Button ID="signInButton" runat="server" OnClick="Button_Click" CssClass="masterButton" 
                    Text="Save" />&nbsp;
                <asp:Button ID="cancelButton" runat="server" CssClass="masterButton"
                    Text="Cancel"  />
                </td>
            <td class="registrationFirstCol" style="width: 31%"></td>
            <td></td>            
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="registrationFirstCol" style="width: 31%">&nbsp;</td>
            <td>&nbsp;</td>            
        </tr>
    </table> 
    </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

