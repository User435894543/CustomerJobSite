<%@ Page Title="Job And Shipping Cost Report" MasterPageFile="/Site.Master" AutoEventWireup="true" CodeBehind="CostReport.aspx.cs" Inherits="VVT.ASP.NETv2.CostReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
         <p class="lead" style="font-size: 50px; color: #229F7A;">Job and Shipping Cost Report</p>
        <p class="lead instructions">
            1 Enter Customer Number <br />
2 Enter Start and End Dates <br />
4 Click "Generate" <br />
5 Click "Download PDF" or "Download Excel" <br />
        </p>
    </div>

    <div class="form-group">
        <asp:Label ID="Label1" runat="server" Text="Customer Number:" AssociatedControlID="TextBox1"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label ID="Label3" runat="server" Text="Start Date:" AssociatedControlID="TextBox3"></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label ID="Label2" runat="server" Text="End Date:" AssociatedControlID="TextBox2"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="button-group">
        <asp:Button ID="Button1" runat="server" Text="Generate" OnClick="Button1_Click" CssClass="btn btn-primary" />
        <asp:Button ID="Button3" runat="server" Text="Download PDF" OnClick="Button3_Click" CssClass="btn btn-secondary" />
        <asp:Button ID="Button2" runat="server" Text="Download Excel" OnClick="Button2_Click" CssClass="btn btn-success" />
    </div>

    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="Count" HeaderText="Count" />
            <asp:BoundField DataField="Date" HeaderText="Date" />
            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="25%"/>
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Order ID" HeaderText="Order ID" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:BoundField DataField="Item SKU #" HeaderText="Item SKU #" ItemStyle-Width="80%"/>
            <asp:BoundField DataField="Pick/Pack Fee" HeaderText="Pick/Pack Fee" />
            <asp:BoundField DataField="Shipment Cost" HeaderText="Shipment Cost" />
            <asp:BoundField DataField="Tracking #" HeaderText="Tracking #" />
        </Columns>
    </asp:GridView>
</asp:Content>
