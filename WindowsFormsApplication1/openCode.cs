using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO.IsolatedStorage;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BrightIdeasSoftware;

namespace ProjectTitanium
{
    public class openCode
    {
        public TextBox tb = new TextBox();

        public MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();

        public AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();

        public string teamname = "";
        public List<string> teamslist = new List<string>();

        public openCode()
        {
#if DEBUG
            connString.Database = "launchpad";
            connString.Server = "localhost";
            connString.UserID = "root";
            connString.Password = "";
#else
                        connString.Database = "launchpad";
                        connString.Server = "9.190.175.12";
                        connString.UserID = "root";
                        connString.Password = "password";
#endif

            connString.Port = 3306;

            connString.ConvertZeroDateTime = true;
        }

        public void AutoCompleteText() // Enables autofill of the Tab1 (Specifically used on Tab1)
        {
            try
            {
                tb.AutoCompleteMode = AutoCompleteMode.Suggest;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;

                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("select * from user", conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string ibmshortname = "";

                    while (reader.Read())
                    {
                        ibmshortname = reader.GetString("user_shortname");
                        mycollection.Add(ibmshortname);
                    }
                }

                tb.AutoCompleteCustomSource = mycollection;


                //if (ProjectTitanium.Properties.Settings.Default.lastUser != "user")
                //{
                //    t.Text = ProjectTitanium.Properties.Settings.Default.lastUser.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Application failed to connect to database. Please ensure connection to the database server is possible and try " +
            "again." + "\n" + ex.Message, "Database Connection Error", MessageBoxButtons.OK);
            }
        }

        public void AutoLoadTeamID()
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                string query = "SELECT id FROM team";

                MySqlDataAdapter teamda = new MySqlDataAdapter();
                DataTable teamdt = new DataTable();

                teamda.SelectCommand = new MySqlCommand(query, conn);
                teamda.Fill(teamdt);

                foreach (DataRow row in teamdt.Rows)
                {
                    teamslist.Add(row[0].ToString());
                }
            }
        }

        public List<string> AutoLoadTeamID(string username)
        {
            List<string> user_teams = new List<string>();

            using (var conn = new MySqlConnection(connString.ToString()))
            {
                string query = "Select team_id from user_team where user_user_shortname='" + username + "'";

                MySqlDataAdapter teamda = new MySqlDataAdapter();
                DataTable teamdt = new DataTable();

                teamda.SelectCommand = new MySqlCommand(query, conn);
                teamda.Fill(teamdt);

                foreach (DataRow row in teamdt.Rows)
                {
                    user_teams.Add(row[0].ToString());
                }
            }

            return user_teams;
        }

        public void addcolifnotthere(DataGridView dgv, DataGridViewTextBoxColumn c)
        {
            if (!(dgv.Columns.Contains(c)))
            {
                dgv.Columns.Insert(0, c);
            }
        }

        public void addcolifnotthere(DataGridView dgv, DataGridViewCheckBoxColumn c)
        {
            if (!(dgv.Columns.Contains(c)))
            {
                dgv.Columns.Insert(0, c);
            }
        }

        //public void addcolifnotthere(DataListView dlv, DataGridViewTextBoxColumn c)
        //{
        //    if (!(dlv.Columns.))
        //    {
        //        dlv.Columns.Insert(0, c);
        //    }
        //}

        public void Clear_All_Text(Control con) // Clears all input controls on the function it's invocked from
        {
            foreach (Control c in con.Controls)
            {
                if (c is System.Windows.Forms.TextBox || c is ComboBox || c is RichTextBox || c is DataGridView || c is RadioButton || c is CheckBox)
                {
                    if (c is System.Windows.Forms.TextBox)
                    {
                        ((System.Windows.Forms.TextBox)c).ResetText();
                    }

                    if (c is ComboBox)
                    {
                        if (((ComboBox)c).DropDownStyle == ComboBoxStyle.DropDownList)
                        {
                            ((ComboBox)c).Text = null;
                        }
                        else
                        {
                            ((ComboBox)c).ResetText();
                        }
                    }

                    if (c is RichTextBox)
                    {
                        ((RichTextBox)c).ResetText();
                    }

                    if (c is DataGridView)
                    {
                        ((DataGridView)c).DataSource = null;

                        //((DataGridView)c).Columns.Clear();

                        c.DataBindings.Clear();
                    }

                    if (c is RadioButton)
                    {
                        ((RadioButton)c).Checked = false;
                    }

                    if (c is CheckBox)
                    {
                        ((CheckBox)c).Checked = false;
                    }
                }
                else
                {
                    Clear_All_Text(c);
                }
            }
        }

        public void Execute_Query(string s)
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();
                var cmd = new MySqlCommand(s, conn);

                cmd.ExecuteNonQuery();
            }
        }

        public void Execute_Query(string s, DataRow dr)
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                // Notice below how there a 'using' for MySqlCommand, instead of just the connection? Weird, isn't it :)

                using (var cmd = new MySqlCommand(s, conn))
                {
                    cmd.Parameters.AddWithValue("@id", dr[0].ToString());
                    cmd.Parameters.AddWithValue("@name", dr[1].ToString());
                    cmd.Parameters.AddWithValue("@loc", dr[2].ToString());

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                }
            }
        }
    }
}
