using iTextSharp.text;
using iTextSharp.text.pdf;
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
    public partial class CostReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //generate report
        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DateTime endDate = DateTime.Parse(TextBox2.Text);
            DateTime startDate = DateTime.Parse(TextBox3.Text);
            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";

            OdbcConnection dbConn = new OdbcConnection(connectStr);
            dbConn.ConnectionTimeout = 0;
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {
                string error = ex + " : DB error cannot connect";
                dbConn.Close();
            }

            string queryFF = "SELECT DISTINCT Job.\"Date-Entered\", JobShipTo.\"Actual-Ship-Date\", Job.\"Contact-Name\", Job.\"Job-ID\", Job.\"External-Order-Number\", Job.\"Quantity-Ordered\", Job.\"Job-Desc\", Job.\"Quotation-Amount\", " +
                "JobShipTo.\"Billable-Freight\", JobShipTo.\"Waybill-Number\" " +
                "FROM PUB.cust AS cust " +
                "INNER JOIN PUB.Job AS Job " +
                "ON cust.\"Cust-code\" = Job.\"Cust-ID-Ordered-by\" " +
                "LEFT JOIN PUB.JobShipTo AS JobShipTo " +
                "ON Job.\"Job-ID\" = JobShipTo.\"Job-ID\" " +
                "INNER JOIN PUB.ScheduleByJob AS ScheduleByJob " +
                "ON Job.\"Job-ID\" = ScheduleByJob.\"Job-ID\" " +
                "WHERE cust.\"Cust-code\" = " + TextBox1.Text + " AND " +
                "Job.\"Date-Entered\" >= '" + startDate + "' AND " +
                "Job.\"Date-Entered\" <= '" + endDate + "' AND " +
                "ScheduleByJob.\"TagStatus-ID\" = 97 AND " +
                "Job.\"Job-Open\" = 1 AND " +
                "ScheduleByJob.\"Tag-Complete\" = 0 AND " +
                "ScheduleByJob.\"TagStatus-ID\" = 97 AND " +
                "ScheduleByJob.\"System-ID\" = 'Viso' AND " +
                "cust.\"System-ID\" = 'Viso' ";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryFF, dbConn);
            custDTadap.Fill(dt);
            dbConn.Close();

            DataTable finaldt = new DataTable();
            finaldt.Columns.Add("Count", typeof(int));
            finaldt.Columns.Add("Date", typeof(string));
            finaldt.Columns.Add("Ship Date", typeof(string));
            finaldt.Columns.Add("Name", typeof(string));
            finaldt.Columns.Add("Status", typeof(string));
            finaldt.Columns.Add("Viso Job ID", typeof(string));
            finaldt.Columns.Add("Order ID", typeof(string));
            finaldt.Columns.Add("Quantity", typeof(int));
            finaldt.Columns.Add("Item SKU #", typeof(string));
            finaldt.Columns.Add("Pick/Pack Fee", typeof(string));
            finaldt.Columns.Add("Shipment Cost", typeof(string));
            finaldt.Columns.Add("Tracking #", typeof(string));
            finaldt.Columns.Add("Shipping Totals", typeof(string));

            Dictionary<string, List<decimal>> orderShippingCosts = new Dictionary<string, List<decimal>>();
            Dictionary<string, DataRow> lastRowForOrder = new Dictionary<string, DataRow>();
            int rowCount = 1;

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    DataRow newRow = finaldt.NewRow();
                    newRow["Count"] = rowCount;
                    newRow["Date"] = Convert.ToDateTime(row["Date-Entered"]).ToString("MM/dd/yyyy");
                    newRow["Ship Date"] = Convert.ToDateTime(row["Actual-Ship-Date"]).ToString("MM/dd/yyyy");
                    newRow["Name"] = row["Contact-Name"];
                    newRow["Status"] = "Shipped";
                    newRow["Viso Job ID"] = row["Job-ID"];
                    newRow["Order ID"] = row["External-Order-Number"];
                    newRow["Quantity"] = row["Quantity-Ordered"];
                    newRow["Item SKU #"] = row["Job-Desc"];

                    decimal shipmentCost = Convert.ToDecimal(row["Billable-Freight"]);
                    newRow["Pick/Pack Fee"] = "$" + row["Quotation-Amount"].ToString();
                    newRow["Shipment Cost"] = "$" + shipmentCost.ToString("F2");
                    newRow["Tracking #"] = row["Waybill-Number"];

                    string orderId = row["External-Order-Number"].ToString();
                    if (!orderShippingCosts.ContainsKey(orderId))
                    {
                        orderShippingCosts[orderId] = new List<decimal>();
                    }
                    orderShippingCosts[orderId].Add(shipmentCost);

                    finaldt.Rows.Add(newRow);
                    lastRowForOrder[orderId] = newRow;
                    rowCount++;
                }
                catch (Exception ex) { }
            }

            foreach (var orderId in lastRowForOrder.Keys)
            {
                if (orderShippingCosts[orderId].Count > 1)
                {
                    lastRowForOrder[orderId]["Shipping Totals"] = "$" + orderShippingCosts[orderId].Sum().ToString("F2");
                }
            }

            // Calculate Totals
            int totalQuantity = finaldt.AsEnumerable().Sum(row => row.Field<int>("Quantity"));
            decimal totalShippingCost = finaldt.AsEnumerable()
                .Where(row => row["Shipping Totals"] != DBNull.Value && row["Shipping Totals"].ToString() != "")
                .Sum(row => decimal.Parse(row["Shipping Totals"].ToString().Replace("$", "")));

            decimal totalPickPackFee = finaldt.AsEnumerable()
                .Where(row => row["Pick/Pack Fee"] != DBNull.Value && row["Pick/Pack Fee"].ToString() != "")
                .Sum(row => decimal.Parse(row["Pick/Pack Fee"].ToString().Replace("$", "")));

            // Insert Total Row
            DataRow totalRow = finaldt.NewRow();
            totalRow["Item SKU #"] = "Totals:";
            totalRow["Pick/Pack Fee"] = "$" + totalPickPackFee.ToString("F2");
            totalRow["Shipping Totals"] = "$" + totalShippingCost.ToString("F2");

            // Add Total Row to DataTable
            finaldt.Rows.Add(totalRow);

            // Bind to GridView
            GridView1.DataSource = finaldt;
            GridView1.DataBind();


        }

        //export to pdf
        protected void Button3_Click(object sender, EventArgs e)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                document.Add(new Paragraph("Job and Shipping Cost Report"));
                document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                document.Add(new Paragraph("\n"));

                int columnCount = GridView1.Columns.Count;
                PdfPTable table = new PdfPTable(columnCount);
                table.WidthPercentage = 100;

                // Define column widths, making 'Tracking #' and 'Item SKU #' wider
                float[] columnWidths = new float[columnCount];
                int trackingColumnIndex = -1;
                int skuColumnIndex = -1;
                //int dateColIndex = -1;
                //int nameColIndex = -1;

                for (int i = 0; i < columnCount; i++)
                {
                    string headerText = GridView1.Columns[i].HeaderText;

                    if (headerText == "Tracking #")
                    {
                        trackingColumnIndex = i;
                        columnWidths[i] = 4.8f; // Wider for 'Tracking #'
                    }
                    else if (headerText == "Item SKU #")
                    {
                        skuColumnIndex = i;
                        columnWidths[i] = 4f; // Wider for 'Item SKU #'
                    }
                    else
                    {
                        columnWidths[i] = 1.9f; // Default width for other columns
                    }
                }

                // Apply column widths if needed
                if (trackingColumnIndex != -1 || skuColumnIndex != -1)
                {
                    table.SetWidths(columnWidths);
                }

                // Colors for alternating columns
                BaseColor column1Color = new BaseColor(240, 240, 240);
                BaseColor column2Color = new BaseColor(255, 255, 255);

                // Add header row with alternating background color
                for (int i = 0; i < columnCount; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(GridView1.Columns[i].HeaderText));
                    cell.BackgroundColor = i % 2 == 0 ? column1Color : column2Color;
                    table.AddCell(cell);
                }

                // Add data rows
                foreach (GridViewRow row in GridView1.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        string cellText = row.Cells[i].Text.Trim();
                        if (string.IsNullOrEmpty(cellText) || cellText == "&nbsp;")
                        {
                            cellText = "";
                        }

                        PdfPCell dataCell = new PdfPCell(new Phrase(cellText));
                        dataCell.BackgroundColor = i % 2 == 0 ? column1Color : column2Color;
                        table.AddCell(dataCell);
                    }
                }

                document.Add(table);
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CostReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }

        }


        //export excel data
        protected void Button2_Click(object sender, EventArgs e)
        {
            #region old query just take from GridView1
            //DataTable dt = new DataTable();
            //DateTime endDate = DateTime.Parse(TextBox2.Text);
            //DateTime startDate = DateTime.Parse(TextBox3.Text);
            //string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";

            //// open the connection and error check
            //OdbcConnection dbConn = new OdbcConnection(connectStr);
            //dbConn.ConnectionTimeout = 0; //0 is infinity
            //try
            //{
            //    dbConn.Open();
            //}
            //catch (Exception ex)
            //{

            //    string error = ex + " : DB error cannot connect";

            //    dbConn.Close();



            //}


            //string queryFF = "SELECT Job.\"Date-Entered\", Job.\"Contact-Name\", Job.\"External-Order-Number\", Job.\"Quantity-Ordered\", Job.\"Job-Desc\", Job.\"Quotation-Amount\", " +
            //    "JobShipTo.\"Billable-Freight\", JobShipTo.\"Waybill-Number\" " +
            //    "FROM PUB.cust AS cust " +

            //   "INNER JOIN PUB.Job AS Job " +
            //   "ON cust.\"Cust-code\" = Job.\"Cust-ID-Ordered-by\" " +

            //   "LEFT JOIN PUB.JobShipTo AS JobShipTo " +

            //  "ON Job.\"Job-ID\" = JobShipTo.\"Job-ID\" " +
            //  "INNER JOIN PUB.ScheduleByJob AS ScheduleByJob " +
            //  "ON Job.\"Job-ID\" = ScheduleByJob.\"Job-ID\" " +
            //  "WHERE cust.\"Cust-code\" = " + TextBox1.Text + " AND " +
            //  "Job.\"Date-Entered\" >= \'" + startDate + "\' AND " +
            //  "Job.\"Date-Entered\" <= \'" + endDate + "\' AND " +
            //  "ScheduleByJob.\"TagStatus-ID\" = 97 AND " +
            //  "Job.\"Job-Open\" = 1 AND " +
            //  "ScheduleByJob.\"Tag-Complete\" = 0 AND " +
            //  "ScheduleByJob.\"TagStatus-ID\" = 97 AND " +
            //  "ScheduleByJob.\"System-ID\" = \'Viso\' AND " +
            //  "cust.\"System-ID\" = \'Viso\' ";// +
            //                                   //"GROUP BY " +
            //                                   //"Job.\"Date-Entered\", Job.\"Contact-Name\", Job.\"External-Order-Number\", Job.\"Job-ID\", Job.\"Quantity-Ordered\", Job.\"Job-Desc\", Job.\"Quotation-Amount\", JobShipTo.\"Waybill-Number\"";

            //OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryFF, dbConn); //connects to database and passes sql string above to query

            //custDTadap.Fill(dt);

            //dbConn.Close();

            //DataTable finaldt = new DataTable();

            //// Define the schema for finaldt (columns).
            //finaldt.Columns.Add("Count", typeof(int));
            //finaldt.Columns.Add("Date", typeof(string)); // Use string if dates are formatted
            //finaldt.Columns.Add("Name", typeof(string));
            //finaldt.Columns.Add("Status", typeof(string));
            //finaldt.Columns.Add("Order ID", typeof(string));
            //finaldt.Columns.Add("Quantity", typeof(int));
            //finaldt.Columns.Add("Item SKU #", typeof(string));
            //finaldt.Columns.Add("Pick/Pack Fee", typeof(string)); // Assuming it's monetary
            //finaldt.Columns.Add("Shipment Cost", typeof(string)); // Assuming it's monetary
            //finaldt.Columns.Add("Tracking #", typeof(string));

            //// Initialize variables for running totals.
            //decimal totalPickPackFee = 0;
            //decimal totalShipmentCost = 0;

            //// Add row numbers sequentially and copy data from dt to finaldt.
            //int rowCount = 1;
            //foreach (DataRow row in dt.Rows)
            //{
            //    DataRow newRow = finaldt.NewRow();
            //    newRow["Count"] = rowCount;
            //    newRow["Date"] = Convert.ToDateTime(row["Date-Entered"]).ToString("MM/dd/yyyy");
            //    newRow["Name"] = row["Contact-Name"];
            //    newRow["Status"] = "Shipped";
            //    newRow["Order ID"] = row["External-Order-Number"];
            //    newRow["Quantity"] = row["Quantity-Ordered"];
            //    newRow["Item SKU #"] = row["Job-Desc"];

            //    // Parse and format monetary values.
            //    decimal pickPackFee = Convert.ToDecimal(row["Quotation-Amount"]);
            //    decimal shipmentCost = Convert.ToDecimal(row["Billable-Freight"]);

            //    totalPickPackFee += pickPackFee;
            //    totalShipmentCost += shipmentCost;

            //    newRow["Pick/Pack Fee"] = "$" + pickPackFee.ToString("F2");
            //    newRow["Shipment Cost"] = "$" + shipmentCost.ToString("F2");
            //    newRow["Tracking #"] = row["Waybill-Number"];

            //    finaldt.Rows.Add(newRow);
            //    rowCount++;
            //}

            //// Add a totals row to the last row.
            //DataRow totalsRow = finaldt.NewRow();
            //totalsRow["Count"] = DBNull.Value; // No count for the totals row.
            //totalsRow["Date"] = DBNull.Value;
            //totalsRow["Name"] = DBNull.Value;
            //totalsRow["Status"] = DBNull.Value;
            //totalsRow["Order ID"] = DBNull.Value;
            //totalsRow["Quantity"] = DBNull.Value;
            //totalsRow["Item SKU #"] = "Total";
            //totalsRow["Pick/Pack Fee"] = "$" + totalPickPackFee.ToString("F2");
            //totalsRow["Shipment Cost"] = "$" + totalShipmentCost.ToString("F2");
            //totalsRow["Tracking #"] = DBNull.Value;

            //// Add the totals row to finaldt.
            //finaldt.Rows.Add(totalsRow);
            #endregion


            DataTable finaldt = ConvertGridViewToDataTable(GridView1);

            ExportToExcel(finaldt);

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ExportToExcel(DataTable dt)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=ExportedData.xls");
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView gridView = new GridView();
                    gridView.DataSource = dt;
                    gridView.DataBind();

                    // Modify specific columns
                    foreach (GridViewRow row in gridView.Rows)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            TableCell cell = row.Cells[i];

                            // Remove &nbsp;
                            if (cell.Text == "&nbsp;")
                            {
                                cell.Text = "";
                            }

                            // Set "Item SKU #" column width
                            if (gridView.HeaderRow.Cells[i].Text == "Item SKU #")
                            {
                                cell.Attributes.Add("style", "width:400px;"); // Adjust width
                            }

                            // Format "Tracking #" as text
                            if (gridView.HeaderRow.Cells[i].Text == "Tracking #")
                            {
                                cell.Text = $"=\"{cell.Text}\""; // Forces Excel to interpret as text
                                cell.Attributes.Add("style", "width:200px;"); // Adjust width
                            }
                        }
                    }

                    gridView.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
        }




        public static DataTable ConvertGridViewToDataTable(GridView gridView)
        {
            // Create a new DataTable
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable from the GridView's header row
            foreach (TableCell headerCell in gridView.HeaderRow.Cells)
            {
                dataTable.Columns.Add(headerCell.Text);
            }

            // Add rows to the DataTable from the GridView's data rows
            foreach (GridViewRow row in gridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dataRow[i] = row.Cells[i].Text.Trim(); // Optionally, handle formatting or stripping HTML
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

    }

}