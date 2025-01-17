using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VVT.ASP.NETv2
{
    public partial class ShippingCostReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //Generate CR with parameters given by user
        protected void Button1_Click(object sender, EventArgs e)
        {
            shippingcost cryrpt = new shippingcost();

            cryrpt.DataSourceConnections.Clear();  //clear the connections (will popualte with fresh sql query defined data

            //set the databse login info, use twice first one is to login into sql server

            //getting write only error


            cryrpt.SetDatabaseLogon("Bob", "Orchard", "monarch18", "gams1");
            cryrpt.SetDatabaseLogon("Bob", "Orchard"); //this one for that annoying prompt to login into database


            //this does not error out!?
            //this opens connection to DB with the login info down..
            ConnectionInfo crconnectioninfo = new ConnectionInfo();
            crconnectioninfo.ServerName = "monarch18";
            crconnectioninfo.DatabaseName = "gams1";
            crconnectioninfo.UserID = "Bob";
            crconnectioninfo.Password = "Orchard";

            DateTime date = DateTime.Parse(TextBox2.Text);

            //params
            cryrpt.SetParameterValue("Customer Number", TextBox1.Text); // Replace with actual parameter name and value
            cryrpt.SetParameterValue("Ending Bill Date", date);      // You can pass integers, strings, etc.
            cryrpt.SetParameterValue("Status", 97);

            //uneeded
            //cryrpt.SetParameterValue("System-ID", "Viso");
            cryrpt.SetParameterValue("Expense Code", "Include");


            CrystalReportViewer1.ReportSource = cryrpt;
            CrystalReportViewer1.RefreshReport();


            cryrpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Job and Shipping Cost Report");

        }

        //eport data generated in crystal report but in better format
        protected void Button2_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DateTime date = DateTime.Parse(TextBox2.Text);
            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";

            // open the connection and error check
            OdbcConnection dbConn = new OdbcConnection(connectStr);
            dbConn.ConnectionTimeout = 0; //0 is infinity
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {

                string error = ex + " : DB error cannot connect";

                dbConn.Close();

             

            }

            string queryFF = "SELECT Job.\"Date-Entered\", Job.\"Contact-Name\", Job.\"External-Order-Number\", Job.\"Job-ID\", Job.\"Quantity-Ordered\", Job.\"Job-Desc\", Job.\"Quotation-Amount\", " +
               "COALESCE(SUM(JobShipTo.\"Billable-Freight\"), 0) AS \"Ship Cost\" " +
               "FROM PUB.cust AS cust " +

              "INNER JOIN PUB.Job AS Job " +
              "ON cust.\"Cust-code\" = Job.\"Cust-ID-Ordered-by\" " +

              "LEFT JOIN PUB.JobShipTo AS JobShipTo " +

             "ON Job.\"Job-ID\" = JobShipTo.\"Job-ID\" " +
             "INNER JOIN PUB.ScheduleByJob AS ScheduleByJob " +
             "ON Job.\"Job-ID\" = ScheduleByJob.\"Job-ID\" " +
             "WHERE cust.\"Cust-code\" = "+TextBox1.Text+" AND " +
             "ScheduleByJob.\"TagStatus-ID\" = 97 AND " +
             "Job.\"Date-Entered\" <= \'" +date +"\' "+
             "GROUP BY " +
             "Job.\"Date-Entered\", Job.\"Contact-Name\", Job.\"External-Order-Number\", Job.\"Job-ID\", Job.\"Quantity-Ordered\", Job.\"Job-Desc\", Job.\"Quotation-Amount\"";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryFF, dbConn); //connects to database and passes sql string above to query

            custDTadap.Fill(dt);

            dbConn.Close();

            ExportToExcel(dt);

        }

        protected void ExportToExcel(DataTable dt)
        {
            // Set the response header to prompt the user to download an Excel file
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=ExportedData.xls");
            Response.Charset = "";

            // Create a StringWriter to hold the HTML table
            using (StringWriter sw = new StringWriter())
            {
                // Create an HtmlTextWriter that can render the HTML
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // Create a GridView to render the DataTable as an HTML table
                    GridView gridView = new GridView();
                    gridView.DataSource = dt;
                    gridView.DataBind();

                    // Render the GridView to the HtmlTextWriter
                    gridView.RenderControl(hw);

                    // Write the content to the response
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
        }
    }
}