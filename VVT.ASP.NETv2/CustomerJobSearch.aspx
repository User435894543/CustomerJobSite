<%@ Page Title="Customer Jobs" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="CustomerJobSearch.aspx.cs" Inherits="VVT.ASP.NETv2.CustomerJobSearch" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  
<!DOCTYPE html>

<html>


    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.0.min.js"></script>
<script  type="text/javascript">

    $(document).ready(function () {
        // Hide the Label to start with
        $('#<%=Label1.ClientID %>').hide();

            $('#<%=Refresh.ClientID %>').click(function ()
            {
                // Show the Label when save button is clicked
                $('#<%=Label1.ClientID %>').show();
            });
        });

</script>


    <style>
.container { 
  height: 200px;
  position: relative;
  border: 3px solid green; 
}

        .center {
            margin: 0;
            position: absolute;
            left: 50%;
        }

.fright{
            margin: 0;
            position: absolute;
            left: 62%;
}
        .under {
            align-self:end
        }
        .undercenter {
            margin: 0;
            position: absolute;
            left: 50%;
            align-self:end
        }
</style>


<body>
 
          <div class="navbar navbar-inverse navbar-fixed-top">
                    <ul class="nav navbar-nav">
                        <li><a runat="server" href="~/">Home</a></li>



                        <!--<li><a runat="server" href="~/About">About</a></li>-->
                        <li><a runat="server" href="~/Contact">Contact</a></li>

                    </ul>
                </div>

    
        <asp:datagrid id="GridView1" runat="server" GridLines="Vertical" CellPadding="3" BackColor="White"
BorderColor="#999999" BorderWidth="1px" BorderStyle="None" Width="100%" Height="100%" Font-Size="11"
Font-Names="Verdana">
<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
<ItemStyle BorderWidth="2px" ForeColor="Black" BorderStyle="Solid" BorderColor="Black" BackColor="#EEEEEE"></ItemStyle>
<HeaderStyle Font-Bold="True" HorizontalAlign="Center" BorderWidth="2px" ForeColor="White" BorderStyle="Solid"
BorderColor="Black" BackColor="#007A5D"></HeaderStyle>
<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
</asp:datagrid>

    <div class="left" runat="server">
        </div>
</body>
</html>

     <div class="center" runat="server">
                <asp:Button ID="Export" runat="server" Text="Export To Excel" OnClick="Export_Click" OnClientClick="ExporthLbl_Click"  Font-Size="Medium"/>
         </div>
       
    
    <!--
    <div class="fright" runat="server">
               <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="...Running" style="right"></asp:Label>
         </div>
        -->

    <asp:Button ID="Refresh" runat="server" Text="Refresh"  OnClientClick="RefreshLbl_Click" OnClick="Refresh_Click" Font-Size="Medium" style="Left"/>
    <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="...Running"></asp:Label>

    <div class="under" runat="server">
        <asp:Label ID="Label3" runat="server" Font-Size="Medium"></asp:Label>
        </div>

</asp:Content>
