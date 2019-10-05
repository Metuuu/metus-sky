using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vehicle : MonoBehaviour {

    // Parts
    public GameObject gameObj;
    [HideInInspector] public Transform transform;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public GravityBody gravityBody;
    GameObject cameras;
    Transform characterSpot;
    Vector3 mountPoint;

    // Data
    public string name;

    public VehicleType vehicleType;

    public bool hasInteriorDimension;
    public bool mountedToHolder;
    public bool inUse;
    public bool canLeaveVehicle;
    public bool engineOn;

    // Other
    Transform character;// = Character.Transform;
    RigidbodyData rigidbodyData = new RigidbodyData();
    public MonoBehaviour[] disabledOnMount; // ---------------------- jatka tästä
    public Vector3 velocity;
    public Vector3 velocityChange;

    Transform mountedTransform;

    bool lerpToHolderPosition;



    // --- Awake ---
    void Awake() {
        gameObj = gameObject;
        transform = gameObj.transform;
        rigidbody = transform.GetComponent<Rigidbody>();
        cameras = transform.Find("Cameras").gameObject;
        characterSpot = transform.Find("CharacterSpot");
        gravityBody = transform.GetComponent<GravityBody>();
        mountPoint = transform.position - transform.Find("HolderCollider").Find("CenterMountPoint").position;
    }


    // --- Fixed Update ---
    void FixedUpdate() {
        if (engineOn) {
            velocity = rigidbody.velocity; // (velocity before collision)
        }
    }

    // --- Update ---
    void Update() {
        if (engineOn) {
            velocityChange = velocity - rigidbody.velocity;
            //Debug.Log(velocityChange);
        }

        if (lerpToHolderPosition) {
            if (Vector3.Distance(mountedTransform.localPosition, Vector3.zero) < 0.01f) {// && mountedTransform.localRotation == new Quaternion()) {
                lerpToHolderPosition = false;
            }
            else {
                LerpToHolderPosition(Time.fixedDeltaTime);

            }
        }

    }


    // Hop in
    public void EnterVehicle() {
        if (Character.InInteriorDimension) {
            Character.PlayerControllerScript.LeaveInteriorDimension();
        }
        cameras.SetActive(true);
        inUse = true;
        Character.Transform.parent = characterSpot;
        Character.Transform.localPosition = Vector3.zero;
        Character.Transform.localRotation = new Quaternion();
        Character.Cameras.transform.localRotation = new Quaternion();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Hop off
    public void LeaveVehicle() {
        canLeaveVehicle = false;
        cameras.SetActive(false);
        inUse = false;
        Character.Transform.parent = null;
        Character.PlayerControllerScript.LeaveVehicle(velocity);
        if (hasInteriorDimension) {
            Character.PlayerControllerScript.EnterInteriorDimension();
            Character.RigidBody.velocity = Vector3.zero;
            Character.RigidBody.angularVelocity = Vector3.zero;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }


    /*
    // Start the engine
    public void StartTheEngine() {
        audioScr.PlayEngineStartSound();
        light.SetActive(true);
        engineOn = true;
    }

    // Stop the engine
    public void StopTheEngine() {
        audioScr.PlayEngineStopSound();
        light.SetActive(false);
        engineOn = false;
    }*/

    

    #region [ - Mounting to holder - ]

    // Mount Vehicle To Holder
    public void MountToHolder(Transform holderT) {

        //transform.position = holderT.position + mountPoint;
        
        mountedTransform = new GameObject(transform.name + "NoRB").transform;
        mountedTransform.position = transform.position;
        mountedTransform.rotation = transform.rotation;

        Other.MoveChilds(transform, mountedTransform);

        mountedTransform.parent = holderT;
        //mountedTransform.localRotation = Quaternion.Euler(new Vector3(0, mountedTransform.localRotation.eulerAngles.y, 0));

        MountObjects_Enable(false);

        transform.parent = mountedTransform;
        rigidbody.isKinematic = true;
        lerpToHolderPosition = true;
        
    }

    // Unmount Vehicle From Holder
    public void UnmountFromHolder() {

        lerpToHolderPosition = false;

        MountObjects_Enable(true);
        rigidbody.isKinematic = false;
        transform.parent = null;

        transform.position = mountedTransform.position;
        transform.rotation = mountedTransform.rotation;

        Other.MoveChilds(mountedTransform, transform);

        Destroy(mountedTransform.gameObject);

    }

    

    // - Lerp to holder rotation and position -
    void LerpToHolderPosition(float t) {
        mountedTransform.localPosition = Vector3.Lerp(mountedTransform.localPosition, Vector3.zero, t);
        mountedTransform.localRotation = Quaternion.Lerp(mountedTransform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), t);
    }

    #endregion




    // Enable objects
    void MountObjects_Enable(bool enable) {
        for (int i = 0, len = disabledOnMount.Length; i < len; ++i) {
            if (disabledOnMount[i].GetType() == typeof(GameObject)) {
                disabledOnMount[i].gameObject.SetActive(enable);
            } else {
                disabledOnMount[i].enabled = enable;
            }
        }

    }





    #region [ - Child Classes - ]

    // - Vehicle Type -
    [System.Serializable]
    public class VehicleType {
        
        // Enums
        public enum Type { Bike, Car, Spaceship }
        public enum Size { Small, Medium, Large }

        // Data
        public Type type;
        public Size size;

        VehicleType(Type type, Size size) {
            this.type = type;
            this.size = size;
        }

    }

    #endregion


}