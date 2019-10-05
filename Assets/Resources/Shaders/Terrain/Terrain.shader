// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:1,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33036,y:33151,varname:node_2865,prsc:2|emission-1933-OUT,custl-6543-OUT,disp-5572-OUT,tess-6728-OUT;n:type:ShaderForge.SFN_Multiply,id:5583,x:32241,y:33443,varname:node_5583,prsc:2|A-1618-OUT,B-6711-OUT;n:type:ShaderForge.SFN_NormalVector,id:1618,x:31990,y:33320,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:6728,x:32655,y:33752,ptovrint:False,ptlb:Tesselation,ptin:_Tesselation,varname:node_6728,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:5,max:60;n:type:ShaderForge.SFN_Multiply,id:6711,x:31990,y:33472,varname:node_6711,prsc:2|A-7868-OUT,B-6790-OUT;n:type:ShaderForge.SFN_Slider,id:2090,x:31312,y:33731,ptovrint:False,ptlb:DepthLowPlainHills,ptin:_DepthLowPlainHills,varname:node_2090,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.02,max:1;n:type:ShaderForge.SFN_Tex2d,id:8212,x:31363,y:33177,ptovrint:False,ptlb:MediumDetail,ptin:_MediumDetail,varname:node_8212,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:5642,x:31312,y:33833,ptovrint:False,ptlb:DepthMed,ptin:_DepthMed,varname:node_5642,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.002,max:1;n:type:ShaderForge.SFN_Multiply,id:6271,x:31990,y:33595,varname:node_6271,prsc:2|A-5420-OUT,B-2090-OUT;n:type:ShaderForge.SFN_Slider,id:466,x:31312,y:33932,ptovrint:False,ptlb:DepthOcean,ptin:_DepthOcean,varname:node_466,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:3468,x:31990,y:33720,varname:node_3468,prsc:2|A-5156-OUT,B-5642-OUT;n:type:ShaderForge.SFN_LightVector,id:5377,x:31990,y:33176,varname:node_5377,prsc:2;n:type:ShaderForge.SFN_Dot,id:990,x:32202,y:33176,varname:node_990,prsc:2,dt:0|A-5377-OUT,B-1618-OUT;n:type:ShaderForge.SFN_Multiply,id:6543,x:32615,y:32946,varname:node_6543,prsc:2|A-3849-OUT,B-9900-RGB,C-990-OUT;n:type:ShaderForge.SFN_LightColor,id:9900,x:32079,y:32911,varname:node_9900,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:9931,x:32079,y:33026,varname:node_9931,prsc:2;n:type:ShaderForge.SFN_Multiply,id:125,x:32241,y:33569,varname:node_125,prsc:2|A-1618-OUT,B-6271-OUT;n:type:ShaderForge.SFN_Multiply,id:509,x:32241,y:33700,varname:node_509,prsc:2|A-1618-OUT,B-3468-OUT;n:type:ShaderForge.SFN_Tex2d,id:9347,x:32124,y:31957,ptovrint:False,ptlb:DefaultGroundTexture,ptin:_DefaultGroundTexture,varname:node_9347,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1feabf6ed54578e479505b471705ceb9,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_TexCoord,id:9221,x:31702,y:31910,varname:node_9221,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:7517,x:31903,y:31987,varname:node_7517,prsc:2|A-9221-UVOUT,B-7157-OUT;n:type:ShaderForge.SFN_Add,id:1788,x:32485,y:33569,varname:node_1788,prsc:2|A-5583-OUT,B-125-OUT,C-509-OUT,D-8324-OUT;n:type:ShaderForge.SFN_Slider,id:7157,x:31545,y:32080,ptovrint:False,ptlb:TextureShrinkMultiplier,ptin:_TextureShrinkMultiplier,varname:node_7157,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:1000;n:type:ShaderForge.SFN_Lerp,id:3849,x:32676,y:32387,varname:node_3849,prsc:2|A-3067-OUT,B-2332-OUT,T-1164-OUT;n:type:ShaderForge.SFN_Tex2d,id:8310,x:32124,y:32139,ptovrint:False,ptlb:MountainsTexture,ptin:_MountainsTexture,varname:_node_9347_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4785e388e18957e4aa226e9fe8f7f15f,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_Tex2d,id:433,x:33107,y:32863,ptovrint:False,ptlb:DebugTexture,ptin:_DebugTexture,varname:node_433,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Divide,id:818,x:31858,y:34610,varname:node_818,prsc:2|A-466-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Vector1,id:7447,x:31546,y:34488,varname:node_7447,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:4154,x:32024,y:34610,varname:node_4154,prsc:2|A-818-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Set,id:7772,x:32422,y:34448,varname:SyvyysKeskittaja,prsc:2|IN-5170-OUT;n:type:ShaderForge.SFN_Get,id:577,x:32485,y:33508,varname:node_577,prsc:2|IN-7772-OUT;n:type:ShaderForge.SFN_NormalVector,id:4434,x:31858,y:34762,prsc:2,pt:False;n:type:ShaderForge.SFN_Add,id:5170,x:32224,y:34448,varname:node_5170,prsc:2|A-4411-OUT,B-6908-OUT,C-9962-OUT,D-4154-OUT;n:type:ShaderForge.SFN_Divide,id:7797,x:31858,y:34471,varname:node_7797,prsc:2|A-5642-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Multiply,id:9962,x:32024,y:34471,varname:node_9962,prsc:2|A-7797-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Divide,id:5087,x:31858,y:34342,varname:node_5087,prsc:2|A-2090-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Multiply,id:6908,x:32024,y:34342,varname:node_6908,prsc:2|A-5087-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Subtract,id:5572,x:32749,y:33530,varname:node_5572,prsc:2|A-577-OUT,B-1788-OUT;n:type:ShaderForge.SFN_Vector1,id:1933,x:32715,y:33252,varname:node_1933,prsc:2,v1:-0.1;n:type:ShaderForge.SFN_Smoothstep,id:4707,x:31884,y:32646,varname:node_4707,prsc:2|A-8462-OUT,B-7876-OUT,V-7868-OUT;n:type:ShaderForge.SFN_Vector1,id:7876,x:31667,y:32577,varname:node_7876,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5686,x:32042,y:32647,varname:node_5686,prsc:2|A-4707-OUT,B-2146-OUT;n:type:ShaderForge.SFN_Vector1,id:2146,x:31884,y:32771,varname:node_2146,prsc:2,v1:30;n:type:ShaderForge.SFN_Clamp01,id:1164,x:32373,y:32647,varname:node_1164,prsc:2|IN-5686-OUT;n:type:ShaderForge.SFN_Smoothstep,id:7871,x:31884,y:32511,varname:node_7871,prsc:2|A-8847-OUT,B-7876-OUT,V-7868-OUT;n:type:ShaderForge.SFN_Multiply,id:3746,x:32042,y:32511,varname:node_3746,prsc:2|A-7871-OUT,B-5854-OUT;n:type:ShaderForge.SFN_Clamp01,id:6186,x:32373,y:32511,varname:node_6186,prsc:2|IN-3746-OUT;n:type:ShaderForge.SFN_Tex2d,id:2295,x:32124,y:32314,ptovrint:False,ptlb:MountainTopTexture,ptin:_MountainTopTexture,varname:_TestTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9b3b99a770975174c88a44462e2f2c33,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_Vector1,id:8462,x:31704,y:32646,varname:node_8462,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Vector1,id:8847,x:31704,y:32511,varname:node_8847,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Lerp,id:2332,x:32373,y:32241,varname:node_2332,prsc:2|A-8310-RGB,B-2295-RGB,T-6186-OUT;n:type:ShaderForge.SFN_Vector1,id:5854,x:31884,y:32456,varname:node_5854,prsc:2,v1:200;n:type:ShaderForge.SFN_Tex2d,id:2565,x:31363,y:32802,ptovrint:False,ptlb:LargeMountains,ptin:_LargeMountains,varname:node_2565,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9249,x:31492,y:33396,ptovrint:False,ptlb:Ocean,ptin:_Ocean,varname:_BigMountains_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8324,x:32241,y:33832,varname:node_8324,prsc:2|A-1618-OUT,B-9994-OUT;n:type:ShaderForge.SFN_Multiply,id:9994,x:31990,y:33844,varname:node_9994,prsc:2|A-9249-R,B-466-OUT;n:type:ShaderForge.SFN_Divide,id:127,x:31858,y:34213,varname:node_127,prsc:2|A-6790-OUT,B-7447-OUT;n:type:ShaderForge.SFN_Multiply,id:4411,x:32024,y:34213,varname:node_4411,prsc:2|A-127-OUT,B-4434-OUT;n:type:ShaderForge.SFN_Slider,id:6790,x:31312,y:33627,ptovrint:False,ptlb:DepthLargeMountais,ptin:_DepthLargeMountais,varname:_DepthOcean_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.08,max:1;n:type:ShaderForge.SFN_Tex2d,id:2079,x:31363,y:32987,ptovrint:False,ptlb:PlainHills,ptin:_PlainHills,varname:node_2079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1526,x:32124,y:31778,ptovrint:False,ptlb:OceanBottomTexture,ptin:_OceanBottomTexture,varname:_DefaultGroundTexture_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:af075ea93db262f448a6f6c59558dcfd,ntxv:2,isnm:False|UVIN-7517-OUT;n:type:ShaderForge.SFN_Lerp,id:3067,x:32382,y:31948,varname:node_3067,prsc:2|A-1526-RGB,B-9347-RGB,T-9786-OUT;n:type:ShaderForge.SFN_Tex2d,id:1030,x:31327,y:32039,ptovrint:False,ptlb:_MainTex,ptin:__MainTex,varname:node_1030,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Smoothstep,id:3683,x:31052,y:33230,varname:node_3683,prsc:2|A-5551-OUT,B-8898-OUT,V-9249-R;n:type:ShaderForge.SFN_Vector1,id:5551,x:30841,y:33212,varname:node_5551,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:8898,x:30841,y:33264,varname:node_8898,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Set,id:7666,x:31191,y:33230,varname:OceanNoise,prsc:2|IN-3683-OUT;n:type:ShaderForge.SFN_Get,id:9786,x:32341,y:32080,varname:node_9786,prsc:2|IN-7666-OUT;n:type:ShaderForge.SFN_Get,id:6047,x:31194,y:32654,varname:node_6047,prsc:2|IN-7666-OUT;n:type:ShaderForge.SFN_Subtract,id:7868,x:31540,y:32767,varname:node_7868,prsc:2|A-2565-R,B-6320-OUT;n:type:ShaderForge.SFN_OneMinus,id:6320,x:31363,y:32654,varname:node_6320,prsc:2|IN-6047-OUT;n:type:ShaderForge.SFN_Subtract,id:5420,x:31540,y:32976,varname:node_5420,prsc:2|A-2079-R,B-6320-OUT;n:type:ShaderForge.SFN_Subtract,id:5156,x:31540,y:33161,varname:node_5156,prsc:2|A-8212-R,B-6320-OUT;proporder:6728-2090-8212-5642-466-9347-7157-8310-433-2295-9249-2079-2565-6790-1526-1030;pass:END;sub:END;*/

Shader "Shader Forge/Terrain" {
    Properties {
        _Tesselation ("Tesselation", Range(1, 60)) = 5
        _DepthLowPlainHills ("DepthLowPlainHills", Range(0, 1)) = 0.02
        _MediumDetail ("MediumDetail", 2D) = "white" {}
        _DepthMed ("DepthMed", Range(0, 1)) = 0.002
        _DepthOcean ("DepthOcean", Range(0, 1)) = 0
        _DefaultGroundTexture ("DefaultGroundTexture", 2D) = "black" {}
        _TextureShrinkMultiplier ("TextureShrinkMultiplier", Range(1, 1000)) = 1
        _MountainsTexture ("MountainsTexture", 2D) = "black" {}
        _DebugTexture ("DebugTexture", 2D) = "white" {}
        _MountainTopTexture ("MountainTopTexture", 2D) = "black" {}
        _Ocean ("Ocean", 2D) = "white" {}
        _PlainHills ("PlainHills", 2D) = "white" {}
        _LargeMountains ("LargeMountains", 2D) = "white" {}
        _DepthLargeMountais ("DepthLargeMountais", Range(0, 1)) = 0.08
        _OceanBottomTexture ("OceanBottomTexture", 2D) = "black" {}
        __MainTex ("_MainTex", 2D) = "white" {}
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
            uniform float _Tesselation;
            uniform float _DepthLowPlainHills;
            uniform sampler2D _MediumDetail; uniform float4 _MediumDetail_ST;
            uniform float _DepthMed;
            uniform float _DepthOcean;
            uniform sampler2D _DefaultGroundTexture; uniform float4 _DefaultGroundTexture_ST;
            uniform float _TextureShrinkMultiplier;
            uniform sampler2D _MountainsTexture; uniform float4 _MountainsTexture_ST;
            uniform sampler2D _MountainTopTexture; uniform float4 _MountainTopTexture_ST;
            uniform sampler2D _LargeMountains; uniform float4 _LargeMountains_ST;
            uniform sampler2D _Ocean; uniform float4 _Ocean_ST;
            uniform float _DepthLargeMountais;
            uniform sampler2D _PlainHills; uniform float4 _PlainHills_ST;
            uniform sampler2D _OceanBottomTexture; uniform float4 _OceanBottomTexture_ST;
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
                o.pos = UnityObjectToClipPos( v.vertex );
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
                    float3 SyvyysKeskittaja = (((_DepthLargeMountais/node_7447)*v.normal)+((_DepthLowPlainHills/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthOcean/node_7447)*v.normal));
                    float4 _LargeMountains_var = tex2Dlod(_LargeMountains,float4(TRANSFORM_TEX(v.texcoord0, _LargeMountains),0.0,0));
                    float4 _Ocean_var = tex2Dlod(_Ocean,float4(TRANSFORM_TEX(v.texcoord0, _Ocean),0.0,0));
                    float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                    float node_6320 = (1.0 - OceanNoise);
                    float node_7868 = (_LargeMountains_var.r-node_6320);
                    float4 _PlainHills_var = tex2Dlod(_PlainHills,float4(TRANSFORM_TEX(v.texcoord0, _PlainHills),0.0,0));
                    float4 _MediumDetail_var = tex2Dlod(_MediumDetail,float4(TRANSFORM_TEX(v.texcoord0, _MediumDetail),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(node_7868*_DepthLargeMountais))+(v.normal*((_PlainHills_var.r-node_6320)*_DepthLowPlainHills))+(v.normal*((_MediumDetail_var.r-node_6320)*_DepthMed))+(v.normal*(_Ocean_var.r*_DepthOcean))));
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
                float4 _OceanBottomTexture_var = tex2D(_OceanBottomTexture,TRANSFORM_TEX(node_7517, _OceanBottomTexture));
                float4 _DefaultGroundTexture_var = tex2D(_DefaultGroundTexture,TRANSFORM_TEX(node_7517, _DefaultGroundTexture));
                float4 _Ocean_var = tex2D(_Ocean,TRANSFORM_TEX(i.uv0, _Ocean));
                float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                float4 _MountainsTexture_var = tex2D(_MountainsTexture,TRANSFORM_TEX(node_7517, _MountainsTexture));
                float4 _MountainTopTexture_var = tex2D(_MountainTopTexture,TRANSFORM_TEX(node_7517, _MountainTopTexture));
                float node_7876 = 1.0;
                float4 _LargeMountains_var = tex2D(_LargeMountains,TRANSFORM_TEX(i.uv0, _LargeMountains));
                float node_6320 = (1.0 - OceanNoise);
                float node_7868 = (_LargeMountains_var.r-node_6320);
                float3 finalColor = emissive + (lerp(lerp(_OceanBottomTexture_var.rgb,_DefaultGroundTexture_var.rgb,OceanNoise),lerp(_MountainsTexture_var.rgb,_MountainTopTexture_var.rgb,saturate((smoothstep( 0.7, node_7876, node_7868 )*200.0))),saturate((smoothstep( 0.5, node_7876, node_7868 )*30.0)))*_LightColor0.rgb*dot(lightDirection,i.normalDir));
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
            uniform float _Tesselation;
            uniform float _DepthLowPlainHills;
            uniform sampler2D _MediumDetail; uniform float4 _MediumDetail_ST;
            uniform float _DepthMed;
            uniform float _DepthOcean;
            uniform sampler2D _DefaultGroundTexture; uniform float4 _DefaultGroundTexture_ST;
            uniform float _TextureShrinkMultiplier;
            uniform sampler2D _MountainsTexture; uniform float4 _MountainsTexture_ST;
            uniform sampler2D _MountainTopTexture; uniform float4 _MountainTopTexture_ST;
            uniform sampler2D _LargeMountains; uniform float4 _LargeMountains_ST;
            uniform sampler2D _Ocean; uniform float4 _Ocean_ST;
            uniform float _DepthLargeMountais;
            uniform sampler2D _PlainHills; uniform float4 _PlainHills_ST;
            uniform sampler2D _OceanBottomTexture; uniform float4 _OceanBottomTexture_ST;
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
                o.pos = UnityObjectToClipPos( v.vertex );
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
                    float3 SyvyysKeskittaja = (((_DepthLargeMountais/node_7447)*v.normal)+((_DepthLowPlainHills/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthOcean/node_7447)*v.normal));
                    float4 _LargeMountains_var = tex2Dlod(_LargeMountains,float4(TRANSFORM_TEX(v.texcoord0, _LargeMountains),0.0,0));
                    float4 _Ocean_var = tex2Dlod(_Ocean,float4(TRANSFORM_TEX(v.texcoord0, _Ocean),0.0,0));
                    float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                    float node_6320 = (1.0 - OceanNoise);
                    float node_7868 = (_LargeMountains_var.r-node_6320);
                    float4 _PlainHills_var = tex2Dlod(_PlainHills,float4(TRANSFORM_TEX(v.texcoord0, _PlainHills),0.0,0));
                    float4 _MediumDetail_var = tex2Dlod(_MediumDetail,float4(TRANSFORM_TEX(v.texcoord0, _MediumDetail),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(node_7868*_DepthLargeMountais))+(v.normal*((_PlainHills_var.r-node_6320)*_DepthLowPlainHills))+(v.normal*((_MediumDetail_var.r-node_6320)*_DepthMed))+(v.normal*(_Ocean_var.r*_DepthOcean))));
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
                float4 _OceanBottomTexture_var = tex2D(_OceanBottomTexture,TRANSFORM_TEX(node_7517, _OceanBottomTexture));
                float4 _DefaultGroundTexture_var = tex2D(_DefaultGroundTexture,TRANSFORM_TEX(node_7517, _DefaultGroundTexture));
                float4 _Ocean_var = tex2D(_Ocean,TRANSFORM_TEX(i.uv0, _Ocean));
                float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                float4 _MountainsTexture_var = tex2D(_MountainsTexture,TRANSFORM_TEX(node_7517, _MountainsTexture));
                float4 _MountainTopTexture_var = tex2D(_MountainTopTexture,TRANSFORM_TEX(node_7517, _MountainTopTexture));
                float node_7876 = 1.0;
                float4 _LargeMountains_var = tex2D(_LargeMountains,TRANSFORM_TEX(i.uv0, _LargeMountains));
                float node_6320 = (1.0 - OceanNoise);
                float node_7868 = (_LargeMountains_var.r-node_6320);
                float3 finalColor = (lerp(lerp(_OceanBottomTexture_var.rgb,_DefaultGroundTexture_var.rgb,OceanNoise),lerp(_MountainsTexture_var.rgb,_MountainTopTexture_var.rgb,saturate((smoothstep( 0.7, node_7876, node_7868 )*200.0))),saturate((smoothstep( 0.5, node_7876, node_7868 )*30.0)))*_LightColor0.rgb*dot(lightDirection,i.normalDir));
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
            uniform float _Tesselation;
            uniform float _DepthLowPlainHills;
            uniform sampler2D _MediumDetail; uniform float4 _MediumDetail_ST;
            uniform float _DepthMed;
            uniform float _DepthOcean;
            uniform sampler2D _LargeMountains; uniform float4 _LargeMountains_ST;
            uniform sampler2D _Ocean; uniform float4 _Ocean_ST;
            uniform float _DepthLargeMountais;
            uniform sampler2D _PlainHills; uniform float4 _PlainHills_ST;
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
                o.pos = UnityObjectToClipPos( v.vertex );
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
                    float3 SyvyysKeskittaja = (((_DepthLargeMountais/node_7447)*v.normal)+((_DepthLowPlainHills/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthOcean/node_7447)*v.normal));
                    float4 _LargeMountains_var = tex2Dlod(_LargeMountains,float4(TRANSFORM_TEX(v.texcoord0, _LargeMountains),0.0,0));
                    float4 _Ocean_var = tex2Dlod(_Ocean,float4(TRANSFORM_TEX(v.texcoord0, _Ocean),0.0,0));
                    float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                    float node_6320 = (1.0 - OceanNoise);
                    float node_7868 = (_LargeMountains_var.r-node_6320);
                    float4 _PlainHills_var = tex2Dlod(_PlainHills,float4(TRANSFORM_TEX(v.texcoord0, _PlainHills),0.0,0));
                    float4 _MediumDetail_var = tex2Dlod(_MediumDetail,float4(TRANSFORM_TEX(v.texcoord0, _MediumDetail),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(node_7868*_DepthLargeMountais))+(v.normal*((_PlainHills_var.r-node_6320)*_DepthLowPlainHills))+(v.normal*((_MediumDetail_var.r-node_6320)*_DepthMed))+(v.normal*(_Ocean_var.r*_DepthOcean))));
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
            uniform float _Tesselation;
            uniform float _DepthLowPlainHills;
            uniform sampler2D _MediumDetail; uniform float4 _MediumDetail_ST;
            uniform float _DepthMed;
            uniform float _DepthOcean;
            uniform sampler2D _LargeMountains; uniform float4 _LargeMountains_ST;
            uniform sampler2D _Ocean; uniform float4 _Ocean_ST;
            uniform float _DepthLargeMountais;
            uniform sampler2D _PlainHills; uniform float4 _PlainHills_ST;
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
                    float3 SyvyysKeskittaja = (((_DepthLargeMountais/node_7447)*v.normal)+((_DepthLowPlainHills/node_7447)*v.normal)+((_DepthMed/node_7447)*v.normal)+((_DepthOcean/node_7447)*v.normal));
                    float4 _LargeMountains_var = tex2Dlod(_LargeMountains,float4(TRANSFORM_TEX(v.texcoord0, _LargeMountains),0.0,0));
                    float4 _Ocean_var = tex2Dlod(_Ocean,float4(TRANSFORM_TEX(v.texcoord0, _Ocean),0.0,0));
                    float OceanNoise = smoothstep( 0.0, 0.5, _Ocean_var.r );
                    float node_6320 = (1.0 - OceanNoise);
                    float node_7868 = (_LargeMountains_var.r-node_6320);
                    float4 _PlainHills_var = tex2Dlod(_PlainHills,float4(TRANSFORM_TEX(v.texcoord0, _PlainHills),0.0,0));
                    float4 _MediumDetail_var = tex2Dlod(_MediumDetail,float4(TRANSFORM_TEX(v.texcoord0, _MediumDetail),0.0,0));
                    v.vertex.xyz += (SyvyysKeskittaja-((v.normal*(node_7868*_DepthLargeMountais))+(v.normal*((_PlainHills_var.r-node_6320)*_DepthLowPlainHills))+(v.normal*((_MediumDetail_var.r-node_6320)*_DepthMed))+(v.normal*(_Ocean_var.r*_DepthOcean))));
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
