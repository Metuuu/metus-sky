using UnityEngine;
using System.Collections;

public static class TerrainGenerator
{

    // Muuttujat, joiden ei haluta katoavan ettei garabge collectorin tarttee huolehtia niistä <- tän asian kanssa k
    static int i, xx, yy, heightMapLength, len, sideInt, intX, intY, verticesLength;
    static float min, max, x, y, getScaleOff, height;
    static float[] terrainHeight;


    // Generate terrain height -- vanha väärään systeemiin tehty ei toimiva kakka..
    public static float[] GenerateTerrainHeight(int gridSize, string side, float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, bool normalizeScale = false) {

        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        heightMapLength = heightMap.GetLength(0);

        verticesLength = gridSize + 1;
        terrainHeight = new float[verticesLength * verticesLength];

        getScaleOff = (!normalizeScale) ? getScaleOff = 1f : getScaleOff = 0.1f;// / heightMapLength;

        



        sideInt = (int)char.GetNumericValue(side[0]);
        len = side.Length;

        // - Quarters -

        // laskee luvun..

        min = 0;
        max = 0;

        switch (side[0]) {
            case '0':
                min = 0;
                if (i == len - 1) { // Vika (kerta)
                    max = 0.25f;
                }
                break;
            case '1':
                min = 0.25f;
                if (i == len - 1) { // Vika
                    max = 0.5f;
                }
                break;
            case '2':
                min = 0.5f;
                if (i == len - 1) { // Vika
                    max = 0.75f;
                }
                break;
            case '3':
                min = 0.75f;
                if (i == len - 1) { // Vika
                    max = 1f;
                }
                break;
        }

        for (i = 1; i < len; ++i) {
            
            switch (side[i]) {
                case '0':
                    min = Mathf.Pow(0.25f, i) * 0;
                    if (i == len - 1) { // Vika
                        max = min + Mathf.Pow(0.25f, i+1) * 1;
                    }
                    break;
                case '1':
                    min = Mathf.Pow(0.25f, i) * 1;
                    if (i == len - 1) { // Vika
                        max = min + Mathf.Pow(0.25f, i+1) * 2;
                    }
                    break;
                case '2':
                    min = Mathf.Pow(0.25f, i) * 2;
                    if (i == len - 1) { // Vika
                        max = min + Mathf.Pow(0.25f, i+1) * 3;
                    }
                    break;
                case '3':
                    min = Mathf.Pow(0.25f, i) * 3;
                    if (i == len - 1) { // Vika
                        max = min + Mathf.Pow(0.25f, i+1) * 4;
                    }
                    break;
            }
        }

        float increment = (max - min) / verticesLength;

        for (y = min, yy = 0; yy < verticesLength; y += increment, intY = Mathf.RoundToInt(y*(heightMapLength-1)), ++yy) {
            for (x = min, xx = 0; xx < verticesLength; x += increment, intX = Mathf.RoundToInt(x*(heightMapLength-1)), ++xx) {
                try {
                    height = heightCurve.Evaluate(heightMap[intX, intY]) * heightMultiplier;
                }
                catch {
                    Debug.Log("asdf");
                }
                terrainHeight[yy * verticesLength + xx] = height * getScaleOff;
            }
        }

        return terrainHeight;

    }


}