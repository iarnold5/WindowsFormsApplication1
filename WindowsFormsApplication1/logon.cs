using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO.IsolatedStorage;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProjectTitanium
{
    public partial class logon : Form
    {
        public logon()
        {
            InitializeComponent();

            loggingIn.Visible=false;
        }

        private void logon_Load(object sender, EventArgs e)
        {
            loggingIn.Hide();

            //filluser();
        }

        //void filluser()
        //{
        //    //try
        //    //{
        //    //txtUsername.AutoCompleteSource = AutoCompleteSource.CustomSource;

        //    //txtUsername.AutoCompleteCustomSource = Properties.Settings.Default.listUsers ;
        //    string[] list = new string[Properties.Settings.Default.listUsers.Count];
        //    list = Properties.Settings.Default.listUsers.Cast<string>().ToArray();
        //    if (list.Length != 0)
        //    {
        //        string s = "user@au1.ibm.com";
        //        Properties.Settings.Default.listUsers.Add(s);

        //        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        //        source.AddRange(list);
        //        txtUsername.AutoCompleteMode = AutoCompleteMode.Suggest;
        //        txtUsername.AutoCompleteSource = AutoCompleteSource.CustomSource;
        //        txtUsername.AutoCompleteCustomSource = source;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Application failed to connect to database. Please ensure connection to the database server is possible and try " +
        //    //"again." + "\n" + "\n" + ex.Message, "Database Connection Error", MessageBoxButtons.OK);
        //    //    //this.Close();
        //    //    return;
        //    //}
        //}

        private void login()
        {
            loggingIn.Visible = true;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;

            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter a username and password to proceed.", "Login Details Missing");
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
                loggingIn.Visible = false;
                return;
            }

            bool connectSuccess = false;
            string errortxt = "";

            try
            {

                string server = "LDAP://bluepages.ibm.com/ou=bluepages,o=ibm.com";

                DirectoryEntry oRoot = new DirectoryEntry(server, "", "", AuthenticationTypes.Anonymous);

                DirectorySearcher searcher = new DirectorySearcher(oRoot);
                searcher.PropertiesToLoad.Add("c");
                searcher.PropertiesToLoad.Add("uid");
                
                //searcher.PropertiesToLoad.AddRange(New String() { "cn", "mail"})
                // would also work and saves you some code
                
                //searcher.Filter = "(emailAddress=" & txtUsername.Text & ")"
                searcher.Filter = "(&(emailAddress=" + txtUsername.Text + "))";

                SearchResult results = default(SearchResult);
                results = searcher.FindOne();

                string serial = results.GetDirectoryEntry().Properties["uid"].Value.ToString();
                string cCode = results.GetDirectoryEntry().Properties["c"].Value.ToString();

                DirectoryEntry oRoot2 = new DirectoryEntry(server, "uid=" + serial + ",c=" + cCode + ",ou=bluepages,o=ibm.com", txtPassword.Text, AuthenticationTypes.SecureSocketsLayer);

                object obj = new object();
                obj = oRoot2.NativeObject;

                connectSuccess = true;

            }
            catch (Exception ex)
            {
                loggingIn.Visible = false;
                errortxt = ex.Message;
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }

            if (connectSuccess)
            {
                string s = txtUsername.Text;
                Properties.Settings.Default.lastUserEmail = s;
                Properties.Settings.Default.lastUser = s.TrimEnd('@');
                Properties.Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(errortxt, "Login Error");
                loggingIn.Visible = false;
                return;
            }
            //this.Close();
        }

        //public static bool IsAuthenticated(string ldap, string usr, string pwd)
        //{
        //    bool authenticated = false;

        //    try
        //    {
        //        DirectoryEntry entry = new DirectoryEntry(ldap, usr, pwd);
        //        object nativeObject = entry.NativeObject;
        //        authenticated = true;
        //    }
        //    catch (DirectoryServicesCOMException cex)
        //    {
        //        MessageBox.Show(cex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    return authenticated;
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }
    }
}
