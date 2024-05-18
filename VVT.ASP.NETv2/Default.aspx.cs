using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

namespace VVT.ASP.NETv2
{
    public partial class _Default : Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

       string custNumber = "";

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string password = txtPassword.Text;
            

       
            if (AuthenticateUser(username, password, custNumber))
            {
                Session["USER"] = username;
                Session["PASS"] = password;
                Session["CUST"] = custNumber;

                // Redirect to some authenticated page
                Response.Redirect("CustomerJobSearch.aspx");
            }
            else
            {
                // Show error message
                // For simplicity, just setting a label text here, you can handle this better
                Label1.Text = "Invalid username or password";
            }
        }

        private bool AuthenticateUser(string username, string password,string custNum)
        {

            //change this to shared folder .txt file
            string[] users = File.ReadAllLines(Server.MapPath("~/App_Data/loginDB.txt"));
            //string[] users = File.ReadAllLines(Server.MapPath(@"C:\Users\Kyle\Source\Repos\CustomerJobSite\VVT.ASP.NETv2\loginDB.txt"));

            foreach (string user in users)
            {
                string[] parts = user.Split(',');

                custNumber = parts[2];

                if (parts.Length == 3 && parts[0] == username && parts[1] == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
    }
