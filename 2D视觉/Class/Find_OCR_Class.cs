using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6524.Class
{
    public class Find_OCR_Class
    {
        public void Find_OCR(HObject ho_Image4, out HObject ho_Rectangle, out HObject ho_RectangleXLD,
     HTuple hv_Mult, HTuple hv_Add, HTuple hv_Row, HTuple hv_Column, HTuple hv_Phi,
     HTuple hv_Length1, HTuple hv_Length2, HTuple hv_ModelName, out HTuple hv_Result)
        {




            // Local iconic variables 

            HObject ho_ImageScaled, ho_ImageReduced, ho_Characters;
            HObject ho_RegionUnion;

            // Local control variables 

            HTuple hv_TextModel = new HTuple(), hv_TextResultID = new HTuple();
            HTuple hv_ResultValue = new HTuple(), hv_Length = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RectangleXLD);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Characters);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            hv_Result = new HTuple();
            ho_ImageScaled.Dispose();
            HOperatorSet.ScaleImage(ho_Image4, out ho_ImageScaled, hv_Mult, hv_Add);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                hv_Length2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageScaled, ho_Rectangle, out ho_ImageReduced);
            hv_TextModel.Dispose();
            HOperatorSet.CreateTextModelReader("auto", hv_ModelName, out hv_TextModel);
            hv_TextResultID.Dispose();
            HOperatorSet.FindText(ho_ImageReduced, hv_TextModel, out hv_TextResultID);

            ho_Characters.Dispose();
            HOperatorSet.GetTextObject(out ho_Characters, hv_TextResultID, "all_lines");
            hv_ResultValue.Dispose();
            HOperatorSet.GetTextResult(hv_TextResultID, "class", out hv_ResultValue);

            hv_Length.Dispose();
            HOperatorSet.TupleLength(hv_ResultValue, out hv_Length);
            hv_Result.Dispose();
            hv_Result = "";
            HTuple end_val11 = hv_Length - 1;
            HTuple step_val11 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val11, step_val11); hv_Index = hv_Index.TupleAdd(step_val11))
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Result = hv_Result + (hv_ResultValue.TupleSelect(
                            hv_Index));
                        hv_Result.Dispose();
                        hv_Result = ExpTmpLocalVar_Result;
                    }
                }
            }
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_Characters, out ho_RegionUnion);
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Phi1.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
            HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_Row1, out hv_Column1,
                out hv_Phi1, out hv_Length11, out hv_Length21);
            ho_RectangleXLD.Dispose();
            HOperatorSet.GenRectangle2ContourXld(out ho_RectangleXLD, hv_Row1, hv_Column1,
                hv_Phi1, hv_Length11, hv_Length21);
            ho_ImageScaled.Dispose();
            ho_ImageReduced.Dispose();
            ho_Characters.Dispose();
            ho_RegionUnion.Dispose();

            hv_TextModel.Dispose();
            hv_TextResultID.Dispose();
            hv_ResultValue.Dispose();
            hv_Length.Dispose();
            hv_Index.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Phi1.Dispose();
            hv_Length11.Dispose();
            hv_Length21.Dispose();

            return;
        }
    }
}
