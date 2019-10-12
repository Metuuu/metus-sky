using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNormals: MonoBehaviour {

	MeshFilter mf;
	Mesh mesh;
	Vector3[] vertices;
	Vector3[] normals;
	float nextActionTime = 0.0f;
	readonly float period = 5;

	Matrix4x4 localToWorld;


	void Start () {
		localToWorld = transform.localToWorldMatrix;
		mf = transform.GetComponent<MeshFilter>();
		mesh = mf.mesh;
		vertices = mesh.vertices;
		normals = mesh.normals;
	}

	void Update() {
		if (Time.time > nextActionTime) {
			nextActionTime += period;
			for (int iii = 0; iii < vertices.Length; ++iii) {
				Debug.DrawLine(
					transform.position + localToWorld.MultiplyVector(vertices[iii]),
					transform.position + localToWorld.MultiplyVector(vertices[iii]) * 1.025f,
					Color.green,
					period
				);
				Debug.DrawLine(
					transform.position + localToWorld.MultiplyVector(vertices[iii]) * 1.025f,
					transform.position + localToWorld.MultiplyVector(vertices[iii]) * 1.05f,
					Color.yellow,
					period
				);
			}
		}
	}

}
