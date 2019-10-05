using UnityEngine;
using System.Collections;

public class GravityBody : MonoBehaviour {


    // - Init -
    public Transform transform;
    public Transform realBodyTransform;
    GravityAttractor attractor;
    Rigidbody rb;

    public float radius;
    public bool inSpace;
    public bool gravityEnabled = true;

    public bool freezeRotation;
    public bool dragFromAtmosphere;

    [HideInInspector] public bool grounded;
    [HideInInspector] public Vector3 localGlobalUp;
    [HideInInspector] public GameObject closestPlanet;
    [HideInInspector] public float closestPlanetRadius;
    [HideInInspector] public float distanceToClosestPlanet;
    [HideInInspector] public float distanceToClosestPlanetSurface;

    public LocalGlobalUp LGUp = new LocalGlobalUp();

    // Space event
    public delegate void SpaceEnteredHandler(); //object source, System.EventArgs args
    public event SpaceEnteredHandler SpaceEntered;
    protected virtual void OnSpaceEntered() {
        if (SpaceEntered != null) {
            SpaceEntered();
        }
    }
    public delegate void SpaceLeavedHandler(); //object source, System.EventArgs args
    public event SpaceLeavedHandler SpaceLeaved;
    protected virtual void OnSpaceLeaved() {
        if (SpaceLeaved != null) {
            SpaceLeaved();
        }
    }


    // TODO: Pitäis tehä joku static class ja joku script refreshaa aina refreshaa sen classin gravity attractor arrayn kun planeetat tulee tarpeeksi lähelle tai menee kauemmas että gravityn vaikutus alkaisi tai hiipuisi pois
    // tällähetkellä jokainen tämmönen scripti ettii fixed updatessa sen kaiken tiedon uudestaan ja uudestaan ja menee paljon tehoo hukkaan.
    float gravitationalConstant = (6.67408f * Mathf.Pow(10,-11)/10f);


    // - Awake -
    void Awake () {
        transform = gameObject.transform;
        realBodyTransform = gameObject.transform;
        rb = transform.GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (freezeRotation) {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }


    void Update() {

        closestPlanet = GetClosestPlanet();

        if (closestPlanet == null) { return; }

		//closestPlanetRadius = closestPlanet.GetComponent<PlanetScript>().planetData.radius;
		PlanetTestScript planetScript = closestPlanet.GetComponent<PlanetTestScript>();
		if (planetScript == null) { return; }

		closestPlanetRadius = planetScript.radius;

		distanceToClosestPlanet = Vector3.Distance(transform.position, closestPlanet.transform.position);
        distanceToClosestPlanetSurface = Vector3.Distance(transform.position, closestPlanet.transform.position) - closestPlanetRadius;

        if (distanceToClosestPlanet > closestPlanetRadius * 1.03f) {
            if (!inSpace) {
                inSpace = true;
                OnSpaceEntered();
                if (freezeRotation) {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }

        }
        else {
            if (inSpace) {
                inSpace = false;
                OnSpaceLeaved();
                if (freezeRotation) {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }

        }

    }
    
    // - Fixed Update -
    void FixedUpdate () {

        if (gravityEnabled) {// && !grounded) {

            GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
            foreach (GameObject planet in planets) {
                if (planet.name != transform.name) {
                    float gravityAcceleration = CalculateGravityAcceleration(planet);
                    //Debug.Log(planet.name+": "+gravityAcceleration);

                    bool isClosestPlanet = (closestPlanet == planet);
                    planet.GetComponent<GravityAttractor>().Attract(gravityAcceleration, rb, ref localGlobalUp, freezeRotation, 1f, realBodyTransform.up, isClosestPlanet);
                    LGUp.value = localGlobalUp;
                }
            }

        }

    }

    
    
    // Get closest planet
    GameObject GetClosestPlanet() {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject t in planets) {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist) {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }



    // Calculate gravity force
    float CalculateGravityAcceleration(GameObject planet) {

        Transform pTransform = planet.transform;
		//PlanetTestScript planetScript = planet.GetComponent<PlanetTestScript>();
		PlanetTestScript planetScript = planet.GetComponent<PlanetTestScript>();
		if (planetScript == null) { return 0; }
		//PlanetScript planetScript = planet.GetComponent<PlanetScript>();
		float distance = Vector3.Distance(pTransform.position, transform.position)-radius;
        return ((gravitationalConstant * planetScript.mass * Mathf.Pow(10, planetScript.mass10pow)) / (distance*distance));
        //return ((gravitationalConstant * planetScript.planetData.mass * Mathf.Pow(10, planetScript.planetData.mass10pow)) / (distance*distance));

    }




    // - On Enable -
    void OnEnable() {
        rb = transform.GetComponent<Rigidbody>();
    }


    // Gravity body data
    public class LocalGlobalUp {
        public Vector3 value;
    }


}
