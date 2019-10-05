/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

	public bool hasAtmosphere;



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
	public GPUPlanetData(string name, float planetSize, GPUNoiseGenerator.NoiseData ocean, GPUNoiseGenerator.NoiseData plainHills, GPUNoiseGenerator.NoiseData largeMountains, GPUNoiseGenerator.NoiseData mediumDetail, bool hasAtmosphere) {
		this.name = name;
		this.seed = 1;
		this.planetSize = planetSize;

		this.ocean = ocean;
		this.plainHills = plainHills;
		this.largeMountains = largeMountains;
		this.mediumDetail = mediumDetail;

		this.hasAtmosphere = hasAtmosphere;

	}



	// ---- METHODS ----
	#region

	// Generate Planet
	public GameObject GeneratePlanet(Vector3 position) {
		planet = new GameObject(name);
		planet.AddComponent<MeshFilter>();
		planet.AddComponent<MeshRenderer>();
		planet.AddComponent<MeshCollider>();
		PlanetScript ps = planet.AddComponent<PlanetScript>();
		planet.transform.position = position;

		// Add planet script
		PlanetScript planetScript = planet.GetComponent<PlanetScript>();
		planetScript.planetData = this;

		if (hasAtmosphere) {
			atmosphere = AtmosphereGenerator.GenerateAtmosphere(planet.transform, planetSize / 2f + 15f, 0.25f);
		}

		return planet;
	}



	// Generate Planet LOD - Versio jota ei ole muokattu
	public GameObject GeneratePlanetChunk(int gridSize, Vector3 position, ref List<RenderTexture> renderTextureList, string side = "") {

		Vector2 rotation = Vector3.zero;
		Mesh chunkMesh = null;
		Material[] materials;

		// Jos full niin pitää kaikki sivut generoida
		if (side[0] == 'F') {

			// TODO: LOD Full planet generation jos ees haluun sitä
			return new GameObject(side);
		}
		// Generoidaan yksi sivu tai quarter
		else {

			chunkMesh = GenerateSphericalMesh(Mathf.RoundToInt(gridSize), side); // generoi spherical meshen

			materials = new Material[1];
			materials[0] = new Material(Shader.Find("Shader Forge/Terrain"));


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

			RenderTexture oceanTex = GPUNoiseGenerator.GenerateNoise(seed, ocean);
			RenderTexture plainHillsTex = GPUNoiseGenerator.GenerateNoise(seed, plainHills);
			RenderTexture largeMountainsTex = GPUNoiseGenerator.GenerateNoise(seed, largeMountains);
			RenderTexture mediumDetailTex = GPUNoiseGenerator.GenerateNoise(seed, mediumDetail);
			materials[0].SetTexture(OCEAN_NOISE_TEXTURE_NAME, oceanTex);
			materials[0].SetTexture(PLAIN_HILLS_NOISE_TEXTURE_NAME, plainHillsTex);
			materials[0].SetTexture(LARGE_MOUNTAINS_NOISE_TEXTURE_NAME, largeMountainsTex);
			materials[0].SetTexture(MEDIUM_DETAIL_NOISE_TEXTURE_NAME, mediumDetailTex);

			renderTextureList = new List<RenderTexture> {
				oceanTex,
				plainHillsTex,
				largeMountainsTex,
				mediumDetailTex
			};

			/*
            mat.SetTexture("_MedTex", );
            mat.SetTexture("_HighTex", );
            mat.SetTexture("_DisplacementTexture", );
            * /

		}


		return GeneratePlanetChunkObj(side, position, planetSize, rotation, chunkMesh, chunkMesh, materials, gridSize);


	}


	// Generate planet mesh
	Mesh GenerateSphericalMesh(int gridSize, string side = "") {
		return SphericalCubeGenerator.GetMesh(gridSize, side);
	}



	// Generate planet chunk object - Ei muokattu
	GameObject GeneratePlanetChunkObj(string side, Vector3 position, float planetSize, Vector2 rotation, Mesh mesh, Mesh meshCollider, Material[] materials, int gridSize) {

		GameObject planetChunk = new GameObject(side);
		planetChunk.AddComponent<MeshFilter>().mesh = mesh;
		planetChunk.AddComponent<MeshRenderer>();
		//planetChunk.AddComponent<MeshCollider>().sharedMesh = meshCollider;


		Renderer rend = planetChunk.GetComponent<Renderer>();

		//rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		rend.materials = materials;


		// Set scale multiplier
		int shrinkMultiplier = (int)Mathf.Pow(2, (QualitySettings.CurrentLOD.LodsCount - side.Length));
		/*if (shrinkMultiplier != 2) {* /
		shrinkMultiplier *= 8; //}

		rend.material.SetFloat("_TextureShrinkMultiplier", shrinkMultiplier);

		// Perlin noise maps
		Vector2 matTiling = rend.material.GetTextureScale(PLAIN_HILLS_NOISE_TEXTURE_NAME);
		Vector2 textureScale = new Vector2(matTiling.x / gridSize, matTiling.y / gridSize);
		rend.material.SetTextureScale(OCEAN_NOISE_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(PLAIN_HILLS_NOISE_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(LARGE_MOUNTAINS_NOISE_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(MEDIUM_DETAIL_NOISE_TEXTURE_NAME, textureScale);

		rend.material.SetTextureScale(OCEAN_BOTTOM_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(DEFAULT_GROUND_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(MOUNTAINS_TEXTURE_NAME, textureScale);
		rend.material.SetTextureScale(MOUNTAIN_TOP_TEXTURE_NAME, textureScale);

		planetChunk.transform.position = position;
		planetChunk.transform.localRotation = Quaternion.Euler(rotation);
		planetChunk.transform.localScale = new Vector3(planetSize, planetSize, planetSize);

		return planetChunk;
	}

	#endregion


}*/