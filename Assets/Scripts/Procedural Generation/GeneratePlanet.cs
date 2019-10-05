using UnityEngine;
using System.Collections;


public class GeneratePlanet : MonoBehaviour {

	[SerializeField] Material material;
	[SerializeField] string name;
	[SerializeField] Renderer noiseTaulu;
	[SerializeField] int octaves;
    [SerializeField] GPUNoiseGenerator.NoiseResolution noiseResolution;

    // ----------------------------------------

    bool useFalloff;

    public AnimationCurve meshHeightCurve;

    public TerrainType[] regions;

    public Gradient gradient;


    // - Falloff map settings -
    bool fallOffMapInverted;
    [Range(0, 1)]
    float falloffMinPercentage;
    [Range(0, 1)]
    float falloffMaxPercentage;
    [Range(0.2f, 10)]
    float a;
    [Range(0.2f, 10)]
    float b;
    // ------------------------

        

    // Generate planet
    public PlanetData GeneratePlanetData(string name, float planetSize, bool hasAtmosphere = false, float terrainMeshHeightMultiplier = 1, float noiseScale = 10) {
        return PlanetGenerator.GeneratePlanet(name, planetSize, material, meshHeightCurve, regions, gradient, terrainMeshHeightMultiplier, noiseScale, noiseTaulu, octaves, noiseResolution, hasAtmosphere);
    }


}




