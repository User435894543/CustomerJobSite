<%@ Page Title="Home Page" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VVT.ASP.NETv2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <h1>&nbsp;</h1>
        <p class="lead" style="font-size: 50px; color: #229F7A;">VISOgraphic Inc.</p>
        <p class="lead" style="font-size: 35px; color: #229F7A;">Customer Job Portal</p>
    </div>

            <div>
            <label>Username:</label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
            <label>Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" OnClientClick="btnLogin_Click1"/>
        </div>
            <asp:Label ID="Label1" runat="server"></asp:Label>

        <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.0.min.js"></script>
<script  type="text/javascript">

    $(document).ready(function () {
        // Hide the Label to start with
        $('#<%=Label2.ClientID %>').hide();

            $('#<%=btnLogin.ClientID %>').click(function ()
            {
                // Show the Label when save button is clicked
               // $('#<%=Label2.Text%>') = "aaa"; - i have no clue how to get this without first hide and show ^ value set to label1 above
                $('#<%=Label2.ClientID %>').show();
            });
        });

</script>
    <br>
            <asp:Label ID="Label2" Text="Loading please wait..." runat="server"></asp:Label>
    </br>
        </asp:Content>
