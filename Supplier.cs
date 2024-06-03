namespace Purchasing_Order
{
    class Supplier
    {
        readonly Database db = new Database();

        public string SupID;
        public string Supplierd;
        public string Addressd;
        public string Gmail;
        public string Number;

        public int SupplierInsert(int Supid, string Supplier, string Address, string Email, int Phone)
        {

            SupID = Supid.ToString();
            Supplierd = Supplier;
            Addressd = Address;
            Gmail = Email;
            Number = Phone.ToString();


            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand("Insert into Supplier values (@a,@b,@c,@d,@e) ", db.con);
            db.cmd.Parameters.AddWithValue("a", Supid);
            db.cmd.Parameters.AddWithValue("b", Supplier);
            db.cmd.Parameters.AddWithValue("c", Address);
            db.cmd.Parameters.AddWithValue("d", Email);
            db.cmd.Parameters.AddWithValue("e", Phone);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;
        }
        public int SupplierUpdate(string Supplier,string Address, string Email, int Phone,int Supid)
        {

            SupID = Supid.ToString();
            Supplierd = Supplier;
            Addressd = Address;
            Gmail = Email;
            Number = Phone.ToString();

            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand("update Supplier set SUPPLIER=@a,ADDRESS=@b,EMAIL=@c,PHONE=@d where SUPID=@e", db.con);
            db.cmd.Parameters.AddWithValue("a", Supplier);
            db.cmd.Parameters.AddWithValue("b", Address);
            db.cmd.Parameters.AddWithValue("c", Email);
            db.cmd.Parameters.AddWithValue("d", Phone);
            db.cmd.Parameters.AddWithValue("e", Supid);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;

        }

        public int SupplierDelete(int supid)
        {
            SupID = supid.ToString();


            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand("delete from Supplier where SUPID=@a", db.con);
            db.cmd.Parameters.AddWithValue("a", supid);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;


        }
        public int InsertItem(string a)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(a, db.con);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;
        }
        public int UpdateItem(string a)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(a, db.con);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;
        }
        public int DeleteItem(string a)
        {
            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand(a, db.con);
            int i = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return i;
        }
        
    }
}
