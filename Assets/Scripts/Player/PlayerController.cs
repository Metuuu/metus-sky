using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region [ - Init - ]

    // - Classes -
    private Transform transform;
    private PlanetLocalDirections PLDir;
    private PlayerAudio audio;

    private GameObject cameras;
    private Transform camerasT;
    private GameObject characterHUD;

    private MeshRenderer meshRend;
    private Collider collider;

    private Movement movementScr;
    private GravityBody gravityBodyScr;
    private ObjectInteractions objectInteractionsScr;

    private FollowObject bgstarsFollowObjScr;

    // Materials
    private static PhysicMaterial movementPhysicMaterial;
    private static PhysicMaterial fallingPhysicMaterial;
    private static PhysicMaterial spacePhysicMaterial;


    // - Collision Dimension -
    private GameObject realDimensionCharacter;
    private InteriorDimension colDimScript;
    private bool justEnteredCollisionDimension;


    // - Status Effects -
    private float stunTime;

    // Urine
    [SerializeField] private ParticleSystem UrinePS;
    [SerializeField] private Transform UrinePSTransform;
    [SerializeField] private ParticleSystem.ForceOverLifetimeModule UrineFoltModule;
    private float gravitationalConstant = (6.67408f * Mathf.Pow(10, -11) / 10f);
    private float urine = 100;
    [SerializeField] private float urineGainRate;
    [SerializeField] private float peeingRate;


    // - Settings -
    bool lockMouse = true;

    #endregion


    // - Awake -
    void Awake() {
        transform = gameObject.transform;
        Character.CharacterInit(gameObject, GameObject.Find("CharacterHUD"));
        UrineFoltModule = UrinePS.forceOverLifetime;
        UrinePSTransform = UrinePS.transform;
        camerasT = Character.Cameras.transform;
    }

    // - Start -
    void Start() {
        movementScr = Character.MovementScript;
        gravityBodyScr = Character.GravityBodyScript;
        objectInteractionsScr = Character.ObjectInteractionsScript;
        collider = Character.Collider;
        meshRend = Character.MeshRenderer;
        PLDir = GetComponent<PlanetLocalDirections>();
        audio = GetComponent<PlayerAudio>();
        Cursor.lockState = CursorLockMode.Locked;

        // Physics Materials
        movementPhysicMaterial = Resources.Load("PhysicMaterials/PlayerMovementPhysicMat") as PhysicMaterial;
        fallingPhysicMaterial = Resources.Load("PhysicMaterials/PlayerfallingPhysicMat") as PhysicMaterial;
        spacePhysicMaterial = Resources.Load("PhysicMaterials/PlayerSpacePhysicMat") as PhysicMaterial;

        // Listeners
        Character.GravityBodyScript.SpaceEntered += OnSpaceEntered;
        Character.GravityBodyScript.SpaceLeaved += OnSpaceLeaved;
    }
    
    // - Update -
    void Update() {

        #region [ - Mouse lock - ]
        if (Input.GetKeyDown(KeyCode.Escape)) {
            lockMouse = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            lockMouse = !lockMouse;

            if (lockMouse) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        #endregion

        
        #region [ - Falled - ]
        
        if (!Character.IsFalling) {
            if ((PLDir.rightVel + PLDir.forwardVel).magnitude > 1.5f) {
                Fall();
            }
        }
        else if (!Character.IsStunned) {
            if (Character.RigidBody.velocity.magnitude < 0.14f) { // Recover from fall
                RecoverFromFall();
            }
        }
        #endregion

        #region [ - Stunned - ]
        if (Character.IsStunned) {
            stunTime -= Time.deltaTime;
            if (stunTime <= 0) {
                RecoverFromStun();
            }
        }
        
        #endregion


        #region [ - Urine - ]

        // gaining urine
        if (urine < 100) {
            urine += urineGainRate;
        }

        // pee
        if (Input.GetKey(KeyCode.G)) {
            if (urine > 0) {
                UrinePSTransform.localRotation = Quaternion.Euler(camerasT.localRotation.eulerAngles.x - 20, camerasT.localRotation.eulerAngles.y, camerasT.localRotation.eulerAngles.z);
                float urineLose = peeingRate * Time.deltaTime;
                urine -= urineLose;
				
				PlanetScript planetScript = gravityBodyScr.closestPlanet.GetComponent<PlanetScript>();
				//PlanetTestScript planetScript = gravityBodyScr.closestPlanet.GetComponent<PlanetTestScript>();
				Vector3 peeGrav = (-gravityBodyScr.localGlobalUp * ((gravitationalConstant * planetScript.planetData.mass * Mathf.Pow(10, planetScript.planetData.mass10pow)) / (gravityBodyScr.distanceToClosestPlanet * gravityBodyScr.distanceToClosestPlanet)));

                if (UrinePS.startSpeed < 0.9) {
                    UrinePS.startSpeed += 4 * Time.deltaTime;
                }

                UrineFoltModule.x = peeGrav.x / 30;
                UrineFoltModule.y = peeGrav.y / 30;
                UrineFoltModule.z = peeGrav.z / 30;

                UrinePS.Play();
            }
        }
        else {
            if (UrinePS.startSpeed > 0) {
                UrinePS.startSpeed -= 5 * Time.deltaTime;
            }
            else {
                UrinePS.Stop();
            }
        }

        urine = Mathf.Clamp(urine, 0, 100);

        #endregion


    }

    #region [ - Status Effects - ]

    public void Fall() {
        Character.IsFalling = true;
        Character.RigidBody.constraints = RigidbodyConstraints.None;
        gravityBodyScr.freezeRotation = false;
        collider.material = fallingPhysicMaterial;
    }
    public void RecoverFromFall() {
        Character.IsFalling = false;

        if (!Character.IsStunned) {
            //if (Character.gravityBodyScr.freezeRotation) { // miks tää if lauseke. en tiiä kun pitkästä aikaa katon koodia mut ukko ei noussu ylös tämän takia (liittyykö aluksen sisällä olemiseen)
            Character.RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            //}
            Character.GravityBodyScript.freezeRotation = true;
            collider.material = movementPhysicMaterial;
        }

    }
    public void Stun(float stunTime) {
        Character.IsStunned = true;
		if (this.stunTime + stunTime > 6) {
			this.stunTime = 6;
		} else {
			this.stunTime += stunTime;
		}
    }
    public void RecoverFromStun() {
        Character.IsStunned = false;
        stunTime = 0;
        if (!Character.IsFalling) {
            Character.RigidBody.constraints = RigidbodyConstraints.FreezeRotation; // pitääkö tän olla tässäkin ?
            Character.GravityBodyScript.freezeRotation = true;
            collider.material = movementPhysicMaterial;
        }

    }

    #endregion

    #region [ - Collision events - ]

    // - On collision enter -
    void OnCollisionEnter(Collision col) {

        Vector3[] collisionLocalRelativeVel = PLDir.ConvertToLocalVelocities(col.relativeVelocity);

        //Debug.Log((collisionLocalRelativeVel[1].magnitude));

		// TODO: improve crippled stun
        if (collisionLocalRelativeVel[1].magnitude > 3) {
            Debug.Log(" CRIPPLED - " + collisionLocalRelativeVel[1].magnitude);
            audio.CrippleSound();
            Stun(3);
            Character.RigidBody.AddTorque(PLDir.forward * -30f + PLDir.right * 40f, ForceMode.Impulse);
        }
        
    }

    #endregion


    #region [ - Trigger events - ]

    void OnTriggerEnter(Collider col) {
        if (col.tag == "InteriorDimension") {
            colDimScript = col.GetComponent<InteriorDimension>();
            EnterInteriorDimension();
        }

    }

    #endregion



    #region [ - Vehicle events - ]

    // Enter vehicle
    public void EnterVehicle() {
        Character.SetActive(false);
    }

    // Leave vehicle
    public void LeaveVehicle(Vector3 velocity) {
        Character.SetActive(true);
        Character.RigidBody.velocity = velocity;
        Character.MovementScript.oldPos = transform.position;
    }

    #endregion


    #region [ - Space - ]

    private void OnSpaceEntered() {
        if (Character.InInteriorDimension) {
            gravityBodyScr.gravityEnabled = false;
        }
    }
    private void OnSpaceLeaved() {
        if (Character.InInteriorDimension) {
            gravityBodyScr.gravityEnabled = true;
        }
    }



#endregion


#region [ - Interior Dimension - ]

// Enter
public void EnterInteriorDimension() {

        if (Character.InInteriorDimension) { return; }
        
        Character.InInteriorDimension = true;

        // Create interior dimension
        colDimScript.interiorDimTransform = Instantiate(colDimScript.interiorDimGameObject, Vector3.zero, colDimScript.transform.rotation).transform;
        colDimScript.interiorDimTransform.GetComponent<FollowRotation>().obj = colDimScript.transform;   // <- follow rotation pitäis saaha toimimaan ilman että ukon liikkuminen jumittaa
        colDimScript.interiorDimTransform.rotation = colDimScript.transform.rotation;

        // Real dimension character setup
        realDimensionCharacter = new GameObject("CharacterRealDimension");
        realDimensionCharacter.transform.localScale = transform.localScale;
        realDimensionCharacter.transform.position = transform.position;
        realDimensionCharacter.transform.rotation = transform.rotation;
        realDimensionCharacter.AddComponent<FollowRotation>().obj = transform;
        realDimensionCharacter.AddComponent<InteriorDimensionExitMonitoring>();
        CapsuleCollider capCol = realDimensionCharacter.AddComponent<CapsuleCollider>();
        Other.CopyComponent(transform.GetComponent<CapsuleCollider>(), capCol);
        capCol.isTrigger = true;
        // follow relative position
        FollowPositionRelativeToObject fprto = realDimensionCharacter.AddComponent<FollowPositionRelativeToObject>();
        fprto.sourceParentObject = colDimScript.interiorDimTransform;
        fprto.destinationParentObject = colDimScript.transform;
        fprto.sourceTransform = transform;
        // cameras
        Vector3 locPos = camerasT.localPosition;
        Transform realDimCams = new GameObject("Cameras").transform;
        realDimCams.parent = realDimensionCharacter.transform;
        realDimCams.localPosition = locPos;
        realDimCams.rotation = camerasT.rotation;
        realDimCams.gameObject.AddComponent<FollowRotation>().obj = camerasT;
        realDimCams.gameObject.AddComponent<FollowLocalPosition>().obj = camerasT;
        camerasT.Find("Camera").parent = realDimCams;
        camerasT.Find("FarCamera").parent = realDimCams;
        // pippeli
        locPos = Character.Pippeli.transform.localPosition;
        Character.Pippeli.transform.parent = realDimensionCharacter.transform;
        Character.Pippeli.transform.localPosition = locPos;

        // Set character position inside collision dimension
        transform.position = transform.position - colDimScript.transform.position + colDimScript.posDifference;
        movementScr.oldPos = transform.position;
        gameObject.layer = 16; // layer to "CollisionDimension" layer
        

        // gravity body script
        gravityBodyScr.transform = realDimensionCharacter.transform;
        
        // background star particles to follow real dimension character
        //bgstarsFollowObjScr.obj = realDimensionCharacter.transform;
        
    }

    // Leave
    public void LeaveInteriorDimension() { // TODO: jäsentele vähän ja tee nää koodit selvemmäksi
        
        // Set character back to real dimension
        Character.InInteriorDimension = false;
        transform.position = realDimensionCharacter.transform.position;
        movementScr.oldPos = transform.position;
        gameObject.layer = 14; // layer to "Player" layer
        Character.Pippeli.layer = 0;
        // gravity body script
        gravityBodyScr.transform = transform;
        // cameras
        Other.MoveChilds(realDimensionCharacter.transform.Find("Cameras"), camerasT);


        /*camerasT.parent = transform;
        camerasT.localPosition = locPos;*/
        // pippeli
        Vector3 locPos = Character.Pippeli.transform.localPosition;
        Character.Pippeli.transform.parent = transform;
        Character.Pippeli.transform.localPosition = locPos;

        //bgstarsFollowObjScr.obj = transform;

        Destroy(realDimensionCharacter);
        Destroy(colDimScript.interiorDimTransform.gameObject);

    }

    #endregion

    

}
