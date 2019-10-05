using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takaovi : MonoBehaviour {

    // [ - Init - ]
    Transform myTransform;
    public GameObject takaoviAuki;
    bool closed = true;
    public AudioClip doorSound;



    // --- Start ---
    void Start () {
        myTransform = transform;
        takaoviAuki.SetActive(false);
    }
    
    // --- Update ---
    void Update () {
        
    }



    // [ - Events - ]

    // Toggle door
    public void ToggleDoor() {
        closed = !closed;
        if (closed) {
            myTransform.GetComponent<BoxCollider>().enabled = true;
            myTransform.GetComponent<MeshRenderer>().enabled = true;
            takaoviAuki.SetActive(false);

        }
        else {
            myTransform.GetComponent<BoxCollider>().enabled = false;
            myTransform.GetComponent<MeshRenderer>().enabled = false;
            takaoviAuki.SetActive(true);
        }
    }


}
