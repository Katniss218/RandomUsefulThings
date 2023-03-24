using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 angVel = Vector3.zero;
    /*void Update()
    {

        if( Input.GetMouseButton( 0 ) )
        {
            float mouseX = Input.GetAxis( "Mouse X" );

            angVel = Quaternion.AngleAxis( -mouseX * 10 * Time.deltaTime, this.transform.up ) * angVel;
        }

        angVel.ToAngleAxis( out float angle, out Vector3 axis );

        this.transform.rotation = angVel * this.transform.rotation;
        // this.transform.RotateAround( this.transform.position, axis, angle * Time.fixedDeltaTime );
        Debug.Log( (this.transform.rotation.eulerAngles.y - prevRot.eulerAngles.y) + ", " + (angle) + ", " + Time.deltaTime );

        angle *= 0.95f;
        angVel = Quaternion.AngleAxis( angle, axis );

        prevRot = this.transform.rotation;
    }*/

    Quaternion prevRot = Quaternion.identity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Input.GetMouseButton( 0 ) )
        {
            float mouseX = Input.GetAxis( "Mouse X" );
            float mouseY = Input.GetAxis( "Mouse Y" );

            Vector3 impulse = this.transform.up * (mouseX * 40);
            impulse += this.transform.right * (-mouseY * 40);

            angVel = impulse + angVel;
        }
        if( angVel == Vector3.zero )
        {
            return;
        }

        Quaternion deltaRot = Quaternion.AngleAxis( angVel.magnitude * Time.fixedDeltaTime, angVel.normalized );

        this.transform.rotation = deltaRot * this.transform.rotation;
       // this.transform.RotateAround( this.transform.position, axis, angle * Time.fixedDeltaTime );

        Debug.Log( (this.transform.rotation.eulerAngles.y - prevRot.eulerAngles.y) + " ---- " + angVel.magnitude );

        angVel *= 0.98f;
        if( angVel.sqrMagnitude < 0.01f)
        {
            angVel = Vector3.zero;
        }

        prevRot = this.transform.rotation;
    }
}
