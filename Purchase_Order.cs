using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;

namespace Purchasing_Order
{
    public partial class Purchase_Order : Form
    {
        [Obsolete]
        public Purchase_Order()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        readonly Purchase_Ordering po = new Purchase_Ordering();
        private void Purchase_Order_Load(object sender, EventArgs e)
        {
            purchasingview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            lbl_date.Text = DateTime.Now.ToString("yyyy/MM/dd");
            GetID();
            try
            {
                db.con.Open();
                db.da = new System.Data.SqlClient.SqlDataAdapter("Select UNIT_PRICE,QUANTITY,TOTAL_AMOUNT from Purchase_Order", db.con);
                DataTable dttt = new DataTable();
                db.da.Fill(dttt);
                db.con.Close();
                db.cmd.Dispose();
                purchasingview.DataSource = dttt;
            }
            catch (Exception)
            {
                MessageBox.Show("Please check data again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT SUPPLIER FROM Supplier", db.con);
                db.con.Open();
                db.dr = db.cmd.ExecuteReader();
                AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();
                while (db.dr.Read())
                {
                    mycollection.Add(db.dr.GetString(0));
                }
                txt_supplier.AutoCompleteCustomSource = mycollection;
                db.con.Close();
                db.cmd.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }
        public void GetID()
        {
            try
            {
                string usid;
                string query = "select PO_NUMBER from PurchaseOsave WHERE DATEON = '" + lbl_date.Text + "'ORDER BY PO_NUMBER DESC";
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
                    usid = DateTime.Now.ToString("yyMd1");
                }
                else
                {
                    usid = DateTime.Now.ToString("yyMd1");
                }
                db.con.Close();
                db.cmd.Dispose();
                lbl_ponum.Text = usid.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void Txt_vat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_pm.Focus();
            }
        }
        private void Txt_pm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_ts.Focus();
            }
        }
        private void Txt_ts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_dt.Focus();
            }
        }
        private void Txt_dt_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_addnew.Focus();
            }
        }
        private void Txt_vat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        
       
        private void Purchasingview_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in purchasingview.Rows)
                {
                    row.Cells[3].Value = Convert.ToDouble(row.Cells[1].Value) * Convert.ToDouble(row.Cells[2].Value);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not Accessible", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            try
            {
                string query3 = "delete from Purchase_Order";
                po.DeletePO(query3);
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Hide();
            Home h = new Home();
            h.Show();
        }

        private void Txt_supplier_TextChanged(object sender, EventArgs e)
        {
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("Select * from Supplier where SUPPLIER='" + txt_supplier.Text + "'", db.con);
                db.con.Open();
                db.cmd.ExecuteNonQuery();
                db.dr = db.cmd.ExecuteReader();
                while (db.dr.Read())
                {
                    string ADDRESS = (string)db.dr["ADDRESS"].ToString();
                    txt_address.Text = ADDRESS;

                }
                db.con.Close();
                db.cmd.Dispose();

            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT Distinct ITEM FROM Supplier_Item WHERE SUPPLIER= '" + txt_supplier.Text + "'", db.con);
                db.con.Open();
                db.cmd.ExecuteNonQuery();
                db.dr = db.cmd.ExecuteReader();
                ITEM_DESCRIPTION.Items.Clear();
                while (db.dr.Read())
                {
                    string ITEM = (string)db.dr["ITEM"].ToString();
                    ITEM_DESCRIPTION.Items.Add(ITEM);

                }
                db.con.Close();
                db.cmd.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Txt_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_vat.Focus();
            }
        }
        private void Purchasingview_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = purchasingview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)purchasingview.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    ((DataGridViewComboBoxColumn)purchasingview.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }

        private void Btn_save_Click_1(object sender, EventArgs e)
        {
            if (txt_supplier.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string query = "Select * from Supplier where SUPPLIER = '" + txt_supplier.Text + "'";
            db.da = new System.Data.SqlClient.SqlDataAdapter(query, db.con);
            db.dt = new System.Data.DataTable();
            db.da.Fill(db.dt);
            if (db.dt.Rows.Count == 0)
            {
                MessageBox.Show("Supplier typed wasn't a Registered one!", "Unregistered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_vat.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter VAT value !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_pm.SelectedItem == null)
            {
                MessageBox.Show("Please Select Payment Mode !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_ts.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter Time Span !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_dt.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Please Enter Delivery Address !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (purchasingview.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Please Select Supplier Items!", "Please Fill them", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int x = 0; x < purchasingview.Rows.Count - 1; x++)
            {
                if (purchasingview.Rows[x].Cells[0].Value == DBNull.Value)
                {
                    MessageBox.Show("Please Select the Items!", "Please Fill them", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (purchasingview.Rows[x].Cells[1].Value == DBNull.Value)
                {
                    MessageBox.Show("Unit Price Cannot be Empty!", "Please Fill them", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (purchasingview.Rows[x].Cells[2].Value == DBNull.Value)
                {
                    MessageBox.Show("Quantity Cannot be Empty!", "Please Fill them", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (purchasingview.Rows[x].Cells[3].Value == DBNull.Value)
                    {
                        MessageBox.Show("Total Amount Cannot be Empty!", "Please Fill them", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            GetID();


            try
            {
                for (int x = 0; x < purchasingview.Rows.Count - 1; x++)
                {
                    string query1 = "Insert into Purchase_Order values ('" + purchasingview.Rows[x].Cells[0].Value + "','" + purchasingview.Rows[x].Cells[1].Value + "','" + purchasingview.Rows[x].Cells[2].Value + "','" + purchasingview.Rows[x].Cells[3].Value + "')";
                    po.InsertPO(query1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Data Cannot be Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            try
            {
                bool flag = true;
                for (int x = 0; x < purchasingview.Rows.Count - 1; x++)
                {
                    string query2 = "Insert into PurchaseOsave values ('" + lbl_ponum.Text + "','" + txt_supplier.Text + "','" + txt_address.Text + "','" + txt_vat.Text + "','" + lbl_date.Text + "','" + purchasingview.Rows[x].Cells[0].Value + "','" + purchasingview.Rows[x].Cells[1].Value + "','" + purchasingview.Rows[x].Cells[2].Value + "','" + purchasingview.Rows[x].Cells[3].Value + "','" + txt_pm.Text + "','" + txt_ts.Text + "','" + txt_dt.Text + "')";
                    int i = po.InsertPOSave(query2);
                    db.con.Close();
                }
                if (flag)
                {
                    MessageBox.Show("Data saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                flag = false;
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_supplier.Clear();
            txt_address.Clear();
            txt_dt.Clear();
            txt_pm.SelectedItem = null;
            txt_ts.Clear();
            txt_vat.Clear();
        }

        private void Btn_addnew_Click_1(object sender, EventArgs e)
        {
            lbl_date.Text = DateTime.Now.ToString("yyyy/MM/dd");
            GetID();
            try
            {
                string query3 = "delete from Purchase_Order";
                po.DeletePO(query3);
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                db.con.Open();
                db.da = new System.Data.SqlClient.SqlDataAdapter("Select UNIT_PRICE,QUANTITY,TOTAL_AMOUNT from Purchase_Order", db.con);
                DataTable dt = new DataTable();
                db.da.Fill(dt);
                db.con.Close();
                db.cmd.Dispose();
                purchasingview.DataSource = dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    } 
}
            
    

    


    
    

