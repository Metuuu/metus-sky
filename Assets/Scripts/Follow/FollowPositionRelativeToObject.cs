using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositionRelativeToObject : MonoBehaviour {

    public Transform sourceParentObject;
    public Transform sourceTransform;
    public Transform destinationParentObject;
    public Transform transform;
    public Vector3 offset;

    // - Start -
    void Start () {
        transform = gameObject.transform;
    }
    
    // - Update -
    void LateUpdate () {
        transform.position = (sourceTransform.position - sourceParentObject.position) + destinationParentObject.position + offset;
    }


}
