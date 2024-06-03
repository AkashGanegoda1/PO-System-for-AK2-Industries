using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

namespace Purchasing_Order
{
    public partial class Signup : Form
    {
        [Obsolete]
        public Signup()
        {
            InitializeComponent();

        }
        readonly Database db = new Database();
        readonly Register rg = new Register();
        private void Signup_Load(object sender, EventArgs e)
        {
            txt_username.Enabled = false;
            txt_id.Enabled = false;
            guna2Panel1.Visible = true;
            label8.Visible = false;
            guna2HtmlLabel1.Visible = false;
            guna2HtmlLabel2.Visible = false;
            guna2HtmlLabel3.Visible = false;
            guna2HtmlLabel4.Visible = false;
            guna2HtmlLabel5.Visible = false;
            Pictureprofile.Visible = false;
            txt_gmail.Visible = false;
            txt_password.Visible = false;
            guna2ControlBox1.Visible = false;
            guna2ControlBox2.Visible = false;
            txt_id.Visible = false;
            dateTimePicker1.Visible = false;
            btn_signup.Visible = false;
            guna2Button2.Visible = false;
            txt_username.Visible = false;
            guna2ToggleSwitch1.Visible = false;
            guna2PictureBox1.Visible = false;

            dateTimePicker1.Text = DateTime.Today.ToString();
            GetID();
            GetUsername();
            txt_password.UseSystemPasswordChar = true;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }
        public void GetID()
        {
            try
            {
                string usid;
                string query = "select uid from Registration order by uid desc";
                db.con.Open();
                db.cmd = new System.Data.SqlClient.SqlCommand(query, db.con);
                db.dr = db.cmd.ExecuteReader();
                if (db.dr.Read())
                {
                    int id = int.Parse(db.dr[0].ToString()) + 1;
                    usid = id.ToString("00000");
                }
                else if (Convert.IsDBNull(db.dr))
                {
                    usid = ("00001");
                }
                else
                {
                    usid = ("00001");
                }
                db.con.Close();
                db.cmd.Dispose();
                txt_id.Text = usid.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_pass.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Enter the Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                db.con.Open();
                db.da = new System.Data.SqlClient.SqlDataAdapter("Select Count(*) from SignupAccess where Password='" + txt_pass.Text + "'", db.con);
                DataTable dt = new DataTable();
                db.da.Fill(dt);
                db.con.Close();
                db.cmd.Dispose();
                if (dt.Rows[0][0].ToString() == "1")
                {
                    guna2Panel1.Visible = false;
                    label8.Visible = true;
                    guna2HtmlLabel1.Visible = true;
                    guna2HtmlLabel2.Visible = true;
                    guna2HtmlLabel3.Visible = true;
                    guna2HtmlLabel4.Visible = true;
                    guna2HtmlLabel5.Visible = true;
                    txt_gmail.Visible = true;
                    txt_password.Visible = true;
                    guna2ControlBox1.Visible = true;
                    guna2ControlBox2.Visible = true;
                    txt_id.Visible = true;
                    dateTimePicker1.Visible = true;
                    btn_signup.Visible = true;
                    guna2Button2.Visible = true;
                    txt_username.Visible = true;
                    guna2ToggleSwitch1.Visible = true;
                    guna2PictureBox1.Visible = true;
                }
                else
                {
                    MessageBox.Show("Incorrect Admin Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error ,Contact admin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bunifuButton2.Focus();
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
        private void Txt_gmail_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_password.Focus();
            }
        }

        private void Txt_username_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_password.Focus();
            }
        }

        private void Txt_password_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_signup.Focus();
            }
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.ShowDialog();
        }

        private void Guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Btn_signup_Click(object sender, EventArgs e)
        {
            db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Registration WHERE username = '" + txt_username.Text + "'", db.con);
            db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
            DataSet ds = new DataSet();
            db.da.Fill(ds);
            int s = ds.Tables[0].Rows.Count;
            if (s > 0)
            {
                MessageBox.Show("Username : " + txt_username.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Regex.IsMatch(txt_gmail.Text, @"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$"))
            {
                MessageBox.Show("Please Enter the Gmail in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Registration WHERE gmail = '" + txt_gmail.Text + "'", db.con);
            db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
            DataSet ds1 = new DataSet();
            db.da.Fill(ds1);
            int f = ds1.Tables[0].Rows.Count;
            if (f > 0)
            {
                MessageBox.Show("Gmail : " + txt_gmail.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_username.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter the Username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Regex.IsMatch(txt_password.Text, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,12}$"))
            {
                MessageBox.Show("Please Enter the Password in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {


                MemoryStream stream = new MemoryStream();
                Pictureprofile.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Profilepic = stream.ToArray();

                int i = Convert.ToInt32(txt_id.Text);

                int a = rg.Registering(dateTimePicker1.Text, i, txt_gmail.Text, txt_username.Text, txt_password.Text, Profilepic);
                if (a == 1)
                {
                    MessageBox.Show("Username: " + txt_username.Text + " Registered Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Register Unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            dateTimePicker1.ResetText();
            txt_gmail.Clear();
            txt_password.Clear();
            GetID();
            GetUsername();
        }

        [Obsolete]
        private void BunifuButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.Show();
        }

        [Obsolete]
        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.Show();
        }
        readonly string userid = "AK2OFCS-";
        public void GetUsername()
        {
            try
            {
                db.con.Open();
                db.cmd = new System.Data.SqlClient.SqlCommand("Select Count(username) from Registration", db.con);
                int i = Convert.ToInt32(db.cmd.ExecuteScalar());
                db.con.Close();
                i++;
                txt_username.Text = userid + i.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
        
    

