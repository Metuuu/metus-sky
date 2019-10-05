using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfCustom {
    

    // Laskee kulman. n = minkä vektorin ympäri (unityn oma kulma laskuri laskee vaan lähimmän kulman eli 180° sisältä eikä 360°)
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }


}



