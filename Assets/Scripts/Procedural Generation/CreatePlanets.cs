using UnityEngine;
using System.Collections;
//using System.Diagnostics;
//using System.Threading;

public class CreatePlanets : MonoBehaviour {

    public GeneratePlanet generatePlanet;
    public string name;

    public int planetCount;
    public float minSize;
    public float maxSize;
    public float minDistance;
    public float maxDistance;

    public float terrainMeshHeightMultiplier = 1;
    public float noiseScale = 10;

    public bool hasAtmosphere;


    // Use this for initialization
    void Start () {

        //createPlanets();
        
        
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();

        float planetSize = Random.Range(minSize, maxSize);
        GameObject planet = generatePlanet.GeneratePlanetData(name, planetSize, hasAtmosphere, terrainMeshHeightMultiplier, noiseScale).GeneratePlanet();

        //stopwatch.Stop();
        //UnityEngine.Debug.Log("Generate planet: "+stopwatch.Elapsed);

        float x = Random.Range(-maxDistance, maxDistance);
        float y = Random.Range(-maxDistance, maxDistance);
        float z = Random.Range(-maxDistance, maxDistance);
        planet.transform.position = new Vector3(x, y, z);
        //planet.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);

    }


    /*
    // Tekee planeetat avaruuteen
    private void createPlanets() {
        for (int i = 0; i < planetCount; ++i) {
            float x = Random.Range(-maxDistance, maxDistance);
            float y = Random.Range(-maxDistance, maxDistance);
            float z = Random.Range(-maxDistance, maxDistance);
            GameObject planet = (GameObject)Instantiate(planetPrefab, new Vector3(x, y, z), Quaternion.identity);
            planet.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);

        }

    }*/


    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            float planetSize = Random.Range(minSize, maxSize);
            GameObject planet = generatePlanet.GeneratePlanetData(name, planetSize, hasAtmosphere, terrainMeshHeightMultiplier, noiseScale).GeneratePlanet();

            float x = Random.Range(-maxDistance, maxDistance);
            float y = Random.Range(-maxDistance, maxDistance);
            float z = Random.Range(-maxDistance, maxDistance);
            planet.transform.position = new Vector3(x, y, z);
            //planet.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
        }
    }    

}



