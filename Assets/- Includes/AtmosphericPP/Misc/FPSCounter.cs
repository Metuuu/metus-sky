using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	
	float fpsMeasurePeriod = 0.5f;
	int fpsAccumulator = 0;
	float fpsNextPeriod = 0;
	int currentFps;
	string display = "{0} FPS";

	void Start()
	{
		fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
	}

	void Update()
	{

		// measure average frames per second
		fpsAccumulator++;
		if (Time.realtimeSinceStartup > fpsNextPeriod)
		{
			currentFps = (int)(fpsAccumulator / fpsMeasurePeriod);
			fpsAccumulator = 0;
			fpsNextPeriod += fpsMeasurePeriod;
			//guiText.text = string.Format(display, currentFps);
		}


	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width-100,2,100,20));
	    GUILayout.Label("FPS:"+currentFps);
	    GUILayout.EndArea();
	}

}
