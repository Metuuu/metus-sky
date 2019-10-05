using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNormals: MonoBehaviour {

	MeshFilter mf;
	Mesh mesh;
	Vector3[] vertices;
	Vector3[] normals;


	void Start () {
		mf = transform.GetComponent<MeshFilter>();
		mesh = mf.mesh;
		vertices = mesh.vertices;
		normals = mesh.normals;
	}

	
	void FixedUpdate () {
		for (int iii = 0; iii < vertices.Length; ++iii) {
			Debug.DrawLine(transform.position + transform.localRotation * vertices[iii] + transform.localRotation * normals[iii] * 9.5f, transform.position + transform.localRotation * vertices[iii] + transform.localRotation * (normals[iii] * 10.5f), Color.green);
			Debug.DrawLine(transform.position + transform.localRotation * vertices[iii] + transform.localRotation * normals[iii] * 10.5f, transform.position + transform.localRotation * vertices[iii] + transform.localRotation * (normals[iii] * 11), Color.yellow);
		}
	}

}
