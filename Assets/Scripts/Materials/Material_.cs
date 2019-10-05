using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MaterialType { wood, glass, metal }


public class Material_ : MonoBehaviour
{

    Rigidbody parentRigidbody;
    Rigidbody rb;
    GravityBody gravityBody;

    public string name;

    public MaterialType type;

    [HideInInspector]public List<GameObject> holdedBy;
    public List<GameObject> heldObjects;

    Collider collider;

    AudioSource audioS;
    public AudioClip breakSound;

    public float collisionResistance;
    public float scatterResistance;

    public float mass;

    public float maxCondition;
    public float condition;
    //public float condition when drops held objects

    
    public bool free;
    


    // -- Init --
    public Collider Init(Rigidbody parentRigidbody) {

        for (int i=0; i< heldObjects.Count; ++i) {
            heldObjects[i].GetComponent<Material_>().holdedBy.Add(gameObject);
        }

        this.parentRigidbody = parentRigidbody;
        audioS = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
        return collider;
    }

    // -- Collision event --
    public void Collision(Collision collision) {
        //Debug.Log(collision.relativeVelocity.magnitude + ", collider: "+gameObject.name);
        if (collision.relativeVelocity.magnitude > collisionResistance) {
            audioS.enabled = true;
            audioS.PlayOneShot(breakSound, 0.7f);
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            for (int i=0; i<heldObjects.Count; ++i) { // Drop held objects
                heldObjects[i].GetComponent<Material_>().HolderDestroyed(gameObject);
            }

            GameObject.Destroy(gameObject, 1f);
        }

    }



    // -- holder destroyed --
    public void HolderDestroyed(GameObject holder) {
        holdedBy.Remove(holder);

        if (!free) {
            // if no more holders get a rigidbody
            if (holdedBy.Count == 0) {
                gameObject.transform.parent = null;
                rb = gameObject.AddComponent<Rigidbody>();
                gravityBody = gameObject.AddComponent<GravityBody>();
                rb.mass = mass;
                parentRigidbody.mass = parentRigidbody.mass - mass;
                free = true;
            }
        }

    }


    // -- On Destroy --
    void OnDestroy() {
        for (int i=0, len=holdedBy.Count; i<len; ++i) {
            holdedBy[i].GetComponent<Material_>().heldObjects.Remove(gameObject);
        }
    }


}
