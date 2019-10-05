using UnityEngine;
using System.Collections;

//This will draw the frustum of a camera in the scene during game play (not in the editor)
//Desighned to render the frustum of a camera as seen by the main camera not the main camera itself

[RequireComponent (typeof (Camera))]
public class DrawCameraFrustum : MonoBehaviour 
{
	
	public Color lineColor = new Color(1,1,1,1);
	
	Material lineMaterial;
	
	Vector4[] volume =
	{
		// near
		new Vector4(-1, -1, -1, 1), new Vector4( 1, -1, -1, 1), new Vector4( 1,  1, -1, 1),  new Vector4(-1,  1, -1, 1),
		// far
		new Vector4(-1, -1, 1, 1),	new Vector4( 1, -1, 1, 1),	new Vector4( 1,  1, 1, 1),  new Vector4(-1,  1, 1, 1)
	};
	
	int[,] indices = 
	{
		{0,1}, {1,2}, {2,3}, {3,0}, 
		{4,5}, {5,6}, {6,7}, {7,4},
		{0,4}, {1,5}, {2,6}, {3,7}
	};

	// Use this for initialization
	void Start () {
		
		CreateLineMaterial();
	
	}
	
	void OnPostRender() 
	{
		
		Matrix4x4 MVP = GetComponent<Camera>().projectionMatrix * GetComponent<Camera>().worldToCameraMatrix;
		Matrix4x4 invMVP = Matrix4x4.Inverse(MVP);
		
		Vector4[] frustumCorners = new Vector4[8];
		
		for(int i = 0; i < 8; i++)
		{
			frustumCorners[i] = invMVP * volume[i];	
			
			frustumCorners[i].x /= frustumCorners[i].w;
			frustumCorners[i].y /= frustumCorners[i].w;
			frustumCorners[i].z /= frustumCorners[i].w;
			frustumCorners[i].w = 1.0f;
		}
		
		GL.PushMatrix();
		
		GL.LoadIdentity();
		GL.MultMatrix(Camera.main.worldToCameraMatrix);
		GL.LoadProjectionMatrix(Camera.main.GetComponent<Camera>().projectionMatrix);
		
		lineMaterial.SetPass( 0 );
		GL.Begin( GL.LINES );
		GL.Color( lineColor );
		
		for(int i = 0; i < 12; i++)
		{
			Vector4 p0 = frustumCorners[indices[i,0]];
			Vector4 p1 = frustumCorners[indices[i,1]];
			
			GL.Vertex3( p0.x, p0.y, p0.z );
			GL.Vertex3( p1.x, p1.y, p1.z );
		}

		GL.End();
		
		GL.PopMatrix();
	}
	
	void CreateLineMaterial() 
	{
		if( !lineMaterial ) 
		{
			lineMaterial = new Material( 	"Shader \"Lines/Colored Blended\" {" +
											"SubShader { Pass { " +
											"    Blend SrcAlpha OneMinusSrcAlpha " +
											"    ZWrite Off Cull Off Fog { Mode Off } " +
											"    BindChannels {" +
											"      Bind \"vertex\", vertex Bind \"color\", color }" +
											"} } }" );
			
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}
}


















