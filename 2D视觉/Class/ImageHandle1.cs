using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6524.Class
{
    public class ImageHandle1
    {
        public int Dis_R = 75; //圆心偏距
        public int Max_R = 190;//圆半径上下限
        public int Min_R = 170;
        public int lim_Row = 857;
        public int lim_Column = 1105;

        public double RectangestartX= 75; //圆心偏距
        public double RectangestartY = 190;//圆半径上下限
        public double RectangeendX = 170;
        public double RectangeendY = 857;
   



        public double Add = 1105;
        public double Mult = 1105;
        public double MinThreshold  = 1105;
        public double MaxThreshod = 1105;


        public bool Judgment6524(HObject ho_Image,out HObject rectange, out HObject ho_Circle, out HTuple hv_Row,
      out HTuple hv_Column, out HTuple hv_Radius, out HTuple hv_Distance)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 
            HObject imagereduce;
        HObject ho_ImageOut = null, ho_ImageMean, ho_ImageScaled;
            HObject ho_ImageScaled1, ho_Intersection1, ho_RegionFillUp;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_SelectedRegions1;
            HObject ho_ImageScaled2, ho_Regions1, fillregion;
            // Local control variables 

            HTuple hv_Result = new HTuple(), hv_Dis_R = new HTuple();
            HTuple hv_Max_R = new HTuple(), hv_Min_R = new HTuple();
            HTuple hv_Channels = new HTuple(), hv_Number = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled1);
            HOperatorSet.GenEmptyObj(out ho_Intersection1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out fillregion);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled2);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out imagereduce);
            hv_Row = new HTuple();
            hv_Column = new HTuple();
            hv_Radius = new HTuple();
            hv_Distance = new HTuple();
            ho_ImageOut.Dispose();
            ho_ImageOut = new HObject(ho_Image);
            hv_Result.Dispose();
            hv_Result = 0;
            //圆心最大偏差距
            hv_Dis_R.Dispose();
            hv_Dis_R = Dis_R;
            hv_Max_R.Dispose();
            hv_Max_R = Max_R;
            hv_Min_R.Dispose();
            hv_Min_R = Min_R;



            hv_Channels.Dispose();
            HOperatorSet.CountChannels(ho_ImageOut, out hv_Channels);

            if ((int)(new HTuple(hv_Channels.TupleEqual(3))) != 0)
            {
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Rgb1ToGray(ho_ImageOut, out ExpTmpOutVar_0);
                    ho_ImageOut.Dispose();
                    ho_ImageOut = ExpTmpOutVar_0;
                }
            }
        
            HOperatorSet.GenRectangle1(out rectange, RectangestartX, RectangestartY, RectangeendX, RectangeendY);
            HOperatorSet.ReduceDomain(ho_ImageOut, rectange, out imagereduce);       

            HOperatorSet.ScaleImage(imagereduce, out ho_ImageScaled2, Mult, Add);
            ho_Regions1.Dispose();
            HOperatorSet.Threshold(ho_ImageScaled2, out ho_Regions1, MinThreshold, MaxThreshod);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regions1, out ho_ConnectedRegions);
            fillregion.Dispose();
            HOperatorSet.FillUp(ho_ConnectedRegions, out fillregion);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(fillregion, out ho_SelectedRegions, "area",
                "and", 1500, 9999999);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "circularity",
                "and", 0.9, 1);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);






            if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
            {

                hv_Row.Dispose(); hv_Column.Dispose(); hv_Radius.Dispose();
                HOperatorSet.InnerCircle(ho_SelectedRegions1, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, hv_Radius);
                hv_Distance.Dispose();
                HOperatorSet.DistancePp(hv_Row, hv_Column, lim_Row, lim_Column, out hv_Distance);
                //    圆心距偏差
                if ((int)(new HTuple(hv_Distance.TupleLess(hv_Dis_R))) != 0)
                {

                    //   控制半径
                    if ((int)((new HTuple(hv_Min_R.TupleLessEqual(hv_Radius))).TupleAnd(new HTuple(hv_Radius.TupleLessEqual(
                        hv_Max_R)))) != 0)
                    // if((hv_Min_R.D<= hv_Radius.D)&(hv_Radius.D<= hv_Max_R.D))
                    {

                        hv_Result.Dispose();
                        hv_Result = 1;
                        ho_ImageOut.Dispose();
                        ho_ImageMean.Dispose();
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled1.Dispose();
                        ho_Intersection1.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_SelectedRegions1.Dispose();

                        hv_Result.Dispose();
                        hv_Dis_R.Dispose();
                        hv_Max_R.Dispose();
                        hv_Min_R.Dispose();
                        hv_Channels.Dispose();
                        hv_Number.Dispose();

                        return true;
                    }
                    else
                    {
                        hv_Result.Dispose();
                        hv_Result = 0;
                        ho_ImageOut.Dispose();
                        ho_ImageMean.Dispose();
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled1.Dispose();
                        ho_Intersection1.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_SelectedRegions1.Dispose();

                        hv_Result.Dispose();
                        hv_Dis_R.Dispose();
                        hv_Max_R.Dispose();
                        hv_Min_R.Dispose();
                        hv_Channels.Dispose();
                        hv_Number.Dispose();

                        return false;
                    }
                }
                else
                {
                    hv_Result.Dispose();
                    hv_Result = 0;
                    ho_ImageOut.Dispose();
                    ho_ImageMean.Dispose();
                    ho_ImageScaled.Dispose();
                    ho_ImageScaled1.Dispose();
                    ho_Intersection1.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();
                    ho_SelectedRegions1.Dispose();

                    hv_Result.Dispose();
                    hv_Dis_R.Dispose();
                    hv_Max_R.Dispose();
                    hv_Min_R.Dispose();
                    hv_Channels.Dispose();
                    hv_Number.Dispose();

                    return false;

                }



            }
            else
            {
                ho_ImageOut.Dispose();
                ho_ImageMean.Dispose();
                ho_ImageScaled.Dispose();
                ho_ImageScaled1.Dispose();
                ho_Intersection1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_SelectedRegions1.Dispose();

                hv_Result.Dispose();
                hv_Dis_R.Dispose();
                hv_Max_R.Dispose();
                hv_Min_R.Dispose();
                hv_Channels.Dispose();
                hv_Number.Dispose();

                return false;
            }
            ho_ImageOut.Dispose();
            ho_ImageMean.Dispose();
            ho_ImageScaled.Dispose();
            ho_ImageScaled1.Dispose();
            ho_Intersection1.Dispose();
            ho_RegionFillUp.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();

            hv_Result.Dispose();
            hv_Dis_R.Dispose();
            hv_Max_R.Dispose();
            hv_Min_R.Dispose();
            hv_Channels.Dispose();
            hv_Number.Dispose();

            return false;
        }
    }
}
