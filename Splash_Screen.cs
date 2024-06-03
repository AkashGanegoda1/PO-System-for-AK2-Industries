using System;
using System.Windows.Forms;

namespace Purchasing_Order
{
    public partial class Splash_Screen : Form
    {
        [Obsolete]
        public Splash_Screen()
        {
            InitializeComponent();
        }
        private void Splash_Screen_Load_1(object sender, EventArgs e)
        {
            WinAPI.AnimateWindow(this.Handle, 500, WinAPI.BLEND);
        }

        [Obsolete]
        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                guna2ProgressBar1.Value += 3;
                if (guna2ProgressBar1.Value >= 100)
                {
                    timer1.Stop();
                    this.Hide();
                    Login lg = new Login();
                    lg.Show();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
