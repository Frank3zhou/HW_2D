using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6524.Class
{
    public partial class find_bar
    {

        public void find_barcode(HObject ho_Image, out HObject ho_Rectangle, out HObject ho_SymbolRegions,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2,
            HTuple hv_mult, HTuple hv_add, HTuple hv_barcodetype, out HTuple hv_DecodedDataStrings)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_ImageScaled;

            // Local control variables 

            HTuple hv_BarCodeHandle = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_SymbolRegions);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            hv_DecodedDataStrings = new HTuple();
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                hv_Length2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImageScaled.Dispose();
            HOperatorSet.ScaleImage(ho_ImageReduced, out ho_ImageScaled, hv_mult, hv_add);
            hv_BarCodeHandle.Dispose();
            HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out hv_BarCodeHandle);
            ho_SymbolRegions.Dispose(); hv_DecodedDataStrings.Dispose();
            HOperatorSet.FindBarCode(ho_ImageScaled, out ho_SymbolRegions, hv_BarCodeHandle,
                hv_barcodetype, out hv_DecodedDataStrings);
            ho_ImageReduced.Dispose();
            ho_ImageScaled.Dispose();

            hv_BarCodeHandle.Dispose();

            return;
        }
    }
}

        // Main procedure 
      




