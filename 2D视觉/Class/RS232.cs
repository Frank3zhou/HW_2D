﻿using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace _6524
{
    public class RS232
    {
        SerialPort m_SP = new SerialPort();
        private StringBuilder builder = new StringBuilder();//避免在事件处理方法中反复的创建，定义到外面。
        public static String result;
        public static bool result_ok;
        string m_port = "COM4";
        int m_baud = 19200, m_stopBits = 1, m_parity = 0, m_dataBits = 8;
        object lockObject = new object();


        #region Init
        public void Init()
        {
            

            if (m_SP.IsOpen)
                Close();

        

            string[] myPorts = SerialPort.GetPortNames();

            m_SP.PortName = COMPort;
            m_SP.BaudRate = BaudRate;
            m_SP.StopBits = (StopBits)StopBits;
            m_SP.Parity = (Parity)Parity;
            m_SP.DataBits = DataBits;
            m_SP.RtsEnable = true;

        }
        #endregion


        #region 屬性
        public SerialPort SerialPort
        {

            get
            {
                lock (lockObject)
                {
                    return m_SP;
                }
            }
            set
            {
                lock (lockObject)
                {
                    m_SP = value;
                }
            }
        }

        public string COMPort
        {
            get { return m_SP.PortName; }
            set { m_SP.PortName = value; }
        }

        public int BaudRate
        {
            get { return m_SP.BaudRate; }
            set { m_SP.BaudRate = value; }
        }

        public int StopBits
        {
            get { return (int)m_SP.StopBits; }
            set { m_SP.StopBits = (StopBits)value; }
        }

        public int Parity
        {
            get { return (int)m_SP.Parity; }
            set { m_SP.Parity = (Parity)value; }
        }

        public int DataBits
        {
            get { return m_SP.DataBits; }
            set { m_SP.DataBits = value; }
        }

        public int ReadTimeout
        {
            get { return m_SP.ReadTimeout; }
            set { m_SP.ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return m_SP.WriteTimeout; }
            set { m_SP.WriteTimeout = value; }
        }
        #endregion

        #region 外部調用
        public int Open()
        {

            int result = -1;

            try
            {
                if (!m_SP.IsOpen)
                    m_SP.Open();

                if (m_SP.IsOpen)
                {

                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception)
            {
                result = 0;
            }

            return result;
        }




        public int Close()
        {
            int result = -1;

            try
            {
                m_SP.Close();

                result = 1;
            }
            catch (Exception)
            {
                result = 0;
            }

            return result;
        }
        #endregion
        public void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {




            result = "";
            result_ok = false;
            try
            {
                if (m_SP.IsOpen)
                {


                    result = m_SP.ReadExisting();    //读取串口数据(监听)
                    Thread.Sleep(500);
                    byte[] byteRead = new byte[m_SP.BytesToRead];


                    result_ok = true;
                    m_SP.DiscardInBuffer();

                    result_ok = false;
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    //Thread.Sleep(waitTime);
                }
            }
            catch (Exception)
            {

            }
        }

        private void RS232_Load(object sender, EventArgs e)
        {

        }

        String bytesToHexString(byte[] bArr)
        {
            string result = string.Empty;
            for (int i = 0; i < 13; i++)//逐字节变为16进制字符，以%隔开
            {
                result += Convert.ToString(bArr[i], 16).ToUpper().PadLeft(2, '0') + " ";
            }
            return result;
        }

    }
}
