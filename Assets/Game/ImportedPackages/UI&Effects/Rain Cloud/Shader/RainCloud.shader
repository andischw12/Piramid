Shader "RainCloud/RainCloud" 
{
	Properties 
	{
		_Color("Color",Color) = (1,1,1,1)
		_MainTex("MainTex",2D)="white"{}
		_Alpha("Alpha", Range(0,1)) = 0.5
		_Height("Displacement Amount",range(0,1)) = 0.15
		_HeightAmount("Turbulence Amount",range(0,2)) = 1
		_HeightTileSpeed("Turbulence Tile&Speed",Vector) = (1.0,1.0,0.05,0.0)

		[Space][Space][Space]
		[KeywordEnum(Static,Dynamic)]_MaskType("Mask Type", Float) = 0
		_StaticMaskTex("Static Mask",2D)="white"{}
		_DynamicMaskParams("Dynamic Mask(x:Tiling,y:Intensity1,z:Intensity2,w:TimeScale)", Vector)=(4,2,3,1)

		[Space][Space][Space]
		_SkyboxTint ("Skybox Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _SkyboxExposure ("Skybox Exposure", Range(0, 8)) = 1.0
		[NoScaleOffset] _Skybox ("Skybox Cubemap", Cube) = "grey" {}
		_SkyboxBlendStart("Skybox Blend Start", Float)=500
		_SkyboxBlendEnd("Skybox Blend End", Float)=2000

		[Space][Space][Space]
		[Enum(Low,16,Middle,8,High,4,VeryHigh,1)] _Quality("Quality",Float) = 16

		[Space][Space][Space]
		[KeywordEnum(Off,Mask,Blend)]_Debug("Debug", Float) = 0
	}

	SubShader 
	{
		LOD 300		
        Tags 
		{
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

		Pass
		{
		    Name "FORWARD"
            Tags 
			{
                "LightMode"="ForwardBase"
            }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			
			#pragma multi_compile_fwdbase
            #pragma target 3.0

			#pragma shader_feature _MASKTYPE_STATIC _MASKTYPE_DYNAMIC
			#pragma shader_feature _DEBUG_OFF _DEBUG_MASK _DEBUG_BLEND

			float4 _DynamicMaskParams;

			float perlin_noise_hash21(float2 p)
			{
				float h = dot(p,float2(127.1,311.7));
				return  -1.+2.*frac(sin(h)*43758.5453123);
			}

			float2 perlin_noise_hash22(float2 p)
			{
				p = mul(p,float2x2(127.1,311.7,269.5,183.3));
				p = -1.0 + 2.0 * frac(sin(p)*43758.5453123);
				return sin(p*6.283 + _Time.y*_DynamicMaskParams.w);
			}

			float perlin_noise(float2 p)
			{
				float2 pi = floor(p);
				float2 pf = p-pi;
				
				float2 w = pf*pf*(3.-2.*pf);
				
				float f00 = dot(perlin_noise_hash22(pi+float2(.0,.0)),pf-float2(.0,.0));
				float f01 = dot(perlin_noise_hash22(pi+float2(.0,1.)),pf-float2(.0,1.));
				float f10 = dot(perlin_noise_hash22(pi+float2(1.0,0.)),pf-float2(1.0,0.));
				float f11 = dot(perlin_noise_hash22(pi+float2(1.0,1.)),pf-float2(1.0,1.));
				
				float xm1 = lerp(f00,f10,w.x);
				float xm2 = lerp(f01,f11,w.x);
				
				float ym = lerp(xm1,xm2,w.y); 
				return ym;
			
			}

			float perlin_noise_sum(float2 p)
			{
				p *= 4.;
				float a = 1., r = 0., s=0.;
				
				for (int i=0; i<5; i++) {
				r += a*perlin_noise(p); s+= a; p *= 2.; a*=.5;
				}
				
				return r/s;
			}

			half perlin_noise_do(float2 uv)
			{     
				float f = perlin_noise_sum(uv);
				f = f*0.5+0.5;
				return saturate(f);
			}

			sampler2D _StaticMaskTex;
			float4 _StaticMaskTex_ST;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _Height;
			float4 _HeightTileSpeed;
			half _HeightAmount;
			half4 _Color;
			half _Alpha;

			samplerCUBE _Skybox;
			half4 _SkyboxTint;
			half _SkyboxExposure;
			float _SkyboxBlendStart;
			float _SkyboxBlendEnd;

			float _Quality;

			struct v2f 
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				float4 posWorld : TEXCOORD4;
				
				UNITY_FOG_COORDS(7)
			};

			v2f vert (appdata_full v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex) + frac(_Time.y*_HeightTileSpeed.zw);
				o.uv.zw = v.texcoord * _HeightTileSpeed.xy;
				o.uv2.xy = TRANSFORM_TEX(v.texcoord,_StaticMaskTex);
				o.uv2.zw = v.texcoord;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				TANGENT_SPACE_ROTATION;
				o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float3 viewRay=normalize(i.viewDir*-1);
				viewRay.z=abs(viewRay.z)+0.2;
				viewRay.xy *= _Height;

				float3 shadeP = float3(i.uv.xy,0);
				float3 shadeP2 = float3(i.uv.zw,0);


				float linearStep = _Quality;

				float4 T = tex2D(_MainTex, shadeP2.xy);
				float h2 = T.a * _HeightAmount;

				float3 lioffset = viewRay / (viewRay.z * linearStep);
				float d = 1.0 - tex2Dlod(_MainTex, float4(shadeP.xy,0,0)).a * h2;
				float3 prev_d = d;
				float3 prev_shadeP = shadeP;
				while(d > shadeP.z)
				{
					prev_shadeP = shadeP;
					shadeP += lioffset;
					prev_d = d;
					d = 1.0 - tex2Dlod(_MainTex, float4(shadeP.xy,0,0)).a * h2;
				}
				float d1 = d - shadeP.z;
				float d2 = prev_d - prev_shadeP.z;
				float w = saturate(d1 / (d1 - d2));
				shadeP = lerp(shadeP, prev_shadeP, w);
				

#if defined(_MASKTYPE_STATIC)
				half perlinNoise = tex2D(_StaticMaskTex,i.uv2.xy).r;
#endif
#if defined(_MASKTYPE_DYNAMIC)
				half perlinNoise = perlin_noise_do(i.uv2.zw*_DynamicMaskParams.x).x;
				perlinNoise *=_DynamicMaskParams.y;
				perlinNoise = pow(perlinNoise, _DynamicMaskParams.z);
				perlinNoise = saturate(perlinNoise);
#endif
#if defined(_DEBUG_MASK)
				return half4(perlinNoise.xxx, 1);
#endif

				half4 c = tex2D(_MainTex,shadeP.xy) * T * _Color;
				half Alpha = lerp(c.a*perlinNoise, 1.0, _Alpha);

				float3 normal = normalize(i.normalDir);
				half3 lightDir = UnityWorldSpaceLightDir(i.posWorld);
				float NdotL = max(0,dot(normal,lightDir));
				half3 lightColor = _LightColor0.rgb;
                fixed3 finalColor = c.rgb*(NdotL*lightColor + 1.0);

				float3 viewDir = -UnityWorldSpaceViewDir(i.posWorld);
				half3 skyboxC = texCUBE (_Skybox, viewDir).rgb;
				skyboxC = skyboxC * _SkyboxTint.rgb * unity_ColorSpaceDouble.rgb;
				skyboxC *= _SkyboxExposure;
				float viewDist = length(viewDir);
				float blendStart = _SkyboxBlendStart;
				float blendEnd = _SkyboxBlendEnd;
				float blend = saturate((viewDist - blendStart) / (blendEnd - blendStart));
				half3 blendC = lerp(finalColor.rgb, skyboxC.rgb, blend);

#if defined(_DEBUG_BLEND)
				return half4(blend.xxx,1);
#endif

				return half4(blendC, saturate(Alpha));
			}
		ENDCG
		}
	}

	FallBack "Diffuse"
}
