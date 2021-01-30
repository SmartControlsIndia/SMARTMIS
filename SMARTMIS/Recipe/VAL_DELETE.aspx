<%@ Page Language="C#" MasterPageFile="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="VAL_DELETE.aspx.cs" Inherits="VAL_DELETE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .style3
        {
            width: 287px;
        }
        .style5
        {
            width: 126px;
        }
        .style6
        {
            width: 113px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="50%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>VALVE DELETE</u></span> </h2>
                          
                    </td>
                </tr>
          </table>
          <table align="center" frame="above" 
              style="width: 50%; background-color:White;">
              <tr align="center">
                  <td align="right"  colspan="70%">
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Valve Name</td>
                  <td align="left"  colspan="30%">
                      <asp:DropDownList ID="txtvalvename" runat="server" Height="20px" Width="200px" 
                          AutoPostBack="True" onselectedindexchanged="txtvalvename_SelectedIndexChanged">
                      </asp:DropDownList>
                  </td>
              </tr>
              <tr align="center">
                  <td align="right" colspan="70%">
                     </td>
                  <td align="left" colspan="30%">
                      </td>
              </tr>
              <tr align="center">
                  <td align="right" colspan="70%">
                      </td>
                  <td align="left" colspan="30%">
                      <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                          Text="DELETE VALVE" Width="126px" style="height: 26px" />
                  </td>
              </tr>
          </table>
     
    

          <table align="center" frame="below" 
              style="width: 50%; background-color:White;">
              <tr align="center">
                  <td align="center" >
                  <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
            Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
        
                      </td>
                  
              </tr>
              
          </table>      
   </td>
</tr>
</table>
 
</asp:Content>