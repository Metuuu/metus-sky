// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "DrawBehindEverything" {
    SubShader {
        Pass {
            CGPROGRAM
            #pragma fragment frag
            #include "UnityCG.cginc"
         
			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float2 pos : SV_POSITION;
				fixed4 color : COLOR;
				fixed4 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float3 BoundsMin;
			float3 BoundsMax;

			float4 frag(v2f input) : SV_Target
			{
				float4 col = tex2D(_MainTex, input.uv);
				return col;
			}


			float4 vert(float4 v:POSITION) : SV_POSITION{
				return UnityObjectToClipPos(v);
			}

			ENDCG
        }
    } 
}