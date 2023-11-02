using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;

using System.Xml.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace _6524
{

    [Serializable]
    public class Shape_matching 
    {
        #region Input   



        /// <summary>
        /// 设置输入特性的变量
        /// </summary>
        /// 

        private int hv_Matching_num = 20; //输入匹配参数，最大匹配个数
        private double hv_Matching_rote_min = 0; //输入匹配参数，最小旋转角度
        private double hv_Matching_rote_max = 360;  //输入匹配参数，最大旋转角度
        private double hv_Matching_rote_step = 0.01; //输入匹配参数，角度步长
        private double hv_Matching_scale_min = 0.9; //输入匹配参数，最小缩放
        private double hv_Matching_scale_max = 1.1; //输入匹配参数，最大缩放
        private double hv_Matching_scale_step = 0.02; //输入匹配参数，缩放步长
        private bool hv_Matching_runtime_Max_enabled = false; //输入匹配参数，是否启用搜索时间限制
        private int hv_Matching_runtime_Max = 100; //输入匹配参数，最大搜索时间
        private double hv_Matching_min_Score = 0.8; //输入匹配参数，匹配最小分数
        private int hv_Matching_min_Contrastratio = 15; //输入匹配参数，最小对比度
        private int hv_Matching_Pyramid_level = 0; //输入匹配参数，金字塔级别 非常重要，太低速度慢很多
        private double hv_Matching_Greedy_algorithm = 1; //输入匹配参数，贪心算法
        private double hv_Matching_overlap_Max = 0.4; //输入匹配参数，最大重叠
        private string hv_Serialize_ModelID; //输入匹配参数，序列化模型
        private string hv_Serialize_HomMat2D; //输入匹配参数，序列化变换二维矩阵
        private string hv_Serialize_ModelImage; //输入匹配参数，序列化模型图片
        private int inputimagetask = 0;
        private int inputimageID = 0;
        #endregion


        #region Output

        /// <summary>
        /// 设置输出特性的变量
        /// </summary>
        private HTuple Hv_Row = new HTuple(); //输出匹配结果坐标
        private HTuple Hv_Column = new HTuple(); //输出匹配结果坐标
        private HTuple Hv_Angle = new HTuple(); //输出匹配结果角度
        private HTuple Hv_Scale = new HTuple(); //输出匹配结果缩放值
        private HTuple Hv_Score = new HTuple(); //输出匹配结果分数



        #endregion


        /// <summary>
        /// 静态内部参数,与模型绑定    
        /// </summary>



        public string jsonString;
        public HObject img, ho_TemplateImage, ho_ModelContours, ho_Transregion;
        public HObject ho_TransContours;//模板图轮廓
        public HObject ho_Transregion_final;//结果轮廓
        public HObject ho_model_image;//模型的图片
        // Local control variables 


        public HTuple hv_ModelID = new HTuple(), hv_Row1 = new HTuple();
        public HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
        public HTuple hv_Column2 = new HTuple(), hv_RefRow = new HTuple();
        public HTuple hv_RefColumn = new HTuple(), hv_HomMat2D = new HTuple();
        public HTuple hv_I = new HTuple(), hv_AlignmentHomMat2D = new HTuple();
      



        #region 属性

      //public int Hv_Matching_num { get => hv_Matching_num; set => hv_Matching_num = value; }
      //public double Hv_Matching_rote_min { get => hv_Matching_rote_min; set => hv_Matching_rote_min = value; OnpropertyChanged();}
      //public double Hv_Matching_rote_max { get => hv_Matching_rote_max; set => hv_Matching_rote_max = value; }
      //public double Hv_Matching_rote_step { get => hv_Matching_rote_step; set => hv_Matching_rote_step = value; }
      //public double Hv_Matching_scale_min { get => hv_Matching_scale_min; set => hv_Matching_scale_min = value; }
      //public double Hv_Matching_scale_max { get => hv_Matching_scale_max; set => hv_Matching_scale_max = value; }
      //public double Hv_Matching_scale_step { get => hv_Matching_scale_step; set => hv_Matching_scale_step = value; }
      //public bool Hv_Matching_runtime_Max_enabled { get => hv_Matching_runtime_Max_enabled; set => hv_Matching_runtime_Max_enabled = value; }
      //public int Hv_Matching_runtime_Max { get => hv_Matching_runtime_Max; set => hv_Matching_runtime_Max = value; }
      //public double Hv_Matching_min_Score { get => hv_Matching_min_Score; set => hv_Matching_min_Score = value; }
      //public int Hv_Matching_min_Contrastratio { get => hv_Matching_min_Contrastratio; set => hv_Matching_min_Contrastratio = value; }
      //public int Hv_Matching_Pyramid_level { get => hv_Matching_Pyramid_level; set => hv_Matching_Pyramid_level = value; }
      //public int Hv_Matching_Greedy_algorithm { get => hv_Matching_Greedy_algorithm; set => hv_Matching_Greedy_algorithm = value; }
      //public double Hv_Matching_overlap_Max { get => hv_Matching_overlap_Max; set => hv_Matching_overlap_Max = value; }
      //public string Hv_Serialize_ModelID { get => hv_Serialize_ModelID; set => hv_Serialize_ModelID = value; }
      //public string Hv_Serialize_HomMat2D { get => hv_Serialize_HomMat2D; set => hv_Serialize_HomMat2D = value; }
      //public string Hv_Serialize_ModelImage { get => hv_Serialize_ModelImage; set => hv_Serialize_ModelImage = value; }

        public double Hv_Matching_rote_min     
        {
            get => hv_Matching_rote_min;
            set
            {
                hv_Matching_rote_min = value;
            }
        }

  
     public double Hv_Matching_rote_max { get => hv_Matching_rote_max; set { hv_Matching_rote_max = value; } }
       public double Hv_Matching_rote_step { get => hv_Matching_rote_step; set { hv_Matching_rote_step = value; } }
      public double Hv_Matching_scale_min { get => hv_Matching_scale_min; set { hv_Matching_scale_min = value; } }
       public double Hv_Matching_scale_max { get => hv_Matching_scale_max; set { hv_Matching_scale_max = value; } }

       public double Hv_Matching_scale_step { get => hv_Matching_scale_step; set { hv_Matching_scale_step = value; } }

  public bool Hv_Matching_runtime_Max_enabled { get => hv_Matching_runtime_Max_enabled; set { hv_Matching_runtime_Max_enabled = value; } }

    public int Hv_Matching_runtime_Max { get => hv_Matching_runtime_Max; set { hv_Matching_runtime_Max = value; } }

      public double Hv_Matching_min_Score { get => hv_Matching_min_Score; set { hv_Matching_min_Score = value; } }

       public int Hv_Matching_min_Contrastratio { get => hv_Matching_min_Contrastratio; set { hv_Matching_min_Contrastratio = value; } }

       public int Hv_Matching_Pyramid_level { get => hv_Matching_Pyramid_level; set { hv_Matching_Pyramid_level = value; } }

       public double Hv_Matching_Greedy_algorithm { get => hv_Matching_Greedy_algorithm; set { hv_Matching_Greedy_algorithm = value; } }
 public string Hv_Serialize_ModelID { get => hv_Serialize_ModelID; set { hv_Serialize_ModelID = value; } }

 public string Hv_Serialize_HomMat2D { get => hv_Serialize_HomMat2D; set { hv_Serialize_HomMat2D = value; } }
public string Hv_Serialize_ModelImage { get => hv_Serialize_ModelImage; set { hv_Serialize_ModelImage = value; } }

    public int Hv_Matching_num { get => hv_Matching_num; set { hv_Matching_num = value; } }

        public double Hv_Matching_overlap_Max { get => hv_Matching_overlap_Max; set { hv_Matching_overlap_Max = value; } }

        public HTuple Row
        {
            get
            {
                return Hv_Row;
            }

            set
            {
                Hv_Row = value;
               
            }
        }

        public HTuple Column
        {
            get
            {
                return Hv_Column;
            }

            set
            {
                Hv_Column = value;
               
            }
        }

      
        public HTuple Angle
        {
            get
            {
                return Hv_Angle;
            }

            set
            {
                Hv_Angle = value;
               
            }
        }

        
        public HTuple Scale
        {
            get
            {
                return Hv_Scale;
            }

            set
            {
                Hv_Scale = value;
               
            }
        }

      
        public HTuple Score
        {
            get
            {
                return Hv_Score;
            }

            set
            {
                Hv_Score = value;
               
            }
        }

      
        public int Inputimagetask
        {
            get
            {
                return inputimagetask;
            }

            set
            {
                inputimagetask = value;
            }
        }

     
        public int InputimageID
        {
            get
            {
                return inputimageID;
            }

            set
            {
                inputimageID = value;
            }
        }

        #endregion



    

  
      









        public Shape_matching()
        {

            HOperatorSet.GenEmptyObj(out ho_TemplateImage);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Transregion_final);
            HOperatorSet.GenEmptyObj(out ho_Transregion);
            HOperatorSet.GenEmptyObj(out img);
            HOperatorSet.GenEmptyObj(out ho_model_image);



        }

        public bool LoadingModel(string path)
        {
            try
            {
                hv_ModelID.Dispose();
                HOperatorSet.ReadShapeModel(path, out hv_ModelID);
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
          
        }

        public bool SaveModel(string path)
        {
            try
            { 

                HOperatorSet.WriteShapeModel(hv_ModelID, path); 
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        /// <summary>
        ///初始化使用
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// 创建模型
        /// </summary>
        public bool createmodel(HObject ho_Image, HObject ho_Reg, out HObject REC_Object)
        {
            try
            {
                ho_TemplateImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Reg, out ho_TemplateImage);
                //
                //Matching 01: Create the shape model
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ModelID.Dispose();
                    HOperatorSet.CreateShapeModel(ho_TemplateImage, 7, (new HTuple(0)).TupleRad()
        , (new HTuple(360)).TupleRad(), (new HTuple(0.5193)).TupleRad(), (new HTuple("none")).TupleConcat(
        "no_pregeneration"), "use_polarity", ((new HTuple(12)).TupleConcat(23)).TupleConcat(
        4), 4, out hv_ModelID);
                }
                //
                //Matching 01: Get the model contour for transforming it later into the image
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                if (Hv_Matching_runtime_Max_enabled)
                {
                    HOperatorSet.SetShapeModelParam(hv_ModelID, "timeout", Hv_Matching_runtime_Max);
                }

                //
                //Matching 01: Set a reference position to show the model
                hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                HOperatorSet.SmallestRectangle1Xld(ho_ModelContours, out hv_Row1, out hv_Column1,
                    out hv_Row2, out hv_Column2);
                hv_RefRow.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RefRow = ((hv_Row2.TupleMax()
                        ) - (hv_Row1.TupleMin())) / 2;
                }
                hv_RefColumn.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RefColumn = ((hv_Column2.TupleMax()
                        ) - (hv_Column1.TupleMin())) / 2;
                }
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
                ho_TransContours.Dispose();
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);


                Hv_Row.Dispose(); Hv_Column.Dispose(); Hv_Angle.Dispose(); Hv_Scale.Dispose(); Hv_Score.Dispose();
                HOperatorSet.FindScaledShapeModel(ho_TemplateImage, hv_ModelID, (0 * Math.PI) / 180, (3 * Math.PI) / 180, Hv_Matching_scale_min, Hv_Matching_scale_max,
                    Hv_Matching_min_Score, Hv_Matching_num, Hv_Matching_overlap_Max, (new HTuple("least_squares")).TupleConcat(
                    "max_deformation 2"), Hv_Matching_Pyramid_level, Hv_Matching_Greedy_algorithm,
                    out Hv_Row, out Hv_Column, out Hv_Angle, out Hv_Scale, out Hv_Score);





                //Matching 01: Transform the model contours into the detected positions
                //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                hv_HomMat2D.Dispose();
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, Hv_Scale, Hv_Scale, 0, 0, out ExpTmpOutVar_0);
                    hv_HomMat2D.Dispose();
                    hv_HomMat2D = ExpTmpOutVar_0;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, Hv_Angle, 0, 0,
                        out ExpTmpOutVar_0);
                    hv_HomMat2D.Dispose();
                    hv_HomMat2D = ExpTmpOutVar_0;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Hv_Row, Hv_Column, out ExpTmpOutVar_0);
                    hv_HomMat2D.Dispose();
                    hv_HomMat2D = ExpTmpOutVar_0;
                }
                ho_TransContours.Dispose();
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
                    hv_HomMat2D);

















                HTuple Row1 = new HTuple();
                HTuple Row2 = new HTuple();
                HTuple Column1 = new HTuple();
                HTuple Column2 = new HTuple();
                HObject Region = new HObject();
                HObject UNIRegion = new HObject();
                Region.Dispose();
                HOperatorSet.GenRegionContourXld(ho_TransContours, out Region, "filled");
                UNIRegion.Dispose();
                HOperatorSet.Union1(Region, out UNIRegion);
                Row1.Dispose();
                Column1.Dispose();
                Row2.Dispose();
                Column2.Dispose();







                HOperatorSet.SmallestRectangle1(UNIRegion, out Row1, out Column1, out Row2, out Column2);
                HOperatorSet.CropRectangle1(ho_TemplateImage, out REC_Object, Row1, Column1, Row2, Column2);
                //ho_model_image.Dispose();
              //  HOperatorSet.CopyImage(REC_Object, out ho_model_image);
              //  HOperatorSet.WriteImage(ho_model_image, "bmp", 0, "C:\\Users\\Administrator\\Desktop\\7.bmp");

              //  //hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Scale.Dispose(); hv_Score.Dispose();
              //  //HOperatorSet.FindScaledShapeModel(REC_Object, hv_ModelID,0, 6, 0.94, 1.1,
              //  //    0.4, 1, 0.4, (new HTuple("least_squares")).TupleConcat("max_deformation 2"), 3, 4,
              //  //    out hv_Row, out hv_Column, out hv_Angle, out hv_Scale, out hv_Score);

              //  Hv_Row.Dispose(); Hv_Column.Dispose(); Hv_Angle.Dispose(); Hv_Scale.Dispose(); Hv_Score.Dispose();
              //  HOperatorSet.FindScaledShapeModel(REC_Object, hv_ModelID, (Hv_Matching_rote_min * Math.PI) / 180, (Hv_Matching_rote_max * Math.PI) / 180, Hv_Matching_scale_min, Hv_Matching_scale_max,
              //Hv_Matching_min_Score, Hv_Matching_num, Hv_Matching_overlap_Max, (new HTuple("least_squares")).TupleConcat(
              //"max_deformation 2"), Hv_Matching_Pyramid_level, Hv_Matching_Greedy_algorithm,
              //out Hv_Row, out Hv_Column, out Hv_Angle, out Hv_Scale, out Hv_Score);
              //  //Matching 01: Transform the model contours into the detected positions
              //  //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

              //  hv_HomMat2D.Dispose();
              //  HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
              //  using (HDevDisposeHelper dh = new HDevDisposeHelper())
              //  {
              //      HTuple ExpTmpOutVar_0;
              //      HOperatorSet.HomMat2dScale(hv_HomMat2D, Hv_Scale, Hv_Scale, 0, 0, out ExpTmpOutVar_0);
              //      hv_HomMat2D.Dispose();
              //      hv_HomMat2D = ExpTmpOutVar_0;
              //  }
              //  using (HDevDisposeHelper dh = new HDevDisposeHelper())
              //  {
              //      HTuple ExpTmpOutVar_0;
              //      HOperatorSet.HomMat2dRotate(hv_HomMat2D, Hv_Angle, 0, 0,
              //          out ExpTmpOutVar_0);
              //      hv_HomMat2D.Dispose();
              //      hv_HomMat2D = ExpTmpOutVar_0;
              //  }
              //  using (HDevDisposeHelper dh = new HDevDisposeHelper())
              //  {
              //      HTuple ExpTmpOutVar_0;
              //      HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Hv_Row, Hv_Column, out ExpTmpOutVar_0);
              //      hv_HomMat2D.Dispose();
              //      hv_HomMat2D = ExpTmpOutVar_0;
              //  }
              //  ho_TransContours.Dispose();
              //  HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
              //      hv_HomMat2D);



                return true;
            }
            catch (Exception)
            {
                REC_Object = null;
                return false;
            }


            //
        }



        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public  bool action(int TASKID, int OperateID)
        {
            try
            {
                ho_Transregion_final.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Transregion_final);

             
                if (Hv_Matching_runtime_Max_enabled)
                {
                    HOperatorSet.SetShapeModelParam(hv_ModelID, "timeout", Hv_Matching_runtime_Max);
                }

                //Matching 01: Find the model
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    Hv_Row.Dispose(); Hv_Column.Dispose(); Hv_Angle.Dispose(); Hv_Scale.Dispose(); Hv_Score.Dispose();
                    HOperatorSet.FindScaledShapeModel(img, hv_ModelID, (Hv_Matching_rote_min * Math.PI) / 180, (Hv_Matching_rote_max * Math.PI) / 180, Hv_Matching_scale_min, Hv_Matching_scale_max,
                        Hv_Matching_min_Score, Hv_Matching_num, Hv_Matching_overlap_Max, (new HTuple("least_squares")).TupleConcat(
                        "max_deformation 2"), Hv_Matching_Pyramid_level, Hv_Matching_Greedy_algorithm,
                        out Hv_Row, out Hv_Column, out Hv_Angle, out Hv_Scale, out Hv_Score);

                }


               // ho_Transregion_final.Dispose();


                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_HomMat2D.Dispose();
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dScale(hv_HomMat2D, Hv_Scale.TupleSelect(hv_I), Hv_Scale.TupleSelect(
                            hv_I), 0, 0, out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dRotate(hv_HomMat2D, Hv_Angle.TupleSelect(hv_I), 0, 0,
                            out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Hv_Row.TupleSelect(hv_I), Hv_Column.TupleSelect(
                            hv_I), out ExpTmpOutVar_0);
                        hv_HomMat2D.Dispose();
                        hv_HomMat2D = ExpTmpOutVar_0;
                    }
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
                        hv_HomMat2D);
                    ho_Transregion.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Transregion, "filled");


                    HOperatorSet.Union2(ho_Transregion_final, ho_Transregion, out ho_Transregion_final);

                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }


        /// <summary>
        /// 释放
        /// </summary>
        public void dispose()
        {

            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            img.Dispose();

            Hv_Row.Dispose();
            Hv_Column.Dispose();
            Hv_Angle.Dispose();
            Hv_Scale.Dispose();
            Hv_Score.Dispose();
            // hv_Matching_num.Dispose();
            //hv_Matching_rote_min.Dispose();
            //hv_Matching_rote_max.Dispose();
            //hv_Matching_rote_step.Dispose();
            //hv_Matching_scale_min.Dispose();
            //hv_Matching_scale_max.Dispose();
            //hv_Matching_scale_step.Dispose();
            //hv_Matching_runtime_Max_enabled.Dispose();
            //hv_Matching_runtime_Max.Dispose();
            //hv_Matching_min_Score.Dispose();
            //hv_Matching_min_Contrastratio.Dispose();
            //hv_Matching_Pyramid_level.Dispose();
            //hv_Matching_Greedy_algorithm.Dispose();
            //hv_Matching_overlap_Max.Dispose();
            hv_ModelID.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_RefRow.Dispose();
            hv_RefColumn.Dispose();
            hv_HomMat2D.Dispose();
            hv_I.Dispose();
            hv_AlignmentHomMat2D.Dispose();
        }
    }
}
