
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace FANUCTCP
{
    /// <summary>TCP/IP連線調用模組類別</summary>
	public class TCPIP
    {


        /*========================================================================*/
        /*    變數宣告                                                             */
        /*========================================================================*/

        private System.Net.Sockets.Socket m_Listener;
        private System.Net.Sockets.Socket m_Socket;
        private string m_IpAddress;
        private int m_Port;
        private Dictionary<string, System.Net.Sockets.Socket> m_Clients;
        private bool m_ConnectFlag;
        /*========================================================================*/
        /*    Constructor                                                         */
        /*========================================================================*/
        /// <summary>建構函式</summary>
        public TCPIP()
        {
            m_IpAddress = "127.0.0.1";
            m_Port = 2023;
            m_Clients = new Dictionary<string, System.Net.Sockets.Socket>();
        }
        /// <summary>解構函式</summary> 
        ~TCPIP()
        {
            if (m_Listener != null)
                m_Listener.Dispose();
            if (m_Socket != null)
                m_Socket.Dispose();
        }
        /*========================================================================*/
        /*    變數存取                                                             */
        /*========================================================================*/
        /// <summary>IP位置設定</summary>
        /// <value>取得或設定IP位置：m_IpAddress</value>
        public string IpAddress
        {
            get { return m_IpAddress; }
            set { if (value is string) m_IpAddress = value; }
        }
        /// <summary>連接埠位置設定</summary>
        /// <value>取得或設定連接埠位置：m_Port</value>
        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }
        /// <summary>客戶端IP標籤</summary>
        /// <value>取得或設定連接埠位置：m_Clients</value>
        public Dictionary<string, System.Net.Sockets.Socket> Clients
        {
            get { return m_Clients; }
            set { m_Clients = value; }
        }

        public bool ConnectFlag
        {
            get { return m_ConnectFlag; }
            set { m_ConnectFlag = value; }
        }
        /*========================================================================*/
        /*    Function                                                            */
        /*========================================================================*/

        /// <summary>伺服器初始化</summary>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool SeverInitialize()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(m_IpAddress), m_Port);
            m_Listener = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_Listener.Bind(localEndPoint);  //Binding IP position
                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>伺服器監聽連線</summary> 
        /// <remarks> 
        /// <para>執行之前要先調用<see cref="SeverInitialize()"/></para>
        /// <para>預設超時為10秒</para>
        /// </remarks>
        /// <param name="clientIP">客戶端IP</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool ServerListen(out string clientIP)
        {
            try
            {
                m_Listener.Listen(10);
                m_Listener.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, 1000, 1000), null);
                m_Socket = m_Listener.Accept();

                clientIP = m_Socket.RemoteEndPoint.ToString();

                //接收與傳送逾時
                //m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                //m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);

                m_Clients.Add(clientIP, m_Socket);

                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>客戶端連線</summary> 
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool ClientConnect()
        {
            bool rtn = false;
            try
            {

                m_Socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Socket.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, 1000, 1000), null);
                //m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                //m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);

                if (m_Socket.Connected == false)
                {
                    //m_Socket.Connect(m_IpAddress, m_Port);

                    IAsyncResult ConnectResult = m_Socket.BeginConnect(m_IpAddress, m_Port, null, null);
                    ConnectResult.AsyncWaitHandle.WaitOne(2000, true);  //等待2秒

                    if (!ConnectResult.IsCompleted)
                    {
                        m_Socket.Close();
                        //处理连接不成功的动作
                        rtn = false;
                    }
                    else
                    {
                        //处理连接成功的动作
                        rtn = true;

                    }

                }

                return rtn;

            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);

            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>中斷連線</summary>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool DisConnect()
        {
            try
            {
                if (m_Socket != null)
                {
                    if (m_Socket.Connected == true)
                    {
                        m_Socket.Shutdown(SocketShutdown.Both);
                    }
                    m_Socket.Close();
                }
                if (m_Listener != null)
                {
                    if (m_Listener.Connected == true)
                    {
                        m_Listener.Shutdown(SocketShutdown.Both);
                    }
                    m_Listener.Close();
                }
                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>傳送字串資料</summary>
        /// <param name="sendStr">要傳送的字串</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Send(string sendStr)
        {
            try
            {
                byte[] sendByteArr = Encoding.ASCII.GetBytes(sendStr);
                m_Socket.Send(sendByteArr, sendByteArr.Length, SocketFlags.None);

                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>傳送位元組資料</summary>
        /// <param name="sendByteArr">要傳送的資料的位元組</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Send(byte[] sendByteArr)
        {
            try
            {
                m_Socket.Send(sendByteArr, sendByteArr.Length, SocketFlags.None);

                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>傳送字串資料</summary>
        /// <param name="socket">Socket物件</param>
        /// <param name="sendStr">要傳送的字串</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Send(System.Net.Sockets.Socket socket, string sendStr)
        {
            try
            {
                byte[] sendByteArr = Encoding.ASCII.GetBytes(sendStr);
                socket.Send(sendByteArr, sendByteArr.Length, SocketFlags.None);

                return true;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>接收字串資料</summary>
        /// <param name="reciveStr">接收的字串</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Recive(out string reciveStr)
        {
            bool rtn = true;
            m_ConnectFlag = true;
            try
            {
                byte[] bytesReceived = new byte[1024];
                int recvByteNum = m_Socket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                reciveStr = System.Text.Encoding.ASCII.GetString(bytesReceived, 0, recvByteNum);

                if (recvByteNum > 0)
                {
                    rtn = true;
                    return rtn;
                }
                else
                {
                    m_ConnectFlag = false;
                    return false;
                }
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);

            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>接收位元組資料</summary> 
        /// <param name="reciveStr">接收的字串</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool BitRecive(out string reciveStr)
        {
            try
            {
                byte[] bytesReceived = new byte[1024];
                int recvByteNum = m_Socket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                reciveStr = BitConverter.ToString(bytesReceived); //轉成16進制

                if (recvByteNum > 0)
                    return true;
                else
                    return false;
            }
            catch (SocketException e)
            {
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>接收字串資料</summary> 
        /// <param name="socket">Socket物件</param>
        /// <param name="clientIP">Client的IP位置</param>
        /// <param name="reciveStr">接收的字串</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Recive(System.Net.Sockets.Socket socket, out string clientIP, out string reciveStr)
        {
            try
            {
                bool blockingState = socket.Blocking;
                try
                {
                    byte[] tmp = new byte[1];

                    socket.Blocking = false;
                    socket.Send(tmp, 0, 0);
                    Console.WriteLine("Connected!");
                }
                catch (SocketException e)
                {
                    // 10035 == WSAEWOULDBLOCK
                    if (e.NativeErrorCode.Equals(10035))
                        Console.WriteLine("Still Connected, but the Send would block");
                    else
                    {
                        Console.WriteLine("Disconnected: error code {0}..!", e.NativeErrorCode);
                        throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
                    }
                }
                finally
                {
                    socket.Blocking = blockingState;
                }
                string ip = "";
                bool rr = Poll(socket, out ip);
                Console.WriteLine("rr = " + rr);

                byte[] bytesReceived = new byte[1024];
                int recvByteNum = socket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                reciveStr = System.Text.Encoding.ASCII.GetString(bytesReceived, 0, recvByteNum);

                clientIP = socket.RemoteEndPoint.ToString();

                if (recvByteNum > 0)
                    return true;
                else
                    return false;
            }
            catch (SocketException e)
            {
                clientIP = "";
                reciveStr = "";
                return false;
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }

        /// <summary>確認連線狀態</summary> 
        /// <param name="socket">Socket物件</param>
        /// <param name="clientIP">Client的IP位置</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        public bool Poll(System.Net.Sockets.Socket socket, out string clientIP)
        {
            try
            {
                if (socket.Poll(-1, SelectMode.SelectWrite))
                {
                    Console.WriteLine("This Socket is writable.");
                }
                else if (socket.Poll(-1, SelectMode.SelectRead))
                {
                    Console.WriteLine("This Socket is readable.");
                }
                else if (socket.Poll(-1, SelectMode.SelectError))
                {
                    Console.WriteLine("This Socket has an error.");
                    clientIP = socket.RemoteEndPoint.ToString();
                    return false;
                }

                clientIP = socket.RemoteEndPoint.ToString();

                return true;
            }
            catch (SocketException e)
            {
                clientIP = socket.RemoteEndPoint.ToString();
                return false;
                throw new System.Exception(e.ErrorCode.ToString() + ":" + e.Message);
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }



        /// <summary>建立 keepalive 作業所需的輸入資料</summary>
        /// <param name="onOff">是否啟用1:on ,0:off</param>
        /// <param name="keepAliveTime">啟動時間</param>
        /// <param name="keepAliveInterval">啟動間隔</param>
        /// <returns>執行結果</returns>
        /// <exception cref="System.Exception">Thrown when error happened.</exception>
        private byte[] GetKeepAliveSetting(int onOff, int keepAliveTime, int keepAliveInterval)
        {

            try
            {
                byte[] buffer = new byte[12];
                BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
                BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
                BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
                return buffer;
            }
            catch (Exception err)
            {
                throw new System.Exception(err.Message);
            }
        }



        public bool IsConnected(string Type)//判断是否断线
        {
            System.Net.Sockets.Socket SocketTemp;
            bool blockingState;
            if (Type == "Server")
                SocketTemp = m_Listener;
            else
                SocketTemp = m_Socket;
            blockingState = SocketTemp.Blocking;
            try
            {


                byte[] tmp = new byte[1];
                SocketTemp.Blocking = false;
                SocketTemp.Send(tmp, 0, 0);

                return true;


            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK  
                if (e.NativeErrorCode.Equals(10035))
                    return true;
                else
                {
                    return false;
                }
            }
            finally
            {
                SocketTemp.Blocking = blockingState;
            }

            //   Console.WriteLine("Connected: {0}", SocketTemp.Connected);


        }


    }
}
