using UnityEngine;
using System.Collections;


public static class GenerateSphericalTerrain
{

    public static Vector3[] GenerateTerrainMesh(Vector3[] parentVertices, float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve) {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);


        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;


        int verticesPerLine = Mathf.RoundToInt(Mathf.Sqrt(parentVertices.Length));


        ChunkMeshData meshData = new ChunkMeshData(width , height);


        int vertexIndex = 0;

        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                ++vertexIndex;
            }

        }

        return meshData.vertices;
    
             
    }


}


public class ChunkMeshData
{
    public Vector3[] vertices;

    // Mesh Data
    public ChunkMeshData(int meshWidth, int meshHeight) {
        vertices = new Vector3[meshWidth * meshHeight];
    }

}