using UnityEngine;
using System.Collections;

using LibNoise;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public static class Noise {

    // Muuttujat joihin ei haluta garbagecollectorin käyttävän aikaa
    static float maxPossibleHeight, offsetX, offsetY, minLocalNoiseHeight, maxLocalNoiseHeight, halfWidth, halfHeight;
    static int i, x, y;
    static float noiseHeight, sampleX, sampleY, perlinValue, normalizerdHeight; // tärkeämmät semmoset jotka vaikuttaa enemmän (eli luodaan looppien sisällä)


    // enum
    public enum NoiseSource { Unity, LibNoise, GPU_RenderTexture, GPU_2DFloatArray, GPU_RenderTextureTo2DFloatArray };
    public enum NormalizeMode { Local, Global };


	// Generate noise map

	//mapSizeSqrt, mapSizeSqrt, seed, side, offset, octaves, persistence, lacunarity, noiseSource

	public static float[,] GenerateNoiseMap(int seed, Vector3 offset, GPUNoiseGenerator.NoiseData noise, float scale, NoiseSource source, int performanceTestLoops = 1)
	{
		int mapSizeSqrt = (int)noise.resolution;
		float[,] noiseMap = new float[mapSizeSqrt, mapSizeSqrt];

		System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
		st.Start();
		for (int performanceLoopIndex = 0; performanceLoopIndex < performanceTestLoops; ++performanceLoopIndex) {

			switch (source) {
				case NoiseSource.Unity:
					#region [ - Unity Noise - ]

					Vector2 chunkOffset = offset;

					//GenerateFalloffMap(ref falloffMap, mapChunkSize);

					/* Generate Height Map
						(mapSide tarkottaa yhen spherical cuben reunan HeightMapin kokoa)
						   [5]
						   [4]
						[1][2][3]
						   [0]
					*/
				
					// Laskee sphericalCuben valitun lähtöpisteen
					for (int i = 0, len = int.Parse(((int)noise.side).ToString()); i <= len; ++i) {
						switch (i) {
							case 0:
								chunkOffset.x += mapSizeSqrt;
								break;
							case 1:
								chunkOffset.x -= mapSizeSqrt;
								chunkOffset.y += mapSizeSqrt;
								break;
							case 2:
								chunkOffset.x += mapSizeSqrt;
								break;
							case 3:
								chunkOffset.x += mapSizeSqrt;
								break;
							case 4:
								chunkOffset.x -= mapSizeSqrt;
								chunkOffset.y += mapSizeSqrt;
								break;
							case 5:
								chunkOffset.y += mapSizeSqrt;
								break;
						}
					}
					// Laskee quartereitten (..quartereitten (..quartereitten (..jne))) lähtöpisteen
					int quarterSize = mapSizeSqrt;
					
					for (int i = 0, len = noise.quarter.Length; i < len; ++i) {
						//quarterSize /= 2; // quarterin leveys\korkeus
						scale *= 2;

						switch (noise.quarter[i]) {
							case '0': // vasenala
									  // -> piste ei liiku minnekkään
								Debug.Log("scale: " + scale + ", quarter size: " + quarterSize + ", side:" + noise.quarter[i] + ", offset: " + chunkOffset.x + " | " + chunkOffset.y);
								break;
							case '1': // oikeeala
								chunkOffset.x += quarterSize / scale;
								Debug.Log("scale: " + scale + ", quarter size: " + quarterSize + ", side:" + noise.quarter[i] + ", offset: " + chunkOffset.x + " | " + chunkOffset.y);
								break;
							case '2': // vasenylä
								chunkOffset.y += quarterSize / scale;
								Debug.Log("scale: " + scale + ", quarter size: " + quarterSize + ", side:" + noise.quarter[i] + ", offset: " + chunkOffset.x + " | " + chunkOffset.y);
								break;
							case '3': // oikeeylä
								chunkOffset.x += quarterSize / scale;
								chunkOffset.y += quarterSize / scale;
								Debug.Log("scale: " + scale + ", quarter size: " + quarterSize + ", side:" + noise.quarter[i] + ", offset: " + chunkOffset.x + " | " + chunkOffset.y);
								break;
						}
					}


				
					Vector2[] octaveOffset = new Vector2[noise.octaves];

					maxPossibleHeight = 0;
					noise.amplitude = 1;
					noise.frequency = 1;

					NormalizeMode normalizeMode = NormalizeMode.Global;
					System.Random prng = new System.Random(seed);
					Vector2 randomOffsetFromSeed = new Vector2(prng.Next(-100000, 100000) - chunkOffset.x, prng.Next(-100000, 100000) - chunkOffset.y);

					for (i = 0; i < noise.octaves; ++i) {
						octaveOffset[i] = randomOffsetFromSeed;

						maxPossibleHeight += noise.amplitude;
						noise.amplitude *= noise.persistence;
					}

					if (scale <= 0) {
						scale = 0.0001f;
					}

					maxLocalNoiseHeight = float.MinValue;
					minLocalNoiseHeight = float.MaxValue;

					halfWidth = mapSizeSqrt / 2f;
					halfHeight = mapSizeSqrt / 2f;

					for (y = 0; y < mapSizeSqrt; ++y) {
						for (x = 0; x < mapSizeSqrt; ++x) {

							noise.amplitude = 1;
							noise.frequency = 1;
							noiseHeight = 0;

							for (i = 0; i < noise.octaves; ++i) { // Oktaavien teko kuluttaa sikana eli tarkkana
								sampleX = (x - mapSizeSqrt) / scale * noise.frequency + octaveOffset[i].x * noise.frequency;
								sampleY = (y - mapSizeSqrt) / scale * noise.frequency + octaveOffset[i].y * noise.frequency;

								perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // tää on raskas, * 2 - 1 tekee sen että se menee [-1, 1])
								noiseHeight += perlinValue * noise.amplitude;
								//Debug.Log("X: " + x + ", Y: " + y + " | " + perlinValue + " | " + amplitude);

								noise.amplitude *= noise.persistence;
								noise.frequency *= noise.lacunarity;
							}

							if (noiseHeight > maxLocalNoiseHeight) {
								maxLocalNoiseHeight = noiseHeight;
							}
							else if (noiseHeight < minLocalNoiseHeight) {
								minLocalNoiseHeight = noiseHeight;
							}

							noiseMap[x, y] = noiseHeight;

							//Debug.Log("X: " + x + ", Y: " + y + " | " + noiseMap[x, y]);
							//noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);

						}
					}
					for (int y = 0; y < mapSizeSqrt; ++y) {
						for (int x = 0; x < mapSizeSqrt; ++x) {
							if (normalizeMode == NormalizeMode.Local) {
								noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
							} else {
								float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
								noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
							}
						}
					}
					#endregion
					break;

				case NoiseSource.LibNoise:
					#region [ - LibNoise - ]

					Perlin perlinNoise = new Perlin {
						Quality = QualityMode.Medium,
						OctaveCount = noise.octaves,
						Lacunarity = noise.lacunarity,
						Seed = seed,
						Persistence = noise.persistence,
						Frequency = noise.frequency
					};

					LibNoise.Unity.ModuleBase moduleBase;
					moduleBase = perlinNoise;

					Noise2D noise2D = new LibNoise.Unity.Noise2D((int)noise.resolution, (int)noise.resolution, moduleBase);
					//noise.GeneratePlanar((double)offset.x, (double)(offset.x + mapWidth), (double)(offset.y + mapHeight), (double)offset.y);
					noise2D.GeneratePlanar((int)noise.resolution, 0, (int)noise.resolution, 0);

					noiseMap = noise2D.m_data;
					#endregion
					break;


				case NoiseSource.GPU_2DFloatArray:
					#region [ - GPU 2D Float Array - ]
					noiseMap = GPUNoiseGenerator.GenerateNoiseArray(seed, noise);
					#endregion
					break;

				case NoiseSource.GPU_RenderTextureTo2DFloatArray:
					#region [ - GPU Render Texture to 2D Float Array - ]
					noiseMap = GPUNoiseGenerator.GenerateNoiseArrayFromRenderTexture(seed, noise);
					#endregion
					break;

				default:
					return null;
			}

		}
		st.Stop();
		if (performanceTestLoops > 1)
			Debug.Log(string.Format("Generated noise with {0} {1} times and it took {2} ms to complete.", source.ToString(), performanceTestLoops, st.ElapsedMilliseconds));

		return noiseMap;
	}



}
