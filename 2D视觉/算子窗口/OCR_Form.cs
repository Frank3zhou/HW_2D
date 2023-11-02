using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524.算子窗口
{
    public partial class OCR_Form : Form
    {
        ZKHwindows m_ZKHwindows;
        HObject img;
        public OCR_Form()
        {
            InitializeComponent();
        }

        private void OCR_Form_Load(object sender, EventArgs e)
        {
            m_ZKHwindows = new ZKHwindows();
            HOperatorSet.GenEmptyObj(out img);
            m_ZKHwindows.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_ZKHwindows);
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "fill");
        }
    }
}
