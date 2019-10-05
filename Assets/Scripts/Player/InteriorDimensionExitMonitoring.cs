using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorDimensionExitMonitoring : MonoBehaviour {


    int interiorDimColliderCounter = 0;
    bool changedLastFrame = true;


    // - OnTriggerEnter -
    void OnTriggerEnter(Collider col) {
        if (col.tag == "InteriorDimension") {
            ++interiorDimColliderCounter;
            changedLastFrame = true;
        }

    }

    // - OnTriggerExit -
    void OnTriggerExit(Collider col) {
        if (col.tag == "InteriorDimension") {
            --interiorDimColliderCounter;
            changedLastFrame = true;
        }

    }

    // - Late Update -
    void FixedUpdate() {

        if (changedLastFrame == false) {
            if (interiorDimColliderCounter == 0) {
                Character.PlayerControllerScript.LeaveInteriorDimension();
            }
        }
        
        changedLastFrame = false;


    }



}
