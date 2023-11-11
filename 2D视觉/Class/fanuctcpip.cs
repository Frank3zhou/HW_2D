using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524.Class
{
    public class fanuctcpip
    {

        TCPIP m_TCP = new TCPIP();
        Process process = new Process();
        public fanuctcpip()
        {
            //通过后台打开发那科服务器
            string path = Application.StartupPath + "\\FANUCTCP\\bin\\Debug\\FANUCTCP.exe";
            process.StartInfo.FileName = path;
            // process.StartInfo.Arguments = "/c" + "start "+ path;
            process.StartInfo.UseShellExecute = false;   //是否使用操作系统shell启动 
            process.StartInfo.CreateNoWindow = true;   //是否在新窗口中启动该进程的值 (不显示程序窗口)
            process.Start();
            Thread.Sleep(100);
            conneecservert();//连接后台程序


        }

        public bool connectFanuc(string IP)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("0," + IP);
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool writePR(string path, string value)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("2," + path + "," + value);
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool readPR(string path, ref string value)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("1," + path);

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                value = str[1] + "," + str[2] + "," + str[3] + "," + str[4] + "," + str[5] + "," + str[6] + ",";
                                return true;
                            }
                        }

                    }
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool readR(string path, ref string value)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("3," + path);

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                value = str[1];
                                return true;
                            }
                        }

                    }
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool WrieR(string path, string value)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("4," + path + "," + value);
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool disconnectrobot()
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("5," + "1");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool closerobotprogram()
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("6," + "1");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool conneecservert()//连接后台程序
        {
            try
            {
                bool connect = m_TCP.ClientConnect();
                return connect;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void close()
        {
            try
            {
                disconnectrobot();
                closerobotprogram();

                m_TCP.DisConnect();
                process.WaitForExit(10);  //等待程序执行完退出进程
                process.Close();
                process.Dispose();
            }
            catch (Exception)
            {


            }


        }

        public bool readNowPR(ref string value)//0,连接机械手
        {
            try
            {
                if (m_TCP.IsConnected("127.0.0.1"))
                    m_TCP.Send("7,");

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(20);
                    string receivestring;
                    if (m_TCP.Recive(out receivestring))
                    {
                        string[] str = receivestring.Split(',');
                        if (str.Length > 0)
                        {
                            if (Convert.ToInt32(str[0]) == 9)
                            {
                                return false;
                            }
                            else
                            {
                                value = str[1] + "," + str[2] + "," + str[3] + "," + str[4] + "," + str[5] + "," + str[6] + ",";
                                return true;
                            }
                        }

                    }
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public  void  Writelog(string log)//0,连接机械手
        {
            if (m_TCP.IsConnected("127.0.0.1"))
                m_TCP.Send(log);
        }
    }
}
