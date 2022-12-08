using System;
using System.Collections.Generic;
using System.Text;

namespace WaveFunctionCollapse
{
    // WFC algotithm created by an AI.
    public class WFCSolver
    {

#warning TODO - lacks the "superposition"
#warning TODO - lacks passing the grid multiple times and choosing the tile with the least states.
#warning TODO - lacks backtracking if it failed.
        public static void WFC()
        {
            // Define the grid size
            const int gridWidth = 10;
            const int gridHeight = 10;

            // Define the possible states that each element can be in
            string[] possibleStates = { "A", "B", "C" };

            // Create a 2D array to represent the grid
            string[,] grid = new string[gridWidth, gridHeight];

            // Initialize the grid by assigning each element a random state
            Random rng = new Random();
            for( int x = 0; x < gridWidth; x++ )
            {
                for( int y = 0; y < gridHeight; y++ )
                {
                    int stateIndex = rng.Next( possibleStates.Length );
                    grid[x, y] = possibleStates[stateIndex];
                }
            }

            // Iterate over the grid, applying the rules of wave function collapse
            // to each element in turn
            for( int x = 0; x < gridWidth; x++ )
            {
                for( int y = 0; y < gridHeight; y++ )
                {
                    // Get the current element's state
                    string currentState = grid[x, y];

                    // Get the states of the elements to the left, right, top, and bottom
                    // of the current element
                    string leftState = (x > 0) ? grid[x - 1, y] : null;
                    string rightState = (x < gridWidth - 1) ? grid[x + 1, y] : null;
                    string topState = (y > 0) ? grid[x, y - 1] : null;
                    string bottomState = (y < gridHeight - 1) ? grid[x, y + 1] : null;

                    // Apply the rules of wave function collapse to determine the new state
                    // of the current element. For example, you could use the following rule:
                    // If the element to the left has the same state as the current element,
                    // then the current element should be in that state. Otherwise, choose
                    // a random state from the possible states.
                    if( currentState == leftState )
                    {
                        grid[x, y] = leftState;
                    }
                    else
                    {
                        int newStateIndex = rng.Next( possibleStates.Length );
                        grid[x, y] = possibleStates[newStateIndex];
                    }
                }
            }
        }

    }
}

