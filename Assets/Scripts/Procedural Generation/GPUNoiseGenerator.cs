using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUNoiseGenerator {

	public enum NoiseType { PERLIN }
	public enum NoiseResolution { TRASH = 32, VERY_VERY_LOW = 64, VERY_LOW = 128, LOW = 256, MEDIUM = 512, HIGH = 1024, VERY_HIGH = 2048, ULTRA = 4096 }
	//public enum NoiseResolution { TRASH = 24, VERY_VERY_LOW = 48, VERY_LOW = 96, LOW = 120, MEDIUM = 240, HIGH = 1080, VERY_HIGH = 2160, ULTRA = 4320 }
	public enum Side { Bottom, Left, Front, Right, Top, Back }


	[System.Serializable]
    private static class NoiseComputeShaders {
		public static ComputeShader perlin;// = (ComputeShader)Resources.Load("Shaders/Noise/PerlinCS"); //tuolla alhaalla on init mutta voikohan tehdä suoraan näin
    }


    [System.Serializable]
    public class NoiseData {
		public NoiseType noiseType;
		public NoiseResolution resolution;
		public Side side;
		public LodLowerOn lodLowerOn;
		public string quarter;
		public int octaves;
		public float frequency;
		public float amplitude;
		public float lacunarity;
		public float persistence;
		public float maxNoiseHeight = 1f;
		/*NoiseData(NoiseType noiseType) {
			this.noiseType = noiseType
		}*/
	}

	[System.Serializable]
    public class LodLowerOn {
		public int left = 0;
		public int right = 0;
		public int top = 0;
		public int bottom = 0;
	}


    // Initialize
    static GPUNoiseGenerator() {
		NoiseComputeShaders.perlin = (ComputeShader)Resources.Load("Shaders/Noise/PerlinCS");
	}


	// Generate noise render texture
	public static RenderTexture GenerateNoise(int seed, NoiseData noiseData, float planetRadius) {

		// Create new texture
		RenderTexture texture = RenderTexture.GetTemporary((int)noiseData.resolution, (int)noiseData.resolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear); // TODO: KORJAA MEMORY LEAK ? tää ollut myös RHalf mutta nyt compute shader testeihin varmuuden vuoks RFloat
		texture.enableRandomWrite = true;
		texture.Create();

		// Noise type
		ComputeShader shader;
		switch (noiseData.noiseType) {
			default: //NoiseShader.PERLIN:
				shader = NoiseComputeShaders.perlin;
				break;
		}

		// Init vars
		float zoom = 1;
		Vector3 offset = Vector3.zero;


		// Sides
		int kernelIndex = 0;
		switch (noiseData.side) {
			case Side.Bottom:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereBottomTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeBottomTex");
				break;
			case Side.Left:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereLeftTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeLeftTex");
				break;
			case Side.Front:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereFrontTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeFrontTex");
				break;
			case Side.Right:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereRightTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeRightTex");
				break;
			case Side.Top:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereTopTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeTopTex");
				break;
			case Side.Back:
				kernelIndex = shader.FindKernel("PerlinNoiseSphereBackTex");
				//kernelIndex = shader.FindKernel("PerlinNoiseCubeBackTex");
				break;
		}

		// Quarter
		int len = noiseData.quarter.Length;

		for (int i = 0; i < len; ++i) {
			zoom *= 2;
			switch (noiseData.side) {
				case Side.Bottom:
					offset.x *= 2;
					offset.z *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							offset.z += 1;
							break;
						case '1': // oikeeala
							offset.z += 1;
							break;
						case '2': // vasenylä
							offset.x += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Left:
					offset.z *= 2;
					offset.y *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.z += 1;
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							break;
						case '2': // vasenylä
							offset.z += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Front:
					offset.x *= 2;
					offset.y *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							break;
						case '2': // vasenylä
							offset.x += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Right:
					offset.z *= 2;
					offset.y *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							offset.z += 1;
							break;
						case '2': // vasenylä
							break;
						case '3': // oikeeylä
							offset.z += 1;
							break;
					}
					break;
				case Side.Top:
					offset.x *= 2;
					offset.z *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							break;
						case '1': // oikeeala
							break;
						case '2': // vasenylä
							offset.x += 1;
							offset.z += 1;
							break;
						case '3': // oikeeylä
							offset.z += 1;
							break;
					}
					break;
				case Side.Back:
					offset.x *= 2;
					offset.y *= 2;
					switch (noiseData.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							break;
						case '1': // oikeeala
							break;
						case '2': // vasenylä
							offset.x += 1;
							offset.y += 1;
							break;
						case '3': // oikeeylä
							offset.y += 1;
							break;
					}
					break;
			}
		}


		// Noise Resolution
		offset *= (int)noiseData.resolution;


		// Calculate noise map
		shader.SetFloat("resolution", (int)noiseData.resolution);
		shader.SetFloat("zoom", zoom);
		shader.SetFloat("Octaves", noiseData.octaves);
		shader.SetFloat("Frequency", noiseData.frequency);
		shader.SetFloat("Amplitude", noiseData.amplitude);
		shader.SetFloat("Lacunarity", noiseData.lacunarity);
		shader.SetFloat("Persistence", noiseData.persistence);
		shader.SetVector("ChunkOffset", offset);
		shader.SetFloat("MaxNoiseHeight", noiseData.maxNoiseHeight);
		shader.SetFloat("radius", planetRadius);
		shader.SetInt("lodLowerOnLeft", noiseData.lodLowerOn.left);
		shader.SetInt("lodLowerOnTop", noiseData.lodLowerOn.top);
		shader.SetInt("lodLowerOnRight", noiseData.lodLowerOn.right);
		shader.SetInt("lodLowerOnBottom", noiseData.lodLowerOn.bottom);
		shader.SetTexture(kernelIndex, "tex", texture);
		shader.Dispatch(kernelIndex, (int)noiseData.resolution / 32, (int)noiseData.resolution / 32, 1);
		return texture;

	}



	/**
		Generate noise array from render texture
		This was way slower than generating the array straight.
		Old version now. This can be deleted when wanted.
	 */
	public static float[,] GenerateNoiseArrayFromRenderTexture(int seed, NoiseData noiseType) {
		int resolution = (int)noiseType.resolution;

		// Create new texture
		RenderTexture texture = RenderTexture.GetTemporary(resolution, resolution, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear); // TODO: KORJAA MEMORY LEAK ? tää ollut myös RHalf mutta nyt compute shader testeihin varmuuden vuoks RFloat
		texture.enableRandomWrite = true;
		texture.Create();

		// Noise type
		ComputeShader shader;
		switch (noiseType.noiseType) {
			default: //NoiseShader.PERLIN:
				shader = NoiseComputeShaders.perlin;
				break;
		}

		// Init vars
		float zoom = 1;
		Vector3 offset = Vector3.zero;


		// Sides
		int kernelIndex = 0;
		switch (noiseType.side) {
			case Side.Bottom:
				kernelIndex = shader.FindKernel("PerlinNoiseBottom");
				break;
			case Side.Left:
				kernelIndex = shader.FindKernel("PerlinNoiseLeft");
				break;
			case Side.Front:
				kernelIndex = shader.FindKernel("PerlinNoiseFront");
				break;
			case Side.Right:
				kernelIndex = shader.FindKernel("PerlinNoiseRight");
				break;
			case Side.Top:
				kernelIndex = shader.FindKernel("PerlinNoiseTop");
				break;
			case Side.Back:
				kernelIndex = shader.FindKernel("PerlinNoiseBack");
				break;
		}

		// Quarter
		int len = noiseType.quarter.Length;

		for (int i = 0; i < len; ++i) {
			zoom *= 2;
			switch (noiseType.side) {
				case Side.Bottom:
					offset.x *= 2;
					offset.z *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							offset.z += 1;
							break;
						case '1': // oikeeala
							offset.z += 1;
							break;
						case '2': // vasenylä
							offset.x += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Left:
					offset.z *= 2;
					offset.y *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.z += 1;
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							break;
						case '2': // vasenylä
							offset.z += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Front:
					offset.x *= 2;
					offset.y *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							break;
						case '2': // vasenylä
							offset.x += 1;
							break;
						case '3': // oikeeylä
							break;
					}
					break;
				case Side.Right:
					offset.z *= 2;
					offset.y *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.y += 1;
							break;
						case '1': // oikeeala
							offset.y += 1;
							offset.z += 1;
							break;
						case '2': // vasenylä
							break;
						case '3': // oikeeylä
							offset.z += 1;
							break;
					}
					break;
				case Side.Top:
					offset.x *= 2;
					offset.z *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							break;
						case '1': // oikeeala
							break;
						case '2': // vasenylä
							offset.x += 1;
							offset.z += 1;
							break;
						case '3': // oikeeylä
							offset.z += 1;
							break;
					}
					break;
				case Side.Back:
					offset.x *= 2;
					offset.y *= 2;
					switch (noiseType.quarter[i]) {
						case '0': // vasenala
							offset.x += 1;
							break;
						case '1': // oikeeala
							break;
						case '2': // vasenylä
							offset.x += 1;
							offset.y += 1;
							break;
						case '3': // oikeeylä
							offset.y += 1;
							break;
					}
					break;
			}
		}


		// Noise Resolution
		offset *= resolution;

		float freqMultiplier = 1;
		switch (noiseType.resolution) {
			case NoiseResolution.TRASH:
				freqMultiplier = 16;
				break;
			case NoiseResolution.VERY_VERY_LOW:
				freqMultiplier = 8;
				break;
			case NoiseResolution.VERY_LOW:
				freqMultiplier = 4;
				break;
			case NoiseResolution.LOW:
				freqMultiplier = 2;
				break;
			case NoiseResolution.HIGH:
				freqMultiplier = 0.5f;
				break;
			case NoiseResolution.VERY_HIGH:
				freqMultiplier = 0.25f;
				break;
			case NoiseResolution.ULTRA:
				freqMultiplier = 0.125f;
				break;
		}


		// Calculate noise map
		shader.SetFloat("resolution", resolution);
		shader.SetFloat("zoom", zoom);
		shader.SetFloat("Octaves", noiseType.octaves);
		shader.SetFloat("Frequency", noiseType.frequency * freqMultiplier);
		shader.SetFloat("Amplitude", noiseType.amplitude);
		shader.SetFloat("Lacunarity", noiseType.lacunarity);
		shader.SetFloat("Persistence", noiseType.persistence);
		shader.SetVector("Offset", offset);
		shader.SetFloat("MaxNoiseHeight", noiseType.maxNoiseHeight);
		shader.SetTexture(kernelIndex, "tex", texture);

		shader.Dispatch(kernelIndex, resolution / 32, resolution / 32, 1);

		RenderTexture.active = texture;
		Texture2D texture2d = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);
		texture2d.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
		texture2d.Apply();
		RenderTexture.active = null;
		texture.Release();

		float[,] heightMap = new float[resolution, resolution];

		Color[] pixels = texture2d.GetPixels();
		for (int x = 0; x < resolution; x++) {
			for (int y = 0; y < resolution; y++) {
				heightMap[x, y] = pixels[x + y * resolution].r;
			}
		}
		return heightMap;
	}



	// Generate noise noise array
	public static float[,] GenerateNoiseArray(int seed, NoiseData noiseType, float planetRadius, int? meshGridSize = null, Vector3? offset = null, float zoom = 1) { // TODO: right top ja back ei toimi vielä - Kehittä DepthAndTesselation Koodia
		int resolution = (int)noiseType.resolution;
		int mgs = resolution;
		if (meshGridSize != null) {
			mgs = (int)meshGridSize;
			resolution = mgs;
			if (mgs % 32 != 0) {
				resolution = (mgs - (mgs % 32) + 32);
			}
		}

		// Noise type
		ComputeShader shader;
		switch (noiseType.noiseType) {
			default: //NoiseShader.PERLIN:
				shader = NoiseComputeShaders.perlin;
				break;
		}

		// Init vars
		float chunkZoom = 1;
		Vector3 chunkOffset = Vector3.zero;


		// Join Table - [(0)bottom, (1)left, (2)right, (3)top] 1 = true, 0 = false
		int[,] borderJoin = {
			// 0, 1, 2, 3 - quarters
			{ 0, 0, 0, 0 },	// 0 - bottom
			{ 1, 0, 1, 0 },	// 1 - left
			{ 1, 0, 0, 0 },	// 2 - front
			{ 1, 1, 0, 0 },	// 3 - right
			{ 1, 1, 1, 1 },	// 4 - top
			{ 1, 1, 1, 0 }		// 5 - back
		};

		int joinBottomQuarter = 0,
			joinRightQuarter = 0;


		// Sides
		string kernelName = "PerlinNoiseSphere";
		//string kernelName = "PerlinNoiseCube";
		switch (noiseType.side) {
			case Side.Bottom:
				kernelName += "Bottom";
				break;
			case Side.Left:
				kernelName += "Left";
				break;
			case Side.Front:
				kernelName += "Front";
				break;
			case Side.Right:
				kernelName += "Right";
				break;
			case Side.Top:
				kernelName += "Top";
				break;
			case Side.Back:
				kernelName += "Back";
				break;
		}


		// Quarter
		string quarterName = "";
		int quarterLen = noiseType.quarter.Length;
		int sideIndex = (int)noiseType.side;

		bool bottommost = true;
		bool leftmost = true;
		bool rightmost = true;
		bool topmost = true;

		if (quarterLen > 0) {
			char lastQuarter = noiseType.quarter[quarterLen - 1];

			for (int i = 0; i < quarterLen; ++i) {
				char quarter = noiseType.quarter[i];
				if (bottommost && quarter != '0' && quarter != '1') {
					// joinBottom = borderJoin[sideIndex, 0];
					bottommost = false;
				}
				if (leftmost && quarter != '0' && quarter != '2') {
					// joinLeft = borderJoin[sideIndex, 1];
					leftmost = false;
				}
				if (rightmost && quarter != '1' && quarter != '3') {
					// joinRight = borderJoin[sideIndex, 2];
					rightmost = false;
				}
				if (topmost && quarter != '2' && quarter != '3') {
					// joinTop = borderJoin[sideIndex, 3];
					topmost = false;
				}
			}

			if (quarterLen > 1) {
				joinBottomQuarter = bottommost ? 0 : 1;
				joinRightQuarter = rightmost ? 0 : 1;
			} else {
				if (lastQuarter == '2' || lastQuarter == '3')
					joinBottomQuarter = 1;
				if (lastQuarter == '0' || lastQuarter == '2')
					joinRightQuarter = 1;
			}
		}

		int joinBottomB = bottommost ? 1 : 0;
		int joinLeftB = leftmost ? 1 : 0;
		int joinRightB = rightmost ? 1 : 0;
		int joinTopB = topmost ? 1 : 0;

		// if (bottommost)
		// 	joinBottomB = quarterBorderJoin[sideIndex, 0];
		// if (leftmost)
		// 	joinLeftB = quarterBorderJoin[sideIndex, 1];
		// if (rightmost)
		// 	joinRightB = quarterBorderJoin[sideIndex, 2];
		// if (topmost)
		// 	joinTopB = quarterBorderJoin[sideIndex, 3];



		for (int i = 0; i < quarterLen; ++i) {
            chunkZoom *= 2;
            switch (noiseType.side) {
                case Side.Bottom:
                    chunkOffset.x *= 2;
                    chunkOffset.z *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.x += 1;
                            chunkOffset.z += 1;
                            break;
                        case '1': // oikeeala
                            chunkOffset.z += 1;
                            break;
                        case '2': // vasenylä
                            chunkOffset.x += 1;
                            break;
                        case '3': // oikeeylä
                            break;
                    }
                    break;
                case Side.Left:
                    chunkOffset.z *= 2;
                    chunkOffset.y *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.z += 1;
                            chunkOffset.y += 1;
                            break;
                        case '1': // oikeeala
                            chunkOffset.y += 1;
                            break;
                        case '2': // vasenylä
                            chunkOffset.z += 1;
                            break;
                        case '3': // oikeeylä
                            break;
                    }
                    break;
                case Side.Front:
                    chunkOffset.x *= 2;
                    chunkOffset.y *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.x += 1;
                            chunkOffset.y += 1;
                            break;
                        case '1': // oikeeala
                            chunkOffset.y += 1;
                            break;
                        case '2': // vasenylä
                            chunkOffset.x += 1;
                            break;
                        case '3': // oikeeylä
                            break;
                    }
                    break;
                case Side.Right:
                    chunkOffset.z *= 2;
                    chunkOffset.y *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.y += 1;
                            break;
                        case '1': // oikeeala
                            chunkOffset.y += 1;
                            chunkOffset.z += 1;
                            break;
                        case '2': // vasenylä
                            break;
                        case '3': // oikeeylä
                            chunkOffset.z += 1;
                            break;
                    }
                    break;
                case Side.Top:
                    chunkOffset.x *= 2;
                    chunkOffset.z *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.x += 1;
                            break;
                        case '1': // oikeeala
                            break;
                        case '2': // vasenylä
                            chunkOffset.x += 1;
                            chunkOffset.z += 1;
                            break;
                        case '3': // oikeeylä
                            chunkOffset.z += 1;
                            break;
                    }
                    break;
                case Side.Back:
                    chunkOffset.x *= 2;
                    chunkOffset.y *= 2;
                    switch (noiseType.quarter[i]) {
                        case '0': // vasenala
                            chunkOffset.x += 1;
                            break;
                        case '1': // oikeeala
                            break;
                        case '2': // vasenylä
                            chunkOffset.x += 1;
                            chunkOffset.y += 1;
                            break;
                        case '3': // oikeeylä
                            chunkOffset.y += 1;
                            break;
                    }
                    break;
            }
        }


		// Noise Resolution
		chunkOffset *= resolution;


		// Calculate noise map
		shader.SetFloat("resolution", resolution);
		shader.SetFloat("zoom", chunkZoom * zoom);
		shader.SetFloat("Octaves", noiseType.octaves);
		shader.SetFloat("Frequency", noiseType.frequency);
		shader.SetFloat("Amplitude", noiseType.amplitude);
		shader.SetFloat("Lacunarity", noiseType.lacunarity);
		shader.SetFloat("Persistence", noiseType.persistence);
		shader.SetVector("ChunkOffset", chunkOffset);
		shader.SetVector("Offset", (Vector4)(offset != null ? offset : Vector3.zero));
		shader.SetFloat("MaxNoiseHeight", noiseType.maxNoiseHeight);

		// shader.SetInt("joinBottom", joinBottom);
		shader.SetInt("joinBottomB", joinBottomB);
		// shader.SetInt("joinLeft", joinLeft);
		shader.SetInt("joinLeftB", joinLeftB);
		// shader.SetInt("joinRight", joinRight);
		shader.SetInt("joinRightB", joinRightB);
		// shader.SetInt("joinTop", joinTop);
		shader.SetInt("joinTopB", joinTopB);

		shader.SetInt("joinRightQuarter", joinRightQuarter);
		shader.SetInt("joinBottomQuarter", joinBottomQuarter);

		shader.SetInt("lodLowerOnLeft", noiseType.lodLowerOn.left);
		shader.SetInt("lodLowerOnTop", noiseType.lodLowerOn.top);
		shader.SetInt("lodLowerOnRight", noiseType.lodLowerOn.right);
		shader.SetInt("lodLowerOnBottom", noiseType.lodLowerOn.bottom);

		shader.SetFloat("radius", planetRadius);


		// FULL RESOLUTION HEIGHT MAP
		/*int size = resolution * resolution;
		float[] heightMapArray = new float[size];
		ComputeBuffer buffer = new ComputeBuffer(size, sizeof(float), ComputeBufferType.Default);
		buffer.SetData(heightMapArray);

		// Dispatch Shader
		int kernelIndex = shader.FindKernel("PerlinNoise" + kernelName + quarterName + "V");
		shader.SetBuffer(kernelIndex, "buffer", buffer);
		//shader.Dispatch(kernelIndex, size / 1024, 1, 1);
		shader.Dispatch(kernelIndex, resolution / 32, resolution / 32, 1);

		buffer.GetData(heightMapArray);

		buffer.Dispose();
		buffer.Release();
		buffer = null;

		float[,] hmArray2d = new float[resolution, resolution];


		for (int i = 0; i < resolution; ++i) {
			for (int j = 0; j < resolution; ++j) {
				hmArray2d[i, j] = heightMapArray[i + j * resolution];
			}
		}*/


		// HEIGHT MAP FOR MESH ONLY

		int size = resolution * resolution;
		float[] heightMapArray = new float[size];
		ComputeBuffer buffer = new ComputeBuffer(size, sizeof(float), ComputeBufferType.Default);
		buffer.SetData(heightMapArray);

		// Dispatch Shader
		int kernelIndex = shader.FindKernel(kernelName + quarterName);
		shader.SetBuffer(kernelIndex, "buffer", buffer);
		//shader.Dispatch(kernelIndex, size / 1024, 1, 1);

		shader.Dispatch(kernelIndex, resolution / 32, resolution / 32, 1);
		//shader.Dispatch(kernelIndex, mgs / 32, mgs / 32, 1);

		buffer.GetData(heightMapArray);

		buffer.Dispose();
		buffer.Release();
		buffer = null;

		float[,] hmArray2d = new float[mgs, mgs];

		float hmPosMultiplier = ((float)resolution / mgs);

		int x;
		int y;
		for (int i = 0; i < mgs; ++i) {
			x = Mathf.RoundToInt(i * hmPosMultiplier);
			for (int j = 0; j < mgs; ++j) {
				y = Mathf.RoundToInt(j * hmPosMultiplier);
				hmArray2d[i, j] = heightMapArray[x + y * resolution];
			}
		}

		return hmArray2d;
	}


}
