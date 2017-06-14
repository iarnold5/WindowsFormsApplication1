using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ProjectTitanium
{
    public partial class addUser : Form
    {
        openCode ocode = new openCode();
        MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();

        string ibmshortname_checker = "";

        CheckState qpchk = CheckState.Unchecked;

        public addUser()
        {
            InitializeComponent();

            connString = ((openCode)ocode).connString;

            ocode.AutoLoadTeamID();
        }

        private void addUser_Load(object sender, EventArgs e)
        {
            ocode.tb = this.textBox2;
            ocode.AutoCompleteText();

            comboBox1.Items.AddRange(ocode.teamslist.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (qpchk == CheckState.Unchecked)
            {
                if (ibmshortname_checker != "" && textBox3.Text != "" && comboBox1.Text != "")
                {
                    string addormod = button1.Text.ToString();

                    if ((MessageBox.Show("Are you sure you want to " + addormod.ToLower() + " user (" + ibmshortname_checker + "), to database??", addormod + " User Request", MessageBoxButtons.OKCancel)) == DialogResult.OK)
                    {
                        using (var conn = new MySqlConnection(connString.ToString()))
                        {
                            conn.Open();

                            ocode.AutoCompleteText();

                            string fullname = "";
                            fullname = textBox3.Text;
                            string teamid = "";
                            teamid = comboBox1.Text;

                            string insert_user = "";
                            insert_user = "INSERT INTO user(user_shortname, user_name) VALUES('" + ibmshortname_checker + "', '" + fullname + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "'";
                            string assign_team = "";
                            assign_team = "INSERT IGNORE INTO user_team(user_user_shortname, team_id) VALUES('" + ibmshortname_checker + "', '" + teamid + "')";

                            try
                            {
                                ocode.Execute_Query(insert_user);
                                ocode.Execute_Query(assign_team);

                                // Users get a default profile if this is the first time their name is being added to database. Will affect the people changing teams as they won't get default profile.
                                if (!ocode.mycollection.Contains(ibmshortname_checker))
                                {
                                    adddefprofile(ibmshortname_checker, teamid);
                                }

                                ocode.tb = this.textBox2;
                                ocode.AutoCompleteText();

                                comboBox1.Items.AddRange(ocode.teamslist.ToArray());

                                MessageBox.Show("User " + addormod.ToLower() + "ed!!", "Data Entry Successful");
                            }
                            catch (MySqlException x)
                            {
                                MessageBox.Show(x.ToString(), "MySQL Exception Error 87");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("User was not added to database.", "Add User Cancelled");
                        ocode.Clear_All_Text(tableLayoutPanel1);
                    }
                }
                else
                {
                    MessageBox.Show("Please ensure a valid shortname, full name and, team are specified, and try again.", "Add User Failed");
                }
            }
            else if (qpchk == CheckState.Checked)
            {
                if ((MessageBox.Show("Are you sure you want to add multiple users to database??", "Add Multiple Users Request", MessageBoxButtons.OKCancel)) == DialogResult.OK)
                {
                    using (MySqlConnection conn = new MySqlConnection(connString.ToString()))
                    {
                        conn.Open();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            ocode.AutoCompleteText();

                            string ibmshortname = "";
                            string fullname = "";
                            string teamid = "";

                            ibmshortname = row.Cells["Column1"].Value.ToString();
                            fullname = row.Cells["Column2"].Value.ToString();
                            teamid = row.Cells["Column3"].Value.ToString();

                            if (ibmshortname != "" && fullname != "" && teamid != "")
                            {
                                string insert_user = "";
                                insert_user = "INSERT INTO user(user_shortname, user_name) VALUES('" + ibmshortname + "', '" + fullname + "') ON DUPLICATE KEY UPDATE user_name = '" + fullname + "'";
                                string assign_team = "";
                                assign_team = "INSERT IGNORE INTO user_team(user_user_shortname, team_id) VALUES('" + ibmshortname + "', '" + teamid + "')";

                                try
                                {
                                    ocode.Execute_Query(insert_user);
                                    ocode.Execute_Query(assign_team);

                                    // Users get a default profile if this is the first time their name is being added to database. Will affect the people changing teams as they won't get default profile.
                                    if (!ocode.mycollection.Contains(ibmshortname))
                                    {
                                        adddefprofile(ibmshortname, teamid);
                                    }
                                }
                                catch (Exception x)
                                {
                                    MessageBox.Show(x.Message);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Row No. " + row.Index + " didn't have data in proper format. Please ensure a valid shortname, full name and, team are specified, and try again.", "Add User Failed");
                            }

                        }
                    }
                    MessageBox.Show("Users added to database.", "Data Entry Successful");
                }
                else
                {
                    MessageBox.Show("No users were added to database.", "Add Users Cancelled");
                    ocode.Clear_All_Text(tableLayoutPanel1);
                }
            }
        }

        public void adddefprofile(string ibmname, string teamid)
        {
            string defprofilename = ibmname + "_" + teamid;

            string defchoices = "";

            if (teamid == "COMMS")
            {
                defchoices = "x1.x2.x3.x4.x5.f1.f2.f3.f4.f5.i6.f7.";
            }
            else if(teamid == "IBM")
            {
                defchoices = "x5.";
            }

            using (var conn = new MySqlConnection(connString.ToString()))
            {
                conn.Open();

                string s = "INSERT INTO profile(user_user_shortname, profiledata, profile_name) VALUES('" + ibmname + "', '" + defchoices + "', '" + defprofilename + "') ON DUPLICATE KEY UPDATE profiledata = '" + defchoices + "'";
                ocode.Execute_Query(s);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ocode.Clear_All_Text(tableLayoutPanel1);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!ocode.mycollection.Contains(ibmshortname_checker))
            {
                button1.Text = "Add";
            }
            else
            {
                button1.Text = "Modify";
            }
            ibmshortname_checker = textBox2.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            qpchk = checkBox1.CheckState;

            if(qpchk == CheckState.Unchecked)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                button11.Enabled = false;
            }
            else if (qpchk == CheckState.Checked)
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                button11.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();

            string teamid = "";
            teamid = comboBox1.Text;

            if (teamid == "")
            {
                MessageBox.Show("Please select a team before continuing. Can be modified after paste if required.", "Team Required");
            }
            else
            {
                if (o.GetDataPresent(DataFormats.Text))
                {
                    if (dataGridView1.RowCount > 0)
                    {
                        dataGridView1.Rows.Clear();
                    }
                    
                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

                    int myRowIndex = 0;

                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        Array.Resize(ref pastedRowCells, (pastedRowCells.Count() + 1));
                        pastedRowCells[pastedRowCells.Count() - 1] = teamid;

                        dataGridView1.Rows.Add();

                        if (dataGridView1.Rows.Count == 0) { myRowIndex = 0; } else { myRowIndex = dataGridView1.Rows.Count - 1; }
                        
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
                    //dataGridView1.Rows.RemoveAt((dataGridView1.RowCount - 1));
                    MessageBox.Show("Paste Successful!");
                }
            }
        }

        private void addUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
