using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524
{
    public partial class Login : Form
    {
        public int State =0;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin")
            {
                if (textBox2.Text == "root")
                {
                    State = 2;
                    MessageBox.Show("登录成功");
                }
            }
            else if (textBox1.Text == "HW")
            {
                if (textBox2.Text == "1121")
                {
                    State = 1;
                }
            }
            else
            { 

            }
            this.Close();
        }
    }
}
