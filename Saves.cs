using DataGridView_Merge_Cells;
using System;
using System.Data;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Saves : Form
    {
        [Obsolete]
        public Saves()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        private void Saves_Load(object sender, EventArgs e)
        {
            purchasingview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            purchasingview.DataSource =  db.GetData("Select * from PurchaseOsave");
            purchasingview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            var col = new DataGridViewMergedTextBoxColumn();
            const string field = "PO_NUMBER";
            col.HeaderText = field;
            col.Name = field;
            col.DataPropertyName = field;
            int colidx = purchasingview.Columns[field].Index;
            purchasingview.Columns.Remove(field);
            purchasingview.Columns.Insert(colidx, col);
        }

        private void Txt_filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void Txt_filter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                db.con.Open();
                db.cmd = new System.Data.SqlClient.SqlCommand("select * from PurchaseOsave where SUPPLIER like '%" + txt_filter.Text + "%'", db.con);
                db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                DataTable dt = new DataTable();
                db.da.Fill(dt);
                purchasingview.DataSource = dt;
                db.con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_filter_Click_1(object sender, EventArgs e)
        {
            try
            {
                db.con.Open();
                db.da = new System.Data.SqlClient.SqlDataAdapter("select * from PurchaseOsave where DATEON between '" + DatePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + DatePicker2.Value.ToString("yyyy-MM-dd") + "'", db.con);
                db.con.Close();
                DataTable dt = new DataTable();
                db.da.Fill(dt);
                purchasingview.DataSource = dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_clear_Click_1(object sender, EventArgs e)
        {
            purchasingview.DataSource = db.GetData("Select * from PurchaseOsave");
        }
    }
}
