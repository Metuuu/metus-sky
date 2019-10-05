using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUNoiseGenerator {

    public enum NoiseType { PERLIN }
    public enum NoiseResolution { TRASH = 32, VERY_VERY_LOW = 64, VERY_LOW = 128, LOW = 256, MEDIUM = 512, HIGH = 1024, VERY_HIGH = 2048, ULTRA = 4096 }
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


    // Initialize
    static GPUNoiseGenerator() {
		NoiseComputeShaders.perlin = (ComputeShader)Resources.Load("Shaders/Noise/PerlinCS");
	}


	// Generate noise render texture
	public static RenderTexture GenerateNoise(int seed, NoiseData noiseData) {

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

		float freqMultiplier = 1;
		switch (noiseData.resolution) {
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
		shader.SetFloat("resolution", (int)noiseData.resolution);
		shader.SetFloat("zoom", zoom);
		shader.SetFloat("Octaves", noiseData.octaves);
		shader.SetFloat("Frequency", noiseData.frequency * freqMultiplier);
		shader.SetFloat("Amplitude", noiseData.amplitude);
		shader.SetFloat("Lacunarity", noiseData.lacunarity);
		shader.SetFloat("Persistence", noiseData.persistence);
		shader.SetVector("Offset", offset);
		shader.SetFloat("MaxNoiseHeight", noiseData.maxNoiseHeight);
		shader.SetTexture(kernelIndex, "tex", texture);
		shader.Dispatch(kernelIndex, (int)noiseData.resolution / 32, (int)noiseData.resolution / 32, 1);
		return texture;

	}



	// Generate noise render texture
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
	public static float[,] GenerateNoiseArray(int seed, NoiseData noiseType, int? meshGridSize = null) { // TODO: right top ja back ei toimi vielä - Kehittä DepthAndTesselation Koodia
		int resolution = (int)noiseType.resolution;

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


		// Join Table - [bottom, left, right, top] 1 = true, 0 = false
		int[,] borderJoin = { 
			{ 0, 0, 0, 0 },	// 0 - bottom
			{ 1, 0,	1, 0 },	// 1 - left
			{ 1, 0,	0, 0 },	// 2 - front
			{ 1, 1,	0, 0 },	// 3 - right
			{ 1, 1,	1, 1 },	// 4 - top
			{ 1, 1,	1, 0 }	// 5 - back
		};

		int joinBottom = 0, 
			joinBottomB = 0,
			joinLeft = 0,
			joinLeftB = 0,
			joinRight = 0,
			joinRightB = 0,
			joinTop = 0,
			joinTopB = 0;


		// Sides
		string kernelName = "PerlinNoise";
		switch (noiseType.side) {
			case Side.Bottom:
				kernelName = "Bottom";
				break;
			case Side.Left:
				kernelName = "Left";
				break;
			case Side.Front:
				kernelName = "Front";
				break;
			case Side.Right:
				kernelName = "Right";
				break;
			case Side.Top:
				kernelName = "Top";
				break;
			case Side.Back:
				kernelName = "Back";
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
					joinBottom = borderJoin[sideIndex, 0];
					bottommost = false;
				}
				if (leftmost && quarter != '0' && quarter != '2') {
					joinLeft = borderJoin[sideIndex, 1];
					leftmost = false;
				}
				if (rightmost && quarter != '1' && quarter != '3') {
					joinRight = borderJoin[sideIndex, 2];
					rightmost = false;
				}
				if (topmost && quarter != '2' && quarter != '3') {
					joinTop = borderJoin[sideIndex, 3];
					topmost = false;
				}
			}

			if (quarterLen > 1) {
				joinBottom = bottommost ? 0 : 1;
				joinRight = rightmost ? 0 : 1;
			} else {
				if (lastQuarter == '2' || lastQuarter == '3')
					joinBottom = 1;
				if (lastQuarter == '0' || lastQuarter == '2')
					joinRight = 1;
			}
		}
		if (bottommost)
			joinBottomB = borderJoin[sideIndex, 0];
		if (leftmost)
			joinLeftB = borderJoin[sideIndex, 1];
		if (rightmost)
			joinRightB = borderJoin[sideIndex, 2];
		if (topmost)
			joinTopB = borderJoin[sideIndex, 3];
	


		for (int i = 0; i < quarterLen; ++i) {
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

		shader.SetInt("joinBottom", joinBottom);
		shader.SetInt("joinBottomB", joinBottomB);
		shader.SetInt("joinLeft", joinLeft);
		shader.SetInt("joinLeftB", joinLeftB);
		shader.SetInt("joinRight", joinRight);
		shader.SetInt("joinRightB", joinRightB);
		shader.SetInt("joinTop", joinTop);
		shader.SetInt("joinTopB", joinTopB);
		
		if (meshGridSize == null) {
			meshGridSize = resolution;
		}
		shader.SetInt("meshGridSize", (int)meshGridSize);


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
		int mgs = (int)meshGridSize;
		int size = mgs * mgs;
		float[] heightMapArray = new float[size];
		ComputeBuffer buffer = new ComputeBuffer(size, sizeof(float), ComputeBufferType.Default);
		buffer.SetData(heightMapArray);

		// Dispatch Shader
		int kernelIndex = shader.FindKernel("PerlinNoise" + kernelName + quarterName + "V");
		shader.SetBuffer(kernelIndex, "buffer", buffer);
		//shader.Dispatch(kernelIndex, size / 1024, 1, 1);
		shader.Dispatch(kernelIndex, mgs / 32, mgs / 32, 1);

		buffer.GetData(heightMapArray);

		buffer.Dispose();
		buffer.Release();
		buffer = null;

		float[,] hmArray2d = new float[mgs, mgs];


		for (int i = 0; i < meshGridSize; ++i) {
			for (int j = 0; j < meshGridSize; ++j) {
				hmArray2d[i, j] = heightMapArray[i + j * mgs];
			}
		}

		return hmArray2d;
	}


}
