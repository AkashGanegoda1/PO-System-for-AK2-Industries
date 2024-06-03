namespace Purchasing_Order
{
    class Purchase_Ordering
    {
        readonly Database db = new Database();
        public void InsertPO(string a)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(a, db.con);
            db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
        }
        public int InsertPOSave(string q)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(q, db.con);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;
        }
        public void DeletePO(string d)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(d, db.con);
            db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
        }
    }
}
    

