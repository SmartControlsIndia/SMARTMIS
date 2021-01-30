<%@ Page Language="C#" MasterPageFile="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="REC_DELETE.aspx.cs" Inherits="REC_DELETE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .style3
        {
            width: 287px;
        }
        .style1
        {
            width: 279px;
        }
        .style4
        {
            width: 90px;
        }
        .style5
        {
            width: 272px;
        }
          .style2
        {
            
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   

  <table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
             <table align="center" width="50%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="5" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>RECIPE DELETE</u></span> </h2>
                          
                    </td>
                   </tr>
                <tr align="center">
                
                    <td align="left"  class="style5">
                        Recipe Code</td>
                    <td align="left" class="style1">
                        <asp:DropDownList ID="TxtRecCode" runat="server" AutoPostBack="true" 
                            Height="20px" onselectedindexchanged="TxtRecCode_SelectedIndexChanged" 
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="style4">
                         Press Make</td>
                    <td align="right">
                        <asp:TextBox ID="txtpressmake" runat="server" Enabled="False" Height="20px" 
                            Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                
                    <td align="left" class="style5">
                        Discription</td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="TxtDiscription" runat="server" Enabled="False" Height="20px" 
                            Width="587px"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                
                    <td align="left" class="style5">
                        Created By</td>
                    <td align="left" class="style1">
                        <asp:TextBox ID="TxtCreatedBy" runat="server" Enabled="False" Height="20px" 
                            Width="200px"></asp:TextBox>
                    </td>
                    <td align="right" class="style4">
                         Created On</td>
                    <td align="right">
                        <asp:TextBox ID="TxtCreatedOn" runat="server" Enabled="False" Height="20px" 
                            Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                     <td colspan="2">
                            <asp:Button ID="btndelete" runat="server" onclick="btndelete_Click" Text="DELETE" />
                     </td>
                </tr>
                <tr align="center">
                      <td align="center" colspan="2">
                      <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
                Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
                      </td>
                </tr>
              
            </table>     
        <br />
             <table align="center" width="90%">
            <tr align="center" 
                style="color: #0000FF; font-weight: bold; font-size: small  ; background-color: #CCCCFF;" b>
                <td colspan="2" style="text-align: center">
                    STEP NO</td>
                <td style="text-align: center">
                    &nbsp;</td>
                <td colspan="17" style="text-align: center">
                    VALUE SELECTION</td>
            </tr>
            <tr align="center" style="background-color:	#736AFF; color: #FFFFFF;">
                <td>
                </td>
                <td class="style2">
                    Time|Temp</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblValve1" runat="server" Text="valve0"></asp:Label>
                </td>
                <td>
                    PS</td>
                <td>
                    <asp:Label ID="lblValve2" runat="server" Text="valve1"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve3" runat="server" Text="valve2"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve4" runat="server" Text="valve3"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve5" runat="server" Text="valve4"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve6" runat="server" Text="valve5"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve7" runat="server" Text="valve6"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve8" runat="server" Text="valve7"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve9" runat="server" Text="valve8"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve10" runat="server" Text="valve9"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve11" runat="server" Text="valve10"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve12" runat="server" Text="valve11"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve13" runat="server" Text="valve12"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve14" runat="server" Text="valve13"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve15" runat="server" Text="valve14"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblValve16" runat="server" Text="valve15"></asp:Label>
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Step01"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min1" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec1" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S01</td>
                <td>
                    <asp:CheckBox ID="val1chk1" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps1" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk1" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk1" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Step02"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min2" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec2" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S02</td>
                <td>
                    <asp:CheckBox ID="val1chk2" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps2" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk2" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk2" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Step03"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min3" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec3" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S03</td>
                <td>
                    <asp:CheckBox ID="val1chk3" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps3" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk3" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk3" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Step04"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min4" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec4" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S04</td>
                <td>
                    <asp:CheckBox ID="val1chk4" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps4" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk4" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk4" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Step05"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min5" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec5" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S05</td>
                <td>
                    <asp:CheckBox ID="val1chk5" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps5" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk5" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk5" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Step06"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min6" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec6" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S06</td>
                <td>
                    <asp:CheckBox ID="val1chk6" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps6" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk6" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk6" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Step07"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min7" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec7" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S07</td>
                <td>
                    <asp:CheckBox ID="val1chk7" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps7" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk7" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk7" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Step08"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min8" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec8" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S08</td>
                <td>
                    <asp:CheckBox ID="val1chk8" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps8" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk8" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk8" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val13chk8" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val14chk8" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val15chk8" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val16chk8" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label9" runat="server" Text="Step09"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min9" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec9" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S09</td>
                <td>
                    <asp:CheckBox ID="val1chk9" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps9" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk9" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk9" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label10" runat="server" Text="Step10"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min10" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec10" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S10</td>
                <td>
                    <asp:CheckBox ID="val1chk10" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps10" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk10" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk10" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val13chk10" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val14chk10" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val15chk10" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val16chk10" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Step11"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min11" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec11" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S11</td>
                <td>
                    <asp:CheckBox ID="val1chk11" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps11" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk11" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk11" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val13chk11" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val14chk11" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val15chk11" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val16chk11" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Step12"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min12" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec12" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S12</td>
                <td>
                    <asp:CheckBox ID="val1chk12" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps12" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk12" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk12" runat="server" />
                </td>
                  <td>
                    <asp:CheckBox ID="val13chk12" runat="server" />
                </td>
                  <td>
                    <asp:CheckBox ID="val14chk12" runat="server" />
                </td>
                  <td>
                    <asp:CheckBox ID="val15chk12" runat="server" />
                </td>
                  <td>
                    <asp:CheckBox ID="val16chk12" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label13" runat="server" Text="Step13"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min13" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec13" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S13</td>
                <td>
                    <asp:CheckBox ID="val1chk13" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps13" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk13" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk13" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label14" runat="server" Text="Step14"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min14" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec14" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S14</td>
                <td>
                    <asp:CheckBox ID="val1chk14" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps14" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk14" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk14" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label15" runat="server" Text="Step15"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min15" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec15" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S15</td>
                <td>
                    <asp:CheckBox ID="val1chk15" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps15" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk15" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk15" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val13chk15" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val14chk15" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val15chk15" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val16chk15" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label16" runat="server" Text="Step16"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min16" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec16" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S16</td>
                <td>
                    <asp:CheckBox ID="val1chk16" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps16" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk16" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk16" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label17" runat="server" Text="Step17"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min17" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec17" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S17</td>
                <td>
                    <asp:CheckBox ID="val1chk17" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps17" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk17" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk17" runat="server" />
                </td>
            </tr>
           <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label20" runat="server" Text="Step18"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min18" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec18" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S18</td>
                <td>
                    <asp:CheckBox ID="val1chk18" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps18" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk18" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk18" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val13chk18" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val14chk18" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val15chk18" runat="server" />
                </td>
                 <td>
                    <asp:CheckBox ID="val16chk18" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#B4CFEC">
                <td>
                    <asp:Label ID="Label21" runat="server" Text="Step19"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min19" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec19" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S19</td>
                <td>
                    <asp:CheckBox ID="val1chk19" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps19" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk19" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk19" runat="server" />
                </td>
            </tr>
            <tr align="center" style="background-color:#C2DFFF">
                <td>
                    <asp:Label ID="Label22" runat="server" Text="Step20"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="min20" runat="server" Width="22px"></asp:TextBox>
                    <asp:TextBox ID="sec20" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    S20</td>
                <td>
                    <asp:CheckBox ID="val1chk20" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="ps20" runat="server" Width="22px"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="val2chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val3chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val4chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val5chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val6chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val7chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val8chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val9chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val10chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val11chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val12chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val13chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val14chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val15chk20" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="val16chk20" runat="server" />
                </td>
            </tr>
            </table>
         <br />
       </td>
    </tr>
  </table>
       
    </asp:Content>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               