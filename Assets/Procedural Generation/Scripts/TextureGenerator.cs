using UnityEngine;
using System.Collections;

public static class TextureGenerator {


    // Generate texture from colormap
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {

		Texture2D texture = new Texture2D(width, height) {
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		texture.SetPixels(colorMap);
        texture.Apply();

        return texture;

    }



    // Generate texture from heightmap
    public static Texture2D TextureFromHeightMap(float[,] heightMap) {

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < width; ++y) {
			for (int x = 0; x < width; ++x) {

				// FRONT SIDE DEBUGGING
				/*if (x == 0 && y == 0) {
					Debug.Log("HEIGTMAP OIK Y: " + heightMap[0, 0]);
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
				} else if (x == 31 && y == 0) {
					Debug.Log("HEIGTMAP VAS Y: " + heightMap[31, 0]);
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
				} else if (x == 31 && y == 31) {
					Debug.Log("HEIGTMAP VAS A: " + heightMap[31, 31]);
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
				} else if (x == 0 && y == 31) {
					Debug.Log("HEIGTMAP OIK A: " + heightMap[0, 31]);
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
				} else {
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, 0.5f);
					heightMap[x, y] = 0.5f;
				}*/

				colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
        }
        
        return TextureFromColorMap(colorMap, width, height);
    }

}
