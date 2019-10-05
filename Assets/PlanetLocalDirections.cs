using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLocalDirections : MonoBehaviour {


    #region [ - Init - ]

    // Classes
    Rigidbody rb;
    GravityBody.LocalGlobalUp localGlobalUp;
    Transform transform;

    // Directions
    public Vector3 up;
    public Vector3 right;
    public Vector3 forward;

    // Velocities
    public bool calculateVelocities;
    public Vector3 upVel;
    public Vector3 rightVel;
    public Vector3 forwardVel;
    public Vector3 upAngVel;
    public Vector3 rightAngVel;
    public Vector3 forwardAngVel;

    // Magnitudes (näillä kaikilla on myös negatiivinen suunta)
    public bool calculateMagnitudes;
    public float upMag; 
    public float rightMag;
    public float forwardMag;
    public float forwardAngMag;
    public float rightAngMag;
    public float upAngMag;

    // Debug
    public bool drawDebugLines;
    
    // Other
    Vector3 velocityPoint;
    bool magnitudeNegative;

    #endregion


    // - Start -
    void Start() {
        rb = GetComponent<Rigidbody>();
        GravityBody gb = GetComponent<GravityBody>();
        if (gb == null) {
            localGlobalUp = GetComponent<GravityBody>().LGUp;
        } else {
            localGlobalUp = gb.LGUp;
        }
        transform = gameObject.transform;
    }

    
    // - Update -
    void Update() {
        
        #region [ - Directions - ]
        up = localGlobalUp.value;

        float Xangle = 360 + (MathfCustom.AngleSigned(up, transform.forward, transform.right) - 180);
        if (Xangle >= 270) {
            right = Vector3.Cross(up, transform.forward).normalized;
            forward = Vector3.Cross(right, up).normalized;
        }
        else {
            forward = Vector3.Cross(transform.right, up).normalized;
            right = Vector3.Cross(up, forward).normalized;
        }
        #endregion


        #region [ - Velocities - ]
        if (calculateVelocities) {

            upVel = Vector3.Project(rb.velocity, up);
            rightVel = Vector3.Project(rb.velocity, right);
            forwardVel = Vector3.Project(rb.velocity, forward);

            upAngVel = Vector3.Project(rb.angularVelocity, up);
            rightAngVel = Vector3.Project(rb.angularVelocity, right);
            forwardAngVel = Vector3.Project(rb.angularVelocity, forward);

        }
        #endregion


        #region [ - Magnitudes - ]
        if (calculateMagnitudes) {

            upMag = upVel.magnitude;
            velocityPoint = rb.velocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, up) > Vector3.Distance(Vector3.zero, up);
            if (magnitudeNegative) {
                upMag *= -1;
            }

            rightMag = upVel.magnitude;
            velocityPoint = rb.velocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, right) > Vector3.Distance(Vector3.zero, right);
            if (magnitudeNegative) {
                rightMag *= -1;
            }

            forwardMag = forwardVel.magnitude;
            velocityPoint = rb.velocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, forward) > Vector3.Distance(Vector3.zero, forward);
            if (magnitudeNegative) {
                forwardMag *= -1;
            }

            forwardAngMag = forwardAngVel.magnitude;
            velocityPoint = rb.angularVelocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, forward) < Vector3.Distance(velocityPoint, -forward);
            if (magnitudeNegative) {
                forwardAngMag *= -1;
            }

            upAngMag = upAngVel.magnitude;
            velocityPoint = rb.angularVelocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, up) < Vector3.Distance(velocityPoint, -up);
            if (magnitudeNegative) {
                upAngMag *= -1;
            }

            rightAngMag = rightAngVel.magnitude;
            velocityPoint = rb.angularVelocity.normalized;
            magnitudeNegative = Vector3.Distance(velocityPoint, right) < Vector3.Distance(velocityPoint, -right);
            if (magnitudeNegative) {
                rightAngMag *= -1;
            }

        }
        #endregion


        #region [ - Debug - ]
        if (drawDebugLines) {
            Debug.DrawLine(transform.position, transform.position + up, Color.green);
            Debug.DrawLine(transform.position, transform.position + right, Color.red);
            Debug.DrawLine(transform.position, transform.position + forward, Color.blue);
        }
        #endregion
        
    }



    #region [ - Convert - ]

    /// <summary> Rerturns velocities in array {Right, Up, Forward} </summary>
    public Vector3[] ConvertToLocalVelocities(Vector3 velocity) {
        return new Vector3[] { Vector3.Project(velocity, right), Vector3.Project(velocity, up), Vector3.Project(velocity, forward) };
    }


    #endregion



    // --- On Enable ---
    void OnEnable() {
        rb = Character.RigidBody;
    }


}
