using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Character {

    #region [ - variables - ]

    // Data
    private static string name;
    private static float mass;

    // Parts
    private static GameObject gameObject;
    private static Transform transform;
    private static Rigidbody rb;
    private static Collider collider;
    private static MeshRenderer meshRenderer;
    private static GameObject cameras;
    private static GameObject characterHUD;
    private static GameObject pippeli;

    // Scripts
    private static PlayerController playerControllerScr;
    private static Movement movementScr;
    private static GravityBody gravityBodyScr;
    private static PlanetLocalDirections planetLocalDirections;
    private static ObjectInteractions objectInteractionsScr;

    // Status
    private static bool inSpace;
    private static bool inInteriorDimension;
    private static bool falling;
    private static bool stunned;
    
    // Information
    public static Transform closestPlanet;

    // Other
    public static RigidbodyData rigidbodyData = new RigidbodyData();

    #endregion
    
    #region [ - Getters and Setters - ]

    // Data
    public static string Name {
        get { return name; }
    }

    // Parts
    public static GameObject GameObject {
        get { return gameObject; }
    }
    public static Transform Transform {
        get { return transform; }
    }
    public static Rigidbody RigidBody {
        get { return rb; }
    }
    public static Collider Collider {
        get { return collider; }
    }
    public static MeshRenderer MeshRenderer {
        get { return meshRenderer; }
    }
    public static GameObject Cameras {
        get { return cameras; }
    }
    public static GameObject CharacterHUD {
        get { return characterHUD; }
    }
    public static GameObject Pippeli {
        get { return pippeli; }
    }

    // Scripts
    public static PlayerController PlayerControllerScript {
        get { return playerControllerScr; }
    }
    public static Movement MovementScript {
        get { return movementScr; }
    }
    public static GravityBody GravityBodyScript {
        get { return gravityBodyScr; }
    }
    public static PlanetLocalDirections PlanetLocalDirections {
        get { return planetLocalDirections; }
    }
    public static ObjectInteractions ObjectInteractionsScript {
        get { return objectInteractionsScr; }
    }

    // Status
    public static bool InSpace {
        get { return inSpace; }
        set { inSpace = value; }
    }
    public static bool IsStunned {
        get { return stunned; }
        set { stunned = value; }
    }
    public static bool IsFalling {
        get { return falling; }
        set { falling = value; }
    }
    public static bool InInteriorDimension {
        get { return inInteriorDimension; }
        set { inInteriorDimension = value; }
    }


    #endregion


    // - Init -
    public static void CharacterInit(GameObject player, GameObject characterHud) {

        // Data
        name = "Metu";
        mass = 7;

        // Parts
        gameObject = player;
        transform = gameObject.transform;
        rb = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        cameras = transform.Find("Cameras").gameObject;
        pippeli = transform.Find("UrineParticleSys").gameObject;
        characterHUD = characterHud;

        // Scripts
        playerControllerScr = gameObject.GetComponent<PlayerController>();
        movementScr = gameObject.GetComponent<Movement>();
        gravityBodyScr = gameObject.GetComponent<GravityBody>();
        planetLocalDirections = gameObject.GetComponent<PlanetLocalDirections>();
        objectInteractionsScr = gameObject.GetComponent<ObjectInteractions>();

    }

    
    // Set Active
    public static void SetActive(bool active) {
        if (active) {
            gameObject.AddComponent<Rigidbody>();
            rb = gameObject.GetComponent<Rigidbody>();
            rigidbodyData.restoreRigidbody(rb);
            //playerControllerScr.rb = rb;
            Cameras.SetActive(true);
            CharacterHUD.SetActive(true);
            movementScr.enabled = true;
            gravityBodyScr.enabled = true;
            planetLocalDirections.enabled = true;
            objectInteractionsScr.enabled = true;
            collider.enabled = true;
            MeshRenderer.enabled = true;
        }
        else {
            Cameras.SetActive(false);
            CharacterHUD.SetActive(false);
            movementScr.enabled = false;
            gravityBodyScr.enabled = false;
            PlanetLocalDirections.enabled = false;
            objectInteractionsScr.enabled = false;
            collider.enabled = false;
            MeshRenderer.enabled = false;
            rigidbodyData.saveRigidbody(rb);
            GameObject.Destroy(rb);
        }

    }
    

}
