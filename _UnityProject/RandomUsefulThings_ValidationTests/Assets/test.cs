using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geometry
{
    public class test : MonoBehaviour
    {
        public Triangle2D triangle;

        public Vector2 p;

        public Vector3 baryc;

        public bool intersects;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine( triangle.P1, triangle.P2 );
            Gizmos.DrawLine( triangle.P2, triangle.P3 );
            Gizmos.DrawLine( triangle.P3, triangle.P1 );
            Gizmos.color = new Color( 1.0f, 0.0f, 0.0f );
            Gizmos.DrawSphere( triangle.P1, 0.05f );
            Gizmos.color = new Color( 0.5f, 0.0f, 0.0f );
            Gizmos.DrawSphere( triangle.P2, 0.05f );
            Gizmos.color = new Color( 0.0f, 0.0f, 0.0f );
            Gizmos.DrawSphere( triangle.P3, 0.05f );

            baryc = triangle.BarycentricCoordinates( p );
            float c = Triangle2D.InterpolateTriangle( p, triangle.P1, triangle.P2, triangle.P3, 1.0f, 0.5f, 0.0f );

            Gizmos.color = new Color( c, 0, 0 );
            Gizmos.DrawSphere( p, 0.1f );
            /*c = Geometry.Circle.FromThreePoints( v1, v2, v3 );

            Gizmos.color = Color.red;
            Gizmos.DrawSphere( v1, 0.1f );

            Gizmos.color = Color.green;
            Gizmos.DrawSphere( v2, 0.1f );

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere( v3, 0.1f );

            Gizmos.color = Color.white;
            if( c != null )
            {
                Gizmos.DrawSphere( c.Value.Center, c.Value.Radius );
            }*/
        }
    }
}