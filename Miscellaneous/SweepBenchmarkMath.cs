using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public class SweepBenchmarkMath<TIn, TOut>
    {
        // benchmarks a function with an array of different values, gives the time as a range of measurements.

        public Func<float, TIn> ParameterFunc; // receives the benchmark progress [0..1] and returns the corresponding value.

        public int Iterations; // iterations per repeats.
        public int Steps; // steps (time points) per run.

        List<(string name, Func<TIn, TOut> action)> _actions;
        public Func<TIn, TOut> Reference;
        public Func<TOut, TOut, TOut> GetError; // returns (left - right);

        public SweepBenchmarkMath( int steps = 1000, int iterations = 1000 )
        {
            this.Steps = steps;
            this.Iterations = iterations;

            _actions = new List<(string, Func<TIn, TOut>)>();
        }

        public void Add( string name, Func<TIn, TOut> action )
        {
            _actions.Add( (name, action) );
        }

        public void Run()
        {
            _actions.Insert( 0, ("__empty i.e. () => { }", ( _ ) => // Allows us to subtract the average running time of the overhead later.
            {
                return default( TOut );
            }
            ) );

            double[][] subtotals = new double[_actions.Count][]; // actions, runs
            (TOut, TIn)[][] errors = new (TOut, TIn)[_actions.Count][];

            for( int actionI = 0; actionI < subtotals.Length; actionI++ )
            {
                subtotals[actionI] = new double[Steps];
                errors[actionI] = new (TOut, TIn)[Steps];
            }

            for( int i = 0; i < Steps; i++ )
            {
                float time = (float)i / Steps;
                TIn input = ParameterFunc( time );
                TOut refVal = Reference( input );

                for( int actionI = 0; actionI < subtotals.Length; actionI++ )
                {
                    var del = _actions[actionI].action;

                    TOut val = del( input );
                    TOut error = GetError( val, refVal );

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for( int j = 0; j < Iterations; j++ )
                    {
                        del( input );
                    }
                    sw.Stop();

                    subtotals[actionI][i] = sw.ElapsedTicks / 10000.0; // to miliseconds.
                    errors[actionI][i] = (error, input);
                }
            }

            double totalReference = 0.0f;
            for( int i = 0; i < Steps; i++ )
            {
                totalReference += subtotals[0][i];
            }
            double avgReference = totalReference / Steps; // average per single step.

            double previousTotal = 1.0f;

            for( int actionI = 1; actionI < subtotals.Length; actionI++ )
            {
                double total = 0.0f;
                Array.Sort( subtotals[actionI] );
                Array.Sort( errors[actionI] );

                for( int i = 0; i < Steps; i++ )
                {
                    total += subtotals[actionI][i];
                }

                double totalClamped = total - totalReference; // Don't take into account the running time of the lambda call.
                double minClamped = (subtotals[actionI][0]) - avgReference;
                double maxClamped = (subtotals[actionI][Steps - 1]) - avgReference;
                double avgClamped = (total / Steps) - avgReference;
                double medianClamped = (subtotals[actionI][Steps / 2]) - avgReference;

                var minError = errors[actionI][0];
                var maxError = errors[actionI][Steps - 1];
                var medianError = errors[actionI][Steps / 2];

                Console.WriteLine();
                Console.WriteLine( $"#=#=#   #=#=#   #=#=#   #=#=#   #=#=#" );
                Console.WriteLine( $"{_actions[actionI].name}, in range [{ParameterFunc( 0.0f )}..{ParameterFunc( 1.0f )}], with {Steps} steps" );
                Console.WriteLine( $"total running time: {System.Math.Round( total, 2 )} μs ({total / previousTotal})" );
                Console.WriteLine();
                Console.WriteLine( $"min time> {System.Math.Round( minClamped, 3 )} μs" );
                Console.WriteLine( $"max time> {System.Math.Round( maxClamped, 3 )} μs" );
                Console.WriteLine( $"avg time> {System.Math.Round( avgClamped, 3 )} μs" );
                Console.WriteLine( $"med time> {System.Math.Round( medianClamped, 3 )} μs" );
                Console.WriteLine();
                Console.WriteLine( $"min error> {minError.Item1}, at {minError.Item2}" );
                Console.WriteLine( $"max error> {maxError.Item1}, at {maxError.Item2}" );
                Console.WriteLine( $"med error> {medianError.Item1}, at {medianError.Item2}" );
                Console.WriteLine();
                previousTotal = total;
            }
        }
    }

}
