using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Geometry.Circle c;

    public int num = 5;


    private void OnDrawGizmos()
    {
        var points = c.GetPoints( num );
        int i = 0;
        foreach( var point in points )
        {
            Gizmos.color = new Color( (float)i / points.Length, 0.2f, 0.2f );
            Gizmos.DrawSphere( point, 0.1f );
            i++;
        }
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
