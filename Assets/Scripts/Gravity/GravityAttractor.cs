using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
    
    // - Init -
    Transform myTransform;
    public float gravity;

    // älä rasita turhaan garbage collectoria
    Vector3 heading;
    float distance;
    Vector3 direction;

	
    // - Start -
    private void Awake() {
        myTransform = transform;
    }



    // - Attract object -
    public void Attract(float gravityAcceleration, Rigidbody rb, ref Vector3 localGlobalUp, bool freezeRotation, float rotatingSpeed, Vector3 up, bool closestPlanet) {
        heading = myTransform.position - rb.position;
        distance = heading.magnitude;
        direction = heading / distance;
        
        rb.AddForce(direction * gravityAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);


        // Freeze Rotation
        if (closestPlanet) {
            localGlobalUp = -direction.normalized;
        
            if (freezeRotation) {
                Quaternion targetRotation = Quaternion.FromToRotation(-up, direction) * rb.rotation;
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotatingSpeed * Time.deltaTime);
            }
        }


    }


}