using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace RandomUsefulThings.Misc
{
    /// <summary>
    /// Purpose - comparing relative running time of multiple functions.
    /// </summary>
    public class UnscaledTimeBenchmark
    {
        public enum Mode
        {
            Milisecond,
            Microsecond,
            Nanosecond,
            UnscaledTimeUnit
        }

        public List<(string, Action)> actions = new List<(string, Action)>();
        public int iterations = 10000; // iterations per repeats.
        public int repeats = 200; // repeats per run.

        public void Add( string name, Action action )
        {
            actions.Add( (name, action) );
        }

        public void Run( Mode timeMode )
        {
            actions.Insert( 0, ("__baseline", () => // some baseline. This will set the value of U.
            {
                int a = 5;
                int b = 7;
                int c = a + b;
                float f = 50.0f * (float)c;
            }
            ) );
            actions.Insert( 1, ("__empty i.e. () => { }", () => // I found that empty lambdas and empty methods have the same running time of approx. 0.9 U.
            {
            }
            ) );

            double[] total = new double[actions.Count];
            for( int i = 0; i < total.Length; i++ )
            {
                total[i] = 0.0;
            }

            for( int a = 0; a < repeats; a++ )
            {
                for( int i = 0; i < total.Length; i++ )
                {
                    var ac = actions[i];
                    var del = ac.Item2;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for( int j = 0; j < iterations; j++ )
                    {
                        del();
                    }
                    sw.Stop();
                    total[i] += sw.ElapsedTicks / 10000.0;
                }
            }

            // const int TIME_UNIT_MULTIPLIER = 1_000_000; // 1000 * seconds taken

            double baseline = (total[0] / repeats) * 1000 / iterations; // multiplying by 1000 prevents accumulating rounding issues since numbers are generally small.

            Console.WriteLine( $"__baseline = {RS( baseline, 2 ).ToString( CultureInfo.InvariantCulture )} ticks" );
            Console.WriteLine( $"__1/baseline = {(1 / baseline).ToString( CultureInfo.InvariantCulture )} ticks" );

            Console.WriteLine( $"\nRESULTS, scaled so __baseline = 1 tick (1 U)\n ----------------------------------\n" );

            for( int i = 1; i < total.Length; i++ )
            {
                double totalClamped = i == 1 ? total[i] : total[i] - total[1]; // Don't take into account the running time of the lambda call.
                Console.WriteLine( $"{actions[i].Item1} AVG" );

                if( timeMode == Mode.Milisecond )
                {
                    Console.WriteLine( $"-- {System.Math.Round( (totalClamped / repeats) / iterations, 2 )} ms" );
                }
                else if( timeMode == Mode.Microsecond )
                {
                    Console.WriteLine( $"-- {System.Math.Round( ((totalClamped / repeats) * 1000) / iterations, 2 )} μs" );
                }
                else if( timeMode == Mode.Nanosecond )
                {
                    Console.WriteLine( $"-- {System.Math.Round( (((totalClamped / repeats) * 1000) / iterations) * 1000, 2 )} ns" );
                }
                else if( timeMode == Mode.UnscaledTimeUnit )
                {
                    double timeTaken = (totalClamped / repeats) * 1000 / iterations; // multiplying by 1000 prevents accumulating rounding issues since numbers are generally small.
                    Console.WriteLine( $"-- {RS( timeTaken / baseline, 2 ).ToString( CultureInfo.InvariantCulture )} U" );
                }

                Console.WriteLine();
            }
        }

        static double RS( double d, int sigDigits )
        {
            if( d == 0 )
                return 0;

            double scale = System.Math.Pow( 10, System.Math.Floor( System.Math.Log10( System.Math.Abs( d ) ) ) + 1 );
            return scale * System.Math.Round( d / scale, sigDigits );
        }
    }
}