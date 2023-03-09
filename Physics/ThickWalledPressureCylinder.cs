using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class ThickWalledPressureCylinder
    {
        // A thin-walled pressure vessel is one with its radius less than 10x its wall thickness.
        static bool IsThickWalled( float radius, float thickness ) => radius / 10.0f < thickness;

        public double Pressure { get; set; }

        public double AmbientPressure { get; set; }

        public double Radius { get; set; }

        public double Thickness { get; set; }

        public double InternalRadius { get => Radius - Thickness; }

        // VERY OFTEN, there is the `Pressure * squareInternalRadius`, and `AmbientPressure * squareRadius`
        // I wonder why...

        /// <summary>
        /// Calculates the stress in the outward direction.
        /// </summary>
        public double CalculateRadialStress( float percAlongThickness )
        {
            // output units are the same as input unit for pressure, as long as the unit for distance (r, rInternal, etc) is consistent.
            // Use consistent unit for pressure and ambient pressure.
            double radiusToPoint = MathMethods.Interpolation.Lerp( InternalRadius, Radius, percAlongThickness );
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            return ((Pressure * squareInternalRadius) - (AmbientPressure * squareRadius) + (squareInternalRadius * squareRadius) * (Pressure - AmbientPressure) / (radiusToPoint * radiusToPoint))
                / (squareRadius - squareInternalRadius);
        }

        /// <summary>
        /// Calculates the stress in the tangent direction (along the wall of the cylinder).
        /// </summary>
        public double CalculateHoopStress( float percAlongThickness )
        {
            // output units are the same as input unit for pressure, as long as the unit for distance (r, rInternal, etc) is consistent.
            // Use consistent unit for pressure and ambient pressure.
            double radiusToPoint = MathMethods.Interpolation.Lerp( InternalRadius, Radius, percAlongThickness );
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            return ((Pressure * squareInternalRadius) - (AmbientPressure * squareRadius) - (squareInternalRadius * squareRadius) * (Pressure - AmbientPressure) / (radiusToPoint * radiusToPoint))
                / (squareRadius - squareInternalRadius);
        }

        public double CalculateLongitudinalStress()
        {
            // "If the ends of the cylinder are capped, must include longitudinal stress."
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            return ((Pressure * squareInternalRadius) - (AmbientPressure * squareRadius)) /
                (squareRadius - squareInternalRadius);
        }

        public double CalculateMaximumShearStress( float percAlongThickness )
        {
            return 0.5 * (CalculateHoopStress( percAlongThickness ) - CalculateRadialStress( percAlongThickness ));
        }

        public double CalculateRadialDisplacement( double youngsModulus, double poissonsRatio, float percAlongThickness )
        {
            double radiusToPoint = MathMethods.Interpolation.Lerp( InternalRadius, Radius, percAlongThickness );
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            double firstPart = ((1 - poissonsRatio) / youngsModulus)
                * (((squareInternalRadius * Pressure - squareRadius * AmbientPressure) * radiusToPoint) / (squareRadius - squareInternalRadius));

            double secondPart = ((1 + poissonsRatio) / youngsModulus)
                * (((Pressure - AmbientPressure) * squareInternalRadius * squareRadius) / ((squareRadius - squareInternalRadius) * radiusToPoint));

            return firstPart + secondPart;
        }

        public double CalculateRadialStress2( float percAlongThickness )
        {
            if( AmbientPressure != 0.0 )
            {
                throw new InvalidProgramException();
            }

            double radiusToPoint = MathMethods.Interpolation.Lerp( InternalRadius, Radius, percAlongThickness );
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            return ((squareInternalRadius * Pressure) / (squareRadius - squareInternalRadius)) * (1 - squareRadius / (radiusToPoint * radiusToPoint));
        }

        public double CalculateHoopStress2( float percAlongThickness )
        {
            if( AmbientPressure != 0.0 )
            {
                throw new InvalidProgramException();
            }

            double radiusToPoint = MathMethods.Interpolation.Lerp( InternalRadius, Radius, percAlongThickness );
            double squareRadius = Radius * Radius;
            double squareInternalRadius = InternalRadius * InternalRadius;

            return ((squareInternalRadius * Pressure) / (squareRadius - squareInternalRadius)) * (1 + squareRadius / (radiusToPoint * radiusToPoint));
        }
    }
}
