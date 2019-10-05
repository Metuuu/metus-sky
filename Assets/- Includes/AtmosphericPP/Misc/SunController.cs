using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {
	
	private Vector3 erot = new Vector3(0.1f,0,0);

	void Start()
	{
		erot = transform.rotation.eulerAngles;
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5,40,Screen.width,80));
		//GUILayout.BeginHorizontal();
		GUILayout.Label("SUN rotation:");
		//Vector3 erot = transform.rotation.eulerAngles;
		erot.x = GUILayout.HorizontalSlider( erot.x , -90.0f, 90.0f,GUILayout.MinWidth(Screen.width));
		transform.rotation = Quaternion.Euler(erot);
		//GUILayout.EndHorizontal();
		if (GUILayout.Button("Exit",GUILayout.MaxWidth(200)))
			Application.Quit();
		GUILayout.EndArea();
	}
}
