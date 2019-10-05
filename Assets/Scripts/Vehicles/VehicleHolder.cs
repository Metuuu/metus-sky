using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHolder : MonoBehaviour {

    #region [ - Init - ]

    // Parts
    Transform transform;

    bool insideRigidbody;
    Rigidbody rootRigidbody; // if inside vehicle

    // Allowed Vehicles
    public Vehicle.VehicleType[] allowedVehicleTypes;

    // Settings
    public bool mounted;
    public bool rotateAfterMounted;
    public bool rotated;
    public LayerMask m_HolderColliderLayer;
    public Vector3 m_Position;
    public Vector3 m_halfExtents; // Half of the size of the box in each dimension
    public float m_Force;

    // Sound
    public AudioClip mountSound;
    public AudioClip unmountSound;

    // Other
    Collider[] colliders;
    Vehicle mountedVehicle;
    
    #endregion


    // --- Start ---
    void Start() {
        transform = gameObject.transform;
        rootRigidbody = transform.root.GetComponent<Rigidbody>();
        if (rootRigidbody != null) {
            insideRigidbody = true;
        }
    }

    // --- Fixed Update ---
    void FixedUpdate() {
        
        if (!mounted)
        {
            colliders = Physics.OverlapBox(transform.position + m_Position, m_halfExtents, new Quaternion(), m_HolderColliderLayer);

            foreach (Collider collider in colliders) {

                mountedVehicle = collider.transform.parent.GetComponent<Vehicle>();

                for (int i=0, len=allowedVehicleTypes.Length; i<len; ++i) {
                    if (allowedVehicleTypes[i].type == mountedVehicle.vehicleType.type && allowedVehicleTypes[i].size == mountedVehicle.vehicleType.size) {
                        if (!mountedVehicle.engineOn) {
                            //PullTowards(vehicle.rigidbody);
                            mountedVehicle.MountToHolder(transform);
                            mounted = true;
                            break;
                        }
                    }
                }
                if (mounted) {
                    break;
                }
                      
            }

        }
        else {
            if (mountedVehicle.engineOn) {
                mountedVehicle.UnmountFromHolder();
                //IgnoreCollisionsWithObject(false);
                mounted = false;
            }
        }


    }

    // --- On Draw Gizmos ---
    /*void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position + m_Position, m_halfExtents*2);
    }*/




    // Pull Vehicle Towards (TODO: Että kulkis smoothisti loksahtaen  paikkaan)
    void PullTowards(Rigidbody rb) {
        //rb.AddExplosionForce(m_Force * -1, transform.position + m_Position, m_Radius);
    }

    


    // Ignore collision with mounted objects TÄLLÄ EI TEEKKÄÄN MITÄÄN
    void IgnoreCollisionsWithObject(bool ignoreCollision) {

        if (insideRigidbody) { // only needed if the vehicle holder is inside a rigidbody

            Component[] myColliders = rootRigidbody.GetComponentsInChildren(typeof(Collider)); // Mount
            Component[] colliders = mountedVehicle.gameObj.GetComponentsInChildren(typeof(Collider)); // Mounted vehicle
            foreach (Collider myCollider in myColliders) {
                Debug.Log(myCollider.name + ", " + ignoreCollision);
                foreach (Collider collider in colliders) {
                    Physics.IgnoreCollision(myCollider, collider, ignoreCollision);
                }
            }

            if (ignoreCollision) {
                // Enable collisions with player // olijoskus T OD  O: kehitä paremmaksi ettei tartteis erikseen playerin collidereja enablettaa
                colliders = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren(typeof(Collider)); // Mounted vehicle
                foreach (Collider myCollider in myColliders) {
                    foreach (Collider collider in colliders) {
                        Physics.IgnoreCollision(myCollider, collider, false);
                    }

                }
            }

        }

    }



}