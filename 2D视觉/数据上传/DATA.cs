using _6524.Class;
using HslCommunication;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SixLabors.ImageSharp.PixelFormats;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _6524
{
    public delegate string PLCresult(string AA);
    public partial class DATA : Form
    {
        RS232 rS232;
        public PLCresult pLCresult;
        string parma_path = Application.StartupPath + @"\\DATA\\Param.ini";
        public DATA()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                panel7.Enabled = false;
                IniAPI.INIWriteValue(parma_path, "System", "Passive_get","false");
            }
            else
            {
                panel7.Enabled = true;
                IniAPI.INIWriteValue(parma_path, "System", "Passive_get", "true");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                panel3.Enabled = false;
                IniAPI.INIWriteValue(parma_path, "System", "initiative_get", "false") ;
            }
            else
            {
                panel3.Enabled = true;
                IniAPI.INIWriteValue(parma_path, "System", "initiative_get", "true");
            }
        }

        private void DATA_Load(object sender, EventArgs e)
        {
          //  checkBox6.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "initiative_get_code_enable", "false"));
            checkBox5.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "USE_CODE", "false"));
            checkBox2.Checked= Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "initiative_get", "false"));
            checkBox1.Checked=Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "Passive_get", "false"));
            textBox23.Text = IniAPI.INIGetStringValue(parma_path, "System", "Turn_on_path", "");
          //  textBox25.Text= IniAPI.INIGetStringValue(parma_path, "System", "initiative_Get_On","");
            panel3.Enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "initiative_get", "false"));
            panel7.Enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "Passive_get", "false"));
            comboBox6.Text= IniAPI.INIGetStringValue(parma_path, "COM", "BaudRate","");
            comboBox3.Text=IniAPI.INIGetStringValue(parma_path, "COM", "DataBits", "");
            comboBox4.Text= IniAPI.INIGetStringValue(parma_path, "COM", "StopBits", "");
            comboBox7.Text=IniAPI.INIGetStringValue(parma_path, "COM", "COMPort","");
            comboBox5.SelectedIndex=Convert.ToInt32( IniAPI.INIGetStringValue(parma_path, "COM", "Parity",""));
            textBox26.Text=IniAPI.INIGetStringValue(parma_path, "COM", "Functioncode", "");
            checkBox7.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "System", "LOCK", "false"));
            panel8.Enabled = checkBox7.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_IP", textBox1.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_PORT", textBox2.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_1_Path", textBox3.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_2_Path", textBox4.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_3_Path", textBox6.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_4_Path", textBox5.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_5_Path", textBox7.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_6_Path", textBox12.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_7_Path", textBox11.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_8_Path", textBox10.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_9_Path", textBox9.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Data_10_Path", textBox8.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Pressure_NUM", numericUpDown1.Value.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "Pressure_Value_length", numericUpDown2.Value.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PC_To_NG", textBox14.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PC_To_NG_Finished", textBox15.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PC_To_init", textBox13.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PC_To_init_Finished", textBox16.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_To_NG", textBox19.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_To_NG_Finished", textBox17.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_To_init", textBox20.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_To_init_Finished", textBox18.Text);
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "enable", checkBox3.Checked.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox1.Text, "PLC_funcode", textBox21.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string PATH = "";
            if (numericUpDown3.Value == 1)
            {
                PATH = "Data_1_Path";
            }
            else if (numericUpDown3.Value == 2)
            {
                PATH = "Data_2_Path";
            }
            else if (numericUpDown3.Value == 3)
            {
                PATH = "Data_3_Path";
            }
            else if (numericUpDown3.Value == 4)
            {
                PATH = "Data_4_Path";
            }
            else if (numericUpDown3.Value == 5)
            {
                PATH = "Data_5_Path";
            }
            else if (numericUpDown3.Value == 6)
            {
                PATH = "Data_6_Path";
            }
            else if (numericUpDown3.Value == 7)
            {
                PATH = "Data_7_Path";
            }
            else if (numericUpDown3.Value == 8)
            {
                PATH = "Data_8_Path";
            }
            else if (numericUpDown3.Value == 9)
            {
                PATH = "Data_9_Path";
            }
            else if (numericUpDown3.Value == 10)
            {
                PATH = "Data_10_Path";
            }

            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_1", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_NUM", "0"));
                for (int i = 0; i < num; i++)
                {

                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站1" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                    }

                }
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_2", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_NUM", "0"));
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站2" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                    }

                }
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_3", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_NUM", "0"));
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站3" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                    }

                }
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_4", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_NUM", "0"));
                for (int i = 0; i < num; i++)
                {
                    OperateResult<Int16[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站4" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                    }

                }
                MC1.dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PressurePLCinit();
        }

        public void PressurePLCinit()
        {
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_1", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PC_To_init", ""), true);
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_2", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PC_To_init", ""), true);
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_3", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PC_To_init", ""), true);
                MC1.dispose();
            }
            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_4", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PC_To_init", ""), true);
                MC1.dispose();
            }
        }
        public string Getcode()
        {
            TCPIP m_TCP = new TCPIP();
            string Result = "";
            Task task = Task.Factory.StartNew(() =>
            {
                m_TCP.IpAddress = "127.0.0.1";
                m_TCP.Port = 5000;
                if (m_TCP.ClientConnect())
                {
                    m_TCP.Send("#GET#");//发送请求指令
                }
                Thread.Sleep(100);
                if (m_TCP.IsConnected("127.0.0.1"))
                {
                    string receivestr;
                    if (m_TCP.Recive(out receivestr))
                    {

                        Result = receivestr;
                        //string[] a = receivestr.Split('#');
                        // Result = a[1].ToString();
                        // disp_message(HWindowshandle, "接受打码信息:" + Result, "window", 52, 12, "black", "true");
                    }
                    else
                    {
                        Result = "";
                    }
                }
                m_TCP.DisConnect();
            });

            // 等待任务完成
            task.Wait(3000);//任务等待3秒
            return Result;
        }

        public void getdatatoexcel1(int Num, string code)
        {
            string PATH = "";
            if (Num == 1)
            {
                PATH = "Data_1_Path";
            }
            else if (Num == 2)
            {
                PATH = "Data_2_Path";
            }
            else if (Num == 3)
            {
                PATH = "Data_3_Path";
            }
            else if (Num == 4)
            {
                PATH = "Data_4_Path";
            }
            else if (Num == 5)
            {
                PATH = "Data_5_Path";
            }
            else if (Num == 6)
            {
                PATH = "Data_6_Path";
            }
            else if (Num == 7)
            {
                PATH = "Data_7_Path";
            }
            else if (Num == 8)
            {
                PATH = "Data_8_Path";
            }
            else if (Num == 9)
            {
                PATH = "Data_9_Path";
            }
            else if (Num == 10)
            {
                PATH = "Data_10_Path";
            }
            string m_Year = DateTime.Now.ToString("yyyy");
            string m_Month = DateTime.Now.ToString("MM");
            string m_Day = DateTime.Now.ToString("dd");
            string pressure_log_xlsl = Application.StartupPath + "\\Pressurelog" + "\\";
            CreateFiles(pressure_log_xlsl);
            string Excelname = pressure_log_xlsl + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            DataTable d1 = new DataTable();
            DataTable d2 = new DataTable();
            DataTable d3 = new DataTable();
            DataTable d4 = new DataTable();
            bool isColumns1 = false;
            bool isColumns2 = false;
            bool isColumns3 = false;
            bool isColumns4 = false;
            if (RW_XLSX.sheetnameExists(Excelname, "铆压1"))
            {
                d1 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压1", true);
                isColumns1=true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压2"))
            {
                d2 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压2", true);
                isColumns2 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压3"))
            {
                d3 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压3", true);
                isColumns3 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压4"))
            {
                d4 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压4", true);
                isColumns4 = true;
            }


            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_1", "enable", "true")))
            {
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_NUM", "0"));
                if (!isColumns1)
                {
                    d1.Columns.Add("编号", Type.GetType("System.String"));
                    d1.Columns.Add("设定值", Type.GetType("System.String"));
                    d1.Columns.Add("上限值", Type.GetType("System.String"));
                    d1.Columns.Add("下限值", Type.GetType("System.String"));
                    d1.Columns.Add("结果", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d1.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }

                }
                DataRow newRow = d1.NewRow();
                newRow[0] = code;
                int Set_value = 6750;
                int Up_value = 6950;
                int Down_value = 6550;

                newRow[1] = Set_value.ToString();//设定值
                newRow[2] = Up_value.ToString();//上限
                newRow[3] = Down_value.ToString();//下限
                bool res_Pre = true;//压机结果

                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();

                for (int i = 0; i < num; i++)
                {

                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站1" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                        newRow[i + 5] = Result.Content[0].ToString();
                        if (Convert.ToInt32(Result.Content[0]) > Up_value || Convert.ToInt32(Result.Content[0]) < Down_value)
                        {
                            res_Pre = false;
                        }
                    }

                }
                if (res_Pre)
                {
                    newRow[4] = "OK";//下限
                }
                else
                {
                    newRow[4] = "NG";//下限
                }
                d1.Rows.Add(newRow);
                MC1.dispose();
            }

            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_2", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_NUM", "0"));
                if (!isColumns2)
                {
                    d2.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d2.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow = d2.NewRow();
                newRow[0] = code;
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站2" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);

                        newRow[i + 1] = Result.Content[0].ToString();
                    }

                }
                d2.Rows.Add(newRow);
                MC1.dispose();
            }

            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_3", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_NUM", "0"));
                if (!isColumns3)
                {
                    d3.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d3.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow = d3.NewRow();
                newRow[0] = code;
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站3" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);
                        newRow[i + 1] = Result.Content[0].ToString();

                    }

                }
                d3.Rows.Add(newRow);
                MC1.dispose();
            }


            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_4", "enable", "true")))
            {
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_NUM", "0"));
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                if (!isColumns4)
                {
                    d4.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d4.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow = d4.NewRow();
                newRow[0] = code;

                for (int i = 0; i < num; i++)
                {
                    OperateResult<Int16[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站4" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);

                        newRow[i + 1] = Result.Content[0].ToString();
                    }

                }
                d4.Rows.Add(newRow);
                MC1.dispose();
            }


            RW_XLSX.TableToExcel4(d1, d2, d3, d4, "铆压1", "铆压2", "铆压3", "铆压4", Excelname);
        }



        public void getdatatoexcel4(int Num, string code)
        {
            string PATH = "";
            if (Num == 1)
            {
                PATH = "Data_1_Path";
            }
            else if (Num == 2)
            {
                PATH = "Data_2_Path";
            }
            else if (Num == 3)
            {
                PATH = "Data_3_Path";
            }
            else if (Num == 4)
            {
                PATH = "Data_4_Path";
            }
            else if (Num == 5)
            {
                PATH = "Data_5_Path";
            }
            else if (Num == 6)
            {
                PATH = "Data_6_Path";
            }
            else if (Num == 7)
            {
                PATH = "Data_7_Path";
            }
            else if (Num == 8)
            {
                PATH = "Data_8_Path";
            }
            else if (Num == 9)
            {
                PATH = "Data_9_Path";
            }
            else if (Num == 10)
            {
                PATH = "Data_10_Path";
            }
            string m_Year = DateTime.Now.ToString("yyyy");
            string m_Month = DateTime.Now.ToString("MM");
            string m_Day = DateTime.Now.ToString("dd");
            string pressure_log_xlsl = Application.StartupPath + "\\Pressurelog" + "\\";
            CreateFiles(pressure_log_xlsl);
            string Excelname = pressure_log_xlsl + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            DataTable d1 = new DataTable();
            DataTable d2 = new DataTable();
            DataTable d3 = new DataTable();
            DataTable d4 = new DataTable();
            DataTable d5 = new DataTable();
            bool isColumns1 = false;
            bool isColumns2 = false;
            bool isColumns3 = false;
            bool isColumns4 = false;
            bool isColumns5 = false;
            if (RW_XLSX.sheetnameExists(Excelname, "铆压1"))
            {
                d1 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压1", true);
                isColumns1 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压2"))
            {
                d2 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压2", true);
                isColumns2 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压3"))
            {
                d3 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压3", true);
                isColumns3 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "铆压4"))
            {
                d4 = RW_XLSX.ReadExcelToDataTable(Excelname, "铆压4", true);
                isColumns4 = true;
            }
            if (RW_XLSX.sheetnameExists(Excelname, "螺丝机"))
            {
                d5 = RW_XLSX.ReadExcelToDataTable(Excelname, "螺丝机", true);
                isColumns5 = true;
            }


            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_1", "enable", "true")))
            {
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_NUM", "0"));
                if (!isColumns1)
                {
                    d1.Columns.Add("编号", Type.GetType("System.String"));
                    d1.Columns.Add("设定值", Type.GetType("System.String"));
                    d1.Columns.Add("上限值", Type.GetType("System.String"));
                    d1.Columns.Add("下限值", Type.GetType("System.String"));
                    d1.Columns.Add("结果", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d1.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }

                }
                DataRow newRow1 = d1.NewRow();
                newRow1[0] = code;
                int Set_value = 6750;
                int Up_value = 6950;
                int Down_value = 6550;

                newRow1[1] = Set_value.ToString();//设定值
                newRow1[2] = Up_value.ToString();//上限
                newRow1[3] = Down_value.ToString();//下限
                bool res_Pre = true;//压机结果
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();

                for (int i = 0; i < num; i++)
                {

                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站1" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);


                        newRow1[i + 5] = Result.Content[0].ToString();
                        if (Convert.ToInt32(Result.Content[0]) > Up_value || Convert.ToInt32(Result.Content[0]) < Down_value)
                        {
                            res_Pre = false;
                        }

                    }

                }
                if (res_Pre)
                {
                    newRow1[4] = "OK";//下限
                }
                else
                {
                    newRow1[4] = "NG";//下限
                }
                d1.Rows.Add(newRow1);
                MC1.dispose();
            }

            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_2", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_NUM", "0"));
                if (!isColumns2)
                {
                    d2.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d2.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow2 = d2.NewRow();
                newRow2[0] = code;
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站2" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);

                        newRow2[i + 1] = Result.Content[0].ToString();
                    }

                }
                d2.Rows.Add(newRow2);
                MC1.dispose();
            }

            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_3", "enable", "true")))
            {
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_NUM", "0"));
                if (!isColumns3)
                {
                    d3.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d3.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow3 = d3.NewRow();
                newRow3[0] = code;
                for (int i = 0; i < num; i++)
                {
                    OperateResult<short[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站3" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);
                        newRow3[i + 1] = Result.Content[0].ToString();

                    }

                }
                d3.Rows.Add(newRow3);
                MC1.dispose();
            }


            if (Convert.ToBoolean(IniAPI.INIWriteValue(parma_path, "Pressure_Machine_4", "enable", "true")))
            {
                int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_NUM", "0"));
                basePLC MC1 = new basePLC();
                MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_IP", "");
                MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_PORT", ""));
                MC1.pLCMode = PLCMode.MC;
                MC1.init();
                if (!isColumns4)
                {
                    d4.Columns.Add("编号", Type.GetType("System.String"));
                    for (int i = 0; i < num; i++)
                    {
                        d4.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                    }
                }
                DataRow newRow4 = d4.NewRow();
                newRow4[0] = code;

                for (int i = 0; i < num; i++)
                {
                    OperateResult<Int16[]> Result = MC1.ReadInt16(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", PATH, "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "Pressure_Value_length", "0"))).ToString(), 1);
                    if (Result.IsSuccess)
                    {

                        Console.WriteLine("铆压工站4" + "第" + numericUpDown3.Value.ToString() + "轮，第" + (i + 1).ToString() + "颗压力值：" + Result.Content[0]);

                        newRow4[i + 1] = Result.Content[0].ToString();
                    }

                }
                d4.Rows.Add(newRow4);
                MC1.dispose();
            }



            #region 螺丝机
            if (!isColumns5)
            {
                d5.Columns.Add("编号", Type.GetType("System.String"));
                d5.Columns.Add("扭力", Type.GetType("System.String"));
            }
            DataRow newRow5 = d5.NewRow();
            newRow5[0] = code;




            RS232 rS232 = new RS232();
            rS232.BaudRate = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "BaudRate", ""));
            rS232.DataBits = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "DataBits", ""));
            rS232.StopBits = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "StopBits", ""));
            rS232.COMPort = IniAPI.INIGetStringValue(parma_path, "COM", "COMPort", "");
            rS232.Parity = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "Parity", ""));
            rS232.Init();
            if ((rS232.Open() == 1))
            {
                string AA = rS232.SerialPort.ReadLine();
                string[] BB = AA.Split(Convert.ToChar(IniAPI.INIGetStringValue(parma_path, "COM", "Functioncode", "")));
                newRow5[1] = BB[19];

            }

            rS232.Close();

            d5.Rows.Add(newRow5);
            #endregion

            RW_XLSX.TableToExcel5(d1, d2, d3, d4, d5,"铆压1", "铆压2", "铆压3", "铆压4", "螺丝机", Excelname);
        }


        public void getdatatoexcel2(string code )
        {
            string m_Year = DateTime.Now.ToString("yyyy");
            string m_Month = DateTime.Now.ToString("MM");
            string m_Day = DateTime.Now.ToString("dd");
            string pressure_log_xlsl = Application.StartupPath + "\\Pressurelog" + "\\";
            CreateFiles(pressure_log_xlsl);
            string Excelname = pressure_log_xlsl + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            DataTable d1 = new DataTable();
            DataTable d2 = new DataTable();
            DataTable d3 = new DataTable();
            DataTable d4 = new DataTable();
            bool isColumns = false;
          
           
            

            DataTable d = new DataTable();
            for (int A = 0; A < 4; A++)
            {
                d = new DataTable();
                if (Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, "Pressure_M_"+(A+1).ToString(), "enable", "false")))
                {
                    if (RW_XLSX.sheetnameExists(Excelname, "Pressure_M_" + (A + 1).ToString()))
                    {
                        d = RW_XLSX.ReadExcelToDataTable(Excelname, "Pressure_M_" + (A + 1).ToString(), true);
                        isColumns = true;
                    }
                    int num = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "Pressure_NUM", "0"));
                    if (!isColumns)
                    {
                        d.Columns.Add("编号", Type.GetType("System.String"));
                        d.Columns.Add("设定值", Type.GetType("System.String"));
                        d.Columns.Add("上限值", Type.GetType("System.String"));
                        d.Columns.Add("下限值", Type.GetType("System.String"));
                        d.Columns.Add("结果", Type.GetType("System.String"));
                        for (int i = 0; i < num; i++)
                        {
                            d.Columns.Add("第" + (i + 1).ToString() + "颗", Type.GetType("System.String"));
                        }

                    }
                    DataRow newRow = d.NewRow();
                    newRow[0] = code;
                    int Set_value = 6750;
                    int Up_value = 6950;
                    int Down_value = 6550;
                    try
                    {
                        string Set_value_PATH;
                        string Up_value_PATH;
                        string Down_value_PATH;
                        Set_value_PATH = IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "Pressure_Value_SET", "0");
                        Up_value_PATH = IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "Pressure_Value_UP", "0");
                        Down_value_PATH = IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "Pressure_Value_DOWN", "0");
                        Set_value =Convert.ToInt32(pLCresult(Set_value_PATH)) ;
                        Up_value= Convert.ToInt32(pLCresult(Up_value_PATH));
                        Down_value= Convert.ToInt32(pLCresult(Down_value_PATH));
                    }
                    catch (Exception)
                    {

                        
                    }
                 

                    newRow[1] = Set_value.ToString();//设定值
                    newRow[2] = Up_value.ToString();//上限
                    newRow[3] = Down_value.ToString();//下限
                    bool res_Pre = true;//压机结果


                    for (int i = 0; i < num; i++)
                    {

                        
                            string path = IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "PLC_funcode", "D") + (Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "PLC_To_init_Path", "")) + i * Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_M_" + (A + 1).ToString(), "Pressure_Value_length", "0"))).ToString();

                            newRow[i + 5] = pLCresult(path);//PLC 回调获取
                        if (Convert.ToInt32(newRow[i + 5]) > Up_value || Convert.ToInt32(newRow[i + 5]) < Down_value)
                        {
                            res_Pre = false;
                        }


                    }
                    if(res_Pre)
                    {
                        newRow[4] = "OK";//下限
                    }
                else
                    {
                        newRow[4] = "NG";//下限
                    }
                    d.Rows.Add(newRow);
                    if (A + 1 == 1)
                    {
                        d1 = d;
                    }
                    else if (A + 1 == 2)
                    {
                        d2 = d;
                    }
                    else if (A + 1 == 3)
                    {
                        d3 = d;

                    }
                    else if (A + 1 == 4)
                    {
                        d4 = d;
                    }
                    else
                    { 
                    }
                   
                }

            }
            RW_XLSX.TableToExcel4(d1, d2, d3, d4, "Pressure_M_1", "Pressure_M_2", "Pressure_M_3", "Pressure_M_4", Excelname);
        }

        public void getdatatoexcel3(string code)
        {
            string m_Year = DateTime.Now.ToString("yyyy");
            string m_Month = DateTime.Now.ToString("MM");
            string m_Day = DateTime.Now.ToString("dd");
            string pressure_log_xlsl = Application.StartupPath + "\\Pressurelog" + "\\";
            CreateFiles(pressure_log_xlsl);
            string Excelname = pressure_log_xlsl + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            DataTable d = new DataTable();

            bool isColumns = false;
            if (RW_XLSX.sheetnameExists(Excelname, "螺丝机"))
            {
                d = RW_XLSX.ReadExcelToDataTable(pressure_log_xlsl + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx", "螺丝机", true);

                isColumns = true;
            }
          
      

               
                if (!isColumns)
                {
                    d.Columns.Add("编号", Type.GetType("System.String"));
                    d.Columns.Add("扭力", Type.GetType("System.String"));
                }
                 DataRow newRow = d.NewRow();
                newRow[0] = code;

        


            RS232 rS232 = new RS232();
            rS232.BaudRate = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "BaudRate", ""));
            rS232.DataBits = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "DataBits", ""));
            rS232.StopBits = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "StopBits", ""));
            rS232.COMPort = IniAPI.INIGetStringValue(parma_path, "COM", "COMPort", "");
            rS232.Parity = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "COM", "Parity", ""));
            rS232.Init();
            if ((rS232.Open() == 1))
            {
                string AA = rS232.SerialPort.ReadLine();
                string[] BB = AA.Split(Convert.ToChar(IniAPI.INIGetStringValue(parma_path, "COM", "Functioncode", "")));
                newRow[1]  = BB[19];
           
            }

            rS232.Close();

            d.Rows.Add(newRow);

            RW_XLSX.TableToExcel1(d,  "螺丝机", Excelname);

        }
        private void CreateFiles(string filepath)
        {
            try
            {
                bool flag = !Directory.Exists(filepath);
                if (flag)
                {
                    Directory.CreateDirectory(filepath);
                }
                bool flag2 = Directory.Exists(filepath);
                if (flag2)
                {
                    filepath += "\\";
                }
                else
                {
                    filepath += "-";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("创建文件夹失败，请确认路径");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string A = Getcode();
                getdatatoexcel1(Convert.ToInt32(numericUpDown3.Value), A);


            }
            catch (Exception)
            {


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            pressuremachine1NG();
        }
        public void pressuremachine1NG()
        {
            basePLC MC1 = new basePLC();
            MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_IP", "");
            MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PLC_PORT", ""));
            MC1.pLCMode = PLCMode.MC;
            MC1.init();
            MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_1", "PC_To_NG", ""), true);
            MC1.dispose();
        }

        public void pressuremachine2NG()
        {
            basePLC MC1 = new basePLC();
            MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_IP", "");
            MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PLC_PORT", ""));
            MC1.pLCMode = PLCMode.MC;
            MC1.init();
            MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_2", "PC_To_NG", ""), true);
            MC1.dispose();
        }

        public void pressuremachine3NG()
        {
            basePLC MC1 = new basePLC();
            MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_IP", "");
            MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PLC_PORT", ""));
            MC1.pLCMode = PLCMode.MC;
            MC1.init();
            MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_3", "PC_To_NG", ""), true);
            MC1.dispose();
        }
        public void pressuremachine4NG()
        {
            basePLC MC1 = new basePLC();
            MC1.IP = IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_IP", "");
            MC1.Port = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PLC_PORT", ""));
            MC1.pLCMode = PLCMode.MC;
            MC1.init();
            MC1.Write(IniAPI.INIGetStringValue(parma_path, "Pressure_Machine_4", "PC_To_NG", ""), true);
            MC1.dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pressuremachine2NG();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pressuremachine3NG();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pressuremachine4NG();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_IP", "");
            textBox2.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_PORT", "");
            textBox3.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_1_Path", "");
            textBox4.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_2_Path", "");
            textBox6.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_3_Path", "");
            textBox5.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_4_Path", "");
            textBox7.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_5_Path", "");
            textBox12.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_6_Path", "");
            textBox11.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_7_Path", "");
            textBox10.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_8_Path", "");
            textBox9.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_9_Path", "");
            textBox8.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Data_10_Path", "");
            numericUpDown1.Value = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Pressure_NUM", "0"));
            numericUpDown2.Value = Convert.ToInt32(IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "Pressure_Value_length", "0"));
            textBox14.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PC_To_NG", "");
            textBox15.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PC_To_NG_Finished", "");
            textBox13.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PC_To_init", "");
            textBox16.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PC_To_init_Finished", "");
            textBox19.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_To_NG", "");
            textBox17.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_To_NG_Finished", "");
            textBox20.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_To_init", "");
            textBox18.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_To_init_Finished", "");
            checkBox3.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "enable", "true"));
            textBox21.Text = IniAPI.INIGetStringValue(parma_path, comboBox1.Text, "PLC_funcode", "D");
        }


        public static class RW_XLSX
        {
            /// <summary>
            /// 将excel文件内容读取到DataTable数据表中
            /// </summary>
            /// <param name="fileName">文件完整路径名</param>
            /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
            /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
            /// <returns>DataTable数据表</returns>
            public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
            {
                //定义要返回的datatable对象
                DataTable data = new DataTable();
                //excel工作表
                ISheet sheet = null;
                //数据开始行(排除标题行)
                int startRow = 0;
                try
                {
                    if (!File.Exists(fileName))
                    {
                        throw new Exception("文件不存在");
                    }
                    //根据指定路径读取文件
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //根据文件流创建excel数据结构
                    IWorkbook workbook = null;
                    var fileType = Path.GetExtension(fileName).ToLower();
                    //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                    #region 判断Excel版本
                    switch (fileType)
                    {
                        //.XLSX是07版(或者07以上的)的Office Excel
                        case ".xlsx":
                            workbook = new XSSFWorkbook(fs);
                            break;
                        //.XLS是03版的Office Excel
                        case ".xls":
                            workbook = new HSSFWorkbook(fs);
                            break;
                        default:
                            throw new Exception("Excel文档格式有误");
                    }
                    #endregion

                    //IWorkbook workbook = new HSSFWorkbook(fs);
                    //如果有指定工作表名称
                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        sheet = workbook.GetSheet(sheetName);
                        //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        if (sheet == null)
                        {
                            sheet = workbook.CreateSheet(sheetName);
                            //sheet = workbook.GetSheetAt(0);
                        }
                    }
                    else
                    {
                        //如果没有指定的sheetName，则尝试获取第一个sheet
                        sheet = workbook.GetSheetAt(0);
                    }
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        //一行最后一个cell的编号 即总的列数
                        int cellCount = firstRow.LastCellNum;
                        //如果第一行是标题列名
                        if (isFirstRowColumn)
                        {
                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                            {
                                ICell cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    string cellValue = cell.StringCellValue;
                                    if (cellValue != null)
                                    {
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }
                                }
                            }
                            startRow = sheet.FirstRowNum + 1;
                        }
                        else
                        {
                            startRow = sheet.FirstRowNum;
                        }
                        //最后一列的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null || row.FirstCellNum < 0) continue; //没有数据的行默认是null　　　　　　　

                            DataRow dataRow = data.NewRow();

                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {

                                //if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                //    dataRow[j] = row.GetCell(j);

                                #region 格式转换 NPOI获取Excel单元格中不同类型的数据
                                ICell cell = row.GetCell(j);
                                if (cell != null)
                                {

                                    //获取指定的单元格信息

                                    switch (cell.CellType)
                                    {
                                        //首先在NPOI中数字和日期都属于Numeric类型
                                        //通过NPOI中自带的DateUtil.IsCellDateFormatted判断是否为时间日期类型
                                        case CellType.Numeric when DateUtil.IsCellDateFormatted(cell):
                                            dataRow[j] = cell.DateCellValue;
                                            break;
                                        case CellType.Numeric:
                                            //其他数字类型
                                            dataRow[j] = cell.NumericCellValue;
                                            break;
                                        //空数据类型
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        //公式类型
                                        case CellType.Formula:
                                            {
                                                HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(workbook);
                                                dataRow[j] = eva.Evaluate(cell).StringValue;
                                                break;
                                            }
                                        //布尔类型
                                        case CellType.Boolean:
                                            dataRow[j] = row.GetCell(j).BooleanCellValue;
                                            break;
                                        //错误
                                        case CellType.Error:
                                            // dataRow[j] = HSSF Constants.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        //其他类型都按字符串类型来处理（未知类型CellType.Unknown，字符串类型CellType.String）
                                        default:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                    }

                                }
                                #endregion
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public static bool sheetnameExists(string path, string name)
            {
                if (!File.Exists(path))
                {
                    return false;
                }
                FileStream fs=null;
                try
                {

                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    IWorkbook workbook = null;
                  
                    var fileType = Path.GetExtension(path).ToLower();
                    //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                    #region 判断Excel版本
                    switch (fileType)
                    {
                        //.XLSX是07版(或者07以上的)的Office Excel
                        case ".xlsx":
                            workbook = new XSSFWorkbook(fs);
                            break;
                        //.XLS是03版的Office Excel
                        case ".xls":
                            workbook = new HSSFWorkbook(fs);
                            break;
                        default:
                            throw new Exception("Excel文档格式有误");
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(name))
                    {
                        ISheet sheet = workbook.GetSheet(name);
                        //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        if (sheet == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
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
                finally
                {
                    fs.Dispose();
                   
                }
            }

            /// <summary>
            /// Datable导出成Excel
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file">导出路径(包括文件名与扩展名)</param>
            public static void TableToExcel4(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4, string sheetName1, string sheetName2, string sheetName3, string sheetName4, string file)
            {
                IWorkbook workbook;
                string fileExt = Path.GetExtension(file).ToLower();
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    workbook = null;
                }
                if (workbook == null)
                {
                    return;
                }



                ISheet sheet1 = workbook.CreateSheet(sheetName1);
                ISheet sheet2 = workbook.CreateSheet(sheetName2);
                ISheet sheet3 = workbook.CreateSheet(sheetName3);
                ISheet sheet4 = workbook.CreateSheet(sheetName4);

                //表头  
                IRow row1 = sheet1.CreateRow(0);
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    ICell cell = row1.CreateCell(i);
                    cell.SetCellValue(dt1.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    IRow row2 = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt1.Columns.Count; j++)
                    {
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue(dt1.Rows[i][j].ToString());
                    }
                }

                //表头  
                IRow row3 = sheet2.CreateRow(0);
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    ICell cell = row3.CreateCell(i);
                    cell.SetCellValue(dt2.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    IRow row4 = sheet2.CreateRow(i + 2);
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        ICell cell = row4.CreateCell(j);
                        cell.SetCellValue(dt2.Rows[i][j].ToString());
                    }
                }


                //表头  
                IRow row5 = sheet3.CreateRow(0);
                for (int i = 0; i < dt3.Columns.Count; i++)
                {
                    ICell cell = row5.CreateCell(i);
                    cell.SetCellValue(dt3.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    IRow row6 = sheet3.CreateRow(i + 2);
                    for (int j = 0; j < dt3.Columns.Count; j++)
                    {
                        ICell cell = row6.CreateCell(j);
                        cell.SetCellValue(dt3.Rows[i][j].ToString());
                    }
                }


                //表头  
                IRow row7 = sheet4.CreateRow(0);
                for (int i = 0; i < dt4.Columns.Count; i++)
                {
                    ICell cell = row7.CreateCell(i);
                    cell.SetCellValue(dt4.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    IRow row8 = sheet4.CreateRow(i + 2);
                    for (int j = 0; j < dt4.Columns.Count; j++)
                    {
                        ICell cell = row8.CreateCell(j);
                        cell.SetCellValue(dt4.Rows[i][j].ToString());
                    }
                }

                //转为字节数组  
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                var buf = stream.ToArray();
                //Thread.Sleep(1000); 
                //保存为Excel文件  
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }


            /// <summary>
            /// Datable导出成Excel
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file">导出路径(包括文件名与扩展名)</param>
            public static void TableToExcel5(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4, DataTable dt5, string sheetName1, string sheetName2, string sheetName3, string sheetName4, string sheetName5, string file)
            {
                IWorkbook workbook;
                string fileExt = Path.GetExtension(file).ToLower();
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    workbook = null;
                }
                if (workbook == null)
                {
                    return;
                }



                ISheet sheet1 = workbook.CreateSheet(sheetName1);
                ISheet sheet2 = workbook.CreateSheet(sheetName2);
                ISheet sheet3 = workbook.CreateSheet(sheetName3);
                ISheet sheet4 = workbook.CreateSheet(sheetName4);
                ISheet sheet5 = workbook.CreateSheet(sheetName5);
                //表头  
                IRow row1 = sheet1.CreateRow(0);
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    ICell cell = row1.CreateCell(i);
                    cell.SetCellValue(dt1.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    IRow row2 = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt1.Columns.Count; j++)
                    {
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue(dt1.Rows[i][j].ToString());
                    }
                }

                //表头  
                IRow row3 = sheet2.CreateRow(0);
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    ICell cell = row3.CreateCell(i);
                    cell.SetCellValue(dt2.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    IRow row4 = sheet2.CreateRow(i + 2);
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        ICell cell = row4.CreateCell(j);
                        cell.SetCellValue(dt2.Rows[i][j].ToString());
                    }
                }


                //表头  
                IRow row5 = sheet3.CreateRow(0);
                for (int i = 0; i < dt3.Columns.Count; i++)
                {
                    ICell cell = row5.CreateCell(i);
                    cell.SetCellValue(dt3.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    IRow row6 = sheet3.CreateRow(i + 2);
                    for (int j = 0; j < dt3.Columns.Count; j++)
                    {
                        ICell cell = row6.CreateCell(j);
                        cell.SetCellValue(dt3.Rows[i][j].ToString());
                    }
                }


                //表头  
                IRow row7 = sheet4.CreateRow(0);
                for (int i = 0; i < dt4.Columns.Count; i++)
                {
                    ICell cell = row7.CreateCell(i);
                    cell.SetCellValue(dt4.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    IRow row8 = sheet4.CreateRow(i + 2);
                    for (int j = 0; j < dt4.Columns.Count; j++)
                    {
                        ICell cell = row8.CreateCell(j);
                        cell.SetCellValue(dt4.Rows[i][j].ToString());
                    }
                }


                //表头  
                IRow row9 = sheet5.CreateRow(0);
                for (int i = 0; i < dt5.Columns.Count; i++)
                {
                    ICell cell = row9.CreateCell(i);
                    cell.SetCellValue(dt5.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    IRow row10 = sheet5.CreateRow(i + 2);
                    for (int j = 0; j < dt5.Columns.Count; j++)
                    {
                        ICell cell = row10.CreateCell(j);
                        cell.SetCellValue(dt5.Rows[i][j].ToString());
                    }
                }

                //转为字节数组  
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                var buf = stream.ToArray();
                //Thread.Sleep(1000); 
                //保存为Excel文件  
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// Datable导出成Excel
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file">导出路径(包括文件名与扩展名)</param>
            public static void TableToExcel1(DataTable dt, string sheetName, string file)
            {
                IWorkbook workbook;
                string fileExt = Path.GetExtension(file).ToLower();
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    workbook = null;
                }
                if (workbook == null)
                {
                    return;
                }



                ISheet sheet1 = workbook.CreateSheet(sheetName);


                //表头  
                IRow row1 = sheet1.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row1.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);

                }

                //数据  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row2 = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }



                //转为字节数组  
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                var buf = stream.ToArray();
                //Thread.Sleep(1000); 
                //保存为Excel文件  
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "PLC_To_init_Path", textBox22.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "enable", checkBox4.Checked.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "PLC_funcode", textBox24.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_NUM", numericUpDown5.Value.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_Value_length", numericUpDown4.Value.ToString());
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
           // IniAPI.INIWriteValue(parma_path, "System", "Passive_get_code_enable", checkBox5.Checked.ToString());

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
           // IniAPI.INIWriteValue(parma_path, "System", "initiative_get_code_enable", checkBox6.Checked.ToString());
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox22.Text= IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "PLC_To_init_Path","" );
            checkBox4.Checked=Convert.ToBoolean( IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "enable", "false"));
            textBox24.Text=IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "PLC_funcode", "");
            numericUpDown5.Value =Convert.ToDecimal( IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_NUM", "0"));
            numericUpDown4.Value= Convert.ToDecimal(IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_Value_length","0"));
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
           // IniAPI.INIWriteValue(parma_path, "System", "initiative_Get_On", textBox25.Text.ToString());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (button11.Text == "打开串口")
            {
                rS232 = new RS232();
                rS232.BaudRate = Convert.ToInt32(comboBox6.Text);
                rS232.DataBits = Convert.ToInt32(comboBox3.Text);
                rS232.StopBits = Convert.ToInt32(comboBox4.Text);
                rS232.COMPort = comboBox7.Text;
                rS232.Parity = comboBox5.SelectedIndex;





                rS232.Init();
                if (!(rS232.Open() == 1))
                {
                    button11.BackColor = Color.Red;
                    MessageBox.Show("螺丝机控制器连接失败");
                }
                else
                {

                    panel2.Enabled = true;
                    //int brightness = 150;
                    button11.Text = "关闭串口";
                    button11.BackColor = Color.Green;
                    //  rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                    //  button12.Text = "关闭光源";
                }
            }
            else
            {
                rS232.Close();
                button11.Text = "打开串口";
                button11.BackColor = Color.White;
              
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, "COM", "BaudRate", comboBox6.Text);
            IniAPI.INIWriteValue(parma_path, "COM", "DataBits", comboBox3.Text);
            IniAPI.INIWriteValue(parma_path, "COM", "StopBits", comboBox4.Text);
            IniAPI.INIWriteValue(parma_path, "COM", "COMPort", comboBox7.Text);
            IniAPI.INIWriteValue(parma_path, "COM", "Parity", comboBox5.SelectedIndex.ToString());
            IniAPI.INIWriteValue(parma_path, "COM", "Functioncode", textBox26.Text);
            MessageBox.Show("保存成功");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string AA = rS232.SerialPort.ReadLine();
                string[] BB = AA.Split(Convert.ToChar(textBox26.Text));
                textBox27.Text = BB[19];
                textBox28.Text = BB[21];
                textBox29.Text = BB[25];

                // getdatatoexcel3("1");
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败，请先正确配置螺丝控制器");
            }
       
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox7.Checked)
            {
                panel8.Enabled = false;
                IniAPI.INIWriteValue(parma_path, "System", "LOCK", "false");
            }
            else
            {
                panel8.Enabled = true;
                IniAPI.INIWriteValue(parma_path, "System", "LOCK", "true");
            }
        }

        private void checkBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, "System", "USE_CODE", checkBox5.Checked.ToString());
        }

        private void textBox23_TextChanged_1(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, "System", "Turn_on_path", textBox23.Text.ToString());
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox22.Text = IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "PLC_To_init_Path", "");
            checkBox4.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "enable", "false"));
            textBox24.Text = IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "PLC_funcode", "");
            numericUpDown5.Value = Convert.ToDecimal(IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_NUM", "0"));
            numericUpDown4.Value = Convert.ToDecimal(IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_Value_length", "0"));

            textBox31.Text= IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_Value_UP", "");
            textBox25.Text = IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_Value_DOWN","");
            textBox30.Text= IniAPI.INIGetStringValue(parma_path, comboBox2.Text, "Pressure_Value_SET", "");


        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "PLC_To_init_Path", textBox22.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "enable", checkBox4.Checked.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "PLC_funcode", textBox24.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_NUM", numericUpDown5.Value.ToString());
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_Value_length", numericUpDown4.Value.ToString());

            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_Value_UP", textBox31.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_Value_DOWN", textBox25.Text);
            IniAPI.INIWriteValue(parma_path, comboBox2.Text, "Pressure_Value_SET", textBox30.Text);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
