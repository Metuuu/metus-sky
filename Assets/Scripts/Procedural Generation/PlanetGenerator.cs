using UnityEngine;
using System.Collections;


public static class PlanetGenerator
{

    public static PlanetData GeneratePlanet(string name, float planetSize, Material material, AnimationCurve meshHeightCurve, TerrainType[] regions, Gradient gradient, float terrainMeshHeightMultiplier, float noiseScale, Renderer noiseTaulu, int octaves, GPUNoiseGenerator.NoiseResolution noiseResolution, bool hasAtmosphere) {
        return new PlanetData(name, planetSize, material, meshHeightCurve, regions, gradient, terrainMeshHeightMultiplier, noiseScale, noiseTaulu, octaves, noiseResolution, hasAtmosphere);
    }


}


// Planet Data
[System.Serializable]
public class PlanetData
{

    // ---- INIT ----
    GameObject planet;
    public AtmosphereData atmosphere;

    public readonly string name;
    public readonly int seed;
    public float planetSize;

    public bool hasAtmosphere;
    
    Material planetMaterial;

    int currentBiome = 0;
    Biome[] biomes;

    public Vector2 offset;
    public float noiseScale;

    float terrainMeshHeightMultiplier;

    Renderer noiseTaulu;

    Gradient gradient;

    int octaves; // <- tää siirretään sitten sillein että automaattisesti valitsee tarpeeks tarkan
    GPUNoiseGenerator.NoiseResolution noiseResolution; // ja tääkin pitäis osata valita ittesä automaattisesti
    

    // - PLANET DATA -
    public PlanetData(string name, float planetSize, Material material, AnimationCurve meshHeightCurve, TerrainType[] regions, Gradient gradient, float terrainMeshHeightMultiplier, float noiseScale, Renderer noiseTaulu, int octaves, GPUNoiseGenerator.NoiseResolution noiseResolution, bool hasAtmosphere) {
        this.name = name;
        this.seed = 1;
        this.planetSize = planetSize;

        this.noiseScale = noiseScale;
        this.offset = Vector2.zero;

        this.terrainMeshHeightMultiplier = terrainMeshHeightMultiplier;

        this.planetMaterial = material;

        biomes = new Biome[5];
        biomes[0] = new Biome("pinta", regions, meshHeightCurve); // TODO: biomes
        this.gradient = gradient;

        this.noiseTaulu = noiseTaulu;

        this.octaves = octaves;
        this.noiseResolution = noiseResolution;

        this.hasAtmosphere = hasAtmosphere;

    }



    // ---- METHODS ----
    #region

    // Generate Planet
    public GameObject GeneratePlanet() {
        planet = new GameObject(name);
        planet.AddComponent<MeshFilter>();
        planet.AddComponent<MeshRenderer>();
        planet.AddComponent<MeshCollider>();
        PlanetScript ps = planet.AddComponent<PlanetScript>();

        // Add planet script
        PlanetScript planetScript = planet.GetComponent<PlanetScript>();
        //planetScript.planetData = this; // <--------------------<----------------------<---------------------<------------------<-------- Jos käytät tätä scriptiä niin muista tää ------<------------------<
        
        if (hasAtmosphere) {
            atmosphere = AtmosphereGenerator.GenerateAtmosphere(planet.transform, planetSize / 2f +15f, 0.25f);
        }

        return planet;
    }



    // Generate Planet LOD
    public GameObject GeneratePlanetChunk(string name, int gridSize, Vector3 position, string side = "") {
        
        Mesh chunkMesh = null;
        Texture2D texture;
        float[,] heightMap;
        Material[] materials;
        Material[] debugMaterial = new Material[1];

        // Jos full niin pitää kaikki sivut generoida
        if (side[0] == 'F') {

            // TODO: LOD Full planet generation
            #region
            return new GameObject(name);
            /*
            // Luo planeetan spherical cube mesh
            gridSize = Mathf.RoundToInt(gridSize);// * 0.41f);
            chunkMesh = GenerateSphericalMesh(gridSize, side);

            // Sitten aletaan tehdä maastoa
            materials = new Material[6];
            for (int i = 0; i < 6; ++i) {
            
                texture = GenerateTexture(CHMapArea, gridSize);
                materials[i] = GenerateMaterial(texture);
                terrainMesh = GenerateTerrainMesh(CHMap);      // color/heightmapin pohjalta tehdään muodot
                GenerateChunkTerrain(chunkMesh, terrainMesh);      // yhdistää pallon ja maaston

            }
            */
            #endregion

        }
        // Generoidaan yksi sivu tai quarter
        else {

            //gridSize = gridSize - 1;
            chunkMesh = GenerateSphericalMesh(Mathf.RoundToInt(gridSize), planetSize, side); // generoi spherical meshen

			//texture = GenerateTexture(CHMap, gridSize);        // josta tehään tekstuuri

			//terrainVertices = GenerateTerrainVertices(CHMap, side, gridSize); // heightmapin pohjalta tehdään muodot

			GPUNoiseGenerator.Side s = GPUNoiseGenerator.Side.Bottom;
			switch (side.Substring(0,1)) {
				case "0":
					s = GPUNoiseGenerator.Side.Bottom;
					break;
				case "1":
					s = GPUNoiseGenerator.Side.Left;
					break;
				case "2":
					s = GPUNoiseGenerator.Side.Front;
					break;
				case "3":
					s = GPUNoiseGenerator.Side.Right;
					break;
				case "4":
					s = GPUNoiseGenerator.Side.Top;
					break;
				case "5":
					s = GPUNoiseGenerator.Side.Back;
					break;
			}
            heightMap = GenerateHeightMap(s, side.Substring(0, side.Length - 1));

            GenerateChunkTerrain(chunkMesh, heightMap);      // yhdistää pallon ja maaston


            // [ - DEBUG - ]
            Texture2D noiseTexture = TextureGenerator.TextureFromHeightMap(heightMap);
            Texture2D colorTexture = TextureGenerator.TextureFromColorMap(GenerateColorMap(heightMap,heightMap.GetLength(0)), heightMap.GetLength(0), heightMap.GetLength(1));

            noiseTaulu.material.mainTexture = colorTexture;
            //debugMaterial[0] = noiseTaulu.material;
            // [ --------- ]

            materials = new Material[1];
            //materials[0] = GenerateMaterial(noiseTexture); //  <- Generoi materiaalin -- laitto kyllä tässä vaan tekstuurin materiaaliin
            materials[0] = noiseTaulu.material;

        }


        return GeneratePlanetChunkObj(side, position, chunkMesh, chunkMesh, materials, gridSize); //new Material[0], gridSize);


    }



    float[,] GenerateHeightMap(GPUNoiseGenerator.Side side, string quarter) {
		GPUNoiseGenerator.NoiseData noise = new GPUNoiseGenerator.NoiseData {
			frequency = 1,
			octaves = octaves,
			side = side,
			quarter = quarter,
			persistence = biomes[currentBiome].persistance,
			lacunarity = biomes[currentBiome].lacunarity,
			maxNoiseHeight = 1,
			amplitude = 1,
			resolution = noiseResolution
		};
		return Noise.GenerateNoiseMap(seed, offset, noise, 1, Noise.NoiseSource.Unity);
    }


    // Generate color map
    Color[] GenerateColorMap(float[,] heightMap, int size) {//, bool useFalloff = false) {

        Color[] colorMap = new Color[size * size];

        for (int y = 0; y < size; ++y) {
            for (int x = 0; x < size; ++x) {
                /*if (useFalloff) {
                    heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - falloffMap[x, y]);
                }*/
                
                float currentHeight = heightMap[x, y];

                colorMap[y * size + x] = gradient.Evaluate(Mathf.Clamp01(heightMap[x, y]));

                /*for (int i = 0; i < biomes[0].terrainTypes.Length; ++i) {
                    if (currentHeight >= biomes[0].terrainTypes[i].height) {
                        colorMap[y * size + x] = biomes[0].terrainTypes[i].color;
                    }
                    else { break; }
                }*/

            }
        }
        return colorMap;
    }


    // Generate planet mesh
    Mesh GenerateSphericalMesh(int gridSize, float planetSize, string side = "") {
        //return SphericalCubeGenerator.GenerateMesh("Planet_mesh", planetSize, gridSize, side);
        return null; // TODO: tää filu toimii nyt tärkeenä lähteenä V2 versiota varten niin tänne jää ei toimivaa koodia
    }


    // Generate planet terrain vertices ----------- ei toimiva poista tää ja tee erillinen heightmap teko joka tekee oikeen heightmapin siden perusteella lodiin
    /*float[] GenerateTerrainVertices(ColorAndHeightMap colorAndHeightMap, string side, int gridSize) {
        return TerrainGenerator.GenerateTerrainHeight(gridSize, side, CHMap.heightMap, biomes[currentBiome].meshHeightMultiplier, biomes[currentBiome].meshHeightCurve, true);
    }*/


    // Generate chunk terrain
    void GenerateChunkTerrain(Mesh chunkMesh, float[,] heightMap) {

        int len = chunkMesh.vertices.Length;

        Vector3[] newVertices = new Vector3[len];
        Vector3[] chunkVertices = chunkMesh.vertices;
        Vector3[] chunkNormals = chunkMesh.normals;

        int heightMapLength = heightMap.GetLength(0);
        int verticesLengthSqrt = (int)Mathf.Sqrt(len); // vertices length = (gridsize + 1)^2 eli sqrt muuttaa sen vaan (gridsize + 1)
        float incresiment = (float)(heightMapLength)/(float)(verticesLengthSqrt);

        float x = 0, y = 0;
        int xx = 0, yy = Mathf.FloorToInt(incresiment / 2);

        for (int i = 0; i < len; ++i, x+=incresiment) {
            
            if (x >= heightMapLength-1) {
                x = 0;
                y += incresiment;
                yy = Mathf.FloorToInt(y + incresiment / 2);
                
            }
            xx = Mathf.FloorToInt(x + incresiment/2);

            //Debug.Log("X: " + xx + ", Y: " + yy + " | " + heightMap[xx, yy]);

            // Kertoo spherical cuben normaalit heightmapillä että saa muodot pinnalle. lukee siis heigthmapista pinnan verticeä vastaavan pisteen korkeuden
            // (voisi laittaa ottamaan keskimääräisen siltä alueelta mutta olisi tietenkin hitaampi) 
            newVertices[i].x = chunkVertices[i].x + chunkNormals[i].x * heightMap[xx,yy] * terrainMeshHeightMultiplier;
            newVertices[i].y = chunkVertices[i].y + chunkNormals[i].y * heightMap[xx,yy] * terrainMeshHeightMultiplier;
            newVertices[i].z = chunkVertices[i].z + chunkNormals[i].z * heightMap[xx,yy] * terrainMeshHeightMultiplier;
        }

        chunkMesh.vertices = newVertices;

    }


    // Generate planet texture
    Texture2D GenerateTexture(Color[] colorMap, int size) {
        return TextureGenerator.TextureFromColorMap(colorMap, size, size);
    }


    // Generate planet material
    Material GenerateMaterial(Texture2D texture) {
        Material mat = new Material(planetMaterial);
        mat.SetTexture("texture", texture);
        return mat;
    }




    // Generate planet chunk object
    GameObject GeneratePlanetChunkObj(string name, Vector3 position, Mesh mesh, Mesh meshCollider, Material[] materials, int gridSize) {

        GameObject planetChunk = new GameObject(name);
        planetChunk.AddComponent<MeshFilter>().mesh = mesh;
        planetChunk.AddComponent<MeshRenderer>();
        //planetChunk.AddComponent<MeshCollider>().sharedMesh = meshCollider;

        Renderer rend = planetChunk.GetComponent<Renderer>();

        rend.materials = materials;

        Vector2 matTiling = rend.material.mainTextureScale;
        rend.material.mainTextureScale = new Vector2(matTiling.x / gridSize, matTiling.y / gridSize);

        planetChunk.transform.position = position;

        return planetChunk;
    }

    #endregion
    



    // ---- CHILD CLASSES ----


    // - Biome - EI EI EI EI EI NÄIN TAI EHKÄ EN TIIÄ
    // TODO: Biomes -> tänne kaikki octaves, persistance, lacunarity multiplier, falloff jne sitten prodecurally generated.
    public enum Biomes { stone = 0, sand = 1, lava = 2 }
    public class Biome
    {
        public string name;
        public TerrainType[] terrainTypes;
        
        public float persistance;
        public float lacunarity;
        public float meshHeightMultiplier;
        public AnimationCurve meshHeightCurve;


        // Initialization
        public Biome(string name, TerrainType[] terrainTypes, AnimationCurve meshHeightCurve) {
            this.name = name;
            this.meshHeightCurve = meshHeightCurve;
            this.persistance = 0.5f;
            this.lacunarity = 2f;
            this.terrainTypes = terrainTypes;
            this.meshHeightMultiplier = 1f;

            // TODO: random generate terrain types ja kaikki muu
        }


        // TODO: Terrain type creation inside biome
        /* // Terrain type
        public class TerrainType
        {
            public string name;
            public float height;
            public Color color;
            // TODO: terrain type angle. Different angle = different color;
            // TODO: terrain type color to material with texture
        }*/
    }



}







// Terrain type
[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}


// Map data
public struct MapData {
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap) {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }

}