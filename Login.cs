using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Purchasing_Order
{
    public partial class Login : Form
    {
        [Obsolete]
        public Login()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        public static string Cmbval
        {
            get; set;
        }
        private int count = 1;
        private void Slider()
        {
            if (count == 5)
            {
                count = 1;
            }
            guna2PictureBox1.ImageLocation = string.Format(@"Img\{0}.jpg", count);
            count++;
        }
        private void Login_Load(object sender, EventArgs e)
        {
            txt_password.UseSystemPasswordChar = true;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        [Obsolete]
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Account_Recovery ar = new Account_Recovery();
            ar.Show();
        }

        [Obsolete]
        private void Btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_userid.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Enter the Username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!Regex.IsMatch(txt_password.Text, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,12}$"))
                {
                    MessageBox.Show("Please Enter the Correct Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = "Select * from Registration where username = '" + txt_userid.Text + "' and password = '" + txt_password.Text + "'";
                db.da = new System.Data.SqlClient.SqlDataAdapter(query, db.con);
                db.dt = new System.Data.DataTable();
                db.da.Fill(db.dt);
                if (db.dt.Rows.Count == 1)
                {

                    Cmbval = txt_userid.Text;
                    this.Hide();
                    Home h = new Home();
                    h.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Password for the selected user account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_login.Focus();
            }
        }

        private void Guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                txt_password.UseSystemPasswordChar = false;
            }
            else
            {
                txt_password.UseSystemPasswordChar = true;
            }
        }

        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            db.con.Dispose();
        }

        private void Guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [Obsolete]
        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Signup s = new Signup();
            s.Show();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Slider();
        }

        private void Txt_userid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_password.Focus();
            }
        }
    }
}
