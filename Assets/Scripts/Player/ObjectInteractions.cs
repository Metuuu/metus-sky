using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectInteractions : MonoBehaviour {


    #region [ - Init - ]
    public float grabRange;
    public LayerMask raycastLayer;

    Transform middleObject;
    public Transform grabbedObject;
    InteractiveObjectScript middleObjectScript;

    Rigidbody grabbeObjRigidB;
    float grabbedObjectSize;
    float grabbedObjDist;
    public float grabDistSpeed;

    private float origAngularDrag;

    public float maxPullForce;
    float pullVelocity;
    public float pullForce;

    private Vector3 grabbedObjRotation;

    public float maxThrowForce;
    public float throwingForceLoadSpeed;
    float throwForce = 0;
    int throwTimer = 5;

    bool justPickedUp = false;

    Vector3 objectRotation;

    bool holdingObject = false;

    public GameObject MiddleInfoTxtObj;
    public GameObject ThrowingInfoTxtObj;
    Text MiddleInfoTxt;
    Text ThrowingInfoTxt;

    Camera cam;
    Transform camTransform;

    // Rotate grabbed obj juttuja
    public float rotatingForce;
    public float rotationSensitivity;

    #endregion


    // --- Start ---
    void Start() {
        cam = Camera.main;
        camTransform = cam.transform;
        if (MiddleInfoTxtObj != null) {
            MiddleInfoTxt = MiddleInfoTxtObj.GetComponent<Text>();
            ThrowingInfoTxt = ThrowingInfoTxtObj.GetComponent<Text>();
        }
    }


    // --- Update ---
    void Update() {

        #region [ - JOS KÄDESSÄ ON JO TAVARA - ]
        if (grabbedObject != null) {

            // Kirjottaa heittovoiman hudiin
            ThrowingInfoTxt.text = "Throwing power: " + Mathf.Round((throwForce) / (maxThrowForce) * 100) + " / 100";

            // Hiiren rullalla voi tuoda objectia lähemmäs ja kauemmas
            float wheelAxis = Input.GetAxis("Mouse ScrollWheel");
            float tmp_dist = grabbedObjDist;
            grabbedObjDist += wheelAxis * grabDistSpeed;
            grabbedObjDist = Mathf.Clamp(grabbedObjDist, 0, grabRange);//-1);
            //Tarkistaa että voiko viedä tavaraa eteenpäin vai tuleeko jotain tielle
            /*if (Physics.Linecast(grabbedObject.position, grabbedObject.position + ((cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane)) + camTransform.forward).normalized * wheelAxis))) {
                grabbedObjDist = tmp_dist; //Jos tulee niin distancee pienennetään
                Debug.Log("HIT");
            }*/

            // Heittovoima kasvaa kun pitää pohjassa
            if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) && justPickedUp == false && throwForce < maxThrowForce) {
                if (throwTimer > 0) {
                    --throwTimer;
                }
                else { throwForce += throwingForceLoadSpeed * Time.deltaTime; }
            }
            // Jos painaa oikeeta pohjassa samalla niin sillon se laskee
            else if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && justPickedUp == false && throwForce > 0) {
                throwForce -= throwingForceLoadSpeed * Time.deltaTime;
            }
            // Heittää / tiputtaa kun nostaa hiiren oikeen napin ylös
            else if (Input.GetMouseButtonUp(1) && justPickedUp == false) {;
                grabbeObjRigidB.AddForce(camTransform.forward * throwForce);
                DropObject();
                throwForce = 0;
                throwTimer = 5;
                return;
            }
            else if (Input.GetMouseButtonUp(1)) {
                justPickedUp = false;
            }

            // Keskinapilla voi rotate objecteja kädessä kannateltavaa objectia
            if (Input.GetMouseButton(2)) {
                MouseRotateGrabbedObj();
            }

            // - Objecti liikkuu koko ajan tietyllä voimalla ruudun keskelle säädetylle etäisyydelle - 
            Vector3 newPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane)) + camTransform.forward * (grabbedObjDist);// +1);

            float oldToNewDist = Vector3.Distance(grabbedObject.position, newPosition);

            // Jos tavara liian kaukana niin se tipahtaa
            if (oldToNewDist > grabRange) {
                DropObject();
                return;
            }

            // jos ei ole tarpeeksi lähellä keskipistettä niin kiihdyttää itseään sitä kohti
            if (oldToNewDist > 0.01) {
                pullVelocity = (pullForce * 10f * (Character.RigidBody.mass / grabbeObjRigidB.mass) * Time.deltaTime); // TODO: jos ukolla painava tavara niin sen pitää rajoittaa kävelynopeutta
                //Debug.Log(pullVelocity);
                //pullVelocity = Mathf.Clamp(pullVelocity, 0, maxPullForce);
                //grabbeObjRigidB.velocity = Vector3.ClampMagnitude((newPosition - grabbedObject.position).normalized * (Mathf.Pow(oldToNewDist, 0.15f)), pullVelocity);
                grabbeObjRigidB.velocity = Vector3.ClampMagnitude((newPosition - grabbedObject.position).normalized, pullVelocity);
                //grabbeObjRigidB.velocity += Character.rb.velocity;
            }
            // pysähtyy keskelle
            else {
                pullVelocity = 0;
                grabbeObjRigidB.velocity *= 0.6f; //Character.rb.velocity;
            }

            // - Rotation kuntoon esim kun kääntyy -
            //Vector3 newRot = new Vector3(grabbedObject.rotation.eulerAngles.x, camTransform.rotation.eulerAngles.y, grabbedObject.rotation.eulerAngles.z);
            //grabbedObject.eulerAngles = newRot;



        }
        #endregion

        #region [- JOS KÄDESSÄ EI OLE TAVARAA - ]
        else {

            #region [ - input tarkistus ... - ]

            #endregion

            // Katsoo onko edessä objectia
            middleObject = GetMouseHoverObject(grabRange);
            //Debug.Log(middleObject);

            // Jos objectin saa ottaa käteen
            if (middleObject != null) {
                middleObjectScript = middleObject.GetComponent<InteractiveObjectScript>();

                if (middleObjectScript != null) {

                    switch (middleObjectScript.objectType) {

                        // Info
                        case (InteractiveObjectScript.type.Info):
                            MiddleInfoTxt.text = middleObjectScript.info;
                            break;
                        
                        // Button
                        case (InteractiveObjectScript.type.Button):
                            MiddleInfoTxt.text = "Press [E] to use";
                            if (Input.GetKeyDown(KeyCode.E)) {
                                middleObjectScript.buttonEvent.Invoke();
                            }
                            break;

                        // Grabbable
                        case (InteractiveObjectScript.type.Grabbable):
                            if (Input.GetMouseButtonDown(1)) {
                                TryGrabObject(middleObject);
                            }
                            break;
                    }
                    

                    return;
                }
            }
            // Jos objecti on esim joku puu tai terrain ei sitä tietenkään haluta käteen ja siitä ei sanota tietoa guissa
            MiddleInfoTxt.text = null;
        }
        #endregion

    }




    //************************************************************\\
    // --------------------- CUSTOM FUNCTIONS ---------------------
    //************************************************************\\

    #region

    // -- KATSOO ONKO RUUDUN KESKELLÄ OBJECTI --
    Transform GetMouseHoverObject(float range) {

        Vector3 position = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));

        RaycastHit raycastHit;

        Vector3 target = position + camTransform.forward * range;

        if (Physics.Linecast(position, target, out raycastHit, raycastLayer)) {
            return raycastHit.collider.transform;
        }
        else { return null; }
    }



    // -- YRITÄ OTTAA TAVARA KÄTEEN --
    void TryGrabObject(Transform grabObject) {
        if (grabObject == null) {
            return;
        }
        else {
            grabbedObject = grabObject;
            grabbeObjRigidB = grabbedObject.GetComponent<Rigidbody>();
            grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size.magnitude;
            grabbeObjRigidB.useGravity = false;
            origAngularDrag = grabbeObjRigidB.angularDrag;
            grabbeObjRigidB.angularDrag = 100;
            objectRotation = grabbedObject.eulerAngles;
            grabbedObjDist = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane)) + camTransform.forward, grabbedObject.position);
            grabbedObjRotation = grabbedObject.eulerAngles;
            justPickedUp = true;

            //Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }



    // -- ROTATE OBJECT WITH MOUSE --
    void MouseRotateGrabbedObj() {

        //Get how far the mouse has moved by using the Input.GetAxis().
        float rotationX = Input.GetAxis("Mouse X") * 18000 * Time.deltaTime;
        float rotationY = Input.GetAxis("Mouse Y") * 18000 * Time.deltaTime;

        // Rotate the object around the camera's "up" axis, and the camera's "right" axis.
        grabbedObject.Rotate(camTransform.up, -Mathf.Deg2Rad * rotationX, Space.World);
        grabbedObject.Rotate(camTransform.right, Mathf.Deg2Rad * rotationY, Space.World);

    }



    // -- OBJECTIN TIPUTUS POIS --
    void DropObject() {

        ThrowingInfoTxt.text = null;

        if (grabbedObject == null) {
            return;
        }
        else {
            grabbeObjRigidB.useGravity = true;
            grabbeObjRigidB.angularDrag = origAngularDrag;
            grabbedObject = null;
        }
    }

    #endregion

}

