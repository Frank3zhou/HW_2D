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
    public partial class modelSet : Form
    {
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
    public    int state1mode;
    public    int state2mode;
    public    int state1halcon1;
    public    int state1halcon2;
    public    int state1halcon3;
    public    int state2halcon1;
    public    int state2halcon2 = 1;
    public    int state2halcon3 = 1;
 
    public    string state1takepictrue1;
    public    string state1takepictrue2;
    public    string state1takepictrue3;
    public    string state2takepictrue1;
    public    string state2takepictrue2;
    public    string state2takepictrue3;
    public    string state1finishtakepicture;
    public    string state2finishtakepicture;
    public    string state1resultOK;
    public    string state2resultOK;
    public    string state1resultNG;
    public    string state2resultNG;

    public string Modelname = "PLCmodel1";//;



        public modelSet()
        {
            InitializeComponent();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void modelSet_Load(object sender, EventArgs e)
        {
          

       
            groupBox1.BringToFront();
            groupBox2.BringToFront();
          


           
        }

   
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            state1takepictrue1 = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            state1takepictrue2 = textBox2.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            state1takepictrue3 = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            state1finishtakepicture = textBox6.Text;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            state2takepictrue1 = textBox16.Text;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            state2takepictrue2 = textBox15.Text;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            state2takepictrue3 = textBox12.Text;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            state2finishtakepicture = textBox11.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            state1resultOK = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            state1resultNG = textBox8.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            state2resultNG = textBox9.Text;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            state2resultOK = textBox10.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            state1mode = 1;
            panel3.Enabled = true;
            panel4.Enabled = false;
            panel7.Enabled = false;
            panel20.Enabled = true;
            panel22.Enabled = false;
            panel23.Enabled = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            state1mode = 2;
            panel3.Enabled = true;
            panel4.Enabled = true;
            panel7.Enabled = false;
            panel20.Enabled = true;
            panel22.Enabled = true;
            panel23.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            state1mode = 3;
            panel3.Enabled = true;
            panel4.Enabled = true;
            panel7.Enabled = true;
            panel20.Enabled = true;
            panel22.Enabled = true;
            panel23.Enabled = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            state2mode = 1;
            panel18.Enabled = true;
            panel17.Enabled = false;
            panel16.Enabled = false;
            panel26.Enabled = true;
            panel25.Enabled = false;
            panel24.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            state2mode = 2;
            panel18.Enabled = true;
            panel17.Enabled = true;
            panel16.Enabled = false;
            panel26.Enabled = true;
            panel25.Enabled = true;
            panel24.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            state2mode = 3;
            panel18.Enabled = true;
            panel17.Enabled = true;
            panel16.Enabled = true;
            panel26.Enabled = true;
            panel25.Enabled = true;
            panel24.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            state1halcon1 = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            state2halcon3 = comboBox2.SelectedIndex;
        }

        private void modelSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult m_result = MessageBox.Show("是否保存当前设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (m_result == DialogResult.Yes)
            {
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1mode", state1mode.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2mode", state2mode.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1halcon1", state1halcon1.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1halcon2", state1halcon2.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1halcon3", state1halcon3.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2halcon1", state2halcon1.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2halcon2", state2halcon2.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2halcon3", state2halcon3.ToString());
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1takepictrue1", state1takepictrue1);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1takepictrue2", state1takepictrue2);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1takepictrue3", state1takepictrue3);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2takepictrue1", state2takepictrue1);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2takepictrue2", state2takepictrue2);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2takepictrue3", state2takepictrue3);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1finishtakepicture", state1finishtakepicture);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2finishtakepicture", state2finishtakepicture);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1resultOK", state1resultOK);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2resultOK", state2resultOK);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state1resultNG", state1resultNG);
                IniAPI.INIWriteValue(Param_Path, Modelname, "state2resultNG", state2resultNG);
               
            }
            else
            {

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Modelname = "PLCmodel3";
            loadini();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Modelname = "PLCmodel2";
            loadini();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Modelname = "PLCmodel1";
            loadini();
        }
        public void loadini()
        {
            state1mode = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state1mode", "1"));
            state2mode = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state2mode", "1"));
            state1halcon1 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state1halcon1", "1"));
            state1halcon2 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state1halcon2", "1"));
            state1halcon3 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state1halcon3", "1"));
            state2halcon1 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state2halcon1", "1"));
            state2halcon2 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state2halcon2", "1"));
            state2halcon3 = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, Modelname, "state2halcon3", "1"));


            state1takepictrue1 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1takepictrue1", "1");
            state1takepictrue2 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1takepictrue2", "1");
            state1takepictrue3 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1takepictrue3", "1");
            state2takepictrue1 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2takepictrue1", "1");
            state2takepictrue2 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2takepictrue2", "1");
            state2takepictrue3 = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2takepictrue3", "1");
            state1finishtakepicture = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1finishtakepicture", "1");
            state2finishtakepicture = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2finishtakepicture", "1");
            state1resultOK = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1resultOK", "1");
            state2resultOK = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2resultOK", "1");
            state1resultNG = IniAPI.INIGetStringValue(Param_Path, Modelname, "state1resultNG", "1");
            state2resultNG = IniAPI.INIGetStringValue(Param_Path, Modelname, "state2resultNG", "1");

            if (state1mode == 1)
            {
                radioButton1.Checked = true;
            }
            else if (state1mode == 2)
            {
                radioButton2.Checked = true;
            }
            else if (state1mode == 3)
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (state2mode == 1)
            {
                radioButton6.Checked = true;
            }
            else if (state2mode == 2)
            {
                radioButton5.Checked = true;
            }
            else if (state2mode == 3)
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            textBox1.Text = state1takepictrue1;
            textBox2.Text = state1takepictrue2;
            textBox5.Text = state1takepictrue3;
            textBox16.Text = state2takepictrue1;
            textBox15.Text = state2takepictrue2;
            textBox12.Text = state2takepictrue3;
            textBox6.Text = state1finishtakepicture;
            textBox11.Text = state2finishtakepicture;
            textBox7.Text = state1resultOK;
            textBox8.Text = state1resultNG;
            textBox10.Text = state2resultOK;
            textBox9.Text = state2resultNG;
            comboBox1.SelectedIndex = state1halcon1;
            comboBox3.SelectedIndex = state1halcon2;
            comboBox4.SelectedIndex = state1halcon3;
            comboBox6.SelectedIndex = state2halcon1;
            comboBox5.SelectedIndex = state2halcon2;
            comboBox2.SelectedIndex = state2halcon3;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            state1halcon2= comboBox3.SelectedIndex;

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            state1halcon3 = comboBox4.SelectedIndex;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            state2halcon1 = comboBox6.SelectedIndex;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            state2halcon2 = comboBox5.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            state2halcon3 = comboBox2.SelectedIndex;
        }
    }
}
