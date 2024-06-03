using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Suppliers : Form
    {
        [Obsolete]
        public Suppliers()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        readonly Supplier sp = new Supplier();
        
        public void SupplierDataLoad()
        {
            GetID();
            SupplierData.DataSource = db.GetData("Select * from Supplier");
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {
            SupplierDataLoad();
            SupplierData.Columns[0].Visible = false;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        
        
        private void Txt_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_address.Focus();
            }
        }

        private void Txt_address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_email.Focus();
            }
        }

        private void Txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_phone.Focus();
            }
        }

        private void Txt_phone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_save.Focus();
            }
        }

        public void GetID()
        {
            try
            {
                string usid;
                string query = "select SUPID from Supplier order by SUPID desc";
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
                txt_supid.Text = usid.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SupplierData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_supid.Text = SupplierData.CurrentRow.Cells["SUPID"].Value.ToString();
            txt_supplier.Text = SupplierData.CurrentRow.Cells["SUPPLIER"].Value.ToString();
            txt_address.Text = SupplierData.CurrentRow.Cells["ADDRESS"].Value.ToString();
            txt_email.Text = SupplierData.CurrentRow.Cells["EMAIL"].Value.ToString();
            txt_phone.Text = SupplierData.CurrentRow.Cells["PHONE"].Value.ToString();
        }

        [Obsolete]
        private void Guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }

        private void Btn_update_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txt_supplier.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_address.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Address cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!Regex.IsMatch(txt_email.Text, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
                {

                    MessageBox.Show("Please Enter the Gmail in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!Regex.IsMatch(txt_phone.Text, @"^7|0|(?:\+94)[0-9]{9,10}$"))
                {
                    MessageBox.Show("Please Enter Contact Number in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int z = Convert.ToInt32(txt_phone.Text);
                int v = Convert.ToInt32(txt_supid.Text);
                int i = sp.SupplierUpdate(txt_supplier.Text, txt_address.Text, txt_email.Text, z, v);
                SupplierDataLoad();
                if (i == 1)
                {
                    MessageBox.Show("Data Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Data cannot be Updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Phone Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_supplier.Clear();
            txt_address.Clear();
            txt_email.Clear();
            txt_phone.Clear();
        }

        private void Btn_delete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txt_supplier.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txt_address.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Address cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!Regex.IsMatch(txt_email.Text, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
                {

                    MessageBox.Show("Please Enter the Gmail in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!Regex.IsMatch(txt_phone.Text, @"^7|0|(?:\+94)[0-9]{9,10}$"))
                {
                    MessageBox.Show("Please Enter Contact Number in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int d = Convert.ToInt32(txt_supid.Text);
                int i = sp.SupplierDelete(d);
                SupplierDataLoad();
                if (i == 1)
                {
                    MessageBox.Show("Data Deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Data Cannot be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please check data again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_supplier.Clear();
            txt_address.Clear();
            txt_email.Clear();
            txt_phone.Clear();
        }

        private void Btn_save_Click_1(object sender, EventArgs e)
        {
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Supplier WHERE SUPPLIER = '" + txt_supplier.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet ds = new DataSet();
                db.da.Fill(ds);
                int s = ds.Tables[0].Rows.Count;
                if (s > 0)
                {
                    MessageBox.Show("Supplier : " + txt_supplier.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please try again", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Supplier WHERE ADDRESS = '" + txt_address.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet dss = new DataSet();
                db.da.Fill(dss);
                int v = dss.Tables[0].Rows.Count;
                if (v > 0)
                {
                    MessageBox.Show("Address : " + txt_address.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please try again", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Supplier WHERE EMAIL = '" + txt_email.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet dsss = new DataSet();
                db.da.Fill(dsss);
                int h = dsss.Tables[0].Rows.Count;
                if (h > 0)
                {
                    MessageBox.Show("Email : " + txt_email.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please try again", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Supplier WHERE PHONE = '" + txt_phone.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet dssss = new DataSet();
                db.da.Fill(dssss);
                int m = dssss.Tables[0].Rows.Count;
                if (m > 0)
                {
                    MessageBox.Show("Phone : " + txt_phone.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Invalid Phone Number", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (txt_supplier.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_address.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Address cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Regex.IsMatch(txt_email.Text, @"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$"))
            {

                MessageBox.Show("Please Enter the Gmail in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Regex.IsMatch(txt_phone.Text, @"^7|0|(?:\+94)[0-9]{9,10}$"))
            {
                MessageBox.Show("Please Enter Contact Number in Correct Way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int b = Convert.ToInt32(txt_supid.Text);
                int o = Convert.ToInt32(txt_phone.Text);
                int i = sp.SupplierInsert(b, txt_supplier.Text, txt_address.Text, txt_email.Text, o);
                if (i == 1)
                {
                    MessageBox.Show("Data saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Data Cannot be Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Cannot be Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            try
            {
                SupplierDataLoad();
            }

            catch (Exception)
            {
                MessageBox.Show("Please check data again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_supplier.Clear();
            txt_address.Clear();
            txt_email.Clear();
            txt_phone.Clear();
        }

        private void Txt_supplier_TextChanged(object sender, EventArgs e)
        {
            if ((txt_supplier.Text.Trim().Equals(string.Empty)))
            {
                GetID();
            }
        }

        private void Txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
