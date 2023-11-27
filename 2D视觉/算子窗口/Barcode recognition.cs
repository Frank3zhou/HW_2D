using _6524.Class;
using _6524.算子窗口;
using HalconDotNet;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace _6524
{
    public partial class Barcode_recognition : Form
    {
        HTuple Barcode_type = new HTuple();

        double Mult;
        double add;


        ZKHwindows m_ZKHwindows;
        HObject img;
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址
        HObject sacleimg = new HObject();

        public Barcode_recognition()
        {
            InitializeComponent();
        }

        private void Barcode_recognition_Load(object sender, EventArgs e)
        {
            m_ZKHwindows = new ZKHwindows();
            HOperatorSet.GenEmptyObj(out img);
            m_ZKHwindows.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_ZKHwindows);
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "fill");

        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
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
              HOperatorSet.DrawRectangle2(m_ZKHwindows.hWindowControl.HalconWindow, out row, out col, out PI,out Length1,out Length2);

                m_ZKHwindows.Drawing = false;
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "起始X:" + row.D.ToString(), "window", 12, 12, "black",
                  "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "起始Y:" + col.D.ToString(), "window", 32, 12, "black",
                    "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Length1：" + Length1.D.ToString(), "window",
                    52, 12, "black", "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "Length2：" + Length2.D.ToString(), "window",
              72, 12, "black", "true");
                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "角度：" + PI.D.ToString(), "window",
         92, 12, "black", "true");
                HOperatorSet.GenRectangle2(out rectange, row, col, PI ,Length1,Length2);
           
                HOperatorSet.DispObj(rectange, m_ZKHwindows.hWindowControl.HalconWindow);
               
                textBox1.Text = row.D.ToString("F0");
                textBox2.Text = col.D.ToString("F0");
                textBox4.Text = Length1.D.ToString("F0");
                textBox5.Text = Length2.D.ToString("F0");
                textBox3.Text = PI.D.ToString();
              
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {




                if (comboBox2.Text == "二维码")
                {

                }
                else if (comboBox2.Text == "条形码")
                {

                }
                else if (comboBox2.Text == "OCR")
                {

                }
                else
                {
                    MessageBox.Show("请选择正确的识别内容类型");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("请先选择识别内容类型");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                double Mult = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Mult", "75"));
                double Add = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Add", "75"));
                double MinThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MinThreshold", "75"));
                double MaxThreshold = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MaxThreshold", "75"));
                double RectangleStartX = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "RectangleStartX", "0"));


                double RectangleStartY = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "RectangleStartY", "0"));
                double Length1 = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Length1", "0"));
                double Length2 = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Length2", "0"));
                double PI = Convert.ToDouble(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "PI", "0"));
                string Recognition = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Recognition", "0");
                 string   type= IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Type", "0");
                bool Use_Result = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Use_Result", "false"));
              //  checkBox1.Checked = Use_Result;
                string Result = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Result", "0");
              //  textBox6.Text = Result;

                if (Recognition == "二维码")
                {
                    HObject rectange = new HObject();
                    HObject XLDS = new HObject();
                    QR_Code__recognition qR_Code__Recognition = new QR_Code__recognition();
                    HTuple result;
                    qR_Code__Recognition.Find2Code(img, out rectange, out XLDS, RectangleStartX, RectangleStartY, PI, Length1, Length2, type, out result);
                    if (result != null)
                    {
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "二维码内容:" + result.ToString(), "window", 12, 12, "black", "true");


                        HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "margin");
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "blue");
                        HOperatorSet.DispObj(rectange, m_ZKHwindows.hWindowControl.HalconWindow);
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(XLDS, m_ZKHwindows.hWindowControl.HalconWindow);
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
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "条行码内容:" + result.ToString(), "window", 12, 12, "black", "true");


                        HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "margin");
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "blue");

                        HOperatorSet.DispObj(rectange, m_ZKHwindows.hWindowControl.HalconWindow);
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(rectangexld, m_ZKHwindows.hWindowControl.HalconWindow);
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
                        disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "OCR内容:" + result.ToString(), "window", 12, 12, "black", "true");
                        if (Use_Result)
                        {
                            if (result == Result)
                            {
                                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "结果对比:" + "OK", "window", 32, 12, "black", "true");
                            }
                            else
                            {
                                disp_message(m_ZKHwindows.hWindowControl.HalconWindow, "结果对比:" + "NG", "window", 32, 12, "black", "true");
                            }
                        }
                       

                        HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "margin");
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "blue");

                        HOperatorSet.DispObj(rectange, m_ZKHwindows.hWindowControl.HalconWindow);
                        HOperatorSet.SetColor(m_ZKHwindows.hWindowControl.HalconWindow, "green");
                        HOperatorSet.DispObj(rectangexld, m_ZKHwindows.hWindowControl.HalconWindow);
                    }

                }
                else
                {
                    MessageBox.Show("请选择正确的识别内容类型");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("请先选择识别内容类型");
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
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Add", add.ToString());

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
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Mult", Mult.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Add", add.ToString());

            }
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            //  HObject outimg = new HObject();
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "fill");
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar8.Value >= trackBar7.Value)
                {
                    trackBar7.Value = trackBar8.Value;
                }
                m_ZKHwindows.hWindowControl.Refresh();
                HObject outreg = new HObject();
                outreg.Dispose();
                HOperatorSet.Threshold(m_ZKHwindows.NowImage, out outreg, trackBar8.Value, trackBar7.Value);
                HOperatorSet.DispObj(outreg, m_ZKHwindows.hWindowControl.HalconWindow);

                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MinThreshold", trackBar8.Value.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MaxThreshold", trackBar7.Value.ToString());
            }
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            HOperatorSet.SetDraw(m_ZKHwindows.hWindowControl.HalconWindow, "fill");
            if (m_ZKHwindows.NowImage != null)
            {
                if (trackBar7.Value <= trackBar8.Value)
                {
                    trackBar8.Value = trackBar7.Value;
                }
                HObject outreg = new HObject();
                m_ZKHwindows.hWindowControl.Refresh();
                outreg.Dispose();
                HOperatorSet.Threshold(m_ZKHwindows.NowImage, out outreg, trackBar8.Value, trackBar7.Value);
                HOperatorSet.DispObj(outreg, m_ZKHwindows.hWindowControl.HalconWindow);

                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MinThreshold", trackBar8.Value.ToString());
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "MaxThreshold", trackBar7.Value.ToString());
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {


           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (comboBox2.Text == "二维码")
            {
                comboBox3.Enabled = true;
              //  button4.Enabled = false;
                comboBox3.Items.Clear();    
                comboBox3.Items.Add( "Data Matrix ECC 200" );
                comboBox3.Items.Add("QR Code");
                comboBox3.Items.Add("Micro QR Code");
            }
            else if (comboBox2.Text == "条形码")
            {
                comboBox3.Enabled = true;
              //  button4.Enabled = false;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Code 128");
                comboBox3.Items.Add("Code 39");
                comboBox3.Items.Add("auto");
            }
            else if (comboBox2.Text == "OCR")
            {
                comboBox3.Items.Clear();
              //  comboBox3.Enabled= false;
                comboBox3.Items.Add("Pharma_Rej.omc");
                comboBox3.Items.Add("Document_Rej.omc");
                comboBox3.Items.Add("DotPrint_Rej.omc");

                //  button4.Enabled = true;

            }
            else
            {
                MessageBox.Show("请选择正确的识别内容类型");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "RectangleStartX" , textBox1.Text);
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "RectangleStartY" , textBox2.Text);
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "PI", textBox3.Text);
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Length1" , textBox4.Text);
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Length2" , textBox5.Text);
            
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Recognition", comboBox2.Text);
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Type", comboBox3.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            { 
                   panel3.Enabled= true;
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Use_Result", checkBox1.Checked.ToString());
            }
            else
            {
                panel3.Enabled = false;
                IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Use_Result", checkBox1.Checked.ToString());
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Result", textBox6.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Use_Result =Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Use_Result", "false")) ;
            checkBox1.Checked = Use_Result;
            string Result = IniAPI.INIGetStringValue(Param_Path, "Run_number" + comboBox1.SelectedIndex.ToString(), "Result", "0");
            textBox6.Text = Result;
        }
    }
}
