using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.Graphics
{
    /// <summary>
    /// Represents a color value internally as 4 32-bit floats.
    /// </summary>
    public struct ColorRGBA
    {
        // this could be extended to represent the color in different ways using the same 3 variables, as HSV, HSL, YCbCr.

        /// <summary>
        /// The red channel ([0..1] in SDR).
        /// </summary>
        public float R { get; }

        /// <summary>
        /// The green channel ([0..1] in SDR).
        /// </summary>
        public float G { get; }

        /// <summary>
        /// The blue channel ([0..1] in SDR).
        /// </summary>
        public float B { get; }

        /// <summary>
        /// The alpha (transparency) channel, [0..1].
        /// </summary>
        public float A { get; }

        public ColorRGBA( float r, float g, float b )
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 1.0f;
        }

        public ColorRGBA( float r, float g, float b, float a )
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Returns the value of the lowest channel.
        /// </summary>
        /// <returns></returns>
        public float Min()
        {
            return Math.Min( Math.Min( R, G ), B );
        }

        /// <summary>
        /// Returns the value of the highest channel.
        /// </summary>
        public float Max()
        {
            return Math.Max( Math.Max( R, G ), B );
        }

        /// <summary>
        /// Returns the values of the lowest and highest channels.
        /// </summary>
        public (float min, float max) MinMax()
        {
            return (Min(), Max());
        }

        /// <summary>
        /// Returns the lightness (HSL) of the color.
        /// </summary>
        /// <returns>The lightness in range [0..1].</returns>
        public float GetLightness()
        {
            // The lightness (luminosity) is defined as the average between the highest and lowest RGB color channel).
            (float min, float max) = MinMax();

            // It actually can return higher/lower when using HDR.
            return (min + max) / 2.0f;
        }

        public float GetHue()
        {
            // Should return hue in range [0..1]

            float h;

            (float min, float max) = MinMax();
            float delta = max - min;

            if( delta == 0 )
            {
                h = 0;
            }
            else if( max == R )
            {
                h = (((G - B) / delta) % 6);
            }
            else if( max == G )
            {
                h = ((B - R) / delta) + 2;
            }
            else
            {
                h = ((R - G) / delta) + 4;
            }

            h = h * 60;
            if( h < 0 )
            {
                h += 360;
            }

            return h / 360.0f;
        }

        public ColorRGBA AddLuminosity( float delta )
        {
            throw new NotImplementedException();
        }

        public static ColorRGBA FromIntARGB( int argb )
        {
            byte a = (byte)((argb >> 24) & 0xFF);
            byte r = (byte)((argb >> 16) & 0xFF);
            byte g = (byte)((argb >> 8) & 0xFF);
            byte b = (byte)(argb & 0xFF);

            return new ColorRGBA( r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f );
        }

        public static ColorRGBA FromIntRGB( int rgb )
        {
            byte r = (byte)((rgb >> 16) & 0xFF);
            byte g = (byte)((rgb >> 8) & 0xFF);
            byte b = (byte)(rgb & 0xFF);

            return new ColorRGBA( r / 255.0f, g / 255.0f, b / 255.0f );
        }

        /// <summary>
        /// Returns an integer color.
        /// </summary>
        /// <returns>The integer representation of the RGB color. 8 bits per channel, RGB.</returns>
        public int GetIntARGB()
        {
            int a = (int)Math.Round( A * 255.0f );
            return (a << 24) | GetIntRGB();
        }

        /// <summary>
        /// Returns an integer color.
        /// </summary>
        /// <returns>The integer representation of the RGBA color. 8 bits per channel, ARGB.</returns>
        public int GetIntRGB()
        {
            int r = (int)Math.Round( R * 255.0f );
            int g = (int)Math.Round( G * 255.0f );
            int b = (int)Math.Round( B * 255.0f );

            return (r << 16) | (g << 8) | b;
        }

        [Obsolete( "Unconfirmed" )]
        public static void HSLtoRGB( double h, double s, double l, out int r, out int g, out int b )
        {
            double rf, gf, bf;

            if( s == 0 )
            {
                rf = gf = bf = l; // achromatic
            }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                rf = HueToRGB( p, q, h + 1.0 / 3 );
                gf = HueToRGB( p, q, h );
                bf = HueToRGB( p, q, h - 1.0 / 3 );
            }

            r = (int)(rf * 255.0);
            g = (int)(gf * 255.0);
            b = (int)(bf * 255.0);
        }

        /* this should work.
        internal static IRgb ToColor( IHsv item )
        {
            var range = Convert.ToInt32( Math.Floor( item.H / 60.0 ) ) % 6;
            var f = item.H / 60.0 - Math.Floor( item.H / 60.0 );

            var v = item.V * 255.0;
            var p = v * (1 - item.S);
            var q = v * (1 - f * item.S);
            var t = v * (1 - (1 - f) * item.S);

            switch( range )
            {
                case 0:
                    return NewRgb( v, t, p );
                case 1:
                    return NewRgb( q, v, p );
                case 2:
                    return NewRgb( p, v, t );
                case 3:
                    return NewRgb( p, q, v );
                case 4:
                    return NewRgb( t, p, v );
            }
            return NewRgb( v, p, q );
        }
        */

        /* this should work
        private static Tuple<double, double, double> ToHsl( IRgb color )
        {
            color.R = Math.Round( color.R, 0 );
            color.G = Math.Round( color.G, 0 );
            color.B = Math.Round( color.B, 0 );
            var max = Max( color.R, Max( color.G, color.B ) );
            var min = Min( color.R, Min( color.G, color.B ) );

            double h, s, l;

            //saturation
            var cnt = (max + min) / 2d;
            if( cnt <= 127d )
            {
                s = ((max - min) / (max + min));
            }
            else
            {
                s = ((max - min) / (510d - max - min));
            }

            //lightness
            l = ((max + min) / 2d) / 255d;

            //hue
            if( Math.Abs( max - min ) <= float.Epsilon )
            {
                h = 0d;
                s = 0d;
            }
            else
            {
                double diff = max - min;

                if( Math.Abs( max - color.R ) <= float.Epsilon )
                {
                    h = 60d * (color.G - color.B) / diff;
                }
                else if( Math.Abs( max - color.G ) <= float.Epsilon )
                {
                    h = 60d * (color.B - color.R) / diff + 120d;
                }
                else
                {
                    h = 60d * (color.R - color.G) / diff + 240d;
                }

                if( h < 0d )
                {
                    h += 360d;
                }
            }

            return new Tuple<double, double, double>( h, s, l );
        }*/

        /* this should work
        public static IRgb ToColor( IHsl item )
        {
            var rangedH = item.H / 360.0;
            var r = 0.0;
            var g = 0.0;
            var b = 0.0;
            var s = item.S;
            var l = item.L;

            if( !l.BasicallyEqualTo( 0 ) )
            {
                if( s.BasicallyEqualTo( 0 ) )
                {
                    r = g = b = l;
                }
                else
                {
                    var temp2 = (l < 0.5) ? l * (1.0 + s) : l + s - (l * s);
                    var temp1 = 2.0 * l - temp2;

                    r = GetColorComponent( temp1, temp2, rangedH + 1.0 / 3.0 );
                    g = GetColorComponent( temp1, temp2, rangedH );
                    b = GetColorComponent( temp1, temp2, rangedH - 1.0 / 3.0 );
                }
            }
            return new Rgb
            {
                R = 255.0 * r,
                G = 255.0 * g,
                B = 255.0 * b
            };
        }
        */

        [Obsolete( "Unconfirmed" )]
        private static double HueToRGB( double p, double q, double t )
        {
            if( t < 0 )
                t += 1;
            if( t > 1 )
                t -= 1;
            if( t < 1.0 / 6 )
                return p + (q - p) * 6 * t;
            if( t < 1.0 / 2 )
                return q;
            if( t < 2.0 / 3 )
                return p + (q - p) * (2.0 / 3 - t) * 6;
            return p;
        }

        [Obsolete( "Unconfirmed" )]
        public static void HSVtoRGB( double h, double s, double v, out int r, out int g, out int b )
        {
            int i;
            double f, p, q, t;

            if( s == 0 )
            {
                r = g = b = (int)(v * 255.0);
                return;
            }

            h /= 60;
            i = (int)h;
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch( i )
            {
                case 0:
                    r = (int)(v * 255.0);
                    g = (int)(t * 255.0);
                    b = (int)(p * 255.0);
                    break;
                case 1:
                    r = (int)(q * 255.0);
                    g = (int)(v * 255.0);
                    b = (int)(p * 255.0);
                    break;
                case 2:
                    r = (int)(p * 255.0);
                    g = (int)(v * 255.0);
                    b = (int)(t * 255.0);
                    break;
                case 3:
                    r = (int)(p * 255.0);
                    g = (int)(q * 255.0);
                    b = (int)(v * 255.0);
                    break;
                case 4:
                    r = (int)(t * 255.0);
                    g = (int)(p * 255.0);
                    b = (int)(v * 255.0);
                    break;
                default:
                    r = (int)(v * 255.0);
                    g = (int)(p * 255.0);
                    b = (int)(q * 255.0);
                    break;
            }
        }

        /*
        public void RGBtoHSL( out float h, out float s, out float l )
        {
            // assumes rgb is byte, which is wrong.
            double rf = r / 255.0;
            double gf = g / 255.0;
            double bf = b / 255.0;

            double max = Math.Max( Math.Max( rf, gf ), bf );
            double min = Math.Min( Math.Min( rf, gf ), bf );
            double delta = max - min;

            if( delta == 0 )
            {
                h = 0;
            }
            else if( max == rf )
            {
                h = (((gf - bf) / delta) % 6);
            }
            else if( max == gf )
            {
                h = ((bf - rf) / delta) + 2;
            }
            else
            {
                h = ((rf - gf) / delta) + 4;
            }

            h = h * 60;
            if( h < 0 )
            {
                h += 360;
            }

            l = (max + min) / 2.0;

            if( delta == 0 )
            {
                s = 0;
            }
            else
            {
                s = delta / (1 - Math.Abs( 2 * l - 1 ));
            }
        }

        public void RGBtoHSV( out double h, out double s, out double v )
        {
            double min, max, delta;

            min = Math.Min( Math.Min( r, g ), b );
            max = Math.Max( Math.Max( r, g ), b );
            v = max;

            delta = max - min;

            if( max != 0 )
            {
                s = delta / max;
            }
            else
            {
                s = 0;
                h = -1;
                return;
            }

            if( r == max )
                h = (g - b) / delta;
            else if( g == max )
                h = 2 + (b - r) / delta;
            else
                h = 4 + (r - g) / delta;

            h *= 60;
            if( h < 0 )
                h += 360; // output in 360 deg
        }*/
    }
}

