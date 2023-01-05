using System;
using System.Collections.Generic;

namespace Miscellaneous
{
    public class KnapsackProblem
    {
        public class Item
        {
            /// <summary>
            /// The payoff for carrying this item.
            /// </summary>
            public int Payoff { get; set; }

            /// <summary>
            /// The cost for carrying this item.
            /// </summary>
            public int Cost { get; set; }
        }

        [Obsolete( "Unconfirmed" )]
        public (int totalValue, Item[] items) Solve( int capacity, Item[] items )
        {
            // Create a 2D array to store the maximum value for each capacity and each item
            int[,] values = new int[items.Length + 1, capacity + 1];

            // Create a 2D array to store the item choice for each capacity and each item
            int[,] choices = new int[items.Length + 1, capacity + 1];

            // Loop through each item
            for( int i = 1; i <= items.Length; i++ )
            {
                // Loop through each capacity
                for( int j = 0; j <= capacity; j++ )
                {
                    // If the current item is too heavy to fit in the knapsack, 
                    // the maximum value is the same as the maximum value without the current item
                    if( items[i - 1].Cost > j )
                    {
                        values[i, j] = values[i - 1, j];
                    }
                    // Otherwise, the maximum value is the maximum of either taking the current item 
                    // or not taking the current item
                    else
                    {
                        int valueWithItem = values[i - 1, j - items[i - 1].Cost] + items[i - 1].Payoff;
                        if( valueWithItem > values[i - 1, j] )
                        {
                            values[i, j] = valueWithItem;
                            choices[i, j] = 1;
                        }
                        else
                        {
                            values[i, j] = values[i - 1, j];
                        }
                    }
                }
            }

            // Create a list to store the chosen items
            List<Item> chosenItems = new List<Item>();

            // Loop through the items in reverse order
            for( int i = items.Length, j = capacity; i > 0; i-- )
            {
                // If the current item was chosen, add it to the list and subtract its weight from the remaining capacity
                if( choices[i, j] == 1 )
                {
                    chosenItems.Add( items[i - 1] );
                    j -= items[i - 1].Cost;
                }
            }

            // Return the maximum value and the list of chosen items
            return (values[items.Length, capacity], chosenItems.ToArray());
        }
    }
}
