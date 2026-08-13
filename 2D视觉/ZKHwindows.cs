using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6524
{
    //   public delegate void GetXY(HTuple X, HTuple Y, HTuple R, HTuple G, HTuple B);
    public partial class ZKHwindows : UserControl
    {
        //  public  event GetXY getxy;
        public HWindowControl hWindowControl;
        public bool Drawing = false;
        private HObject _nowImage;
        public bool NowImageDrawing = false;
        public HTuple channels = 1;


        public static bool mousemove = false;
        public ZKHwindows()
        {

            InitializeComponent();
            hWindowControl = new HWindowControl();
            hWindowControl.Dock = DockStyle.Fill;

            hWindowControl.HMouseDown += hWindowControl1_HMouseDown;
            hWindowControl.HMouseMove += hWindowControl1_HMouseMove;
            hWindowControl.HMouseWheel += hWindowControl1_HMouseWheel;
            hWindowControl.HMouseUp += hWindowControl1_HMouseUp;



            HOperatorSet.SetDraw(hWindowControl.HalconWindow, "margin");
            HOperatorSet.SetColor(hWindowControl.HalconWindow, "red");
            HOperatorSet.SetLineWidth(hWindowControl.HalconWindow, 3);
            this.Controls.Add(hWindowControl);
        }
        public HObject NowImage
        {
            get
            {
                return _nowImage;
            }
            set
            {
                _nowImage = value;
                ImageChanged();
            }

        }
        //public  void AutoSize(HWindowControl hWindowControl)

        //{
        //    hWindowControl.HMouseDown += hWindowControl1_HMouseDown;
        //    hWindowControl.HMouseMove += hWindowControl1_HMouseMove;
        //    hWindowControl.HMouseWheel += hWindowControl1_HMouseWheel;
        //    hWindowControl.HMouseUp += hWindowControl1_HMouseUp;



        //    HOperatorSet.SetDraw(hWindowControl.HalconWindow, "margin");
        //    HOperatorSet.SetColor(hWindowControl.HalconWindow, "red");
        //    HOperatorSet.SetLineWidth(hWindowControl.HalconWindow, 3);
        //}



        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            try
            {
                if (NowImage != null)
                {
                    ImgZoom(NowImage, hWindowControl.HalconWindow, e.Delta);
                }
            }
            catch (Exception)
            {

               
            }
           

        }
        //图片缩小放大，配合鼠标滚轮事件
        public void ImgZoom(HObject L_Img, HTuple Hwindow, int Delta)
        {
            try
            {
                HTuple Zoom, Row, Col, L_Button;
                HTuple hv_Width, hv_Height;
                HTuple Row0, Column0, Row00, Column00, Ht, Wt;
                HTuple[] Now_Pos = new HTuple[4];
                if (Delta > 0)//鼠标滚动格值，一般120
                {
                    Zoom = 1.2;//向上滚动,放大倍数
                }
                else
                {
                    Zoom = 0.8;//向下滚动,缩小倍数
                }
                HOperatorSet.GetMposition(Hwindow, out Row, out Col, out L_Button);//获取当前鼠标的位置
                HOperatorSet.GetPart(Hwindow, out Row0, out Column0, out Row00, out Column00);//获取当前窗体的大小规格
                HOperatorSet.GetImageSize(L_Img, out hv_Width, out hv_Height);//获取图片大小规格
                Ht = Row00 - Row0;
                Wt = Column00 - Column0;
                if (Ht * Wt < 32000 * 32000 || Zoom == 1.2)
                {
                    Now_Pos[0] = (Row0 + ((1 - (1.0 / Zoom)) * (Row - Row0)));
                    Now_Pos[1] = (Column0 + ((1 - (1.0 / Zoom)) * (Col - Column0)));
                    Now_Pos[2] = Now_Pos[0] + (Ht / Zoom);
                    Now_Pos[3] = Now_Pos[1] + (Wt / Zoom);
                    HOperatorSet.SetPart(Hwindow, Now_Pos[0], Now_Pos[1], Now_Pos[2], Now_Pos[3]);
                    // 在屏幕上同时显示图像和区域
                    HOperatorSet.ClearWindow(Hwindow);
                    HOperatorSet.DispObj(L_Img, Hwindow);

                    //HOperatorSet.DispObj(L_Reg, Hwindow);



                    // 更新窗口
                    //HOperatorSet.FlushBuffer(Hwindow);

                }
                else
                {
                    ImgIsNotStretchDisplay(L_Img, Hwindow);//不拉伸显示
                }
            }
            catch (Exception)
            {

              
            }
          

        }

        //图片不拉伸显示
        public void ImgIsNotStretchDisplay(HObject L_Img, HTuple Hwindow)
        {
            try
            {
                HTuple hv_Width, hv_Height;
                HTuple win_Width, win_Height, win_Col, win_Row, cwin_Width, cwin_Height;
                HOperatorSet.ClearWindow(Hwindow);
                HOperatorSet.GetImageSize(L_Img, out hv_Width, out hv_Height);//获取图片大小规格
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

                HOperatorSet.DispObj(L_Img, Hwindow);
            }
            catch (Exception)
            {

               
            }
          
            //HOperatorSet.FlushBuffer(Hwindow);
        }

        //图片拉伸显示
        public void ImgIsStretchDisplay(HObject L_Img, HTuple Hwindow)
        {
            HTuple hv_Width, hv_Height;
            HOperatorSet.ClearWindow(Hwindow);//清空画面
            HOperatorSet.GetImageSize(L_Img, out hv_Width, out hv_Height);//获取图片大小规格
            HOperatorSet.SetPart(Hwindow, 0, 0, hv_Height - 1, hv_Width - 1);//设置窗体的规格
            HOperatorSet.DispObj(L_Img, Hwindow);

        }

        HTuple oldRow, oldColumn;






        //鼠标按下去拖着图像移动，配合鼠标坐标按下与移动事件
        public void MouseDownMoveImg(HObject L_Img, HTuple Hwindow)
        {

            try
            {
                HTuple row1, col1, row2, col2, Row, Column, Button;
                HOperatorSet.GetMposition(Hwindow, out Row, out Column, out Button);
                double RowMove = Row - oldRow;
                double ColMove = Column - oldColumn;
                HOperatorSet.GetPart(Hwindow, out row1, out col1, out row2, out col2);//得到当前的窗口坐标
                HOperatorSet.SetPart(Hwindow, row1 - RowMove, col1 - ColMove, row2 - RowMove, col2 - ColMove);

                //防止刷新图片太快的时候闪烁
                HOperatorSet.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(Hwindow);
                HOperatorSet.SetSystem("flush_graphic", "true");
                //

                HOperatorSet.DispObj(L_Img, Hwindow);


                //HOperatorSet.FlushBuffer(Hwindow);
            }
            catch (Exception)
            {


            }



        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (!Drawing)
            {
                if (e.Button == MouseButtons.Left)
                {

                } //左键

                else if (e.Button == MouseButtons.Right)
                {

                    ImgIsNotStretchDisplay(NowImage, hWindowControl.HalconWindow);


                } //右键

                else
                {
                    if (NowImage != null)
                    {
                        mousemove = true;
                        SaveMouseDownPosition(hWindowControl.HalconWindow);

                    }
                } //滚轮

            }






        }

        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {

            if (!Drawing)
            {

                mousemove = false;
            }

        }

        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (!Drawing)
                {
                    if (mousemove)
                    {
                        MouseDownMoveImg(NowImage, hWindowControl.HalconWindow);
                    }
                    else
                    {
                        HTuple Row, Column, Button, Rgray, Ggray, Bgray;
                        HObject Rimg = new HObject();
                        HObject Gimg = new HObject();
                        HObject Bimg = new HObject();
                        Rimg.Dispose();
                        Gimg.Dispose();
                        Bimg.Dispose();
                        HOperatorSet.GetMposition(hWindowControl.HalconWindow, out Row, out Column, out Button);
                        if (channels.D == 3)
                        {
                            try
                            {
                                HOperatorSet.Decompose3(NowImage, out Rimg, out Gimg, out Bimg);
                                HOperatorSet.GetGrayval(Rimg, Row, Column, out Rgray);
                                HOperatorSet.GetGrayval(Gimg, Row, Column, out Ggray);
                                HOperatorSet.GetGrayval(Bimg, Row, Column, out Bgray);
                            }
                            catch (Exception)
                            {
                                Rgray = 0;
                                Ggray = 0;
                                Bgray = 0;

                            }


                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.GetGrayval(NowImage, Row, Column, out Rgray);
                                Ggray = Rgray;
                                Bgray = Rgray;
                            }
                            catch (Exception)
                            {
                                Rgray = 0;
                                Ggray = 0;
                                Bgray = 0;
                            }

                        }
                        getxy(Row, Column, Rgray, Ggray, Bgray);
                    }
                }
            }
            catch (Exception)
            {


            }



        }



        private void hWindowControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {


        }


        public void SaveMouseDownPosition(HTuple Hwindow)
        {
            try
            {
                HTuple Button;
                HOperatorSet.GetMposition(Hwindow, out oldRow, out oldColumn, out Button);
            }
            catch (Exception)
            {

            }


        }

        private void ImageChanged()
        {
            try
            {
                ImgIsNotStretchDisplay(NowImage, hWindowControl.HalconWindow);
                HOperatorSet.CountChannels(NowImage, out channels);
            }
            catch (Exception)
            {
            }
        }


        private void getxy(HTuple Row, HTuple Column, HTuple Rgray, HTuple Ggray, HTuple Bgray)
        {
            try
            {
                toolStripStatusLabel2.Text = Row.D.ToString();
                toolStripStatusLabel4.Text = Column.D.ToString();
                toolStripStatusLabel6.Text = Rgray.ToString();
                toolStripStatusLabel8.Text = Ggray.D.ToString();
                toolStripStatusLabel10.Text = Bgray.D.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }
    }

}
