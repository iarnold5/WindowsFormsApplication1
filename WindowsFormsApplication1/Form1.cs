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
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO.IsolatedStorage;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProjectTitanium
{
    public partial class Form1 : Form
    {
        private MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();

        string ibmshortname_checker = "";

        string choices = "";
        
        string teamname = "";

        string tablename = "";

        string profiledata = string.Empty;

        //Label lbl = new Label();

        int qpchk = 0;

        bool launchchk = false;

        DataGridView dataGridView2 = new DataGridView();
        DataGridView dataGridView3 = new DataGridView();
        DataGridView dataGridView4 = new DataGridView();

        DataGridViewTextBoxColumn tblid = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn tblnm = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn tblloc = new System.Windows.Forms.DataGridViewTextBoxColumn();

        DataGridViewTextBoxColumn a1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn a2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn a3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewCheckBoxColumn ax = new System.Windows.Forms.DataGridViewCheckBoxColumn();

        DataGridViewTextBoxColumn u1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn u2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn u3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewCheckBoxColumn ui = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        DataGridViewCheckBoxColumn uf = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        DataGridViewCheckBoxColumn uc = new System.Windows.Forms.DataGridViewCheckBoxColumn();

        AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();

        public Form1()
        {
            InitializeComponent();

            //this.Visible = false;
            //Form login = new Form1();
            //login.Show();

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

            //defaultapplist();
            //button3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AutoLoadTeamID();
            AutoCompleteText();
            //this.tableLayoutPanel11.Controls.Add(lbl, 1, 2);
            //lbl.Anchor = AnchorStyles.None;
            //lbl.AutoSize = true;
            //lbl.Text = "If nothing is visible, click 'Refresh'.\nSee what happens...";
            button3_Click(sender, e);
            
            string userlogged = Properties.Settings.Default.lastUser.ToString();
            userlogged = userlogged.Remove(userlogged.IndexOf('@'));

            textBox1.Text = userlogged;
        }

        void AutoCompleteText() // Enables autofill of the Tab1 (Specifically used on Tab1)
        {
            try
            {
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

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

                textBox1.AutoCompleteCustomSource = mycollection;
                textBox2.AutoCompleteCustomSource = mycollection;


                if (ProjectTitanium.Properties.Settings.Default.lastUser != "user")
                {
                    textBox1.Text = ProjectTitanium.Properties.Settings.Default.lastUser.ToString();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Application failed to connect to database. Please ensure connection to the database server is possible and try " +
            "again." + "\n" + ex.Message, "Database Connection Error", MessageBoxButtons.OK);
                this.Close();
            }
        }

        void AutoLoadTeamID()
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                string query = "SELECT id FROM team";

                MySqlDataAdapter teamda = new MySqlDataAdapter();
                DataTable teamdt = new DataTable();

                teamda.SelectCommand = new MySqlCommand(query, conn);
                teamda.Fill(teamdt);

                List<string> teams = new List<string>();

                foreach (DataRow row in teamdt.Rows)
                {
                    teams.Add(row[0].ToString());
                }
                comboBox1.Items.AddRange(teams.ToArray());
                comboBox2.Items.AddRange(teams.ToArray());
                comboBox3.Items.AddRange(teams.ToArray());
            }
        }

        private void addcolifnotthere(DataGridView dgv, DataGridViewTextBoxColumn c)
        {
            if (!(dgv.Columns.Contains(c)))
            {
                dgv.Columns.Insert(0, c);
            }
        }

        private void addcolifnotthere(DataGridView dgv, DataGridViewCheckBoxColumn c)
        {
            if (!(dgv.Columns.Contains(c)))
            {
                dgv.Columns.Insert(0, c);
            }
        }

        private void initgrid4()
        {
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();

            dataGridView4.AutoGenerateColumns = false;

            dataGridView4.AllowUserToDeleteRows = true;

            this.tableLayoutPanel9.Controls.Add(this.dataGridView4, 1, 1);

            // 
            // tblid
            // 
            this.tblid.HeaderText = "ID";
            this.tblid.Name = "tblid";
            this.tblid.ReadOnly = true;
            // 
            // tblnm
            // 
            this.tblnm.HeaderText = "Name";
            this.tblnm.Name = "tblnm";
            // 
            // tblloc
            // 
            this.tblloc.HeaderText = "Location/URL";
            this.tblloc.Name = "tblloc";
            // 
            // dataGridView4
            // 
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel9.SetColumnSpan(this.dataGridView4, 2);
            addcolifnotthere(dataGridView4, tblloc);
            addcolifnotthere(dataGridView4, tblnm);
            addcolifnotthere(dataGridView4, tblid);
            this.dataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView4.Location = new System.Drawing.Point(3, 36);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.Size = new System.Drawing.Size(982, 258);
            this.dataGridView4.TabIndex = 5;
            //this.dataGridView4.RowsRemoved += new DataGridViewRowsRemovedEventHandler(this.dataGridView4_RowsRemoved);
            this.dataGridView4.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView4_UserDeletingRow);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
        }

        private void initgrid()
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView3.AutoGenerateColumns = false;

            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView3.AllowUserToDeleteRows = false;

            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;

            this.tableLayoutPanel11.Controls.Add(this.dataGridView2, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.dataGridView3, 1, 0);
            // 
            // u1
            // 
            this.u1.HeaderText = "ID";
            this.u1.Name = "u1";
            this.u1.Visible = false;
            this.u1.Width = 43;
            // 
            // u2
            // 
            this.u2.HeaderText = "Name";
            this.u2.Name = "u2";
            this.u2.Width = 60;
            // 
            // u3
            // 
            this.u3.HeaderText = "URL";
            this.u3.Name = "u3";
            this.u3.Width = 54;
            // 
            // ui
            // 
            this.ui.HeaderText = "Internet Explorer";
            this.ui.Name = "ui";
            this.ui.Width = 81;
            // 
            // uf
            // 
            this.uf.HeaderText = "Mozilla Firefox";
            this.uf.Name = "uf";
            this.uf.Width = 71;
            // 
            // uc
            // 
            this.uc.HeaderText = "Google Chrome";
            this.uc.Name = "uc";
            this.uc.Width = 77;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            addcolifnotthere(dataGridView2, uc);
            addcolifnotthere(dataGridView2, uf);
            addcolifnotthere(dataGridView2, ui);
            addcolifnotthere(dataGridView2, u3);
            addcolifnotthere(dataGridView2, u2);
            addcolifnotthere(dataGridView2, u1);
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 245);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(988, 196);
            this.dataGridView2.TabIndex = 0;
            // 
            // a1
            // 
            this.a1.HeaderText = "ID";
            this.a1.Name = "a1";
            this.a1.Visible = false;
            this.a1.Width = 43;
            // 
            // a2
            // 
            this.a2.HeaderText = "Name of Application";
            this.a2.Name = "a2";
            this.a2.Width = 116;
            // 
            // a3
            // 
            this.a3.HeaderText = "Location";
            this.a3.Name = "a3";
            this.a3.Width = 73;
            // 
            // a4
            // 
            this.ax.HeaderText = "Launch";
            this.ax.Name = "ax";
            this.ax.Width = 49;
            // 
            // dataGridView3
            // 
            dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            addcolifnotthere(dataGridView3, ax);
            addcolifnotthere(dataGridView3, a3);
            addcolifnotthere(dataGridView3, a2);
            addcolifnotthere(dataGridView3, a1);
            dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView3.Location = new System.Drawing.Point(3, 43);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new System.Drawing.Size(988, 196);
            dataGridView3.TabIndex = 1;
        }

        private void loaddata(string tmid)
        {
            initgrid();

            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                MySqlDataAdapter appda = new MySqlDataAdapter();
                DataTable appdt = new DataTable();

                appda.SelectCommand = new MySqlCommand("SELECT a.appid, a.app_name, a.app_loc from app a INNER JOIN team_app ta ON a.appid=ta.app_appid where ta.team_id='" + tmid + "'", conn);
                appda.Fill(appdt);

                MySqlDataAdapter urlda = new MySqlDataAdapter();
                DataTable urldt = new DataTable();

                appda.SelectCommand = new MySqlCommand("SELECT u.urlid, u.url_name, u.url_loc from url u INNER JOIN team_url ta ON u.urlid=ta.url_urlid where ta.team_id='" + tmid + "'", conn);
                appda.Fill(urldt);

                for(int i = 0; i< 3; i++)
                {
                    if (dataGridView3.Columns[i].DataPropertyName != appdt.Columns[i].ColumnName.ToString())
                    {
                        dataGridView3.Columns[i].DataPropertyName = appdt.Columns[i].ColumnName.ToString();
                    }

                    if (dataGridView2.Columns[i].DataPropertyName != urldt.Columns[i].ColumnName.ToString())
                    {
                        dataGridView2.Columns[i].DataPropertyName = urldt.Columns[i].ColumnName.ToString();
                    }
                }

                dataGridView3.DataSource = appdt;
                dataGridView2.DataSource = urldt;
            }
            try
            {
                checkchoices(profiledata);
            }
            catch
            {

            }
        }

        private void checkchoices(string data)
        {
            //  WordKey to figuring out 'profiledata' code.
            //  x > x -> Checked
            //  i > i -> Internet Explorer
            //  f > f -> Firefox
            //  c > c -> Chrome

            string[] choices = data.Split('.');

            if (choices.Length >= 1)
            {
                foreach (string s in choices)
                {
                    if (s.Contains('x'))
                    {
                        int ss = int.Parse(s.Trim('x'));
                        checkthebox(dataGridView3, ss, a1, ax);
                    }
                    else if (s.Contains('i'))
                    {
                        int ss = int.Parse(s.Trim('i'));
                        checkthebox(dataGridView2, ss, u1, ui);
                    }
                    else if (s.Contains('f'))
                    {
                        int ss = int.Parse(s.Trim('f'));
                        checkthebox(dataGridView2, ss, u1, uf);
                    }
                    else if (s.Contains('c'))
                    {
                        int ss = int.Parse(s.Trim('c'));
                        checkthebox(dataGridView2, ss, u1, uc);
                    }
                }
            }
            else
            {
                MessageBox.Show("No profile data exist on this profile!");
            }
        }

        private void checkthebox(DataGridView dgv, int id, DataGridViewTextBoxColumn idcol, DataGridViewCheckBoxColumn chkcol)
        {
            int rowno = -1;
           
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string chk = "";
                chk = row.Cells[idcol.Name].Value.ToString();

                if (chk == (id.ToString()))
                {
                    rowno = row.Index;
                    break;
                }
            }

            if (rowno != -1)
            {
                (dgv.Rows[rowno].Cells[chkcol.Name] as DataGridViewCheckBoxCell).Value = true;
            }
            else
            {
                MessageBox.Show("Couldn't check the box at ID: " + id);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //lbl.Dispose();

            Clear_All_Text(tableLayoutPanel11);

            loaddata(teamname);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);

            //#if DEBUG

            //#else
            launchchk = true;
            string[] flines = batchmodifyer();

            File.WriteAllLines(@"C:\launchbatch.bat", flines);

            var process = Process.Start(@"C:\launchbatch.bat");
            if (process.HasExited == true)
            {
                File.Delete(@"C:\launchbatch.bat");
            }

            MessageBox.Show("Launch Successful!", "Success");
            launchchk = false;
            //#endif
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn5 = new MySqlConnection(connString.ToString()))
            {
                if (conn5.State.ToString() != "Open")
                {
                    conn5.Open();
                }

                DataTable changesapp = ((DataTable)dataGridView3.DataSource).GetChanges();
                DataTable changesurl = ((DataTable)dataGridView2.DataSource).GetChanges();

                MySqlDataAdapter mdp = new MySqlDataAdapter();

                if (changesapp != null)
                {
                    foreach (DataRow row in changesapp.Rows)
                    {
                        string s = "";
                        s = "INSERT INTO app(appid, app_name, app_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE app_name = @name, app_loc = @loc";
                        Execute_Query(s, row);
                    }
                    MessageBox.Show("Apps Updated!!", "Success");
                }

                if (changesurl != null)
                {
                    foreach (DataRow row in changesurl.Rows)
                    {
                        string s = "";
                        s = "INSERT INTO url(urlid, url_name, url_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE url_name = @name, url_loc = @loc";
                        Execute_Query(s, row);
                    }
                    MessageBox.Show("URLs Updated!!", "Success");
                }
            }
        }

        private void Execute_Query(string s)
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();
                var cmd = new MySqlCommand(s, conn);
               
                cmd.ExecuteNonQuery();
            }
        }

        private void Execute_Query(string s, DataRow dr)
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                // Notice how there a 'using' for MySqlCommand, instead of just the connection? Weird, isn't it :)

                using (var cmd = new MySqlCommand(s, conn))
                {
                    cmd.Parameters.AddWithValue("@id", dr[0].ToString());
                    cmd.Parameters.AddWithValue("@name", dr[1].ToString());
                    cmd.Parameters.AddWithValue("@loc", dr[2].ToString());

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            launchchk = false;
            if ((MessageBox.Show("The batch file will be the same as the options selected in the program. " +
                "If you need to change your choices, please re-run the program, or you can manually edit your batch file at will." +
                "\n \nClick 'OK' to save your batch file. \n \nWarning: Please DO NOT change the extention of the file or it won't run.", "Confirmation Message", MessageBoxButtons.OKCancel) == DialogResult.OK))
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = "unknown.bat";
                savefile.Filter = "All Files (*.*)|*.*";
                savefile.DefaultExt = "bat";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    string outfile = "";
                    outfile = Path.GetFullPath(savefile.FileName);

                    string[] flines = batchmodifyer();

                    File.WriteAllLines(outfile, flines);
                }
            }
        }

        //public static bool IsRunning(Process process)
        //{
        //    if (process == null)
        //    {
        //        throw new ArgumentNullException("process");
        //    }

        //    try
        //    {
        //        Process.GetProcessById(process.Id);
        //    }
        //    catch (ArgumentException)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private string[] batchmodifyer()
        {
            List<string> lines = new List<string>();
            choices = "";

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                string apploc = "";

                try
                { 
                    apploc = row.Cells["a3"].Value.ToString();
                }
                catch
                {
                    continue;
                }
                
                if (Convert.ToBoolean(row.Cells["ax"].Value) == true)
                {
                    if (apploc != "")
                    {
                        lines.Add("START \"\" " + apploc);
                        choices = choices + "x" + row.Cells["a1"].Value.ToString() + ".";
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            foreach(DataGridViewRow row in dataGridView2.Rows)
            {
                string url = "";

                try
                {
                    url = row.Cells["u3"].Value.ToString();
                }
                catch
                {
                    continue;
                }

                if (Convert.ToBoolean(row.Cells["ui"].Value) == true)
                {
                    if (url != "")
                    {
                        lines.Add("START \"\" iexplore.exe \"" + url + "\"");
                        choices = choices + "i" + row.Cells["u1"].Value.ToString() + ".";

                        Process[] prc = Process.GetProcessesByName("iexplore");

                        if (launchchk == true)
                        {
                            var runningProcessByName = Process.GetProcessesByName("iexplore");
                            if (runningProcessByName.Length == 0)
                            {
                                Process.Start("iexplore.exe");
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (Convert.ToBoolean(row.Cells["uf"].Value) == true)
                {
                    if (url != "")
                    {
                        lines.Add("START \"\" firefox -new-tab \"" + url + "\"");
                        choices = choices + "f" + row.Cells["u1"].Value.ToString() + ".";

                        if (launchchk == true)
                        {
                            var runningProcessByName = Process.GetProcessesByName("firefox");
                            if (runningProcessByName.Length == 0)
                            {
                                Process.Start("firefox");
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (Convert.ToBoolean(row.Cells["uc"].Value) == true)
                {
                    if (url != "")
                    {
                        lines.Add("START \"\" chrome -new-tab \"" + url + "\"");
                        choices = choices + "c" + row.Cells["u1"].Value.ToString() + ".";

                        if (launchchk == true)
                        {
                            var runningProcessByName = Process.GetProcessesByName("chrome");
                            if (runningProcessByName.Length == 0)
                            {
                                Process.Start("chrome");
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            string[] flines = lines.ToArray();

            return flines;
        }

        private bool usernprofile()
        {
            bool verdict = true;

            //string s = Properties.Settings.Default.lastUser.ToString();
            //s = s.Remove(s.IndexOf('@'));

            radioButton3.Checked = true;
            //textBox1.Text = s;

            if (textBox1.Text == "" && !(tableLayoutPanel7.Controls.OfType<RadioButton>().Any(r => r.Checked)))
            {
                MessageBox.Show("Please specify the user and the profile to load/save choices.", "Username required");
                verdict = false;
            }
            else if (textBox1.Text == "" || !(tableLayoutPanel7.Controls.OfType<RadioButton>().Any(r => r.Checked)))
            {
                if (!(tableLayoutPanel7.Controls.OfType<RadioButton>().Any(r => r.Checked)))
                {
                    MessageBox.Show("Please specify the profile to load/save choices to.", "Username required");
                }

                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please specify the user to load/save choices.", "Username required");
                }

                verdict = false;
            }
            else
            {
                verdict = true;
            }

            return verdict;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            launchchk = false;
            if (usernprofile() == true)
            {
                string[] _unused = batchmodifyer();
                string ibmshortname = "";
                ibmshortname = textBox1.Text;
                //string profilename = "";
                //profilename = tableLayoutPanel7.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;

                string profilename = "Profile #1";
                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    conn.Open();

                    string s = "INSERT INTO profile(user_user_shortname, profiledata, profile_name) VALUES('" + ibmshortname + "', '" + choices + "', '" + profilename + "') ON DUPLICATE KEY UPDATE profiledata = '" + choices + "'";
                    Execute_Query(s);
                }

                MessageBox.Show("User preferences saved to database.", "Success");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string ibmshortname = "";
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                ibmshortname = textBox1.Text;
            }

            try
            {
                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    MySqlCommand command = new MySqlCommand("Select team_id from user where user_shortname='" + ibmshortname + "'", conn);
                    command.Connection.Open();
                    teamname = (string)command.ExecuteScalar();
                    //MessageBox.Show("Team name for " + ibmshortname + " is = " + teamname);
                }
                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(teamname);
                radioButton3.Checked = true;
                button8_Click(sender, e);
            }
            catch(Exception x)
            {
                //MessageBox.Show("Error: " + x.Message);
            }
            finally
            {
                Properties.Settings.Default.lastUser = ibmshortname;
                Properties.Settings.Default.team_id = teamname;
                Properties.Settings.Default.Save();

                //MessageBox.Show("USING SETINGS: " + Properties.Settings.Default.lastUser + " is IN = " + Properties.Settings.Default.team_id);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                ibmshortname_checker = textBox2.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (qpchk == 0)
            {
                string addormod = button1.Text.ToString();

                if ((MessageBox.Show("Are you sure you want to " + addormod + " user: " + ibmshortname_checker + ", to database??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
                {
                    using (var conn = new MySqlConnection(connString.ToString()))
                    {
                        conn.Open();

                        string fullname = "";
                        fullname = textBox3.Text;
                        string teamid = "";
                        teamid = comboBox1.Text;

                        string s = "";
                        s = "INSERT INTO user(user_shortname, user_name, team_id) VALUES('" + ibmshortname_checker + "', '" + fullname + "', '" + teamid + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "', team_id = '" + teamid + "'";
                        try
                        {
                            Execute_Query(s);
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        if(addormod == "Add")
                        {
                            adddefprofile(ibmshortname_checker);
                        }
                    }
                    MessageBox.Show("User added/modified!", "Up top!");
                }
                else
                {
                    MessageBox.Show("User was not added to database.", "Add User Cancelled");
                    Clear_All_Text(tableLayoutPanel1);
                }
            }
            else if (qpchk == 1)
            {
                if ((MessageBox.Show("Are you sure you want to add multiple users to database??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
                {
                    using (MySqlConnection conn = new MySqlConnection(connString.ToString()))
                    {
                        conn.Open();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            string ibmshortname = "";
                            string fullname = "";
                            string teamid = "";

                            ibmshortname = row.Cells["Column1"].Value.ToString();
                            fullname = row.Cells["Column2"].Value.ToString();
                            try
                            {
                                teamid = row.Cells["Column3"].Value.ToString();
                            }
                            catch
                            {
                                teamid = "IBM";
                            }

                            string s = "";
                            s = "INSERT INTO user(user_shortname, user_name, team_id) VALUES('" + ibmshortname_checker + "', '" + fullname + "', '" + teamid + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "', team_id = '" + teamid + "'";
                            try
                            {
                                Execute_Query(s);
                            }
                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            adddefprofile(ibmshortname);
                        }
                    }
                    MessageBox.Show("Users added!!", "Up top!");
                }
                else
                {
                    MessageBox.Show("No users were added to database.", "Add Users Cancelled");
                    Clear_All_Text(tableLayoutPanel1);
                }
            }
        }

        private void adddefprofile(string ibmname)
        {
            string defprofilename = "Profile #1";
            string defchoices = "x4.x5.x6.x7.x8.c1.f2.f4.f5.i6.f8.";
            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                string s = "INSERT INTO profile(user_user_shortname, profiledata, profile_name) VALUES('" + ibmname + "', '" + defchoices + "', '" + defprofilename + "') ON DUPLICATE KEY UPDATE profiledata = '" + defchoices + "'";
                Execute_Query(s);
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                button1.Text = "Add";
            }
            else if(radioButton2.Checked == true)
            {
                button1.Text = "Modify";
            }

            //Clear_All_Text(tableLayoutPanel1);
        }

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
                    
                    if(c is RadioButton)
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

        private void button2_Click(object sender, EventArgs e)
        {
           Clear_All_Text(tableLayoutPanel1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            if (usernprofile() == true)
            {
                string ibmshortname = "";
                ibmshortname = textBox1.Text;
                //string profilename = "";
                //profilename = tableLayoutPanel7.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
                profiledata = string.Empty;

                string profilename = "Profile #1";

                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    conn.Open();

                    string s = "";
                    s = "SELECT profiledata FROM profile WHERE user_user_shortname='" + ibmshortname + "' and profile_name='" + profilename + "'";

                    MySqlCommand cmd = new MySqlCommand(s, conn);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        profiledata = reader.GetString("profiledata");
                    }

                    if (profiledata != string.Empty)
                    {
                        label8.Text = "Profile Found!!";
                        button3_Click(sender, e);
                        checkchoices(profiledata);
                    }
                    else
                    {
                        label8.Text = "No profile data available!!";
                        button3_Click(sender, e);
                        //checkchoices(profiledata);
                        //MessageBox.Show("No profile data available!!", "Info");
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abtbx = new AboutBox1();
            abtbx.StartPosition = FormStartPosition.CenterParent;
            abtbx.ShowDialog();
        }

        private void dataGridView4_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow startingBalanceRow = this.dataGridView4.Rows[e.Row.Index];

            if ((MessageBox.Show("Are you sure you want to remove '" + dataGridView4.Rows[e.Row.Index].Cells["tblnm"].Value.ToString() + "' from " + teamname + " team??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
            {
                dataGridView4_RowsRemoved(startingBalanceRow);
            }
            else
            {
                // Do not allow the user to delete the Starting Balance row.
                MessageBox.Show("Deletion of " + startingBalanceRow.Cells["tblnm"].Value.ToString() + " was cancelled successfully!", "Deletion Cancelled");

                // Cancel the deletion if the Starting Balance row is included.
                e.Cancel = true;
            }
        }

        //private void dataGridView4_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        //{
        private void dataGridView4_RowsRemoved(DataGridViewRow row)
        {
            try
            {
                //if ((MessageBox.Show("Are you sure you want to delete " + dataGridView4.Rows[e.RowIndex].Cells["tblnm"].Value.ToString() + " from " + tablename + "??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
                //{
                //DataGridViewRow row = new DataGridViewRow();
                //row = dataGridView4.Rows[e.RowIndex];

                using (MySqlConnection conn = new MySqlConnection(connString.ToString()))
                {
                    if (conn.State.ToString() != "Open")
                    {
                        conn.Open();
                    }

                    if (tablename == "App")
                    {
                        string s = "";
                        s = "DELETE FROM team_app WHERE team_id='" + teamname + "' and app_appid='" + row.Cells["tblid"].Value.ToString() + "'";

                        var cmd = new MySqlCommand(s, conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(row.Cells["tblnm"].Value.ToString() + " successfully removed from " + teamname + "!", "Successful Deletion");
                    }
                    else if (tablename == "URL")
                    {
                        string s = "";
                        s = "DELETE FROM team_url WHERE team_id='" + teamname + "' and url_urlid='" + row.Cells["tblid"].Value.ToString() + "'";

                        var cmd = new MySqlCommand(s, conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(row.Cells["tblnm"].Value.ToString() + " successfully removed from " + teamname + "!", "Successful Deletion");
                    }
                }
                //}
            }
            catch
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text=="" || !(tableLayoutPanel10.Controls.OfType<RadioButton>().Any(r => r.Checked)))
            {
                MessageBox.Show("Please ensure a team is selected and either 'App' or 'URL' table is selected.", "Selection Required");
            }
            else
            {
                tablename = tableLayoutPanel10.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
                teamname = comboBox3.Text;

                Clear_All_Text(tableLayoutPanel9);

                initgrid4();

                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    conn.Open();

                    MySqlDataAdapter tableda = new MySqlDataAdapter();
                    DataTable tabledt = new DataTable();
                    
                    string s = "";

                    if (tablename == "App")
                    {
                        s = "SELECT a.appid, a.app_name, a.app_loc from app a INNER JOIN team_app ta ON a.appid=ta.app_appid where ta.team_id='" + teamname + "'";
                    }
                    else if (tablename == "URL")
                    {
                        s = "SELECT u.urlid, u.url_name, u.url_loc from url u INNER JOIN team_url ta ON u.urlid=ta.url_urlid where ta.team_id='" + teamname + "'";
                    }

                    tableda.SelectCommand = new MySqlCommand(s, conn);
                    tableda.Fill(tabledt);

                    for (int i = 0; i < 3; i++)
                    {
                        if (dataGridView4.Columns[i].DataPropertyName != tabledt.Columns[i].ColumnName.ToString())
                        {
                            dataGridView4.Columns[i].DataPropertyName = tabledt.Columns[i].ColumnName.ToString();
                        }
                    }

                    dataGridView4.DataSource = tabledt;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Are you sure you want to update all changes to " + tablename + " for " + teamname + "??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
            {
                string name = "";
                using (MySqlConnection conn = new MySqlConnection(connString.ToString()))
                {
                    if (conn.State.ToString() != "Open")
                    {
                        conn.Open();
                    }

                    MySqlDataAdapter tableda = new MySqlDataAdapter();

                    DataTable changestable = ((DataTable)dataGridView4.DataSource).GetChanges();

                    if (changestable != null)
                    {
                        if (tablename == "App")
                        {
                            foreach (DataRow row in changestable.Rows)
                            {
                                string s = "";
                                name = row["app_name"].ToString();
                                s = "INSERT INTO app(appid, app_name, app_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE app_name = @name, app_loc = @loc";
                                Execute_Query(s, row);

                                string q = "";
                                q = "INSERT INTO team_app(team_id, app_appid) VALUES ('" + teamname + "',(SELECT appid FROM app WHERE app_name='" + name + "')) ON DUPLICATE KEY UPDATE team_id='" + teamname + "', app_appid=(SELECT appid FROM app WHERE app_name='" + name + "')";
                                Execute_Query(q, row);
                            }
                            MessageBox.Show("Apps Updated!!", "Success");
                        }
                        else if (tablename == "URL")
                        {
                            foreach (DataRow row in changestable.Rows)
                            {
                                string s = "";
                                name = row["url_name"].ToString();
                                s = "INSERT INTO url(urlid, url_name, url_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE url_name = @name, url_loc = @loc";
                                Execute_Query(s, row);

                                string q = "";
                                q = "INSERT INTO team_url(team_id, url_urlid) VALUES ('" + teamname + "',(SELECT urlid FROM url WHERE url_name='" + name + "')) ON DUPLICATE KEY UPDATE team_id='" + teamname + "', url_urlid=(SELECT urlid FROM url WHERE url_name='" + name + "')";
                                Execute_Query(q, row);
                            }
                            MessageBox.Show("URLs Updated!!", "Success");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No updates found.", "Info");
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();

            if (o.GetDataPresent(DataFormats.Text))
            {
                if (dataGridView1.RowCount > 0)
                {
                    dataGridView1.Rows.Clear();
                }

                if (dataGridView1.ColumnCount > 0)
                {
                    dataGridView1.Columns.Clear();
                }

                bool columnsAdded = false;
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

                int myRowIndex = 0;

                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    if (!columnsAdded)
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                        {
                            //dataGridView1.Columns.Add(pastedRowCells[i].ToString(), pastedRowCells[i]);
                            dataGridView1.Columns.Add("Column"+(i+1), pastedRowCells[i]);
                        }

                        columnsAdded = true;
                        dataGridView1.Rows.Add();
                        continue;
                    }
                    else
                    {
                        if (dataGridView1.Rows.Count == 0) { myRowIndex = 0; } else { myRowIndex = dataGridView1.Rows.Count - 1; }

                        dataGridView1.Rows.Add();

                        using (DataGridViewRow myDataGridViewRow = dataGridView1.Rows[myRowIndex])
                        {
                            for (int i = 0; i < pastedRowCells.Length; i++)
                            {
                                myDataGridViewRow.Cells[i].Value = pastedRowCells[i];
                            }
                        }
                        dataGridView1.Update();
                        dataGridView1.EndEdit();
                    }
                }

                dataGridView1.Rows.RemoveAt((dataGridView1.RowCount - 1));
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Control && e.KeyCode == Keys.A)
            {
                if (tableLayoutPanel8.Visible == false)
                {
                    tableLayoutPanel8.Visible = true;
                }
                else
                {
                    tableLayoutPanel8.Visible = false;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.CheckState == CheckState.Checked)
            {
                qpchk = 1;
                tableLayoutPanel13.Enabled = false;
                button11.Enabled = true;
                dataGridView1.Enabled = true;
            }
            else
            {
                qpchk = 0;
                tableLayoutPanel13.Enabled = true;
                button11.Enabled = false;
                dataGridView1.Enabled = false;
            }
        }
    }
}
