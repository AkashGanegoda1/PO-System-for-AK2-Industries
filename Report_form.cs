using System;
using System.Data;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Report_form : Form
    {
        [Obsolete]
        public Report_form()
        {
            InitializeComponent();
        }
        readonly Database db = new Database();
        
        private void Report_form_Load(object sender, EventArgs e)
        {
            Ponumview.DataSource = db.GetData("Select distinct PO_NUMBER,SUPPLIER from PurchaseOsave");
            Ponumview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }
        
        private void Ponumview_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txt_ponum.Text = Ponumview.CurrentRow.Cells["PO_NUMBER"].Value.ToString();
        }
        private void Txt_ponum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        [Obsolete]
        private void Guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home h = new Home();
            h.Show();
        }

        [Obsolete]
        private void Btn_View_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txt_ponum.Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("Please type the PO Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string query = "Select * from PurchaseOsave where PO_NUMBER = '" + txt_ponum.Text + "'";
                db.da = new System.Data.SqlClient.SqlDataAdapter(query, db.con);
                db.dt = new System.Data.DataTable();
                db.da.Fill(db.dt);
                if (db.dt.Rows.Count == 0)
                {
                    MessageBox.Show("Please Enter Correct PO Number!", "Incorrect input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    this.Hide();
                    Report fp = new Report();
                    fp.Show();
                    if (db.con.State != ConnectionState.Open)
                    {
                        db.con.Open();
                    }
                    db.cmd = new System.Data.SqlClient.SqlCommand("select * from PurchaseOsave where PO_NUMBER = '" + txt_ponum.Text + "'", db.con);
                    db.da = new System.Data.SqlClient.SqlDataAdapter(db.cmd);
                    DataSet ds = new DataSet();
                    db.da.Fill(ds, "PurchaseOsave");
                    PurchaseOrderReport cr1 = new PurchaseOrderReport();
                    cr1.SetDataSource(ds);
                    fp.crystalReportViewer1.ReportSource = cr1;
                    fp.crystalReportViewer1.Refresh();
                    db.con.Close();
                    db.cmd.Dispose();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Only Integers Are Allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
