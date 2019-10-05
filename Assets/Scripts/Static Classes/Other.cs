using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


// TODO: keksi näille oikeet paikat eikä mitään Other classia
public static class Other {
    

    public static void MoveChilds(Transform source, Transform destination) {

        List<Transform> childList = new List<Transform>(); // Pitää tehä tää välivaihe koska muuten ei siirrä kaikkia childejä.. en tiiä miks !? (nyt ehkä tiiän, mutjoo.. pitäis for loopilla lopusta alkuun siirtää ne kai)
        foreach (Transform child in source) {
            childList.Add(child);
        }
        foreach (Transform child in childList) {
            child.parent = destination;
        }

    }   
     

    public static void CopyComponent<T>(T original, T destination) where T : Component {
        System.Type type = original.GetType();
        var fields = type.GetFields();
        foreach (var field in fields) {
            if (field.IsStatic) continue;
            field.SetValue(destination, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props) {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(destination, prop.GetValue(original, null), null);
        }
    }


}
