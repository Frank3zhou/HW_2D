using _6524.Class;
using HalconDotNet;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _6524
{
    public partial class Form1 : Form
    {
        SetModel m_modelSet = new SetModel();
        private ZKHwindows _mWindow1 = new ZKHwindows();
        private ZKHwindows _mWindow2 = new ZKHwindows();
        private ZKHwindows _mWindow3 = new ZKHwindows();
        private ZKHwindows _mWindow4 = new ZKHwindows();
        private ZKHwindows _mWindow5 = new ZKHwindows();
        Form_Init form_Init = new Form_Init();
        private ILogNet logNet; //创建log地址
        SaveCallback Savecameranini;
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
        string Path_calibration_Param = Application.StartupPath + @"\\calibration\Param.ini";
        bool heartbeat_enabled;
        string heartbeat_path;
        private static string comname = "COM2";
        CameraParam cameraParam;
        Camera_Form camera_Form;
        State state = new State();
        int SystemLanguage = 0;
        ResourceManager resourceManager;
        bool PLC_connected = false;
        bool Camera1_connected = false;
        bool Camera2_connected = false;
        bool Camera3_connected = false;
        bool Camera4_connected = false;
        MVS_SDK m_Camera1 = new MVS_SDK();
        MVS_SDK m_Camera2 = new MVS_SDK();
        MVS_SDK m_Camera3 = new MVS_SDK();
        MVS_SDK m_Camera4 = new MVS_SDK();
        private MelsecMcNet MC_PLC;
        RS232 rS232 = new RS232();
        int brightness = 150;
        DataTable d1 = new DataTable(); // 相机参数表
        DataTable d2 = new DataTable(); // 相机结果表
        //bool AutoModel=false;//是否启用自动切换机种
        bool Saveimage_Enabled = false;
        bool Deleteimage_Enabled = false;
        double DeleteImagedays = 0;
        string saveimagepath = "";
        string saveImageFormat = "";
        bool robotenabled = false;
        bool autochangemodel = false;
        bool robot_connected =false  ;
        bool robotcamera_connected = false;


        //  private bool isPaused = false;  //主线程暂停

        // 工位图像处理结果
        List<bool> imageresult1 = new List<bool>();
        List<bool> imageresult2 = new List<bool>();
        List<bool> imageresult3 = new List<bool>();
        List<bool> imageresult4 = new List<bool>();
        HObject Img = new HObject();//相机拍照的图片

        fanuctcpip m_Robot;
        MVS_SDK m_Camera; //机器人相机

        //   public string Comname { get => Comname1; set => Comname1 = value; }
        public static string Comname1 { get => comname; set => comname = value; }

        public Form1()
        {

            SystemLanguage = Convert.ToInt16(IniAPI.INIGetStringValue(Param_Path, "System", "Language", "0"));
            if (SystemLanguage == 0)
            {

                resourceManager = new ResourceManager("_6524.Resources.CN_variable", typeof(Program).Assembly);
            }
            else if (SystemLanguage == 1)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                HslCommunication.StringResources.SeteLanguageEnglish();
                resourceManager = new ResourceManager("_6524.Resources.EN_variable", typeof(Program).Assembly);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
                HslCommunication.StringResources.SeteLanguageEnglish();
                resourceManager = new ResourceManager("_6524.Resources.ES_variable", typeof(Program).Assembly);
            }
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_system_state.Text = changelanguage("运行状态");
            int cameranum = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "System", "CameraNum", "1"));
            robotenabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "System", "RobotEnabled", "false"));
            autochangemodel = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "System", "AutoChangeModel", "false"));
            _mWindow1.Dock = DockStyle.Fill;
            _mWindow2.Dock = DockStyle.Fill;
            _mWindow3.Dock = DockStyle.Fill;
            _mWindow4.Dock = DockStyle.Fill;
            _mWindow5.Dock = DockStyle.Fill;


            if (robotenabled)
            {

                if (cameranum == 1)
                {
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.RowCount = 1;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow5, 1, 0);
                }
                else if (cameranum == 0)
                {
                    tableLayoutPanel1.ColumnCount = 1;
                    tableLayoutPanel1.RowCount = 1;
                    tableLayoutPanel1.Controls.Add(_mWindow5, 0, 0);

                }
                else if (cameranum == 2)
                {
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.RowCount = 2;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow2, 1, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow5, 0, 1);


                }
                else if (cameranum == 3)
                {
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.RowCount = 2;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow2, 1, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow3, 0, 1);
                    tableLayoutPanel1.Controls.Add(_mWindow5, 1, 1);
                }

            }
            else
            {
                if (cameranum == 1)
                {
                    tableLayoutPanel1.ColumnCount = 1;
                    tableLayoutPanel1.RowCount = 1;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                }
                else if (cameranum == 2)
                {
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.RowCount = 1;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow2, 1, 0);
                }
                else
                {
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.RowCount = 2;
                    tableLayoutPanel1.Controls.Add(_mWindow1, 0, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow2, 1, 0);
                    tableLayoutPanel1.Controls.Add(_mWindow3, 0, 1);
                    tableLayoutPanel1.Controls.Add(_mWindow4, 1, 1);
                }
            }

            logNet = new LogNetSingle(Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");
            LogNetSingle logNetSingle = logNet as LogNetSingle;
            string logData = logNetSingle.GetAllSavedLog();
            logNetAnalysisControl1.Load += FormLogNetTest_Load;
            panel2.BringToFront();

            //获取所有的日志信息




            Savecameranini = savecameraini;


            form_Init.Show();
            form_Init.Focus();

            string IP = IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(IP), "");
            string Port = IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(Port), "");
            string Mode = IniAPI.INIGetStringValue(Param_Path, "PLC", nameof(Mode), "");
            heartbeat_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "PLC", "heartbeat_enabled", ""));
            heartbeat_path = IniAPI.INIGetStringValue(Param_Path, "PLC", "heartbeat_path", "");
            Saveimage_Enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "SaveImage", "SaveImage_Enabled", ""));
            Deleteimage_Enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "SaveImage", "DeleteImage_Enabled", ""));
            DeleteImagedays = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "SaveImage", "DeleteImagedays", ""));
            saveimagepath = IniAPI.INIGetStringValue(Param_Path, "SaveImage", "SaveImagePath", "");
            saveImageFormat = IniAPI.INIGetStringValue(Param_Path, "SaveImage", "ImageFormat", "");
            //连接PLC
            UpdateInit(20, changelanguage("PLC连接中"));
            MC_PLC = new MelsecMcNet(IP, Convert.ToInt32(Port));
            MC_PLC.ConnectTimeOut = 2000; // 网络连接的超时时间
            OperateResult connect = MC_PLC.ConnectServer();
            if (connect.IsSuccess)
            {
                PLC_connected = true;
                Thread.Sleep(500);
                UpdateInit(40, changelanguage("PLC连接完成"));
            }
            else
            {
                PLC_connected = true;
                Thread.Sleep(500);
                UpdateInit(40, changelanguage("PLC连接失败"));
                btn_system_state.Text = changelanguage("PLC连接失败");
                m_Logprint(HslMessageDegree.ERROR, "PLC连接失败", true);
            }


            rS232.BaudRate = 19200;
            rS232.DataBits = 8;
            rS232.StopBits = 1;
            rS232.COMPort = Comname1;
            rS232.Parity = 0;
            rS232.Init();

            if (!(rS232.Open() == 1))
            {
                m_Logprint(HslMessageDegree.ERROR, "光源控制器连接失败", true);
            }
            else
            {
                brightness = 150;
                rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SC0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SD0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
            }

            rS232.Close();


            Thread.Sleep(500);
            UpdateInit(60, changelanguage("相机连接中"));

            if (cameranum >= 1)
            {
                if (m_Camera1.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机1", "IP", "")))
                {
                    Thread.Sleep(500);
                    Camera1_connected = true;
                    UpdateInit(100, changelanguage("相机1连接完成"));
                    m_Camera1.close();
                }
                else
                {
                    Thread.Sleep(500);
                    Camera1_connected = false;
                    UpdateInit(90, changelanguage("相机1连接失败"));
                    m_Logprint(HslMessageDegree.ERROR, "相机1连接失败", true);



                }
            }
            if (cameranum >= 2)
            {


                if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机2", "IP", "")))
                {
                    Thread.Sleep(500);
                    Camera2_connected = true;
                    UpdateInit(100, changelanguage("相机2连接完成"));
                }
                else
                {
                    Thread.Sleep(500);
                    Camera2_connected = false;
                    UpdateInit(90, changelanguage("相机2连接失败"));
                    m_Logprint(HslMessageDegree.ERROR, "相机2连接失败", true);
                    m_Camera2.close();
                }


            }
            if (cameranum >= 3)

            {
                if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机3", "IP", "")))
                {
                    Thread.Sleep(500);
                    Camera2_connected = true;
                    UpdateInit(100, changelanguage("相机3连接完成"));
                }
                else
                {
                    Thread.Sleep(500);
                    Camera2_connected = false;
                    UpdateInit(90, changelanguage("相机3连接失败"));
                    m_Logprint(HslMessageDegree.ERROR, "相机3连接失败", true);
                    m_Camera3.close();
                }
            }
            if (cameranum >= 4)

            {
                if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机4", "IP", "")))
                {
                    Thread.Sleep(500);
                    Camera4_connected = true;
                    UpdateInit(100, changelanguage("相机4连接完成"));
                    m_Camera4.close();
                }
                else
                {
                    Thread.Sleep(500);
                    Camera4_connected = false;
                    UpdateInit(90, changelanguage("相机4连接失败"));
                    m_Logprint(HslMessageDegree.ERROR, "相机4连接失败", true);
                }
            }

            if (Deleteimage_Enabled)
            {
                DeleteImage();
                UpdateInit(100, changelanguage("定期删除图片中"));
            }



            form_Init.Close();
            form_Init.Dispose();

            文件ToolStripMenuItem.Visible = false;
            pLC设置ToolStripMenuItem.Enabled = false;
            相机设置ToolStripMenuItem.Enabled = false;
            模型设置ToolStripMenuItem.Enabled = false;
            通讯设置ToolStripMenuItem.Enabled = false;
            开发者选项ToolStripMenuItem.Enabled = false;

            机械手示教ToolStripMenuItem.Enabled = false;
            机械手控制ToolStripMenuItem.Enabled = false;


        }

        private void savecameraini(CameraParam Ca_Param)
        {
            IniAPI.INIWriteValue(Param_Path, Ca_Param.Name, nameof(Ca_Param.Ip), Ca_Param.Ip);
            IniAPI.INIWriteValue(Param_Path, Ca_Param.Name, nameof(Ca_Param.Exposure), Ca_Param.Exposure.ToString());
            IniAPI.INIWriteValue(Param_Path, Ca_Param.Name, nameof(Ca_Param.TriggerMode), Ca_Param.TriggerMode);

        }
        private void UpdateInit(int stepValue, string info)
        {
            form_Init.Message = "";
            form_Init.Message = info;
            form_Init.StepValue = stepValue;
            Thread.Sleep(50);
        }
        private void FormLogNetTest_Load(object sender, EventArgs e)
        {
            try
            {
                string source = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt"));   // 传入路径
                logNetAnalysisControl1.SetLogNetSource(source);
            }
            catch (Exception)
            {

                //throw;
            }
            // 该source可以是本地读取的文件，也可以是网络发送过来的数据

        }

        private void Bg_Main_DoWork(object sender, DoWorkEventArgs e)
        {
            //工位拍照数
            int state1takepicturenumber = 1;
            int state2takepicturenumber = 1;
            string nowstep = "";
            while (!Bg_Main.CancellationPending)
            {

                //if (isPaused)
                //{
                //    // 线程暂停
                //    while (isPaused)
                //    {
                //        System.Threading.Thread.Sleep(100);
                //    }
                //}
                try
                {
                    #region 初始化


                    imageresult1.Clear();

                    imageresult2.Clear();

                    imageresult3.Clear();

                    imageresult4.Clear();

                    // 读取表格

                    Thread.Sleep(1);

                    int A = 1;


                    m_Logprint(HslMessageDegree.INFO, "初始化", true);
                    brightness = 0;

                    rS232.BaudRate = 19200;
                    rS232.DataBits = 8;
                    rS232.StopBits = 1;
                    rS232.COMPort = Comname1;
                    rS232.Parity = 0;
                    rS232.Init();

                    if (!(rS232.Open() == 1))
                    {
                        m_Logprint(HslMessageDegree.ERROR, "光源控制器连接失败", true);
                        break;
                    }
                    else
                    {
                        rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                        rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                        rS232.SerialPort.WriteLine("SC0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                        rS232.SerialPort.WriteLine("SD0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 

                    }

                    if (!PLC_connected)
                    {

                        MessageBox.Show(changelanguage("PLC连接失败"));
                        Bg_Main.CancelAsync();
                        button12.Image = global::_6524.Properties.Resources.开始__1_;

                        m_Logprint(HslMessageDegree.ERROR, "PLC连接失败", true);
                        break;
                    }
                    int cameranum = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "System", "CameraNum", "1"));


                    if (cameranum >= 1) //相机开始重连
                    {
                        if (m_Camera1.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机1", "IP", "")))
                        {
                            Thread.Sleep(500);
                            Camera1_connected = true;
                            UpdateInit(100, changelanguage("相机1连接完成"));
                            m_Camera1.close();
                        }
                        else
                        {
                            Thread.Sleep(500);
                            Camera1_connected = false;
                            UpdateInit(90, changelanguage("相机1连接失败"));
                            m_Logprint(HslMessageDegree.ERROR, "相机1连接失败", true);



                        }
                    }
                    if (cameranum >= 2)
                    {


                        if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机2", "IP", "")))
                        {
                            Thread.Sleep(500);
                            Camera2_connected = true;
                            UpdateInit(100, changelanguage("相机2连接完成"));
                        }
                        else
                        {
                            Thread.Sleep(500);
                            Camera2_connected = false;
                            UpdateInit(90, changelanguage("相机2连接失败"));
                            m_Logprint(HslMessageDegree.ERROR, "相机2连接失败", true);
                            m_Camera2.close();
                        }


                    }
                    if (cameranum >= 3)

                    {
                        if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机3", "IP", "")))
                        {
                            Thread.Sleep(500);
                            Camera2_connected = true;
                            UpdateInit(100, changelanguage("相机3连接完成"));
                        }
                        else
                        {
                            Thread.Sleep(500);
                            Camera2_connected = false;
                            UpdateInit(90, changelanguage("相机3连接失败"));
                            m_Logprint(HslMessageDegree.ERROR, "相机3连接失败", true);
                            m_Camera3.close();
                        }
                    }
                    if (cameranum >= 4)

                    {
                        if (m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机4", "IP", "")))
                        {
                            Thread.Sleep(500);
                            Camera4_connected = true;
                            UpdateInit(100, changelanguage("相机4连接完成"));
                            m_Camera4.close();
                        }
                        else
                        {
                            Thread.Sleep(500);
                            Camera4_connected = false;
                            UpdateInit(90, changelanguage("相机4连接失败"));
                            m_Logprint(HslMessageDegree.ERROR, "相机4连接失败", true);
                        }
                    }


                    if (cameranum >= 1)
                    {
                        if (!Camera1_connected)
                        {
                            MessageBox.Show(changelanguage("相机1连接失败"));
                            button12.Image = global::_6524.Properties.Resources.开始__1_;

                            m_Logprint(HslMessageDegree.ERROR, "相机1连接失败", true);
                            Bg_Main.CancelAsync();
                            break;
                        }
                    }
                    if (cameranum >= 2)
                    {
                        if (!Camera2_connected)
                        {
                            MessageBox.Show(changelanguage("相机2连接失败"));
                            button12.Image = global::_6524.Properties.Resources.开始__1_;

                            m_Logprint(HslMessageDegree.ERROR, "相机2连接失败", true);
                            Bg_Main.CancelAsync();
                            break;
                        }

                    }
                    if (cameranum >= 3)
                    {
                        if (!Camera3_connected)
                        {
                            MessageBox.Show(changelanguage("相机3连接失败"));
                            button12.Image = global::_6524.Properties.Resources.开始__1_;

                            m_Logprint(HslMessageDegree.ERROR, "相机3连接失败", true);
                            Bg_Main.CancelAsync();
                            break;
                        }

                    }
                    if (cameranum >= 4)
                    {
                        if (!Camera4_connected)
                        {
                            MessageBox.Show(changelanguage("相机4连接失败"));
                            button12.Image = global::_6524.Properties.Resources.开始__1_;

                            m_Logprint(HslMessageDegree.ERROR, "相机4连接失败", true);
                            Bg_Main.CancelAsync();
                            break;
                        }

                    }

                    //读取机种信息
                    string DCpath = IniAPI.INIGetStringValue(Param_Path, "ModelExcel", "Path", "C:\\Users\\Administrator\\Desktop\\6524");
                    string filenanme = "";
                    if (autochangemodel)
                    {




                        //自动切换机种，机种名必须为1.2.3
                        int model = MC_PLC.ReadInt32("D100").Content;
                        m_Logprint(HslMessageDegree.INFO, "读取机种结果：" + model.ToString(), false);
                        // m_modelSet.Modelname = "PLCmodel" + model.ToString();
                        filenanme = DCpath + "\\" + model.ToString() + ".xlsx";

                    }
                    else
                    {
                        //手动指定机种,机种名必须为1.2.3
                        string  str= IniAPI.INIGetStringValue(Param_Path, "ModelExcel", "UsingExcelPath", "");
                        filenanme = DCpath + "\\" + str;
                      string t = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");



                    }

                    try
                    {
                        d1 = SetModel.ReadExcelToDataTable(filenanme, "Sheet1", true);
                        d2 = SetModel.ReadExcelToDataTable(filenanme, "Sheet2", true);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    if (d1 == null)
                    {
                        MessageBox.Show("配方表载入失败");
                    }


                    Thread.Sleep(10);

                    #endregion
                    //完整的相机拍照处理流程
                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        if (Bg_Main.CancellationPending)
                            break;
                        if (d1.Rows[i][0].ToString() == A.ToString())//确认运行步骤
                        {

                            nowstep = d1.Rows[i][0].ToString();
                            bool waitsignal = false;
                            while (!waitsignal && !Bg_Main.CancellationPending)
                            {
                                Thread.Sleep(10);
                                OperateResult<Boolean> m_Result = MC_PLC.ReadBool(d1.Rows[i][1].ToString());

                                if (m_Result.IsSuccess)
                                {
                                    if (m_Result.Content)
                                    {
                                        waitsignal = true;
                                        MC_PLC.Write(d1.Rows[i][1].ToString(), new bool[] { false });//复位拍照限信号
                                        if (!Convert.ToBoolean(d1.Rows[i][8]))//是否后台处理
                                        {
                                            run(i);

                                        }
                                        else
                                        {
                                            // 创建并启动一个后台任务
                                            Thread thread1 = new Thread(run);
                                            thread1.IsBackground = true;
                                            thread1.Start(i);
                                        }




                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (robotenabled)
                                            {
                                                if (robot_connected && robotcamera_connected)
                                                {
                                                    btn_system_state.BeginInvoke(new Action(() =>
                                                    {
                                                        if (nowstep != null)
                                                        {
                                                            btn_system_state.Text = "等待拍照位" + nowstep + "信号";
                                                            btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Chartreuse;
                                                        }

                                                    }
                                           )
                                               );
                                                }
                                            }
                                            else
                                            {
                                                btn_system_state.BeginInvoke(new Action(() =>
                                                {
                                                    if (nowstep != null)
                                                    {
                                                        btn_system_state.Text = "等待拍照位" + nowstep + "信号";
                                                        btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Chartreuse;
                                                    }

                                                }
                                           )
                                               );
                                            }
                                           

                                        }
                                        catch (Exception)
                                        {

                                            // throw;
                                        }

                                    }



                                }

                            }

                        }




                        if (A < d1.Rows.Count)
                        {
                            A++;
                        }
                        else
                        {
                            A = 1;
                        }



                    }


                }



                catch (Exception ex)
                {
                    m_Logprint(HslMessageDegree.FATAL, ex.ToString(), false);
                    MessageBox.Show(ex.ToString());

                }
                finally
                {
                    rS232.Close();
                }
            }






        }


        private void run(object threadName)
        {
            int i = (int)threadName; // 转换传递的参数
            #region 相机拍照
            //判断拍照相机并拍照
            if (Convert.ToInt32(d1.Rows[i][2]) == 1)  //相机1
            {
                if (m_Camera1.State == 0)
                {
                    m_Camera1.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机1", "IP", ""));
                }
                if (m_Camera1.State == 1)
                {
                    m_Camera1.OpenCamera();
                }
                if (m_Camera1.Start())  //开始采集流
                {
                    m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照中", true);
                    brightness = Convert.ToInt32(d1.Rows[i][4]);
                    rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");
                    Thread.Sleep(100);
                    //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                    if (m_Camera1.Get_Oneframe())
                    {

                        Thread.Sleep(100);
                        _mWindow1.NowImage = m_Camera1.Himage;
                        HOperatorSet.CopyImage(m_Camera1.Himage, out Img);

                        Console.WriteLine("Get_onefrmae is OK ");
                        OperateResult result3 = MC_PLC.Write(d1.Rows[i][3].ToString(), new bool[] { true });  // 1工位结果写入
                        if (result3.IsSuccess)
                        {
                            m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照完成", true);

                        }
                        m_Camera1.Stop();
                        m_Camera1.close();


                    }
                    brightness = 0;
                    rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");

                }

            }
            else if (Convert.ToInt32(d1.Rows[i][2]) == 2)
            {
                if (m_Camera2.State == 0)
                {
                    m_Camera2.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机2", "IP", ""));
                }
                if (m_Camera2.State == 1)
                {
                    m_Camera2.OpenCamera();
                }
                if (m_Camera2.Start())  //开始采集流
                {
                    m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照中", true);
                    brightness = Convert.ToInt32(d1.Rows[i][4]);
                    rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");
                    Thread.Sleep(100);
                    //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                    if (m_Camera2.Get_Oneframe())
                    {

                        Thread.Sleep(100);
                        _mWindow2.NowImage = m_Camera2.Himage;
                        HOperatorSet.CopyImage(m_Camera2.Himage, out Img);
                        Console.WriteLine("Get_onefrmae is OK ");
                        OperateResult result3 = MC_PLC.Write(d1.Rows[i][3].ToString(), new bool[] { true });  // 1工位结果写入
                        if (result3.IsSuccess)
                        {
                            m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][1].ToString() + "拍照完成", true);

                        }
                        m_Camera2.Stop();
                        m_Camera2.close();


                    }
                    brightness = 0;
                    rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");
                }
            }

            else if (Convert.ToInt32(d1.Rows[i][2]) == 3)
            {
                if (m_Camera3.State == 0)
                {
                    m_Camera3.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机3", "IP", ""));
                }
                if (m_Camera3.State == 1)
                {
                    m_Camera3.OpenCamera();
                }
                if (m_Camera3.Start())  //开始采集流
                {
                    m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照中", true);
                    brightness = Convert.ToInt32(d1.Rows[i][4]);
                    rS232.SerialPort.WriteLine("SC0" + brightness.ToString("D3") + "#" + "\r");
                    Thread.Sleep(100);
                    //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                    if (m_Camera3.Get_Oneframe())
                    {

                        Thread.Sleep(100);
                        _mWindow3.NowImage = m_Camera3.Himage;
                        HOperatorSet.CopyImage(m_Camera3.Himage, out Img);

                        Console.WriteLine("Get_onefrmae is OK ");
                        OperateResult result3 = MC_PLC.Write(d1.Rows[i][3].ToString(), new bool[] { true });  // 1工位结果写入
                        if (result3.IsSuccess)
                        {
                            m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照完成", true);

                        }
                        m_Camera3.Stop();
                        m_Camera3.close();


                    }
                    brightness = 0;
                    rS232.SerialPort.WriteLine("SC0" + brightness.ToString("D3") + "#" + "\r");
                }
            }

            else if (Convert.ToInt32(d1.Rows[i][2]) == 4)
            {
                if (m_Camera4.State == 0)
                {
                    m_Camera4.Connect_Cam(IniAPI.INIGetStringValue(Param_Path, "相机4", "IP", ""));
                }
                if (m_Camera4.State == 1)
                {
                    m_Camera4.OpenCamera();
                }
                if (m_Camera4.Start())  //开始采集流
                {
                    m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照中", true);
                    brightness = Convert.ToInt32(d1.Rows[i][4]);
                    rS232.SerialPort.WriteLine("SD0" + brightness.ToString("D3") + "#" + "\r");
                    Thread.Sleep(100);
                    //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                    if (m_Camera4.Get_Oneframe())
                    {

                        Thread.Sleep(100);
                        _mWindow4.NowImage = m_Camera4.Himage;
                        HOperatorSet.CopyImage(m_Camera4.Himage, out Img);

                        Console.WriteLine("Get_onefrmae is OK ");
                        OperateResult result3 = MC_PLC.Write(d1.Rows[i][3].ToString(), new bool[] { true });  // 1工位结果写入
                        if (result3.IsSuccess)
                        {
                            m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "拍照完成", true);

                        }
                        m_Camera4.Stop();
                        m_Camera4.close();


                    }
                    brightness = 0;
                    rS232.SerialPort.WriteLine("SD0" + brightness.ToString("D3") + "#" + "\r");
                }
            }
            else
            {

            }





            #endregion


            #region 处理图像
            if (Convert.ToInt32(d1.Rows[i][2]) == 1)  //相机1
            {
                bool handleresult = runhandleimage(_mWindow1.hWindowControl.HalconWindow, Img, Convert.ToInt32((d1.Rows[i][5])));

                if (Convert.ToInt32(d1.Rows[i][7]) == 1)
                {
                    imageresult1.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 2)
                {
                    imageresult2.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 3)
                {
                    imageresult3.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 4)
                {
                    imageresult4.Add(handleresult);
                }
                m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "图像处理完成", true);
            }
            else if (Convert.ToInt32(d1.Rows[i][2]) == 2)
            {
                bool handleresult = runhandleimage(_mWindow2.hWindowControl.HalconWindow, Img, Convert.ToInt32((d1.Rows[i][5])));

                if (Convert.ToInt32(d1.Rows[i][7]) == 1)
                {
                    imageresult1.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 2)
                {
                    imageresult2.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 3)
                {
                    imageresult3.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 4)
                {
                    imageresult4.Add(handleresult);
                }
                m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "图像处理完成", true);
            }
            else if (Convert.ToInt32(d1.Rows[i][2]) == 3)
            {
                bool handleresult = runhandleimage(_mWindow3.hWindowControl.HalconWindow, Img, Convert.ToInt32((d1.Rows[i][5])));

                if (Convert.ToInt32(d1.Rows[i][7]) == 1)
                {
                    imageresult1.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 2)
                {
                    imageresult2.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 3)
                {
                    imageresult3.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 4)
                {
                    imageresult4.Add(handleresult);
                }
                m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "图像处理完成", true);
            }
            else if (Convert.ToInt32(d1.Rows[i][2]) == 4)
            {
                bool handleresult = runhandleimage(_mWindow4.hWindowControl.HalconWindow, Img, Convert.ToInt32((d1.Rows[i][5])));

                if (Convert.ToInt32(d1.Rows[i][7]) == 1)
                {
                    imageresult1.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 2)
                {
                    imageresult2.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 3)
                {
                    imageresult3.Add(handleresult);
                }
                else if (Convert.ToInt32(d1.Rows[i][7]) == 4)
                {
                    imageresult4.Add(handleresult);
                }
                m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "图像处理完成", true);
            }

            #endregion


            #region 发送结果

            if (Convert.ToInt32(d1.Rows[i][0]) == d1.Rows.Count)
            {
                int statenum = 1;
                for (int X = 0; X < d2.Rows.Count; X++)
                {
                    if (d2.Rows[X][0].ToString() == statenum.ToString())//确认运行步骤
                    {
                        Thread.Sleep(100);
                        if (statenum == 1)
                        {
                            m_Logprint(HslMessageDegree.INFO, "工位1发送结果中", true);
                            OperateResult Writeresult1;
                            if (!imageresult1.Contains(false))
                            {
                                Writeresult1 = MC_PLC.Write(d2.Rows[X][1].ToString(), new bool[] { true });  // 1工位写入OK 结果
                                m_Logprint(HslMessageDegree.INFO, "工位1发送结果OK", true);
                            }
                            else
                            {
                                Writeresult1 = MC_PLC.Write(d2.Rows[X][2].ToString(), new bool[] { true });  // 1工位写入NG结果
                                m_Logprint(HslMessageDegree.INFO, "工位1发送结果NG", true);
                            }

                            if (Writeresult1.IsSuccess)
                            {
                                m_Logprint(HslMessageDegree.INFO, "工位1发送结果完成", true);
                            }
                        }
                        if (statenum == 2)
                        {
                            m_Logprint(HslMessageDegree.INFO, "工位2发送结果中", true);
                            OperateResult Writeresult2;
                            if (!imageresult2.Contains(false))
                            {
                                Writeresult2 = MC_PLC.Write(d2.Rows[X][1].ToString(), new bool[] { true });  // 2工位写入OK 结果
                                m_Logprint(HslMessageDegree.INFO, "工位2发送结果OK", true);
                            }
                            else
                            {
                                Writeresult2 = MC_PLC.Write(d2.Rows[X][2].ToString(), new bool[] { true });  // 2工位写入NG结果
                                m_Logprint(HslMessageDegree.INFO, "工位2发送结果NG", true);
                            }

                            if (Writeresult2.IsSuccess)
                            {
                                m_Logprint(HslMessageDegree.INFO, "工位2发送结果完成", true);
                            }
                        }
                        if (statenum == 3)
                        {
                            m_Logprint(HslMessageDegree.INFO, "工位3发送结果中", true);
                            OperateResult Writeresult3;
                            if (!imageresult3.Contains(false))
                            {
                                Writeresult3 = MC_PLC.Write(d2.Rows[X][1].ToString(), new bool[] { true });  // 3工位写入OK 结果
                            }
                            else
                            {
                                Writeresult3 = MC_PLC.Write(d2.Rows[X][2].ToString(), new bool[] { false });  // 3工位写入NG结果
                            }

                            if (Writeresult3.IsSuccess)
                            {
                                m_Logprint(HslMessageDegree.INFO, "工位3发送结果完成", true);
                            }
                        }
                        if (statenum == 4)
                        {
                            m_Logprint(HslMessageDegree.INFO, "工位4发送结果中", true);
                            OperateResult Writeresult4;
                            if (!imageresult4.Contains(false))
                            {
                                Writeresult4 = MC_PLC.Write(d2.Rows[X][1].ToString(), new bool[] { true });  // 4工位写入OK 结果
                            }
                            else
                            {
                                Writeresult4 = MC_PLC.Write(d2.Rows[X][2].ToString(), new bool[] { false });  // 4工位写入NG结果
                            }

                            if (Writeresult4.IsSuccess)
                            {
                                m_Logprint(HslMessageDegree.INFO, "工位4发送结果完成", true);
                            }
                        }
                        statenum++;
                    }
                }
            }

            #endregion


            #region 保存图像
            if (Convert.ToBoolean(d1.Rows[i][6]))
            {
                string m_Year = DateTime.Now.ToString("yyyy");
                string m_Month = DateTime.Now.ToString("MM");
                string m_Day = DateTime.Now.ToString("dd");
                string imageDC = saveimagepath + "\\" + m_Year + "\\" + m_Month + "\\" + m_Day;


                imageDC = imageDC + "\\" + "PictureStation" + d1.Rows[i][0].ToString();

                CreateFiles(imageDC);




                string imagepath = imageDC + "\\" + DateTime.Now.ToString("HH_mm_ss_ffff");



                m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "存图中", true);



                if (Saveimage(Img, imagepath))
                {
                    m_Logprint(HslMessageDegree.INFO, "拍照位" + d1.Rows[i][0].ToString() + "存图完成", true);
                }
                else
                {
                    m_Logprint(HslMessageDegree.ERROR, "拍照位" + d1.Rows[i][0].ToString() + "存图失败", true);
                }
            }
            else
            {


            }
            #endregion
        }


        private bool Saveimage(HObject image, string name)
        {
            try
            {
                if (saveImageFormat == "BMP")
                {
                    HOperatorSet.WriteImage(image, "bmp", 0, name + ".bmp");
                }
                else
                {
                    HOperatorSet.WriteImage(image, "jpeg", 0, name + ".jpg");
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private void 英语ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SystemLanguage = 1;
            IniAPI.INIWriteValue(Param_Path, "System", "Language", SystemLanguage.ToString());
            MessageBox.Show(changelanguage("changeLanguageinfo"));
        }

        private void logNetAnalysisControl1_Load(object sender, EventArgs e)
        {

        }

        private void 简体中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemLanguage = 0;
            IniAPI.INIWriteValue(Param_Path, "System", "Language", SystemLanguage.ToString());
            MessageBox.Show(changelanguage("changeLanguageinfo"));
        }

        private void 相机设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Camera_Form camera1 = new Camera_Form(Load_Camera_Param("相机1"));
            camera1.saveCallback = Savecameranini;
            camera1.ShowDialog();
        }

        private void 模型设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private CameraParam Load_Camera_Param(String name)
        {
            CameraParam param = new CameraParam();
            param.Ip = IniAPI.INIGetStringValue(Param_Path, name, nameof(param.Ip), param.Ip);
            param.TriggerMode = IniAPI.INIGetStringValue(Param_Path, name, nameof(param.TriggerMode), param.TriggerMode);
            param.Exposure = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, name, nameof(param.Exposure), "0"));
            param.Name = name;
            return param;
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void pLC设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PLC_TOOL pLC_TOOL = new PLC_TOOL();
            pLC_TOOL.ShowDialog();
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!Bg_Main.IsBusy)
            {
               
                if (robotenabled && !bg_robot.IsBusy)
                {
                    bg_robot.RunWorkerAsync();
                }
                   
                button12.Image = global::_6524.Properties.Resources.停止;
                if (heartbeat_enabled&&!Bg_PLC_heartbeat.IsBusy)
                {
                    Bg_PLC_heartbeat.RunWorkerAsync();
                }
                Bg_Main.RunWorkerAsync();

            }
            else
            {
                Bg_Main.CancelAsync();
                if (robotenabled)
                {

                    m_Robot.close();
                    m_Camera.close();
                    m_Camera.dispose();
                    bg_robot.CancelAsync();
                }
                if (heartbeat_enabled)
                {
                    Bg_PLC_heartbeat.CancelAsync();
                }
                button12.Image = global::_6524.Properties.Resources.开始__1_;
            }
        }

        private void m_Logprint(HslMessageDegree leave, string Msg, bool sys_state)
        {
            if (leave == HslMessageDegree.INFO)
            {
                logNet.SetMessageDegree(HslMessageDegree.INFO);//除DEBUG外，都存储
                if (SystemLanguage == 0)
                {
                    logNet.WriteInfo((Msg));
                }
                else
                {
                    logNet.WriteInfo((Msg));
                    logNet.WriteInfo(changelanguage(Msg));
                }

                if (sys_state)
                {
                    btn_system_state.BeginInvoke(new Action(() => { btn_system_state.Text = changelanguage(Msg); btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Chartreuse; }));
                    //btn_system_state.Text = changelanguage(Msg);
                }
            }
            else if (leave == HslMessageDegree.WARN)
            {
                logNet.SetMessageDegree(HslMessageDegree.WARN);//除DEBUG外，都存储
                if (SystemLanguage == 0)
                {
                    logNet.WriteWarn((Msg));
                }
                else
                {
                    logNet.WriteWarn((Msg));
                    logNet.WriteWarn(changelanguage(Msg));
                }

                if (sys_state) { btn_system_state.BeginInvoke(new Action(() => { btn_system_state.Text = changelanguage(Msg); btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Yellow; })); }
            }
            else if (leave == HslMessageDegree.ERROR)
            {
                logNet.SetMessageDegree(HslMessageDegree.ERROR);//除DEBUG外，都存储
                if (SystemLanguage == 0)
                {
                    logNet.WriteError((Msg));
                }
                else
                {
                    logNet.WriteError((Msg));
                    logNet.WriteError(changelanguage(Msg));
                }
                logNet.WriteError(changelanguage(Msg));
                if (sys_state) { btn_system_state.BeginInvoke(new Action(() => { btn_system_state.Text = changelanguage(Msg); btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Red; })); }
            }
            else if (leave == HslMessageDegree.FATAL)
            {
                logNet.SetMessageDegree(HslMessageDegree.FATAL);//除DEBUG外，都存储
                if (SystemLanguage == 0)
                {
                    logNet.WriteFatal((Msg));
                }
                else
                {
                    logNet.WriteFatal((Msg));
                    logNet.WriteFatal(changelanguage(Msg));
                }

                if (sys_state) { btn_system_state.BeginInvoke(new Action(() => { btn_system_state.Text = changelanguage(Msg); btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed; })); }
            }
            else
            {
            }
        }

        private string changelanguage(string Msg)
        {
            string A = resourceManager.GetString(Msg);
            if (A == null)
            {
                return Msg;
            }
            else
            {
                return A;
            }
        }
        private void 西班牙语ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemLanguage = 2;
            IniAPI.INIWriteValue(Param_Path, "System", "Language", SystemLanguage.ToString());
            MessageBox.Show(changelanguage("changeLanguageinfo"));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_Camera1.dispose();
                m_Camera2.dispose();
                m_Camera3.dispose();
                m_Camera4.dispose();
                brightness = 0;
                rS232.SerialPort.WriteLine("SA0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SC0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.SerialPort.WriteLine("SD0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                rS232.Close();
            }
            catch (Exception)
            {

                // throw;
            }

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

        private void DeleteImage()
        {
            try
            {
                List<string> Files = new List<string>();
                Files = new List<string>(Directory.EnumerateFiles(this.saveimagepath, "*.bmp", SearchOption.AllDirectories));
                for (int j = 0; j < Files.Count; j++)
                {
                    DateTime dt3 = DateTime.Now.AddDays(-DeleteImagedays);
                    DirectoryInfo di3 = new DirectoryInfo(Files[j]);
                    DateTime dt4 = di3.CreationTime;
                    int t2 = dt3.CompareTo(dt4);
                    bool flag2 = t2 > 0;
                    if (flag2)
                    {
                        File.Delete(di3.FullName);
                    }
                }
                Files = new List<string>(Directory.EnumerateFiles(this.saveimagepath, "*.jpg", SearchOption.AllDirectories));
                for (int j = 0; j < Files.Count; j++)
                {
                    DateTime dt3 = DateTime.Now.AddDays(-DeleteImagedays);
                    DirectoryInfo di3 = new DirectoryInfo(Files[j]);
                    DateTime dt4 = di3.CreationTime;
                    int t2 = dt3.CompareTo(dt4);
                    bool flag2 = t2 > 0;
                    if (flag2)
                    {
                        File.Delete(di3.FullName);
                    }
                }
                Form1.KillEmptyDirectory(this.saveimagepath);

                m_Logprint(HslMessageDegree.INFO, "定期删图完成", false);
            }
            catch (Exception)
            {
                m_Logprint(HslMessageDegree.FATAL, "定期删图找不到指定路径", false);
            }
        }

        public static void KillEmptyDirectory(string storagepath)
        {
            DirectoryInfo dir = new DirectoryInfo(storagepath);
            DirectoryInfo[] subdirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo subdir in subdirs)
            {
                FileSystemInfo[] subFiles = subdir.GetFileSystemInfos();
                bool flag = subFiles.Count<FileSystemInfo>() == 0;
                if (flag)
                {
                    subdir.Delete();
                }
            }
        }

        private void 通讯设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveimageForm saveimageForm = new SaveimageForm();
            saveimageForm.ShowDialog();

        }

        private void 开发者登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login m_Login = new Login();
            m_Login.ShowDialog();

            if (m_Login.State == 2)
            {
                pLC设置ToolStripMenuItem.Enabled = true;
                相机设置ToolStripMenuItem.Enabled = true;
                模型设置ToolStripMenuItem.Enabled = true;
                通讯设置ToolStripMenuItem.Enabled = true;
                开发者选项ToolStripMenuItem.Enabled = true;
                机械手示教ToolStripMenuItem.Enabled = true;
                机械手控制ToolStripMenuItem.Enabled = true;

            }
            else if (m_Login.State == 1)
            {
                pLC设置ToolStripMenuItem.Enabled = true;
                相机设置ToolStripMenuItem.Enabled = true;
                模型设置ToolStripMenuItem.Enabled = true;
                通讯设置ToolStripMenuItem.Enabled = true;
                // 开发者选项ToolStripMenuItem.Enabled = true;
            }
            else
            {

            }
            m_Login.Close();
        }

        private void 操作员登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 开发者选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            m_modelSet.ShowDialog();

        }

        public bool runhandleimage(HTuple HWindowshandle, HObject img, int A)
        {
            try
            {
                if (A < 99)
                {
                    try
                    {


                        double Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Mult", "75"));
                        double Add = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Add", "75"));
                        double MinThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "MinThreshold", "75"));
                        double MaxThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "MaxThreshold", "75"));
                        int holeCount = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Usecount", "1"));


                        HObject Rectangle;
                        HObject Circle;
                        List<LastResult> LastResult = new List<LastResult>();
                        LastResult lastResult;
                        for (int i = 0; i < holeCount; i++)
                        {
                            lastResult = new LastResult();
                         
                            int Dis_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "eccentricity" + (i).ToString(), "75"));
                            int lim_Row = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "row" + (i).ToString(), "75"));
                            int lim_Column = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "col" + (i).ToString(), "75"));
                            int Min_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Ridusmin" + (i).ToString(), "75"));
                            int Max_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Ridusmax" + (i).ToString(), "75"));
                            double RectangleStartX = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleStartX" + (i).ToString(), "75"));
                            double RectangleStartY = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleStartY" + (i).ToString(), "75"));
                            double RectangleEndX = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleEndX" + (i).ToString(), "75"));
                            double RectangleEndY = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleEndY" + (i).ToString(), "75"));




                            Circle = new HObject();
                            HTuple row = new HTuple();
                            HTuple cloumn = new HTuple();
                            HTuple Radius = new HTuple();
                            HTuple Distance = new HTuple();
                            Rectangle = new HObject();

                            ImageHandle1 m_imageHandle0 = new ImageHandle1();


                            //偏心距限制
                            m_imageHandle0.Dis_R = Dis_R;
                            //初始圆心X
                            m_imageHandle0.lim_Row = lim_Row;
                            //初始圆心Y
                            m_imageHandle0.lim_Column = lim_Column;
                            //结果最大半径
                            m_imageHandle0.Min_R = Min_R;
                            //结果最小半径
                            m_imageHandle0.Max_R = Max_R;

                            m_imageHandle0.Mult = Mult;
                            m_imageHandle0.Add = Add;
                            m_imageHandle0.MinThreshold = MinThreshold;
                            m_imageHandle0.MaxThreshod = MaxThreshold;

                            m_imageHandle0.RectangestartX = RectangleStartX;
                            m_imageHandle0.RectangestartY = RectangleStartY;
                            m_imageHandle0.RectangeendX = RectangleEndX;
                            m_imageHandle0.RectangeendY = RectangleEndY;






                            bool result = m_imageHandle0.Judgment6524(img, out Rectangle, out Circle, out row, out cloumn, out Radius, out Distance);
                            lastResult.Result = result;
                            lastResult.Row = row;
                            lastResult.Col = cloumn;
                            lastResult.Radius = Radius;
                            lastResult.Circle = Circle;
                            lastResult.Retange = Rectangle;
                            lastResult.Distance = Distance;
                            LastResult.Add(lastResult);



                        }
                        bool allresult = true;
                        for (int i = 0; i < LastResult.Count; i++)
                        {
                            if (LastResult[i].Row.Length != 0)
                            {
                                disp_message(HWindowshandle, "X:" + LastResult[i].Row.D.ToString(), "window", 12, (i * 150) + 12, "black", "true");
                                disp_message(HWindowshandle, "Y:" + LastResult[i].Col.D.ToString(), "window", 32, (i * 150) + 12, "black",
                                    "true");
                                disp_message(HWindowshandle, "半径：" + LastResult[i].Radius.D.ToString(), "window",
                                    52, (i * 150) + 12, "black", "true");
                                disp_message(HWindowshandle, "偏心距：" + LastResult[i].Distance.D.ToString("F0"), "window",
                             72, (i * 150) + 12, "black", "true");

                                HOperatorSet.SetDraw(HWindowshandle, "margin");
                                HOperatorSet.SetColor(HWindowshandle, "blue");
                                HOperatorSet.DispObj(LastResult[i].Retange, HWindowshandle);
                                if (LastResult[i].Result)
                                {
                                    HOperatorSet.SetColor(HWindowshandle, "green");
                                }
                                else
                                {
                                    HOperatorSet.SetColor(HWindowshandle, "red");
                                    disp_message(HWindowshandle, "NG", "window", 92, (i * 150) + 12, "black", "true");
                                    allresult = false;

                                }
                                HOperatorSet.DispObj(LastResult[i].Circle, HWindowshandle);

                            }
                            else
                            {
                                allresult = false;
                                HOperatorSet.SetDraw(HWindowshandle, "margin");
                                HOperatorSet.SetColor(HWindowshandle, "red");
                                HOperatorSet.DispObj(LastResult[i].Retange, HWindowshandle);
                                disp_message(HWindowshandle, "NG", "window", 12, (i * 150) + 12, "black", "true");
                            }

                        }



                        return allresult;




                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
                else if (A < 199)
                {
                    try
                    {
                        bool resultall = false;
                        double Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Mult", "75"));
                        double Add = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Add", "75"));
                        double MinThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "MinThreshold", "75"));
                        double MaxThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "MaxThreshold", "75"));
                        double RectangleStartX = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleStartX", "0"));


                        double RectangleStartY = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "RectangleStartY", "0"));
                        double Length1 = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Length1", "0"));
                        double Length2 = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Length2", "0"));
                        double PI = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "PI", "0"));
                        string Recognition = IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Recognition", "0");
                        string type = IniAPI.INIGetStringValue(Param_Path, "Run_number" + A.ToString(), "Type", "0");

                        if (Recognition == "二维码")
                        {
                            HObject rectange = new HObject();
                            HObject XLDS = new HObject();
                            QR_Code__recognition qR_Code__Recognition = new QR_Code__recognition();
                            HTuple result;
                            qR_Code__Recognition.Find2Code(img, out rectange, out XLDS, RectangleStartX, RectangleStartY, PI, Length1, Length2, type, out result);
                            if (result != null)
                            {
                                disp_message(HWindowshandle, "二维码内容:" + result.ToString(), "window", 12, 12, "black", "true");


                                HOperatorSet.SetDraw(HWindowshandle, "margin");
                                HOperatorSet.SetColor(HWindowshandle, "blue");
                                HOperatorSet.DispObj(rectange, HWindowshandle);
                                HOperatorSet.SetColor(HWindowshandle, "green");
                                HOperatorSet.DispObj(XLDS, HWindowshandle);
                                resultall = true;
                            }
                            else
                            {
                                resultall = false;
                            }
                        }
                        else if (Recognition == "条形码")
                        {
                            find_bar find_Bar = new find_bar();
                            HTuple result;
                            HObject rectange = new HObject();
                            HObject rectangexld = new HObject();
                            find_Bar.find_barcode(img, out rectange, out rectangexld, RectangleStartX, RectangleStartY, PI, Length1, Length2, Mult, Add, type, out result);
                            if (result != null)
                            {
                                disp_message(HWindowshandle, "条行码内容:" + result.ToString(), "window", 12, 12, "black", "true");


                                HOperatorSet.SetDraw(HWindowshandle, "margin");
                                HOperatorSet.SetColor(HWindowshandle, "blue");

                                HOperatorSet.DispObj(rectange, HWindowshandle);
                                HOperatorSet.SetColor(HWindowshandle, "green");
                                HOperatorSet.DispObj(rectangexld, HWindowshandle);
                                resultall = true;
                            }
                            else
                            {
                                resultall = false;
                            }
                        }
                        else if (Recognition == "OCR")
                        {
                            HTuple result;
                            HObject rectange = new HObject();
                            HObject rectangexld = new HObject();
                            Find_OCR_Class find_OCR_Class = new Find_OCR_Class();
                            find_OCR_Class.Find_OCR(img, out rectange, out rectangexld, Mult, Add, RectangleStartX, RectangleStartY, PI, Length1, Length2, type, out result);
                            if (result != null)
                            {
                                disp_message(HWindowshandle, "OCR内容:" + result.ToString(), "window", 12, 12, "black", "true");


                                HOperatorSet.SetDraw(HWindowshandle, "margin");
                                HOperatorSet.SetColor(HWindowshandle, "blue");

                                HOperatorSet.DispObj(rectange, HWindowshandle);
                                HOperatorSet.SetColor(HWindowshandle, "green");
                                HOperatorSet.DispObj(rectangexld, HWindowshandle);
                                resultall = true;
                            }
                            else
                            {
                                resultall = false;
                            }

                        }
                        else
                        {
                            // MessageBox.Show("请选择正确的识别内容类型");
                            resultall = false;
                        }

                        return resultall;
                    }

                    catch (Exception)
                    {

                        return false;
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
        }
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_GenParamName = new HTuple(), hv_GenParamValue = new HTuple();
            HTuple hv_Color_COPY_INP_TMP = new HTuple(hv_Color);
            HTuple hv_Column_COPY_INP_TMP = new HTuple(hv_Column);
            HTuple hv_CoordSystem_COPY_INP_TMP = new HTuple(hv_CoordSystem);
            HTuple hv_Row_COPY_INP_TMP = new HTuple(hv_Row);

            // Initialize local and output iconic variables 
            try
            {
                //This procedure displays text in a graphics window.
                //
                //Input parameters:
                //WindowHandle: The WindowHandle of the graphics window, where
                //   the message should be displayed.
                //String: A tuple of strings containing the text messages to be displayed.
                //CoordSystem: If set to 'window', the text position is given
                //   with respect to the window coordinate system.
                //   If set to 'image', image coordinates are used.
                //   (This may be useful in zoomed images.)
                //Row: The row coordinate of the desired text position.
                //   You can pass a single value or a tuple of values.
                //   See the explanation below.
                //   Default: 12.
                //Column: The column coordinate of the desired text position.
                //   You can pass a single value or a tuple of values.
                //   See the explanation below.
                //   Default: 12.
                //Color: defines the color of the text as string.
                //   If set to [] or '' the currently set color is used.
                //   If a tuple of strings is passed, the colors are used cyclically
                //   for every text position defined by Row and Column,
                //   or every new text line in case of |Row| == |Column| == 1.
                //Box: A tuple controlling a possible box surrounding the text.
                //   Its entries:
                //   - Box[0]: Controls the box and its color. Possible values:
                //     -- 'true' (Default): An orange box is displayed.
                //     -- 'false': No box is displayed.
                //     -- color string: A box is displayed in the given color, e.g., 'white', '#FF00CC'.
                //   - Box[1] (Optional): Controls the shadow of the box. Possible values:
                //     -- 'true' (Default): A shadow is displayed in
                //               darker orange if Box[0] is not a color and in 'white' otherwise.
                //     -- 'false': No shadow is displayed.
                //     -- color string: A shadow is displayed in the given color, e.g., 'white', '#FF00CC'.
                //
                //It is possible to display multiple text strings in a single call.
                //In this case, some restrictions apply on the
                //parameters String, Row, and Column:
                //They can only have either 1 entry or n entries.
                //Behavior in the different cases:
                //   - Multiple text positions are specified, i.e.,
                //       - |Row| == n, |Column| == n
                //       - |Row| == n, |Column| == 1
                //       - |Row| == 1, |Column| == n
                //     In this case we distinguish:
                //       - |String| == n: Each element of String is displayed
                //                        at the corresponding position.
                //       - |String| == 1: String is displayed n times
                //                        at the corresponding positions.
                //   - Exactly one text position is specified,
                //      i.e., |Row| == |Column| == 1:
                //      Each element of String is display in a new textline.
                //
                //
                //Convert the parameters for disp_text.
                if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                    new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
                {

                    hv_Color_COPY_INP_TMP.Dispose();
                    hv_Column_COPY_INP_TMP.Dispose();
                    hv_CoordSystem_COPY_INP_TMP.Dispose();
                    hv_Row_COPY_INP_TMP.Dispose();
                    hv_GenParamName.Dispose();
                    hv_GenParamValue.Dispose();

                    return;
                }
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP.Dispose();
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP.Dispose();
                    hv_Column_COPY_INP_TMP = 12;
                }
                //
                //Convert the parameter Box to generic parameters.
                hv_GenParamName.Dispose();
                hv_GenParamName = new HTuple();
                hv_GenParamValue.Dispose();
                hv_GenParamValue = new HTuple();
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                    {
                        //Display no box
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "box");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    "false");
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual(
                        "true"))) != 0)
                    {
                        //Set a color other than the default.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "box_color");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    hv_Box.TupleSelect(0));
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                }
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                    {
                        //Display no shadow.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "shadow");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    "false");
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual(
                        "true"))) != 0)
                    {
                        //Set a shadow color other than the default.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "shadow_color");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    hv_Box.TupleSelect(1));
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                }
                //Restore default CoordSystem behavior.
                if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
                {
                    hv_CoordSystem_COPY_INP_TMP.Dispose();
                    hv_CoordSystem_COPY_INP_TMP = "image";
                }
                //
                if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    //disp_text does not accept an empty string for Color.
                    hv_Color_COPY_INP_TMP.Dispose();
                    hv_Color_COPY_INP_TMP = new HTuple();
                }
                //
                HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
                    hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName,
                    hv_GenParamValue);

                hv_Color_COPY_INP_TMP.Dispose();
                hv_Column_COPY_INP_TMP.Dispose();
                hv_CoordSystem_COPY_INP_TMP.Dispose();
                hv_Row_COPY_INP_TMP.Dispose();
                hv_GenParamName.Dispose();
                hv_GenParamValue.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Color_COPY_INP_TMP.Dispose();
                hv_Column_COPY_INP_TMP.Dispose();
                hv_CoordSystem_COPY_INP_TMP.Dispose();
                hv_Row_COPY_INP_TMP.Dispose();
                hv_GenParamName.Dispose();
                hv_GenParamValue.Dispose();

                throw HDevExpDefaultException;
            }
        }






        enum State
        {
            TakePicture,
            Handle,
            SendResult,
            SaveImage
        }

        private void 机械手控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FANUC12 fanuc_UI = new FANUC12();
            fanuc_UI.ShowDialog();

        }

        private void 机械手示教ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Teach_point M_Teach = new Teach_point();
            M_Teach.ShowDialog();
        }

        private void bg_robot_DoWork(object sender, DoWorkEventArgs e)
        {
            HObject Img = new HObject();
            robotstate m_state = new robotstate();
            m_state = robotstate.init;
            m_Robot = new fanuctcpip();
            m_Camera = new MVS_SDK();
            string robotip = IniAPI.INIGetStringValue(Path_calibration_Param, "Robot", "IP", "0");
            string cameraip = IniAPI.INIGetStringValue(Path_calibration_Param, "robotCamera", "IP", "0");
            string PR1 = null;//补偿值
            double markX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "datum_mark1", "markX", "0"));
            double markY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "datum_mark1", "markY", "0"));
            double center_rotationX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "X", "0"));
            double center_rotationY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "Y", "0"));
  


            bool CameraNg = false;
            while (!bg_robot.CancellationPending)
            {

                try
                {
                    switch (m_state)
                    {
                        case robotstate.init:


                          //  isPaused = true;
                            btn_system_state.BeginInvoke(new Action(() =>
                            {

                                btn_system_state.Text = "机械手连接中";
                                btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;


                            }
                                     )
                                         );

                            if (!m_Robot.connectFanuc(robotip))
                            {
                                //  isPaused = false;

                                button12.Image = global::_6524.Properties.Resources.开始__1_;
                                m_Logprint(HslMessageDegree.ERROR, "机械手连接失败", true);
                                m_Robot.close();
                                // MessageBox.Show("robot Connected Fail");
                                bg_robot.CancelAsync();
                                Bg_Main.CancelAsync();
                                Thread.Sleep(50);
                                btn_system_state.BeginInvoke(new Action(() =>
                                    {

                                        btn_system_state.Text = "机械手连接失败";
                                        btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Red;


                                    }
                                    )
                                        );



                                break;
                            }
                            else
                            {
                                robot_connected = true;
                            }

                          //  isPaused = true;

                            if (m_Camera.Connect_Cam(cameraip))
                            {
                                robotcamera_connected=true;
                                m_state = robotstate.arrive;
                            }
                            else
                            {
                                button12.Image = global::_6524.Properties.Resources.开始__1_;
                                m_Logprint(HslMessageDegree.ERROR, "机械手相机连接失败", true);
                                m_Robot.close();
                                //MessageBox.Show("robot_Camera Connected Fail");
                                bg_robot.CancelAsync();
                                Bg_Main.CancelAsync();
                                Thread.Sleep(50);   
                                btn_system_state.BeginInvoke(new Action(() =>
                                {

                                    btn_system_state.Text = "机械手相机连接失败";
                                    btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Red;


                                }
                        )
                            );

                                break;
                            }
                            //}
                            //else
                            //{
                            //    break;
                            //}



                            #region  载入定位参数


                            #endregion

                            break;
                        case robotstate.arrive:
                            //等待到位
                            string value = null;
                            Thread.Sleep(100);
                            if (m_Robot.readR("5", ref value))
                            {

                                if (Convert.ToInt32(value) == 1)
                                {
                                    m_state = robotstate.take_image;
                                }
                                else
                                {
                                    Thread.Sleep(1000);

                                    m_Robot.Writelog("Wait Robot [R5]==1");

                                }
                            }

                            break;


                        case robotstate.take_image:
                            //拍照
                            brightness = 150;
                            rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                            Thread.Sleep(50);


                            #region 拍照
                            if (m_Camera.State == 0)
                            {
                                m_Camera.Connect_Cam(cameraip);
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
                                    _mWindow5.NowImage = Img;

                                    Console.WriteLine("Get_onefrmae is OK ");

                                    m_Camera.Stop();
                                    m_Camera.close();


                                }
                                else
                                {
                                    CameraNg = true;
                                    m_state = robotstate.send_vaule;
                                }

                            }

                            #endregion


                            brightness = 0;
                            rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 

                            m_state = robotstate.calculate_Compensation;
                            break;


                        case robotstate.calculate_Compensation:
                            //计算补偿值
                            double offx = 0;
                            double offy = 0;
                            double offr = 0;
                            HObject hObject = new HObject();
                            if (run_robot_camera(Img, ref offx, ref offy, ref offr, ref hObject))
                            {
                                CameraNg = false;
                                PR1 = offx.ToString("F2") + "," + offy.ToString("F2") + ",0,0,0," + offr.ToString("F2");//虚拟补偿值
                                disp_message(_mWindow5.hWindowControl.HalconWindow, "结果:" + "  OK", "window", 12, 12, "black", "true");
                                disp_message(_mWindow5.hWindowControl.HalconWindow, "X补偿:" + offx.ToString("F2"), "window", 72, 12, "black", "true");
                                disp_message(_mWindow5.hWindowControl.HalconWindow, "Y补偿:" + offy.ToString("F2"), "window", 132, 12, "black", "true");
                                HOperatorSet.SetColor(_mWindow5.hWindowControl.HalconWindow, "green");
                                HOperatorSet.DispObj(hObject, _mWindow5.hWindowControl.HalconWindow);
                                // disp_message(_mWindow5.hWindowControl.HalconWindow, "X补偿:" + offx.ToString("F2"), "window", 192, 12, "black", "true");
                                //    disp_message(_mWindow5.hWindowControl.HalconWindow, "Y补偿:" + offy.ToString("F2"), "window", 12, 222, "black", "true");


                            }
                            else
                            {
                                CameraNg = true;
                                disp_message(_mWindow5.hWindowControl.HalconWindow, "结果:" + "  NG", "window", 12, 12, "red", "true");

                                PR1 = "0,0,0,0,0,0,";
                            }

                            m_state = robotstate.send_vaule;
                            break;
                        case robotstate.send_vaule:

                            if (CameraNg)
                            {
                                m_Robot.WrieR("6", "2");
                            }
                            else
                            {
                                PR1 = "0,0,0,0,0,0,";
                                if (m_Robot.writePR("1", PR1))
                                {
                                    m_Robot.WrieR("6", "1");
                                }
                            }

                            m_state = robotstate.wait_ok;
                            break;
                        case robotstate.wait_ok:
                            string value2 = null;
                            Thread.Sleep(100);
                            if (m_Robot.readR("7", ref value2))
                            {

                                if (Convert.ToInt32(value2) == 1)
                                {
                                    m_Robot.WrieR("5", "0");
                                    m_Robot.WrieR("7", "0");
                                    //m_Robot.writePR("1", "1,1,0,0,0,0");

                                    m_state = robotstate.arrive;

                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                    m_Robot.Writelog("Wait Robot R[7]==1");


                                }
                            }

                            break;

                    }

                }
                catch (Exception)
                {

                    m_Robot.close();
                }
               //finally {  } 

            }

        }

        private bool run_robot_camera(HObject IMG, ref double offx, ref double offy, ref double offr, ref HObject ResObj)
        {
            Robotcalibration M_Calibration = new Robotcalibration();
            string Path_calibration1 = Application.StartupPath + @"\\calibration\calibration1.mat";
            double offY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offY", "0"));
            double offX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offX", "0"));
            double offR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offR", "0"));
            bool off_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "off_enabled", "false"));
            bool off_Angle_enabled = Convert.ToBoolean(IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "off_Angle_enabled", "false"));
            string strHost = "1";
            if (autochangemodel)
            {




                //自动切换机种，机种名必须为1.2.3
                int model = MC_PLC.ReadInt32("D100").Content;
                strHost = model.ToString();

            }
            else
            {
                //手动指定机种,机种名可以不以1.2.3.命名
                string str = IniAPI.INIGetStringValue(Param_Path, "ModelExcel", "UsingExcelPath", "");

                strHost = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");

            }

            try
            {
                Shape_matching M_Shape_matching = new Shape_matching();
                //模板匹配的参数
                M_Shape_matching.Hv_Matching_rote_min = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_rote_min", ""));
                M_Shape_matching.Hv_Matching_rote_max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_rote_max", ""));
                M_Shape_matching.Hv_Matching_scale_min = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_scale_min", ""));
                M_Shape_matching.Hv_Matching_scale_max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_scale_max", ""));
                M_Shape_matching.Hv_Matching_scale_step = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_scale_step", ""));
                M_Shape_matching.Hv_Matching_rote_step = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_rote_step", ""));
                M_Shape_matching.Hv_Matching_num = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_num", ""));
                M_Shape_matching.Hv_Matching_min_Score = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_min_Score", ""));
                M_Shape_matching.Hv_Matching_overlap_Max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_overlap_Max", ""));
                M_Shape_matching.Hv_Matching_Pyramid_level = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_Pyramid_level", ""));
                M_Shape_matching.Hv_Matching_Greedy_algorithm = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_Greedy_algorithm", ""));
                M_Shape_matching.Hv_Matching_min_Contrastratio = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Hv_Matching_min_Contrastratio", ""));
                //模板
                M_Shape_matching.LoadingModel(System.Windows.Forms.Application.StartupPath + @"\\calibration\Matching0" + strHost + ".shm");

                HTuple dis_X = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "X", "0"));

                HTuple dis_Y = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "Y", "0"));


                ////读取拍照初始位和铆压初始位
                double Pressure_RobotX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotX", "0"));
                double Pressure_RobotY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotY", "0"));
                double Pressure_RobotR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotR", "0"));
                double Picture_RobotX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotX", "0"));
                double Picture_RobotY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotY", "0"));
                double Picture_RobotR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotR", "0"));

                //计算角度偏移
                double Robot_R_off = Picture_RobotR - Pressure_RobotR;


                ////mark初始像素坐标
                HTuple PX0 = new HTuple();
                HTuple PY0 = new HTuple();
                HTuple PR0 = new HTuple();

                ////mark初始机械坐标
                HTuple RX0 = new HTuple();
                HTuple RY0 = new HTuple();

                //当前模板的X，Y,rote
                PX0 = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "ModelX", "0"));
                PY0 = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "ModelY", "0"));
                PR0 = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Model_Angle", "0"));


                M_Calibration.Affine_XY(Path_calibration1, PX0, PY0, out RX0, out RY0);



                M_Shape_matching.img = IMG;
                if (M_Shape_matching.action(0, 0))
                {
                    ////mark初始机械坐标
                    HTuple RX1 = new HTuple();
                    HTuple RY1 = new HTuple();
                    ResObj = M_Shape_matching.ho_Transregion_final;
                    M_Calibration.Affine_XY(Path_calibration1, M_Shape_matching.Row, M_Shape_matching.Column, out RX1, out RY1);

                    //旋转中心半径
                    double R = (Math.Sqrt(((RX0.D - dis_X.D) * (RX0.D - dis_X.D)) + ((RY0.D - dis_Y.D) * (RY0.D - dis_Y.D))));
                    //前后偏差角度
                    double dis_Angle;
                    if (!off_Angle_enabled)
                    {
                        if (off_enabled)
                        {
                            dis_Angle = ((M_Shape_matching.Angle / Math.PI) * 180) - ((PR0 / Math.PI) * 180);
                            dis_Angle = dis_Angle + offR;
                        }
                        else
                        {
                            dis_Angle = ((M_Shape_matching.Angle / Math.PI) * 180) - ((PR0 / Math.PI) * 180);
                        }

                    }
                    else
                    {
                        dis_Angle = 0;
                    }

                    //固定差距值
                    double disX1 = (RX0 - RX1);
                    double disY1 = (RY0 - RY1);
                    double lengrh_C = Math.Sqrt((disX1* disX1)+(disY1 * disY1));
                    double theta = Math.Atan(disX1 / disY1); // 计算 arctan(a / b) 的角度

                    // 将弧度转换为度
                    double degrees = theta * (180.0 / Math.PI);

                    // 最终角度差
                    double final_offR = degrees + Robot_R_off;




                    //旋转后角度偏差补偿
                    double dis_RX = (R * Math.Sin(dis_Angle * (Math.PI / 180)));
                    double dis_RY = 2 * (R * (Math.Sin((dis_Angle * (Math.PI / 180)) / 2)) * (Math.Sin((dis_Angle * (Math.PI / 180)) / 2)));

                    //输出最后的补偿
                    double final_disX;
                    double final_disY;
                    if (off_enabled)
                    {
                        final_disX = disX1 + dis_RX + offX;
                        final_disY = disY1 + dis_RY + offY;
                    }
                    else
                    {
                        final_disX = disX1 + dis_RX;
                        final_disY = disY1 + dis_RY;
                    }






                    double final_disR = dis_Angle;
                    if (final_disR > 180)
                    {
                        final_disR = final_disR - 360;
                    }

                    offx = lengrh_C * Math.Sin(final_offR);
                    offy = lengrh_C * Math.Cos(final_offR); 
                    offr = final_disR;
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

        private void Bg_PLC_heartbeat_DoWork(object sender, DoWorkEventArgs e)
        {

            if (PLC_connected)
            {
                Int32 sleeptime = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "PLC", "wait_time", "1000"));
                while (!Bg_PLC_heartbeat.CancellationPending)
                {
                    try
                    {
                        OperateResult result = MC_PLC.Write(heartbeat_path, new bool[] { true });//复位拍照限信号
                        Thread.Sleep(sleeptime);
                        if (!result.IsSuccess)
                        {
                            MessageBox.Show("PLC连接失败");
                        }

                    }
                    catch (Exception)
                    {

                        MessageBox.Show("PLC连接失败");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先连接PLC");
            }

        }

        private void 光源控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Light_Control light_Control = new Light_Control();
            light_Control.Show();
        }

        private void 圆孔检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageModelSet imageModelSet = new ImageModelSet();
            imageModelSet.Show();
        }

        private void 字符条码二维码识别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Barcode_recognition _Barcode_recognition = new Barcode_recognition();
            _Barcode_recognition.Show();
        }
    }


    enum robotstate
    {
        init,
        arrive,
        take_image,
        calculate_Compensation,
        send_vaule,
        wait_ok
    }
}


