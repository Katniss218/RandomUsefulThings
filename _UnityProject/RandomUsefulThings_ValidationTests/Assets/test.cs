using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Vector3 line1P;
    public Vector3 line1Dir;
    public Vector3 line2P;
    public Vector3 line2Dir;

    public Vector3 projDir;


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
        //vp = Geometry.Vector2.Reflection( v1, v2 );
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( line1P, 0.1f );
        Gizmos.DrawLine( line1P - line1Dir * 10, line1P + line1Dir * 10 );

        Gizmos.color = Color.green;
        Gizmos.DrawSphere( line2P, 0.1f );
        Gizmos.DrawLine( line2P - line2Dir * 10, line2P + line2Dir * 10 );

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine( line2P, line2P + projDir );
        Vector3? intersection = Geometry.Line3D.ProjectedLineIntersection( line1P, line1Dir, line2P, line2Dir, projDir );
        // angle = Geometry.Vector2.Angle( v1, v2 ) * Mathf.Rad2Deg;

        if( intersection != null )
        {
            Gizmos.DrawSphere( intersection.Value, 0.1f );
            Gizmos.DrawLine( intersection.Value, intersection.Value - projDir * 10 );
        }
        else
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere( line1P, 0.05f );
        }
    }
}
