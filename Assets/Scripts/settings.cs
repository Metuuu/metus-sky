using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;
		//QualitySettings.vSyncCount = 1;
	}
}
