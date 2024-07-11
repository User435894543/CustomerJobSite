using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
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

            GridView1.AutoGenerateColumns = false;

            user = Session["USER"].ToString();
            pass = Session["PASS"].ToString();
            cust = Session["CUST"].ToString();


            // comment all out for testing start


            //get customer jobs due report specific cust number ^

            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";


            //open th econnection and error check
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
            dt.Columns[3].ColumnName = "Quantity Bad";
            dt.Columns[4].ColumnName = "Date Ship By Bad";
            dt.Columns[5].ColumnName = "Postage Class";
            dt.Columns[6].ColumnName = "AC Rep";

            //lol no way to format date ship by what a joke
            //same with quantity is because data type coming from sql query^ is immutable
            dt.Columns.Add("Date Ship By");
            dt.Columns.Add("Quantity");

            //postage cost (calcualted field)
            dt.Columns.Add("Postage for Stamps");




            foreach (DataRow dr in dt.Rows) {

                #region Job status in easy read forms

                //07.01 update
                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 02)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " waiting on art and/or data";
                    }

                }
                catch (Exception ex) {

                    // string error = "Error: status 02: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 05)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " print ready – need data";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 05: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 09)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " in prepress";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "09r" || dr["Job Status"].ToString().ToUpper() == "09R" || dr["Job Status"].ToString().ToLower() == "09r")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " revision needed";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09r/09R: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 18)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " out on proof";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 18: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "50" || dr["Job Status"].ToString() == "50d" || dr["Job Status"].ToString().ToUpper() == "50d" || dr["Job Status"].ToString().ToLower() == "50d" || dr["Job Status"].ToString() == "50j" || dr["Job Status"].ToString().ToUpper() == "50j" || dr["Job Status"].ToString().ToLower() == "50j")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " printing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50/50d/50j: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 70)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " bindery";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 70: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 80)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " ready for mailing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 80: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 88)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " mail complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 88: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " job complete ready to mail";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " being delivered";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 95: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }
                #endregion
                //07.01 commented out for above
                #region old status values 07.01
                /*
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
                */
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
                //07.01.2024 - get all chars before comma ","
                try
                {

                    //06.20.2024
                    //dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                    try
                    {
                        dr["Job Description"] = dr["Job Description"].ToString().Substring(0, dr["Job Description"].ToString().IndexOf(','));
                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: parse for job description: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

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


                #endregion - connection reopened

                #region Get postage for Stamps (Stamp jobs only)
                //calculate postage amount

                // Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                //Postage class = Standard Presort then qty ordered x .10 = postage amount



                string aaa = dr["Postage Class"].ToString();

                if (billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .25;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }

                else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .10;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }//end else if can put more else if here

                else { //this will execute if all false^ 

                    dr["Postage for Stamps"] = "$0.00";

                }
                #endregion

                #region formatting quantity
                dr["Quantity"] = String.Format("{0:n0}", dr["Quantity Bad"]);
                #endregion



            }//end foreach



            //drop unformatted column and set ordinal so columns back in order lol
            dt.Columns.Remove("Date Ship By Bad");
            dt.Columns.Remove("Quantity Bad");

            dt.Columns["Job ID"].SetOrdinal(0);
            dt.Columns["Job Status"].SetOrdinal(1);
            dt.Columns["Job Description"].SetOrdinal(2);
            dt.Columns["Quantity"].SetOrdinal(3);
            dt.Columns["Date Ship By"].SetOrdinal(4);
            dt.Columns["Postage Class"].SetOrdinal(5);
            dt.Columns["Postage for Stamps"].SetOrdinal(6);
            dt.Columns["AC Rep"].SetOrdinal(7);


            //formatting?
            // GridView1.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //GridView1.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            GridView1.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            // GridView1.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;




            //use this for testing comment - not actual data just 1 dummy row
            /*
            dt.Columns.Add("Job ID");
            dt.Columns.Add("Job Status");
            dt.Columns.Add("Job Description");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Date Ship By");
            dt.Columns.Add("Postage Class");
            dt.Columns.Add("Postage for Stamps");
            dt.Columns.Add("AC Rep");

            dt.Rows.Add("252374","02 waiting on art and date","	A1235 STOKES-TRAINOR CBGC / 6x9 letter/coupon/ GREY 3.75 x 6.75","4,266","07-08-2024","First Class Presort","$1,066.50","Patrick Fust");
            */

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

        public void Refresh_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dt.Columns.Clear();
            //GridView1.Columns.Clear();
            GridView1.DataSource = dt;
            //GridView1.DataBind();

            GridView1.AutoGenerateColumns = false;

            user = Session["USER"].ToString();
            pass = Session["PASS"].ToString();
            cust = Session["CUST"].ToString();


            #region regular refresh no checkboxes
            //if refresh click and no checkboxes
            //if (CheckBox1.Checked == false && CheckBox2.Checked == false)
            // {
            // comment all out for testing start


            Server.TransferRequest(Request.Url.AbsolutePath, false);

            /*
            //get customer jobs due report specific cust number ^

            string connectStr = "DSN=Progress11;uid=Bob;pwd=Orchard";


            //open th econnection and error check
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
            dt.Columns[3].ColumnName = "Quantity Bad";
            dt.Columns[4].ColumnName = "Date Ship By Bad";
            dt.Columns[5].ColumnName = "Postage Class";
            dt.Columns[6].ColumnName = "AC Rep";

            //lol no way to format date ship by what a joke
            //same with quantity is because data type coming from sql query^ is immutable
            dt.Columns.Add("Date Ship By");
            dt.Columns.Add("Quantity");

            //postage cost (calcualted field)
            dt.Columns.Add("Postage for Stamps");




            foreach (DataRow dr in dt.Rows)
            {

                #region Job status in easy read forms

                //07.01 update
                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 02)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " waiting on art and/or data";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 02: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 05)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " print ready – need data";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 05: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 09)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " in prepress";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "09r" || dr["Job Status"].ToString().ToUpper() == "09R" || dr["Job Status"].ToString().ToLower() == "09r")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " revision needed";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09r/09R: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 18)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " out on proof";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 18: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "50" || dr["Job Status"].ToString() == "50d" || dr["Job Status"].ToString().ToUpper() == "50d" || dr["Job Status"].ToString().ToLower() == "50d" || dr["Job Status"].ToString() == "50j" || dr["Job Status"].ToString().ToUpper() == "50j" || dr["Job Status"].ToString().ToLower() == "50j")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " printing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50/50d/50j: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 70)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " bindery";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 70: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 80)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " ready for mailing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 80: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 88)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " mail complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 88: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " job complete ready to mail";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " being delivered";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 95: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }
                #endregion
                //07.01 commented out for above
                #region old status values 07.01
                /*
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
                //07.01.2024 - get all chars before comma ","
                try
                {

                    //06.20.2024
                    //dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                    try
                    {
                        dr["Job Description"] = dr["Job Description"].ToString().Substring(0, dr["Job Description"].ToString().IndexOf(','));
                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: parse for job description: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

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
                if (pcStampStr == "PC Stamp" || pcStampStr.ToUpper() == "PC Stamp" || pcStampStr.ToLower() == "PC Stamp" || pcStampStr.Contains("PC Stamp"))
                {

                    billIt = true;

                }


                #endregion - connection reopened

                #region Get postage for Stamps (Stamp jobs only)
                //calculate postage amount

                // Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                //Postage class = Standard Presort then qty ordered x .10 = postage amount



                string aaa = dr["Postage Class"].ToString();

                if (billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .25;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }

                else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .10;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }//end else if can put more else if here

                else
                { //this will execute if all false^ 

                    dr["Postage for Stamps"] = "$0.00";

                }
                #endregion

                #region formatting quantity
                dr["Quantity"] = String.Format("{0:n0}", dr["Quantity Bad"]);
                #endregion



            }//end foreach



            //drop unformatted column and set ordinal so columns back in order lol
            dt.Columns.Remove("Date Ship By Bad");
            dt.Columns.Remove("Quantity Bad");

            dt.Columns["Job ID"].SetOrdinal(0);
            dt.Columns["Job Status"].SetOrdinal(1);
            dt.Columns["Job Description"].SetOrdinal(2);
            dt.Columns["Quantity"].SetOrdinal(3);
            dt.Columns["Date Ship By"].SetOrdinal(4);
            dt.Columns["Postage Class"].SetOrdinal(5);
            dt.Columns["Postage for Stamps"].SetOrdinal(6);
            dt.Columns["AC Rep"].SetOrdinal(7);


            //formatting?
            // GridView1.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //GridView1.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            GridView1.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            // GridView1.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;




            //use this for testing comment - not actual data just 1 dummy row
            /*
            dt.Columns.Add("Job ID");
            dt.Columns.Add("Job Status");
            dt.Columns.Add("Job Description");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Date Ship By");
            dt.Columns.Add("Postage Class");
            dt.Columns.Add("Postage for Stamps");
            dt.Columns.Add("AC Rep");

            dt.Rows.Add("252374","02 waiting on art and date","	A1235 STOKES-TRAINOR CBGC / 6x9 letter/coupon/ GREY 3.75 x 6.75","4,266","07-08-2024","First Class Presort","$1,066.50","Patrick Fust");
            

            GridView1.DataSource = dt;
            GridView1.DataBind();
            */
            Label3.Text = "...Done! Last refresh: " + DateTime.Now;

            #endregion



            // comment all out for testing start


            /*


            #region both checkboxes (empty set tell user to uncheck one and refresh

            if (CheckBox1.Checked == true && CheckBox2.Checked == true)
            {
                dt.Columns.Add("Job ID");
                dt.Columns.Add("Job Status");
                dt.Columns.Add("Job Description");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Date Ship By");
                dt.Columns.Add("Postage Class");
                dt.Columns.Add("Postage for Stamps");
                dt.Columns.Add("AC Rep");

                dt.Rows.Add("Error: ", "Cannot have both checkboxes", "Try again", "101", "07-08-2024", "First Class Presort", "$1,066.50", "First Last");


                GridView1.DataSource = dt;
                GridView1.DataBind();


                Label3.Text = "...Done! Last refresh: " + DateTime.Now;
            }
            #endregion


            #region only closed jobs

            if (CheckBox1.Checked == true && CheckBox2.Checked == false)
            {
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


                string queryString = "SELECT Job.\"Job-Id\", Job.\"Default-TagStatus-ID\" ,Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", MailingVersionFreeField.\"Free-Field-Char\", " +

                                    "Job.\"PO-Number\" FROM PUB.JOB INNER JOIN PUB.MailingVersionFreeField ON Job.\"Job-Id\" = MailingVersionFreeField.\"Job-Id\" " +

                                    "WHERE \"Cust-ID-Ordered-by\" = \'1259\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 0 " +

                                    "AND MailingVersionFreeField.\"Sequence\"= 5" +

                                    "ORDER BY Job.\"Job-ID\"";

                OdbcDataAdapter custDTadap = new OdbcDataAdapter(queryString, dbConn); //connects to database and passes sql string above to query

                custDTadap.Fill(dt);

                dbConn.Close();


                //change column names
                dt.Columns[0].ColumnName = "Job ID";
                dt.Columns[1].ColumnName = "Job Status";
                dt.Columns[2].ColumnName = "Job Description";
                dt.Columns[3].ColumnName = "Quantity Bad";
                dt.Columns[4].ColumnName = "Date Ship By Bad";
                dt.Columns[5].ColumnName = "Postage Class";
                dt.Columns[6].ColumnName = "AC Rep";

                //lol no way to format date ship by what a joke
                //same with quantity is because data type coming from sql query^ is immutable
                dt.Columns.Add("Date Ship By");
                dt.Columns.Add("Quantity");

                //postage cost (calcualted field)
                dt.Columns.Add("Postage for Stamps");




                foreach (DataRow dr in dt.Rows)
                {

                    #region Job status in easy read forms

                    //07.01 update
                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 02)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " waiting on art and date";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 02: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 05)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " print ready – need data";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 05: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }


                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 09)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " in prepress";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 09: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (dr["Job Status"].ToString() == "09r" || dr["Job Status"].ToString().ToUpper() == "09R" || dr["Job Status"].ToString().ToLower() == "09r")
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " revision needed";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 09r/09R: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 18)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " out on proof";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 18: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (dr["Job Status"].ToString() == "50" || dr["Job Status"].ToString() == "50d" || dr["Job Status"].ToString().ToUpper() == "50d" || dr["Job Status"].ToString().ToLower() == "50d" || dr["Job Status"].ToString() == "50j" || dr["Job Status"].ToString().ToUpper() == "50j" || dr["Job Status"].ToString().ToLower() == "50j")
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " printing";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 50/50d/50j: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 70)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " bindery";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 70: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }


                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 80)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " ready for mailing";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 80: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 88)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " mail complete";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 88: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " job complete ready to mail";
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
                            dr["Job Status"] = dr["Job Status"].ToString() + " being delivered";
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
                            dr["Job Status"] = dr["Job Status"].ToString() + " complete";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 95: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    //07.01 commented out for above
                    /*
                    #region old status values 07.01
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
                    //07.01.2024 - get all chars before comma ","
                    try
                    {

                        //06.20.2024
                        //dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                        try
                        {
                            dr["Job Description"] = dr["Job Description"].ToString().Substring(0, dr["Job Description"].ToString().IndexOf(','));
                        }
                        catch (Exception ex)
                        {

                            // string error = "Error: parse for job description: " + ex.ToString();

                            //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                            // File.WriteAllText(path, error);

                        }

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
                    if (pcStampStr == "PC Stamp" || pcStampStr.ToUpper() == "PC Stamp" || pcStampStr.ToLower() == "PC Stamp" || pcStampStr.Contains("PC Stamp"))
                    {

                        billIt = true;

                    }


                    #endregion

                    #region Get postage for Stamps (Stamp jobs only)
                    //calculate postage amount

                    // Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                    //Postage class = Standard Presort then qty ordered x .10 = postage amount




                    if (billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                    {

                        int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                        double cost = qty * .25;

                        dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                    }

                    else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                    {

                        int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                        double cost = qty * .10;

                        dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                    }//end else if can put more else if here

                    else
                    { //this will execute if all false^ 

                        dr["Postage for Stamps"] = "$0.00";

                    }
                    #endregion

                    #region formatting quantity
                    dr["Quantity"] = String.Format("{0:n0}", dr["Quantity Bad"]);
                    #endregion



                }//end foreach



                //drop unformatted column and set ordinal so columns back in order lol
                dt.Columns.Remove("Date Ship By Bad");
                dt.Columns.Remove("Quantity Bad");

                dt.Columns["Job ID"].SetOrdinal(0);
                dt.Columns["Job Status"].SetOrdinal(1);
                dt.Columns["Job Description"].SetOrdinal(2);
                dt.Columns["Quantity"].SetOrdinal(3);
                dt.Columns["Date Ship By"].SetOrdinal(4);
                dt.Columns["Postage Class"].SetOrdinal(5);
                dt.Columns["Postage for Stamps"].SetOrdinal(6);
                dt.Columns["AC Rep"].SetOrdinal(7);


                //formatting?
                // GridView1.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                // GridView1.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                //GridView1.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                GridView1.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                GridView1.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                // GridView1.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                GridView1.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                // GridView1.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


                //use this for testing comment - not actual data just 1 dummy row
                /*
                dt.Columns.Add("Job ID");
                dt.Columns.Add("Job Status");
                dt.Columns.Add("Job Description");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Date Ship By");
                dt.Columns.Add("Postage Class");
                dt.Columns.Add("Postage for Stamps");
                dt.Columns.Add("AC Rep");

                dt.Rows.Add("jobID","status","description","qty","ship by","posatge class","postage for stamps","ac rep name");


                GridView1.DataSource = dt;
                GridView1.DataBind();

                Label3.Text = "...Done! Last refresh: " + DateTime.Now;
            }
            #endregion

            #region append closed jobs to end
            if (CheckBox1.Checked == false && CheckBox2.Checked == true)
            {

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
                dt.Columns[3].ColumnName = "Quantity Bad";
                dt.Columns[4].ColumnName = "Date Ship By Bad";
                dt.Columns[5].ColumnName = "Postage Class";
                dt.Columns[6].ColumnName = "AC Rep";

                //lol no way to format date ship by what a joke
                //same with quantity is because data type coming from sql query^ is immutable
                dt.Columns.Add("Date Ship By");
                dt.Columns.Add("Quantity");

                //postage cost (calcualted field)
                dt.Columns.Add("Postage for Stamps");




                foreach (DataRow dr in dt.Rows)
                {

                    #region Job status in easy read forms

                    //07.01 update
                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 02)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " waiting on art and date";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 02: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 05)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " print ready – need data";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 05: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }


                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 09)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " in prepress";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 09: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (dr["Job Status"].ToString() == "09r" || dr["Job Status"].ToString().ToUpper() == "09R" || dr["Job Status"].ToString().ToLower() == "09r")
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " revision needed";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 09r/09R: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 18)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " out on proof";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 18: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (dr["Job Status"].ToString() == "50" || dr["Job Status"].ToString() == "50d" || dr["Job Status"].ToString().ToUpper() == "50d" || dr["Job Status"].ToString().ToLower() == "50d" || dr["Job Status"].ToString() == "50j" || dr["Job Status"].ToString().ToUpper() == "50j" || dr["Job Status"].ToString().ToLower() == "50j")
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " printing";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 50/50d/50j: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 70)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " bindery";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 70: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }


                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 80)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " ready for mailing";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 80: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 88)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " mail complete";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 88: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    try
                    {
                        if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                        {
                            dr["Job Status"] = dr["Job Status"].ToString() + " job complete ready to mail";
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
                            dr["Job Status"] = dr["Job Status"].ToString() + " being delivered";
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
                            dr["Job Status"] = dr["Job Status"].ToString() + " complete";
                        }

                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: status 95: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                    //07.01 commented out for above
                    /*
                    #region old status values 07.01
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
                    //07.01.2024 - get all chars before comma ","
                    try
                    {

                        //06.20.2024
                        //dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                        try
                        {
                            dr["Job Description"] = dr["Job Description"].ToString().Substring(0, dr["Job Description"].ToString().IndexOf(','));
                        }
                        catch (Exception ex)
                        {

                            // string error = "Error: parse for job description: " + ex.ToString();

                            //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                            // File.WriteAllText(path, error);

                        }

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
                    if (pcStampStr == "PC Stamp" || pcStampStr.ToUpper() == "PC Stamp" || pcStampStr.ToLower() == "PC Stamp" || pcStampStr.Contains("PC Stamp"))
                    {

                        billIt = true;

                    }


                    #endregion

                    #region Get postage for Stamps (Stamp jobs only)
                    //calculate postage amount

                    // Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                    //Postage class = Standard Presort then qty ordered x .10 = postage amount

                    if (billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                    {

                        int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                        double cost = qty * .25;

                        dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                    }

                    else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                    {

                        int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                        double cost = qty * .10;

                        dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                    }//end else if can put more else if here

                    else
                    { //this will execute if all false^ 

                        dr["Postage for Stamps"] = "$0.00";

                    }
                    #endregion

                    #region formatting quantity
                    dr["Quantity"] = String.Format("{0:n0}", dr["Quantity Bad"]);
                    #endregion



                }//end foreach



                //drop unformatted column and set ordinal so columns back in order lol
                dt.Columns.Remove("Date Ship By Bad");
                dt.Columns.Remove("Quantity Bad");

                dt.Columns["Job ID"].SetOrdinal(0);
                dt.Columns["Job Status"].SetOrdinal(1);
                dt.Columns["Job Description"].SetOrdinal(2);
                dt.Columns["Quantity"].SetOrdinal(3);
                dt.Columns["Date Ship By"].SetOrdinal(4);
                dt.Columns["Postage Class"].SetOrdinal(5);
                dt.Columns["Postage for Stamps"].SetOrdinal(6);
                dt.Columns["AC Rep"].SetOrdinal(7);


                //formatting?
                // GridView1.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                // GridView1.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                //GridView1.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                GridView1.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                GridView1.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                // GridView1.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                GridView1.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                // GridView1.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


                //use this for testing comment - not actual data just 1 dummy row
                /*
                dt.Columns.Add("Job ID");
                dt.Columns.Add("Job Status");
                dt.Columns.Add("Job Description");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Date Ship By");
                dt.Columns.Add("Postage Class");
                dt.Columns.Add("Postage for Stamps");
                dt.Columns.Add("AC Rep");

                dt.Rows.Add("jobID","status","description","qty","ship by","posatge class","postage for stamps","ac rep name");



            DataTable dt2 = new DataTable();

            //get customer jobs due report specific cust number ^

            string connectStr2 = "DSN=Progress11;uid=Bob;pwd=Orchard";


            //open the econnection and error check
            OdbcConnection dbConn2 = new OdbcConnection(connectStr2);
            dbConn2.ConnectionTimeout = 5000;
            try
            {
                dbConn2.Open();
            }
            catch (Exception ex)
            {

                string error = ex + " : DB error cannot connect";

                dbConn2.Close();

                ErrorLog(error);

            }


            string queryString2 = "SELECT Job.\"Job-Id\", Job.\"Default-TagStatus-ID\" ,Job.\"Job-Desc\", Job.\"Quantity-Ordered\", Job.\"Date-Ship-By\", MailingVersionFreeField.\"Free-Field-Char\", " +

                                "Job.\"PO-Number\", Job.\"Job-Open\" FROM PUB.JOB INNER JOIN PUB.MailingVersionFreeField ON Job.\"Job-Id\" = MailingVersionFreeField.\"Job-Id\" " +

                                "WHERE \"Cust-ID-Ordered-by\" = \'1259\' AND Job.\"System-ID\" = \'Viso\' AND Job.\"Job-Open\" = 0 " +

                                "AND MailingVersionFreeField.\"Sequence\"= 5" +

                                "ORDER BY Job.\"Job-ID\"";

            OdbcDataAdapter custdt2adap = new OdbcDataAdapter(queryString2, dbConn2); //connects to database and passes sql string above to query

            custdt2adap.Fill(dt2);

            dbConn2.Close();


            //change column names
            dt2.Columns[0].ColumnName = "Job ID";
            dt2.Columns[1].ColumnName = "Job Status";
            dt2.Columns[2].ColumnName = "Job Description";
            dt2.Columns[3].ColumnName = "Quantity Bad";
            dt2.Columns[4].ColumnName = "Date Ship By Bad";
            dt2.Columns[5].ColumnName = "Postage Class";
            dt2.Columns[6].ColumnName = "AC Rep";

            //lol no way to format date ship by what a joke
            //same with quantity is because data type coming from sql query^ is immutable
            dt2.Columns.Add("Date Ship By");
            dt2.Columns.Add("Quantity");

            //postage cost (calcualted field)
            dt2.Columns.Add("Postage for Stamps");




            foreach (DataRow dr in dt2.Rows)
            {

                #region Job status in easy read forms

                //07.01 update
                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 02)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " waiting on art and date";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 02: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 05)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " print ready – need data";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 05: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 09)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " in prepress";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "09r" || dr["Job Status"].ToString().ToUpper() == "09R" || dr["Job Status"].ToString().ToLower() == "09r")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " revision needed";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 09r/09R: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 18)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " out on proof";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 18: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (dr["Job Status"].ToString() == "50" || dr["Job Status"].ToString() == "50d" || dr["Job Status"].ToString().ToUpper() == "50d" || dr["Job Status"].ToString().ToLower() == "50d" || dr["Job Status"].ToString() == "50j" || dr["Job Status"].ToString().ToUpper() == "50j" || dr["Job Status"].ToString().ToLower() == "50j")
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " printing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 50/50d/50j: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 70)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " bindery";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 70: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }


                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 80)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " ready for mailing";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 80: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 88)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " mail complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 88: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                try
                {
                    if (Convert.ToInt32(dr["Job Status"].ToString()) == 90)
                    {
                        dr["Job Status"] = dr["Job Status"].ToString() + " job complete ready to mail";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " being delivered";
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
                        dr["Job Status"] = dr["Job Status"].ToString() + " complete";
                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: status 95: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }

                //07.01 commented out for above
                /*
                #region old status values 07.01
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
                //07.01.2024 - get all chars before comma ","
                try
                {

                    //06.20.2024
                    //dr["Job Description"] = dr["Job Description"].ToString().Substring(0, 20);

                    try
                    {
                        dr["Job Description"] = dr["Job Description"].ToString().Substring(0, dr["Job Description"].ToString().IndexOf(','));
                    }
                    catch (Exception ex)
                    {

                        // string error = "Error: parse for job description: " + ex.ToString();

                        //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                        // File.WriteAllText(path, error);

                    }

                }
                catch (Exception ex)
                {

                    // string error = "Error: Description shorten to 20 chars: " + ex.ToString();

                    //  string path = @"\\visonas\public\kyle\CustomerSite\log/customer_site_log.txt";

                    // File.WriteAllText(path, error);

                }
                #endregion

                #region Mark to bill stamp records if sequence = 1 equals PC Stamp will need loop

                dbConn2.Open();
                DataTable dt2StampBillCheck = new DataTable();

                string queryToMarkStampBill = "Select MailingVersionFreeField.\"Free-Field-Char\" FROM PUB.MailingVersionFreeField WHERE \"Job-Id\" = " + dr["Job ID"].ToString() + " AND \"Sequence\"=1";
                OdbcDataAdapter querySeq1 = new OdbcDataAdapter(queryToMarkStampBill, dbConn2); //connects to database and passes sql string above to query

                querySeq1.Fill(dt2StampBillCheck);

                dbConn2.Close();

                string pcStampStr = dt2StampBillCheck.Rows[0][0].ToString();
                bool billIt = false;

                //now mark seq5 (append Y/N) in column "Postage Class" { if dt2StampBillCheck \"Free-Field-Char\" = PC Stamp }
                if (pcStampStr == "PC Stamp" || pcStampStr.ToUpper() == "PC Stamp" || pcStampStr.ToLower() == "PC Stamp" || pcStampStr.Contains("PC Stamp"))
                {

                    billIt = true;

                }


                #endregion

                #region Get postage for Stamps (Stamp jobs only)
                //calculate postage amount

                // Postage Class = First Class Presort then quantity ordered x .25 = postage amount
                //Postage class = Standard Presort then qty ordered x .10 = postage amount


                if (billIt == true && (dr["Postage Class"].ToString() == "First Class Presort" || dr["Postage Class"].ToString().ToUpper() == "First Class Presort" || dr["Postage Class"].ToString().ToLower() == "First Class Presort" || dr["Postage Class"].ToString().Contains("First Class Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .25;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }

                else if (billIt == true && (dr["Postage Class"].ToString() == "Standard Presort" || dr["Postage Class"].ToString().ToUpper() == "Standard Presort" || dr["Postage Class"].ToString().ToLower() == "Standard Presort" || dr["Postage Class"].ToString().Contains("Standard Presort")))
                {

                    int qty = Convert.ToInt32(dr["Quantity Bad"].ToString());

                    double cost = qty * .10;

                    dr["Postage for Stamps"] = "$" + String.Format("{0:n}", cost);
                }//end else if can put more else if here

                else
                { //this will execute if all false^ 

                    dr["Postage for Stamps"] = "$0.00";

                }
                #endregion

                #region formatting quantity
                dr["Quantity"] = String.Format("{0:n0}", dr["Quantity Bad"]);
                #endregion



            }//end foreach



            //drop unformatted column and set ordinal so columns back in order lol
            dt2.Columns.Remove("Date Ship By Bad");
            dt2.Columns.Remove("Quantity Bad");

            dt2.Columns["Job ID"].SetOrdinal(0);
            dt2.Columns["Job Status"].SetOrdinal(1);
            dt2.Columns["Job Description"].SetOrdinal(2);
            dt2.Columns["Quantity"].SetOrdinal(3);
            dt2.Columns["Date Ship By"].SetOrdinal(4);
            dt2.Columns["Postage Class"].SetOrdinal(5);
            dt2.Columns["Postage for Stamps"].SetOrdinal(6);
            dt2.Columns["AC Rep"].SetOrdinal(7);


            //formatting?
            // GridView1.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            //GridView1.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            GridView1.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            // GridView1.Columns[5].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.Columns[6].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            // GridView1.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Center;


            //use this for testing comment - not actual data just 1 dummy row
            /*
            dt2.Columns.Add("Job ID");
            dt2.Columns.Add("Job Status");
            dt2.Columns.Add("Job Description");
            dt2.Columns.Add("Quantity");
            dt2.Columns.Add("Date Ship By");
            dt2.Columns.Add("Postage Class");
            dt2.Columns.Add("Postage for Stamps");
            dt2.Columns.Add("AC Rep");

            dt2.Rows.Add("jobID","status","description","qty","ship by","posatge class","postage for stamps","ac rep name");
            */


            //dt.Merge(dt2);





        }//end refresh button


        public void Export_Click(object sender, EventArgs e)
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

        Response.End();

        }//end export to excelbutton

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }//end CustomerJobSearchclass
}//end namespace