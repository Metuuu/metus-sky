using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	#region [ - Init - ]

	// classes
	new Transform transform;
    [HideInInspector] public Rigidbody rb;
    public Transform cameraT;
    PlanetLocalDirections PLDir;

    // rotation
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    float verticalLookRotation;

    // movement
    float inputX;
    float inputY;
    Vector2 input = Vector2.zero;
    public float walkSpeed = 0.2f;
    public float runSpeed = 0.3f;
    public float movementAcceleration;
    public float jumpForce = 100;
    public int maxLookAngle = 60;
    public float MaximumSlope = 45.0f;
    Vector3 localMove;
    Vector3 moveDir;
    Vector3 targetMoveAmount;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Vector3 localVel;
    Vector3 vel;
    //space movement
    public float speedFromWall;
    public float floatingMovementSpeed;

	// air movement
	public bool allowMovementOnAir;
    public float airAcc;
    public float maxAirMoveSpeed;

    private bool doJump;

    // dont edit in inspector
    public float speed;
    public float curSpeed;
    public bool isRunning;


    // grounded
    public LayerMask groundedMask;
    public LayerMask interiorDimMask;
    public bool grounded;
    public float groundedRaycastR = 0.5f;
    public bool touchesObstacle;

    // fast Collision
    [HideInInspector] public Vector3 oldPos;
    public LayerMask fcCastMask;
    RaycastHit fcHit;

    // temporary calculations
    private float halfPlayerHeight;
    private float fudgeCheck;
    private float bottomCapsuleSphereOrigin; // transform.position.y - this variable = the y coord for the origin of the capsule's bottom sphere
    private float capsuleRadius;
    

    #endregion



    // --- Start ---
    void Start () {
        rb = Character.RigidBody;
        transform = Character.Transform;
        PLDir = GetComponent<PlanetLocalDirections>();
        
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        halfPlayerHeight = capsule.height * 0.5f;
        bottomCapsuleSphereOrigin = halfPlayerHeight - capsule.radius;
        capsuleRadius = (capsule.radius-0.05f)/10f;
    }



    // --- Update ---
    void Update() {

        // Jump input
        if ((grounded || (touchesObstacle && Character.InSpace)) && Input.GetButtonDown("Jump")) {
            doJump = true;
        }

		#region [ - Debug - ]

		// Stop
		if (Input.GetKeyDown(KeyCode.J)) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		// Teleport to facing direction
		if (Input.GetKey(KeyCode.K)) {
			if (isRunning)
				transform.position += Camera.main.transform.forward * 500 * Time.deltaTime;
			else 
				transform.position += Camera.main.transform.forward * 100 * Time.deltaTime;
		}

		#endregion

	}



	// --- Fixed Update ---
	void FixedUpdate() {

        vel = rb.velocity;
        localVel = transform.InverseTransformDirection(rb.velocity);

        #region [ - Fast collision check - ]

        if (!Character.InInteriorDimension) {
            if (Physics.Linecast(oldPos, transform.position, out fcHit, fcCastMask)) {
                if (fcHit.transform != transform) {
                    //Debug.Log(fcHit.transform);
                    transform.position = fcHit.point;
                    rb.velocity = Vector3.zero;


                }
            }
        } else {
            if (Physics.Linecast(oldPos, transform.position, out fcHit, interiorDimMask)) {
                if (fcHit.transform != transform) {
                    //Debug.Log(fcHit.transform);
                    transform.position = fcHit.point;
                    rb.velocity = Vector3.zero;


                }
            }
        }
        oldPos = transform.position;

        #endregion

        

        #region [ - Grounded check - ]

        RaycastHit hit;
        grounded = false;
        for (int i = 0, len = 6; i < len; ++i) {
            
            // Raycast for checking if grounded
            Ray ray;
            switch (i) {
                case 0:
                    ray = new Ray(transform.position + new Vector3(0, 0, 0), -transform.up);
                    break;
                case 1:
                    ray = new Ray(transform.position + transform.TransformDirection(new Vector3(0, 0, capsuleRadius)), -transform.up);
                    break;
                case 2:
                    ray = new Ray(transform.position + transform.TransformDirection(new Vector3(0, 0, -capsuleRadius)), -transform.up);
                    break;
                case 3:
                    ray = new Ray(transform.position + transform.TransformDirection(new Vector3(-capsuleRadius, 0, 0)), -transform.up);
                    break;
                case 4:
                    ray = new Ray(transform.position + transform.TransformDirection(new Vector3(capsuleRadius, 0, 0)), -transform.up);
                    break;
                default:
                    Vector3 randomRayVec = Random.insideUnitCircle * groundedRaycastR;
                    randomRayVec = transform.TransformDirection(new Vector3(randomRayVec.x, 0, randomRayVec.y));
                    ray = new Ray(transform.position + randomRayVec, -transform.up);
                    break;
            }
            //Debug.DrawRay(ray.origin,ray.direction*0.12f, Color.green);
            switch (Character.InInteriorDimension) {
                case true:
                    if (Physics.Raycast(ray, out hit, 0.12f, interiorDimMask) && Vector3.Angle(hit.normal, PLDir.up) <= MaximumSlope) {
                        grounded = true;
                        break;
                    }
                    break;
                case false:
                    if (Physics.Raycast(ray, out hit, 0.12f, groundedMask) && Vector3.Angle(hit.normal, PLDir.up) <= MaximumSlope) {
                        grounded = true;
                        break;
                    }
                    break;
            }
            
            //Debug.Log(hit.collider);
        }
        Character.GravityBodyScript.grounded = grounded;
        #endregion



        #region [ - Inputs - ]

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        input = Vector2.Lerp(input, new Vector2(inputX, inputY), movementAcceleration);
        
        if (Input.GetKey(KeyCode.LeftShift)) { isRunning = true; } // running
        else { isRunning = false; } // walking
        
        // Movement speed
        if (!isRunning) { // walking
            speed = walkSpeed;
        }
        else { // running
            speed = runSpeed;
        }

        
        
        #endregion


        #region [ --- On Planet --- ]
        if (!Character.InSpace) {

            if (!Character.IsFalling && !Character.IsStunned) {

                #region [ - Rotation - ]
                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
                verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
                verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
                cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
                #endregion


                #region [ - Movement -]

                if (grounded) {

                    // - Move amount & direction -
                    if (!doJump) {
                        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
                        if (targetVelocity.magnitude > 1) { targetVelocity = targetVelocity.normalized; }
                        targetVelocity *= speed;

                        Vector3 velocity = transform.InverseTransformDirection(PLDir.forwardVel+PLDir.rightVel);
                        Vector3 velocityChange = (targetVelocity - velocity);
                        //velocityChange.x = Mathf.Clamp(velocityChange.x, -speed, speed);
                        //velocityChange.z = Mathf.Clamp(velocityChange.z, -speed, speed);
                        rb.AddForce(transform.TransformDirection(velocityChange)*80, ForceMode.Force); //TODO: onks tää nyt hyvä
                    }

                    #endregion


                    #region [ - Jump - ]
                    else {
                        rb.velocity = PLDir.rightVel + PLDir.forwardVel;
                        rb.AddForce(PLDir.up * jumpForce, ForceMode.VelocityChange);
                        doJump = false;
                    }
                    #endregion

                }
                else {

                    // air accel
                    if (allowMovementOnAir) {
                        //if (!Mathf.Approximately(inputX + inputY, 0.0f)) {
                        if ((PLDir.rightVel + PLDir.forwardVel).magnitude < maxAirMoveSpeed) {

                            Vector3 targetVelocity = new Vector3(inputX, 0, inputY).normalized;
                            targetVelocity *= airAcc;

                            Vector3 velocity = transform.InverseTransformDirection(PLDir.forwardVel + PLDir.rightVel);
                            Vector3 velocityChange = (targetVelocity - velocity);
                            
                            //velocityChange.x = Mathf.Clamp(velocityChange.x, -speed, speed);
                            //velocityChange.z = Mathf.Clamp(velocityChange.z, -speed, speed);

                            rb.AddForce(transform.TransformDirection(velocityChange), ForceMode.Acceleration);
                        }
                        //}
                    }



                }
                 
                
            }

        }
        #endregion

        #region [ --- In Space --- ]

        else {

            #region [ - Rotation - ]
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
            cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
            #endregion


            #region [ - Movement - ]

            rb.AddForce(cameraT.forward * floatingMovementSpeed * inputY, ForceMode.Acceleration);
            rb.AddForce(cameraT.right * floatingMovementSpeed * inputX, ForceMode.Acceleration);

            #endregion


            #region [ - "Jump" - ]

            if (doJump) {
                rb.AddForce(cameraT.forward * speedFromWall, ForceMode.VelocityChange);
                doJump = false;
            }

            #endregion

        }

		#endregion


		touchesObstacle = false;
    }


    #region [ - Collision Events - ]

    // - On Collision Stay -
    void OnCollisionStay(Collision col) {
        touchesObstacle = true;
    }

    #endregion


    // --- On Enable ---
    void OnEnable() {
        rb = Character.RigidBody;
    }



}