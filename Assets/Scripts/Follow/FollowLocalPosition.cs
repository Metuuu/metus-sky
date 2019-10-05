using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocalPosition : MonoBehaviour {

    Transform transform;
    public Transform obj;

    // - Start -
    void Start() {
        transform = gameObject.transform;
    }

    // - Update -
    void FixedUpdate() {
        transform.localPosition = obj.localPosition;
    }


}
