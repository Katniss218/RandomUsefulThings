using System;
using System.Collections.Generic;
using System.Text;

namespace WaveFunctionCollapse
{
    // Different WFC algorithm written by an AI.
    class WFCSolver2
    {
        public void WFC()
        {
            // Define a grid of pixels, where each pixel has a set of possible states
            // (e.g. different colors or textures)
            int[,] grid = new int[10, 10];

            // Define a list of possible states for each pixel
            List<int> possibleStates = new List<int> { 1, 2, 3, 4, 5 };

            // Iterate over the grid, applying the rules of wave function collapse
            // to each pixel in turn
            for( int x = 0; x < grid.GetLength( 0 ); x++ )
            {
                for( int y = 0; y < grid.GetLength( 1 ); y++ )
                {
                    // Check the states of the surrounding pixels to determine
                    // the possible states for this pixel
                    List<int> allowedStates = GetAllowedStates( grid, x, y, possibleStates );

                    // Choose one of the allowed states for this pixel at random,
                    // with a probability determined by the initial wave function
                    grid[x, y] = ChooseState( allowedStates );
                }
            }

            // The grid is now filled in with a final set of states that satisfy
            // the input constraints
        }

        // This method takes a grid of pixels, the coordinates of a particular pixel,
        // and a list of possible states for each pixel, and returns the list of
        // states that are allowed for that pixel based on the states of the surrounding
        // pixels.
        List<int> GetAllowedStates( int[,] grid, int x, int y, List<int> possibleStates )
        {
            // Create a list to store the allowed states for this pixel
            List<int> allowedStates = new List<int>();

            // Iterate over the surrounding pixels, checking their states
            for( int i = -1; i <= 1; i++ )
            {
                for( int j = -1; j <= 1; j++ )
                {
                    // Skip the current pixel (we only care about the surrounding pixels)
                    if( i == 0 && j == 0 ) continue;

                    // Check if the current surrounding pixel is within the bounds of the grid
                    if( x + i >= 0 && x + i < grid.GetLength( 0 ) && y + j >= 0 && y + j < grid.GetLength( 1 ) )
                    {
                        // If the surrounding pixel has already been assigned a state,
                        // add that state to the list of allowed states for the current pixel
                        if( grid[x + i, y + j] != 0 )
                        {
                            allowedStates.Add( grid[x + i, y + j] );
                        }
                    }
                }
            }

            // If the list of allowed states is empty (i.e. none of the surrounding pixels
            // have been assigned a state yet), return the full list of possible states
            // for this pixel as the allowed states
            if( allowedStates.Count == 0 )
            {
                return possibleStates;
            }

            // Otherwise, return the list of allowed states that were determined based
            // on the states of the surrounding pixels
            return allowedStates;
        }

        // This method takes a list of allowed states for a particular pixel, and
        // chooses one of those states at random, with a probability determined
        // by the initial wave function.
        int ChooseState( List<int> allowedStates )
        {
            // Calculate the total probability of all allowed states
            double totalProbability = 0;
            foreach( int state in allowedStates )
            {
                totalProbability += GetStateProbability( state );
            }

            // Generate a random number between 0 and the total probability
            double randomNumber = GetRandomNumber( 0, totalProbability );

            // Iterate over the allowed states, subtracting their probabilities from
            // the random number until it is less than zero. The state where this
            // occurs is the chosen state.
            foreach( int state in allowedStates )
            {
                randomNumber -= GetStateProbability( state );
                if( randomNumber < 0 )
                {
                    return state;
                }
            }

            // If no state is chosen (which should not happen), return the first state
            // in the list of allowed states as a default
            return allowedStates[0];
        }

        // This method takes a minimum and maximum value and returns a random
        // number within that range.
        double GetRandomNumber( double min, double max )
        {
            // Use the built-in random number generator to generate a random
            // double-precision floating-point number between 0 and 1.
            double randomNumber = new Random().NextDouble();

            // Scale the random number to the desired range, and return it.
            return min + (randomNumber * (max - min));
        }

        // This method takes a particular state and returns its probability
        // according to the initial wave function.
        double GetStateProbability( int state )
        {
            // In this example, the probabilities of the different states are
            // defined in a dictionary, where the keys are the states and the
            // values are their corresponding probabilities.
            Dictionary<int, double> stateProbabilities = new Dictionary<int, double>
            {
                { 1, 0.25 },
                { 2, 0.20 },
                { 3, 0.15 },
                { 4, 0.30 },
                { 5, 0.10 },
            };

            // Return the probability of the specified state, or zero if the
            // state is not in the dictionary (which should not happen).
            return stateProbabilities.ContainsKey( state ) ? stateProbabilities[state] : 0;
        }

        // This method takes a grid of pixels, the coordinates of a particular pixel,
        // and a particular state, and returns the probability of that state according
        // to the initial wave function and the states of the surrounding pixels.
        double GetStateProbability( int[,] grid, int x, int y, int state )
        {
            // In this example, the probabilities of the different states are
            // defined in a dictionary, where the keys are the states and the
            // values are their corresponding probabilities.
            Dictionary<int, double> stateProbabilities = new Dictionary<int, double>
            {
                { 1, 0.25 },
                { 2, 0.20 },
                { 3, 0.15 },
                { 4, 0.30 },
                { 5, 0.10 },
            };

            // Initialize the probability of the specified state to its default value
            double probability = stateProbabilities[state];

            // Iterate over the surrounding pixels, checking their states and
            // adjusting the probability of the specified state accordingly.
            for( int i = -1; i <= 1; i++ )
            {
                for( int j = -1; j <= 1; j++ )
                {
                    // Skip the current pixel (we only care about the surrounding pixels)
                    if( i == 0 && j == 0 ) continue;

                    // Check if the current surrounding pixel is within the bounds of the grid
                    if( x + i >= 0 && x + i < grid.GetLength( 0 ) && y + j >= 0 && y + j < grid.GetLength( 1 ) )
                    {
                        // If the surrounding pixel has the same state as the specified state,
                        // increase the probability of the specified state.
                        if( grid[x + i, y + j] == state )
                        {
                            probability += 0.05;
                        }
                        // If the surrounding pixel has a different state, decrease the probability
                        // of the specified state.
                        else
                        {
                            probability -= 0.01;
                        }
                    }
                }
            }

            // Return the adjusted probability of the specified state.
            return probability;
        }
    }
}
