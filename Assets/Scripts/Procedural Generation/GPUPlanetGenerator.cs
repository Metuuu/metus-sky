using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using System;


// Planet Data
[System.Serializable]
public class GPUPlanetData {

	// ---- INIT ----
	GameObject planet;
	public AtmosphereData atmosphere;

	public readonly string name;
	public readonly int seed;

	public GPUNoiseGenerator.NoiseData ocean;
	public GPUNoiseGenerator.NoiseData plainHills;
	public GPUNoiseGenerator.NoiseData largeMountains;
	public GPUNoiseGenerator.NoiseData mediumDetail;

	public float planetSize;
	public float radius;

	public bool hasAtmosphere;

	public float mass;
	public int mass10pow;

	public Noise.NoiseSource noiseSource;



	// Material texture names
	private static readonly string OCEAN_BOTTOM_TEXTURE_NAME = "_OceanBottomTexture";
	private static readonly string DEFAULT_GROUND_TEXTURE_NAME = "_DefaultGroundTexture";
	private static readonly string MOUNTAINS_TEXTURE_NAME = "_MountainsTexture";
	private static readonly string MOUNTAIN_TOP_TEXTURE_NAME = "_MountainTopTexture";

	private static readonly string OCEAN_NOISE_TEXTURE_NAME = "_Ocean";
	private static readonly string PLAIN_HILLS_NOISE_TEXTURE_NAME = "_PlainHills";
	private static readonly string LARGE_MOUNTAINS_NOISE_TEXTURE_NAME = "_LargeMountains";
	private static readonly string MEDIUM_DETAIL_NOISE_TEXTURE_NAME = "_MediumDetail";



	// - PLANET DATA -
	public GPUPlanetData(string name, float planetSize, GPUNoiseGenerator.NoiseData ocean, GPUNoiseGenerator.NoiseData plainHills, GPUNoiseGenerator.NoiseData largeMountains, GPUNoiseGenerator.NoiseData mediumDetail, bool hasAtmosphere, Noise.NoiseSource noiseSource) {
		this.name = name;
		this.seed = 1;
		this.planetSize = planetSize;
		this.radius = planetSize / 2f;

		this.ocean = ocean;
		this.plainHills = plainHills;
		this.largeMountains = largeMountains;
		this.mediumDetail = mediumDetail;

		this.hasAtmosphere = hasAtmosphere;

		this.noiseSource = noiseSource;

		//int remainder = (int)(planetSize % 500); // TODO: calculate properly
		//mass = 1 + remainder / 500f;
		//mass10pow = (int)(planetSize - remainder) / 500;

		mass = 1;
		mass10pow = 21;

	}



	// ---- METHODS ----
	#region

	// Generate Planet
	public GameObject GeneratePlanet(Vector3 position) {
		planet = new GameObject(name);
		planet.tag = "Planet";
		planet.AddComponent<MeshFilter>();
		planet.AddComponent<MeshRenderer>();

		PlanetScript ps = planet.AddComponent<PlanetScript>();
		ps.planetData = this;
		GravityAttractor ga = planet.AddComponent<GravityAttractor>();

		if (hasAtmosphere) {
			atmosphere = AtmosphereGenerator.GenerateAtmosphere(planet.transform, planetSize / 2f, 0.1f);
			//atmosphere = AtmosphereGenerator.GenerateAtmosphere(planet.transform, planetSize / 2f + 12f, 0.1f);
		}

		planet.transform.position = position;

		return planet;
	}



	// ...
	/*public struct GenerateNoise : IJob {
		public int seed;
		public Noise.NoiseSource noiseSource;
		public GPUNoiseGenerator.NoiseData noiseData;
		public NativeArray<float> heightMapX;
		public NativeArray<float> heightMapY;

		public void Execute() {
			float[,] heightMap;

			switch (noiseSource) {
				case Noise.NoiseSource.GPU_2DFloatArray:
					heightMap = GPUNoiseGenerator.GenerateNoiseArray(seed, noiseData);
					break;
				case Noise.NoiseSource.GPU_RenderTextureTo2DFloatArray:
					heightMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, noiseData);
					break;
				default:
					throw new System.Exception("Not Implemented");
			}

			float[] x = new float[heightMap.Length];
			float[] y = new float[heightMap.Length];
			for (int i = 0; i < heightMap.Length; ++i) {
				x[i] = heightMap[i, 0];
				y[i] = heightMap[i, 1];
			}
			heightMapX.CopyFrom(x);
			heightMapY.CopyFrom(y);
		}
	}*/

	// Generate Planet LOD
	public GameObject GeneratePlanetChunk(int gridSize, Vector3 position, ref List<RenderTexture> renderTextureList, bool hasHeight, bool hasCollider, string side = "") {

		string name = "";
		Vector2 rotation = Vector3.zero;
		Mesh chunkMesh = null;
		Material[] materials;
		float[][,] heightMaps;

		// Jos full niin pitää kaikki sivut generoida
		if (side[0] == 'F') {
			name = "F";
			// TODO: LOD Full planet generation jos ees haluun sitä
			return new GameObject(side);
		}
		// Generoidaan yksi sivu tai quarter
		else {

			chunkMesh = GenerateSphericalMesh(Mathf.RoundToInt(gridSize), side); // generoi spherical meshen

			materials = new Material[1];
			materials[0] = new Material(Shader.Find("Legacy Shaders/Diffuse"));

			//materials[0] = new Material(Shader.Find("Shader Forge/Terrain"));


			//Destroy(mediumHills.texture);

			GPUNoiseGenerator.Side planetSide;

			switch (side[0]) {
				case '0':
					planetSide = GPUNoiseGenerator.Side.Bottom;
					rotation = new Vector2(270, 0);
					break;
				case '1':
					planetSide = GPUNoiseGenerator.Side.Left;
					rotation = new Vector2(0, 90);
					break;
				case '2':
					planetSide = GPUNoiseGenerator.Side.Front;
					rotation = new Vector2(0, 0);
					break;
				case '3':
					planetSide = GPUNoiseGenerator.Side.Right;
					rotation = new Vector2(0, -90);
					break;
				case '4':
					planetSide = GPUNoiseGenerator.Side.Top;
					rotation = new Vector2(90, 0);
					break;
				default: //case '5':
					planetSide = GPUNoiseGenerator.Side.Back;
					rotation = new Vector2(180, 0);
					break;
			}
			

			// TODO: en tiiä pitäiskö vaan parametrina laittaa planet side ja side noise generaattoriin ettei tarttee jokaiselle laittaa niitä erikseen
			ocean.side = planetSide;
			plainHills.side = planetSide;
			largeMountains.side = planetSide;
			mediumDetail.side = planetSide;

			side = side.Substring(1);
			ocean.quarter = side;
			plainHills.quarter = side;
			largeMountains.quarter = side;
			mediumDetail.quarter = side;

			name = planetSide.ToString();
			if (side != "") {
				name += "_" + side;
			}

			float[,] oceanHeightMap = new float[(int)ocean.resolution, (int)ocean.resolution];
			float[,] plainHillsHeightMap = new float[(int)plainHills.resolution, (int)plainHills.resolution];
			float[,] largeMountainsHeightMap = new float[(int)largeMountains.resolution, (int)largeMountains.resolution];
			float[,] mediumDetailHeightMap = new float[(int)mediumDetail.resolution, (int)mediumDetail.resolution];

			switch (noiseSource) {
				case Noise.NoiseSource.GPU_2DFloatArray:
					oceanHeightMap = GPUNoiseGenerator.GenerateNoiseArray(seed, ocean);
					plainHillsHeightMap = GPUNoiseGenerator.GenerateNoiseArray(seed, plainHills);
					largeMountainsHeightMap = GPUNoiseGenerator.GenerateNoiseArray(seed, largeMountains);
					mediumDetailHeightMap = GPUNoiseGenerator.GenerateNoiseArray(seed, mediumDetail);
					break;
				case Noise.NoiseSource.GPU_RenderTextureTo2DFloatArray:
					oceanHeightMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, ocean);
					plainHillsHeightMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, plainHills);
					largeMountainsHeightMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, largeMountains);
					mediumDetailHeightMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, mediumDetail);
					break;
				default:
					throw new System.Exception("Not Implemented");
			}

			//heightMaps = new float[][,] { oceanHeightMap, plainHillsHeightMap, largeMountainsHeightMap, mediumDetailHeightMap };
			heightMaps = new float[][,] { oceanHeightMap, plainHillsHeightMap, largeMountainsHeightMap };

			//RenderTexture oceanTex = GPUNoiseGenerator.GenerateNoise(seed, ocean);
			//RenderTexture plainHillsTex = GPUNoiseGenerator.GenerateNoise(seed, plainHills);
			//RenderTexture largeMountainsTex = GPUNoiseGenerator.GenerateNoise(seed, largeMountains);
			//RenderTexture mediumDetailTex = GPUNoiseGenerator.GenerateNoise(seed, mediumDetail);
			//materials[0].SetTexture(OCEAN_NOISE_TEXTURE_NAME, oceanTex);
			//materials[0].SetTexture(PLAIN_HILLS_NOISE_TEXTURE_NAME, plainHillsTex);
			//materials[0].SetTexture(LARGE_MOUNTAINS_NOISE_TEXTURE_NAME, largeMountainsTex);
			//materials[0].SetTexture(MEDIUM_DETAIL_NOISE_TEXTURE_NAME, mediumDetailTex);

			//materials[0].SetTexture("_MainTex", oceanTex);

			renderTextureList = new List<RenderTexture> {
				//oceanTex,
                //plainHillsTex,
                //largeMountainsTex
                //mediumDetailTex
            };

			//
			//mat.SetTexture("_MedTex", );
			//mat.SetTexture("_HighTex", );
			//mat.SetTexture("_DisplacementTexture", );
			//


		}

		return GeneratePlanetChunkObj(name, side, position, planetSize, rotation, chunkMesh, materials, heightMaps, gridSize, hasHeight, hasCollider);
	}



	// Generate planet mesh
	Mesh GenerateSphericalMesh(int gridSize, string side = "") {
		return SphericalCubeGenerator.GetMesh(gridSize, side);
	}



	// Generate planet chunk object
	GameObject GeneratePlanetChunkObj(string name, string side, Vector3 position, float planetSize, Vector2 rotation, Mesh mesh, Material[] materials, float[][,] heightMaps, int gridSize, bool hasHeight, bool hasCollider) {

		GameObject planetChunk = new GameObject(name);
		planetChunk.AddComponent<MeshFilter>().mesh = mesh;
		planetChunk.AddComponent<MeshRenderer>();

		Renderer rend = planetChunk.GetComponent<Renderer>();

		//rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		rend.materials = materials;


		// Set scale multiplier
		//int shrinkMultiplier = (int)Mathf.Pow(2, (QualitySettings.CurrentLOD.LodsCount - side.Length));
		//if (shrinkMultiplier != 2) { // ------------------------ poista kommenteista kaikki paitsi tämä. tein compute shader testejä ... (enää en kyllä muista mitään ja mitkä kommentit pitää jättää)
		//shrinkMultiplier *= 8; //}

		//rend.material.SetFloat("_TextureShrinkMultiplier", shrinkMultiplier);

		// Perlin noise maps
		//Vector2 matTiling = rend.material.GetTextureScale(PLAIN_HILLS_NOISE_TEXTURE_NAME);
		//Vector2 textureScale = new Vector2(matTiling.x / gridSize, matTiling.y / gridSize);
		//rend.material.SetTextureScale(OCEAN_NOISE_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(PLAIN_HILLS_NOISE_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(LARGE_MOUNTAINS_NOISE_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(MEDIUM_DETAIL_NOISE_TEXTURE_NAME, textureScale);

		//rend.material.SetTextureScale(OCEAN_BOTTOM_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(DEFAULT_GROUND_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(MOUNTAINS_TEXTURE_NAME, textureScale);
		//rend.material.SetTextureScale(MOUNTAIN_TOP_TEXTURE_NAME, textureScale);



		//float[,] heightM = heightMaps[0];

		//int reso = (int)(Mathf.Sqrt(heightM.Length) / 32f);




		//RenderTexture texture = RenderTexture.GetTemporary(reso, reso, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear); // TODO: KORJAA MEMORY LEAK ? tää ollut myös RHalf mutta nyt compute shader testeihin varmuuden vuoks RFloat
		//texture.enableRandomWrite = true;
		//texture.Create();


		//ComputeShader shader1 = (ComputeShader)Resources.Load("Shaders/DepthAndTesselationCS");
		//int kernelIndex1 = shader1.FindKernel("TextureTest");

		//ComputeBuffer hmb = new ComputeBuffer(heightM.Length, sizeof(float), ComputeBufferType.Default);
		//hmb.SetData(heightM);

		//shader1.SetBuffer(kernelIndex1, "heightMapBuffer", hmb);
		//shader1.SetTexture(kernelIndex1, "heightMap", texture);
		//shader1.Dispatch(kernelIndex1, reso, reso, 1);

		//hmb.Dispose();
		//hmb.Release();
		//hmb = null;

		//Texture2D texture = TextureGenerator.TextureFromHeightMap(heightMap);

		float textureScale;
		if (side.Length < 5)
			textureScale = 0.125f;
		else
			textureScale = 0.125f / 8f;

		//rend.material.SetTextureScale("_MainTex", new Vector2(128f / shrinkMultiplier, 128f / shrinkMultiplier));
		rend.material.SetTextureScale("_MainTex", new Vector2(textureScale, textureScale));



		//ComputeShader shader = (ComputeShader)Resources.Load("Shaders/DepthAndTesselationCS"); // TÄMÄ IHAN KESKEN
		//int kernelIndex = shader.FindKernel("DepthAndTesselation");

		//shader.SetFloat("Persistence", noiseType.persistence);
		//shader.SetVector("Offset", offset);
		//shader.SetTexture(kernelIndex, "tex", texture);

		if (hasHeight) {
			MeshFilter mf = planetChunk.GetComponent<MeshFilter>();
			Vector3[] vertices = mf.mesh.vertices;
			Vector3[] normals = mf.mesh.normals;

			int verticesCount = vertices.Length;
			int verticesWidth = (int)Mathf.Sqrt(verticesCount);

			Vector3[] newVertices = vertices;
			Vector3 newVertex;
			float heightmapPoint;

			for (int i = 0; i < heightMaps.Length; ++i) {
				float[,] heightMap = heightMaps[i];
				int resolution = heightMap.GetLength(0);
				float hmPosMultiplier = (float)(resolution - 1) / ((float)(verticesWidth - 1));
				//Debug.Log(" ---------- " + name + " ---------- ");


				for (int x = 0; x < verticesWidth; ++x) {
					for (int y = 0; y < verticesWidth; ++y) {
						heightmapPoint = heightMap[Mathf.RoundToInt(x * hmPosMultiplier), Mathf.RoundToInt(y * hmPosMultiplier)];

						//if (y == 0 || x == 0 || y == verticesWidth - 1 || x == verticesWidth - 1) {
						//	if (Math.Abs(heightmapPoint) > 2) Debug.Log(heightmapPoint);
						//	heightmapPoint = (float)Math.Round(heightmapPoint, 1);
						//}

						newVertex = (normals[x + y * verticesWidth].normalized * heightmapPoint)/100f;
						//newVertex = new Vector3((float)Math.Round(newVertex.x, 2), (float)Math.Round(newVertex.y, 2), (float)Math.Round(newVertex.z, 2));
						newVertices[x + y * verticesWidth] += newVertex;

						//if (y == 0 && x == 0) {
						//	Debug.Log(
						//		//"OIK Y: " +
						//		"x: " + aaa.x + ", y: " + aaa.y + ", z: " + aaa.z
						//	);
						//}
						//if (y == verticesWidth - 1 && x == verticesWidth - 1) {
						//	Debug.Log(
						//		//"VAS A: " +
						//		"x: " + aaa.x + ", y: " + aaa.y + ", z: " + aaa.z
						//	);
						//}
						//if (y == verticesWidth - 1 && x == 0) {
						//	Debug.Log(
						//		//"OIK A: " +
						//		"x: " + aaa.x + ", y: " + aaa.y + ", z: " + aaa.z
						//	);
						//}
						//if (y == 0 && x == verticesWidth - 1) {
						//	Debug.Log(
						//		//"VAS Y: " +
						//		"x: " + aaa.x + ", y: " + aaa.y + ", z: " + aaa.z
						//	);
						//}
					}
				}
			}

			//ComputeBuffer vertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3, ComputeBufferType.Default);
			//vertexBuffer.SetData(vertices);
			//ComputeBuffer normalBuffer = new ComputeBuffer(normals.Length, sizeof(float) * 3, ComputeBufferType.Default);
			//normalBuffer.SetData(normals);
			//ComputeBuffer heightMapBuffer = new ComputeBuffer(heightMap.Length, sizeof(float), ComputeBufferType.Default);
			//heightMapBuffer.SetData(heightMap);


			//string debugString = "";
			//for (int i = 0; i < heightMap.Length; i += 1024) {
			//    debugString += heightMap[i] + ", ";
			//}
			//Debug.Log(debugString);


			//shader.SetBuffer(kernelIndex, "vertexBuffer", vertexBuffer);
			//shader.SetBuffer(kernelIndex, "normalBuffer", normalBuffer);
			//shader.SetBuffer(kernelIndex, "heightMapBuffer", heightMapBuffer);

			//shader.Dispatch(kernelIndex, vertices.Length, 1, 1);


			//vertexBuffer.GetData(newVertices);
			//heightMapBuffer.GetData(heightMap);
			mf.mesh.vertices = newVertices;
			/*mf.mesh.vertices = vertices;*/
			mf.mesh.RecalculateNormals(); // Important! Jos ei käytä niin kaikki shadows menee esim ihan pilalle

			//for (int i = 0; i < vertices.Length; ++i) {
			//    Debug.Log("-----");
			//    //Debug.Log(normals[i]);
			//    Debug.Log(vertices[i]);
			//    Debug.Log(newVertices[i]);
			//}

			//vertexBuffer.Dispose();
			//vertexBuffer.Release();
			//vertexBuffer = null;
			//normalBuffer.Dispose();
			//normalBuffer.Release();
			//normalBuffer = null;
			//heightMapBuffer.Dispose();
			//heightMapBuffer.Release();
			//heightMapBuffer = null;

			//mf.mesh.vertices

			//for (int i = 0; i < vertices.Length; ++i) {
			//    vertex = vertices[i];
			//    vertex.y += newVertices[i].y;
			//}
		}

		// Collider
		if (hasCollider) {
			planetChunk.AddComponent<MeshCollider>();
		}

		// Finalize
		planetChunk.transform.position = position;
		planetChunk.transform.localRotation = Quaternion.Euler(rotation);
		planetChunk.transform.localScale = new Vector3(planetSize, planetSize, planetSize);

		//heightMap = Noise.GenerateNoiseMap(plainHills.resolution, seed, plainHills.side, side, Vector2.zero, 1, plainHills.octaves, plainHills.frequency, plainHills.amplitude, plainHills.persistence, plainHills.lacunarity, noiseSource);
		//Texture2D noiseTexture = TextureGenerator.TextureFromHeightMap(heightMap2);
		//rend.material.mainTexture = noiseTexture;

		Texture2D texture = (Texture2D)Resources.Load("Textures/Ground/Sand_Beach");
		rend.material.SetTexture("_MainTex", texture);
		rend.material.SetTextureScale("_MainTex", new Vector2(100, 100));


		//planetChunk.AddComponent<DrawNormals>();

		return planetChunk;
	}



	#endregion


}
 