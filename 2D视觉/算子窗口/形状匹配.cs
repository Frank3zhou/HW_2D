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

namespace _6524
{

    // 定义一个对象，实现 ICloneable 接口
    public partial class 形状匹配 : Form
    {
        string Path_calibration_Param = System.Windows.Forms.Application.StartupPath + @"\\calibration\Param.ini";
        HObject img;
        DataTable data_result = new DataTable();
        Shape_matching M_Shape_matching = new Shape_matching();

        HSmartWindowControl M_window1 = new HSmartWindowControl();
        ZKHwindows m_ZKHwindows = new ZKHwindows();
        private int nowTaskID;
        private int nowoperationID;

        public int NowTaskID { get => nowTaskID; set => nowTaskID = value; }
        public int NowoperationID { get => nowoperationID; set => nowoperationID = value; }

        HObject sacleimg = new HObject();
        public   double Mult;
        public  double add;
        public bool Scale_enabled = false;
        public 形状匹配(HObject image)
        {
            InitializeComponent();
            HOperatorSet.GenEmptyObj(out img);
            img = image;

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
            if (Scale_enabled)
            {
                HObject outimg = new HObject();


                Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                M_Shape_matching.img = outimg;
            }
            else
            {
                M_Shape_matching.img = img;
            }

           



            if (M_Shape_matching.action(nowTaskID, nowoperationID))
            {
                HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                HOperatorSet.DispObj(M_Shape_matching.ho_Transregion_final, m_ZKHwindows.hWindowControl.HalconWindow);
                Getmodelinfo();
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
               
                show_model_contour( M_Shape_matching.hv_ModelID);
             
              


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
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "ModelX", M_Shape_matching.Row.D.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "ModelY", M_Shape_matching.Column.D.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Model_Angle", M_Shape_matching.Angle.D.ToString());
                //模板匹配的参数
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_min", M_Shape_matching.Hv_Matching_rote_min.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_max", M_Shape_matching.Hv_Matching_rote_max.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_min", M_Shape_matching.Hv_Matching_scale_min.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_max", M_Shape_matching.Hv_Matching_scale_max.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_step", M_Shape_matching.Hv_Matching_scale_step.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_step", M_Shape_matching.Hv_Matching_rote_step.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_num", M_Shape_matching.Hv_Matching_num.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_min_Score", M_Shape_matching.Hv_Matching_min_Score.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_overlap_Max", M_Shape_matching.Hv_Matching_overlap_Max.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_Pyramid_level", M_Shape_matching.Hv_Matching_Pyramid_level.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_Greedy_algorithm", M_Shape_matching.Hv_Matching_Greedy_algorithm.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_min_Contrastratio", M_Shape_matching.Hv_Matching_min_Contrastratio.ToString());
                //模板
                M_Shape_matching.SaveModel(System.Windows.Forms.Application.StartupPath + @"\\calibration\Matching0" + comboBox1.Text + ".shm");

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
                    show_model_contour( M_Shape_matching.hv_ModelID);
                }

            }
                catch (Exception)
                {

                }
            
        }

        public void show_model_contour( HTuple hv_ModelID)
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
                M_Shape_matching.Hv_Matching_rote_min = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_min", ""));
                M_Shape_matching.Hv_Matching_rote_max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_max", ""));
                M_Shape_matching.Hv_Matching_scale_min = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_min", ""));
                M_Shape_matching.Hv_Matching_scale_max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_max", ""));
                M_Shape_matching.Hv_Matching_scale_step = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_scale_step", ""));
                M_Shape_matching.Hv_Matching_rote_step = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_rote_step", ""));
                M_Shape_matching.Hv_Matching_num = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_num", ""));
                M_Shape_matching.Hv_Matching_min_Score = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_min_Score", ""));
                M_Shape_matching.Hv_Matching_overlap_Max = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_overlap_Max", ""));
                M_Shape_matching.Hv_Matching_Pyramid_level = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_Pyramid_level", ""));
                M_Shape_matching.Hv_Matching_Greedy_algorithm = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_Greedy_algorithm", ""));
                M_Shape_matching.Hv_Matching_min_Contrastratio = Convert.ToInt32(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Hv_Matching_min_Contrastratio", ""));
                Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Mult", Mult.ToString()));
                add = Convert.ToDouble(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Add", add.ToString()));
                Scale_enabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Scalenable", "false"));
                //模板
                M_Shape_matching.LoadingModel(System.Windows.Forms.Application.StartupPath + @"\\calibration\Matching0" + comboBox1.Text + ".shm");
                show_model_contour(M_Shape_matching.hv_ModelID);
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
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model",  "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model",  "Add", add.ToString());

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
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Add", add.ToString());

            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Scalenable", checkBox5.Checked.ToString());
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
            }
            else
            {
                IniAPI.INIWriteValue(Path_calibration_Param, "Matching0" + comboBox1.Text + "Model", "Scalenable", checkBox5.Checked.ToString());
                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
            }
        }

        /// <summary>
        /// 刷新算子至窗口
        /// </summary>
        /// <returns></returns>


    }
}
