using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTestScript : MonoBehaviour {


    // - Init -
    Transform planet;
    public float radius;
    public bool hasAtmosphere;
    public AtmosphereData atmosphere;

    public float mass;
    public int mass10pow;


    public Transform player;


    // Use this for initialization
    void Start() {
        planet = transform;
        atmosphere = AtmosphereGenerator.GenerateAtmosphere(planet, radius, 0.25f);
    }

    // Update is called once per frame
    void Update() {
        if (hasAtmosphere) {

            atmosphere.Update();

            if (Vector3.Distance(planet.position, player.position) > radius * 1.17f) {
                atmosphere.atmoFromSpaceTransform.gameObject.SetActive(true);
            }
            else {
                atmosphere.atmoFromSpaceTransform.gameObject.SetActive(false);
            }

        }
    }



}


public class Atmosphere {

    public float throposphere;
    public float stratosphere;
    public float mesosphere;
    public float thremosphere;
    public float exosphere;


}