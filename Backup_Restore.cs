using System;
using System.Windows.Forms;


namespace Purchasing_Order
{
    public partial class Backup_Restore : Form
    {
        public Backup_Restore()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        string connectionString = "";
        string sql = "";

        private void Backup_Restore_Load(object sender, EventArgs e)
        {
            
            btn_browserestore.Enabled = false;
            btn_browsebckup.Enabled = false;
            txt_bckuppath.Enabled = false;
            txt_restorepath.Enabled = false;
            btn_disconnect.Enabled = false;
            btn_backup.Enabled = false;
            btn_restore.Enabled = false;
            txt_source.ReadOnly = true;
            cmb_databases.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_databases.Enabled = false;
            txt_source.Text = System.Net.Dns.GetHostName().ToUpper();

        }

        private void Btn_disconnect_Click_1(object sender, EventArgs e)
        {
            btn_disconnect.Enabled = false;
            txt_source.Enabled = true;
            txt_source.ReadOnly = true;
            btn_connect.Enabled = true;
            cmb_databases.ResetText();
            cmb_databases.Enabled = false;
            btn_backup.Enabled = false;
            btn_restore.Enabled = false;
            txt_bckuppath.Enabled = false;
            txt_restorepath.Enabled = false;
            btn_browsebckup.Enabled = false;
            btn_browserestore.Enabled = false;
            txt_bckuppath.Clear();
            txt_restorepath.Clear();
        }

        private void Btn_connect_Click_1(object sender, EventArgs e)
        {
            try
            {
                connectionString = "Data Source = " + txt_source.Text.Trim() + "; Trusted_Connection = True;";
                db.con = new System.Data.SqlClient.SqlConnection(connectionString);
                db.con.Open();
                sql = "SELECT * FROM sys.databases d where d.name = 'AK2'";
                db.cmd = new System.Data.SqlClient.SqlCommand(sql, db.con);
                db.dr = db.cmd.ExecuteReader();
                cmb_databases.Items.Clear();
                while (db.dr.Read())
                {
                    cmb_databases.Items.Add(db.dr[0].ToString());
                }
                txt_source.Enabled = false;
                btn_connect.Enabled = false;
                cmb_databases.Enabled = true;
                btn_disconnect.Enabled = true;
                
            }
            catch(Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Btn_browsebckup_Click_1(object sender, EventArgs e)
        {
           if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
           {
              txt_bckuppath.Text = folderBrowserDialog1.SelectedPath;
           }
        }

        private void Btn_backup_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cmb_databases.SelectedItem == null)
                {
                    MessageBox.Show("Please Select the Database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_bckuppath.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Choose the Backup Path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.con = new System.Data.SqlClient.SqlConnection(connectionString);
                db.con.Open();
                sql = "BACKUP DATABASE " + cmb_databases.Text + " TO DISK = '" + txt_bckuppath.Text.Trim() + "\\" + cmb_databases.Text + "-" + DateTime.Now.ToString("dddd, dd MMMM yyyy HH-mm-ss") + ".bak'";
                db.cmd = new System.Data.SqlClient.SqlCommand(sql, db.con);
                db.cmd.ExecuteNonQuery();
                db.con.Close();
                db.con.Dispose();
                MessageBox.Show("Database Backup Successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_browserestore_Click_1(object sender, EventArgs e)
        {
            if (txt_restorepath.Text != @"C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Backup")
                openFileDialog1.InitialDirectory = txt_restorepath.Text;
            openFileDialog1.Filter = "Backup Files(*.bak)|*.bak|All Files(*.*)| *.*";
            openFileDialog1.FilterIndex = 0;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_restorepath.Text = openFileDialog1.FileName;
            }
        }

        private void Btn_restore_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cmb_databases.SelectedItem == null)
                {
                    MessageBox.Show("Please Select the Database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_restorepath.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Choose the Restore Path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.con = new System.Data.SqlClient.SqlConnection(connectionString);
                db.con.Open();
                sql = "ALTER DATABASE " + cmb_databases.Text + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                sql += "RESTORE DATABASE " + cmb_databases.Text + " FROM DISK = '" + txt_restorepath.Text.Trim() + "' WITH REPLACE;";
                db.cmd = new System.Data.SqlClient.SqlCommand(sql, db.con);
                db.cmd.ExecuteNonQuery();
                db.con.Close();
                db.con.Dispose();
                MessageBox.Show("Data Restored Successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cmb_databases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_databases.SelectedIndex == 0)
            {
                btn_restore.Enabled = true;
                btn_backup.Enabled = true;
                txt_restorepath.Enabled = true;
                txt_bckuppath.Enabled = true;
                btn_browsebckup.Enabled = true;
                btn_browserestore.Enabled = true;
            }
            else
            {
                txt_bckuppath.Clear();
                txt_restorepath.Clear();
                btn_restore.Enabled = false;
                btn_backup.Enabled = false;
                txt_restorepath.Enabled = false;
                txt_bckuppath.Enabled = false;
                btn_browsebckup.Enabled = false;
                btn_browserestore.Enabled = false;
            }
        }
    }
}
    
    

