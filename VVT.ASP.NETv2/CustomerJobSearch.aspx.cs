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
    public partial class CustomerJobSearch : System.Web.UI.Page
    {

        string user = "";
        string pass = "";
        string cust = "";

        DataTable dt = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {

            user = Session["USER"].ToString();
            pass = Session["PASS"].ToString();
            cust = Session["CUST"].ToString();

            //get customer jobs due report specific cust number ^

            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";


            //open th econnection and error check
            OdbcConnection dbConn = new OdbcConnection(connectStr);
            dbConn.ConnectionTimeout = 5000;
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {

                string error = ex + " : DB error cannot connect";

                dbConn.Close();

                ErrorLog(error);

            }

            string queryString = "SELECT Job.\"Job-Id\",  ScheduleByJob.\"TagStatus-ID\", Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", JobFreeField.\"Free-Field-Char\" " +
                
                "FROM PUB.JOB INNER JOIN PUB.ScheduleByJob ON Job.\"Job-id\" = ScheduleByJob.\"Job-ID\" INNER JOIN PUB.JobFreeField ON Job.\"Job-Id\" = JobFreeField.\"Job-Id\" " +
                
                "WHERE \"Cust-ID-Ordered-by\" = \'"+cust+ "\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 1 AND (ScheduleByJob.\"Tag-Complete\" = 0) AND " +
                
                "ScheduleByJob.\"Work-Center-ID\" =\'950\' AND JobFreeField.\"Sequence\" = 13" +
                
                "ORDER BY Job.\"Job-ID\" DESC";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryString, dbConn); //connects to database and passes sql string above to query

            custDTadap.Fill(dt);

            dbConn.Close();


            //change column names
            dt.Columns[0].ColumnName = "Job ID";
            dt.Columns[1].ColumnName = "Job Status";
            dt.Columns[2].ColumnName = "Job Description";
            dt.Columns[3].ColumnName = "Quantity Ordered";
            dt.Columns[4].ColumnName = "Date Ship By";
            dt.Columns[5].ColumnName = "Type of Job";


            //here is where 
            /*
             Job statues-show description
                02-08 - Waiting for All Files
                09-69 - In Progress
                70-88 - Mailing Production
                90 - Complete
                92 Mailed
             */

            foreach (DataRow dr in dt.Rows) {

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 02 && Convert.ToInt32(dr["Job Status"].ToString()) <= 08)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Waiting for All Files";
                    }
                }
                catch (Exception ex) {

                    // string error = "Error: status 02-08: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 09 && Convert.ToInt32(dr["Job Status"].ToString()) <= 69)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - In Progress";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09-69: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 70 && Convert.ToInt32(dr["Job Status"].ToString()) <= 88)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Mailing Production";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 70-88: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Complete";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 90: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 92)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Mailed";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 92: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                //These are statuses that cannot convert to int {50d, 50j, 50e, 97b}
                try
                {
                    if (dr["Job Status"].ToString().ToUpper() == "50D" || dr["Job Status"].ToString().ToUpper() == "50J" || dr["Job Status"].ToString().ToUpper() == "50E")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - In Progress";
                    }
                }
                catch (Exception ex) {

                    // string error = "Error: status 50(Letter): " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString().ToUpper() == "97B")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Completed";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50(Letter): " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


            }//end foreach




            //this will show data from dataTable (dt)
            GridView1.DataSource = dt;

            GridView1.DataBind();
            


        }//end page load


        public void ErrorLog(string logWrite)
        {

            //write to a public txt file (\\visonas\Public\Kyle\errorLog)
            string machineName = Environment.MachineName;

            logWrite += " : on computer name - " + machineName;

            //remove this and just write errors to log file
            //MessageBox.Show("Please copy and paste the next pop-up box and save to txt file");
            //MessageBox.Show(logWrite);

            //real path:                                       \\visonas\public\kyle\vvt releases\log/vvt_log.txt
            using (StreamWriter writeErrors = new StreamWriter(@"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt"))
            {

                writeErrors.WriteLine(logWrite);
            }

        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dt.Columns.Clear();
            GridView1.Columns.Clear();
            GridView1.DataSource = dt;
            //GridView1.DataBind();

            user = Session["USER"].ToString();
            pass = Session["PASS"].ToString();
            cust = Session["CUST"].ToString();

            //get customer jobs due report specific cust number ^

            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";


            //open th econnection and error check
            OdbcConnection dbConn = new OdbcConnection(connectStr);
            dbConn.ConnectionTimeout = 5000;
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {

                string error = ex + " : DB error cannot connect";

                dbConn.Close();

                ErrorLog(error);

            }

            string queryString = "SELECT Job.\"Job-Id\",  ScheduleByJob.\"TagStatus-ID\", Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", JobFreeField.\"Free-Field-Char\" " +

                "FROM PUB.JOB INNER JOIN PUB.ScheduleByJob ON Job.\"Job-id\" = ScheduleByJob.\"Job-ID\" INNER JOIN PUB.JobFreeField ON Job.\"Job-Id\" = JobFreeField.\"Job-Id\" " +

                "WHERE \"Cust-ID-Ordered-by\" = \'" + cust + "\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 1 AND (ScheduleByJob.\"Tag-Complete\" = 0) AND " +

                "ScheduleByJob.\"Work-Center-ID\" =\'950\' AND JobFreeField.\"Sequence\" = 13" +

                "ORDER BY Job.\"Job-ID\" DESC";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryString, dbConn); //connects to database and passes sql string above to query

            custDTadap.Fill(dt);

            dbConn.Close();


            //change column names
            dt.Columns[0].ColumnName = "Job ID";
            dt.Columns[1].ColumnName = "Job Status";
            dt.Columns[2].ColumnName = "Job Description";
            dt.Columns[3].ColumnName = "Quantity Ordered";
            dt.Columns[4].ColumnName = "Date Ship By";
            dt.Columns[5].ColumnName = "Type of Job";


            //here is where 
            /*
             Job statues-show description
                02-08 - Waiting for All Files
                09-69 - In Progress
                70-88 - Mailing Production
                90 - Complete
                92 Mailed
             */

            foreach (DataRow dr in dt.Rows)
            {

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 02 && Convert.ToInt32(dr["Job Status"].ToString()) <= 08)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Waiting for All Files";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 02-08: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 09 && Convert.ToInt32(dr["Job Status"].ToString()) <= 69)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - In Progress";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09-69: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 70 && Convert.ToInt32(dr["Job Status"].ToString()) <= 88)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Mailing Production";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 70-88: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Complete";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 90: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 92)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Mailed";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 92: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                //These are statuses that cannot convert to int {50d, 50j, 50e, 97b}
                try
                {
                    if (dr["Job Status"].ToString().ToUpper() == "50D" || dr["Job Status"].ToString().ToUpper() == "50J" || dr["Job Status"].ToString().ToUpper() == "50E")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - In Progress";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50(Letter): " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString().ToUpper() == "97B")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Completed";
                    }
                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50(Letter): " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


            }//end foreach


            //this will show data from dataTable (dt)
            GridView1.DataSource = dt;

            GridView1.DataBind();

            Label3.Text = "...Done! Last refresh: "+DateTime.Now;
            Label1.Visible = true;
        }//end refresh button

        protected void Export_Click(object sender, EventArgs e)
        {

        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName ="Customer_Jobs.xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType ="application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition","attachment;filename=" + FileName);
        GridView1.GridLines = GridLines.Both;
        GridView1.HeaderStyle.Font.Bold = true;
        GridView1.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());

            Label2.Text = "...Done!" + DateTime.Now + "\nSee downloads folder...";
            Label2.Visible = true;

            Response.End();

            Label2.Text = "...Done!"+ DateTime.Now+"\nSee downloads folder...";
            Label2.Visible = true;

        }//end export to excelbutton

    }//end CustomerJobSearchclass
}//end namespace