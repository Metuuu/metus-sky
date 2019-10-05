using UnityEditor;

[CustomEditor(typeof(InteractiveObjectScript))]
public class InteractiveObjectsCustomInspector : Editor {
    

    public override void OnInspectorGUI() {

        SerializedProperty sProp;

        InteractiveObjectScript script = (InteractiveObjectScript)target;
        script.objectType = (InteractiveObjectScript.type)EditorGUILayout.EnumPopup("My type", script.objectType);

        switch (script.objectType) {
            case (InteractiveObjectScript.type.Info):
                sProp = serializedObject.FindProperty("info");
                EditorGUILayout.PropertyField(sProp);
                break;
            case (InteractiveObjectScript.type.Button):
                sProp = serializedObject.FindProperty("buttonEvent");
                EditorGUILayout.PropertyField(sProp);
                break; 
            case (InteractiveObjectScript.type.Switch):
                break;
            case (InteractiveObjectScript.type.Pickup):
                break;
            case (InteractiveObjectScript.type.Grabbable):
                break;
        }

        
        serializedObject.ApplyModifiedProperties();

    }
    

}
