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

            string queryString = "SELECT Job.\"Job-Id\",  ScheduleByJob.\"TagStatus-ID\", Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", MailingVersionFreeField.\"Free-Field-Char\", " +

                "Job.\"PO-Number\" FROM PUB.JOB INNER JOIN PUB.ScheduleByJob ON Job.\"Job-id\" = ScheduleByJob.\"Job-ID\" INNER JOIN PUB.MailingVersionFreeField ON Job.\"Job-Id\" = MailingVersionFreeField.\"Job-Id\" " +
                
                "WHERE \"Cust-ID-Ordered-by\" = \'"+cust+ "\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 1 AND (ScheduleByJob.\"Tag-Complete\" = 0) AND " +

                "ScheduleByJob.\"Work-Center-ID\" =\'900\' AND MailingVersionFreeField.\"Sequence\"= 5" +

                "ORDER BY ScheduleByJob.\"TagStatus-ID\"";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryString, dbConn); //connects to database and passes sql string above to query

            custDTadap.Fill(dt);

            dbConn.Close();


            //change column names
            dt.Columns[0].ColumnName = "Job ID";
            dt.Columns[1].ColumnName = "Job Status";
            dt.Columns[2].ColumnName = "Job Description";
            dt.Columns[3].ColumnName = "Quantity";
            dt.Columns[4].ColumnName = "Date Ship By Bad";
            dt.Columns[5].ColumnName = "Postage Class";
            dt.Columns[6].ColumnName = "AC Rep";

            //lol no way to format date ship by what a joke
            dt.Columns.Add("Date Ship By");

            //postage cost (calcualted field)
            dt.Columns.Add("Postage for Stamps");

    


            foreach (DataRow dr in dt.Rows) {

                #region Job status in easy read forms
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
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 09 && Convert.ToInt32(dr["Job Status"].ToString()) <= 79)
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
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 80 && Convert.ToInt32(dr["Job Status"].ToString()) <= 88)
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

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 95)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Delivered";
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
                #endregion

                #region Date Ship By Formatting (MM/dd/yyy)
                //06.20 format date no time stamp just date (mm/dd/yyy)
                try
                {

                    DateTime dateTime = new DateTime();

                    dateTime = DateTime.Parse(dr["Date Ship By Bad"].ToString());

                    string formattedDate = dateTime.ToString("MM-dd-yyyy");

                    dr["Date Ship By"] = formattedDate;

                }
                catch (Exception ex)
                {

                    // string error = "Error: Format Job Ship By " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }
                #endregion

                #region Job Description Format to 20 chars
                //06.20.2024 - shorten description field to 20 charectors
                try
                {

                    dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                }
                catch (Exception ex)
                {

                    // string error = "Error: Description shorten to 20 chars: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }
                #endregion

                #region Mark to bill stamp records if sequence = 1 equals PC Stamp will need loop

                dbConn.Open();
                DataTable dtStampBillCheck = new DataTable();

                string queryToMarkStampBill = "Select MailingVersionFreeField.\"Free-Field-Char\" FROM PUB.MailingVersionFreeField WHERE \"Job-Id\" = " + dr["Job ID"].ToString() + " AND \"Sequence\"=1";
                OdbcDataAdapter querySeq1 = new OdbcDataAdapter(queryToMarkStampBill, dbConn); //connects to database and passes sql string above to query

                querySeq1.Fill(dtStampBillCheck);

                dbConn.Close();

                string pcStampStr = dtStampBillCheck.Rows[0][0].ToString();
                bool billIt = false;

                //now mark seq5 (append Y/N) in column "Postage Class" { if dtStampBillCheck \"Free-Field-Char\" = PC Stamp }
                if (pcStampStr == "PC Stamp" || pcStampStr.ToUpper() == "PC Stamp" || pcStampStr.ToLower() == "PC Stamp" || pcStampStr.Contains("PC Stamp")) {

                    billIt = true;

                }
               

                #endregion

                #region Get postage for Stamps (Stamp jobs only)
                //calculate postage amount
                /*
                 * Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                 * Postage class = Standard Presort then qty ordered x .10 = postage amount
                 * 
                 */

                string aaa = dr["Postage Class"].ToString();

                if ( billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity"].ToString());

                    double cost = qty * .25;

                    dr["Postage for Stamps"] = "$" + cost.ToString("F");
                }

                else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity"].ToString());

                    double cost = qty * .10;

                    dr["Postage for Stamps"] = "$" + cost.ToString("F");
                }//end else if can put more else if here
                
                else { //this will execute if all false^ 

                    dr["Postage for Stamps"] = "$0.00";

                }
                #endregion



            }//end foreach



            //drop unformatted column and set ordinal so columns back in order lol
            dt.Columns.Remove("Date Ship By Bad");

            dt.Columns["Job ID"].SetOrdinal(0);
            dt.Columns["Job Status"].SetOrdinal(1);
            dt.Columns["Job Description"].SetOrdinal(2);
            dt.Columns["Quantity"].SetOrdinal(3);
            dt.Columns["Date Ship By"].SetOrdinal(4);
            dt.Columns["Postage Class"].SetOrdinal(5);
            dt.Columns["Postage for Stamps"].SetOrdinal(6);
            dt.Columns["AC Rep"].SetOrdinal(7);

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


            string queryString = "SELECT Job.\"Job-Id\",  ScheduleByJob.\"TagStatus-ID\", Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", MailingVersionFreeField.\"Free-Field-Char\", " +

                       "Job.\"PO-Number\" FROM PUB.JOB INNER JOIN PUB.ScheduleByJob ON Job.\"Job-id\" = ScheduleByJob.\"Job-ID\" INNER JOIN PUB.MailingVersionFreeField ON Job.\"Job-Id\" = MailingVersionFreeField.\"Job-Id\" " +

                       "WHERE \"Cust-ID-Ordered-by\" = \'" + cust + "\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 1 AND (ScheduleByJob.\"Tag-Complete\" = 0) AND " +

                       "ScheduleByJob.\"Work-Center-ID\" =\'900\' AND MailingVersionFreeField.\"Sequence\"= 5" +

                       "ORDER BY ScheduleByJob.\"TagStatus-ID\"";

            OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryString, dbConn); //connects to database and passes sql string above to query

            custDTadap.Fill(dt);

            dbConn.Close();


            //change column names
            dt.Columns[0].ColumnName = "Job ID";
            dt.Columns[1].ColumnName = "Job Status";
            dt.Columns[2].ColumnName = "Job Description";
            dt.Columns[3].ColumnName = "Quantity";
            dt.Columns[4].ColumnName = "Date Ship By Bad";
            dt.Columns[5].ColumnName = "Postage Class";
            dt.Columns[6].ColumnName = "AC Rep";


            //lol no way to format date ship by what a joke
            dt.Columns.Add("Date Ship By");

            //postage cost (calcualted field)
            dt.Columns.Add("Postage for Stamps");

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
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 09 && Convert.ToInt32(dr["Job Status"].ToString()) <= 79)
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
                    if (Convert.ToInt32(dr["Job Status"].ToString()) >= 80 && Convert.ToInt32(dr["Job Status"].ToString()) <= 88)
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

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 95)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " - Delivered";
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


                //06.20 format date no time stamp just date (mm/dd/yyy)
                try
                {

                    DateTime dateTime = new DateTime();

                    dateTime = DateTime.Parse(dr["Date Ship By Bad"].ToString());

                    string formattedDate = dateTime.ToString("MM-dd-yyyy");

                    dr["Date Ship By"] = formattedDate;

                }
                catch (Exception ex)
                {

                    // string error = "Error: Format Job Ship By " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                //06.20.2024 - shorten description field to 20 charectors
                try
                {

                    dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                }
                catch (Exception ex)
                {

                    // string error = "Error: Description shorten to 20 chars: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                #region Get postage for Stamps (Stamp jobs only)
                //calculate postage amount
                /*
                 * Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                 * Postage class = Standard Presort then qty ordered x .10 = postage amount
                 * 
                 */

                string aaa = dr["Postage Class"].ToString();

                if (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort"))
                {

                    int qty = Convert.ToInt32(dr["Quantity"].ToString());

                    double cost = qty * .25;

                    dr["Postage for Stamps"] = "$" + cost.ToString("F");
                }

                if (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort"))
                {

                    int qty = Convert.ToInt32(dr["Quantity"].ToString());

                    double cost = qty * .10;

                    dr["Postage for Stamps"] = "$" + cost.ToString("F");
                }
                #endregion

            }//end foreach



            //drop unformatted column and set ordinal so columns back in order lol
            dt.Columns.Remove("Date Ship By Bad");

            dt.Columns["Job ID"].SetOrdinal(0);
            dt.Columns["Job Status"].SetOrdinal(1);
            dt.Columns["Job Description"].SetOrdinal(2);
            dt.Columns["Quantity"].SetOrdinal(3);
            dt.Columns["Date Ship By"].SetOrdinal(4);
            dt.Columns["Postage Class"].SetOrdinal(5);
            dt.Columns["Postage for Stamps"].SetOrdinal(6);
            dt.Columns["AC Rep"].SetOrdinal(7);


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