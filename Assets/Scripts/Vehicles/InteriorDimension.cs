using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorDimension : MonoBehaviour {

    public GameObject interiorDimGameObject;
    public Transform transform;
    public Transform interiorDimTransform;
    public Vector3 posDifference;

    // - Start -
    void Start() {
        transform = gameObject.transform.root;
    }



    // - Enter -
    public void Enter() {

    }


    // - Exit -
    public void Exit() {

    }



}
