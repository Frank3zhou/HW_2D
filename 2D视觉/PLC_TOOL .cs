
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Siemens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace _6524
{
   
    public partial class PLC_TOOL : Form
    {

        private MelsecMcNet MC_PLC ;
        private ModbusTcpNet Modbus_PLC;
        string _IP ="192.168.0.100";
        int _port = 502;
        bool _connected=false;
        string _Mode = "MC";
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
        //SiemensS7Net siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1200, "192.168.0.100" )

        //{
        //    ConnectTimeOut = 5000
        //    };
        public PLC_TOOL()
        {
            InitializeComponent();
             textBox1.Text= IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(IP), "");
            textBox2.Text = IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(Port), "");
            comboBox1.Text= IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(Mode), "");
            checkBox1.Checked= Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "PLC", "heartbeat_enabled", ""));
            textBox7.Text=IniAPI.INIGetStringValue(Param_Path, "PLC", "heartbeat_path","");
            textBox8.Text= IniAPI.INIGetStringValue(Param_Path, "PLC", "wait_time","1000" );
        }

        public string IP
        {
            get
            {
                return _IP;
            }

            set
            {
                _IP = value;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
            }
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }

            set
            {
                _connected = value;
            }
        }

        public string Mode
        {
            get
            {
                return _Mode;
            }

            set
            {
                _Mode = value;
            }
        }

        private void PLC_TOOL_Load(object sender, EventArgs e)
        {
            textBox6.BringToFront();
      
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IP = textBox1.Text;
                Port = Convert.ToInt16(textBox2.Text);
                Mode = comboBox1.Text;

                if (!Connected)
                {
                    if (Mode == "MC")
                    {
                        MC_PLC = new MelsecMcNet(IP, Port);
                        MC_PLC.ConnectTimeOut = 2000; // 网络连接的超时时间

                        OperateResult connect = MC_PLC.ConnectServer();
                        if (connect.IsSuccess)
                        {
                            //bool[] M100 = m_PLC.ReadBool("M100", 1).Content;
                            MessageBox.Show("连接成功！");
                            button1.Text = "连接中";
                            button1.BackColor = Color.LightGreen;
                            Connected = true;
                            groupBox2.Enabled = true;
                           textBox1.Enabled=false;
                             textBox2.Enabled=false;
                             comboBox1.Enabled=false;
                        }
                        else
                        {
                            MessageBox.Show("连接失败！");

                        }
                    }
                    else if (Mode == "ModbusTCP")
                    {
                        Modbus_PLC = new ModbusTcpNet(IP, Port);
                        Modbus_PLC.ConnectTimeOut = 2000; // 网络连接的超时时间
                        OperateResult connect = Modbus_PLC.ConnectServer();
                        if (connect.IsSuccess)
                        {
                            //bool[] M100 = m_PLC.ReadBool("M100", 1).Content;
                            MessageBox.Show("连接成功！");
                            button1.Text = "连接中";
                            button1.BackColor = Color.LightGreen;
                            Connected = true;
                            groupBox2.Enabled = true;
                            textBox1.Enabled = false;
                            textBox2.Enabled = false;
                            comboBox1.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("连接失败！");

                        }
                    }
                    else
                    {
                        MessageBox.Show("模式错误！");
                    }
                }
                else
                {
                    if (Mode == "MC")
                    {
                        MC_PLC.ConnectClose();
                        button1.Text = "连接";
                        button1.BackColor = System.Drawing.SystemColors.ControlLight;
                        Connected = false;
                        groupBox2.Enabled = false;
                        textBox1.Enabled = true;
                        textBox2.Enabled = true;
                        comboBox1.Enabled = true;


                    }
                    else if (Mode == "ModbusTCP")
                    {
                        Modbus_PLC.ConnectClose();
                        button1.Text = "连接";
                        button1.BackColor = System.Drawing.SystemColors.ControlLight;
                        Connected = false;
                        groupBox2.Enabled = false;
                        textBox1.Enabled = true;
                        textBox2.Enabled = true;
                        comboBox1.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("模式错误！");
                    }
                }
             
              
            }
            catch (Exception EX )
            {

                MessageBox.Show(EX.ToString());
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IP = textBox1.Text;
            Port = Convert.ToInt16(textBox2.Text);
            Mode = comboBox1.Text;
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(IP), IP);
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(Port), Port.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(Mode), Mode.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "heartbeat_enabled", checkBox1.Checked.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "heartbeat_path", textBox7.Text.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "wait_time", textBox8.Text.ToString());
            MessageBox.Show("保存成功");
        }

        private void button_write_bool_Click(object sender, EventArgs e)
        {
            try
            {
                
                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToBoolean(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result =  Modbus_PLC.Write(textBox_write_address.Text, Convert.ToBoolean(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else 
                    {
                        MessageBox.Show("写入失败");
                    }
                    
                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void PLC_TOOL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connected)
            {

                if (Mode == "MC")
                {
                    MC_PLC.ConnectClose();
                }
                else if (Mode == "ModbusTCP")
                {
                    Modbus_PLC.ConnectClose();
                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
        }

        private void button_write_short_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToInt16(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToInt16(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_int_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToInt32(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToInt32(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_string_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, textBox_write_text.Text);// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, textBox_write_text.Text);// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_read_bool_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {

               
                if (Mode == "MC")
                {
                    OperateResult<bool[]> Result = MC_PLC.ReadBool(textBox3.Text,Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i]+"\n\r";
                        }
                    

                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<bool[]> Result = Modbus_PLC.ReadBool(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_short_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<Int16[]> Result = MC_PLC.ReadInt16(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<Int16[]> Result = Modbus_PLC.ReadInt16(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_int_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<Int32[]> Result = MC_PLC.ReadInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<Int32[]> Result = Modbus_PLC.ReadInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        
        }

        private void button_read_float_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<float[]> Result = MC_PLC.ReadFloat(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<float[]> Result = Modbus_PLC.ReadFloat(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_double_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<double[]> Result = MC_PLC.ReadDouble(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<double[]> Result = Modbus_PLC.ReadDouble(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_write_double_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToDouble(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToDouble(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_read_string_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    textBox6.Text = MC_PLC.ReadString(textBox3.Text, Convert.ToUInt16(textBox4.Text)).Content; // 写入线圈100为通
                  
                }
                else if (Mode == "ModbusTCP")
                {
                    textBox6.Text = Modbus_PLC.ReadString(textBox3.Text, Convert.ToUInt16(textBox4.Text)).Content; // 写入线圈100为通
                  
                    
                    

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_write_long_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToInt64(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToInt64(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_byte_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToBoolean(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToInt16(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_ushort_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToUInt16(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToUInt16(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_uint_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToUInt32(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToUInt32(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_ulong_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, Convert.ToUInt64(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, Convert.ToUInt64(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_write_hex_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text, ToHexBytes(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, ToHexBytes(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        public  byte[] ToHexBytes( string value)
        {
            return SoftBasic.HexStringToBytes(value);
        }

        private void button_write_float_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
                if (Mode == "MC")
                {
                    OperateResult Result = MC_PLC.Write(textBox_write_address.Text,(float)Convert.ToDouble(textBox_write_text.Text));// 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");

                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    //bool coil100 = Modbus_PLC.ReadBool(textBox_write_address.Text).Content;   // 读取线圈100的通断
                    OperateResult Result = Modbus_PLC.Write(textBox_write_address.Text, (float)Convert.ToDouble(textBox_write_text.Text));// 写入寄存器100为12345
                    if (Result.IsSuccess)
                    {
                        LB_runtime.Text = (DateTime.Now - start).ToString("fff");
                        MessageBox.Show("写入成功");
                    }
                    else
                    {
                        MessageBox.Show("写入失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void button_read_byte_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<byte[]> Result = MC_PLC.Read(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<byte[]> Result = Modbus_PLC.Read(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_ushort_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<UInt16[]> Result = MC_PLC.ReadUInt16(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<UInt16[]> Result = Modbus_PLC.ReadUInt16(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_uint_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<UInt32[]> Result = MC_PLC.ReadUInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<UInt32[]> Result = Modbus_PLC.ReadUInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_long_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<Int32[]> Result = MC_PLC.ReadInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<Int32[]> Result = Modbus_PLC.ReadInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void button_read_ulong_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            try
            {


                if (Mode == "MC")
                {
                    OperateResult<UInt32[]> Result = MC_PLC.ReadUInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                else if (Mode == "ModbusTCP")
                {
                    OperateResult<UInt32[]> Result = Modbus_PLC.ReadUInt32(textBox3.Text, Convert.ToUInt16(textBox5.Text)); // 写入线圈100为通
                    if (Result.IsSuccess)
                    {
                        for (int i = 0; i < Convert.ToUInt16(textBox5.Text); i++)
                        {
                            textBox6.Text += Result.Content[i] + "\n\r";
                        }


                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }

                }
                else
                {
                    MessageBox.Show("模式错误！");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }
    }
}
