// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Tutorial/Display Normals" {
	Properties{
		_CameraTex("Camera texture", 2D) = "white" {}
	}
		SubShader{
		Pass{

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		uniform float4x4 lightMatrix;

	struct v2f {
		float4 pos : SV_POSITION;
		float3 worldPos : COLOR0;
		fixed3 color : COLOR1;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		//o.worldPos = v.vertex.xyz;
		o.color = v.normal * 0.5 + 0.5;
		return o;
	}

	uniform sampler2D _CameraTex;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 coords = mul(lightMatrix, fixed4(i.worldPos, 1.0));
	coords /= coords.w;

	coords.x = (coords.x + 1.0) / 2.0;
	coords.y = (coords.y + 1.0) / 2.0;
	coords.z = (coords.z + 1.0) / 2.0;

	//return fixed4(coords.z, 0.0, 0.0, 1);
	float lightDepth = tex2D(_CameraTex, fixed2(coords.x, 1.0 - coords.y)).r;
	float depth = coords.z;

	if (depth < lightDepth + 0.005) {
		return fixed4(1.0, 1.0, 1.0, 1.0);
	}
	else {
		return fixed4(0.1, 0.1, 0.1, 1.0);
	}
	}
		ENDCG

	}
	}
}