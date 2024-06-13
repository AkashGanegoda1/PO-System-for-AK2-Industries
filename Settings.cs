using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Settings : Form
    {
        [Obsolete]
        public Settings()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        string randomCode;
        public static string to;
        private void Settings_Load(object sender, EventArgs e)
        {
            txt_username.Text = Login.Cmbval;
            txt_password.Enabled = false;
            txt_id.Enabled = false;
            txt_username.Enabled = false;
            txt_gmail.Enabled = false;
            bunifuPanel1.Visible = false;
            bunifuPanel2.Visible = false;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        [Obsolete]
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Account_Recovery ac = new Account_Recovery();
            ac.Show();
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home h = new Home();
            h.Show();
        }

        [Obsolete]
        private void Btn_update_Click_1(object sender, EventArgs e)
        {
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("Update Registration set Profilepic = @Profilepi where uid = '" + txt_id.Text + "'", db.con);
                MemoryStream stream = new MemoryStream();
                Pictureprofile.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Profilepi = stream.ToArray();
                db.cmd.Parameters.AddWithValue("@Profilepi", Profilepi);
                db.con.Open();
                int i = db.cmd.ExecuteNonQuery();
                db.con.Close();
                db.cmd.Dispose();
                if (i == 1)
                {
                    MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    Home h = new Home();
                    h.Show();
                }
                else
                {
                    MessageBox.Show("Update Unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Txt_username_TextChanged(object sender, EventArgs e)
        {
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT uid,gmail,password,Profilepic from Registration WHERE username ='" + txt_username.Text + "'", db.con);
                db.con.Open();
                db.cmd.ExecuteNonQuery();
                db.dr = db.cmd.ExecuteReader();
                while (db.dr.Read())
                {
                    txt_id.Text = db.dr[0].ToString();
                    txt_gmail.Text = db.dr[1].ToString();
                    txt_password.Text = db.dr[2].ToString();
                    byte[] img = (byte[])(db.dr[3]);
                    MemoryStream ms = new MemoryStream(img);
                    Pictureprofile.Image = Image.FromStream(ms);
                }
                db.con.Close();
                db.cmd.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Guna2GradientButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opendlg = new OpenFileDialog
                {
                    FileName = "",
                    Filter = "Supported Images|*.jpg;*.jpeg;*.png;"
                };
                if (opendlg.ShowDialog() == DialogResult.OK)
                {
                    Pictureprofile.Load(opendlg.FileName);
                }
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Error When Handling the Process! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Image Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Again! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Obsolete]
        private void Btn_otpverify_Click(object sender, EventArgs e)
        {
            if (txt_otp.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter the OTP!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (randomCode == (txt_otp.Text).ToString())
            {
                to = txt_gmail.Text;
                bunifuPanel1.Visible = false;
                bunifuPanel3.Visible = false;
                bunifuPanel2.Visible = true;
            }
            else
            {
                MessageBox.Show("Please enter correct Verification Code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();

            MailMessage msg = new MailMessage
            {
                From = new MailAddress("")
            };
            msg.To.Add(txt_gmail.Text);
            msg.Subject = "Verify Your Email!";
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

            bunifuLabel3.Visible = false;
            bunifuPanel2.Visible = false;
            bunifuPanel1.Visible = true;
            
        }

        [Obsolete]
        private void Btn_emailsent_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(txt_email.Text, @"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$"))
                {
                    MessageBox.Show(txt_email, "Please Enter the Gmail in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Registration WHERE gmail = '" + txt_email.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet ds = new DataSet();
                db.da.Fill(ds);
                int s = ds.Tables[0].Rows.Count;
                if (s > 0)
                {
                    MessageBox.Show("Gmail : " + txt_email.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    db.cmd = new System.Data.SqlClient.SqlCommand("Update Registration set gmail = '" + txt_email.Text + "' where uid = '" + txt_id.Text + "'", db.con);
                    db.con.Open();
                    int i = db.cmd.ExecuteNonQuery();
                    db.con.Close();
                    db.cmd.Dispose();
                    if (i == 1)
                    {
                        MessageBox.Show("Gmail Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();
                        Home h = new Home();
                        h.Show();
                    }
                    else
                    {
                        MessageBox.Show("Update Unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            
        
    


    

            
            
                
            
            
            
        
    

