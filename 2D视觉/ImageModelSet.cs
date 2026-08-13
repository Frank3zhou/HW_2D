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
using HalconDotNet;
using NPOI.SS.Formula.Functions;
using static NPOI.POIFS.Crypt.CryptoFunctions;

namespace _6524
{
    public partial class ImageModelSet : Form
    {
        ZKHwindows m_ZKHwindows;
        HObject img;
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
        HObject sacleimg=new HObject();
        double Mult ;
        double add ;
        public ImageModelSet()
        {
            InitializeComponent();
        }

        private void ImageModelSet_Load(object sender, EventArgs e)
        {
            m_ZKHwindows = new ZKHwindows();
            HOperatorSet.GenEmptyObj(out img);
            m_ZKHwindows.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_ZKHwindows);
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow,"fill");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                img.Dispose();
                string imagepath = choose_file();
                HOperatorSet.ReadImage(out img, imagepath);
                m_ZKHwindows.NowImage = img;
                sacleimg = img;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {
                HObject circle= new HObject();
                HTuple row = new HTuple();
                HTuple col = new HTuple();  
                HTuple Ridus = new HTuple();
                HTuple area = new HTuple(); 
                HTuple t = new HTuple();
                HTuple t2 = new HTuple();
                MessageBox.Show("请在图片上绘制冲压孔的圆");
                m_ZKHwindows.Drawing=true; 
                HOperatorSet.DrawCircle(m_ZKHwindows.hWindowControl.HalconWindow,out row , out col ,out Ridus);

                m_ZKHwindows.Drawing = false;
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow,"X:" + row.D.ToString(), "window", 12, 12, "black",
                  "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Y:"+ col.D.ToString(), "window", 32, 12, "black",
                    "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow,"半径："+ Ridus.D.ToString(), "window",
                    52, 12, "black", "true");
                HOperatorSet.GenCircle(out circle, row, col, Ridus);
                HOperatorSet.AreaCenter(circle, out area, out t, out t2);
                HOperatorSet.DispObj(circle , m_ZKHwindows.hWindowControl.HalconWindow );
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "面积：" + area.D.ToString(), "window",
              72, 12, "black", "true");
                textBox2.Text = row.D.ToString("F0");
                textBox3.Text = col.D.ToString("F0");
            }
            else
            {
                MessageBox.Show("请先载入图片！");
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

        private void button1_Click(object sender, EventArgs e)
        {


            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "eccentricity", textBox1.Text);
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "row", textBox2.Text);
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "col", textBox3.Text);
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmin", textBox4.Text);
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmax", textBox5.Text);

            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Mult", Mult.ToString());
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Add", add.ToString());
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "MinThreshold", trackBar8.Value.ToString());
            IniAPI.INIWriteValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "MaxThreshold", trackBar7.Value.ToString());


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = IniAPI.INIGetStringValue(Param_Path, "ImageModel"+ comboBox1.SelectedIndex.ToString(), "eccentricity", "75");
            textBox2.Text = IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "row", "75");
            textBox3.Text = IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "col", "75");
            textBox4.Text = IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmin", "75");
            textBox5.Text = IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmax", "75");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int Dis_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "eccentricity", "75"));
            int lim_Row = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "row", "75"));
            int lim_Column = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "col", "75"));
            int Min_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmin", "75"));
            int Max_R = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Ridusmax", "75"));
            double Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Mult", "75"));
            double Add = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "Add", "75"));
            double MinThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "MinThreshold", "75"));
            double MaxThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "ImageModel" + comboBox1.SelectedIndex.ToString(), "MaxThreshold", "75"));
            
               
                    HObject Circle = new HObject();
                    HTuple row = new HTuple();
                    HTuple cloumn = new HTuple();
                    HTuple Radius = new HTuple();
                    HTuple Distance = new HTuple();

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
                    //


                    Circle.Dispose();
                    bool result = m_imageHandle0.Judgment6524(img, out Circle, out row, out cloumn, out Radius, out Distance);
                    if (row.Length!=0)
                    {
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "X:" + row.D.ToString(), "window", 12, 12, "black",
  "true");
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Y:" + cloumn.D.ToString(), "window", 32, 12, "black",
                            "true");
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "半径：" + Radius.D.ToString(), "window",
                            52, 12, "black", "true");
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "偏心距：" + Distance.D.ToString(), "window",
                     72, 12, "black", "true");
                        if (result)
                        {
                            HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        }
                        else
                        {
                            HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "red");
                        }
                        HOperatorSet.DispObj(Circle, m_ZKHwindows.hWindowControl.HalconWindow);
                    }
                  //  return result;
            

            }
            catch (Exception)
            {

            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar1.Value+10 >= trackBar2.Value)
                {
                    trackBar2.Value = trackBar1.Value+10;
                }
                HObject outimg = new HObject();


                Mult = Math.Round(255.0/(trackBar2.Value - trackBar1.Value),3);
                add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                m_ZKHwindows.NowImage = outimg;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar2.Value-10 <= trackBar1.Value)
                {
                    trackBar1.Value = trackBar2.Value-10;
                }
                HObject outimg = new HObject();


                Mult = Math.Round(255.0 / (trackBar2.Value - trackBar1.Value), 3);
                add = (Mult * trackBar1.Value) * -1;
                HOperatorSet.ScaleImage(sacleimg, out outimg, Mult, add);
                m_ZKHwindows.NowImage = outimg;
            }
           
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            //  HObject outimg = new HObject();
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar8.Value >= trackBar7.Value)
                {
                    trackBar7.Value = trackBar8.Value ;
                }
                m_ZKHwindows.hWindowControl.Refresh();
                HObject outreg = new HObject();
                outreg.Dispose();
                HOperatorSet.Threshold(m_ZKHwindows.NowImage, out outreg, trackBar8.Value, trackBar7.Value);
                HOperatorSet.DispObj(outreg, m_ZKHwindows.hWindowControl.HalconWindow);
            }
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar7.Value <= trackBar8.Value)
                {
                    trackBar8.Value = trackBar7.Value ;
                }
                HObject outreg = new HObject();
                m_ZKHwindows.hWindowControl.Refresh();
                outreg.Dispose();
                HOperatorSet.Threshold(m_ZKHwindows.NowImage, out outreg, trackBar8.Value, trackBar7.Value);
                HOperatorSet.DispObj(outreg, m_ZKHwindows.hWindowControl.HalconWindow);
            }
        }
    }
}
