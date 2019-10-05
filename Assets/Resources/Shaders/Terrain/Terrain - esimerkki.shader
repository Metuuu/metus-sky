// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:1,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33036,y:33151,varname:node_2865,prsc:2|emission-1933-OUT,custl-6543-OUT,disp-5572-OUT,tess-6728-OUT;n:type:ShaderForge.SFN_Tex2d,id:7736,x:31572,y:32722,ptovrint:True,ptlb:LowTexture,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5583,x:32241,y:33443,varname:node_5583,prsc:2|A-1618-OUT,B-6711-OUT;n:type:ShaderForge.SFN_NormalVector,id:1618,x:31990,y:33320,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:6728,x:32680,y:33771,ptovrint:False,ptlb:Tesselation,ptin:_Tesselation,varname:node_6728,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:5,max:60;n:type:ShaderForge.SFN_Tex2d,id:9153,x:31280,y:33302,ptovrint:False,ptlb:DisplacementTexture,ptin:_DisplacementTexture,varname:node_9153,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6711,x:31990,y:33472,varname:node_6711,prsc:2|A-7736-R,B-2090-OUT;n:type:ShaderForge.SFN_Slider,id:2090,x:31268,y:33560,ptovrint:False,ptlb:DepthLow,ptin:_DepthLow,varname:node_2090,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.025,max:1;n:type:ShaderForge.SFN_Tex2d,id:8212,x:31572,y:32913,ptovrint:False,ptlb:MedTex,ptin:_MedTex,varname:node_8212,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:5642,x:31268,y:33663,ptovrint:False,ptlb:DepthMed,ptin:_DepthMed,varname:node_5642,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.005,max:1;n:type:ShaderForge.SFN_Multiply,id:6271,x:31990,y:33595,varname:node_6271,prsc:2|A-8212-R,B-5642-OUT;n:type:ShaderForge.SFN_Tex2d,id:421,x:31572,y:33097,ptovrint:False,ptlb:HighTex,ptin:_HighTex,varname:node_421,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:466,x:31268,y:33773,ptovrint:False,ptlb:DepthHigh,ptin:_DepthHigh,varname:node_466,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:3468,x:31990,y:33720,varname:node_3468,prsc:2|A-421-R,B-466-OUT;n:type:ShaderForge.SFN_LightVector,id:5377,x:31990,y:33176,varname:node_5377,prsc:2;n:type:ShaderForge.SFN_Dot,id:990,x:32202,y:33176,varname:node_990,prsc:2,dt:0|A-5377-OUT,B-1618-OUT;n:type:ShaderForge.SFN_Multiply,id:6543,x:32615,y:32946,varname:node_6543,prsc:2|A-3849-OUT,B-9900-RGB,C-990-OUT;n:type:ShaderForge.SFN_LightColor,id:9900,x:32079,y:32911,varname:node_9900,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:9931,x:32079,y:33026,varname:node_9931,prsc:2;n:type:ShaderForge.SFN_Multiply,id:125,x:32241,y:33569,varname:node_125,prsc:2|A-1618-OUT,B-6271-OUT;n:type:ShaderForge.SFN_Multiply,id:509,x:32241,y:33700,varname:node_509,prsc:2|A-1618-OUT,B-3468-OUT;n:type:ShaderForge.SFN_Tex2d,id:9347,x:32176,y:31959,ptovrint:False,ptlb:DefaultGroundTexture,ptin:_DefaultGroundTexture,varname:node_9347,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b1bf4a42f4b70f44e9f099c8ea02e86b,ntxv:3,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_TexCoord,id:9221,x:31702,y:31910,varname:node_9221,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:7517,x:31903,y:31987,varname:node_7517,prsc:2|A-9221-UVOUT,B-7157-OUT;n:type:ShaderForge.SFN_Add,id:1788,x:32518,y:33524,varname:node_1788,prsc:2|A-5583-OUT,B-125-OUT,C-509-OUT;n:type:ShaderForge.SFN_Slider,id:7157,x:31545,y:32080,ptovrint:False,ptlb:TextureShrinkMultiplier,ptin:_TextureShrinkMultiplier,varname:node_7157,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:100;n:type:ShaderForge.SFN_Lerp,id:3849,x:32566,y:32180,varname:node_3849,prsc:2|A-9347-RGB,B-2332-OUT,T-1164-OUT;n:type:ShaderForge.SFN_Tex2d,id:8310,x:32124,y:32139,ptovrint:False,ptlb:TestTex,ptin:_TestTex,varname:_node_9347_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4785e388e18957e4aa226e9fe8f7f15f,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_Tex2d,id:433,x:33107,y:32863,ptovrint:False,ptlb:DebugTexture,ptin:_DebugTexture,varname:node_433,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Divide,id:818,x:31607,y:34167,varname:node_818,prsc:2|A-466-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Vector1,id:7447,x:31293,y:34038,varname:node_7447,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:4154,x:31776,y:34167,varname:node_4154,prsc:2|A-818-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Set,id:7772,x:32146,y:34008,varname:SyvyysKeskittaja,prsc:2|IN-5170-OUT;n:type:ShaderForge.SFN_Get,id:577,x:32631,y:33439,varname:node_577,prsc:2|IN-7772-OUT;n:type:ShaderForge.SFN_NormalVector,id:4434,x:31607,y:34319,prsc:2,pt:False;n:type:ShaderForge.SFN_Add,id:5170,x:31973,y:34008,varname:node_5170,prsc:2|A-6908-OUT,B-9962-OUT,C-4154-OUT;n:type:ShaderForge.SFN_Divide,id:7797,x:31607,y:34028,varname:node_7797,prsc:2|A-5642-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Multiply,id:9962,x:31776,y:34028,varname:node_9962,prsc:2|A-7797-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Divide,id:5087,x:31607,y:33899,varname:node_5087,prsc:2|A-2090-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Multiply,id:6908,x:31776,y:33899,varname:node_6908,prsc:2|A-5087-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Subtract,id:5572,x:32801,y:33508,varname:node_5572,prsc:2|A-577-OUT,B-1788-OUT;n:type:ShaderForge.SFN_Vector1,id:1933,x:32872,y:33250,varname:node_1933,prsc:2,v1:-0.1;n:type:ShaderForge.SFN_Smoothstep,id:4707,x:31884,y:32646,varname:node_4707,prsc:2|A-8462-OUT,B-7876-OUT,V-8212-R;n:type:ShaderForge.SFN_Vector1,id:7876,x:31704,y:32663,varname:node_7876,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5686,x:32042,y:32646,varname:node_5686,prsc:2|A-4707-OUT,B-2146-OUT;n:type:ShaderForge.SFN_Vector1,id:2146,x:31884,y:32773,varname:node_2146,prsc:2,v1:30;n:type:ShaderForge.SFN_Clamp01,id:1164,x:32197,y:32646,varname:node_1164,prsc:2|IN-5686-OUT;n:type:ShaderForge.SFN_Smoothstep,id:7871,x:31884,y:32511,varname:node_7871,prsc:2|A-8847-OUT,B-7876-OUT,V-8212-R;n:type:ShaderForge.SFN_Multiply,id:3746,x:32042,y:32511,varname:node_3746,prsc:2|A-7871-OUT,B-5854-OUT;n:type:ShaderForge.SFN_Clamp01,id:6186,x:32211,y:32511,varname:node_6186,prsc:2|IN-3746-OUT;n:type:ShaderForge.SFN_Tex2d,id:2295,x:32124,y:32314,ptovrint:False,ptlb:snow,ptin:_snow,varname:_TestTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9b3b99a770975174c88a44462e2f2c33,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_Vector1,id:8462,x:31704,y:32604,varname:node_8462,prsc:2,v1:0.6;n:type:ShaderForge.SFN_Vector1,id:8847,x:31684,y:32476,varname:node_8847,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Lerp,id:2332,x:32345,y:32249,varname:node_2332,prsc:2|A-8310-RGB,B-2295-RGB,T-6186-OUT;n:type:ShaderForge.SFN_Vector1,id:5854,x:31884,y:32460,varname:node_5854,prsc:2,v1:200;proporder:7736-6728-9153-2090-8212-5642-466-421-9347-7157-8310-433-2295;pass:END;sub:END;*/

Shader "Shader Forge/Terrain_example" {
    Properties {
        _MainTex ("LowTexture", 2D) = "white" {}
        _Tesselation ("Tesselation", Range(1, 60)) = 5
        _DisplacementTexture ("DisplacementTexture", 2D) = "white" {}
        _DepthLow ("DepthLow", Range(0, 1)) = 0.025
        _MedTex ("MedTex", 2D) = "black" {}
        _DepthMed ("DepthMed", Range(0, 1)) = 0.005
        _DepthHigh ("DepthHigh", Range(0, 1)) = 0
        _HighTex ("HighTex", 2D) = "black" {}
        _DefaultGroundTexture ("DefaultGroundTexture", 2D) = "bump" {}
        _TextureShrinkMultiplier ("TextureShrinkMultiplier", Range(1, 100)) = 1
        _TestTex ("TestTex", 2D) = "black" {}
        _DebugTexture ("DebugTexture", 2D) = "white" {}
        _snow ("snow", 2D) = "black" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Front
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore 
            #pragma target 5.0
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Tesselation;
            uniform float _DepthLow;
            uniform sampler2D _MedTex; uniform float4 _MedTex_ST;
            uniform float _DepthMed;
            uniform sampler2D _HighTex; uniform float4 _HighTex_ST;
            uniform float _DepthHigh;
            uniform sampler2D _DefaultGroundTexture; uniform float4 _DefaultGroundTexture_ST;
            uniform float _TextureShrinkMultiplier;
            uniform sampler2D _TestTex; uniform float4 _TestTex_ST;
            uniform sampler2D _snow; uniform float4 _snow_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_7447 = 2.0;
                    float3 SyvyysKeskittaja = (((_DepthLow/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthHigh/node_7447)*v.normal));
                    float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(v.texcoord0, _MainTex),0.0,0));
                    float4 _MedTex_var = tex2Dlod(_MedTex,float4(TRANSFORM_TEX(v.texcoord0, _MedTex),0.0,0));
                    float4 _HighTex_var = tex2Dlod(_HighTex,float4(TRANSFORM_TEX(v.texcoord0, _HighTex),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(_MainTex_var.r*_DepthLow))+(v.normal*(_MedTex_var.r*_DepthMed))+(v.normal*(_HighTex_var.r*_DepthHigh))));
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
////// Emissive:
                float node_1933 = (-0.1);
                float3 emissive = float3(node_1933,node_1933,node_1933);
                float2 node_7517 = (i.uv0*_TextureShrinkMultiplier);
                float4 _DefaultGroundTexture_var = tex2D(_DefaultGroundTexture,TRANSFORM_TEX(node_7517, _DefaultGroundTexture));
                float4 _TestTex_var = tex2D(_TestTex,TRANSFORM_TEX(node_7517, _TestTex));
                float4 _snow_var = tex2D(_snow,TRANSFORM_TEX(node_7517, _snow));
                float node_7876 = 1.0;
                float4 _MedTex_var = tex2D(_MedTex,TRANSFORM_TEX(i.uv0, _MedTex));
                float3 finalColor = emissive + (lerp(_DefaultGroundTexture_var.rgb,lerp(_TestTex_var.rgb,_snow_var.rgb,saturate((smoothstep( 0.7, node_7876, _MedTex_var.r )*200.0))),saturate((smoothstep( 0.6, node_7876, _MedTex_var.r )*30.0)))*_LightColor0.rgb*dot(lightDirection,i.normalDir));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Front
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore 
            #pragma target 5.0
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Tesselation;
            uniform float _DepthLow;
            uniform sampler2D _MedTex; uniform float4 _MedTex_ST;
            uniform float _DepthMed;
            uniform sampler2D _HighTex; uniform float4 _HighTex_ST;
            uniform float _DepthHigh;
            uniform sampler2D _DefaultGroundTexture; uniform float4 _DefaultGroundTexture_ST;
            uniform float _TextureShrinkMultiplier;
            uniform sampler2D _TestTex; uniform float4 _TestTex_ST;
            uniform sampler2D _snow; uniform float4 _snow_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_7447 = 2.0;
                    float3 SyvyysKeskittaja = (((_DepthLow/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthHigh/node_7447)*v.normal));
                    float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(v.texcoord0, _MainTex),0.0,0));
                    float4 _MedTex_var = tex2Dlod(_MedTex,float4(TRANSFORM_TEX(v.texcoord0, _MedTex),0.0,0));
                    float4 _HighTex_var = tex2Dlod(_HighTex,float4(TRANSFORM_TEX(v.texcoord0, _HighTex),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(_MainTex_var.r*_DepthLow))+(v.normal*(_MedTex_var.r*_DepthMed))+(v.normal*(_HighTex_var.r*_DepthHigh))));
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float2 node_7517 = (i.uv0*_TextureShrinkMultiplier);
                float4 _DefaultGroundTexture_var = tex2D(_DefaultGroundTexture,TRANSFORM_TEX(node_7517, _DefaultGroundTexture));
                float4 _TestTex_var = tex2D(_TestTex,TRANSFORM_TEX(node_7517, _TestTex));
                float4 _snow_var = tex2D(_snow,TRANSFORM_TEX(node_7517, _snow));
                float node_7876 = 1.0;
                float4 _MedTex_var = tex2D(_MedTex,TRANSFORM_TEX(i.uv0, _MedTex));
                float3 finalColor = (lerp(_DefaultGroundTexture_var.rgb,lerp(_TestTex_var.rgb,_snow_var.rgb,saturate((smoothstep( 0.7, node_7876, _MedTex_var.r )*200.0))),saturate((smoothstep( 0.6, node_7876, _MedTex_var.r )*30.0)))*_LightColor0.rgb*dot(lightDirection,i.normalDir));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Front
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore 
            #pragma target 5.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Tesselation;
            uniform float _DepthLow;
            uniform sampler2D _MedTex; uniform float4 _MedTex_ST;
            uniform float _DepthMed;
            uniform sampler2D _HighTex; uniform float4 _HighTex_ST;
            uniform float _DepthHigh;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_7447 = 2.0;
                    float3 SyvyysKeskittaja = (((_DepthLow/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthHigh/node_7447)*v.normal));
                    float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(v.texcoord0, _MainTex),0.0,0));
                    float4 _MedTex_var = tex2Dlod(_MedTex,float4(TRANSFORM_TEX(v.texcoord0, _MedTex),0.0,0));
                    float4 _HighTex_var = tex2Dlod(_HighTex,float4(TRANSFORM_TEX(v.texcoord0, _HighTex),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(_MainTex_var.r*_DepthLow))+(v.normal*(_MedTex_var.r*_DepthMed))+(v.normal*(_HighTex_var.r*_DepthHigh))));
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #include "UnityCG.cginc"
            #include "Tessellation.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d11 glcore 
            #pragma target 5.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Tesselation;
            uniform float _DepthLow;
            uniform sampler2D _MedTex; uniform float4 _MedTex_ST;
            uniform float _DepthMed;
            uniform sampler2D _HighTex; uniform float4 _HighTex_ST;
            uniform float _DepthHigh;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_7447 = 2.0;
                    float3 SyvyysKeskittaja = (((_DepthLow/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthHigh/node_7447)*v.normal));
                    float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(v.texcoord0, _MainTex),0.0,0));
                    float4 _MedTex_var = tex2Dlod(_MedTex,float4(TRANSFORM_TEX(v.texcoord0, _MedTex),0.0,0));
                    float4 _HighTex_var = tex2Dlod(_HighTex,float4(TRANSFORM_TEX(v.texcoord0, _HighTex),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(_MainTex_var.r*_DepthLow))+(v.normal*(_MedTex_var.r*_DepthMed))+(v.normal*(_HighTex_var.r*_DepthHigh))));
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_1933 = (-0.1);
                o.Emission = float3(node_1933,node_1933,node_1933);
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
