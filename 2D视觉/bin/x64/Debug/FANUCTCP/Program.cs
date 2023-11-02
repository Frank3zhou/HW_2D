using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FANUCTCP
{
    internal class Program
    {
        static Fanuc fanuc = new Fanuc();
        static TCPIP m_TCP = new TCPIP();


        static void Main(string[] args)
        {
            bool quitprgram = false;
            bool isfirst = true;
            if (m_TCP.SeverInitialize())
            {

                while (!quitprgram)
                {
                    Thread.Sleep(10);
                    

                    Thread.Sleep(10);
                    string IP;
                    if (isfirst)
                    {
                        if (m_TCP.ServerListen(out IP))
                        {
                            isfirst = false;
                            Console.WriteLine("Connect:" + IP);
                        }

                    }

                    string receivestr;
                    if (m_TCP.Recive(out receivestr))
                    {
                        Console.WriteLine("PC--->Robot:"+receivestr);
                        if (receivestr.Contains(','))
                        {
                            string[] command = receivestr.Split(',');
                            switch (command[0])
                            {
                                case "0"://连接发那科机械手IP (0,127.0.0.1)
                                   
                                    if (fanuc.InterfaceConnect(command[1]))
                                    {
                                        Showsend(command[0] + "," + "Robbot_Connect_OK");
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Robot_Connect_NG");
                                    }

                                    break;

                                case "1"://读取点位(1,1)(读取PR1的点位)
                                    Array xyzpwr;
                                    if (fanuc.readPR(Convert.ToInt32(command[1]), out xyzpwr))
                                    {
                                        Showsend(command[0] + "," + xyzpwr.GetValue(0) + "," + xyzpwr.GetValue(1) + "," + xyzpwr.GetValue(2) + "," + xyzpwr.GetValue(3) + "," + xyzpwr.GetValue(4) + "," + xyzpwr.GetValue(5));
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Read_Point_NG");
                                    }
                                        break;
                                    

                                case "2"://写取点位（2，1，0，0，0，0，0，0）写入点位PR1为0，0，0，0，0，0
                                    Array xyzpwr2 = new float[9];
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[2]), 0);
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[3]), 1);
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[4]), 2);
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[5]), 3);
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[6]), 4);
                                    xyzpwr2.SetValue((float)Convert.ToDouble(command[7]), 5);
                                    if (fanuc.writePR(Convert.ToInt32(command[1]), xyzpwr2))
                                    {
                                        Showsend(command[0] + "," + "Write_OK");
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Read_Point_NG");
                                    }
                                    break;

                                case "3"://读R值 （3，1）读取R1
                                    object r =null ;
                                    if (fanuc.readRDI(Convert.ToInt32(command[1]), ref r))
                                    {

                                        Showsend(command[0] + "," + r);
                                        //Showsend(command[0] + "," + xyzpwr2.GetValue(0) + "," + xyzpwr2.GetValue(1) + "," + xyzpwr2.GetValue(2) + "," + xyzpwr2.GetValue(3) + "," + xyzpwr2.GetValue(4) + "," + xyzpwr2.GetValue(5));
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Read_R_NG");
                                    }
                                    break;


                                case "4"://写R值 （4，1，5）写入R[1]为5
                           
                                    if (fanuc.writeRDI(Convert.ToInt32(command[1]), Convert.ToInt32(command[2])))
                                    {

                                        Showsend(command[0] + "," + "Write_OK");
                                        //Showsend(command[0] + "," + xyzpwr2.GetValue(0) + "," + xyzpwr2.GetValue(1) + "," + xyzpwr2.GetValue(2) + "," + xyzpwr2.GetValue(3) + "," + xyzpwr2.GetValue(4) + "," + xyzpwr2.GetValue(5));
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Write_NG");
                                    }
                                    break;

                                case "5"://断开机械手连接

                                 
                                    fanuc.disconnect();
                                    Showsend(command[0]+"Disconnect_Robot_OK");
                                    break;

                                case "6"://关闭这个程序
                                  
                                    m_TCP.DisConnect();
                                    quitprgram = true;
                                    break;

                                case "7"://获取当前坐标

                                    Array xyzpwr4=null;
                                    if (fanuc.getnowpoint( ref xyzpwr4))
                                    {
                                        Showsend(command[0] + "," + xyzpwr4.GetValue(0) + "," + xyzpwr4.GetValue(1) + "," + xyzpwr4.GetValue(2) + "," + xyzpwr4.GetValue(3) + "," + xyzpwr4.GetValue(4) + "," + xyzpwr4.GetValue(5));
                                    }
                                    else
                                    {
                                        Showsend("9" + "," + "Read_Point_NG");
                                    }
                                 
                                    break;
                            }

                        }

                    }

                }



            }
        }

        private static void  Showsend(string str)
        {
            if (m_TCP != null)
            { 
                m_TCP.Send(str);
                Console.WriteLine("Robot--->PC:"+str);
            }
        }

    }
}
