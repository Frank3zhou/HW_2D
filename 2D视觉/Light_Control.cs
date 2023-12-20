using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524
{
    public partial class Light_Control : Form
    {
        string Path_calibration_Param = Application.StartupPath + @"\\Param.ini";
        //  bool connect = false;
        RS232 rS232;
        public Light_Control()
        {
            this.TopLevel = false;   
            InitializeComponent();
        }

        private void Light_Control_Load(object sender, EventArgs e)
        {
            
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          if(button1.Text == "打开串口")
            {
                rS232 = new RS232();
                rS232.BaudRate = Convert.ToInt32(comboBox2.Text);
                rS232.DataBits = Convert.ToInt32(comboBox3.Text);
                rS232.StopBits = Convert.ToInt32(comboBox4.Text);
                rS232.COMPort = comboBox1.Text;
                rS232.Parity = comboBox5.SelectedIndex;


                rS232.Init();
                if (!(rS232.Open() == 1))
                {
                    button1.BackColor = Color.Red;
                    MessageBox.Show("光源控制器连接失败");
                }
                else
                {
                 
                    panel2.Enabled = true;
                    //int brightness = 150;
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.Green;
                    //  rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                    //  button12.Text = "关闭光源";
                }
            }
            else
            {
                rS232.Close();
                button1.Text = "打开串口";
                button1.BackColor = Color.White ;
                panel2.Enabled = false;
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            run_control(sender,Convert.ToInt32(numericUpDown1.Value),"A");
        }

        private void run_control(object sender,int B ,string C)
        {
           string Functioncode = IniAPI.INIGetStringValue(Path_calibration_Param, "COM", "Functioncode", "SA");
            Button A = (Button)sender;
            if (A.Text == "打开")
            {
                A.Text = "关闭";
                A.BackColor = Color.LimeGreen;
                rS232.SerialPort.WriteLine(Functioncode + C + "0" + B.ToString("D3") + "#" + "\r");//关闭光源 
            }
            else
            {
                A.BackColor = Color.White;
                A.Text = "打开";
                rS232.SerialPort.WriteLine( Functioncode + C + "0000" + "#" + "\r");//关闭光源 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown2.Value),"B");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown4.Value),"C");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown3.Value),"D");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown8.Value),"E");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown7.Value),"F");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown6.Value),"G");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            run_control(sender, Convert.ToInt32(numericUpDown5.Value),"H");
        }

        private void Light_Control_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Light_Control_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "BaudRate", comboBox2.Text);
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "DataBits", comboBox3.Text);
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "StopBits", comboBox4.Text);
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "COMPort", comboBox1.Text);
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "Parity", comboBox5.SelectedIndex.ToString());
            IniAPI.INIWriteValue(Path_calibration_Param, "COM", "Functioncode", textBox1.Text);
            MessageBox.Show("保存成功");
        }
    }
}
