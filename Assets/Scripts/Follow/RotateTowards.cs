using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour {

    public GameObject center;
    public bool inverse;

	[SerializeField]
	bool lowPriority = false; // TODO: do global object that handles different update priorities and calls the update for each object when it is proper time

	Vector3 rot;
    Vector3 normalized;



    void Update() {
		if (lowPriority)
			Rotate();
	}

	void LateUpdate() {
		if (!lowPriority)
			Rotate();
	}


	void Rotate() {
		transform.LookAt(center.transform);

		if (inverse) {
			rot = transform.rotation.eulerAngles;
			normalized = transform.rotation.eulerAngles.normalized;
			rot = new Vector3(rot.x + 180 * normalized.x, rot.y + 180 * normalized.y, rot.z + 180 * normalized.z);
			transform.rotation = Quaternion.Euler(rot);
		}
	}

}
