using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Graphics
{
    class ColorRGBM_HDR
    {
    }
}

/*

# -------------------------------------------------------------------------
# RGBMEncode
def RGBMEncode(color, useGamma=True, exp=6.0):
    """
    Implements conversion of color(RGB) to RGBM(RGBA)
    the expoent used in the conversion defaults to 6.0(float)
    """
    rgbm = [0.0, 0.0, 0.0, 0.0];
    
    expRange = 1.0 / exp;
    gamma = 1.0/2.23333333
    
    if useGamma:
        # linear --> gamma
        color[0] = pow(color[0], gamma) * expRange;
        color[1] = pow(color[1], gamma) * expRange;
        color[2] = pow(color[2], gamma) * expRange;
    
    # encode
    maxRGB = max( color[0], max( color[1], color[2]));

    rgbm[3] = math.ceil( maxRGB * 255.0 ) / 255.0;
    
    rgbm[0] = round( 255.0 * min(color[0] / rgbm[3], 1.0));
    rgbm[1] = round( 255.0 * min(color[1] / rgbm[3], 1.0));
    rgbm[2] = round( 255.0 * min(color[2] / rgbm[3], 1.0));
    rgbm[3] = round( 255.0 * min(rgbm[3], 1.0));
    
    return rgbm

*/