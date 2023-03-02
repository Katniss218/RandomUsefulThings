using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geometry
{
    public class test : MonoBehaviour
    {
        public AABB2D c1;

        public Vector2 p;
        public Vector2 d;

        public bool intersects;
        public bool intersects2;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube( c1.Center, c1.Size );

            intersects2 = c1.Intersects( new Ray2D( p, d ) );

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine( p, p + d );
            Gizmos.DrawSphere( p + d, 0.1f );
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