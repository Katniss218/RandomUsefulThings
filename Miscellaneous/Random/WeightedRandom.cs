using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomUsefulThings.Misc.Random
{
    [Obsolete( "untested" )]
    public class WeightedRandom<T>
    {
        public readonly List<(T val, double weight)> elements = new List<(T, double)>();

        readonly System.Random _random;
        double _totalWeight;
        bool _needsRefresh = true; 

        public WeightedRandom()
        {
            _random = new System.Random();
        }

        public WeightedRandom( int seed )
        {
            _random = new System.Random( seed );
        }

        public WeightedRandom( params (T val, double weight)[] theElements )
        {
            _random = new System.Random();
            elements = theElements.ToList();
        }

        public WeightedRandom( int seed, params (T val, double weight)[] theElements )
        {
            _random = new System.Random( seed );
            elements = theElements.ToList();
        }

        public T Get()
        {
            if( _needsRefresh )
            {
                Refresh();
            }

            double randomValue = _random.NextDouble();
            randomValue *= _totalWeight;

            foreach( var element in elements )
            {
                if( randomValue > element.weight )
                {
                    randomValue -= element.weight;
                    continue;
                }
                return element.val;
            }

            return default( T );
        }

        public double GetTotalWeight()
        {
            if( _needsRefresh )
            {
                Refresh();
            }
            return _totalWeight;
        }

        public void Add( T element, double weight = 1.0 )
        {
            elements.Add( (element, weight) );
            _needsRefresh = true;
        }

        public void Clear()
        {
            elements.Clear();
            _needsRefresh = true;
        }

        void Refresh()
        {
            _totalWeight = 0.0;

            foreach( var e in elements )
            {
                _totalWeight += e.weight;
            }

            _needsRefresh = false;
        }
    }
}