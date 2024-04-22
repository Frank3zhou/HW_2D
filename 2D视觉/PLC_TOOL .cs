
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _6524.Class;
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

        private basePLC MC_PLC=new basePLC() ;
     
        string _IP ="192.168.0.100";
        int _port = 502;
        bool _connected=false;
        PLCMode  Mode = PLCMode.MC;
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
            checkBox1.Checked= Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "PLC", "heartbeat_enabled", "false"));
            checkBox2.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "PLC", "PLCinit_enabled", "false"));
            textBox10.Text = IniAPI.INIGetStringValue(Param_Path, "PLC", "PLCinit_Path", "1000");
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
              //  Mode = comboBox1.Text;

                if (!Connected)
                {
                    if (comboBox1.SelectedIndex==(int)PLCMode.ModbusTCP)
                    {
                        MC_PLC.pLCMode = PLCMode.ModbusTCP;


                    }
                    else if (comboBox1.SelectedIndex == (int)PLCMode.MC)
                    {
                        MC_PLC.pLCMode = PLCMode.MC;
                    }
                    else if (comboBox1.SelectedIndex == (int)PLCMode.FinsTCP)
                    {
                        MC_PLC.pLCMode = PLCMode.FinsTCP;
                    }
                    else if (comboBox1.SelectedIndex == (int)PLCMode.SiemensS7)
                    {
                        MC_PLC.pLCMode = PLCMode.SiemensS7;
                    }
                    else
                    {
                        MessageBox.Show("模式错误！");
                    }
                    MC_PLC.IP = IP;
                    MC_PLC.Port = Port; 
                    if (MC_PLC.init())
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
                  
                        MC_PLC.ConnectClose();
                        button1.Text = "连接";
                        button1.BackColor = System.Drawing.SystemColors.ControlLight;
                        Connected = false;
                        groupBox2.Enabled = false;
                        textBox1.Enabled = true;
                        textBox2.Enabled = true;
                        comboBox1.Enabled = true;


                    
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
          //  Mode = comboBox1.Text;
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(IP), IP);
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(Port), Port.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", nameof(Mode), comboBox1.Text.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "heartbeat_enabled", checkBox1.Checked.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "heartbeat_path", textBox7.Text.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "wait_time", textBox8.Text.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "PLCinit_enabled", checkBox2.Checked.ToString());
            IniAPI.INIWriteValue(Param_Path, "PLC", "PLCinit_Path", textBox10.Text.ToString());
            MessageBox.Show("保存成功");
        }

        private void button_write_bool_Click(object sender, EventArgs e)
        {
            try
            {
                
                DateTime start = DateTime.Now;
           
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
            catch (Exception)
            {

                MessageBox.Show("写入失败");
            }
        }

        private void PLC_TOOL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connected)
            {

               
                    MC_PLC.ConnectClose();
               
            }
        }

        private void button_write_short_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime start = DateTime.Now;
               
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


               
                    textBox6.Text = MC_PLC.ReadString(textBox3.Text, Convert.ToUInt16(textBox4.Text)).Content; // 写入线圈100为通
               
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
            catch (Exception)
            {

                MessageBox.Show("读取失败");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
