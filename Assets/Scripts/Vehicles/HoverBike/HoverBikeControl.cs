using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverBikeControl : MonoBehaviour
{

    // -- Init --

    // classes
    public Vehicle vehicle;

    Transform transform;
    Rigidbody rb;
    GravityBody gravBod;


    // Parts
    public GameObject light;
    HoverBikeAudio audioScr;

    public Transform[] m_hoverPoints;
    public GameObject m_leftAirBrake;
    public GameObject m_rightAirBrake;


    // Movement Data
    public float m_hoverForce = 50;
    public float m_hoverHeight = 0.12f;

    public float m_forwardAcl = 400;
    public float m_backwardAcl = 150;
    float m_currThrust = 0.0f;
    public float m_turnStrength = 5;
    float m_currTurn = 0.0f;
    public float rotateUpForce = 10;

    public float battery;
    public float batteryLeft;
    public float batteryUsage;

    float turnDeadZone = 0.1f;
    public LayerMask layerMask;

    bool grounded;
    public float groundedDrag = 2;
    public float groundedAngularDrag = 4;


    // -- Start --
    void Start() {
        vehicle = GetComponent<Vehicle>();
        transform = gameObject.transform;
        rb = GetComponent<Rigidbody>();
        audioScr = GetComponent<HoverBikeAudio>();
        gravBod = GetComponent<GravityBody>();

    }


    // -- Update --
    void Update() {

        #region [ - KeyPress Events - ]
        if (vehicle.inUse) {

            // start / stop the engine
            if (Input.GetKeyDown(KeyCode.F)) {
                if (vehicle.engineOn) {
                    StopTheEngine();
                } else {
                    StartTheEngine();
                }
            }

            // leave the vehicle
            if (Input.GetKeyDown(KeyCode.E)) {
                if (vehicle.canLeaveVehicle) vehicle.LeaveVehicle();
            } else if (Input.GetKeyUp(KeyCode.E)) {
                vehicle.canLeaveVehicle = true; // Estää sen ettei samalla updatella kun mene sisään niin hyppää ulos
            }

        }
        #endregion


        #region [ - Movement - ]
        if (vehicle.engineOn && vehicle.inUse) {

            //floatAngle = Quaternion.FromToRotation(Vector3.up, gravBod.localGlobalUp); // maanpinnan mukainen rotaation

            // Main Thrust
            m_currThrust = 0.0f;
            float aclAxis = Input.GetAxis("Vertical");
            if (aclAxis > turnDeadZone) {
                m_currThrust = aclAxis * m_forwardAcl;
            }
            else if (aclAxis < -turnDeadZone) {
                m_currThrust = aclAxis * m_backwardAcl;
            }

            // Turning
            m_currTurn = 0.0f;
            float turnAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(turnAxis) > turnDeadZone)
                m_currTurn = turnAxis;
        }
        #endregion

    }

    // -- Fixed Update --
    void FixedUpdate() {

        if (vehicle.engineOn) {

            grounded = false;

            //  Hover Force + grounded check
            RaycastHit hit;
            for (int i = 0; i < m_hoverPoints.Length; ++i) {
                if (Physics.Raycast(m_hoverPoints[i].position, -transform.up, out hit, m_hoverHeight, layerMask)) {
                    rb.AddForceAtPosition(gravBod.localGlobalUp * m_hoverForce * (1.0f - (hit.distance / m_hoverHeight)), m_hoverPoints[i].position);
                    grounded = true;
                }

            }

            // Custom drag force when grounded
            if (grounded) {
                rb.drag = groundedDrag;
                rb.angularDrag = groundedAngularDrag;
                gravBod.dragFromAtmosphere = false;
            }
            // Drag from atmosphere if not grounded
            else {
                rb.drag = 0.1f;                        // <- Poista nämä kun drag from atmosphere toimii
                rb.angularDrag = groundedAngularDrag;  // <- angular dragiä pitää miettiä vielä tässä tapauksessa
                gravBod.dragFromAtmosphere = true;
            }

            // Rotate up if angle too large
            if (Vector3.Angle(gravBod.localGlobalUp, transform.up) > 40) {
                RotateUp();
            }

            if (vehicle.inUse && grounded) {
                // Forward
                if (Mathf.Abs(m_currThrust) > 0) {
                    float angle = Vector3.Angle(gravBod.localGlobalUp, transform.forward); // Don't allow forward force when too big angle
                    if ((angle > 70 && angle < 110)) {
                        rb.AddForce(transform.forward * m_currThrust);
                    }
                }

                // Turn
                if (m_currTurn > 0) {
                    rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
                }
                else if (m_currTurn < 0) {
                    rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
                }
            }
        }

    }


    void RotateUp() {
        if (Vector3.Angle(gravBod.localGlobalUp, transform.forward) > 90) {
            rb.AddRelativeTorque(new Vector3(-rotateUpForce*2f, 0, 0));
        }
        else {
            rb.AddRelativeTorque(new Vector3(rotateUpForce*2f, 0, 0));
        }

        if (Vector3.Angle(gravBod.localGlobalUp, transform.right) > 90) {
            rb.AddRelativeTorque(new Vector3(0, 0, rotateUpForce));
        }
        else {
            rb.AddRelativeTorque(new Vector3(0, 0, -rotateUpForce));
        }

    }



    // -- On Collision --
    void OnCollisionEnter(Collision collision) {
        if (vehicle.inUse) {
            if (collision.relativeVelocity.magnitude > 4.2f) {
                //Debug.Log(rb.velocity.magnitude + " | " + vehicle.velocity.magnitude + " | " + collision.relativeVelocity.magnitude);
                vehicle.LeaveVehicle();
            }
        }

    }



    // -- On Draw Gizmos --
    /*void OnDrawGizmos() {

        if (Application.isPlaying) {

            // Hover Force
            RaycastHit hit;
            for (int i = 0; i < m_hoverPoints.Length; ++i) {

                if (Physics.Raycast(m_hoverPoints[i].position, -gravBod.localGlobalUp, out hit, m_hoverHeight, layerMask)) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(m_hoverPoints[i].position, hit.point);
                    Gizmos.DrawSphere(hit.point, 0.05f);
                }
                else {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(m_hoverPoints[i].position, m_hoverPoints[i].position - gravBod.localGlobalUp * m_hoverHeight);
                }
            }
        }

    }*/




    // -- Other functions --

    // Enter Vehicle
    public void EnterVehicle() {
        vehicle.EnterVehicle();
    }

    // Start the engine
    public void StartTheEngine() {
        audioScr.PlayEngineStartSound();
        light.SetActive(true);
        vehicle.engineOn = true;
    }

    // Stop the engine
    public void StopTheEngine() {
        audioScr.PlayEngineStopSound();
        light.SetActive(false);
        vehicle.engineOn = false;
    }



}
