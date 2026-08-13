using _6524.Class;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _6524
{
    public partial class FANUC12 : Form
    {
        //Fanuc m_fanuc = new Fanuc();
      //  bool Fanucconnected = false;
       // Array xyzwpr = new float[9];
        fanuctcpip fanuctcpip;
        ZKHwindows m_ZKHwindows=new ZKHwindows();
        string Path_calibration_Param = Application.StartupPath + @"\\calibration\Param.ini";
        MVS_SDK m_Camera = new MVS_SDK();
        HObject Img = new HObject();
       bool  m_Cameraconnected=false;
        public FANUC12()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (fanuctcpip.connectFanuc(textBox1.Text))
            {
                MessageBox.Show("连接成功");
            }
             else
            {
                MessageBox.Show("连接失败");
            }

          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a = "";
            if (fanuctcpip.readPR(textBox4.Text, ref a))
                {
                string[] xyzwprstr = a.Split(',');
                X_TEXT.Text = xyzwprstr[0];
                Y_TEXT.Text =  xyzwprstr[1];
                Z_TEXT.Text =  xyzwprstr[2];
                W_TEXT.Text =  xyzwprstr[3];
                P_TEXT.Text =  xyzwprstr[4];
                R_TEXT.Text =  xyzwprstr[5];
                MessageBox.Show("Read Success!");
            }
          





        }

        private void FANUC_UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //m_fanuc.disconnect();

            fanuctcpip.close();
        }

        private void FANUC_UI_Load(object sender, EventArgs e)
        {
             fanuctcpip = new fanuctcpip();
            m_ZKHwindows.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_ZKHwindows);
            textBox5.Text =IniAPI.INIGetStringValue(Path_calibration_Param, "robotCamera", "IP", "");
            textBox1.Text=IniAPI.INIGetStringValue(Path_calibration_Param, "Robot", "IP", "");
            checkBox1.Checked= Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "robotCamera", "Use_Detection_Camera", "false"));
            if (!checkBox1.Checked)
            { 
                groupBox1.Enabled = false;  
            }
            comboBox1.Text = IniAPI.INIGetStringValue(Path_calibration_Param, "robotCamera", "Use_Detection_Camera_name", "相机1");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (fanuctcpip.WrieR(textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Write Success!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string a = "";
            if (fanuctcpip.readR(textBox2.Text, ref a ))
            {
                textBox3.Text = a;
                MessageBox.Show("Read Success!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string PRvalue = X_TEXT.Text + "," + Y_TEXT.Text + "," + Z_TEXT.Text + "," + W_TEXT.Text + "," + P_TEXT.Text + "," + R_TEXT.Text;
            if (fanuctcpip.writePR(textBox4.Text, PRvalue))
            {
                
                MessageBox.Show("Write Success!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string a = "";
            if (fanuctcpip.readNowPR( ref a))
            {
                string[] xyzwprstr = a.Split(',');
                X_TEXT.Text = xyzwprstr[0];
                Y_TEXT.Text = xyzwprstr[1];
                Z_TEXT.Text = xyzwprstr[2];
                W_TEXT.Text = xyzwprstr[3];
                P_TEXT.Text = xyzwprstr[4];
                R_TEXT.Text = xyzwprstr[5];
                MessageBox.Show("Read Success!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "robotCamera","IP", textBox5.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (m_Camera.Connect_Cam(textBox5.Text))
            {
                MessageBox.Show("Camera Connected");
                m_Cameraconnected = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (m_Cameraconnected)
            {

                if (takepicture())
                {

                }
                else
                {
                    MessageBox.Show("Take Image error");
                }

            }
            else
            {
                MessageBox.Show("please connected Camera");
            }
        }

        private bool takepicture()
        {
            try
            {
                if (m_Cameraconnected)
                {
                    if (m_Camera.State == 0)
                    {
                        m_Camera.Connect_Cam(textBox5.Text);
                    }
                    if (m_Camera.State == 1)
                    {
                        m_Camera.OpenCamera();
                    }
                    if (m_Camera.Start())  //开始采集流
                    {
                        //RS232 rS232 = new RS232();
                        //int brightness = 150;
                        //rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");
                        //Thread.Sleep(100);
                        //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                        if (m_Camera.Get_Oneframe())
                        {

                            Thread.Sleep(100);
                            //  m_window.NowImage = m_Camera.Himage;
                            HOperatorSet.CopyImage(m_Camera.Himage, out Img);
                            m_ZKHwindows.NowImage = Img;

                            Console.WriteLine("Get_onefrmae is OK ");

                            m_Camera.Stop();
                            m_Camera.close();


                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "Robot","IP", textBox1.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "robotCamera", "Use_Detection_Camera", checkBox1.Checked.ToString());
            if (checkBox1.Checked)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "robotCamera", "Use_Detection_Camera_Name", comboBox1.Text);
        }
    }
}
