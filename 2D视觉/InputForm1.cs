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
    public partial class InputForm1 : Form
    {
        string _inputstr;

        public string Inputstr { get => _inputstr; set => _inputstr = value; }

        public InputForm1(string label)
        {

           
            InitializeComponent();
            label1.Text = label;
        }

        private void InputForm1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Inputstr= textBox1.Text ;   
            this.Close();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
