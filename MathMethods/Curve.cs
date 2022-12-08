using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods
{
    public class Point
    {
        public float Key { get; set; }
        public float Value { get; set; }

        public Point( float key, float value )
        {
            Key = key;
            Value = value;
        }
    }
    public class Curve
    {
        private List<Point> points;

        public Curve()
        {
            points = new List<Point>();
        }

        public void AddPoint( float key, float value )
        {
            Point point = new Point( key, value );
            points.Add( point );
        }

        public List<Point> GetPoints()
        {
            return points;
        }
        public float CalculateCurveValueAt( float t, Point p1, Point p2 )
        {
            // Calculate the value of the curve at t using cubic interpolation.
            float t0 = p1.Key;
            float t1 = p2.Key;
            float v0 = p1.Value;
            float v1 = p2.Value;
            float a = v0;
            float b = (3 * (v1 - v0)) / (t1 - t0);
            float c = (2 * (v0 - v1)) / ((t1 - t0) * (t1 - t0));
            float d = (v1 - v0) / ((t1 - t0) * (t1 - t0) * (t1 - t0));
            float value = a + (b * (t - t0)) + (c * (t - t0) * (t - t0)) + (d * (t - t0) * (t - t0) * (t - t0));

            return value;
        }

        public float EvaluateCurveAt( float t )
        {
            // Check if t is outside the range of the curve.
            if( t <= points[0].Key )
            {
                return points[0].Value;
            }
            else if( t >= points[points.Count - 1].Key )
            {
                return points[points.Count - 1].Value;
            }

            // Find the two points that surround t.
            Point p1 = null;
            Point p2 = null;
            for( int i = 0; i < points.Count - 1; i++ )
            {
                if( points[i].Key <= t && t <= points[i + 1].Key )
                {
                    p1 = points[i];
                    p2 = points[i + 1];
                    break;
                }
            }

            // Calculate the value of the curve at t using cubic interpolation.
            float value = CalculateCurveValueAt( t, p1, p2 );

            return value;
        }
    }
}
