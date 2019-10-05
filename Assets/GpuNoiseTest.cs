using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuNoiseTest : MonoBehaviour {
    
  
    [System.Serializable]
    private class HeightmapData {
        public GPUNoiseGenerator.NoiseType noiseType;
        public GPUNoiseGenerator.NoiseResolution noiseResolution;
        public GPUNoiseGenerator.Side side;
        public string quarter;
        public int Octaves;
        public float Frequency;
        public float Amplitude;
        public float Lacunarity;
        public float Persistence;
        public RenderTexture texture;
    }

    [SerializeField]
    private HeightmapData generalShape;
    [SerializeField]
    private HeightmapData largeMountains;
    [SerializeField]
    private HeightmapData largeMountainsDetail;
    [SerializeField]
    private HeightmapData largeHills;
    [SerializeField]
    private HeightmapData mediumHills;
    [SerializeField]
    private HeightmapData TinyHills;

    private Material mat;
    private Renderer rend;
    
    

    void Awake() {
        rend = transform.GetComponent<Renderer>();
        mat = new Material(Shader.Find("Shader Forge/Terrain"));

        //InvokeRepeating("generateNoiseTerrain", 0, 0.1f);

        generateNoiseTerrain();
    }
        
    

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            generateNoiseTerrain();
        }
    }



    void generateNoiseTerrain() {
        /*
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        for (int i = 0; i < 1000; ++i) {
            GPUNoiseGenerator.GenerateNoise(0, mediumHills.noiseType, mediumHills.noiseResolution, mediumHills.side, mediumHills.quarter, mediumHills.Octaves, mediumHills.Frequency, mediumHills.Amplitude, mediumHills.Lacunarity, mediumHills.Persistence);
        }

        Debug.Log(sw.ElapsedMilliseconds);
        sw.Stop();*/

        //Destroy(mediumHills.texture);
        //mediumHills.texture = GPUNoiseGenerator.GenerateNoise(0, mediumHills.noiseType, mediumHills.noiseResolution, mediumHills.side, mediumHills.quarter, mediumHills.Octaves, mediumHills.Frequency, mediumHills.Amplitude, mediumHills.Lacunarity, mediumHills.Persistence);
        //setTexturesToShader();
    }

    

    void setTexturesToShader() {

        mat.SetTexture("_DebugTexture", mediumHills.texture);
        /*
        mat.SetTexture("_MedTex", medTex);
        mat.SetTexture("_HighTex", highTex);
        mat.SetTexture("_DisplacementTexture", perlinTexture);
        */
        rend.material = mat;
    }




}
