using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialStructure : MonoBehaviour {

    Material_[] materials;
    Collider[] colliders;
    Vehicle vehicle;


    // - Start -
    void Awake() {
        vehicle = GetComponent<Vehicle>();
        materials = GetComponentsInChildren<Material_>();
        int len = materials.Length;
        colliders = new Collider[len];
        for (int i=0; i<len; ++i) {
            colliders[i] = materials[i].Init(vehicle.rigidbody);
        }
    }
    
    
    // - Collision -
    void OnCollisionEnter(Collision collision) {
        for (int i=0, len=colliders.Length; i<len; ++i) {
            for (int j = 0; j<collision.contacts.Length; ++j) {
                if (collision.contacts[j].thisCollider == colliders[i]) {
                    materials[i].Collision(collision);
                    return;
                }
            }
            
        }
    }



}
