using UnityEngine;
using System.Collections;

public class AlusController : MonoBehaviour {

    // -- Enums --
    public enum SpaceshipGear { Off, LandingGear, PlanetaryDrive, SpaceDrive, HyperDrive }


    #region [ --- Init --- ]

    public Vehicle vehicle;

    // - Classes -
    Rigidbody rb;
    Transform transform;
    PlanetLocalDirections PLDir;

    public SpaceshipAudio audio;
    public CameraEffects cameraEffects;

    public ParticleSystem hyperdrivePS;
    public ParticleSystem thrusterPS;

    public AlusHUD alusHUD;


    // - Setting -

    // mouse
    public bool invertY;
    public AnimationCurve rotationCurve;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    float rotationY = 0F;
    float rotationX = 0F;
    [HideInInspector] public bool rotationLocked;


    // flying

    // gears
    [HideInInspector] public Gear gear;
    public Gear offGear;
    public Gear landingGear = new Gear();
    public Gear planetaryDrive = new Gear();
    public Gear spaceDrive = new Gear();


    // thruster
    bool thrusterOn;
    public Thruster thruster;

    // glide
    float glideValue;


    // readonly
    float maxSpeed;
    float acceleration;
    float rotationSpeed;
    Vector3 locVel;
    Vector3 locAngVel;
    Vector3 currentAcceleration;
    Vector3 lastVelocity;


    // other
    public GameObject SpaceshipOffHUD;
    bool canLeaveVehicle;


    // landing
    public GameObject[] LandingLegs;
    [HideInInspector] public Vector3 startForwardDirection;

    #endregion



    // --- Start ---
    void Start() {
        vehicle = GetComponent<Vehicle>();
        alusHUD = GetComponent<AlusHUD>();
        PLDir = GetComponent<PlanetLocalDirections>();
        transform = gameObject.transform;
        rb = vehicle.rigidbody;
        ChangeGear(offGear);
    }


    // --- Update ---
    void Update() {

        #region [ - KeyPress Events - ]
        if (vehicle.inUse) {

            // start / stop the engine
            if (Input.GetKeyDown(KeyCode.F)) {
                if (vehicle.engineOn) {
                    StopTheEngine();
                }
                else {
                    StartTheEngine();
                }
            }

            // leave the vehicle
            if (Input.GetKeyDown(KeyCode.E)) {
                if (vehicle.canLeaveVehicle) {
                    vehicle.LeaveVehicle();
                    alusHUD.HideHand();
                }
            }
            else if (Input.GetKeyUp(KeyCode.E)) {
                vehicle.canLeaveVehicle = true; // Estää sen ettei samalla updatella kun mene sisään niin hyppää ulos
            }

        }
        #endregion



        #region [ -------- ROTATION -------- ]
        if (gear.name != SpaceshipGear.LandingGear) {

            if (vehicle.engineOn && vehicle.inUse && !rotationLocked) {

                rotationX += Input.GetAxis("Mouse X") * sensitivityX * 0.25f;

                if (invertY) {
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY * 0.25f;
                }
                else {
                    rotationY -= Input.GetAxis("Mouse Y") * sensitivityY * 0.25f;
                }


                // Rotatettaa riippuen hiiren etäisyydestä keskustaan
                Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                float distanceX = mousePosition.x - screenCenter.x;
                float distanceY = mousePosition.y - screenCenter.y;
                float rotationSpeedNormalizerX = rotationCurve.Evaluate(Mathf.Clamp(Mathf.Abs(distanceX / (Screen.width / 2)), -1, 1));
                float rotationSpeedNormalizerY = rotationCurve.Evaluate(Mathf.Clamp(Mathf.Abs(distanceY / (Screen.height / 2)), -1, 1));

                if (rotationSpeedNormalizerX > 0) {
                    rb.AddTorque(transform.up * rotationX * rotationSpeedNormalizerX, ForceMode.Acceleration);
                }
                if (rotationSpeedNormalizerY > 0) {
                    rb.AddTorque(transform.right * rotationY * rotationSpeedNormalizerY, ForceMode.Acceleration);
                }

            }
        }
        #endregion


        #region [ -------- MOVEMENT -------- ]

        locVel = transform.InverseTransformDirection(rb.velocity);
        locAngVel = transform.InverseTransformDirection(rb.angularVelocity);


        #region [ - Thruster - ]
        if (gear.thrusterUseAllowed) {
            thrusterOn = false;
            if (vehicle.engineOn && vehicle.inUse) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    if (thruster.maxHeat >= thruster.heat + thruster.heatUsage * Time.deltaTime) {
                        thruster.heat += thruster.heatUsage * Time.deltaTime;
                        thrusterOn = true;
                    }
                }
                else if (thruster.heat > 0) {
                    thruster.heat = thruster.heat - thruster.heatOut * Time.deltaTime;
                    Mathf.Clamp(thruster.heat, 0, thruster.maxHeat);
                }
            }
        }

        #endregion


        #region [ - Glide - ]
        if (vehicle.engineOn) {
            float lerpValue;
            float evaluatedGlide = gear.glideCurve.Evaluate(glideValue);

            if (locVel.y > 1f) {
                lerpValue = locVel.y - locVel.y * Time.deltaTime * 10f * (1 - evaluatedGlide);
                locVel = new Vector3(locVel.x, lerpValue, locVel.z);
            }
            else if (locVel.y < -1f) {
                lerpValue = locVel.y - locVel.y * Time.deltaTime * 10f * (1 - evaluatedGlide);
                locVel = new Vector3(locVel.x, lerpValue, locVel.z);
            }
            if (locVel.x > 1f) {
                lerpValue = locVel.x - locVel.x * Time.deltaTime * 10f * (1 - evaluatedGlide);
                locVel = new Vector3(lerpValue, locVel.y, locVel.z);
            }
            else if (locVel.x < -1f) {
                lerpValue = locVel.x - locVel.x * Time.deltaTime * 10f * (1 - evaluatedGlide);
                locVel = new Vector3(lerpValue, locVel.y, locVel.z);
            }

            rb.velocity = transform.TransformDirection(locVel);
        }
        #endregion


        #region [ - gear, speed, thruster, particle system - ]
        // Thrusteri pois
        if (!thrusterOn) {
            if (thrusterPS.emissionRate != 0) {
                thrusterPS.emissionRate = 0; // particle system
            }
            acceleration = gear.acceleration;
            maxSpeed = gear.maxSpeed;
            rotationSpeed = gear.rotationSpeed;
            glideValue = gear.normalGlideValue;
        }
        // Thrusteri päällä
        else {
            if (thrusterPS.emissionRate != thruster.thrusterPSEmitRate) {
                thrusterPS.emissionRate = thruster.thrusterPSEmitRate; // particle system
            }
            cameraEffects.ShakeScreen(thruster.thrusterShakeForce, 10);
            acceleration = gear.acceleration * thruster.accelerationMultiplier;
            maxSpeed = gear.maxSpeed * thruster.maxSpeedMultiplier;
            rotationSpeed = gear.rotationSpeed * thruster.rotationMultiplier;
            glideValue = gear.thrusterGlideValue;
        }
        #endregion


        #region [ - Moving - ]
        if (vehicle.engineOn) {

            if (vehicle.inUse) {
                // - Eteen/taakse liikkuminen -
                if (Input.GetKey(KeyCode.W) || thrusterOn) { // eteen

                    if (gear.name == SpaceshipGear.LandingGear) {
                        ChangeGear(planetaryDrive);
                        return;
                    }

                    if (locVel.z < maxSpeed) {
                        rb.AddForce(transform.forward * (rb.velocity.magnitude + acceleration), ForceMode.Acceleration);
                    }
                }
                else if (Input.GetKey(KeyCode.S)) { // taakse
                    if (-locVel.z < gear.maxReverseSpeed) {
                        rb.AddForce(-transform.forward * (rb.velocity.magnitude + gear.reverseSpeed), ForceMode.Acceleration);
                    }
                }

                // - Aluksen kääntäminen sivuttain -
                if (Input.GetKey(KeyCode.A)) {
                    rb.AddTorque(transform.forward * rotationSpeed);
                }
                else if (Input.GetKey(KeyCode.D)) {
                    rb.AddTorque(-transform.forward * rotationSpeed);

                }
            }

            // - Lerp velocity -
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(rb.velocity, maxSpeed), Time.fixedDeltaTime);
        }
        #endregion

        #endregion


    }


    // --- Fixed Update ---
    void FixedUpdate() {
       

        #region [ ------ LANDING GEAR ------ ]
        if (gear.name == SpaceshipGear.LandingGear) {

            float lerpVal = 1;
            float Ydistance = Mathf.Abs(alusHUD.floatHeightSlider.value + vehicle.gravityBody.closestPlanetRadius * 10f - vehicle.gravityBody.distanceToClosestPlanet * 10f);
            Vector3 newLogGlobRightVel = PLDir.rightVel;
            Vector3 newLogGlobUpVel = PLDir.upVel;
            Vector3 newLogGlobForwardVel = PLDir.forwardVel;

            Vector3 newLogGlobRightAngVel = PLDir.rightAngVel;
            Vector3 newLogGlobUpAngVel = PLDir.upAngVel;
            Vector3 newLogGlobForwardAngVel = PLDir.forwardAngVel;

            Vector3 addForce = Vector3.zero;
            Vector3 addTorque = Vector3.zero;

            // --- Position --- // TODO: position muuttaminen landing gearille

            if (PLDir.rightVel.magnitude != 0 || PLDir.forwardVel.magnitude != 0) {
                newLogGlobForwardVel *= 0.9f;
                newLogGlobRightVel *= 0.9f;
            }



            // --- Altitude ---
            /*if (Mathf.Abs(alusHUD.floatHeightSlider.value - vehicle.gravityBody.distanceToClosestPlanetSurface * 10f) < 0.05f && PLDir.upVel.magnitude < 0.05f) {
                newLogGlobUpVel = Vector3.zero;
            }
            else {*/
                // Jos alus on matalemmalla kuin float korkeus
                if (vehicle.gravityBody.distanceToClosestPlanet * 10f < alusHUD.floatHeightSlider.value + vehicle.gravityBody.closestPlanetRadius * 10f) {
                    if (PLDir.upMag < gear.maxSpeed) { // max speed
                        if (PLDir.upMag < 0) {
                            newLogGlobUpVel *= 0.9f;
                            lerpVal = gear.glideCurve.Evaluate(Ydistance) / 2f;
                        }
                        else {
                            lerpVal = gear.glideCurve.Evaluate(Ydistance);
                        }
                        addForce += PLDir.up * gear.acceleration * lerpVal;
                    }
                }
                // ..korkeammalla
                else if (PLDir.upMag < gear.maxReverseSpeed) { // max reverse speed
                    if (PLDir.upMag > 0) {
                        newLogGlobUpVel *= 0.9f;
                        lerpVal = gear.glideCurve.Evaluate(Ydistance) / 2f;
                    }
                    else {
                        lerpVal = gear.glideCurve.Evaluate(Ydistance);
                    }
                    addForce -= PLDir.up * gear.reverseSpeed * lerpVal;
                }
                else {
                    addForce += PLDir.up * gear.reverseSpeed;
                }
            //}

            // --- Direction ---
            if (Mathf.Abs(alusHUD.floatDirectionSlider.value - (alusHUD.floatDirectionSlider.minValue + 180)) < 0.05f && PLDir.rightAngVel.magnitude < 0.05f) { // paikoillaan jos tarpeeksi lähellä tavoite tilaa
                newLogGlobUpAngVel = Vector3.zero;
            }
            else { // liikkuminen kohti tavoite tilaa
                if (alusHUD.floatDirectionSlider.value < alusHUD.floatDirectionSlider.minValue + 180) {
                    if (PLDir.upAngMag < 0) {
                        newLogGlobUpAngVel *= 0.6f;
                    }
                    addTorque -= PLDir.up * gear.rotationSpeed;
                }
                else {
                    if (PLDir.upAngMag > 0) {
                        newLogGlobUpAngVel *= 0.6f;
                    }
                    addTorque += PLDir.up * gear.rotationSpeed;
                }
            }

            // --- Angle ---

            // YlösAlas
            float xVal = 360 + (MathfCustom.AngleSigned(PLDir.up, transform.forward, transform.right) - 180 - 270);
            if (xVal >= 180) {
                xVal -= 360;
            }
            else if (xVal < -180) {
                xVal += 360;
            }
            xVal *= -1;
            if (Mathf.Abs(xVal - alusHUD.floatAngleSlider.value) < 0.05f && PLDir.rightAngVel.magnitude < 0.05f) { // paikoillaan jos tarpeeksi lähellä tavoite tilaa
                newLogGlobRightAngVel = Vector3.zero;
            }
            else { // liikkuminen kohti tavoite tilaa
                if (xVal > alusHUD.floatAngleSlider.value) {
                    if (PLDir.rightAngMag > 0) {
                        newLogGlobRightAngVel *= 0.6f;
                    }
                    addTorque += PLDir.right * gear.rotationSpeed;

                }
                else {
                    if (PLDir.rightAngMag < 0) {
                        newLogGlobRightAngVel *= 0.6f;
                    }
                    addTorque -= PLDir.right * gear.rotationSpeed;
                }
            }

            // Kallistuminen vinoittain // TODO: korjaa bugi jossa oikealle kallistuessa alus ei palaa normaaliin
            float zVal = MathfCustom.AngleSigned(PLDir.right, transform.up, transform.forward);
            zVal += 90;
            if (zVal >= 360) {
                zVal -= 360;
            }
            else if (zVal < 0) {
                zVal += 360;
            }
            if (Mathf.Abs(zVal) -90f < 0.05f && PLDir.forwardAngVel.magnitude < 0.05f) { // paikoillaan jos tarpeeksi lähellä tavoite tilaa
                newLogGlobForwardAngVel = Vector3.zero;
            }
            else { // liikkuminen kohti tavoite tilaa
                if (zVal <= 180) {
                    if (PLDir.forwardAngMag > 0) {
                        newLogGlobForwardAngVel *= 0.6f;
                    }
                    addTorque += PLDir.forward * gear.rotationSpeed;
                }
                else {
                    if (PLDir.forwardAngMag < 0) {
                        newLogGlobForwardAngVel *= 0.6f;
                    }
                    addTorque -= PLDir.forward * gear.rotationSpeed;
                }
            }

            rb.velocity = newLogGlobRightVel + newLogGlobUpVel + newLogGlobForwardVel;
            rb.angularVelocity = newLogGlobRightAngVel + newLogGlobUpAngVel + newLogGlobForwardAngVel;

            rb.AddForce(addForce);
            rb.AddTorque(addTorque);

        }

        #endregion


        #region [ - Alus heiluu eri suuntiin riippuen voimista - ]
        if (vehicle.inUse) {
            currentAcceleration = (locVel - lastVelocity) / Time.deltaTime;
            lastVelocity = locVel;

            cameraEffects.moveCameraBackwards(currentAcceleration.z);
            cameraEffects.moveCameraDownwards(currentAcceleration.y / 400f);
            cameraEffects.moveCameraLeft(currentAcceleration.x / 400f);
        }
        #endregion

    }



    #region [ - Changing gears - ]

    void ChangeGear(Gear gear) {

        this.gear = gear;

        offGear.hud.HudToEnable.SetActive(false);
        landingGear.hud.HudToEnable.SetActive(false);
        planetaryDrive.hud.HudToEnable.SetActive(false);
        spaceDrive.hud.HudToEnable.SetActive(false);
        alusHUD.GearChanged(gear.hud);

        audio.GearChanged(gear.audio);
        

        if (gear.name == SpaceshipGear.LandingGear) {
            vehicle.gravityBody.gravityEnabled = false;
            foreach (GameObject landingLeg in LandingLegs) {
                landingLeg.SetActive(true);
            }
            startForwardDirection = PLDir.forward;
        }
        else {
            vehicle.gravityBody.gravityEnabled = true;
            if (gear.name != SpaceshipGear.Off) {
                foreach (GameObject landingLeg in LandingLegs) {
                    landingLeg.SetActive(false);
                }
            }
        }

        if (gear.name == SpaceshipGear.PlanetaryDrive || gear.name == SpaceshipGear.SpaceDrive) {
            rb.angularDrag = 3;
        } else {
            rb.angularDrag = 0;
        }

    }

    #endregion


    #region [ - Vehicle enter/leave  |  Engine start/stop - ]

    // Hop in
    public void EnterVehicle() {
        vehicle.EnterVehicle();
    }
    

    // Start the engine
    public void StartTheEngine() {
        audio.PlayEngineStartSound();
        vehicle.engineOn = true;
        ChangeGear(landingGear);
        SpaceshipOffHUD.SetActive(false);
        landingGear.hud.HudToEnable.SetActive(true);
    }

    // Stop the engine
    public void StopTheEngine() {
        audio.PlayEngineStopSound();
        ChangeGear(offGear);
        vehicle.engineOn = false;
    }


    // Start landing
    public void StartLanding() {
        ChangeGear(landingGear);
    }


    #endregion



    // Launch ejector seat
    public void LaunchEjectorSeat() {

    }



    // - On collision enter -
    void OnCollisionEnter(Collision col) {

        if (vehicle.engineOn) {
            if (col.gameObject.tag == "Player") {
                rb.velocity = vehicle.velocity;
                col.gameObject.GetComponent<Rigidbody>().velocity = vehicle.velocity;
            }
        }



    }





    #region [ - Spaceship part Classes - ]

    // GEAR
    [System.Serializable]
    public class Gear {

        public SpaceshipGear name;

        // - Data -
        public float maxSpeed;
        public float acceleration;
        public float rotationSpeed;
        public float reverseSpeed;
        public float maxReverseSpeed;

        // thruster
        public bool thrusterUseAllowed;


        // glide
        [Range(0.0f, 1.0f)]
        public float normalGlideValue;
        [Range(0.0f, 1.0f)]
        public float thrusterGlideValue;
        public AnimationCurve glideCurve;


        // - HUD -
        public HUD hud;

        [System.Serializable]
        public struct HUD {
            public GameObject HudToEnable;
            public UnityEngine.UI.Text speedText;
            public UnityEngine.UI.Text thrusterText;
        }


        // - AUDIO -
        public Audio audio;

        [System.Serializable]
        public struct Audio {
            public bool hasAudio;
            public AudioClip EngineSound;
            public bool usePitch;
            public float LowPitch;
            public float HighPitch;
            public float SpeedToRevs;
        }


    }

    // THRUSTER
    [System.Serializable]
    public class Thruster {

        public float thrusterPSEmitRate = 200;
        public int thrusterShakeForce = 2;
        public float maxSpeedMultiplier;
        public float accelerationMultiplier;
        public float rotationMultiplier;
        public float maxHeat;
        public float heatUsage;
        public float heatOut;
        public float heat;

    }

    #endregion



}
