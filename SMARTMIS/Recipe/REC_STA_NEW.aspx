<%@ Page Language="C#" MasterPageFile ="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="REC_STA_NEW.aspx.cs" Inherits="REC_STA_NEW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        
        .style3
        {
            width: 287px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="35%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>
                                RECIPE STRUCTURE NEW</u></span> </h2>
                          
                    </td>
                </tr>   
                 <tr>
                    <td align="left" >
                        Press Make</td>
                    <td align="right" >
                        <%--<asp:TextBox ID="txtpressname" runat="server" Height="20px" Width="200px"></asp:TextBox>--%>
                        <asp:DropDownList ID="txtpressname" runat="server" Height="20px" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" >
                        Step</td>
                    <td align="right" >
                        <asp:DropDownList ID="txtstep" runat="server" Height="20px" Width="200px" 
                            AutoPostBack="True" onselectedindexchanged="txtstep_SelectedIndexChanged">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" >
                        Valves</td>
                    <td align="right" >
                        <asp:DropDownList ID="txtvalve" runat="server" AutoPostBack="True" 
                            Height="20px" onselectedindexchanged="txtvalve_SelectedIndexChanged" 
                            Width="200px">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>     
<table align="center" width="25%">
               <tr align="center" ><td>
                       <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="SAVE" 
        Width="107px" />
        </td>
                </tr>
                
          </table>      
          <table align="center" width="100%">
              <tr align="center">
                  <td align="center" >
                  <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
            Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
        
                      </td>
                  
              </tr>
              
          </table>
   
    


    
    
    
        
    
        <br />
    
         <table align="center" width="90%" frame="box" style="border-style: outset"> 
    <tr align="center">
                <td align="left" >
                    VALVE -1</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve1" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -5</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve5" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -9</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve9" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -13</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve13" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
            
            </tr>
           <tr align="center">
                <td align="left" >
                    VALVE -2</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve2" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -6</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve6" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -10</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve10" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                 <td align="left" >
                     VALVE -14</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve14" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
             
            </tr>
           <tr align="center">
                <td align="left" >
                    VALVE -3</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve3" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -7</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve7" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -11</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve11" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                 <td align="left" >
                     VALVE -15</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve15" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
               
            </tr>
           <tr align="center">
                <td align="left" >
                    VALVE -4</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve4" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -8</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve8" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -12</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve12" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                  <td align="left" >
                      VALVE -16</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve16" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
              
            </tr>
            
        </table>
        
</td>
</tr>
</table>
    
     <br />
  
</asp:Content>