using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace _6524
{

    // 定义一个对象，实现 ICloneable 接口
    public partial class 形状匹配2 : Form
    {
        string Param_Path = System.Windows.Forms.Application.StartupPath + "\\Param.ini";
        HObject img = new HObject();
        DataTable data_result = new DataTable();
        Shape_matching M_Shape_matching = new Shape_matching();

        HSmartWindowControl M_window1 = new HSmartWindowControl();
        ZKHwindows m_ZKHwindows = new ZKHwindows();
        private int nowTaskID;
        private int nowoperationID;

        public int NowTaskID { get => nowTaskID; set => nowTaskID = value; }
        public int NowoperationID { get => nowoperationID; set => nowoperationID = value; }

        HObject sacleimg = new HObject();
        public double Mult;
        public double add;
        public bool Scale_enabled = false;
        List<string> listimage;
        int listimageindex = 0;
        public 形状匹配2()
        {
            InitializeComponent();


        }

        private void 形状匹配_Load(object sender, EventArgs e)
        {
            try
            {




                // M_Shape_matching.Load_data();

                m_ZKHwindows.NowImage = img;
                sacleimg = img;
                data_result.Columns.Add("编号", Type.GetType("System.Int32"));//添加Id列，存储数据类型为Int
                data_result.Columns.Add("分数", Type.GetType("System.Double"));//
                data_result.Columns.Add("位置X", Type.GetType("System.Double"));//
                data_result.Columns.Add("位置Y", Type.GetType("System.Double"));//
                data_result.Columns.Add("角度", Type.GetType("System.Double"));//
                dataGridView1.DataSource = data_result;

                M_window1.Dock = DockStyle.Fill;
                m_ZKHwindows.Dock = DockStyle.Fill;
                panel2.Controls.Add(M_window1);
                panel1.Controls.Add(m_ZKHwindows);
                panel1.BringToFront();
                HOperatorSet.SetDraw(M_window1.HalconWindow, "margin");
                HOperatorSet.SetColor(M_window1.HalconWindow, "green");
                HOperatorSet.SetLineWidth(M_window1.HalconWindow, 5);






                //控件数据与算子绑定
                numericUpDown5.DataBindings.Add(new Binding("Value", M_Shape_matching, "Hv_Matching_num", true, DataSourceUpdateMode.OnPropertyChanged));
                //numericUpDown5.DataBindings.Add(nameof(numericUpDown5.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_num));
                numericUpDown4.DataBindings.Add(nameof(numericUpDown4.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_rote_min));
                numericUpDown3.DataBindings.Add(nameof(numericUpDown3.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_rote_max));
                checkBox9.DataBindings.Add(nameof(checkBox9.Checked), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_runtime_Max_enabled));
                textBox1.DataBindings.Add(nameof(textBox1.Text), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_rote_step), true, DataSourceUpdateMode.OnValidation, 0, "N5");
                textBox2.DataBindings.Add(nameof(textBox2.Text), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_scale_step), true, DataSourceUpdateMode.OnValidation, 0, "N5");
                numericUpDown1.DataBindings.Add(nameof(numericUpDown1.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_scale_min));
                numericUpDown2.DataBindings.Add(nameof(numericUpDown2.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_scale_max));
                textBox3.DataBindings.Add(nameof(textBox3.Text), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_runtime_Max));
                numericUpDown6.DataBindings.Add(nameof(numericUpDown6.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_min_Score));
                numericUpDown7.DataBindings.Add(nameof(numericUpDown7.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_min_Contrastratio));

                numericUpDown16.DataBindings.Add(nameof(numericUpDown16.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_Pyramid_level));
                numericUpDown17.DataBindings.Add(nameof(numericUpDown17.Value), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_Greedy_algorithm));
                textBox4.DataBindings.Add(nameof(textBox4.Text), M_Shape_matching, nameof(M_Shape_matching.Hv_Matching_overlap_Max));
                //M_Shape_matching.hv_Matching_Greedy_algorithm = (int)(Math.Round((decimal)(numericUpDown17.Value / 100), 3));
                //M_Shape_matching.hv_Matching_overlap_Max = Convert.ToDouble(textBox4.Text);
                comboBox1.SelectedItem = 0;
            }
            catch (Exception)
            {


            }



        }


        private void RUN_Click(object sender, EventArgs e)
        {

            HTuple rec_row = new HTuple();
            HTuple rec_col = new HTuple();
            HTuple rec_pi = new HTuple();
            HTuple rec_length1 = new HTuple();
            HTuple rec_length2 = new HTuple();
            HTuple res_xld = new HTuple();
            if (checkBox6.Checked)
            {
                rec_row = Convert.ToDouble(textBox5.Text);
                rec_col = Convert.ToDouble(textBox6.Text);
                rec_pi = Convert.ToDouble(textBox9.Text);
                rec_length1 = Convert.ToDouble(textBox8.Text);
                rec_length2 = Convert.ToDouble(textBox7.Text);
            }
            HObject rec_xld = new HObject();
     
            if (Scale_enabled)
            {
                HObject outimg = new HObject();


                //Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                //add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                M_Shape_matching.img = outimg;
            }
            else
            {
                M_Shape_matching.img = img;
            }

            if (checkBox6.Checked)
            {

                HOperatorSet.GenRectangle2(out rec_xld, rec_row, rec_col, rec_pi, rec_length1, rec_length2);
                HOperatorSet.ReduceDomain(img, rec_xld, out M_Shape_matching.img);
            }




            if (M_Shape_matching.action(nowTaskID, nowoperationID))
            {



                bool Position_enable_X = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X", "false"));
                bool Position_enable_Y = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y", "false"));


                if (M_Shape_matching.Row.TupleLength() == M_Shape_matching.Hv_Matching_num)
                {
                    bool res = true;

                    if (Position_enable_X)
                    {


                        int Position_enable_X_Down = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Down", "0"));
                        int Position_enable_X_Up = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Up", "10000"));
                        if (M_Shape_matching.Row[0].D >= Position_enable_X_Down && M_Shape_matching.Row[0].D <= Position_enable_X_Up)
                        {

                        }
                        else
                        {
                            res = false;
                        }

                    }


                    if (Position_enable_Y)
                    {
                        int Position_enable_Y_Down = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Down", "0"));
                        int Position_enable_Y_Up = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Up", "10000"));

                        if (M_Shape_matching.Column[0].D >= Position_enable_Y_Down && M_Shape_matching.Column[0].D <= Position_enable_Y_Up)
                        {

                        }
                        else
                        {
                            res = false;
                        }


                    }
                    if (!res)
                    {
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "red");
                        HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_ZKHwindows.hWindowControl.HalconWindow);
                        if (checkBox6.Checked)
                        {
                            HOperatorSet.DispObj(rec_xld, m_ZKHwindows.hWindowControl.HalconWindow);
                        }
                        Getmodelinfo();
                    }
                    else
                    {
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_ZKHwindows.hWindowControl.HalconWindow);
                        if (checkBox6.Checked)
                        {
                            HOperatorSet.DispObj(rec_xld, m_ZKHwindows.hWindowControl.HalconWindow);
                        }
                        Getmodelinfo();
                    }
                }

                    




                // Up_Data();
            

        }

    }

        private void Getmodelinfo()
        {
            data_result.Clear();
            for (int i = 0; i < M_Shape_matching.hv_I.D; i++)
            {
                DataRow dr = data_result.NewRow();
                dr["编号"] = i + 1;
                dr["分数"] = (double)(Math.Round((decimal)(M_Shape_matching.Scale[i].D), 2));
                dr["位置X"] = (double)(Math.Round((decimal)(M_Shape_matching.Row[i].D), 2));
                dr["位置Y"] = (double)(Math.Round((decimal)(M_Shape_matching.Column[i].D), 2));
                dr["角度"] = (double)(Math.Round((decimal)(M_Shape_matching.Angle[i].D), 2));

                data_result.Rows.Add(dr);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 创建ColorDialog控件
            ColorDialog colorDialog = new ColorDialog();

            // 设置默认颜色
            colorDialog.Color = Color.Red;

            // 显示对话框
            DialogResult result = colorDialog.ShowDialog();

            // 如果用户点击了“确定”按钮
            if (result == DialogResult.OK)
            {
                // 获取所选颜色
                Color selectedColor = colorDialog.Color;

                // 在控制台输出所选颜色的RGB值
                Console.WriteLine("R={0}, G={1}, B={2}", selectedColor.R, selectedColor.G, selectedColor.B);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "margin");
            HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "red");
            HOperatorSet.SetLineWidth(m_ZKHwindows.hWindowControl.HalconWindow, 3);
            MessageBox.Show("开始绘制模型区域");
            HTuple Row1 = new HTuple();
            HTuple Row2 = new HTuple();
            HTuple Col1 = new HTuple();
            HTuple Col2 = new HTuple();
            HObject Reg = new HObject();
            HObject REC_img = new HObject();
            m_ZKHwindows.Drawing = true;
            HOperatorSet.DrawRectangle1(m_ZKHwindows.hWindowControl.HalconWindow, out Row1, out Col1, out Row2, out Col2);

            HOperatorSet.GenRectangle1(out Reg, Row1, Col1, Row2, Col2);
            m_ZKHwindows.Drawing = false;




            if (M_Shape_matching.createmodel(m_ZKHwindows.NowImage, Reg, out REC_img))
            {
                //HTuple hv_X1 = new HTuple();
                //HTuple hv_Y1 = new HTuple(), hv_X2 = new HTuple(), hv_Y2 = new HTuple();
                //HTuple hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
                //HTuple hv_HomMat2D = new HTuple();
                //HObject ho_TransContours = new HObject();   
                MessageBox.Show("模型绘制成功");

                show_model_contour(M_Shape_matching.hv_ModelID);




            }
            else { MessageBox.Show("模型绘制失败"); }

        }

        public void imgIsNotStretchDisplay(HObject L_img, HTuple Hwindow)
        {
            HTuple hv_Width, hv_Height;
            HTuple win_Width, win_Height, win_Col, win_Row, cwin_Width, cwin_Height;
            HOperatorSet.ClearWindow(Hwindow);
            HOperatorSet.GetImageSize(L_img, out hv_Width, out hv_Height);//获取图片大小规格
            HOperatorSet.GetWindowExtents(Hwindow, out win_Row, out win_Col, out win_Width, out win_Height);//获取窗体大小规格
            cwin_Height = 1.0 * win_Height / win_Width * hv_Width;//宽不变计算高          
            if (cwin_Height > hv_Height)//宽不变高能容纳
            {
                cwin_Height = 1.0 * (cwin_Height - hv_Height) / 2;
                HOperatorSet.SetPart(Hwindow, -cwin_Height, 0, cwin_Height + hv_Height - 1, hv_Width - 1);//设置窗体的规格
            }
            else//高不变宽能容纳
            {
                cwin_Width = 1.0 * win_Width / win_Height * hv_Height;//高不变计算宽
                cwin_Width = 1.0 * (cwin_Width - hv_Width) / 2;
                HOperatorSet.SetPart(Hwindow, 0, -cwin_Width, hv_Height - 1, cwin_Width + hv_Width - 1);//设置窗体的规格

            }

            HOperatorSet.DispObj(L_img, Hwindow);

            //HOperatorSet.FlushBuffer(Hwindow);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //当前模板的X，Y,rote
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "ModelX", M_Shape_matching.Row.D.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "ModelY", M_Shape_matching.Column.D.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Model_Angle", M_Shape_matching.Angle.D.ToString());
                //模板匹配的参数
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_min", M_Shape_matching.Hv_Matching_rote_min.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_max", M_Shape_matching.Hv_Matching_rote_max.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_min", M_Shape_matching.Hv_Matching_scale_min.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_max", M_Shape_matching.Hv_Matching_scale_max.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_step", M_Shape_matching.Hv_Matching_scale_step.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_step", M_Shape_matching.Hv_Matching_rote_step.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_num", M_Shape_matching.Hv_Matching_num.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_min_Score", M_Shape_matching.Hv_Matching_min_Score.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_overlap_Max", M_Shape_matching.Hv_Matching_overlap_Max.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_Pyramid_level", M_Shape_matching.Hv_Matching_Pyramid_level.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_Greedy_algorithm", M_Shape_matching.Hv_Matching_Greedy_algorithm.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_min_Contrastratio", M_Shape_matching.Hv_Matching_min_Contrastratio.ToString());

                //区域限制参数
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Region_enabled", checkBox6.Checked.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "RegionX", textBox5.Text);
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "RegionY", textBox6.Text);
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Regionlength1", textBox8.Text);
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Regionlength2", textBox7.Text);
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "RegionPI", textBox9.Text);


                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Scalenable", checkBox5.Checked.ToString());


                //模板
                M_Shape_matching.SaveModel(System.Windows.Forms.Application.StartupPath + @"\\Halconmodel\\" + comboBox1.Text + ".shm");

                MessageBox.Show("保存成功");
            }
            catch (Exception)
            {

                MessageBox.Show("保存失败");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {





        }

        private void 形状匹配_FormClosing(object sender, FormClosingEventArgs e)
        {
            // data_result.Columns.Clear();
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Halcon Model|*.shm";
                saveFileDialog1.Title = "Save an Model File";
                //saveFileDialog1.ShowDialog();

                // 打开文件对话框并检查用户是否选择了文件
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件的完整路径
                    string selectedImagePath = saveFileDialog1.FileName;

                    // 获取所选文件的名称

                    M_Shape_matching.SaveModel(selectedImagePath);
                    // 在控制台输出所选文件的名称
                    // Console.WriteLine("所选文件的名称是: " + selectedImageName);
                }

            }
            catch (Exception)
            {

            }

        }

        private void button13_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "Halcon Model|*.shm";
                OpenFileDialog1.Title = "Save an Model File";
                //saveFileDialog1.ShowDialog();

                // 打开文件对话框并检查用户是否选择了文件
                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件的完整路径
                    string selectedImagePath = OpenFileDialog1.FileName;

                    // 获取所选文件的名称

                    M_Shape_matching.LoadingModel(selectedImagePath);
                    show_model_contour(M_Shape_matching.hv_ModelID);
                }

            }
            catch (Exception)
            {

            }

        }

        public void show_model_contour(HTuple hv_ModelID)
        {



            // Local iconic variables 

            HObject ho_ModelContours, ho_Image, ho_TransContours;

            // Local control variables 

            HTuple hv_X1 = new HTuple(), hv_Y1 = new HTuple();
            HTuple hv_X2 = new HTuple(), hv_Y2 = new HTuple(), hv_RefRow = new HTuple();
            HTuple hv_RefColumn = new HTuple(), hv_HomMat2D = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_ModelContours.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
            //
            //Matching 01: Set a reference position to show the model
            hv_X1.Dispose(); hv_Y1.Dispose(); hv_X2.Dispose(); hv_Y2.Dispose();
            HOperatorSet.SmallestRectangle1Xld(ho_ModelContours, out hv_X1, out hv_Y1, out hv_X2,
                out hv_Y2);
            hv_RefRow.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RefRow = ((hv_X2.TupleMax()
                    ) - (hv_X1.TupleMin())) / 2;
            }
            hv_RefColumn.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RefColumn = ((hv_Y2.TupleMax()
                    ) - (hv_Y1.TupleMin())) / 2;
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Image.Dispose();
                HOperatorSet.GenImageConst(out ho_Image, "byte", hv_RefColumn * 2, hv_RefRow * 2);
            }
            hv_HomMat2D.Dispose();
            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
            ho_TransContours.Dispose();
            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
            imgIsNotStretchDisplay(ho_Image, M_window1.HalconWindow);
            HOperatorSet.DispObj(ho_TransContours, M_window1.HalconWindow);
            ho_ModelContours.Dispose();
            ho_Image.Dispose();

            hv_X1.Dispose();
            hv_Y1.Dispose();
            hv_X2.Dispose();
            hv_Y2.Dispose();
            hv_RefRow.Dispose();
            hv_RefColumn.Dispose();
            hv_HomMat2D.Dispose();

            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //模板匹配的参数
                M_Shape_matching.Hv_Matching_rote_min = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_min", ""));
                M_Shape_matching.Hv_Matching_rote_max = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_max", ""));
                M_Shape_matching.Hv_Matching_scale_min = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_min", ""));
                M_Shape_matching.Hv_Matching_scale_max = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_max", ""));
                M_Shape_matching.Hv_Matching_scale_step = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_scale_step", ""));
                M_Shape_matching.Hv_Matching_rote_step = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_rote_step", ""));
                M_Shape_matching.Hv_Matching_num = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_num", ""));
                M_Shape_matching.Hv_Matching_min_Score = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_min_Score", ""));
                M_Shape_matching.Hv_Matching_overlap_Max = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_overlap_Max", ""));
                M_Shape_matching.Hv_Matching_Pyramid_level = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_Pyramid_level", ""));
                M_Shape_matching.Hv_Matching_Greedy_algorithm = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_Greedy_algorithm", ""));
                M_Shape_matching.Hv_Matching_min_Contrastratio = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Hv_Matching_min_Contrastratio", ""));
                Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Mult", Mult.ToString()));
                add = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Add", add.ToString()));
                Scale_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Scalenable", "false"));
                checkBox5.Checked = Scale_enabled;

                checkBox2.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X", "false"));
                checkBox3.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y", "false"));

                numericUpDown11.Value= Convert.ToInt16(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Down", "0"));



                numericUpDown10.Value  = Convert.ToInt16(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Up", "10000"));




                numericUpDown13.Value = Convert.ToInt16(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Down", "0"));


                numericUpDown12.Value= Convert.ToInt16(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Up", "10000"));





                //模板
                M_Shape_matching.LoadingModel(System.Windows.Forms.Application.StartupPath + @"\\Halconmodel\\" + comboBox1.Text + ".shm");
                show_model_contour(M_Shape_matching.hv_ModelID);
                checkBox6.Checked = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "rec_enable", "false"));
                textBox6.Text = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "RegionY", "false");
                textBox7.Text = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Regionlength2", "false");
                textBox8.Text = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "Regionlength1", "false");
                textBox9.Text = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "RegionPI", "false");
                textBox5.Text = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.Text, "RegionX", "false");
            }
            catch (Exception)
            {


            }

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {


            if (m_ZKHwindows.NowImage != null)
            {

                if (trackBar2.Value - 10 <= trackBar1.Value)
                {
                    trackBar1.Value = trackBar2.Value - 10;
                }
                HObject outimg = new HObject();


                Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                m_ZKHwindows.NowImage = outimg;
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Add", add.ToString());

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {

                if (trackBar1.Value + 10 >= trackBar2.Value)
                {
                    trackBar2.Value = trackBar1.Value + 10;
                }
                HObject outimg = new HObject();


                Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                m_ZKHwindows.NowImage = outimg;
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Add", add.ToString());

            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Scalenable", checkBox5.Checked.ToString());
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
            }
            else
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Scalenable", checkBox5.Checked.ToString());
                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            img.Dispose();
            string imagepath = choose_file();
            HOperatorSet.ReadImage(out img, imagepath);
            m_ZKHwindows.NowImage = img;
            sacleimg = img;
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

        public string choose_DC()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

                // 设置对话框的标题
                folderBrowserDialog1.Description = "选择文件夹";

                // 打开文件夹对话框并检查用户是否选择了文件夹
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件夹的路径
                    return folderBrowserDialog1.SelectedPath;

                    // 在控制台输出所选文件夹的路径
                    //Console.WriteLine("所选文件夹的路径是: " + selectedFolderPath);
                }
                else { return null; }
            }
            catch (Exception)
            {

                return null;
            }

        }

        public void ListFiles(FileSystemInfo info, List<string> listPics)
        {

            DirectoryInfo dir = info as DirectoryInfo;

            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件
                if (file != null)
                {
                    string extension = Path.GetExtension(file.Name);
                    if (extension.ToUpper() == ".BMP")
                        listPics.Add(file.FullName);
                }

            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {



                string path = choose_DC();
                DirectoryInfo folder = new DirectoryInfo(path);
                FileSystemInfo fileinfo1 = folder as FileSystemInfo;
                listimage = new List<string>();
                ListFiles(fileinfo1, listimage);

                HOperatorSet.ReadImage(out img, listimage[0]);
                m_ZKHwindows.NowImage = img;
                listimageindex = 0;

            }
            catch (Exception)
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listimage.Count > 0)
            {
                try
                {
                    HTuple rec_row = new HTuple();
                    HTuple rec_col = new HTuple();
                    HTuple rec_pi = new HTuple();
                    HTuple rec_length1 = new HTuple();
                    HTuple rec_length2 = new HTuple();
                    HTuple res_xld = new HTuple();
                    if (checkBox6.Checked)
                    {
                        rec_row = Convert.ToDouble(textBox5.Text);
                        rec_col = Convert.ToDouble(textBox6.Text);
                        rec_pi = Convert.ToDouble(textBox9.Text);
                        rec_length1 = Convert.ToDouble(textBox8.Text);
                        rec_length2 = Convert.ToDouble(textBox7.Text);
                    }
                    HObject rec_xld = new HObject();
                    listimageindex--;
                    HOperatorSet.ReadImage(out img, listimage[listimageindex]);
                    m_ZKHwindows.NowImage = img;
                    sacleimg = img;
                    if (Scale_enabled)
                    {
                        HObject outimg = new HObject();


                        //Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                        //add = (Mult * trackBar1.Value) * -1;


                        HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                        M_Shape_matching.img = outimg;
                    }
                    else
                    {
                        M_Shape_matching.img = img;
                    }

                    if (checkBox6.Checked)
                    {

                        HOperatorSet.GenRectangle2(out rec_xld, rec_row, rec_col, rec_pi, rec_length1, rec_length2);
                        HOperatorSet.ReduceDomain(img, rec_xld, out M_Shape_matching.img);
                    }




                    if (M_Shape_matching.action(nowTaskID, nowoperationID))
                    {
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_ZKHwindows.hWindowControl.HalconWindow);
                        if (checkBox6.Checked)
                        {
                            HOperatorSet.DispObj(rec_xld, m_ZKHwindows.hWindowControl.HalconWindow);
                        }
                        Getmodelinfo();
                        // Up_Data();
                    }

                }
                catch (Exception)
                {

                    MessageBox.Show("图片不存在或已经是第一张");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listimage.Count > 0)
            {
                try
                {
                    HTuple rec_row = new HTuple();
                    HTuple rec_col = new HTuple();
                    HTuple rec_pi = new HTuple();
                    HTuple rec_length1 = new HTuple();
                    HTuple rec_length2 = new HTuple();
                    HTuple res_xld = new HTuple();
                    if (checkBox6.Checked)
                    {
                        rec_row = Convert.ToDouble(textBox5.Text);
                        rec_col = Convert.ToDouble(textBox6.Text);
                        rec_pi = Convert.ToDouble(textBox9.Text);
                        rec_length1 = Convert.ToDouble(textBox8.Text);
                        rec_length2 = Convert.ToDouble(textBox7.Text);
                    }
                    HObject rec_xld = new HObject();
                    listimageindex++;
                    HOperatorSet.ReadImage(out img, listimage[listimageindex]);
                    m_ZKHwindows.NowImage = img;
                    sacleimg = img;
                    if (Scale_enabled)
                    {
                        HObject outimg = new HObject();


                        //Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                        //add = (Mult * trackBar1.Value) * -1;


                        HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                        M_Shape_matching.img = outimg;
                    }
                    else
                    {
                        M_Shape_matching.img = img;
                    }


                    if (checkBox6.Checked)
                    {

                        HOperatorSet.GenRectangle2(out rec_xld, rec_row, rec_col, rec_pi, rec_length1, rec_length2);
                        HOperatorSet.ReduceDomain(img, rec_xld, out M_Shape_matching.img);
                    }



                    if (M_Shape_matching.action(nowTaskID, nowoperationID))
                    {
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_ZKHwindows.hWindowControl.HalconWindow);
                        if (checkBox6.Checked)
                        {
                            HOperatorSet.DispObj(rec_xld, m_ZKHwindows.hWindowControl.HalconWindow);
                        }
                        Getmodelinfo();
                        // Up_Data();
                    }

                }
                catch (Exception)
                {

                    MessageBox.Show("图片不存在或已经是最后一张");
                }
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

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

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {
                HObject rectange = new HObject();
                HTuple row = new HTuple();
                HTuple col = new HTuple();
                HTuple Length1 = new HTuple();
                HTuple Length2 = new HTuple();
                HTuple PI = new HTuple();

                MessageBox.Show("请在图片上区域");
                m_ZKHwindows.Drawing = true;
                HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "margin");
                HOperatorSet.DrawRectangle2(m_ZKHwindows.hWindowControl.HalconWindow, out row, out col, out PI, out Length1, out Length2);

                m_ZKHwindows.Drawing = false;
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "起始X:" + row.D.ToString(), "window", 12, 12, "black",
                  "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "起始Y:" + col.D.ToString(), "window", 32, 12, "black",
                    "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Length1：" + Length1.D.ToString(), "window",
                    52, 12, "black", "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Length2：" + Length2.D.ToString(), "window",
              72, 12, "black", "true");

                HOperatorSet.GenRectangle2(out rectange, row, col, PI, Length1, Length2);

                HOperatorSet.DispObj(rectange, m_ZKHwindows.hWindowControl.HalconWindow);

                textBox5.Text = row.D.ToString("F0");
                textBox6.Text = col.D.ToString("F0");
                textBox8.Text = Length1.D.ToString("F0");
                textBox7.Text = Length2.D.ToString("F0");
                textBox9.Text = PI.D.ToString("F0");

            }
            else
            {
                MessageBox.Show("请先载入图片！");
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
        
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "rec_enable", checkBox6.Checked.ToString());
       
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X", checkBox2.Checked.ToString());

            }
            else
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X", checkBox2.Checked.ToString());

            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y", checkBox3.Checked.ToString());

            }
            else
            {
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y", checkBox3.Checked.ToString());

            }
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Down", numericUpDown11.Value.ToString());
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_X_Up", numericUpDown10.Value.ToString());
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Down", numericUpDown13.Value.ToString());
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.Text, "Position_enable_Y_Up", numericUpDown12.Value.ToString());

        }

        /// <summary>
        /// 刷新算子至窗口
        /// </summary>
        /// <returns></returns>


    }
}
