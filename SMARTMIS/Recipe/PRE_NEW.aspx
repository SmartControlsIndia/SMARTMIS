<%@ Page Language="C#" MasterPageFile="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="PRE_NEW.aspx.cs" Inherits="PRE_NEW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/menu.css" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="50%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>PRESS NEW</u></span> </h2>
                          
                    </td>
                </tr>
                <tr align="center">
                    <td align="right" >
                        Press Make</td>
                    <td align="left">
                        <asp:TextBox ID="Txpressname" runat="server" Height="20px" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                    <td align="right" >
                        &nbsp;</td>
                    <td align="left" >
                        &nbsp;</td>
                </tr>
                <tr align="center">
                    <td align="right">
                        &nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnnew" runat="server" onclick="btnnew_Click1" Text="SAVE" 
                            Width="106px" />
                    </td>
                </tr>
                <tr align="center">
                      <td  align="center" colspan="2">
                        <asp:Label ID="Label19" runat="server" align="center" Font-Bold="True" 
                              Font-Size="Small" Font-Strikeout="False" ForeColor="#CC0000"></asp:Label>
                     
                          &nbsp;</td>
                    
                  </tr>
        </table>
        </td>
    </tr>
</table>
  
     

<%--
   <asp:Panel ID="Panel1" runat="server" Width="100%">
          <table align="center" frame="below" 
              style="width: 50%; background-color:White;">
              <tr align="center">
                  <td align="center">
                  <asp:Label ID="Label19" align="center"  runat="server" Font-Bold="True" 
            Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
        
                      </td>
                  
              </tr>
              
          </table>
      </asp:Panel>
     --%>
        
</asp:Content>