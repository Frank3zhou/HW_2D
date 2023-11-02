using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading.Tasks;
using System.Threading;
using HslCommunication;
using MvCamCtrl.NET;

namespace _6524
{
    public delegate void SaveCallback(CameraParam Ca_Param);//回调相机运行至标定点,并且拍照，返回像素坐标信息
    public partial class Camera_Form : Form
    {
          public HWindowControl hWindow;
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
        MVS_SDK mvs_SDK=new MVS_SDK();
        public SaveCallback saveCallback = null;
        CameraParam cameraParam = new CameraParam();
        private CancellationTokenSource _cancellationTokenSource;
        ZKHwindows m_ZKHwindows = new ZKHwindows();
     // HWindowControl m_ZKHwindows=new HWindowControl();
        public Camera_Form(CameraParam cameraParam)
        {
            InitializeComponent();
           // groupBox1.Text = cameraParam.Name;
            textBox2.Text = cameraParam.Ip;
            textBox1.Text = cameraParam.Exposure.ToString();
            comboBox2.Text = cameraParam.TriggerMode;
        }

      

        private void Camera_Form_Load(object sender, EventArgs e)
        {
          //  groupBox1.Text=cameraParam.Name;    
            m_ZKHwindows.Dock = DockStyle.Fill; 
            panel1.Controls.Add(m_ZKHwindows);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mvs_SDK.Connect_Cam(textBox2.Text))
                {
                    //mvs_SDK.Set_callback(1);

                    button1.BackColor = Color.LightGreen;
                    button1.Text = "连接中";

                    tbExposure.Enabled = true;
                    tbGain.Enabled = true;
                    tbFrameRate.Enabled = true;
                    bnGetParam.Enabled = true;
                    bnSetParam.Enabled = true;
                }
                else
                {
                    button1.BackColor = Color.Red;
                    MessageBox.Show("连接失败");
                }
                    

               
            }
            catch (Exception)
            {

                throw;
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mvs_SDK.State == 0)
                {
                    mvs_SDK.Connect_Cam(textBox2.Text);
                }
                if (mvs_SDK.State == 1)
                {
                    mvs_SDK.OpenCamera();
                }
                if (mvs_SDK.Start())  //开始采集流
                {
                    //  HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.HalconWindow);
                    if (mvs_SDK.Get_Oneframe())
                    {
                        m_ZKHwindows.NowImage = mvs_SDK.Himage;
                        Console.WriteLine("Get_onefrmae is OK ");
                    }
                    else
                    {
                        Console.WriteLine("Get_onefrmae is fail ");
                    }
                }

               
               


                mvs_SDK.Stop();//开始停止采集流
                mvs_SDK.dispose();
            }
            catch (Exception)
            {
                throw;
                
            }

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                mvs_SDK.Set_callback(1);//0是bitmap,1是Hobject
                mvs_SDK.Set_Exposure(textBox1.Text);//设置曝光
                if (comboBox2.Text == "On")//设置触发模式，并且切换触发源
                {
                    mvs_SDK.Set_TriggerMode(1);
                }
                else
                {
                    mvs_SDK.Set_TriggerMode(0);
                }
                //mvs_SDK.Set_win(m_ZKHwindows.hWindowControl.HalconWindow);//绑定图像窗口
                cameraParam.Exposure =Convert.ToInt32(textBox1.Text);
                cameraParam.Ip= textBox2.Text;
                cameraParam.TriggerMode = comboBox2.SelectedItem.ToString();
                cameraParam.Name= comboBox1.SelectedItem.ToString();
                saveCallback(cameraParam);

                MessageBox.Show("参数设置成功");
            }
            catch (Exception)
            {
                MessageBox.Show("参数设置失败");

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cancellationTokenSource == null)
                {
                    if (mvs_SDK.State == 0)
                    {
                        mvs_SDK.Connect_Cam(textBox2.Text);
                    }
                    if (mvs_SDK.State == 1)
                    {
                        mvs_SDK.OpenCamera();
                    }
                    mvs_SDK.Set_callback(1);

                    mvs_SDK.Start();//开始采集流
            
                    button4.Text = "停止采集";
                    _cancellationTokenSource = new CancellationTokenSource();
                    // 启动新的后台线程
                    Task.Run(() => BackgroundThread(_cancellationTokenSource.Token));
                }
                else
                {
                    mvs_SDK.Stop();//开始停止采集流
                    mvs_SDK.dispose();
                  //  mvs_SDK.Stop 
                    // 取消后台线程
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = null;
                    button4.Text = "连续采集";
                }
           
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void BackgroundThread(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 在后台线程中执行耗时的操作
                    // ...
                    Thread.Sleep(30);

                    // m_ZKHwindows.NowImage = mvs_SDK.Himage;
                    if (mvs_SDK.signal)
                        HOperatorSet.DispObj(mvs_SDK.Himage, m_ZKHwindows.hWindowControl.HalconWindow);
                    mvs_SDK.signal = false;
                    // mvs_SDK.Get_Oneframe();
                    // 检查标志位，如果标志位为true，则退出循环
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                catch (Exception)
                {

                }
               
               
            }

            // 后台线程结束后，更新UI
           // UpdateUI("后台线程结束！");
        }

        private void UpdateUI(string message)
        {
            if (InvokeRequired)
            {
                // 在主线程中执行更新UI的操作
                Invoke(new Action<string>(UpdateUI), message);
            }
            else
            {
                // 显示处理结果
                
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Camera_Form_Load_1(object sender, EventArgs e)
        {

        }

        private void bnGetParam_Click(object sender, EventArgs e)
        {
            string ExposureTime = "0";
            string Gain = "0";
            string ResultingFrameRate = "0";
            mvs_SDK.getcamerainfo(ref    ExposureTime, ref  Gain, ref   ResultingFrameRate);
           
                tbExposure.Text=ExposureTime;
                tbGain.Text=Gain;
                tbFrameRate.Text=ResultingFrameRate;


        }

        private void bnSetParam_Click(object sender, EventArgs e)
        {
            if (mvs_SDK.SetCamera(tbExposure.Text, tbGain.Text, tbFrameRate.Text))
            {
                MessageBox.Show("设置成功！");
            }
            else
            {
                MessageBox.Show("设置失败！");
            }
        }

        private void Camera_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            mvs_SDK.dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            cameraParam = Load_Camera_Param(comboBox1.Text);
                   textBox2.Text = cameraParam.Ip;
            textBox1.Text = cameraParam.Exposure.ToString();
            comboBox2.Text = cameraParam.TriggerMode;
        }
    }



    public partial class CameraParam
    {
        string triggerMode = "On";
        string ip;
        int exposure;
        string name ;

        public string TriggerMode { get => triggerMode; set => triggerMode = value; }
        public string Ip { get => ip; set => ip = value; }
        public int Exposure { get => exposure; set => exposure = value; }
        public string Name { get => name; set => name = value; }
    }
}
