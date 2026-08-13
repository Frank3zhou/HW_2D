using HalconDotNet;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6524.Class
{
    public class QR_Code__recognition
    {
        public bool  Find2Code(HObject ho_Image20231012095440, out HObject ho_Rectangle,
      out HObject ho_SymbolXLDs, HTuple hv_Row1, HTuple hv_Column1, HTuple pi,HTuple length1,
      HTuple length2, HTuple hv_modelname, out HTuple hv_DecodedDataStrings)
        {


            try
            {
                // Local iconic variables 

                HObject ho_ImageReduced;

                // Local control variables 

                HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                hv_DecodedDataStrings = new HTuple();

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row1, hv_Column1,pi, length1, length2);


                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image20231012095440, ho_Rectangle, out ho_ImageReduced
                    );
                hv_DataCodeHandle.Dispose();
                HOperatorSet.CreateDataCode2dModel(hv_modelname, new HTuple(), new HTuple(),
                    out hv_DataCodeHandle);
                ho_SymbolXLDs.Dispose(); hv_ResultHandles.Dispose(); hv_DecodedDataStrings.Dispose();
                HOperatorSet.FindDataCode2d(ho_ImageReduced, out ho_SymbolXLDs, hv_DataCodeHandle,
                    new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                ho_ImageReduced.Dispose();

                hv_DataCodeHandle.Dispose();
                hv_ResultHandles.Dispose();

               
                return true;
            }
            catch (Exception)
            {
                hv_DecodedDataStrings = null;
                ho_Rectangle=null;
                ho_SymbolXLDs=null;
                return false;
            }

           
        }
    }
}
