using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace awwBreak
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Icon = awwBreak.Properties.Resources.Icon_Main;
            //this.WindowState = FormWindowState.Minimized;
            this.BringToFront();
            this.WindowState = FormWindowState.Normal;
            this.ShowDialog();

        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Image = awwBreak.Properties.Resources.notificationBox_ok_hover;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = awwBreak.Properties.Resources.notificationBox_ok;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
