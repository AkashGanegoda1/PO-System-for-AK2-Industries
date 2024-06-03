using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Reset_Password : Form
    {
        [Obsolete]
        public Reset_Password()
        {
            InitializeComponent();
        }
        private void Reset_Password_Load(object sender, EventArgs e)
        {
            txt_email.Text = Account_Recovery.Cmbval;
            txt_newpass.UseSystemPasswordChar = true;
            txt_cnewpass.UseSystemPasswordChar = true;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }
        
        readonly Database db = new Database();
        
        private void Guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                txt_newpass.UseSystemPasswordChar = false;
            }
            else
            {
                txt_newpass.UseSystemPasswordChar = true;
            }
        }

        private void Guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch2.Checked)
            {
                txt_cnewpass.UseSystemPasswordChar = false;
            }
            else
            {
                txt_cnewpass.UseSystemPasswordChar = true;
            }
        }

        private void Txt_newpass_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_cnewpass.Focus();
            }
        }

        private void Txt_cnewpass_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_reset.Focus();
            }
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.Show();
        }

        [Obsolete]
        private void Btn_reset_Click_1(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txt_newpass.Text, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,12}$"))
            {
                MessageBox.Show("Please Enter Your new Password in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Regex.IsMatch(txt_cnewpass.Text, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,12}$"))
            {
                MessageBox.Show("Please Confirm Your new Password in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_newpass.Text == txt_cnewpass.Text)
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("UPDATE[dbo].[Registration] SET [password] = '" + txt_cnewpass.Text + "'WHERE gmail ='" + txt_email.Text + "'", db.con);
                db.con.Open();
                int i = db.cmd.ExecuteNonQuery();
                db.con.Close();
                db.cmd.Dispose();
                if (i == 1)
                {
                    MessageBox.Show("Password Reset Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Login l = new Login();
                    l.Show();
                }
            }
            else
            {
                MessageBox.Show("Password do not match eachother!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
