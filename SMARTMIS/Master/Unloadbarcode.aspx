<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="Unloadbarcode.aspx.cs" Inherits="SmartMIS.Master.Unloadbarcode" Title="SMARTMIS" %>
<asp:Content ID="UnloadBarcodeContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
 <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
     <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <asp:ScriptManager ID="unloadBarcodeScriptManager" runat="server"></asp:ScriptManager>
    
    
    <asp:UpdatePanel ID="unloadBracodeUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
                   <div id="BarcodeFromToDiv" runat="server">
                  
                <tr>
                    <td colspan="12">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Unload Barcode</p>
                        </div>
                    </td>
                </tr>
     <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
          <asp:TextBox ID="BarcodeFromTextBox" runat="server" MaxLength="10" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
      <td style="width: 8%">
          <asp:TextBox ID="barcodeToTextBox" runat="server" MaxLength="3" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
      <asp:Button ID="BarcodeWiseButton" runat="server" CausesValidation="false" Text="Save" onclick="ViewButton_Click"/>&nbsp;
       <asp:Button ID="Button2" runat="server" CausesValidation="false" Text="Delete" onclick="deleteButton_Click"/>&nbsp;</td> 
                  
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">  &nbsp; </td>      
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">         
       </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; &nbsp; </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
        </td>       
   </tr>
    <tr>
                    <td>
                        <asp:Timer ID="oemNotifyTimer" runat="server" Interval="5000" Enabled="false" ontick="NotifyTimer_Tick">
                            
                        </asp:Timer>
                        <asp:HiddenField ID="rIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center" style="width: 413px">
                        <div id="oemNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="rNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="rNotifyLabel" runat="server" Text="Record Not Found." ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    </tr>
   </div>
                            </table>
                            
                            &nbsp;&nbsp;
                            <asp:Panel ID="gvpanel" runat="server" Height="500px" CssClass="panel" ScrollBars="Both"  >
<asp:GridView ID="MainGridView" runat="server" AutoGenerateColumns="true" 
                    Width="100%" CellPadding="3" ForeColor="Black"   AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true" HeaderStyle-BackColor="#C3D9FF" EmptyDataText = "NO RECORDS FOUND" >
                    <FooterStyle BackColor="#C3D9FF" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />                                   
                        <Columns>
                           

<asp:TemplateField HeaderText="SR No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="35%" />
</asp:TemplateField>

 
                        </Columns>
                        
                    </asp:GridView>
</asp:Panel>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="BarcodeWiseButton" />
        
        </Triggers>
        
        
      
    </asp:UpdatePanel>
    
     
</asp:Content>
