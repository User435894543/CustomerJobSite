<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VVT.ASP.NETv2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <h1>&nbsp;</h1>
        <p class="lead" style="font-size: 50px; color: #229F7A;">VISOgraphic Inc.</p>
        <p class="lead" style="font-size: 35px; color: #229F7A;">Customer Job Search</p>
    </div>

            <div>
            <label>Username:</label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
            <label>Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" OnClientClick="btnLogin_Click1"/>
        </div>
            <asp:Label ID="Label1" runat="server"></asp:Label>
</asp:Content>
