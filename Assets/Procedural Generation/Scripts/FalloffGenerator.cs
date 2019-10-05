using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Threading;


public static class FalloffGenerator
{

    public static float[,] GenerateFalloffMap(ref float[,] map, int size, bool invert, float a, float b, float minPercentage, float maxPercentage) {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();


        map = new float[size, size];

        for (int i = 0; i < size; ++i) {
            for (int j = 0; j < size; ++j) {

                float x = i / (float)size * 2-1f;
                float y = j / (float)size * 2-1f;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                value = Evaluate(value, a, b);

                if (value > 1-minPercentage) {
                    value = 1-minPercentage;
                } else if (value < 1-maxPercentage) {
                    value = 1-maxPercentage;
                }
                if (invert) {
                    value = 1 - value;
                }
                map[i, j] = value;

            }
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log("Falloff: "+stopwatch.Elapsed);

        return map;
    }


    static float Evaluate(float value, float a, float b) {
        // a = 3
        // b = 2.2f

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));

    }



}
