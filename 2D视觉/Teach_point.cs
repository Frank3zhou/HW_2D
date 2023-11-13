using _6524.Class;
using HalconDotNet;
using HslCommunication.LogNet;
using HslCommunication;
using MvCamCtrl.NET;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using Microsoft.VisualBasic;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.ConstrainedExecution;
using NPOI.SS.Formula.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace _6524
{

    public partial class Teach_point : Form
    {
        ZKHwindows m_window;
        DataTable D_Teach_point = new DataTable(); //示教点位和结果表
        fanuctcpip m_Robot;
        bool robotconnected = false;
        MVS_SDK m_Camera = new MVS_SDK();
        bool m_Cameraconnected = false;
        string cameraip;
        HObject Img = new HObject();
        private CancellationTokenSource _cancellationTokenSource;

        //九点标定参数
        string Path_calibration1 = Application.StartupPath + @"\\calibration\calibration1.mat";
        string Path_calibration_Param = Application.StartupPath + @"\\calibration\Param.ini";
        Robotcalibration M_Calibration = new Robotcalibration();
        int Rotate_Number;
        HTuple RobotX, RobotY, PixelX, PixelY;
        double[] RX;
        double[] RY;
        double[] PX;
        double[] PY;


        // 旋转中心参数
        HObject ho_Contour = new HObject();//拟合圆的轮廓
        HObject ho_Contour_Circle = new HObject();//拟合出得到的圆
        HObject ho_Circle_region = new HObject();//拟合出圆的reigon
                                                 //  HObject ho_Region;//
        HTuple hv_Row = new HTuple();  //拟合圆的的点X数组
        HTuple hv_Column = new HTuple();//拟合圆的的点Y数组
        HTuple hv_Row_Result = new HTuple();  //拟合圆的的结果点像素X
        HTuple hv_Column_Result = new HTuple();//拟合圆的的结果像素Y
        HTuple hv_Radius_Result = new HTuple();////拟合圆的的结果半径
        HTuple hv_StartPhi = new HTuple();
        HTuple hv_EndPhi = new HTuple();
        HTuple hv_PointOrder = new HTuple();


        //原理

        //旋转中心参数
        // ∆X=2R(sin(θ/2))²
        // ∆Y=R*sinθ;
        //旋转中心和当前坐标的相对位置，可以通过推算得知旋转中心实际当前位置，再将值反推
        HTuple dis_X, dis_Y;



        public Teach_point()
        {
            InitializeComponent();
        }

        private void Teach_point_Load(object sender, EventArgs e)
        {
            m_window = new ZKHwindows();
            m_window.Dock = DockStyle.Fill;
            groupBox7.Controls.Add(m_window);
            groupBox6.BringToFront();
            D_Teach_point = Create_teachpoint(3, 3);
            dataGridView1.DataSource = D_Teach_point;
            textBox13.Text = IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offY", "0");
            textBox15.Text = IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offX", "0");
            textBox12.Text = IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offR", "0");
            checkBox5.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "off_enabled", "false"));
            checkBox6.Checked = Convert.ToBoolean(IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "off_Angle_enabled", "false"));
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (robotconnected)
            {

                string a = "";
                if (m_Robot.readNowPR(ref a))
                {
                    string[] xyzwprstr = a.Split(',');
                    textBox4.Text = xyzwprstr[0];
                    textBox3.Text = xyzwprstr[1];
                    textBox5.Text = xyzwprstr[2];
                    textBox10.Text = xyzwprstr[3];
                    textBox9.Text = xyzwprstr[4];
                    textBox6.Text = xyzwprstr[5];
                    MessageBox.Show("Read Success!");
                }

            }
            else
            {
                MessageBox.Show(" please connect robot");
            }

        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                //载入当前坐标
                double Teach_X, Teach_Y, Teach_U;
                Teach_X = Convert.ToDouble(textBox4.Text);
                Teach_Y = Convert.ToDouble(textBox3.Text);
                Teach_U = Convert.ToDouble(textBox6.Text);
                //通过行列推算点位
                double distance_move = Convert.ToInt16(textBox1.Text);//平移移动量
                int X_move_number = Convert.ToInt16(textBox8.Text);//平移X移动次数
                int Y_move_number = Convert.ToInt16(textBox7.Text);//平移Y移次数
                                                                   //通过旋转次次数推算点位
                double Rotate_Angle = Convert.ToInt16(textBox2.Text);//旋转角度
                Rotate_Number = Convert.ToInt16(textBox14.Text);//旋转次数


                int Number = X_move_number * Y_move_number;
                if (checkBox2.Checked)
                {
                    Number = Number + Rotate_Number;
                }
                else
                {
                    Rotate_Number = 0;
                }

                double[] RX = new double[Number];
                double[] RY = new double[Number];
                double[] PX = new double[Number];
                double[] PY = new double[Number];


                D_Teach_point.Rows.Clear();
                double Teach_X_init = Teach_X - (distance_move * ((X_move_number - 1) / 2));//第一点位置
                double Teach_Y_init = Teach_Y - (distance_move * ((Y_move_number - 1) / 2));
                for (int i = 0; i < Number; i++)
                {
                    if (i < Number - Rotate_Number)
                    {
                        DataRow newRow = D_Teach_point.NewRow();
                        newRow[0] = i + 1;

                        int X_now = i / X_move_number;//现在在第几行
                                                      //   int Y_now = i / Y_move_number;//现在在第几列
                        newRow[1] = Teach_X_init + (distance_move * X_now);
                        int remainder = i % Y_move_number;
                        newRow[2] = Teach_Y_init + (distance_move * remainder);





                        newRow[3] = Teach_U;
                        newRow[4] = 0;
                        newRow[5] = 0;
                        // newRow[6] = 0;

                        D_Teach_point.Rows.Add(newRow);
                    }
                    else
                    {
                        DataRow newRow = D_Teach_point.NewRow();
                        newRow[0] = i + 1;


                        newRow[1] = Teach_X;//X 
                        newRow[2] = Teach_Y;//Y
                        newRow[3] = Teach_U + (Rotate_Angle * (i - (Number - (Rotate_Number / 2))));//U
                        newRow[4] = 0;
                        newRow[5] = 0;

                        D_Teach_point.Rows.Add(newRow);
                    }


                }
            }
            catch (Exception)
            {

                MessageBox.Show("数据生成失败");
            }
        }
        public DataTable Create_teachpoint(int X, int Y)
        {
            DataTable m_DataTable = new DataTable();
            int Number = X * Y;

            m_DataTable.Columns.Add("Id", Type.GetType("System.Int32"));//添加Id列，存储数据类型为Int
            m_DataTable.Columns.Add("X", Type.GetType("System.Double"));//
            m_DataTable.Columns.Add("Y", Type.GetType("System.Double"));//
            m_DataTable.Columns.Add("U", Type.GetType("System.Double"));//
            m_DataTable.Columns.Add("PointX", Type.GetType("System.Double"));//
            m_DataTable.Columns.Add("PointY", Type.GetType("System.Double"));//

            for (int i = 0; i < Number; i++)
            {
                DataRow newRow = m_DataTable.NewRow();
                newRow[0] = i + 1;
                newRow[1] = 0;
                newRow[2] = 0;
                newRow[3] = 0;
                newRow[4] = 0;
                newRow[5] = 0;
                //  newRow[6] = 0;

                m_DataTable.Rows.Add(newRow);
            }
            return m_DataTable;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = !checkBox4.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = !checkBox3.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string roobtIP = IniAPI.INIGetStringValue(Path_calibration_Param, "Robot", "IP", "192.168.3.100"); ;
            m_Robot = new fanuctcpip();
            if (m_Robot.connectFanuc(roobtIP))
            {
                MessageBox.Show("Robot Connected");
                robotconnected = true;
            }
            else
            {
                MessageBox.Show("Robot Connected fail");
                robotconnected = false;
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {

            cameraip = IniAPI.INIGetStringValue(Path_calibration_Param, "robotCamera", "IP", "192.168.3.2");
            if (m_Camera.Connect_Cam(cameraip))
            {
                MessageBox.Show("Camera Connected");
                m_Cameraconnected = true;
            }
            else
            {
                MessageBox.Show("Camera fail");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (m_Cameraconnected)
            {
                if (checkBox7.Checked)//连续采集
                {
                    if (_cancellationTokenSource == null)
                    {
                        if (m_Camera.State == 0)
                        {
                            m_Camera.Connect_Cam(textBox2.Text);
                        }
                        if (m_Camera.State == 1)
                        {
                            m_Camera.OpenCamera();
                        }
                        m_Camera.Set_callback(1);

                        m_Camera.Start();//开始采集流

                        button8.Text = "停止采集";
                        _cancellationTokenSource = new CancellationTokenSource();
                        // 启动新的后台线程
                        Task.Run(() => BackgroundThread(_cancellationTokenSource.Token));
                    }
                    else
                    {
                        m_Camera.Stop();//开始停止采集流
                        m_Camera.dispose();
                        //  mvs_SDK.Stop 
                        // 取消后台线程
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource = null;
                        button8.Text = "采集图像";
                    }
                }
                else //采集一张
                {
                    if (takepicture())
                    {

                    }
                    else
                    {
                        MessageBox.Show("Take Image error");
                    }
                }

            }
            else
            {
                MessageBox.Show("please connected Camera");
            }



        }

        private void BackgroundThread(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                if (m_Camera.signal)
                    HOperatorSet.DispObj(m_Camera.Himage, m_window.hWindowControl.HalconWindow);
                m_Camera.signal = false;
                // mvs_SDK.Get_Oneframe();
                // 检查标志位，如果标志位为true，则退出循环
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            // 后台线程结束后，更新UI
            // UpdateUI("后台线程结束！");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {



                //HTuple row, col, r;
                //HObject cir = new HObject();
                //find_circle(m_window.NowImage, out row, out col, out r);
                //HOperatorSet.GenCircle(out cir, row, col, r);
                string strHost = "";
                strHost = Interaction.InputBox("输入模板型号", "frrjiftest", strHost, 0, 0);
                if (string.IsNullOrEmpty(strHost))
                {
                    System.Environment.Exit(0);
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

                    //  MessageBox.Show("将机器人手动偏移一定距离");
                    //新采集一张图
                    if (m_Cameraconnected)
                    {

                        if (takepicture())
                        {

                        }
                        else
                        {
                            MessageBox.Show("Take Image error");
                        }

                    }
                    else
                    {
                        MessageBox.Show("please connected Camera");
                    }




                    double Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Mult", ""));
                    double add = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Add", ""));
                    bool Scale_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Scalenable", "false"));

                    HObject outimg = new HObject();
                    outimg = Img;
                    if (Scale_enabled)
                    {
                        HOperatorSet.ScaleImage(Img, out outimg, Mult, add);
                        M_Shape_matching.img = outimg;

                    }
                    else
                    {
                        M_Shape_matching.img = outimg;
                    }





                    //    M_Shape_matching.img = m_window.NowImage;

                    if (M_Shape_matching.action(0, 0))
                    {
                        HOperatorSet.DispObj(outimg, m_window.hWindowControl.HalconWindow);

                        HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_window.hWindowControl.HalconWindow);

                        HObject C = new HObject();
                        HTuple center_row, center_column, pointorder,arear;
                      
                        center_row = new HTuple();
                        center_column = new HTuple();
                        center_column.Dispose();
                        center_row.Dispose();

                        pointorder = new HTuple();
                        pointorder.Dispose();

                        arear = new HTuple();
                        arear.Dispose();

                        //HOperatorSet.AreaCenterXld(M_Shape_matching.ho_Transregion_final,out arear,out center_row,out center_column,out pointorder);
                        HOperatorSet.GenCircle(out C, M_Shape_matching.Row, M_Shape_matching.Column, 10);
                        HOperatorSet.GenCircle(out C, center_row, center_column, 10);
                        HOperatorSet.DispText(m_window.hWindowControl.HalconWindow, "X：" + M_Shape_matching.Row.D.ToString("F2"), "window", 12, 12, "black", new HTuple(), new HTuple());
                        HOperatorSet.DispText(m_window.hWindowControl.HalconWindow, "Y：" + M_Shape_matching.Column.D.ToString("F2"), "window", 32, 12, "black", new HTuple(), new HTuple());
                        HOperatorSet.DispText(m_window.hWindowControl.HalconWindow, "R：" + M_Shape_matching.Angle.D.ToString(), "window", 62, 12, "black", new HTuple(), new HTuple());
                        HOperatorSet.DispObj(C, m_window.hWindowControl.HalconWindow);


                    }
                    else
                    {
                        MessageBox.Show("测试失败");
                    }

                }
                catch (Exception)
                {

                    MessageBox.Show("测试失败");
                }



            }
            catch (Exception)
            {

                // throw;
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {


        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (m_window.NowImage != null)
            {
                形状匹配 m_match = new 形状匹配(m_window.NowImage);
                m_match.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先采集图片");
            }

            //int model222 = 1;
            ////输入mark的像素坐标
            //HTuple PX = new HTuple();
            //HTuple PY = new HTuple();
            //HTuple RX = new HTuple();
            //HTuple RY = new HTuple();
            //M_Calibration.Affine_XY(Path_calibration1, PX, PY, out RX, out RY);
            //if (model222 == 1)
            //{
            //    IniAPI.INIWriteValue(Path_calibration_Param, "datum_mark1", "markX", RX.D.ToString());
            //    IniAPI.INIWriteValue(Path_calibration_Param, "datum_mark1", "markY", RY.D.ToString());
            //}
            //else
            //{
            //    IniAPI.INIWriteValue(Path_calibration_Param, "datum_mark2", "markX", RX.D.ToString());
            //    IniAPI.INIWriteValue(Path_calibration_Param, "datum_mark2", "markY", RY.D.ToString());
            //}





        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            double offY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offY", "0"));
            double offX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offX", "0"));
            double offR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "offR", "0"));
            bool off_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "compensate", "off_enabled", "false"));
            bool off_Angle_enabled = Convert.ToBoolean(IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "off_Angle_enabled", "false"));
            string strHost = "1";
            strHost = Interaction.InputBox("输入模板型号", "frrjiftest", strHost, 0, 0);

            if (string.IsNullOrEmpty(strHost))
            {
                System.Environment.Exit(0);
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

                dis_X = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "X", "0"));

                dis_Y = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "center_rotation", "Y", "0"));
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




                ////读取拍照初始位和铆压初始位
                double Pressure_RobotX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotX", "0"));
                double Pressure_RobotY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotY", "0"));
                double Pressure_RobotR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotR", "0"));
                double Picture_RobotX = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotX", "0"));
                double Picture_RobotY = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotY", "0"));
                double Picture_RobotR = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotR", "0"));

                //计算角度偏移
                double Robot_R_off = Picture_RobotR - Pressure_RobotR;




                MessageBox.Show("将机器人手动偏移Mark点一定距离，但不要超出相机视野范围");
                //新采集一张图
                if (m_Cameraconnected)
                {

                    if (takepicture())
                    {

                    }
                    else
                    {
                        MessageBox.Show("Take Image error");
                    }

                }
                else
                {
                    MessageBox.Show("please connected Camera");
                }
                M_Shape_matching.img = m_window.NowImage;
                if (M_Shape_matching.action(0, 0))
                {
                    ////mark初始机械坐标
                    HTuple RX1 = new HTuple();
                    HTuple RY1 = new HTuple();

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
                    double disX1 = (RX1 - RX0);
                    double disY1 = (RY1 - RY0);

                    double lengrh_C = Math.Sqrt((disX1 * disX1) + (disY1 * disY1));
                    double theta = Math.Atan(disY1 / disX1); // 计算 arctan(a / b) 的角度

                    // 将弧度转换为度
                    double degrees = theta * (180.0 / Math.PI);

                    // 最终角度差
                    double final_offR = degrees + Robot_R_off;



                    //旋转后角度偏差补偿
                    double dis_RX = (R * Math.Sin(dis_Angle * (Math.PI / 180)));
                    double dis_RY = 2 * (R * (Math.Sin((dis_Angle * (Math.PI / 180)) / 2)) * (Math.Sin((dis_Angle * (Math.PI / 180)) / 2)));

                    //输出最后的补偿2
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


                    double offx = lengrh_C *Math.Sin(final_offR);
                    double offy = lengrh_C * Math.Cos(final_offR);



                    //double final_disR = dis_Angle;
                    //if (final_disR > 180)
                    //{
                    //    final_disR = final_disR - 360;
                    //}
                    MessageBox.Show("X 补偿：" + offx.ToString() + "\r\n" + "Y 补偿：" + offy.ToString() + "\r\n" +"拍照位偏差X:"+ disX1.ToString() +"\r\n" +"拍照位偏差Y:"+disY1.ToString());


                 DialogResult a =  MessageBox.Show("是否发送补偿给机械手", "注意", MessageBoxButtons.OKCancel);
                    if (a == DialogResult.OK)
                    {
                     string    PR1 = offx.ToString("F3")+"," + offy .ToString("F3")+ ",0,0,0,0,";
                        if (m_Robot.writePR("1", PR1))
                        {
                            m_Robot.WrieR("6", "1");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("测试失败");
                }

            }
            catch (Exception)
            {

                MessageBox.Show("测试失败");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (robotconnected)
            {
                string strHost = "";
                strHost = Interaction.InputBox("输入模板型号", "frrjiftest", strHost, 0, 0);
                if (string.IsNullOrEmpty(strHost))
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    try
                    {
                        string a = "";
                        if (m_Robot.readNowPR(ref a))
                        {
                            string[] xyzwprstr = a.Split(',');

                            //当前模板的X，Y,rote
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotX", xyzwprstr[0]);
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotY", xyzwprstr[1]);
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Pressure_RobotR", xyzwprstr[5]);
                            MessageBox.Show("保存模型" + strHost + "铆压初始位置成功");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            else
            {
                MessageBox.Show("请连接机械手");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Teach_point_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Camera.dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (robotconnected)
            {
                string strHost = "";
                strHost = Interaction.InputBox("输入模板型号", "frrjiftest", strHost, 0, 0);
                if (string.IsNullOrEmpty(strHost))
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    try
                    {
                        string a = "";
                        if (m_Robot.readNowPR(ref a))
                        {
                            string[] xyzwprstr = a.Split(',');
                
                            //当前模板的X，Y,rote
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotX", xyzwprstr[0]);
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotY", xyzwprstr[1]);
                            IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + strHost + "Model", "Picture_RobotR", xyzwprstr[5]);
                            MessageBox.Show("保存模型"+ strHost+"拍照位成功");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            else 
            {
                MessageBox.Show("请连接机械手");
            }
    
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

            IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "offX", textBox15.Text);

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                panel2.Enabled = true;
                IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "off_enabled", checkBox5.Checked.ToString());
            }
            else
            {
                panel2.Enabled = false;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "offY", textBox13.Text);
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "offR", textBox15.Text);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            
              
                IniAPI.INIWriteValue(Path_calibration_Param, "compensate", "off_Angle_enabled", checkBox6.Checked.ToString());
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
            RS232 rS232 = new RS232();
            rS232.BaudRate = 19200;
            rS232.DataBits = 8;
            rS232.StopBits =1;
            rS232.COMPort = Form1.Comname1;
            rS232.Parity = 0;
            if (button12.Text == "打开光源")
            {
                rS232.Init();

                if (!(rS232.Open() == 1))
                {
                    MessageBox.Show("光源控制器连接失败");
                }
                else
                {
                    int brightness = 150;

                    rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                    button12.Text = "关闭光源";
                }

            }
            else
            {
                rS232.Init();

                if (!(rS232.Open() == 1))
                {
                    MessageBox.Show("光源控制器连接失败");
                }
                else
                {
                    int brightness = 0;

                    rS232.SerialPort.WriteLine("SB0" + brightness.ToString("D3") + "#" + "\r");//关闭光源 
                    button12.Text = "打开光源";
                }
            }
            

            rS232.Close();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if(button8.Text=="停止采集")
            {
                checkBox7.Checked = true;
                MessageBox.Show("请先停止采集");
            }
        }

        private void Teach_point_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_Robot.close();
            }
            catch (Exception)
            {

               // throw;
            }
          
        }       

        private void button13_Click(object sender, EventArgs e)
        {
            HObject img = new HObject(); 
            try
            {
                img.Dispose();
                string imagepath = choose_file();
                HOperatorSet.ReadImage(out img, imagepath);
                m_window.NowImage = img;
              //  sacleimg = img;
            }
            catch (Exception)
            {


            }
        }

        public string choose_file()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                // 设置对话框的标题和筛选条件
                openFileDialog1.Title = "选择图像文件";
                openFileDialog1.Filter = "图像文件 (*.bmp, *.jpg, *.jpeg, *.png) | *.bmp; *.jpg; *.jpeg; *.png";

                // 打开文件对话框并检查用户是否选择了文件
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件的完整路径
                    string selectedImagePath = openFileDialog1.FileName;

                    // 获取所选文件的名称
                    return selectedImagePath;

                    // 在控制台输出所选文件的名称
                    // Console.WriteLine("所选文件的名称是: " + selectedImageName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }


        }



        private bool takepicture()
        {
            try
            {
                if (m_Cameraconnected)
                {
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
                            m_window.NowImage = Img;

                            Console.WriteLine("Get_onefrmae is OK ");

                            m_Camera.Stop();
                            m_Camera.close();


                        }

                    }
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

        private void button3_Click(object sender, EventArgs e)
        {
            bool failtoteach = false;
            bool robotarrive = false;
            //拟合圆的的点X数组
            double[] R_PX = new double[Rotate_Number];
            double[] R_PY = new double[Rotate_Number];
            RX = new double[D_Teach_point.Rows.Count - Rotate_Number];
            RY = new double[D_Teach_point.Rows.Count - Rotate_Number];
            PX = new double[D_Teach_point.Rows.Count - Rotate_Number];
            PY = new double[D_Teach_point.Rows.Count - Rotate_Number];
            try
            {
                if (m_Cameraconnected && robotconnected)
                {
                    #region 载入模板
                    string strHost = "Teach";
                    //strHost = Interaction.InputBox("输入模板型号", "frrjiftest", strHost, 0, 0);
                    //if (string.IsNullOrEmpty(strHost))
                    //{
                    //    System.Environment.Exit(0);
                    //}

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
                    #endregion
                    for (int i = 0; i < D_Teach_point.Rows.Count; i++)
                    {
                        robotarrive = false;
                        //将示教坐标写人PR6
                        string PRvalue = D_Teach_point.Rows[i][1].ToString() + "," + D_Teach_point.Rows[i][2].ToString() + "," + textBox5.Text + "," + textBox10.Text + "," + textBox9.Text + "," + D_Teach_point.Rows[i][3].ToString();
                        if (m_Robot.writePR("6", PRvalue))
                        {
                            //告诉机器人可以走PR6点位了
                            if (m_Robot.WrieR("3", "1"))
                            {
                                for (int T = 0; T < 1500; T++)
                                {
                                    Thread.Sleep(200);
                                    string A = null;
                                    //等待机器人到位信号
                                    if (m_Robot.readR("4", ref A))
                                    {
                                        if (A == "1")
                                        {
                                            Thread.Sleep(20);
                                            m_Robot.WrieR("4", "0");
                                            robotarrive = true;
                                            break;
                                        }
                                    }



                                }
                            }
                            else
                            {
                                failtoteach = true;
                                break;
                            }


                            //  MessageBox.Show("Write Success!");
                        }
                        else
                        {
                            failtoteach = true;
                            break;
                        }


                        //机器人到位后开始拍照取点
                        if (robotarrive)
                        {
                            if (takepicture())
                            {
                                //HTuple R_pixelX = new HTuple();
                                //HTuple R_pixelY = new HTuple();
                                // halcon 处理图像获取XY像素坐标
                                HTuple row = new HTuple();
                                HTuple col = new HTuple();
                                HTuple r = new HTuple();
                                HObject cir = new HObject();
                                row.Dispose();
                                col.Dispose();
                                r.Dispose();
                                find_circle(m_window.NowImage, out row, out col, out r);
                                HOperatorSet.GenCircle(out cir, row, col, r);



                                HOperatorSet.DispObj(cir, m_window.hWindowControl.HalconWindow);

                                M_Shape_matching.img = m_window.NowImage;
                                //if (M_Shape_matching.action(0, 0))
                                //{
                                //    R_pixelX = M_Shape_matching.Row;
                                //    R_pixelY = M_Shape_matching.Column;

                                //    HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_window.hWindowControl.HalconWindow);
                                //}
                                //else
                                //{
                                //    MessageBox.Show("模板匹配失败!");
                                //    failtoteach = true;
                                //    break;
                                //}


                                //D_Teach_point.Rows[i][4] = R_pixelX.D;
                                //D_Teach_point.Rows[i][5] = R_pixelY.D;
                                D_Teach_point.Rows[i][4] = row.D;
                                D_Teach_point.Rows[i][5] = col.D;


                                // 手动赋值
                                if (i < D_Teach_point.Rows.Count - Rotate_Number)
                                {
                                    RX[i] = Convert.ToDouble(D_Teach_point.Rows[i][1]);
                                    RY[i] = Convert.ToDouble(D_Teach_point.Rows[i][2]);
                                    PX[i] = Convert.ToDouble(D_Teach_point.Rows[i][4]);
                                    PY[i] = Convert.ToDouble(D_Teach_point.Rows[i][5]);
                                }
                                else
                                {
                                    R_PX[i - (D_Teach_point.Rows.Count - Rotate_Number)] = Convert.ToDouble(D_Teach_point.Rows[i][4]);
                                    R_PY[i - (D_Teach_point.Rows.Count - Rotate_Number)] = Convert.ToDouble(D_Teach_point.Rows[i][5]);

                                    //计算旋转中心
                                }

                            }
                            else
                            {
                                MessageBox.Show("take image fail!");
                                failtoteach = true;
                                break;
                            }
                        }
                    }

                    if (failtoteach)
                    {
                        string PRvalue = textBox4.Text + "," + textBox3.Text + "," + textBox5.Text + "," + textBox10.Text + "," + textBox9.Text + "," + textBox6.Text;
                        m_Robot.writePR("6", PRvalue);

                        //告诉机器人可以走PR6点位了
                        if (m_Robot.WrieR("3", "1"))
                        {
                            for (int T = 0; T < 1500; T++)
                            {
                                Thread.Sleep(200);
                                string A = null;
                                //等待机器人到位信号
                                if (m_Robot.readR("4", ref A))
                                {
                                    if (A == "1")
                                    {
                                        Thread.Sleep(20);
                                        m_Robot.WrieR("4", "0");
                                        robotarrive = true;
                                        break;
                                    }
                                }



                            }
                        }

                        MessageBox.Show("标定失败");
                    }
                    RobotX = RX;
                    RobotY = RY;
                    PixelX = PX;
                    PixelY = PY;

                    //创建标定文件
                    M_Calibration.Create_calibration(PixelX, PixelY, RobotX, RobotY, Path_calibration1);

                    //创建旋转中心
                    if (Rotate_Number > 3)
                    {
                        hv_Row = R_PX;
                        hv_Column = R_PY;

                        if (createrotecenterpoint())
                        {
                            IniAPI.INIWriteValue(Path_calibration_Param, "center_rotation", "X", dis_X.D.ToString());
                            IniAPI.INIWriteValue(Path_calibration_Param, "center_rotation", "Y", dis_Y.D.ToString());

                            MessageBox.Show("旋转中心建立完成");
                        }
                        else
                        {
                            MessageBox.Show("旋转中心建立失败");
                        }
                    }

                    //测试标定文件
                    HTuple N_PixelX = new HTuple();
                    HTuple N_PixelY = new HTuple();
                    HTuple A_RobotX = new HTuple();
                    HTuple A_RobotY = new HTuple();
                    M_Calibration.Affine_XY(Path_calibration1, N_PixelX, N_PixelY, out A_RobotX, out A_RobotY);


                    //开始计算校正精度

                   
                }
                else
                {
                    MessageBox.Show("Please connect robot or camera！");
                }

                



            }
            catch (Exception)
            {
                string PRvalue = textBox4.Text + "," + textBox3.Text + "," + textBox5.Text + "," + textBox10.Text + "," + textBox9.Text + "," + textBox6.Text;
                m_Robot.writePR("6", PRvalue);
                Thread.Sleep(1000);
                //告诉机器人可以走PR6点位了
                if (m_Robot.WrieR("3", "1"))
                {
                    for (int T = 0; T < 1500; T++)
                    {
                        Thread.Sleep(200);
                        string A = null;
                        //等待机器人到位信号
                        if (m_Robot.readR("4", ref A))
                        {
                            if (A == "1")
                            {
                                Thread.Sleep(20);
                                m_Robot.WrieR("4", "0");
                                robotarrive = true;
                                break;
                            }
                        }



                    }
                }
                MessageBox.Show("自动示教失败！");

            }
        }


        private bool createrotecenterpoint() //输入点的实际坐标值
        {
            try
            {
                // 拟合一个圆
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Row, hv_Column);//通过多点拟合轮廓

                hv_Row_Result.Dispose(); hv_Column_Result.Dispose(); hv_Radius_Result.Dispose(); hv_StartPhi.Dispose(); hv_EndPhi.Dispose(); hv_PointOrder.Dispose();
                HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2, out hv_Row_Result,
                    out hv_Column_Result, out hv_Radius_Result, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                ho_Contour_Circle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_Contour_Circle, hv_Row_Result, hv_Column_Result, hv_Radius_Result,
                    0, 6.28318, "positive", 1);

                ///需要反推算计算出旋转中心
                ///
                M_Calibration.Affine_XY(Path_calibration1, hv_Row_Result, hv_Column_Result, out dis_X, out dis_Y);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public void find_circle(HObject ho_Image20230826183057975, out HTuple hv_Row,
      out HTuple hv_Column, out HTuple hv_Radius)
        {



            // Local iconic variables 

            HObject ho_ImageScaled, ho_Regions, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_SelectedRegions1;

            // Local control variables 

            HTuple hv_Number = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            hv_Row = new HTuple();
            hv_Column = new HTuple();
            hv_Radius = new HTuple();
            ho_ImageScaled.Dispose();
            HOperatorSet.ScaleImage(ho_Image20230826183057975, out ho_ImageScaled, 5.4,
                -141);
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_ImageScaled, out ho_Regions, 0, 158);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 1500, 99999);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "circularity",
                "and", 0.8, 1);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
            {
                hv_Row.Dispose(); hv_Column.Dispose(); hv_Radius.Dispose();
                HOperatorSet.SmallestCircle(ho_SelectedRegions1, out hv_Row, out hv_Column,
                    out hv_Radius);

            }
            ho_ImageScaled.Dispose();
            ho_Regions.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();

            hv_Number.Dispose();

            return;
        }

    }


}
