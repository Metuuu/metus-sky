using UnityEngine;
using System.Collections;

public class HeightController : MonoBehaviour {
	public float maxH = 500000.0f;
	void OnGUI()
	{
		GUILayout.Label("Height:");
		Vector3 pos = transform.position;
		pos.y = GUILayout.HorizontalSlider(transform.position.y , 0, maxH, GUILayout.MinWidth(Screen.width));
		transform.position = pos;
	}
}
