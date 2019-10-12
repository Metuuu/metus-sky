using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTest : MonoBehaviour {


	// - Init -
	//[SerializeField] GameObject plane;
	Renderer planeRenderer;
	MeshFilter mf;
	float[,] heightMap;
	Vector3[] originalVertices;
	float originalScale = 0;
	float radius = 100;

	[SerializeField] int noisePerformanceTestLoopTimes = 1;
	[SerializeField] Noise.NoiseSource noiseSource;
	[SerializeField] int seed = 1;
	[SerializeField] int octaves = 4;
	[SerializeField] float frequency = 1;
	[SerializeField] float amplitude = 1;
	[SerializeField] float persistance = 0.5f;
	[SerializeField] float lacunarity = 1.87f;
	[SerializeField] float maxNoiseHeight = 1f;
	[SerializeField] Vector3 offset = new Vector3(0, 0, 0);
	[SerializeField] float zoom = 1;

	[SerializeField] float noiseScale = 10;
	[SerializeField] GPUNoiseGenerator.NoiseResolution noiseResolution = GPUNoiseGenerator.NoiseResolution.LOW;
	[SerializeField] int gridSize = 24;
	[SerializeField] bool useRenderTextureForDrawingTexture = false;

	[SerializeField] GPUNoiseGenerator.Side side = GPUNoiseGenerator.Side.Bottom;
	[SerializeField] string quarter = "";

	[SerializeField] bool enableHeight = false;
	[SerializeField] bool autoUdpate = true;


	int lastNoisePerformanceTestLoopTimes = 0;
	Noise.NoiseSource lastNoiseSource;
	int lastSeed = 0;
	int lastOctaves = 0;
	float lastFrequency = 0;
	float lastAmplitude = 0;
	float lastPersistance = 0;
	float lastLacunarity = 0;
	float lastMaxNoiseHeight = 0;
	Vector3 lastOffset = new Vector3(0, 0, 0);
	float lastNoiseScale = 0;
	float lastZoom = 1;
	bool lastUseRenderTextureForDrawingTexture = false;
	GPUNoiseGenerator.NoiseResolution lastNoiseResolution = GPUNoiseGenerator.NoiseResolution.VERY_LOW;
	int lastGridSize = 0;
	GPUNoiseGenerator.Side lastSide = GPUNoiseGenerator.Side.Bottom;
	string lastQuarter = "";
	bool lastEnableHeight = false;

	RenderTexture rt = null;


	// - Start -
	void Start() {
		planeRenderer = gameObject.GetComponent<Renderer>();
		mf = gameObject.GetComponent<MeshFilter>();

		originalScale = transform.localScale.x;

		//gridSize = (int)noiseResolution;

		SetMeshGrid(gridSize, true);
		originalVertices = (Vector3[])mf.mesh.vertices.Clone();

	}

	// - Update -
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) || (autoUdpate && (noisePerformanceTestLoopTimes != lastNoisePerformanceTestLoopTimes || noiseSource != lastNoiseSource || seed != lastSeed || octaves != lastOctaves || frequency != lastFrequency || amplitude != lastAmplitude || persistance != lastPersistance || lacunarity != lastLacunarity || maxNoiseHeight != lastMaxNoiseHeight || offset != lastOffset || noiseScale != lastNoiseScale || zoom != lastZoom || noiseResolution != lastNoiseResolution || gridSize != lastGridSize || lastUseRenderTextureForDrawingTexture != useRenderTextureForDrawingTexture || side != lastSide || quarter != lastQuarter || enableHeight != lastEnableHeight))) {

			if (noiseSource != Noise.NoiseSource.GPU_RenderTexture) {
				DrawNoiseToPlane();
			} else {
				DrawNoiseToPlaneGPURenderTexture();
			}

			//if (lastNoiseResolution != noiseResolution) {
			if (lastGridSize != gridSize) {
				SetMeshGrid(gridSize);
				originalVertices = (Vector3[])mf.mesh.vertices.Clone();
			}

			if (enableHeight)
				AddHeightToPlane();
			else
				ClearPlaneHeight();

			lastNoisePerformanceTestLoopTimes = noisePerformanceTestLoopTimes;
			lastNoiseSource = noiseSource;
			lastSeed = seed;
			lastOctaves = octaves;
			lastFrequency = frequency;
			lastAmplitude = amplitude;
			lastPersistance = persistance;
			lastLacunarity = lacunarity;
			lastMaxNoiseHeight = maxNoiseHeight;
			lastOffset = offset;
			lastNoiseScale = noiseScale;
			lastZoom = zoom;
			lastNoiseResolution = noiseResolution;
			lastGridSize = gridSize;
			lastSide = side;
			lastQuarter = quarter;
			lastEnableHeight = enableHeight;
			lastUseRenderTextureForDrawingTexture = useRenderTextureForDrawingTexture;
		}
	}

	void OnDestroy() {
		if (rt != null)
			rt.Release();
	}


	// Draw noisemap to the plane
	void DrawNoiseToPlane() {
		GPUNoiseGenerator.NoiseData noiseData = new GPUNoiseGenerator.NoiseData {
			frequency = frequency,
			octaves = octaves,
			side = side,
			quarter = quarter,
			persistence = persistance,
			lacunarity = lacunarity,
			maxNoiseHeight = maxNoiseHeight,
			amplitude = amplitude,
			resolution = noiseResolution
		};

		heightMap = Noise.GenerateNoiseMap(seed, offset, noiseData, noiseScale, noiseSource, gridSize, zoom, noisePerformanceTestLoopTimes, radius);

		if (!useRenderTextureForDrawingTexture) {
			Texture2D noiseTexture = TextureGenerator.TextureFromHeightMap(heightMap);
			planeRenderer.material.mainTexture = noiseTexture;
		} else {
			rt = GPUNoiseGenerator.GenerateNoise(seed, noiseData, radius);
			planeRenderer.material.mainTexture = rt;
		}
	}

	void DrawNoiseToPlaneGPURenderTexture() {
		System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
		st.Start();
		for (int performanceLoopIndex = 0; performanceLoopIndex < noisePerformanceTestLoopTimes; ++performanceLoopIndex) {
			GPUNoiseGenerator.NoiseData noiseData = new GPUNoiseGenerator.NoiseData {
				frequency = frequency,
				octaves = octaves,
				side = side,
				quarter = quarter,
				lacunarity = lacunarity,
				maxNoiseHeight = maxNoiseHeight,
				persistence = persistance,
				amplitude = amplitude,
				resolution = noiseResolution
			};
			if (rt != null)
				rt.Release();
			rt = GPUNoiseGenerator.GenerateNoise(seed, noiseData, radius);
		}
		st.Stop();
		if (noisePerformanceTestLoopTimes != 1)
			Debug.Log(string.Format("Generated noise with {0} {1} times and it took {2} ms to complete.", noiseSource.ToString(), noisePerformanceTestLoopTimes, st.ElapsedMilliseconds));

		planeRenderer.material.mainTexture = rt;
	}

	void AddHeightToPlane() {
		ClearPlaneHeight();
		Vector3[] vertices = mf.mesh.vertices;
		Vector3[] normals = mf.mesh.normals;

		int verticesCount = vertices.Length;
		int verticesWidth = (int)Mathf.Sqrt(verticesCount);

		Vector3[] newVertices = new Vector3[verticesCount];

		int resolution = heightMap.GetLength(0);
		float hmPosMultiplier = (float)(resolution - 1) / (float)(verticesWidth - 1);

		/*Debug.Log("------ res, vetices, hmposmulti ------");
		Debug.Log(resolution);
		Debug.Log(verticesWidth);
		Debug.Log(hmPosMultiplier);*/
		//Debug.Log("------ " + gameObject.name + " ------");

		for (int x = 0; x < verticesWidth; ++x) {
			for (int y = 0; y < verticesWidth; ++y) {
				/*if (y == 0 && x == 0) {
					Debug.Log(
						"OIK Y: " + Mathf.RoundToInt(x * hmPosMultiplier) + ", " + Mathf.RoundToInt(y * hmPosMultiplier) + " -- " +""
						//heightMap[Mathf.RoundToInt(x * hmPosMultiplier) - 1, Mathf.RoundToInt(y * hmPosMultiplier) - 1]
					);
				}
				if (y == verticesWidth - 1 && x == verticesWidth - 1) {
					Debug.Log(
						"VAS A: " + Mathf.RoundToInt(x * hmPosMultiplier) + ", " + Mathf.RoundToInt(y * hmPosMultiplier) + " -- " +""
						//heightMap[Mathf.RoundToInt(x * hmPosMultiplier) - 1, Mathf.RoundToInt(y * hmPosMultiplier) - 1]
					);
				}
				if (y == verticesWidth - 1 && x == 0) {
					Debug.Log(
						"OIK A: " + Mathf.RoundToInt(x * hmPosMultiplier) + ", " + Mathf.RoundToInt(y * hmPosMultiplier) + " -- " +""
						//heightMap[Mathf.RoundToInt(x * hmPosMultiplier) - 1, Mathf.RoundToInt(y * hmPosMultiplier) - 1]
					);
				}
				if (y == 0 && x == verticesWidth - 1) {
					Debug.Log(
						"VAS Y: " + Mathf.RoundToInt(x * hmPosMultiplier) + ", " + Mathf.RoundToInt(y * hmPosMultiplier) + " -- " +""
						//heightMap[Mathf.RoundToInt(x * hmPosMultiplier) - 1, Mathf.RoundToInt(y * hmPosMultiplier) - 1]
					);
				}*/
				//Debug.Log(x);
				//Debug.Log("--------------------------------------------");
				//Debug.Log(Mathf.RoundToInt((float)x * hmPosMultiplier));
				//Debug.Log((int)(x * hmPosMultiplier));
				//Debug.Log(Mathf.RoundToInt((float)y * hmPosMultiplier));
				//newVertices[x + y * verticesWidth] = (vertices[x + y * verticesWidth] + new Vector3(1f, 1f, Random.Range(-1.0f, 2.0f)));
				newVertices[x + y * verticesWidth] = vertices[x + y * verticesWidth] + (new Vector3(0, 0, -1) * (heightMap[Mathf.RoundToInt(x * hmPosMultiplier), Mathf.RoundToInt(y * hmPosMultiplier)] / 1f));
				//newVertices[x + y * verticesWidth] = vertices[x + y * verticesWidth];
			}
		}
		mf.mesh.vertices = newVertices;
	}

	void ClearPlaneHeight() {
		mf.mesh.vertices = (Vector3[])originalVertices.Clone();
	}




	private void SetMeshGrid(int gridSize, bool fixTransform = false) {
		Mesh mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";

		Vector3[] vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
		//Vector3[] normals = new Vector3[(gridSize + 1) * (gridSize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		for (int i = 0, y = 0; y <= gridSize; y++) {
			for (int x = 0; x <= gridSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
				//normals[i] = vertices[i].normalized;
				uv[i] = new Vector2((float)x / gridSize, (float)y / gridSize);
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[gridSize * gridSize * 6];
		for (int ti = 0, vi = 0, y = 0; y < gridSize; y++, vi++) {
			for (int x = 0; x < gridSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + gridSize + 1;
				triangles[ti + 5] = vi + gridSize + 2;
			}
		}
		mesh.triangles = triangles;
		//mesh.normals = normals;
		mf.mesh = mesh;

		if (fixTransform) {
			transform.Rotate(90, 0, 0);
			transform.position += new Vector3(10, 0, 10);
		}
		transform.localScale = new Vector3(originalScale * -(10f / gridSize), originalScale * -(10f / gridSize), 1);
	}

}