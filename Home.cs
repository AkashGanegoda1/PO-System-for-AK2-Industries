using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using GMap.NET;

namespace Purchasing_Order
{
    public partial class Home : Form
    {
        readonly GMap.NET.WindowsForms.GMapControl gmap;

        [Obsolete]
        public Home()
        {
            InitializeComponent();
            gmap = new GMap.NET.WindowsForms.GMapControl
            {
                MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap,
                Dock = DockStyle.Fill
            };
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.ShowCenter = false;
            gmap.MinZoom = 1;
            gmap.MaxZoom = 20;
            bunifuGradientPanel1.Controls.Add(gmap);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = Login.Cmbval;
            gmap.Position = new PointLatLng(Convert.ToDouble(6.868906833672648), Convert.ToDouble(79.91390269697152));
            gmap.Zoom = 3;
            gmap.Update();
            gmap.Refresh();
            gmap.DragButton = MouseButtons.Left;
            var markersOverlay = new GMap.NET.WindowsForms.GMapOverlay("marker1");
            var marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new PointLatLng(Convert.ToDouble(6.868906833672648), Convert.ToDouble(79.91390269697152)),
            GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red_small);
            markersOverlay.Markers.Add(marker);
            gmap.Overlays.Add(markersOverlay);

            try
            {
                string query1 = "SELECT COUNT(PO_NUMBER) FROM PurchaseOsave";
                db.con.Open();
                db.cmd = new System.Data.SqlClient.SqlCommand(query1, db.con);
                Int32 rowscount = Convert.ToInt32(db.cmd.ExecuteScalar());
                db.cmd.Dispose();
                db.con.Close();

                label3.Text = rowscount.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                string query11 = "SELECT COUNT(SUPPLIER) FROM Supplier";
                db.con.Open();
                db.cmd = new System.Data.SqlClient.SqlCommand(query11, db.con);
                Int32 rowscount = Convert.ToInt32(db.cmd.ExecuteScalar());
                db.cmd.Dispose();
                db.con.Close();
                label9.Text = rowscount.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                db.con.Open();
                db.cmd = db.con.CreateCommand();
                db.cmd.CommandType = System.Data.CommandType.Text;
                db.cmd.CommandText = "Select EMAIL FROM Supplier";
                db.cmd.ExecuteNonQuery();
                db.dt = new System.Data.DataTable();
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                db.da.Fill(db.dt);
                foreach (DataRow dr in db.dt.Rows)
                {
                    txt_email.Items.Add(dr["EMAIL"].ToString());
                }
                db.con.Close();
                db.cmd.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bunifuTextBox2.Visible = false;
            lbltime.Text = DateTime.Today.ToLongDateString();
            label11.Text = DateTime.Today.ToLongDateString();
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        readonly Database db = new Database();
      
        private Form activeform = null;
        private void OpenChildForm(Form ChildForm)
        {
            activeform?.Close();
            activeform = ChildForm;
            ChildForm.TopLevel = false;
            guna2Panel1.Controls.Add(ChildForm);
            guna2Panel1.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenChildForm2(Form ChildForm)
        {
            activeform?.Close();
            activeform = ChildForm;
            ChildForm.TopLevel = false;
            guna2Panel2.Controls.Add(ChildForm);
            guna2Panel2.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenChildForm3(Form ChildForm)
        {
            activeform?.Close();
            activeform = ChildForm;
            ChildForm.TopLevel = false;
            guna2Panel3.Controls.Add(ChildForm);
            guna2Panel3.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.ShowDialog();
            foreach (string filename in openFileDialog1.FileNames)
            {
                bunifuTextBox2.Visible = true;
                bunifuTextBox2.Text = filename.ToString();
            }
        }

        private void Label2_TextChanged_1(object sender, EventArgs e)
        {
            db.cmd = new System.Data.SqlClient.SqlCommand("SELECT Profilepic from Registration WHERE username ='" + label2.Text + "'", db.con);
            db.con.Open();
            db.cmd.ExecuteNonQuery();
            db.dr = db.cmd.ExecuteReader();
            if (db.dr.Read())
            {
                byte[] img = (byte[])(db.dr[0]);
                MemoryStream ms = new MemoryStream(img);
                bunifuPictureBox1.Image = Image.FromStream(ms);
            }
            else
            {
                bunifuPictureBox1 = null;
            }
            db.con.Close();
            db.cmd.Dispose();
        }
   
        private void Txt_subject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_body.Focus();
            }
        }

        private void Txt_body_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                linkLabel1.Focus();
            }
        }

        private void Btn_emailsent_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txt_email.SelectedItem == null)
                {
                    MessageBox.Show("Please Select a Gmail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_subject.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Enter the Subject!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_body.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please Enter the Content!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                db.con.Open();
                db.da = new System.Data.SqlClient.SqlDataAdapter("SELECT * from Supplier where EMAIL = '" + txt_email.Text + "'", db.con);
                DataTable dt = new DataTable();
                db.da.Fill(dt);
                db.con.Close();
                db.cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    MailMessage msg = new MailMessage
                    {
                        From = new MailAddress("ak2.industries.pvt.ltd@gmail.com")
                    };
                    msg.To.Add(txt_email.Text);
                    msg.Subject = txt_subject.Text;
                    msg.Body = txt_body.Text;
                    foreach (string filename in openFileDialog1.FileNames)
                    {
                        if(string.IsNullOrEmpty(bunifuTextBox2.Text))
                        {
                           //No Attachment Will be Sent
                        }
                        else
                        {
                            if (File.Exists(filename))
                            {
                                string fname = Path.GetFileName(filename);
                                msg.Attachments.Add(new Attachment(filename));
                            }
                        }
                        
                    }
                    SmtpClient smt = new SmtpClient
                    {
                        Host = "smtp.gmail.com"
                    };
                    System.Net.NetworkCredential ntcd = new NetworkCredential
                    {
                        UserName = "ak2.industries.pvt.ltd@gmail.com",
                        Password = "kjukzicspqdnmmqn"
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
                        MessageBox.Show("Mail sent Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, Email does not Registered in Our System", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_email.SelectedItem = null;
            txt_subject.Clear();
            txt_body.Clear();
            bunifuTextBox2.Visible = false;
            bunifuTextBox2.Clear();
        }

        private void Btn_generate_Click_1(object sender, EventArgs e)
        {
            QRCoder.QRCodeGenerator QG = new QRCoder.QRCodeGenerator();
            var MyData = QG.CreateQrCode(txt_qr.Text, QRCoder.QRCodeGenerator.ECCLevel.H);
            var code = new QRCoder.QRCode(MyData);
            pictureBox2.Image = code.GetGraphic(100);
        }

        private void Btn_save_Click_1(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Save Files",
                    CheckPathExists = true,
                    DefaultExt = "jpeg",
                    Filter = "PNG|*.png|JPEG|*.jpeg",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                saveFileDialog1.ShowDialog();
                foreach (string filename in saveFileDialog1.FileNames)
                {
                    pictureBox2.Image.Save(filename);
                    if (filename == null)
                    {
                        MessageBox.Show("Save Unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File Not Found or Supported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("System Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Obsolete]
        private void Btn_purchaseorder_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Purchase_Order purchase = new Purchase_Order();
            purchase.Show();
        }

        [Obsolete]
        private void Btn_saves_Click_1(object sender, EventArgs e)
        {
            OpenChildForm3(new Saves()); 
        }

        [Obsolete]
        private void Btn_supplier_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Suppliers purliers = new Suppliers();
            purliers.Show();
        }

        [Obsolete]
        private void Btn_supplieritem_Click(object sender, EventArgs e)
        {
            OpenChildForm2(new Supplier_Items());
        }

        [Obsolete]
        private void Btn_report_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Report_form form = new Report_form();
            form.Show();
        }

        private void Guna2Button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Backup_Restore());
        }

        [Obsolete]
        private void Btn_settings_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Settings s = new Settings();
            s.Show();
        }

        [Obsolete]
        private void Guna2Button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.Show();
        }
    }
}
       
    

