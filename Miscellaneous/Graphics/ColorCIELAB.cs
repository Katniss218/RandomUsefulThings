using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.Graphics
{
    internal class ColorXYZ
    {
        public float X, Y, Z;

        internal static ColorXYZ WhiteReference => new ColorXYZ
        {
            X = 95.047f,
            Y = 100.0f,
            Z = 108.883f
        }; // TODO: Hard-coded!

        internal const float Epsilon = 0.008856f; // Intent is 216/24389
        internal const float Kappa = 903.3f; // Intent is 24389/27


        internal static double CubicRoot( double n )
        {
            return Math.Pow( n, 1.0 / 3.0 );
        }

        // RGB to XYZ
        internal static void ToColorSpace( ColorRGBA color, ColorXYZ item )
        {
            float r = PivotRgb( color.R / 255.0f );
            float g = PivotRgb( color.G / 255.0f );
            float b = PivotRgb( color.B / 255.0f );

            // Observer. = 2°, Illuminant = D65
            item.X = r * 0.4124f + g * 0.3576f + b * 0.1805f;
            item.Y = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            item.Z = r * 0.0193f + g * 0.1192f + b * 0.9505f;
        }

        // XYZ to RGB
        internal static ColorRGBA ToColor( ColorXYZ item )
        {
            // (Observer = 2°, Illuminant = D65)
            float x = item.X / 100.0f;
            float y = item.Y / 100.0f;
            float z = item.Z / 100.0f;

            float r = x * 3.2406f + y * -1.5372f + z * -0.4986f;
            float g = x * -0.9689f + y * 1.8758f + z * 0.0415f;
            float b = x * 0.0557f + y * -0.2040f + z * 1.0570f;

            r = (float)(r > 0.0031308 ? 1.055 * Math.Pow( r, 1 / 2.4 ) - 0.055 : 12.92 * r);
            g = (float)(g > 0.0031308 ? 1.055 * Math.Pow( g, 1 / 2.4 ) - 0.055 : 12.92 * g);
            b = (float)(b > 0.0031308 ? 1.055 * Math.Pow( b, 1 / 2.4 ) - 0.055 : 12.92 * b);

            return new ColorRGBA( ToRgb( r ), ToRgb( g ), ToRgb( b ) );
        }

        private static float ToRgb( float n )
        {
            float result = 255.0f * n;
            if( result < 0 )
                return 0;
            if( result > 255 )
                return 255;
            return result;
        }

        private static float PivotRgb( float n )
        {
            return (float)((n > 0.04045 ? Math.Pow( (n + 0.055) / 1.055, 2.4 ) : n / 12.92) * 100.0);
        }
    }
    public struct ColorCIELAB
    {
        float L, A, B;

        internal static void ToColorSpace( ColorRGBA color, ColorCIELAB item )
        {
            var xyz = new ColorXYZ();
            ColorXYZ.ToColorSpace( color, xyz );

            ColorXYZ white = ColorXYZ.WhiteReference;
            float x = PivotXyz( xyz.X / white.X );
            float y = PivotXyz( xyz.Y / white.Y );
            float z = PivotXyz( xyz.Z / white.Z );

            item.L = Math.Max( 0, 116 * y - 16 );
            item.A = 500 * (x - y);
            item.B = 200 * (y - z);
        }

        internal static ColorRGBA ToColor( ColorCIELAB item )
        {
            var y = (item.L + 16.0) / 116.0;
            var x = item.A / 500.0 + y;
            var z = y - item.B / 200.0;

            var white = ColorXYZ.WhiteReference;
            var x3 = x * x * x;
            var z3 = z * z * z;
            var xyz = new ColorXYZ
            {
                X = white.X * (x3 > ColorXYZ.Epsilon ? x3 : (x - 16.0 / 116.0) / 7.787),
                Y = white.Y * (item.L > (ColorXYZ.Kappa * ColorXYZ.Epsilon) ? Math.Pow( ((item.L + 16.0) / 116.0), 3 ) : item.L / ColorXYZ.Kappa),
                Z = white.Z * (z3 > ColorXYZ.Epsilon ? z3 : (z - 16.0 / 116.0) / 7.787)
            };

            return xyz.ToRgb();
        }

        private static float PivotXyz( float n )
        {
            return n > ColorXYZ.Epsilon ? CubicRoot( n ) : (ColorXYZ.Kappa * n + 16) / 116;
        }

        private static double CubicRoot( double n )
        {
            return Math.Pow( n, 1.0 / 3.0 );
        }


        /// <summary>
        /// Calculates the CIE76 delta-e value: http://en.wikipedia.org/wiki/Color_difference#CIE76
        /// </summary>
        public double CompareCIE76( IColorSpace colorA, IColorSpace colorB )
        {
            var a = colorA.To<Lab>(); // CIELAB (LAB) color space.
            var b = colorB.To<Lab>();

            var differences = Distance( a.L, b.L ) + Distance( a.A, b.A ) + Distance( a.B, b.B );
            return Math.Sqrt( differences );
        }

        private static double Distance( double a, double b )
        {
            return (a - b) * (a - b);
        }

        /// <summary>
        /// Calculates the DE2000 delta-e value: http://en.wikipedia.org/wiki/Color_difference#CIEDE2000
        /// Correct implementation provided courtesy of Jonathan Hofinger, jaytar42
        /// </summary>
        public double Compare( IColorSpace c1, IColorSpace c2 )
        {
            //Set weighting factors to 1
            double k_L = 1.0d;
            double k_C = 1.0d;
            double k_H = 1.0d;


            //Change Color Space to L*a*b:
            Lab lab1 = c1.To<Lab>();
            Lab lab2 = c2.To<Lab>();

            //Calculate Cprime1, Cprime2, Cabbar
            double c_star_1_ab = Math.Sqrt( lab1.A * lab1.A + lab1.B * lab1.B );
            double c_star_2_ab = Math.Sqrt( lab2.A * lab2.A + lab2.B * lab2.B );
            double c_star_average_ab = (c_star_1_ab + c_star_2_ab) / 2;

            double c_star_average_ab_pot7 = c_star_average_ab * c_star_average_ab * c_star_average_ab;
            c_star_average_ab_pot7 *= c_star_average_ab_pot7 * c_star_average_ab;

            double G = 0.5d * (1 - Math.Sqrt( c_star_average_ab_pot7 / (c_star_average_ab_pot7 + 6103515625) )); //25^7
            double a1_prime = (1 + G) * lab1.A;
            double a2_prime = (1 + G) * lab2.A;

            double C_prime_1 = Math.Sqrt( a1_prime * a1_prime + lab1.B * lab1.B );
            double C_prime_2 = Math.Sqrt( a2_prime * a2_prime + lab2.B * lab2.B );
            //Angles in Degree.
            double h_prime_1 = ((Math.Atan2( lab1.B, a1_prime ) * 180d / Math.PI) + 360) % 360d;
            double h_prime_2 = ((Math.Atan2( lab2.B, a2_prime ) * 180d / Math.PI) + 360) % 360d;

            double delta_L_prime = lab2.L - lab1.L;
            double delta_C_prime = C_prime_2 - C_prime_1;

            double h_bar = Math.Abs( h_prime_1 - h_prime_2 );
            double delta_h_prime;
            if( C_prime_1 * C_prime_2 == 0 ) delta_h_prime = 0;
            else
            {
                if( h_bar <= 180d )
                {
                    delta_h_prime = h_prime_2 - h_prime_1;
                }
                else if( h_bar > 180d && h_prime_2 <= h_prime_1 )
                {
                    delta_h_prime = h_prime_2 - h_prime_1 + 360.0;
                }
                else
                {
                    delta_h_prime = h_prime_2 - h_prime_1 - 360.0;
                }
            }
            double delta_H_prime = 2 * Math.Sqrt( C_prime_1 * C_prime_2 ) * Math.Sin( delta_h_prime * Math.PI / 360d );

            // Calculate CIEDE2000
            double L_prime_average = (lab1.L + lab2.L) / 2d;
            double C_prime_average = (C_prime_1 + C_prime_2) / 2d;

            //Calculate h_prime_average

            double h_prime_average;
            if( C_prime_1 * C_prime_2 == 0 ) h_prime_average = 0;
            else
            {
                if( h_bar <= 180d )
                {
                    h_prime_average = (h_prime_1 + h_prime_2) / 2;
                }
                else if( h_bar > 180d && (h_prime_1 + h_prime_2) < 360d )
                {
                    h_prime_average = (h_prime_1 + h_prime_2 + 360d) / 2;
                }
                else
                {
                    h_prime_average = (h_prime_1 + h_prime_2 - 360d) / 2;
                }
            }
            double L_prime_average_minus_50_square = (L_prime_average - 50);
            L_prime_average_minus_50_square *= L_prime_average_minus_50_square;

            double S_L = 1 + ((.015d * L_prime_average_minus_50_square) / Math.Sqrt( 20 + L_prime_average_minus_50_square ));
            double S_C = 1 + .045d * C_prime_average;
            double T = 1
                - .17 * Math.Cos( DegToRad( h_prime_average - 30 ) )
                + .24 * Math.Cos( DegToRad( h_prime_average * 2 ) )
                + .32 * Math.Cos( DegToRad( h_prime_average * 3 + 6 ) )
                - .2 * Math.Cos( DegToRad( h_prime_average * 4 - 63 ) );
            double S_H = 1 + .015 * T * C_prime_average;
            double h_prime_average_minus_275_div_25_square = (h_prime_average - 275) / (25);
            h_prime_average_minus_275_div_25_square *= h_prime_average_minus_275_div_25_square;
            double delta_theta = 30 * Math.Exp( -h_prime_average_minus_275_div_25_square );

            double C_prime_average_pot_7 = C_prime_average * C_prime_average * C_prime_average;
            C_prime_average_pot_7 *= C_prime_average_pot_7 * C_prime_average;
            double R_C = 2 * Math.Sqrt( C_prime_average_pot_7 / (C_prime_average_pot_7 + 6103515625) );

            double R_T = -Math.Sin( DegToRad( 2 * delta_theta ) ) * R_C;

            double delta_L_prime_div_k_L_S_L = delta_L_prime / (S_L * k_L);
            double delta_C_prime_div_k_C_S_C = delta_C_prime / (S_C * k_C);
            double delta_H_prime_div_k_H_S_H = delta_H_prime / (S_H * k_H);

            double CIEDE2000 = Math.Sqrt(
                delta_L_prime_div_k_L_S_L * delta_L_prime_div_k_L_S_L
                + delta_C_prime_div_k_C_S_C * delta_C_prime_div_k_C_S_C
                + delta_H_prime_div_k_H_S_H * delta_H_prime_div_k_H_S_H
                + R_T * delta_C_prime_div_k_C_S_C * delta_H_prime_div_k_H_S_H
                );

            return CIEDE2000;
        }
        private double DegToRad( double degrees )
        {
            return degrees * Math.PI / 180;
        }



        /// <summary>
        /// Compare colors using the Cie94 algorithm. The first color (a) will be used as the reference color.
        /// </summary>
        /// <param name="a">Reference color</param>
        /// <param name="b">Comparison color</param>
        /// <returns></returns>
        public double CompareCIE94( IColorSpace a, IColorSpace b )
        {
            var labA = a.To<Lab>();
            var labB = b.To<Lab>();

            var deltaL = labA.L - labB.L;
            var deltaA = labA.A - labB.A;
            var deltaB = labA.B - labB.B;

            var c1 = Math.Sqrt( labA.A * labA.A + labA.B * labA.B );
            var c2 = Math.Sqrt( labB.A * labB.A + labB.B * labB.B );
            var deltaC = c1 - c2;

            var deltaH = deltaA * deltaA + deltaB * deltaB - deltaC * deltaC;
            deltaH = deltaH < 0 ? 0 : Math.Sqrt( deltaH );

            const double sl = 1.0;
            const double kc = 1.0;
            const double kh = 1.0;

            var sc = 1.0 + Constants.K1 * c1;
            var sh = 1.0 + Constants.K2 * c1;

            var deltaLKlsl = deltaL / (Constants.Kl * sl);
            var deltaCkcsc = deltaC / (kc * sc);
            var deltaHkhsh = deltaH / (kh * sh);
            var i = deltaLKlsl * deltaLKlsl + deltaCkcsc * deltaCkcsc + deltaHkhsh * deltaHkhsh;
            return i < 0 ? 0 : Math.Sqrt( i );
        }

        internal class ApplicationConstants
        {
            internal double Kl { get; private set; }
            internal double K1 { get; private set; }
            internal double K2 { get; private set; }

            public ApplicationConstants( Application application )
            {
                switch( application )
                {
                    case Application.GraphicArts:
                        Kl = 1.0;
                        K1 = .045;
                        K2 = .015;
                        break;
                    case Application.Textiles:
                        Kl = 2.0;
                        K1 = .048;
                        K2 = .014;
                        break;
                }
            }
        }

    }
}
