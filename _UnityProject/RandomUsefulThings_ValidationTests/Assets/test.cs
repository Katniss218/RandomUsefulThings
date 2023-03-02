using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Geometry.Vector2 v1;
    public Geometry.Vector2 v2;
    public Geometry.Vector2 v3;
    public  Geometry.Vector2 v4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        v4 = Geometry.Line2D.ClosestPointOnLine( v1, v2, v3 );
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( v1, 0.1f );

        Gizmos.color = Color.green;
        Gizmos.DrawLine( v2, v3 );

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( v4, 0.1f );
    }
}
