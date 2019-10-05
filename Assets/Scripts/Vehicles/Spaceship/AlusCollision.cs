using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlusCollision : MonoBehaviour
{

    public LayerMask fcCastMask;
    RaycastHit fcHit;
    Rigidbody rb;

    Vector3 oldPos;

    void Start() {
        rb = GetComponent<Rigidbody>();
        oldPos = transform.position;
    }

    void Update() {

        //Move the object forward here...
        if (Physics.Linecast(oldPos, transform.position, out fcHit, fcCastMask)) {
            if (fcHit.transform != transform) {

                //Debug.Log(fcHit.transform);

                transform.position = fcHit.point;


                // HIT
                rb.velocity = Vector3.zero;


            }
        }
        oldPos = transform.position;
    }



    // - On collision enter -
    void OnCollisionEnter(Collision col) {

        // Kevyeisiin törmäys
        /*Rigidbody colRb = col.gameObject.GetComponent<Rigidbody>();
        if (colRb) {
            if (rb.mass / colRb.mass > 100f) {
                //colRb.velocity = (colRb.mass - rb.mass) / (colRb.mass + rb.mass) * rb.velocity;
                //vel = col.relativeVelocity;
                rb.velocity = (rb.mass * 2f) / (rb.mass + colRb.mass) * rb.velocity;
                return;
            }
        }*/

    }


}
