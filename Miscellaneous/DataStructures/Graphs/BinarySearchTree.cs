using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Graphs
{
    public class BinarySearchTree
    {
        public class Node
        {
            public int data;
            public Node left, right;

            public Node( int item )
            {
                data = item;
                left = right = null;
            }
        }
        Node root;

        public BinarySearchTree()
        {
            root = null;
        }

        public void Insert( int data )
        {
            root = InsertRec( root, data );
        }

        private Node InsertRec( Node root, int data )
        {
            if( root == null )
            {
                root = new Node( data );
                return root;
            }

            if( data < root.data )
                root.left = InsertRec( root.left, data );
            else if( data > root.data )
                root.right = InsertRec( root.right, data );

            return root;
        }

        /// <summary>
        /// Prints the values in order.
        /// </summary>
        public void Inorder()
        {
            InorderRec( root );
        }

        private void InorderRec( Node root )
        {
            // Inorder traversal - left, this, right
            if( root != null )
            {
                InorderRec( root.left );
                Console.Write( root.data + " " );
                InorderRec( root.right );
            }
        }
    }


    public class SelfBalancingBinarySearchTree
    {
        public class Node
        {
            public int data;
            public Node left, right;

            public Node( int item )
            {
                data = item;
                left = right = null;
            }
        }
        Node root;

        public SelfBalancingBinarySearchTree()
        {
            root = null;
        }

        public void Insert( int data )
        {
            root = InsertRec( root, data );
        }

        private Node InsertRec( Node root, int data )
        {
            if( root == null )
            {
                root = new Node( data );
                return root;
            }

            if( data < root.data )
                root.left = InsertRec( root.left, data );
            else if( data > root.data )
                root.right = InsertRec( root.right, data );

            int balance = GetBalance( root );

            // If the left subtree is heavier than the right subtree
            if( balance > 1 )
            {
                // If the new data is less than the data in the left child
                // perform a right rotation to balance the tree
                if( data < root.left.data )
                {
                    root = RotateRight( root );
                }
                // If the new data is greater than the data in the left child
                // perform a left-right rotation to balance the tree
                else
                {
                    root.left = RotateLeft( root.left );
                    root = RotateRight( root );
                }
            }
            // If the right subtree is heavier than the left subtree
            else if( balance < -1 )
            {
                // If the new data is greater than the data in the right child
                // perform a left rotation to balance the tree
                if( data > root.right.data )
                {
                    root = RotateLeft( root );
                }
                // If the new data is less than the data in the right child
                // perform a right-left rotation to balance the tree
                else
                {
                    root.right = RotateRight( root.right );
                    root = RotateLeft( root );
                }
            }

            return root;
        }

        private int GetBalance( Node root )
        {
            if( root == null )
                return 0;

            return Height( root.left ) - Height( root.right );
        }

        private int Height( Node root )
        {
            if( root == null )
                return 0;

            return 1 + System.Math.Max( Height( root.left ), Height( root.right ) );
        }

        private Node RotateRight( Node root )
        {
            Node newRoot = root.left;
            root.left = newRoot.right;
            newRoot.right = root;
            return newRoot;
        }

        private Node RotateLeft( Node root )
        {
            Node newRoot = root.right;
            root.right = newRoot.left;
            newRoot.left = root;
            return newRoot;
        }

        public void Inorder()
        {
            InorderRec( root );
        }

        private void InorderRec( Node root )
        {
            if( root != null )
            {
                InorderRec( root.left );
                Console.Write( root.data + " " );
                InorderRec( root.right );
            }
        }
    }



    public class AVLTree<T> where T : IComparable<T>
    {
        // Adelson-Velsky and Landis
        // it is a self-balancing tree.

        private class Node
        {
            public T data;
            public int height;
            public Node left, right;

            public Node( T item )
            {
                data = item;
                height = 1;
                left = right = null;
            }
        }

        private Node root;

        public AVLTree()
        {
            root = null;
        }

        private int Height( Node node )
        {
            if( node == null )
                return 0;
            return node.height;
        }

        private int Max( int a, int b )
        {
            return (a > b) ? a : b;
        }

        private int GetBalance( Node node )
        {
            if( node == null )
                return 0;
            return Height( node.left ) - Height( node.right );
        }

        private Node RotateRight( Node y )
        {
            Node x = y.left;
            Node T2 = x.right;

            x.right = y;
            y.left = T2;

            y.height = Max( Height( y.left ), Height( y.right ) ) + 1;
            x.height = Max( Height( x.left ), Height( x.right ) ) + 1;

            return x;
        }

        private Node RotateLeft( Node x )
        {
            Node y = x.right;
            Node T2 = y.left;

            y.left = x;
            x.right = T2;

            x.height = Max( Height( x.left ), Height( x.right ) ) + 1;
            y.height = Max( Height( y.left ), Height( y.right ) ) + 1;

            return y;
        }

        public void Insert( T data )
        {
            root = InsertRec( root, data );
        }

        private Node InsertRec( Node node, T data )
        {
            if( node == null )
                return new Node( data );

            if( data.CompareTo( node.data ) < 0 )
                node.left = InsertRec( node.left, data );
            else if( data.CompareTo( node.data ) > 0 )
                node.right = InsertRec( node.right, data );
            else
                return node;

            node.height = 1 + Max( Height( node.left ), Height( node.right ) );

            int balance = GetBalance( node );

            if( balance > 1 && data.CompareTo( node.left.data ) < 0 )
                return RotateRight( node );

            if( balance < -1 && data.CompareTo( node.right.data ) > 0 )
                return RotateLeft( node );

            if( balance > 1 && data.CompareTo( node.left.data ) > 0 )
            {
                node.left = RotateLeft( node.left );
                return RotateRight( node );
            }

            if( balance < -1 && data.CompareTo( node.right.data ) < 0 )
            {
                node.right = RotateRight( node.right );
                return RotateLeft( node );
            }

            return node;
        }

        [Obsolete( "Not sure if this works right" )]
        public void Delete( T data )
        {
            root = DeleteRec( root, data );
        }

        [Obsolete( "Not sure if this works right" )]
        private Node DeleteRec( Node node, T data )
        {
            if( node == null )
                return node;

            if( data.CompareTo( node.data ) < 0 )
                node.left = DeleteRec( node.left, data );
            else if( data.CompareTo( node.data ) > 0 )
                node.right = DeleteRec( node.right, data );
            else
            {
                if( (node.left == null) || (node.right == null) )
                {
                    Node temp = null;
                    if( temp == node.left )
                        temp = node.right;
                    else
                        temp = node.left;

                    if( temp == null )
                    {
                        temp = node;
                        node = null;
                    }
                    else
                        node = temp;
                }
                else
                {
                    Node temp = MinValueNode( node.right );

                    node.data = temp.data;

                    node.right = DeleteRec( node.right, temp.data );
                }
            }

            if( node == null )
                return node;

            node.height = 1 + Max( Height( node.left ), Height( node.right ) );

            int balance = GetBalance( node );

            if( balance > 1 && GetBalance( node.left ) >= 0 )
                return RotateRight( node );

            if( balance > 1 && GetBalance( node.left ) < 0 )
            {
                node.left = RotateLeft( node.left );
                return RotateRight( node );
            }

            if( balance < -1 && GetBalance( node.right ) <= 0 )
                return RotateLeft( node );

            if( balance < -1 && GetBalance( node.right ) > 0 )
            {
                node.right = RotateRight( node.right );
                return RotateLeft( node );
            }

            return node;
        }

        private Node MinValueNode( Node node )
        {
            Node current = node;

            while( current.left != null )
                current = current.left;

            return current;
        }

        public void Inorder()
        {
            InorderRec( root );
        }

        private void InorderRec( Node node )
        {
            if( node != null )
            {
                InorderRec( node.left );
                Console.Write( node.data + " " );
                InorderRec( node.right );
            }
        }
    }

}
