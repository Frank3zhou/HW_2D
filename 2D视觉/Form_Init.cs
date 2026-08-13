using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _6524
{
    public partial class Form_Init : Form
    {
    
        public Form_Init()
        {
            InitializeComponent();
        }
        public string Message
        {
            //set { backgroundWorker1.ReportProgress(1, value); }
            set
            {
                label1.Text = value;
                label1.Update();
            }

        }

        public int StepValue
        {
            set
            {

                progressBar1.Value = value;
                progressBar1.Update();
            }
        }

        public void PerformStep()
        {
            progressBar1.PerformStep();
            progressBar1.Update();
        }

        private void Form_Init_Load(object sender, EventArgs e)
        {


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
