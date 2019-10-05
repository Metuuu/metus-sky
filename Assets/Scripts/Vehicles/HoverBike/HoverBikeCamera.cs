using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverBikeCamera : MonoBehaviour {

    // - Enums -
    public enum view { firstPerson, thirdPerson }

    // - Init -
    Transform myTransform;
    public Transform target;
    GravityBody gravBod;

    public view viewType = view.firstPerson;

    public float height;
    public float distance;
    Vector3 camOffset;
    

    // - Start -
    void Start () {
        myTransform = transform;
        gravBod = target.GetComponent<GravityBody>();
    }
    
    
    // - Update -
    void Update () {
        
        camOffset = new Vector3(target.position.x, target.position.y, target.position.z) + target.up * height - target.forward * distance;
        myTransform.position = camOffset;

        /* shittiä ...... en osannu tehä smooth rotation sivuttais törmäilylle
        middleRot = Vector3.Angle(Vector3.up, -gravBod.localGlobalUp);
        Debug.Log(Vector3.Angle(Vector3.up, gravBod.localGlobalUp) - Vector3.Angle(Vector3.up, target.up));

        if (Vector3.Angle(Vector3.up, -gravBod.localGlobalUp) < Vector3.Angle(Vector3.up, target.up)) {
            //Debug.Log(Vector3.Angle(Vector3.up, target.up));
        } else {
            //Debug.Log(-Vector3.Angle(Vector3.up, target.up));
        }
        */

        myTransform.rotation = target.rotation;


    }


}
