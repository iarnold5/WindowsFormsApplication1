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
    public partial class updateApps : Form
    {
        openCode ocode = new openCode();
        MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();
        string teamname;
        string tablename;
        
        DataGridView dataGridView4 = new DataGridView();

        DataGridViewTextBoxColumn tblid = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn tblnm = new System.Windows.Forms.DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn tblloc = new System.Windows.Forms.DataGridViewTextBoxColumn();

        public updateApps()
        {
            InitializeComponent();

            connString = ((openCode)ocode).connString;
            teamname = ((openCode)ocode).teamname;

            ocode.AutoLoadTeamID();
        }

        private void updateApps_Load(object sender, EventArgs e)
        {
            comboBox3.Items.AddRange(ocode.teamslist.ToArray());

            tablenameset();
        }

        private void tablenameset()
        {
            if (this.Text == "Update Apps...")
            {
                tablename = "App";
            }
            else if (this.Text == "Update URLs...")
            {
                tablename = "URL";
            }

            label2.Text = tablename + "s";
            label5.Text = "Add, update or remove " + tablename + " as required.";
        }

        private void initgrid4()
        {
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();

            dataGridView4.AutoGenerateColumns = false;

            dataGridView4.AllowUserToDeleteRows = true;

            this.tableLayoutPanel9.Controls.Add(this.dataGridView4, 2, 2);

            // 
            // tblid
            // 
            this.tblid.HeaderText = "ID (Readonly)";
            this.tblid.Name = "tblid";
            this.tblid.ReadOnly = true;
            this.tblid.Visible = false; 
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
            this.tableLayoutPanel9.SetColumnSpan(this.dataGridView4, 1);
            ocode.addcolifnotthere(dataGridView4, tblloc);
            ocode.addcolifnotthere(dataGridView4, tblnm);
            ocode.addcolifnotthere(dataGridView4, tblid);
            this.dataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView4.Location = new System.Drawing.Point(3, 36);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.Size = new System.Drawing.Size(982, 258);
            this.dataGridView4.TabIndex = 5;
            //this.dataGridView4.RowsRemoved += new DataGridViewRowsRemovedEventHandler(this.dataGridView4_RowsRemoved);
            this.dataGridView4.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView4_UserDeletingRow);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("Please ensure a team is selected.", "Selection Required");
            }
            else
            {
                ocode.Clear_All_Text(tableLayoutPanel9);

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

        private void dataGridView4_RowsRemoved(DataGridViewRow row)
        {
            try
            {
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
                dataGridView4.Update();
            }
            catch
            {

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
                                name = row["app_name"].ToString();

                                try
                                {
                                    MySqlDataAdapter da = new MySqlDataAdapter();
                                    DataTable dt = new DataTable();

                                    string ss = "SELECT * FROM app WHERE app_name='" + name + "'";
                                    da.SelectCommand = new MySqlCommand(ss, conn);
                                    da.Fill(dt);

                                    if (dt.Rows.Count == 0)
                                    {
                                        string s = "";
                                        s = "INSERT INTO app(appid, app_name, app_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE app_name = @name, app_loc = @loc";
                                        ocode.Execute_Query(s, row);

                                        string q = "";
                                        q = "INSERT INTO team_app(team_id, app_appid) VALUES ('" + teamname + "',(SELECT appid FROM app WHERE app_name='" + name + "')) ON DUPLICATE KEY UPDATE team_id='" + teamname + "', app_appid=(SELECT appid FROM app WHERE app_name='" + name + "')";
                                        ocode.Execute_Query(q, row);
                                    }
                                    else if (dt.Rows.Count == 1)
                                    {
                                        DialogResult dx = MessageBox.Show("Click 'OK' if this is the App you wanted to add to " + teamname + "." + "\n\n" + dt.Rows[0].ItemArray[2].ToString() + " - " + dt.Rows[0].ItemArray[2].ToString() + "\n\n" + "This is done to prevent duplication of Apps in the database. If you want this App to be added as it is, please rename the App and try again." + "\n\n" + "Click 'Cancel' to skip this app and try again.", "Confirmation", MessageBoxButtons.OKCancel);

                                        if (dx == DialogResult.OK)
                                        {
                                            string q = "";
                                            q = "INSERT INTO team_app(team_id, app_appid) VALUES ('" + teamname + "'," + dt.Rows[0].ItemArray[0].ToString() + ") ON DUPLICATE KEY UPDATE team_id='" + teamname + "', app_appid=" + dt.Rows[0].ItemArray[0].ToString() + "";
                                            ocode.Execute_Query(q, row);
                                        }
                                        else if (dx == DialogResult.Cancel)
                                        {
                                            continue;
                                        }
                                    }
                                    else if (dt.Rows.Count > 1)
                                    {
                                        MessageBox.Show("Multiple entries found. Try to change the name of app and try again.", "Duplicate entries");
                                    }
                                }
                                catch (Exception x)
                                {
                                    MessageBox.Show(x.Message);
                                }
                            }
                            MessageBox.Show("Apps Updated!!", "Success");
                        }
                        else if (tablename == "URL")
                        {
                            foreach (DataRow row in changestable.Rows)
                            {
                                name = row["url_name"].ToString();

                                try
                                {
                                    MySqlDataAdapter da = new MySqlDataAdapter();
                                    DataTable dt = new DataTable();

                                    string ss = "SELECT * FROM url WHERE url_name='" + name + "'";
                                    da.SelectCommand = new MySqlCommand(ss, conn);
                                    da.Fill(dt);

                                    if (dt.Rows.Count == 0)
                                    {
                                        string s = "";
                                        s = "INSERT INTO url(urlid, url_name, url_loc) VALUES(@id, @name, @loc) ON DUPLICATE KEY UPDATE url_name = @name, url_loc = @loc";
                                        ocode.Execute_Query(s, row);

                                        string q = "";
                                        q = "INSERT INTO team_url(team_id, url_urlid) VALUES ('" + teamname + "',(SELECT urlid FROM url WHERE url_name='" + name + "')) ON DUPLICATE KEY UPDATE team_id='" + teamname + "', url_urlid=(SELECT urlid FROM url WHERE url_name='" + name + "')";
                                        ocode.Execute_Query(q, row);
                                    }
                                    else if (dt.Rows.Count == 1)
                                    {
                                        DialogResult dx = MessageBox.Show("Click 'OK' if this is the URL you wanted to add to " + teamname + "." + "\n\n" + dt.Rows[0].ItemArray[2].ToString() + " - " + dt.Rows[0].ItemArray[2].ToString() + "\n\n" + "This is done to prevent duplication of URLs in the database. If you want this URL to be added as it is, please rename the URL and try again." + "\n\n" + "Click 'Cancel' to skip this URL and try again.", "Confirmation", MessageBoxButtons.OKCancel);

                                        if (dx == DialogResult.Yes)
                                        {
                                            string q = "";
                                            q = "INSERT INTO team_url(team_id, url_urlid) VALUES ('" + teamname + "'," + dt.Rows[0].ItemArray[0].ToString() + ") ON DUPLICATE KEY UPDATE team_id='" + teamname + "', url_urlid=" + dt.Rows[0].ItemArray[0].ToString() + "";
                                            ocode.Execute_Query(q, row);
                                        }
                                        else if (dx == DialogResult.Cancel)
                                        {
                                            continue;
                                        }
                                    }
                                    else if (dt.Rows.Count > 1)
                                    {
                                        MessageBox.Show("Multiple entries found. Try to change the name of app and try again.", "Duplicate entries");
                                    }
                                }
                                catch (Exception x)
                                {
                                    MessageBox.Show(x.Message);
                                }
                            }
                            MessageBox.Show("URLs Updated!!", "Success");
                        }

                        ocode.Clear_All_Text(tableLayoutPanel9);
                    }
                    else
                    {
                        MessageBox.Show("No updates found.", "Info");
                    }
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                teamname = comboBox3.Text;
                label4.Text = teamname;
                button9_Click(sender, e);
            }
            else
            {
                label4.Text = teamname;
            }
        }
    }
}
