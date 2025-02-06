
<%@ Page Title="Job And Shipping Cost Report" MasterPageFile="/Site.Master" AutoEventWireup="true" CodeBehind="CostReport.aspx.cs" Inherits="VVT.ASP.NETv2.ShippingCostReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<!DOCTYPE html>

<html>

<body>
    <form id="form1">
    


                 <div class="jumbotron">
        <h1>&nbsp;</h1>
        <p class="lead" style="font-size: 50px; color: #229F7A;">Job and Shipping Cost Report</p>
</div>

        <strong>
          How to use: Please type in your customer number and pick an end date. <br />
            Generate button will generate the report. Export button will export data to excel.<br />
            </strong>

            <br />
         <br />
         <br />


        <asp:Label ID="Label1" runat="server" Text="Customer Number:"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

        <br />

         <asp:Label ID="Label2" runat="server" Text="End Date:"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" TextMode="Date"></asp:TextBox>

        <br />
        <asp:Button ID="Button1" runat="server" Text="Generate" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="Export" OnClick="Button2_Click" />
                 <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    </form>
</body>
</html>
         </asp:Content>