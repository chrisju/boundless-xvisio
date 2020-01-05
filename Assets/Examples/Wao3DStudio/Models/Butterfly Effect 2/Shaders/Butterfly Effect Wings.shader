// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:865,x:33223,y:32192,varname:node_865,prsc:2|normal-6599-RGB,custl-2930-OUT,clip-875-OUT;n:type:ShaderForge.SFN_Tex2d,id:7009,x:31476,y:31553,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_7009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1be0585cae0981343afb2c53f88d2038,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:7407,x:32421,y:33209,ptovrint:False,ptlb:Alpha Cutout,ptin:_AlphaCutout,varname:node_7407,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:8923,x:32714,y:32687,varname:node_8923,prsc:2|A-7009-A,B-2763-OUT;n:type:ShaderForge.SFN_OneMinus,id:2763,x:32852,y:33021,varname:node_2763,prsc:2|IN-7407-OUT;n:type:ShaderForge.SFN_Clamp01,id:875,x:32907,y:32611,varname:node_875,prsc:2|IN-8923-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:3977,x:32067,y:31840,varname:node_3977,prsc:2;n:type:ShaderForge.SFN_LightColor,id:4756,x:32261,y:32107,varname:node_4756,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2930,x:32650,y:32154,varname:node_2930,prsc:2|A-3977-OUT,B-4756-RGB,C-6726-OUT;n:type:ShaderForge.SFN_NormalVector,id:7222,x:31238,y:32558,prsc:2,pt:True;n:type:ShaderForge.SFN_HalfVector,id:5515,x:31238,y:32790,varname:node_5515,prsc:2;n:type:ShaderForge.SFN_Dot,id:840,x:31593,y:32599,varname:node_840,prsc:2,dt:0|A-7222-OUT,B-5515-OUT;n:type:ShaderForge.SFN_Power,id:8604,x:31833,y:32652,varname:node_8604,prsc:2|VAL-840-OUT,EXP-2492-OUT;n:type:ShaderForge.SFN_Add,id:6726,x:32375,y:32237,varname:node_6726,prsc:2|A-9325-OUT,B-556-OUT;n:type:ShaderForge.SFN_Exp,id:2492,x:31671,y:32870,varname:node_2492,prsc:2,et:1|IN-7198-OUT;n:type:ShaderForge.SFN_Multiply,id:556,x:32293,y:32519,varname:node_556,prsc:2|A-8604-OUT,B-4842-OUT,C-5636-RGB;n:type:ShaderForge.SFN_Color,id:6369,x:31499,y:31773,ptovrint:False,ptlb:DiffuseColor,ptin:_DiffuseColor,varname:node_2323,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:7198,x:31255,y:33050,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_7198,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:20;n:type:ShaderForge.SFN_Slider,id:4842,x:31958,y:33074,ptovrint:False,ptlb:Spec,ptin:_Spec,varname:node_4842,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:6599,x:32740,y:31902,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_6599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e2589de488ff1734b832c613ad0ab854,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Color,id:5636,x:32092,y:32822,ptovrint:False,ptlb:SpecColor,ptin:_SpecColor,varname:node_5636,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4852941,c2:0.4852941,c3:0.4852941,c4:1;n:type:ShaderForge.SFN_Multiply,id:6985,x:31917,y:32132,varname:node_6985,prsc:2|A-7009-RGB,B-6369-RGB;n:type:ShaderForge.SFN_LightVector,id:7560,x:31238,y:32415,varname:node_7560,prsc:2;n:type:ShaderForge.SFN_Dot,id:8160,x:31713,y:32351,varname:node_8160,prsc:2,dt:0|A-7560-OUT,B-7222-OUT;n:type:ShaderForge.SFN_Multiply,id:9325,x:32075,y:32257,varname:node_9325,prsc:2|A-6985-OUT,B-8160-OUT;proporder:6369-5636-7009-6599-7407-7198-4842;pass:END;sub:END;*/

Shader "Wao3DStudio/Butterfly Effect/Wings" {
    Properties {
        _DiffuseColor ("DiffuseColor", Color) = (1,1,1,1)
        _SpecColor ("SpecColor", Color) = (0.4852941,0.4852941,0.4852941,1)
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _AlphaCutout ("Alpha Cutout", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 20)) = 0
        _Spec ("Spec", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _AlphaCutout;
            uniform float4 _DiffuseColor;
            uniform float _Gloss;
            uniform float _Spec;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
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
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(saturate((_Diffuse_var.a*(1.0 - _AlphaCutout))) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = (attenuation*_LightColor0.rgb*(((_Diffuse_var.rgb*_DiffuseColor.rgb)*dot(lightDirection,normalDirection))+(pow(dot(normalDirection,halfDirection),exp2(_Gloss))*_Spec*_SpecColor.rgb)));
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _AlphaCutout;
            uniform float4 _DiffuseColor;
            uniform float _Gloss;
            uniform float _Spec;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
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
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(saturate((_Diffuse_var.a*(1.0 - _AlphaCutout))) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = (attenuation*_LightColor0.rgb*(((_Diffuse_var.rgb*_DiffuseColor.rgb)*dot(lightDirection,normalDirection))+(pow(dot(normalDirection,halfDirection),exp2(_Gloss))*_Spec*_SpecColor.rgb)));
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
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _AlphaCutout;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(saturate((_Diffuse_var.a*(1.0 - _AlphaCutout))) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
