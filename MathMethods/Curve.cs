using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    /// <summary>
    /// Represents an arbitrary non-uniform bezier spline.
    /// </summary>
    /// <typeparam name="T">The interpolated point type.</typeparam>
    public class Curve<T>
    {
        struct Entry
        {
            public T value;
            public float time;

            public static int Compare( Entry left, Entry right )
            {
                if( left.time == right.time )
                {
                    return 0;
                }
                return left.time < right.time ? -1 : 1;
            }

            public override string ToString()
            {
                return $"({time}:{value})";
            }
        }

        Entry[] _points;
        float _timeStart;
        float _timeEnd;

        public Curve( T[] values, float[] times )
        {
            if( values.Length != times.Length )
            {
                throw new ArgumentException( "The length of the Values array must be the same as the length of the Times array." );
            }

            _points = new Entry[values.Length];
            for( int i = 0; i < values.Length; i++ )
            {
                _points[i] = new Entry() { value = values[i], time = times[i] };
            }
            Array.Sort<Entry>( _points, Entry.Compare );
            _timeStart = _points[0].time;
            _timeEnd = _points[_points.Length - 1].time;
        }

        int FindFirstPointAheadOf( float t )
        {
            for( int i = 0; i < _points.Length; i++ )
            {
                if( _points[i].time > t )
                {
                    return i;
                }
            }
            return -1;
        }

        int FindCurveInterval( float t, int length )
        {
            // find the beginning of the subarray of length l that the time t lies in.
            // TODO - what if the array is not a multiple of length?
            for( int i = 0; i < _points.Length; i++ )
            {
                if( _points[i].time > t ) // what is the start of [i-1..i] lies in?
                {
                    return (i / length) * length;
                }
            }
            return -1;
        }

        public T EvaluateLinearBezier( Func<T, T, float, T> interpolator, float t ) // interpolator is lerp or slerp
        {
            if( t <= _timeStart )
                return _points[0].value;
            if( t >= _timeEnd )
                return _points[_points.Length - 1].value;

            int indexOfSecondPoint = FindFirstPointAheadOf( t );
            if( indexOfSecondPoint == -1 )
            {
                throw new InvalidOperationException( $"The time '{t}' was outside of the curve [{_timeStart}..{_timeEnd}]." );
            }

            int i0 = indexOfSecondPoint - 1;
            int i1 = indexOfSecondPoint;

            Entry p0 = _points[i0];
            Entry p1 = _points[i1];

            float intervalTime = p1.time - p0.time;
            if( intervalTime == 0 )
            {
                throw new InvalidOperationException( $"The interval between points {i0} and {i1} was 0." );
            }
            float timeOnInterval = (t - p0.time) / (intervalTime);

            T p0_1 = interpolator( p0.value, p1.value, timeOnInterval );

            return p0_1;
        }

        public T EvaluateQuadraticBezier( Func<T, T, float, T> interpolator, float t ) // interpolator is lerp or slerp
        {
            if( t <= _timeStart )
                return _points[0].value;
            if( t >= _timeEnd )
                return _points[_points.Length - 1].value;

            int indexOfFirstPoint = FindCurveInterval( t, 3 );
            if( indexOfFirstPoint == -1 )
            {
                throw new InvalidOperationException( $"The time '{t}' was outside of the curve [{_timeStart}..{_timeEnd}]." );
            }

            // quadratic and higher order curves we don't determine which point lies "after" t, but in which segment the t lies.
            int i0 = indexOfFirstPoint;
            int i1 = indexOfFirstPoint + 1;
            int i2 = indexOfFirstPoint + 2;

            if( i1 >= _points.Length ) i1 = i0;
            if( i2 >= _points.Length ) i2 = i1;

            Entry p0 = _points[i0];
            Entry p1 = _points[i1];
            Entry p2 = _points[i2];

            float intervalTime = p2.time - p0.time;
            if( intervalTime == 0 )
            {
                throw new InvalidOperationException( $"The interval between points {i0} and {i2} was 0." );
            }
            float timeOnInterval = Interpolation.InverseLerp( p0.time, p2.time, t );

            // start point can have a time that makes the camera wait.
            // only the start and end points of each subinterval have a time value, and they need the same T as the previous point.

            T p0_1 = interpolator( p0.value, p1.value, timeOnInterval );
            T p1_2 = interpolator( p1.value, p2.value, timeOnInterval );
            T p01_12 = interpolator( p0_1, p1_2, timeOnInterval );

            return p01_12;
        }

        public T EvaluateCubicBezier( Func<T, T, float, T> interpolator, float t ) // interpolator is lerp or slerp
        {
            if( t <= _timeStart )
                return _points[0].value;
            if( t >= _timeEnd )
                return _points[_points.Length - 1].value;

            int indexOfFirstPoint = FindCurveInterval( t, 4 );
            if( indexOfFirstPoint == -1 )
            {
                throw new InvalidOperationException( $"The time '{t}' was outside of the curve [{_timeStart}..{_timeEnd}]." );
            }

            // quadratic and higher order curves we don't determine which point lies "after" t, but in which segment the t lies.
            int i0 = indexOfFirstPoint;
            int i1 = indexOfFirstPoint + 1;
            int i2 = indexOfFirstPoint + 2;
            int i3 = indexOfFirstPoint + 3;

            if( i1 >= _points.Length ) i1 = i0;
            if( i2 >= _points.Length ) i2 = i1;
            if( i3 >= _points.Length ) i3 = i2;

            Entry p0 = _points[i0];
            Entry p1 = _points[i1];
            Entry p2 = _points[i2];
            Entry p3 = _points[i3];

            float intervalTime = p3.time - p0.time;
            if( intervalTime == 0 )
            {
                throw new InvalidOperationException( $"The interval between points {i0} and {i3} was 0." );
            }
            float timeOnInterval = Interpolation.InverseLerp( p0.time, p3.time, t );

            // start point can have a time that makes the camera wait.
            // only the start and end points of each subinterval have a time value, and they need the same T as the previous point.

            T p0_1 = interpolator( p0.value, p1.value, timeOnInterval );
            T p1_2 = interpolator( p1.value, p2.value, timeOnInterval );
            T p2_3 = interpolator( p2.value, p3.value, timeOnInterval );
            T p01_12 = interpolator( p0_1, p1_2, timeOnInterval );
            T p12_23 = interpolator( p1_2, p2_3, timeOnInterval );
            T p0112_1223 = interpolator( p01_12, p12_23, timeOnInterval );

            return p0112_1223;
        }

        public static float Bernstein( int i, int n, float t )
        {
            return MathMethods.Binomial( n, i ) * (float)System.Math.Pow( t, i ) * (float)System.Math.Pow( 1 - t, n - i );
        }
    }


    /*
    public static class BSplineToBezier
    {
        // not tested at all, but chat gave me this, might be helpful??

        public static List<PointF> ConvertBSplineToBezierSpline( List<PointF> controlPoints, List<float> knotVector, int degree )
        {
            int n = controlPoints.Count - 1;
            List<PointF> bezierPoints = new List<PointF>();

            for( int i = 0; i <= n - degree; i++ )
            {
                List<PointF> bezierSegmentPoints = new List<PointF>();
                List<float> bezierWeights = new List<float>();

                // Compute the control points and weights for the Bezier segment
                for( int j = 0; j <= degree; j++ )
                {
                    int index = i + j;
                    bezierSegmentPoints.Add( controlPoints[index] );
                    bezierWeights.Add( ComputeBezierWeight( knotVector, index, degree ) );
                }

                // Convert the Bezier segment into a single Bezier curve
                List<PointF> curvePoints = ConvertBezierSegmentToBezierCurve( bezierSegmentPoints, bezierWeights );

                // Add the points to the final Bezier spline
                if( bezierPoints.Count > 0 )
                {
                    // Remove the first point to avoid duplicating points
                    curvePoints.RemoveAt( 0 );
                }
                bezierPoints.AddRange( curvePoints );
            }

            return bezierPoints;
        }

        private static float ComputeBezierWeight( List<float> knotVector, int index, int degree )
        {
            float left = knotVector[index + degree] - knotVector[index];
            float right = knotVector[index + degree + 1] - knotVector[index + 1];

            return left / (left + right);
        }

        private static List<PointF> ConvertBezierSegmentToBezierCurve( List<PointF> controlPoints, List<float> weights )
        {
            int n = controlPoints.Count - 1;
            List<PointF> curvePoints = new List<PointF>();

            for( float t = 0; t <= 1; t += 0.05f )
            {
                PointF point = new PointF( 0, 0 );
                float weightSum = 0;

                for( int i = 0; i <= n; i++ )
                {
                    float weight = weights[i] * Bernstein( i, degree, t );
                    point.X += controlPoints[i].X * weight;
                    point.Y += controlPoints[i].Y * weight;
                    weightSum += weight;
                }

                // Normalize the point by the sum of the weights
                point.X /= weightSum;
                point.Y /= weightSum;

                curvePoints.Add( point );
            }

            return curvePoints;
        }

        private static float Bernstein( int i, int n, float t )
        {
            return Binomial( n, i ) * (float)Math.Pow( t, i ) * (float)Math.Pow( 1 - t, n - i );
        }


    }

    public static class BSplineToBezier2
    {
        public static List<PointF> BsplineToBezier( List<PointF> controlPoints, int degree, int numberOfSegments )
        {
            // Calculate knots
            int n = controlPoints.Count - 1;
            int m = n + degree + 1;
            List<float> knots = new List<float>();
            for( int i = 0; i <= m; i++ )
            {
                if( i < degree + 1 )
                    knots.Add( 0 );
                else if( i > n )
                    knots.Add( n - degree + 1 );
                else
                    knots.Add( i - degree );
            }

            // Convert B-spline to Bezier
            List<PointF> bezierPoints = new List<PointF>();
            for( int i = 0; i < numberOfSegments; i++ )
            {
                float t = (float)i / (float)numberOfSegments;
                List<PointF> deBoorPoints = new List<PointF>( controlPoints );
                for( int r = 1; r <= degree; r++ )
                {
                    for( int j = degree; j >= r; j-- )
                    {
                        float alpha = (t - knots[j]) / (knots[j + degree - r] - knots[j]);
                        deBoorPoints[j] = (1 - alpha) * deBoorPoints[j - 1] + alpha * deBoorPoints[j];
                    }
                }
                bezierPoints.Add( deBoorPoints[degree] );
            }
            bezierPoints.Add( controlPoints[n] );

            return bezierPoints;
        }
    }
    public static class BSplineToBezier3
    {
        // knots = knot vector.
        public static List<PointF> ConvertBsplineToBezier( List<PointF> controlPoints, List<float> knots, int degree )
        {
            int n = controlPoints.Count - 1;
            int m = knots.Count - 1;
            int p = degree;

            List<PointF> bezierPoints = new List<PointF>();

            for( int i = p; i < n - p; i++ )
            {
                float t = knots[i];

                List<PointF> segmentControlPoints = new List<PointF>();
                for( int j = i - p; j <= i; j++ )
                {
                    segmentControlPoints.Add( controlPoints[j] );
                }
                for( int j = 1; j <= p; j++ )
                {
                    float alpha = (t - knots[i - p + j]) / (knots[i + j] - knots[i - p + j]);
                    for( int k = 0; k < segmentControlPoints.Count; k++ )
                    {
                        segmentControlPoints[k] = new PointF(
                            (1 - alpha) * segmentControlPoints[k].X + alpha * segmentControlPoints[k + 1].X,
                            (1 - alpha) * segmentControlPoints[k].Y + alpha * segmentControlPoints[k + 1].Y );
                    }
                }
                bezierPoints.Add( segmentControlPoints[0] );
            }
            bezierPoints.Add( controlPoints[n] );

            return bezierPoints;
        }
    }

    public static class BSplineToBezier4
    {
        public static List<Point> ConvertBsplineToBezier( List<Point> points, int degree )
        {
            List<Point> bezierPoints = new List<Point>();
            int n = points.Count - degree - 1;

            // calculate knots
            double[] knots = new double[n + degree + 1];
            for( int i = 0; i < knots.Length; i++ )
            {
                if( i < degree + 1 )
                    knots[i] = 0;
                else if( i >= n + degree )
                    knots[i] = 1;
                else
                    knots[i] = (double)(i - degree) / (n - degree + 1);
            }

            // convert B-spline to Bezier spline
            for( int i = 0; i < n; i++ )
            {
                Point[] b = new Point[degree + 1];
                for( int j = 0; j <= degree; j++ )
                    b[j] = points[i + j];

                for( int r = degree; r > 0; r-- )
                {
                    for( int j = 0; j < r; j++ )
                    {
                        double alpha = (knots[i + j + degree + 1] - knots[i + j + degree - r + 1]) / (knots[i + j + degree + 1] - knots[i + j + 1]);
                        b[j] = (1 - alpha) * b[j] + alpha * b[j + 1];
                    }
                }
                bezierPoints.AddRange( b );
            }

            // add last point
            bezierPoints.Add( points[points.Count - 1] );

            return bezierPoints;
        }
    }

    public static class BSplineToCubicBezier
    {
        public static List<PointF> ConvertBsplineToBezier( List<PointF> controlPoints, int degree, float tolerance )
        {
            // Determine the number of segments in the B-spline
            int numSegments = controlPoints.Count - degree - 1;

            // Initialize the output list of Bezier control points
            List<PointF> bezierPoints = new List<PointF>();

            // Loop through each segment of the B-spline
            for( int i = 0; i < numSegments; i++ )
            {
                // Compute the Bezier control points for this segment
                List<PointF> segmentPoints = ComputeBezierControlPoints( controlPoints, degree, i );

                // Add the control points to the output list
                bezierPoints.AddRange( segmentPoints );
            }

            // Add the last point of the B-spline to the output list
            bezierPoints.Add( controlPoints[controlPoints.Count - 1] );

            return bezierPoints;
        }

        private static List<PointF> ComputeBezierControlPoints( List<PointF> controlPoints, int degree, int segmentIndex )
        {
            // Compute the Bezier control points for the given segment of the B-spline
            List<PointF> segmentPoints = new List<PointF>();
            segmentPoints.Add( controlPoints[segmentIndex] );

            for( int j = 1; j <= degree; j++ )
            {
                float t = (float)j / (float)(degree + 1);
                PointF newPoint = ComputeBsplinePoint( controlPoints, degree, segmentIndex, t );
                segmentPoints.Add( newPoint );
            }

            return segmentPoints;
        }

        private static PointF ComputeBsplinePoint( List<PointF> controlPoints, int degree, int segmentIndex, float t )
        {
            // Compute the point on the B-spline at the given parameter value
            PointF point = new PointF( 0, 0 );

            for( int i = 0; i <= degree; i++ )
            {
                float blend = ComputeBsplineBlend( i, degree, t );
                PointF controlPoint = controlPoints[segmentIndex - degree + i];
                point.X += blend * controlPoint.X;
                point.Y += blend * controlPoint.Y;
            }

            return point;
        }

        private static float ComputeBsplineBlend( int i, int degree, float t )
        {
            // Compute the B-spline blending function value for the given index and parameter value
            float blend = 0;

            if( degree == 1 )
            {
                if( i == 0 )
                {
                    blend = 1 - t;
                }
                else
                {
                    blend = t;
                }
            }
            else
            {
                float denom1 = controlPoints[segmentIndex + degree - i].X - controlPoints[segmentIndex - i].X;
                float denom2 = controlPoints[segmentIndex + degree - i].Y - controlPoints[segmentIndex - i].Y;

                if( denom1 == 0 && denom2 == 0 )
                {
                    blend = 0;
                }
                else
                {
                    float alpha = (t - (float)i / (float)degree) * (float)degree / (float)(degree + 1);
                    blend = (1 - alpha) * ComputeBsplineBlend( i, degree - 1, t ) + alpha * ComputeBsplineBlend( i + 1, degree - 1, t );
                }
            }

            return blend;
        }
    }

    public static class BSplineToCubicBezier2
    {
        public List<PointF> ConvertBsplineToBezier( List<PointF> controlPoints, List<float> knots, int degree, int resolution )
        {
            List<PointF> result = new List<PointF>();

            for( int i = degree; i < controlPoints.Count - degree; i++ )
            {
                List<PointF> bezierControlPoints = new List<PointF>();
                for( int j = 0; j <= 3; j++ )
                {
                    int index = i - degree + j;
                    float weight = BasisFunction( knots, index, degree, j );
                    bezierControlPoints.Add( new PointF( controlPoints[index].X * weight, controlPoints[index].Y * weight ) );
                }

                for( int j = 1; j <= resolution; j++ )
                {
                    float t = (float)j / resolution;
                    PointF point = DeCasteljau( bezierControlPoints, t );
                    result.Add( point );
                }
            }

            return result;
        }

        private float BasisFunction( List<float> knots, int index, int degree, int j )
        {
            if( degree == 0 )
            {
                if( j == 0 && knots[index] <= 1 && knots[index + 1] > 1 )
                    return 1;
                else
                    return 0;
            }
            else
            {
                float left = (knots[index + degree] - knots[index]);
                float right = (knots[index + degree + 1] - knots[index + 1]);
                float alpha = 0, beta = 0;

                if( left > 0 )
                    alpha = (1 - (knots[index + degree] - knots[index + j]) / left) * BasisFunction( knots, index, degree - 1, j );
                if( right > 0 )
                    beta = ((knots[index + degree + 1] - knots[index + j + 1]) / right) * BasisFunction( knots, index + 1, degree - 1, j - 1 );

                return alpha + beta;
            }
        }

        private PointF DeCasteljau( List<PointF> controlPoints, float t )
        {
            if( controlPoints.Count == 1 )
                return controlPoints[0];

            List<PointF> tempPoints = new List<PointF>( controlPoints );

            while( tempPoints.Count > 1 )
            {
                for( int i = 0; i < tempPoints.Count - 1; i++ )
                {
                    float x = (1 - t) * tempPoints[i].X + t * tempPoints[i + 1].X;
                    float y = (1 - t) * tempPoints[i].Y + t * tempPoints[i + 1].Y;
                    tempPoints[i] = new PointF( x, y );
                }

                tempPoints.RemoveAt( tempPoints.Count - 1 );
            }

            return tempPoints[0];
        }
    }*/
}
