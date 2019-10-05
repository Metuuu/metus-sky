using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGeneratePlanet : MonoBehaviour {

    [SerializeField]
    private float planetSize;

    [SerializeField]
    private Vector3 position;

	[SerializeField]
	private bool hasAtmosphere;

	[SerializeField]
    private Noise.NoiseSource noiseSource;

    [SerializeField]
    private GPUNoiseGenerator.NoiseData ocean;
    [SerializeField]
    private GPUNoiseGenerator.NoiseData plainHills;
    [SerializeField]
    private GPUNoiseGenerator.NoiseData largeMountains;
    [SerializeField]
    private GPUNoiseGenerator.NoiseData mediumDetail;


    // Use this for initialization
    void Start() {
        new GPUPlanetData(name, planetSize, ocean, plainHills, largeMountains, mediumDetail, hasAtmosphere, noiseSource).GeneratePlanet(position);
    }


}
