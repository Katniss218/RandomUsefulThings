using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathMethods.Graphs
{
    /// <summary>
    /// Defines a node in a tree data structure that contains children of the same type as itself.
    /// </summary>
    public interface ITreeNode<out T>
    {
        IEnumerable<T> Children { get; }
    }

    // example how to use the ITreeNode<T>
    class Node : ITreeNode<Node>
    {
        public IEnumerable<Node> Children { get; }
    }

    public static class GraphTraversal
    {
        public static void DepthFirstRec<T>( T node, Action<T> action ) where T : ITreeNode<T>
        {
            action( node );
            foreach( T child in node.Children )
            {
                DepthFirstRec( child, action ); // Recursively call the function for each child node
            }
        }

        [Obsolete( "Untested" )]
        public static void DepthFirst<T>( T root, Action<T> action ) where T : ITreeNode<T>
        {
            Stack<T> stack = new Stack<T>();
            stack.Push( root );

            while( stack.Count > 0 )
            {
                T node = stack.Pop();
                action( node );

                foreach( T child in node.Children.Reverse() )
                {
                    stack.Push( child );
                }
            }
        }

        /*      MORRIS TRAVERSAL sample. (not tested)
        public static void DepthFirstSearch<T>(T root, Action<T> action) where T : ITreeNode<T>
        {
            T curr = root;

            while (curr != null)
            {
                if (curr.Children == null)
                {
                    action(curr); // Do whatever you want to do with the node here
                    curr = curr.Children;
                }
                else
                {
                    T prev = curr.Children;
                    while (prev.Children != null && prev.Children != curr)
                    {
                        prev = prev.Children;
                    }

                    if (prev.Children == null)
                    {
                        prev.Children = curr;
                        curr = curr.Children;
                    }
                    else
                    {
                        prev.Children = null;
                        action(curr); // Do whatever you want to do with the node here
                        curr = curr.Children;
                    }
                }
            }
        }
        */

        public static void BreadthFirst<T>( T root, Action<T> action ) where T : ITreeNode<T>
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue( root );

            while( queue.Count > 0 )
            {
                // execute each node and enqueue its children.
                // adds each layer to the queue.
                // The queue will grow to contain all of the nodes of a given layer, one node of the previous layer at a time.
                T node = queue.Dequeue();
                action( node );

                // enqueue each child
                foreach( T child in node.Children )
                {
                    queue.Enqueue( child );
                }
            }
        }
    }

}
