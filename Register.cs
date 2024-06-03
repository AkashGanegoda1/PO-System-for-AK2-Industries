namespace Purchasing_Order
{
    class Register
    {
        readonly Database db = new Database();

        public string Date;
        public string ID;
        public string Gmail;
        public string Username;
        public string Password; 
        public int Registering(string date, int id, string gmail, string username, string passowrd, byte[] profilepic)
        {

            Date = date;
            ID = id.ToString();
            Gmail = gmail;
            Username = username;
            Password = passowrd;

            db.con.Open();
            db.cmd = new System.Data.SqlClient.SqlCommand("Insert into Registration values (@a,@b,@c,@d,@e,@f) ", db.con);
            db.cmd.Parameters.AddWithValue("a", date);
            db.cmd.Parameters.AddWithValue("b", id);
            db.cmd.Parameters.AddWithValue("c", gmail);
            db.cmd.Parameters.AddWithValue("d", username);
            db.cmd.Parameters.AddWithValue("e", passowrd);
            db.cmd.Parameters.AddWithValue("f", profilepic);
            int a = db.cmd.ExecuteNonQuery();
            db.con.Close();
            db.cmd.Dispose();
            return a;
            
        }
    }
}
