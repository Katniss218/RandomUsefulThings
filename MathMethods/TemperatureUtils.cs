using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods
{
    public class TemperatureUtils
    {
        /// <summary>
        /// Gravitational constant. Valid when the calculations are done in kilograms and meters.
        /// </summary>
        public const double G = 0.00000000006674;

        /// <summary>
        /// Stephan boltzmann constant
        /// </summary>
        public const double SIGMA = 0.00000005670374419;

        public const double MASS_SUN = 1898000000000000000000000000000.0; // kg, I think
        public const double MASS_JUPITER = 1898000000000000000000000000.0; // kg, I think
        public const double MASS_EARTH = 5972000000000000000000000.0; // kg, I think

        public const double RADIUS_SUN = 696340000.0; // m
        public const double RADIUS_JUPITER = 69911000.0; // m
        public const double RADIUS_EARTH = 6371000.0; // m

        public const double AU = 149597870700.0; // m

        public const double HOUR_TO_SECONDS = 3600.0;
        public const double DAY_TO_SECONDS = HOUR_TO_SECONDS * 24;
        public const double MONTH_TO_SECONDS = DAY_TO_SECONDS * 30;
        public const double YEAR_TO_SECONDS = DAY_TO_SECONDS * 365;


       /* private static Gradient _blackbody = null;
        private static Gradient blackbody
        {
            get
            {
                if( _blackbody == null )
                {
                    _blackbody = GenerateBlackbody();
                }
                return _blackbody;
            }
        }

        private static Gradient GenerateBlackbody()
        {
            // Create the blackbody gradient
            // Max temperature of BLACKBODY_LOOKUP_MAX at time of 1.0f

            GradientColorKey[] keys = new GradientColorKey[8];
            keys[0] = new GradientColorKey( new Color( 1.000f, 0.220f, 0.0f ), 0.033f );
            keys[1] = new GradientColorKey( new Color( 1.000f, 0.494f, 0.0f ), 0.06f );
            keys[2] = new GradientColorKey( new Color( 1.000f, 0.706f, 0.420f ), 0.1f );
            keys[3] = new GradientColorKey( new Color( 1.000f, 0.894f, 0.808f ), 0.167f );
            keys[4] = new GradientColorKey( new Color( 0.914f, 0.929f, 1.0f ), 0.255f );
            keys[5] = new GradientColorKey( new Color( 0.769f, 0.843f, 1.0f ), 0.36f );
            keys[6] = new GradientColorKey( new Color( 0.659f, 0.773f, 1.0f ), 0.671f );
            keys[7] = new GradientColorKey( new Color( 0.624f, 0.749f, 1.0f ), 1.0f );

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
            alphaKeys[0] = new GradientAlphaKey( 1.0f, 0.0f );

            return new Gradient() { colorKeys = keys, alphaKeys = alphaKeys };
        }

        private const double BLACKBODY_LOOKUP_MAX = 29800.0f;

        /// <summary>
        /// Returns a blackbody radiation color given temperature in kelvins
        /// </summary>
        public static Color GetBlackbodyColor( double temperature )
        {
            if( temperature < 798.0 )
            {
                return Color.black;
            }
            Color color = blackbody.Evaluate( Math.Clamp( (float)(temperature / BLACKBODY_LOOKUP_MAX) ), 0.0f, 1.0f );
            color *= MathMethods.Lerp( 1, 5, (float)(temperature / BLACKBODY_LOOKUP_MAX) );
            return color;
        }*/

        public static double GetSolarConstant( double starTemperature, double starRadius, double planetSma )
        {
            double radioSq = (4.0 * Math.PI * starRadius) / (4.0 * Math.PI * planetSma);
            radioSq *= radioSq;

            double Ks = SIGMA * (starTemperature * starTemperature * starTemperature * starTemperature) * radioSq;

            return Ks;
        }

        public static double GetLuminosity( double temperature, double surfaceArea ) // total energy emitted by a blackbody, Watts
        {
            return SIGMA * (temperature * temperature * temperature * temperature) * surfaceArea;
        }

        /// "Energy Intercepted" is the amount of energy potentially available to a body, if it was perfectly absorbant.
        public static double GetEnergyIntercepted( double starTemperature, double starRadius, double planetSma, double planetRadius ) // Watts
        {
            // return [W] = Ks [W/m^2] * PI * r [m] ^2

            double Ks = GetSolarConstant( starTemperature, starRadius, planetSma );
            return Ks * Math.PI * (planetRadius * planetRadius);
        }

        public static double GetEnergyAbsorbed( double energyIntercepted, double albedo )
        {
            return energyIntercepted * (1.0 - albedo);
        }

        [Obsolete( "Unconfirmed" )]
        static double HeatTransferCoefficient( double thickness, double thermalConductivity, double specificHeatCapacity )
        {
            return thermalConductivity / (thickness * specificHeatCapacity);
        }

        [Obsolete( "Unconfirmed" )]
        public static double HeatTransferRate( double thickness, double thermalConductivity, double specificHeatCapacity, double temperatureDifference, double surfaceArea )
        {
            // nobody knows what units to plug in.
            return HeatTransferCoefficient( thickness, thermalConductivity, specificHeatCapacity) * temperatureDifference * surfaceArea;
        }

        // Supposedly this calculates a "feels like" temperature, but idk what that equation is or anything.
        [Obsolete("Unconfirmed")]
        public static double CalculateHeatIndex( double temperature, double relativeHumidity )
        {
            double heatIndex = -42.379
                + 2.04901523 * temperature
                + 10.14333127 * relativeHumidity
                - 0.22475541 * temperature * relativeHumidity
                - 0.00683783 * temperature * temperature
                - 0.05481717 * relativeHumidity * relativeHumidity
                + 0.00122874 * temperature * temperature * relativeHumidity
                + 0.00085282 * temperature * relativeHumidity * relativeHumidity
                - 0.00000199 * temperature * temperature * relativeHumidity * relativeHumidity;
            if( relativeHumidity < 13 && temperature >= 80.0 && temperature <= 112.0 )
            {
                heatIndex -= ((13.0 - relativeHumidity) / 4.0) * Math.Sqrt( (17.0 - Math.Abs( temperature - 95.0 )) / 17.0 );
            }
            if( relativeHumidity > 85.0 && temperature >= 80.0 && temperature <= 87.0 )
            {
                heatIndex += ((relativeHumidity - 85.0) / 10.0) * ((87.0 - temperature) / 5.0);
            }
            return heatIndex;
        }

        [Obsolete( "Unconfirmed" )]
        public static double CalculateWindChill( double temperature, double windSpeed )
        {
            if( temperature > 50.0 || windSpeed < 3.0 ) return temperature;
            double windChill = 35.74 + 0.6215 * temperature - 35.75 * Math.Pow( windSpeed, 0.16 ) + 0.4275 * temperature * Math.Pow( windSpeed, 0.16 );
            return windChill;
        }
    }
}
