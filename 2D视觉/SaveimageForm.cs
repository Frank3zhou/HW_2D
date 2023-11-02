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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _6524
{
    public partial class SaveimageForm : Form
    {
        string inipath = Application.StartupPath + "\\Param.ini";//配置表地址
        public SaveimageForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = choose_DC();
            IniAPI.INIWriteValue(inipath, "SaveImage", "SaveImagePath", textBox1.Text);
           
        }
        public string choose_DC()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

                // 设置对话框的标题
                folderBrowserDialog1.Description = "选择文件夹";

                // 打开文件夹对话框并检查用户是否选择了文件夹
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件夹的路径
                    return folderBrowserDialog1.SelectedPath;

                    // 在控制台输出所选文件夹的路径
                    //Console.WriteLine("所选文件夹的路径是: " + selectedFolderPath);
                }
                else { return null; }
            }
            catch (Exception)
            {

                return null;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
            }
            IniAPI.INIWriteValue(inipath, "SaveImage", "SaveImage_Enabled", checkBox2.Checked.ToString());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked) 
            {
                radioButton2.Checked= false;
                IniAPI.INIWriteValue(inipath, "SaveImage", "ImageFormat", radioButton1.Text);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                IniAPI.INIWriteValue(inipath, "SaveImage", "ImageFormat", radioButton2.Text);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(inipath, "SaveImage", "DeleteImage_Enabled", checkBox1.Checked.ToString());

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(inipath, "SaveImage", "DeleteImagedays", numericUpDown1.Value.ToString());
        }

        private void SaveimageForm_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Convert.ToInt32(IniAPI.INIGetStringValue(inipath, "SaveImage", "DeleteImagedays", ""));
            checkBox1.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(inipath, "SaveImage", "DeleteImage_Enabled", ""));
           string ImageFormat= IniAPI.INIGetStringValue(inipath, "SaveImage", "ImageFormat", "");
            if (ImageFormat == "JPG")
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton2.Checked = false;
                radioButton1.Checked = true;
            }
           textBox1.Text = IniAPI.INIGetStringValue(inipath, "SaveImage", "SaveImagePath", "");
            checkBox2.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(inipath, "SaveImage", "SaveImage_Enabled", ""));
            if (checkBox2.Checked)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
            }


        }

     
    }
}
