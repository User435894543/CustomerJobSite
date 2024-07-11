<%@ Page Title="Customer Jobs" MasterPageFile="/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="CustomerJobSearch.aspx.cs" Inherits="VVT.ASP.NETv2.CustomerJobSearch" %>

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
.label-style {
    font-weight: bold;
    margin-right: 5px; /* Adjust spacing between label and GridView */
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

        .gridview-style {
    width: 100%; /* Adjust the width as per your layout */
    border-collapse: collapse; /* Optional: For border styling */
}
</style>


<body>


    <!--adding legend for statuses ex) 02, 05, etc-->

<div style="display: flex; align-items: flex-start; border-style: none; background-color:white; border-color:white; border-width: 0px">

            <asp:Label ID="legend" runat="server"  CssClass="label-style" Font-Size="Medium" Text ="Status Legend:<br/>02 - waiting on art and/or data<br/>05 - print ready - need data<br/>09 - in prepress<br/>09r - revision needed<br/>18 - out on proof<br/>50/50d/50e - printing<br/>70 - bindery<br/>80 - ready for mailing<br/>88 - mail complete<br/>90 - job complete ready to mail<br/>92 - being delivered<br/>95 - complete"></asp:Label>
       

        <asp:datagrid id="GridView1" runat="server" CssClass="gridview-style" GridLines="Vertical" CellPadding="3"
BorderColor="#999999" BorderWidth="1px" BorderStyle="None" Width="100%" Height="100%" Font-Size="11"
Font-Names="Verdana" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateColumns="False">
<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
<ItemStyle BorderWidth="2px" ForeColor="Black" BorderStyle="Solid" BorderColor="Black" BackColor="#EEEEEE"></ItemStyle>
<HeaderStyle Font-Bold="True" HorizontalAlign="center" ForeColor="White"
BorderColor="Black" BackColor="#007A5D"></HeaderStyle>
<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>


             <Columns>

 
                 <asp:BoundColumn DataField="Job ID" HeaderText="Job ID" />
                 <asp:BoundColumn DataField="Job Status" HeaderText="Job Status" />
                 <asp:BoundColumn DataField="Job Description" HeaderText="Job Description" />
                 <asp:BoundColumn DataField="Quantity" HeaderText="Quantity" />
                 <asp:BoundColumn DataField="Date Ship BY" HeaderText="Date Ship By" ItemStyle-Width="10%"/>
                 <asp:BoundColumn DataField="Postage Class" HeaderText="Postage Class" />
                 <asp:BoundColumn DataField="Postage for Stamps" HeaderText="Postage for Stamps" />
                 <asp:BoundColumn DataField="AC Rep" HeaderText="AC Rep" />

            </Columns>


</asp:datagrid>
    </div>
        

    <div class="left" runat="server">
        </div>
</body>
</html>

    <br>
     <div class="center" runat="server">
                <asp:Button ID="Export" runat="server" Text="Export To Excel" OnClick="Export_Click" OnClientClick="ExporthLbl_Click"  Font-Size="Medium"/>
         <asp:Label ID="Label4" runat="server" Font-Size="Medium"></asp:Label>
         <asp:Label ID="Label5" runat="server" Font-Size="Medium"></asp:Label>
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
    </br>

    <asp:CheckBox ID="CheckBox1" runat="server" Text ="See Closed Jobs Only&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Visible="False"/>
    <asp:CheckBox ID="CheckBox2" runat="server" Text ="See Closed Jobs at the end of Open Jobs" Visible="False"/>

</asp:Content>
