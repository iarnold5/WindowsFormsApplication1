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
using BrightIdeasSoftware;
using BrightIdeasSoftware.Design;

namespace ProjectTitanium
{
    public partial class Form1 : Form
    {
        openCode ocode = new openCode();

        //public updateApps upApps = new updateApps();

        MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();
        string teamname;

        string choices = "";

        string profiledata = string.Empty;

        //Label lbl = new Label();

        bool launchchk = false;

        DataGridView dataGridView2 = new DataGridView();
        DataGridView dataGridView3 = new DataGridView();

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

        //DataListView dlv = new DataListView();

        public Form1()
        {
            InitializeComponent();

            connString = ((openCode)ocode).connString;
            teamname = ((openCode)ocode).teamname;

            //this.Visible = false;
            //Form login = new Form1();
            //login.Show();

            //#if DEBUG
            //            connString.Database = "launchpad";
            //            connString.Server = "localhost";
            //            connString.UserID = "root";
            //            connString.Password = "";
            //#else
            //            connString.Database = "launchpad";
            //            connString.Server = "9.190.175.12";
            //            connString.UserID = "root";
            //            connString.Password = "password";
            //#endif

            //connString.Port = 3306;

            //connString.ConvertZeroDateTime = true;

            //defaultapplist();
            //button3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ocode.tb = this.textBox1;
            ocode.AutoCompleteText();

            if (ProjectTitanium.Properties.Settings.Default.lastUser != "user" && ProjectTitanium.Properties.Settings.Default.lastUser != "")
            {
                textBox1.Text = Properties.Settings.Default.lastUser.ToString();
            }
        }

        //private void newinitgrid(DataTable dt)
        //{
        //    dlv.CheckBoxes = true;
        //    this.tableLayoutPanel11.Controls.Add(this.dlv, 2, 0);
        //    dlv.DataSource = dt;

        //    dlv.AutoResizeColumns(); //.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

        //    //ocode.addcolifnotthere(dataGridView3, ax);
        //    //ocode.addcolifnotthere(dataGridView3, a3);
        //    //ocode.addcolifnotthere(dataGridView3, a2);
        //    //ocode.addcolifnotthere(dataGridView3, a1);

        //    dlv.Dock = System.Windows.Forms.DockStyle.Fill;
        //    dlv.Name = "dataListView1";
        //}

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
            this.u1.HeaderText = "ID (Readonly)";
            this.u1.Name = "u1";
            this.u1.ReadOnly = true;
            this.u1.Visible = false;
            this.u1.Width = 43;
            // 
            // u2
            // 
            this.u2.HeaderText = "Name";
            this.u2.Name = "u2";
            this.u2.Width = 60;
            this.u2.DefaultCellStyle.Font = new Font("Arial", 10F);
            // 
            // u3
            // 
            this.u3.HeaderText = "URL";
            this.u3.Name = "u3";
            this.u3.Width = 54;
            this.u3.DefaultCellStyle.Font = new Font("Arial", 10F);
            // 
            // ui
            // 
            this.ui.HeaderText = "Internet Explorer";
            this.ui.Name = "ui";
            this.ui.Width = 40;
            // 
            // uf
            // 
            this.uf.HeaderText = "Mozilla Firefox";
            this.uf.Name = "uf";
            this.uf.Width = 40;
            // 
            // uc
            // 
            this.uc.HeaderText = "Google Chrome";
            this.uc.Name = "uc";
            this.uc.Width = 40;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ocode.addcolifnotthere(dataGridView2, uc);
            ocode.addcolifnotthere(dataGridView2, uf);
            ocode.addcolifnotthere(dataGridView2, ui);
            ocode.addcolifnotthere(dataGridView2, u3);
            ocode.addcolifnotthere(dataGridView2, u2);
            ocode.addcolifnotthere(dataGridView2, u1);
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 245);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(988, 196);
            this.dataGridView2.TabIndex = 0;
            // 
            // a1
            // 
            this.a1.HeaderText = "ID (Readonly)";
            this.a1.Name = "a1";
            this.a1.ReadOnly = true;
            this.a1.Visible = false;
            this.a1.Width = 43;
            // 
            // a2
            // 
            this.a2.HeaderText = "Name of Application";
            this.a2.Name = "a2";
            this.a2.Width = 116;
            this.a2.DefaultCellStyle.Font = new Font("Arial", 10F);
            // 
            // a3
            // 
            this.a3.HeaderText = "Location";
            this.a3.Name = "a3";
            this.a3.Width = 73;
            this.a3.DefaultCellStyle.Font = new Font("Arial", 10F);
            // 
            // a4
            // 
            this.ax.HeaderText = "Launch";
            this.ax.Name = "ax";
            this.ax.Width = 40;
            // 
            // dataGridView3
            // 
            dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ocode.addcolifnotthere(dataGridView3, ax);
            ocode.addcolifnotthere(dataGridView3, a3);
            ocode.addcolifnotthere(dataGridView3, a2);
            ocode.addcolifnotthere(dataGridView3, a1);
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



                //newinitgrid(appdt);
            }

            try
            {
                checkchoices(profiledata);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Load Data Error 272");
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
            ocode.Clear_All_Text(tableLayoutPanel11);

            loaddata(teamname);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);

            #if DEBUG
                MessageBox.Show("Batch file created and executed. Program waits for the process and then deletes the batch file. Launch success message shown later.");
            #else
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
            #endif
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
        
        private void button7_Click(object sender, EventArgs e)
        {
            launchchk = false;
            if (textBox1.Text != "")
            {
                string[] _unused = batchmodifyer();
                string ibmshortname = "";
                ibmshortname = textBox1.Text;

                string profilename = ibmshortname + "_" + teamname;
                using (var conn = new MySqlConnection(connString.ToString()))
                {
                    conn.Open();

                    string s = "INSERT INTO profile(user_user_shortname, profiledata, profile_name) VALUES('" + ibmshortname + "', '" + choices + "', '" + profilename + "') ON DUPLICATE KEY UPDATE profiledata = '" + choices + "'";
                    ocode.Execute_Query(s);
                }

                MessageBox.Show("User preferences saved to database.", "Success");
            }
            else
            {
                MessageBox.Show("Please specify the user.", "Username required");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string ibmshortname = "";

            ibmshortname = textBox1.Text;

            if (ocode.mycollection.Contains(ibmshortname))
            {
                try
                {
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange((ocode.AutoLoadTeamID(ibmshortname)).ToArray());
                    comboBox2.SelectedIndex = 0;
                    teamname = comboBox2.Text;
                    button8_Click(sender, e);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Team Selection Error 585");
                }
                finally
                {
                    Properties.Settings.Default.lastUser = ibmshortname;
                    Properties.Settings.Default.team_id = teamname;
                    Properties.Settings.Default.Save();

                    //MessageBox.Show("USING SETINGS: " + Properties.Settings.Default.lastUser + " is IN = " + Properties.Settings.Default.team_id);
                }
            }
            else
            {
                comboBox2.SelectedIndex = -1;
                label8.Text = "";
                ocode.Clear_All_Text(this.tableLayoutPanel11);
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (qpchk == 0)
        //    {
        //        string addormod = button1.Text.ToString();

        //        if ((MessageBox.Show("Are you sure you want to " + addormod + " user: " + ibmshortname_checker + ", to database??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
        //        {
        //            using (var conn = new MySqlConnection(connString.ToString()))
        //            {
        //                conn.Open();

        //                string fullname = "";
        //                fullname = textBox3.Text;
        //                string teamid = "";
        //                teamid = comboBox1.Text;

        //                string s = "";
        //                s = "INSERT INTO user(user_shortname, user_name, team_id) VALUES('" + ibmshortname_checker + "', '" + fullname + "', '" + teamid + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "', team_id = '" + teamid + "'";
        //                try
        //                {
        //                    Execute_Query(s);
        //                }
        //                catch (MySqlException ex)
        //                {
        //                    MessageBox.Show(ex.ToString());
        //                }

        //                if(addormod == "Add")
        //                {
        //                    adddefprofile(ibmshortname_checker);
        //                }
        //            }
        //            MessageBox.Show("User added/modified!", "Up top!");
        //        }
        //        else
        //        {
        //            MessageBox.Show("User was not added to database.", "Add User Cancelled");
        //            Clear_All_Text(tableLayoutPanel1);
        //        }
        //    }
        //    else if (qpchk == 1)
        //    {
        //        if ((MessageBox.Show("Are you sure you want to add multiple users to database??", "Confirmation", MessageBoxButtons.OKCancel)) == DialogResult.OK)
        //        {
        //            using (MySqlConnection conn = new MySqlConnection(connString.ToString()))
        //            {
        //                conn.Open();

        //                foreach (DataGridViewRow row in dataGridView1.Rows)
        //                {
        //                    string ibmshortname = "";
        //                    string fullname = "";
        //                    string teamid = "";

        //                    ibmshortname = row.Cells["Column1"].Value.ToString();
        //                    fullname = row.Cells["Column2"].Value.ToString();
        //                    try
        //                    {
        //                        teamid = row.Cells["Column3"].Value.ToString();
        //                    }
        //                    catch
        //                    {
        //                        teamid = "IBM";
        //                    }

        //                    string s = "";
        //                    s = "INSERT INTO user(user_shortname, user_name, team_id) VALUES('" + ibmshortname_checker + "', '" + fullname + "', '" + teamid + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "', team_id = '" + teamid + "'";
        //                    try
        //                    {
        //                        Execute_Query(s);
        //                    }
        //                    catch (MySqlException ex)
        //                    {
        //                        MessageBox.Show(ex.ToString());
        //                    }

        //                    adddefprofile(ibmshortname);
        //                }
        //            }
        //            MessageBox.Show("Users added!!", "Up top!");
        //        }
        //        else
        //        {
        //            MessageBox.Show("No users were added to database.", "Add Users Cancelled");
        //            Clear_All_Text(tableLayoutPanel1);
        //        }
        //    }
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            if (textBox1.Text != "")
            {
                string ibmshortname = "";
                ibmshortname = textBox1.Text;
                //string profilename = "";
                //profilename = tableLayoutPanel7.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
                profiledata = string.Empty;

                string profilename = ibmshortname + "_" + teamname;

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
                        //checkchoices(profiledata);
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
            else
            {
                MessageBox.Show("Please specify the user.", "Username required");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abtbx = new AboutBox1();
            abtbx.StartPosition = FormStartPosition.CenterParent;
            abtbx.ShowDialog();
        }
        
        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Keydown not needed after making seperate forms
            //if (e.Alt && e.Control && e.KeyCode == Keys.A)
            //{
            //    if (tableLayoutPanel8.Visible == false)
            //    {
            //        tableLayoutPanel8.Visible = true;
            //    }
            //    else if (tableLayoutPanel8.Visible == true)
            //    {
            //        tableLayoutPanel8.Visible = false;
            //    }
            //}
        }

        private void addNewInfoToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hi there. \n\nThe app is still under development, and new teams would need to be added by me for now. \n\nIf you need any new team added, or any new feature, or have any issues, please contact 'asolanki@au1.ibm.com' via Sametime or email. \n\nThank you.", "Information");
        }

        private void addUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addUser addU = new addUser();
            addU.StartPosition = FormStartPosition.CenterParent;
            if(addU.ShowDialog() == DialogResult.OK)
            {
                ocode.tb = this.textBox1;
                ocode.AutoCompleteText();
            }
        }

        private void updateAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateApps upApps = new updateApps();
            upApps.StartPosition = FormStartPosition.CenterParent;
            upApps.Text = sender.ToString();
            upApps.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ibmshortname = "";

            ibmshortname = textBox1.Text;

            if (ocode.mycollection.Contains(ibmshortname))
            {
                try
                {
                    teamname = comboBox2.Text;
                    button8_Click(sender, e);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Team Selection Error 810");
                }
                finally
                {
                    Properties.Settings.Default.lastUser = ibmshortname;
                    Properties.Settings.Default.team_id = teamname;
                    Properties.Settings.Default.Save();

                    //MessageBox.Show("USING SETINGS: " + Properties.Settings.Default.lastUser + " is IN = " + Properties.Settings.Default.team_id);
                }
            }
            else
            {
                comboBox2.SelectedIndex = -1;
                label8.Text = "";
                ocode.Clear_All_Text(this.tableLayoutPanel11);
            }
        }
    }
}
