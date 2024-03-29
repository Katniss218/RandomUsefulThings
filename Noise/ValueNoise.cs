﻿using RandomUsefulThings.Math;
using System;

// GLSL noises
// https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83

namespace Noise
{
    public class ValueNoise
    {
        /// <summary>
        /// Returns a single layer of noise at the given coordinates.
        /// </summary>
        public static float ValueAt( float x, float y, float amp, float freq )
        {
            x *= freq;
            y *= freq;

            float x0 = (float)System.Math.Floor( x );
            float x1 = x0 + 1.0f;
            float y0 = (float)System.Math.Floor( y );
            float y1 = y0 + 1.0f;

            float v00 = Hash.Get( x0, y0 );
            float v01 = Hash.Get( x0, y1 );
            float v10 = Hash.Get( x1, y0 );
            float v11 = Hash.Get( x1, y1 );


            float sx = x - x0;
            float sy = y - y0;
            float n0 = Interpolation.LerpUnclamped( v00, v10, sx );
            float n1 = Interpolation.LerpUnclamped( v01, v11, sx );
            float v = Interpolation.LerpUnclamped( n0, n1, sy );

            float result = v * amp;

            return result;
        }

        // It's possible to take less samples and interpolate the results of the noise.
    }
}