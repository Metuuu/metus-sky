using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour {

    public Transform obj;
    Transform transform;

    // - Start -
    void Start () {
        transform = gameObject.transform;
    }
    
    // - Update -
    void LateUpdate() {
        transform.rotation = obj.rotation;
    }


}
