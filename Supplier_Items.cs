using DataGridView_Merge_Cells;
using System;
using System.Data;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Supplier_Items : Form
    {
        [Obsolete]
        public Supplier_Items()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        readonly Supplier supplier = new Supplier();
        private void Supplier_Items_Load(object sender, EventArgs e)
        {
            SupplierData.DataSource = db.GetData("Select * from Supplier_Item");
            SupplierData.Columns[0].Visible = false;
            

            var col = new DataGridViewMergedTextBoxColumn();
            const string field = "SUPPLIER";
            col.HeaderText = field;
            col.Name = field;
            col.DataPropertyName = field;
            int colidx = SupplierData.Columns[field].Index;
            SupplierData.Columns.Remove(field);
            SupplierData.Columns.Insert(colidx, col);

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
            SupplierData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GetID();
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }
        public void GetID()
        {
            try
            {
                string usid;
                string query = "select ITEMID from Supplier_Item order by ITEMID desc";
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
                txt_itemid.Text = usid.ToString();
                txt_itemid2.Text = usid.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void Txt_supplier_TextChanged(object sender, EventArgs e)
        {
            if((txt_supplier.Text.Trim().Equals(string.Empty)))
            {
                GetID();
            }
        }
        private void SupplierData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_itemid2.Text = SupplierData.CurrentRow.Cells["ITEMID"].Value.ToString();
            txt_supplier.Text = SupplierData.CurrentRow.Cells["SUPPLIER"].Value.ToString();
            txt_item.Text = SupplierData.CurrentRow.Cells["ITEM"].Value.ToString();
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
            else if (txt_item.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Item cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                db.cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM Supplier_Item WHERE ITEM = '" + txt_item.Text + "'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataSet dssss = new DataSet();
                db.da.Fill(dssss);
                int m = dssss.Tables[0].Rows.Count;
                if (m > 0)
                {
                    MessageBox.Show("ITEM : " + txt_item.Text + " Already Exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Please Enter Numbers Only", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string queryy = "Insert into Supplier_Item values ('" + txt_itemid.Text + "','" + txt_supplier.Text + "','" + txt_item.Text + "')";
            int i = supplier.InsertItem(queryy);
            if (i == 1)
            {
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Cannot be Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            GetID();
            SupplierData.DataSource = db.GetData("Select * from Supplier_Item");
        }

        private void Btn_update_Click_1(object sender, EventArgs e)
        {
            if (txt_supplier.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_item.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Item cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string queryy = "Update Supplier_Item set ITEM='" + txt_item.Text + "'where ITEMID='" + txt_itemid2.Text + "'";
            int i = supplier.UpdateItem(queryy);
            if (i == 1)
            {
                MessageBox.Show("Data Saved Updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Cannot be Updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SupplierData.DataSource = db.GetData("Select * from Supplier_Item");
            txt_supplier.Clear();
            txt_item.Clear();
        }

        private void Btn_delete_Click_1(object sender, EventArgs e)
        {
            if (txt_supplier.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Supplier cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txt_item.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Item cannot be empty !!", "Incomplete Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string queryy = "delete from Supplier_Item where ITEMID='" + txt_itemid2.Text + "'";
            int i = supplier.DeleteItem(queryy);
            if (i == 1)
            {
                MessageBox.Show("Data Seleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Cannot be Deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SupplierData.DataSource = db.GetData("Select * from Supplier_Item");
            txt_supplier.Clear();
            txt_item.Clear();
        }

        private void Txt_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_item.Focus();
            }
        }
    }
}

