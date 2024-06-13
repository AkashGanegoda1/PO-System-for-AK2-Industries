using System;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Purchasing_Order
{
    public partial class Account_Recovery : Form
    {
        string randomCode;
        public static string to;

        [Obsolete]
        public Account_Recovery()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        
        public static string Cmbval
        {
            get; set;
        }
        private void Account_Recovery_Load(object sender, EventArgs e)
        {
            bunifuPanel2.Enabled = false;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.Show();
        }

        private void Btn_emailsent_Click_1(object sender, EventArgs e)
        {
            Cmbval = txt_email.Text;

            if (!Regex.IsMatch(txt_email.Text, @"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$"))
            {
                MessageBox.Show("Email Should Be Registered and Cannot be Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            db.con.Open();
            db.da = new System.Data.SqlClient.SqlDataAdapter("SELECT * from Registration where gmail = '" + txt_email.Text + "'", db.con);
            DataTable dt = new DataTable();
            db.da.Fill(dt);
            db.con.Close();
            if (dt.Rows.Count > 0)
            {
                Random rand = new Random();
                randomCode = (rand.Next(999999)).ToString();
                try
                {
                    MailMessage msg = new MailMessage
                    {
                        From = new MailAddress("")
                    };
                    msg.To.Add(txt_email.Text);
                    msg.Subject = "AK2 INDUSTRIES";
                    msg.Body = "Your One-Time Password (OTP) is " + randomCode;

                    SmtpClient smt = new SmtpClient
                    {
                        Host = "smtp.gmail.com"
                    };
                    NetworkCredential ntcd = new NetworkCredential
                    {
                        UserName = "",
                        Password = ""
                    };
                    smt.Credentials = ntcd;
                    smt.EnableSsl = true;
                    smt.Port = 587;
                    smt.Send(msg);
                    if (msg == null)
                    {
                        MessageBox.Show("Mail sent Unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Mail sent Successfully, Please Check Your Mail!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    txt_email.Clear();
                    bunifuPanel2.Enabled = true;
                    bunifuPanel1.Enabled = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Unsuccessfull Attempt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Email Should be Registered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Obsolete]
        private void Btn_otpverify_Click_1(object sender, EventArgs e)
        {
            if (txt_otp.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter the OTP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (randomCode == (txt_otp.Text).ToString())
            {
                to = txt_email.Text;
                Reset_Password obj = new Reset_Password();
                this.Hide();
                obj.Show();
            }
            else
            {
                MessageBox.Show("Please enter correct Verification Code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Txt_email_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_emailsent.Focus();
            }
        }

        private void Txt_otp_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_otpverify.Focus();
            }
        }

        private void Txt_otp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
