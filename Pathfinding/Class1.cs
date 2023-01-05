using Geometry;
using System;
using System.Collections.Generic;

namespace Pathfinding
{
    [Obsolete( "Unconfirmed" )]
    public class AStarPathfinder
    {
        // The grid of nodes that represents the level
        private Node[,] grid;

        // The start and end nodes
        private Node startNode;
        private Node endNode;

        // The width and height of the grid
        private int gridWidth;
        private int gridHeight;

        // The set of open nodes (nodes to be inspected)
        private List<Node> openNodes;

        // The set of closed nodes (nodes that have been inspected)
        private HashSet<Node> closedNodes;

        // The cost of moving horizontally or vertically
        private const int STRAIGHT_COST = 10;

        // The cost of moving diagonally
        private const int DIAGONAL_COST = 14;

        // Constructor
        public AStarPathfinder( int[,] level, Vector2 startPos, Vector2 endPos )
        {
            // Initialize the grid
            gridWidth = level.GetLength( 0 );
            gridHeight = level.GetLength( 1 );
            grid = new Node[gridWidth, gridHeight];
            for( int x = 0; x < gridWidth; x++ )
            {
                for( int y = 0; y < gridHeight; y++ )
                {
                    grid[x, y] = new Node( x, y, level[x, y] == 1 );
                }
            }

            // Set the start and end nodes
            startNode = grid[(int)startPos.X, (int)startPos.Y];
            endNode = grid[(int)endPos.X, (int)endPos.Y];

            // Initialize the open and closed node sets
            openNodes = new List<Node>();
            closedNodes = new HashSet<Node>();
        }

        // Finds the shortest path from the start node to the end node
        public List<Vector2> FindPath()
        {
            // Add the start node to the open node set
            openNodes.Add( startNode );

            // While there are still nodes to inspect
            while( openNodes.Count > 0 )
            {
                // Get the node with the lowest F score
                Node currentNode = GetLowestFScoreNode();

                // If the current node is the end node, we have found the shortest path
                if( currentNode == endNode )
                {
                    return CalculatePath( startNode, endNode );
                }

                // Remove the current node from the open node set and add it to the closed node set
                openNodes.Remove( currentNode );
                closedNodes.Add( currentNode );

                // Get the neighbors of the current node
                List<Node> neighbors = GetNeighbors( currentNode );

                // For each neighbor of the current node
                foreach( Node neighbor in neighbors )
                {
                    // If the neighbor is already in the closed node set, skip it
                    if( closedNodes.Contains( neighbor ) )
                    {
                        continue;
                    }

                    // Calculate the cost to move to the neighbor
                    int cost = STRAIGHT_COST;
                    if( currentNode.X != neighbor.X && currentNode.Y != neighbor.Y )
                    {
                        cost = DIAGONAL_COST;
                    }

                    // Calculate the tentative G score (cost to move to the neighbor)
                    int tentativeGScore = currentNode.G + cost;

                    // If the neighbor is not in the open node set, add it and set its G and F scores
                    if( !openNodes.Contains( neighbor ) )
                    {
                        neighbor.G = tentativeGScore;
                        neighbor.H = CalculateHScore( neighbor, endNode );
                        neighbor.F = neighbor.G + neighbor.H;
                        neighbor.Parent = currentNode;
                        openNodes.Add( neighbor );
                    }
                    // If the neighbor is in the open node set and the cost to move to it is cheaper than its current G score, update its G and F scores
                    else if( tentativeGScore < neighbor.G )
                    {
                        neighbor.G = tentativeGScore;
                        neighbor.F = neighbor.G + neighbor.H;
                        neighbor.Parent = currentNode;
                    }
                }
            }

            // If we get here, it means that we have not found a path to the end node
            return null;
        }

        // Gets the node with the lowest F score in the open node set
        private Node GetLowestFScoreNode()
        {
            Node lowestFScoreNode = openNodes[0];
            for( int i = 1; i < openNodes.Count; i++ )
            {
                if( openNodes[i].F < lowestFScoreNode.F )
                {
                    lowestFScoreNode = openNodes[i];
                }
            }
            return lowestFScoreNode;
        }

        // Calculates the H score (estimated distance from a node to the end node)
        private int CalculateHScore( Node node, Node endNode )
        {
            int dx = Math.Abs( node.X - endNode.X );
            int dy = Math.Abs( node.Y - endNode.Y );
            return DIAGONAL_COST * Math.Min( dx, dy ) + STRAIGHT_COST * Math.Abs( dx - dy );
        }

        // Gets the neighbors of a node
        private List<Node> GetNeighbors( Node node )
        {
            List<Node> neighbors = new List<Node>();
            for( int x = -1; x <= 1; x++ )
            {
                for( int y = -1; y <= 1; y++ )
                {
                    // Skip the current node
                    if( x == 0 && y == 0 )
                    {
                        continue;
                    }

                    // Calculate the coordinates of the neighbor
                    int neighborX = node.X + x;
                    int neighborY = node.Y + y;

                    // Make sure the neighbor is within the bounds of the grid
                    if( neighborX >= 0 && neighborX < gridWidth && neighborY >= 0 && neighborY < gridHeight )
                    {
                        // Add the neighbor to the list
                        neighbors.Add( grid[neighborX, neighborY] );
                    }
                }
            }
            return neighbors;
        }

        // Calculates the path from the start node to the end node by following the chain of parents
        private List<Vector2> CalculatePath( Node startNode, Node endNode )
        {
            List<Vector2> path = new List<Vector2>();
            Node currentNode = endNode;
            while( currentNode != startNode )
            {
                path.Add( new Vector2( currentNode.X, currentNode.Y ) );
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }
    }

    // The node class used by the A* pathfinding algorithm
    public class Node
    {
        // The x and y coordinates of the node
        public int X { get; set; }
        public int Y { get; set; }

        // Whether the node is walkable or not
        public bool IsWall { get; set; }

        // The G, H, and F scores of the node
        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }

        // The parent node of the current node
        public Node Parent { get; set; }

        // Constructor
        public Node( int x, int y, bool isWall )
        {
            X = x;
            Y = y;
            IsWall = isWall;
        }
    }
}

