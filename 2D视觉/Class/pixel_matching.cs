using HalconDotNet;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6524.Class
{
    public class pixel_matching
    {
        HTuple Hv_ModelID = new HTuple();
        public HObject img = new HObject();
        private HObject hv_contour = new HObject();
        private double hv_Matching_Radius = 0;
        HTuple hv_modelrow = new HTuple(), hv_modelcol = new HTuple();
        HTuple hv_I = new HTuple(), hv_AlignmentHomMat2D = new HTuple();
        HObject ho_RegionAffineTrans = new HObject();
        public HObject ho_Transregion_final = new HObject();
        private int hv_Matching_num = 20; //输入匹配参数，最大匹配个数
        private double hv_Matching_rote_min = 0; //输入匹配参数，最小旋转角度
        private double hv_Matching_rote_max = 360;  //输入匹配参数，最大旋转角度
        private double hv_Matching_rote_step = 0.01; //输入匹配参数，角度步长

        //     private double hv_Matching_scale_min = 0.9; //输入匹配参数，最小缩放
        //       private double hv_Matching_scale_max = 1.1; //输入匹配参数，最大缩放
        //       private double hv_Matching_scale_step = 0.02; //输入匹配参数，缩放步长
        //    private bool hv_Matching_runtime_Max_enabled = false; //输入匹配参数，是否启用搜索时间限制
        //  private int hv_Matching_runtime_Max = 100; //输入匹配参数，最大搜索时间
        private double hv_Matching_min_Score = 0.8; //输入匹配参数，匹配最小分数
                                                    // private int hv_Matching_min_Contrastratio = 15; //输入匹配参数，最小对比度
        private int hv_Matching_Pyramid_level = 0; //输入匹配参数，金字塔级别 非常重要，太低速度慢很多
                                                   // private double hv_Matching_Greedy_algorithm = 1; //输入匹配参数，贪心算法
        private double hv_Matching_overlap_Max = 0.4; //输入匹配参数，最大重叠

        public HTuple Hv_Row = new HTuple(); //输出匹配结果坐标
        public HTuple Hv_Column = new HTuple(); //输出匹配结果坐标
        public HTuple Hv_Angle = new HTuple(); //输出匹配结果角度
        public HTuple Hv_Scale = new HTuple(); //输出匹配结果缩放值
        public HTuple Hv_Score = new HTuple(); //输出匹配结果分数


        public int Hv_Matching_num { get => hv_Matching_num; set => hv_Matching_num = value; }
        public double Hv_Matching_rote_min { get => hv_Matching_rote_min; set => hv_Matching_rote_min = value; }
        public double Hv_Matching_rote_max { get => hv_Matching_rote_max; set => hv_Matching_rote_max = value; }
        public double Hv_Matching_rote_step { get => hv_Matching_rote_step; set => hv_Matching_rote_step = value; }
        public double Hv_Matching_min_Score { get => hv_Matching_min_Score; set => hv_Matching_min_Score = value; }
        public int Hv_Matching_Pyramid_level { get => hv_Matching_Pyramid_level; set => hv_Matching_Pyramid_level = value; }
        public double Hv_Matching_overlap_Max { get => hv_Matching_overlap_Max; set => hv_Matching_overlap_Max = value; }
        public HObject Hv_contour { get => hv_contour; set => hv_contour = value; }
        public double Hv_Matching_Radius { get => hv_Matching_Radius; set => hv_Matching_Radius = value; }
        public HTuple Hv_modelrow { get => hv_modelrow; set => hv_modelrow = value; }
        public HTuple Hv_modelcol { get => hv_modelcol; set => hv_modelcol = value; }



        //  HObject ho_TemplateImage = new HObject();
        public bool LoadingModel(string path)
        {


            try
            {
                Hv_ModelID.Dispose();
                HOperatorSet.ReadNccModel(path, out Hv_ModelID);
                //  HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
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

                HOperatorSet.WriteNccModel(Hv_ModelID, path);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool createmodel(HObject ho_Image, HObject Reg, out HObject REC_img)
        {
            HTuple area = new HTuple();
            HTuple model_how = new HTuple();
            HTuple model_col = new HTuple();

            try
            {
                // ho_TemplateImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, Reg, out REC_img);
                HOperatorSet.CreateNccModel(REC_img, "auto", (new HTuple(0)).TupleRad()
       , (new HTuple(360)).TupleRad(), "auto", "use_polarity", out Hv_ModelID);
                HOperatorSet.AreaCenter(Reg, out area, out model_how,
        out model_col);


                //测试 
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    Hv_Row.Dispose(); Hv_Column.Dispose(); Hv_Angle.Dispose(); Hv_Score.Dispose();
                    HOperatorSet.FindNccModel(ho_Image, Hv_ModelID, (new HTuple(0)).TupleRad()
                        , (new HTuple(360)).TupleRad(), 0.7, 1, 0.04, "false", 0, out Hv_Row, out Hv_Column,
                        out Hv_Angle, out Hv_Score);

                }
                //  hv_contour.Dispose();
                // HTuple hv_RefRow, hv_RefColumn;
                //  HOperatorSet.GenCrossContourXld(out hv_contour, model_how, model_col, 200, 0.0);
                return true;
            }



            catch (Exception)
            {
                REC_img = null;

                return false;
            }

        }



        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public bool action(int TASKID, int OperateID)
        {
            try
            {
                HObject ho_reg = new HObject();
                HOperatorSet.GenCircle(out ho_reg, hv_modelrow, hv_modelcol, Hv_Matching_Radius);
                HOperatorSet.FindNccModel(img, Hv_ModelID, (Hv_Matching_rote_min * Math.PI) / 180, (Hv_Matching_rote_max * Math.PI) / 180, Hv_Matching_min_Score, Hv_Matching_num, Hv_Matching_overlap_Max, "false", Hv_Matching_Pyramid_level, out Hv_Row, out Hv_Column,
          out Hv_Angle, out Hv_Score);
                //  HOperatorSet.WriteImage(img, "bmp", 0, "C:/Users/HW/Desktop/12.bmp");
                HOperatorSet.GenEmptyObj(out ho_Transregion_final);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    //Matching 01: Display the center of the match

                    hv_AlignmentHomMat2D.Dispose();
                    HOperatorSet.HomMat2dIdentity(out hv_AlignmentHomMat2D);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dTranslate(hv_AlignmentHomMat2D, -hv_modelrow, -hv_modelcol,
                            out ExpTmpOutVar_0);
                        hv_AlignmentHomMat2D.Dispose();
                        hv_AlignmentHomMat2D = ExpTmpOutVar_0;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dRotate(hv_AlignmentHomMat2D, Hv_Angle.TupleSelect(hv_I),
                            0, 0, out ExpTmpOutVar_0);
                        hv_AlignmentHomMat2D.Dispose();
                        hv_AlignmentHomMat2D = ExpTmpOutVar_0;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTuple ExpTmpOutVar_0;
                        HOperatorSet.HomMat2dTranslate(hv_AlignmentHomMat2D, Hv_Row.TupleSelect(hv_I),
                            Hv_Column.TupleSelect(hv_I), out ExpTmpOutVar_0);
                        hv_AlignmentHomMat2D.Dispose();
                        hv_AlignmentHomMat2D = ExpTmpOutVar_0;
                    }
                    //Matching 01: Display the aligned model region
                    ho_RegionAffineTrans.Dispose();
                    HOperatorSet.AffineTransRegion(ho_reg, out ho_RegionAffineTrans, hv_AlignmentHomMat2D,
                        "nearest_neighbor");

                    HOperatorSet.Union2(ho_Transregion_final, ho_RegionAffineTrans, out ho_Transregion_final);

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

            Hv_ModelID.Dispose();
            img.Dispose();
            hv_contour.Dispose();
            //  hv_Matching_Radius.d
            hv_modelrow.Dispose();
            hv_modelcol.Dispose();
            hv_I.Dispose();
            hv_AlignmentHomMat2D.Dispose(); ho_RegionAffineTrans.Dispose();
            ho_Transregion_final.Dispose();
        }

    }
}
