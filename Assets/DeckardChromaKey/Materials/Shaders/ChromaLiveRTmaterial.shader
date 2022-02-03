// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ChromaLiveRTMaterial"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Toggle]_MateKey("MateKey", Float) = 1
		_ultimatte("ultimatte", 2D) = "white" {}
		_KeyColor1("KeyColor1", Color) = (0,0,0,0)
		[RemapSlidersFull1]_Threshold("Threshold", Vector) = (-1,1,-1,1)
		[RemapSlidersFull1]_ThresholdL("ThresholdL", Vector) = (-1,1,-1,1)
		_MainTex("_MainTex", 2D) = "white" {}
		_PreCorrectGamma("PreCorrectGamma", Float) = 0
		_Border("Border", Range( -0.01 , 0.003)) = 0
		_MaskPower("MaskPower", Float) = 0
		_despillChroma("despillChroma", Range( 0 , 2)) = 0
		_despillLuma("despillLuma", Range( -1 , 5)) = 0
		[Header(Color Correction)]_Gamma("Gamma", Float) = 1
		_whiteBallance("whiteBallance", Range( -1 , 1)) = 1
		_tint("tint", Range( -1 , 1)) = 1
		_Sharpen("Sharpen", Float) = 0
		[HideInInspector]_Blurring("Blurring", Float) = 0
		_ColorCleanup("ColorCleanup", Range( 0 , 1)) = 0
		_KeyingAlgorithm("Keying Algorithm", Range( 0 , 1)) = 0.5
		_TransitionPosition("TransitionPosition", Range( 0 , 1)) = 0.5
		_TransitionFalloff("TransitionFalloff", Range( 0 , 1)) = 0.5
		_CustomMask("CustomMask", 2D) = "white" {}
		_GreenCastRemove("GreenCastRemove", Range( 0 , 1)) = 0
		_cropTop("cropTop", Range( 0 , 1)) = 1
		_cropBottom("cropBottom", Range( 0 , 1)) = 0
		_cropLeft("cropLeft", Range( 0 , 1)) = 0
		_cropRight("cropRight", Range( 0 , 1)) = 0
		[ASEEnd][Toggle]_PortraitMode("PortraitMode", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		[HideInInspector] [ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector] [ToggleUI]_SupportDecals("Boolean", Float) = 1
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 6
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 8
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 40
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 40
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 14
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 10
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector] [ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector] [ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector] [ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector] [ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 1
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Int) = 0
		[HideInInspector] [ToggleUI] _EnableFogOnTransparent("Enable Fog On Transparent", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2
		[HideInInspector] [Enum(Front, 1, Back, 2)] _TransparentCullMode("Transparent Cull Mode", Float) = 2
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4
		[HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Float) = 4
		[HideInInspector] [ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector] [ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 1
		[HideInInspector] [ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector] [ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector] [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer

		struct GlobalSurfaceDescription // GBuffer Forward META TransparentBackface
		{
			float3 Albedo;
			float3 Normal;
			float3 BentNormal;
			float3 Specular;
			float CoatMask;
			float Metallic;
			float3 Emission;
			float Smoothness;
			float Occlusion;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float AlphaClipThresholdDepthPostpass;
			float SpecularAAScreenSpaceVariance;
			float SpecularAAThreshold;
			float SpecularOcclusion;
			float DepthOffset;
			//Refraction
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			//SSS/Translucent
			float Thickness;
			float SubsurfaceMask;
			float DiffusionProfile;
			//Anisotropy
			float Anisotropy;
			float3 Tangent;
			//Iridescent
			float IridescenceMask;
			float IridescenceThickness;
			//BakedGI
			float3 BakedGI;
			float3 BakedBackGI;
		};

		struct AlphaSurfaceDescription // ShadowCaster
		{
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float DepthOffset;
		};

		struct SceneSurfaceDescription // SceneSelection
		{
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct PrePassSurfaceDescription // DepthPrePass
		{
			float Alpha;
			float AlphaClipThresholdDepthPrepass;
			float DepthOffset;
		};

		struct PostPassSurfaceDescription //DepthPostPass
		{
			float Alpha;
			float AlphaClipThresholdDepthPostpass;
			float DepthOffset;
		};

		struct SmoothSurfaceDescription // MotionVectors DepthOnly
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct DistortionSurfaceDescription //Distortion
		{
			float Alpha;
			float2 Distortion;
			float DistortionBlur;
			float AlphaClipThreshold;
		};
		
		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL
		
		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="GBuffer" }

			Cull [_CullMode]
			ZTest [_ZTestGBuffer]

			Stencil
			{
				Ref [_StencilRefGBuffer]
				WriteMask [_StencilWriteMaskGBuffer]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_GBUFFER
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile _ LIGHT_LAYERS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};


			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord5.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord5.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
						)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord5.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord5.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord5.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Albedo = appendResult175.xyz;
				surfaceDescription.Normal = float3( 0, 0, 1 );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 1.0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = 0.0;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				surfaceDescription.AlphaClipThresholdDepthPrepass = 0.5;
				surfaceDescription.AlphaClipThresholdDepthPostpass = 0.5;

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_DISTORTION
				surfaceDescription.Distortion = float2 ( 2, -1 );
				surfaceDescription.DistortionBlur = 1;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );
				ENCODE_INTO_GBUFFER( surfaceData, builtinData, posInput.positionSS, outGBuffer );
				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			CBUFFER_START(UnityMetaPass)
			bool4 unity_MetaVertexControl;
			bool4 unity_MetaFragmentControl;
			CBUFFER_END

			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				outputPackedVaryingsMeshToPS.ase_texcoord.xy = inputMesh.uv0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float2 uv = float2(0.0, 0.0);
				if (unity_MetaVertexControl.x)
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if (unity_MetaVertexControl.y)
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				outputPackedVaryingsMeshToPS.positionCS = float4(uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Albedo = appendResult175.xyz;
				surfaceDescription.Normal = float3( 0, 0, 1 );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 1.0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = 0.0;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
				LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

				float4 res = float4(0.0, 0.0, 0.0, 1.0);
				if (unity_MetaFragmentControl.x)
				{
					res.rgb = clamp(pow(abs(lightTransportData.diffuseColor), saturate(unity_OneOverOutputBoost)), 0, unity_MaxOutputValue);
				}

				if (unity_MetaFragmentControl.y)
				{
					res.rgb = lightTransportData.emissiveColor;
				}

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ZTest LEqual
			ColorMask 0

			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_SHADOWS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			//#define USE_LEGACY_UNITY_MATRIX_VARIABLES

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				#ifdef _ALPHATEST_SHADOW_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow );
				#else
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif
			
			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord1.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord1.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			ColorMask 0

			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS
			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			int _ObjectId;
			int _PassValue;

			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SceneSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SceneSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SceneSurfaceDescription surfaceDescription = (SceneSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord1.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord1.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local _ _DISABLE_DECALS

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH
			#pragma multi_compile _ WRITE_DECAL_BUFFER

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif
			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				return outputPackedVaryingsMeshToPS;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
				#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
				#define SV_TARGET_DECAL SV_Target1
			#else
				#define SV_TARGET_DECAL SV_Target0
			#endif
			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
						, out float4 outDecalBuffer : SV_TARGET_DECAL
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord3.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord3.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Normal = float3( 0, 0, 1 );
				surfaceDescription.Smoothness = 0.0;
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif

				#if defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)
				DecalPrepassData decalPrepassData;
				decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
				decalPrepassData.decalLayerMask = GetMeshRenderingDecalLayer();
				EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Motion Vectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_MOTION_VECTORS
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};


			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS =  inputMesh.normalOS ;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				outputPackedVaryingsMeshToPS.vmeshPositionCS = VMESHpositionCS;
				outputPackedVaryingsMeshToPS.vmeshInterp00.xyz = VMESHpositionRWS;

				outputPackedVaryingsMeshToPS.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				outputPackedVaryingsMeshToPS.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
				o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
				, out float4 outMotionVector : SV_Target0
				#ifdef WRITE_NORMAL_BUFFER
				, out float4 outNormalBuffer : SV_Target1
					#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target2
					#endif
				#elif defined(WRITE_MSAA_DEPTH)
				, out float4 outNormalBuffer : SV_Target1
				, out float1 depthColor : SV_Target2
				#endif

				#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
				#endif
				
				)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord3.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord3.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Normal = float3( 0, 0, 1 );
				surfaceDescription.Smoothness = 0.0;
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );

				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.vmeshPositionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.vmeshPositionCS.z;
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="Forward" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			ColorMask [_ColorMaskTransparentVel] 1

			HLSLPROGRAM

			#define _DISABLE_DECALS 1
			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 100202


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_FORWARD
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
			#pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			// CBuffer must be declared before Material.hlsl since it internaly uses _BlendMode now
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Threshold;
			float4 _ultimatte_ST;
			float4 _KeyColor1;
			float4 _CustomMask_ST;
			float4 _ThresholdL;
			float _cropBottom;
			float _cropTop;
			float _Gamma;
			float _tint;
			float _whiteBallance;
			float _despillLuma;
			float _GreenCastRemove;
			float _MaskPower;
			float _PortraitMode;
			float _TransitionPosition;
			float _cropLeft;
			float _KeyingAlgorithm;
			float _ColorCleanup;
			float _MateKey;
			float _despillChroma;
			float _Sharpen;
			float _PreCorrectGamma;
			float _Blurring;
			float _Border;
			float _TransitionFalloff;
			float _cropRight;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _ultimatte;
			sampler2D _CustomMask;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
			#define HAS_LIGHTLOOP
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 temperature_Deckard1_g87( float3 In, float Temperature, float Tint )
			{
				 float t1 = Temperature * 10 / 6;
				    float t2 = Tint * 10 / 6;
				    // Get the CIE xy chromaticity of the reference white point.
				    // Note: 0.31271 = x value on the D65 white point
				    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
				    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
				    float y = standardIlluminantY + t2 * 0.05;
				    // Calculate the coefficients in the LMS space.
				    float3 w1 = float3(0.949237, 1.03542, 1.08728); // D65 white point
				    // CIExyToLMS
				    float Y = 1;
				    float X = Y * x / y;
				    float Z = Y * (1 - x - y) / y;
				    float L = (0.7328 * X + 0.4296 * Y - 0.1624 * Z);
				    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
				    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
				    float3 w2 = float3(L, M, S);
				    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
				    float3x3 LIN_2_LMS_MAT = {
				        3.90405e-1, 5.49941e-1, 8.92632e-3,
				        7.08416e-2, 9.63172e-1, 1.35775e-3,
				        2.31082e-2, 1.28021e-1, 9.36245e-1
				    };
				    float3x3 LMS_2_LIN_MAT = {
				        2.85847e+0, -1.62879e+0, -2.48910e-2,
				        -2.10182e-1,  1.15820e+0,  3.24281e-4,
				        -4.18120e-2, -1.18169e-1,  1.06867e+0
				    };
				    float3 lms = mul(LIN_2_LMS_MAT, In);
				    lms *= balance;
				    return mul(LMS_2_LIN_MAT, lms);
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs.tangentToWorld[2], surfaceDescription.Alpha);
					ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = inputMesh.tangentOS;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = v.previousPositionOS;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = v.precomputedVelocity;
					#endif
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
					#endif
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(PackedVaryingsMeshToPS packedInput,
					#ifdef OUTPUT_SPLIT_LIGHTING
						out float4 outColor : SV_Target0,
						out float4 outDiffuseLighting : SV_Target1,
						OUTPUT_SSSBUFFER(outSSSBuffer)
					#else
						out float4 outColor : SV_Target0
					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						, out float4 outMotionVec : SV_Target1
					#endif
					#endif
					#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
					#endif
					
						)
			{
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outMotionVec = float4(2.0, 0.0, 0.0, 0.0);
				#endif

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				input.positionSS.xy = _OffScreenRendering > 0 ? (input.positionSS.xy * _OffScreenDownsampleFactor) : input.positionSS.xy;
				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_MainTex = packedInput.ase_texcoord7.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( uv_MainTex, 0, _Border) );
				float4 temp_cast_0 = (_PreCorrectGamma).xxxx;
				float4 temp_output_160_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ) , temp_cast_0 );
				float4 temp_cast_1 = (_PreCorrectGamma).xxxx;
				float4 temp_output_157_0 = pow( tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) ) , temp_cast_1 );
				float4 tex2DNode49 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_2 = (_PreCorrectGamma).xxxx;
				float4 temp_output_158_0 = pow( tex2DNode49 , temp_cast_2 );
				float4 tex2DNode62 = tex2Dbias( _MainTex, float4( ( uv_MainTex + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) );
				float4 temp_cast_3 = (_PreCorrectGamma).xxxx;
				float4 temp_output_159_0 = pow( tex2DNode62 , temp_cast_3 );
				float4 temp_output_104_0 = ( ( temp_output_160_0 + temp_output_157_0 + temp_output_158_0 + temp_output_159_0 ) * 0.25 );
				float3 temp_output_125_0 = ( (tex2DNode4).rgb + saturate( ( ( (tex2DNode4).rgb - (temp_output_104_0).rgb ) * _Sharpen ) ) );
				float4 result90 = float4( temp_output_125_0 , 0.0 );
				float2 uv_ultimatte = packedInput.ase_texcoord7.xy * _ultimatte_ST.xy + _ultimatte_ST.zw;
				float4 tex2DNode91 = tex2D( _ultimatte, uv_ultimatte );
				float4 temp_cast_5 = (_PreCorrectGamma).xxxx;
				float4 temp_output_156_0 = pow( tex2DNode91 , temp_cast_5 );
				float4 AValueClassic303 = temp_output_156_0;
				float4 temp_cast_6 = (_PreCorrectGamma).xxxx;
				float4 KeyColor356 = pow( _KeyColor1 , temp_cast_6 );
				float GarbageMate354 = (( _MateKey )?( 0.0 ):( 1.0 ));
				float4 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g28 = temp_output_156_0.rgb;
				float cleanup274 = _ColorCleanup;
				float temp_output_3_0_g28 = ( break7_g28.y - ( ( break7_g28.x + break7_g28.z ) * cleanup274 ) );
				float3 appendResult8_g28 = (float3(temp_output_3_0_g28 , temp_output_3_0_g28 , temp_output_3_0_g28));
				float3 AValue184 = appendResult8_g28;
				float4 lerpResult382 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float3 break7_g86 = temp_output_157_0.rgb;
				float temp_output_3_0_g86 = ( break7_g86.y - ( ( break7_g86.x + break7_g86.z ) * cleanup274 ) );
				float3 appendResult8_g86 = (float3(temp_output_3_0_g86 , temp_output_3_0_g86 , temp_output_3_0_g86));
				float KeyStyle302 = _KeyingAlgorithm;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , float4( appendResult8_g86 , 0.0 ) ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( _Threshold.x , _Threshold.y , lerpResult316);
				float4 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g84 = temp_output_158_0.rgb;
				float temp_output_3_0_g84 = ( break7_g84.y - ( ( break7_g84.x + break7_g84.z ) * cleanup274 ) );
				float3 appendResult8_g84 = (float3(temp_output_3_0_g84 , temp_output_3_0_g84 , temp_output_3_0_g84));
				float4 lerpResult379 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( float4( appendResult8_g84 , 0.0 ) , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( _Threshold.x , _Threshold.y , lerpResult311);
				float4 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g83 = temp_output_159_0.rgb;
				float temp_output_3_0_g83 = ( break7_g83.y - ( ( break7_g83.x + break7_g83.z ) * cleanup274 ) );
				float3 appendResult8_g83 = (float3(temp_output_3_0_g83 , temp_output_3_0_g83 , temp_output_3_0_g83));
				float4 lerpResult373 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( float4( appendResult8_g83 , 0.0 ) , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( _Threshold.x , _Threshold.y , lerpResult308);
				float4 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 break7_g85 = temp_output_160_0.rgb;
				float temp_output_3_0_g85 = ( break7_g85.y - ( ( break7_g85.x + break7_g85.z ) * cleanup274 ) );
				float3 appendResult8_g85 = (float3(temp_output_3_0_g85 , temp_output_3_0_g85 , temp_output_3_0_g85));
				float4 lerpResult367 = lerp( float4( AValue184 , 0.0 ) , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( float4( appendResult8_g85 , 0.0 ) , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( _Threshold.x , _Threshold.y , lerpResult300);
				float tresholdB324 = _ThresholdL.x;
				float smoothB325 = _ThresholdL.y;
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 texCoord337 = packedInput.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult340 = lerp( ( 1.0 - texCoord337.y ) , texCoord337.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float2 uv_CustomMask = packedInput.ase_texcoord7.xy * _CustomMask_ST.xy + _CustomMask_ST.zw;
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, uv_CustomMask ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_125_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 In1_g87 = localDespill90.xyz;
				float Temperature1_g87 = _whiteBallance;
				float Tint1_g87 = _tint;
				float3 localtemperature_Deckard1_g87 = temperature_Deckard1_g87( In1_g87 , Temperature1_g87 , Tint1_g87 );
				float3 temp_cast_22 = (( 1.0 / _Gamma )).xxx;
				float2 texCoord390 = packedInput.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( texCoord390.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( texCoord390.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( texCoord390.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( texCoord390.x , ( 1.0 - _cropRight ) ));
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g87 , temp_cast_22 ) , lerpResult404));
				
				surfaceDescription.Albedo = appendResult175.xyz;
				surfaceDescription.Normal = float3( 0, 0, 1 );
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 1.0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = 0.0;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = appendResult175.w;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);
				#ifdef DEBUG_DISPLAY
				#ifdef OUTPUT_SPLIT_LIGHTING
					outDiffuseLighting = 0;
					ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#endif

				bool viewMaterial = false;
				int bufferSize = int(_DebugViewMaterialArray[0].x);
				if (bufferSize != 0)
				{
					bool needLinearToSRGB = false;
					float3 result = float3(1.0, 0.0, 1.0);

					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index].x);

						if (indexMaterialProperty != 0)
						{
							viewMaterial = true;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);
						}
					}

					if (!needLinearToSRGB)
						result = SRGBToLinear(max(0, result));

					outColor = float4(result, 1.0);
				}

				if (!viewMaterial)
				{
					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
					{
						float3 result = float3(0.0, 0.0, 0.0);

						GetPBRValidatorDebug(surfaceData, result);

						outColor = float4(result, 1.0f);
					}
					else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
					else
				#endif
					{
				#ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
				#else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
				#endif
					
						LightLoopOutput lightLoopOutput;
						LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, lightLoopOutput);

						// Alias
						float3 diffuseLighting = lightLoopOutput.diffuseLighting;
						float3 specularLighting = lightLoopOutput.specularLighting;
					
						diffuseLighting *= GetCurrentExposureMultiplier();
						specularLighting *= GetCurrentExposureMultiplier();

				#ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = 0;
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
						outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
				#endif

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						float4 VPASSpositionCS = float4(packedInput.vpassPositionCS.xy, 0.0, packedInput.vpassPositionCS.z);
						float4 VPASSpreviousPositionCS = float4(packedInput.vpassPreviousPositionCS.xy, 0.0, packedInput.vpassPreviousPositionCS.z);

						bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
						if (!forceNoMotion)
						{
							float2 motionVec = CalculateMotionVector(VPASSpositionCS, VPASSpreviousPositionCS);
							EncodeMotionVector(motionVec * 0.5, outMotionVec);
							outMotionVec.zw = 1.0;
						}
				#endif
					}

				#ifdef DEBUG_DISPLAY
				}
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}
			ENDHLSL
		}
		
	}
	CustomEditor "Rendering.HighDefinition.LightingShaderGraphGUI"
	
	
}
/*ASEBEGIN
Version=18809
115;175;1408;559;-5054.069;320.2881;1.212953;True;True
Node;AmplifyShaderEditor.RangedFloatNode;273;-2212.319,-361.7162;Inherit;False;Property;_ColorCleanup;ColorCleanup;27;0;Create;True;0;0;0;True;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;48;-1672.862,496.3339;Inherit;False;Constant;_Vector0;Vector 0;17;0;Create;True;0;0;0;False;0;False;0,1;-0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;91;-1781.766,-643.9612;Inherit;True;Property;_ultimatte;ultimatte;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;57;-1768.219,1137.668;Inherit;False;Constant;_Vector2;Vector 2;20;0;Create;True;0;0;0;False;0;False;1,0;0,0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;107;-1731.932,1442.665;Inherit;False;Constant;_Vector3;Vector 3;19;0;Create;True;0;0;0;False;0;False;-1,0;0,-0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;3;-2339.316,-154.0838;Inherit;True;Property;_MainTex;_MainTex;9;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;56;-2191.087,572.2889;Inherit;False;Property;_Border;Border;11;0;Create;True;0;0;0;False;0;False;0;0;-0.01;0.003;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;274;-1847.317,-306.1604;Inherit;False;cleanup;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-1756.371,817.3896;Inherit;False;Constant;_Vector1;Vector 1;18;0;Create;True;0;0;0;False;0;False;0,-1;0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;30;-1383.26,-224.2925;Inherit;False;Property;_PreCorrectGamma;PreCorrectGamma;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1436.773,532.847;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1779.854,136.0301;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1507.172,824.047;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;275;-1159.782,-128.7044;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;178;-1096.444,-649.985;Inherit;False;Property;_KeyColor1;KeyColor1;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;156;-1129.275,-387.0135;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1519.021,1144.325;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-1482.734,1449.322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;196;1102.976,-259.0241;Inherit;False;Property;_MateKey;MateKey;0;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1313.593,1026.957;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1301.745,706.6785;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1316.236,397.4229;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;291;-980.1541,-245.8135;Inherit;False;PreKey;-1;;28;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;405;-791.5744,-561.9812;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-1277.306,1331.954;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-1979.732,1012.17;Inherit;False;Property;_Blurring;Blurring;23;1;[HideInInspector];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;356;-611.8285,-666.4588;Inherit;False;KeyColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;49;-1175.213,675.2783;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1427.174,269.1166;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;354;1367.384,-244.4448;Inherit;False;GarbageMate;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-1126.499,1013.025;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;110;-1090.211,1318.022;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-752.8597,-173.5317;Inherit;False;AValue;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;303;-892.1074,-365.5029;Inherit;False;AValueClassic;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;-638.8922,367.509;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;-761.8969,1604.574;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;355;-1037.803,437.5284;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;380;-207.8155,822.6926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;381;-299.2465,906.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;160;-707.1854,1331.733;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;371;-603.8155,1028.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;372;-695.2465,1112.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;277;-747.9768,937.8534;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-837.6063,616.2765;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;377;-547.8155,854.6926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;-627.1133,1528.396;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;366;-745.744,1476.111;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;369;-702.2465,1809.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;-508.4467,728.4281;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;384;-856.2465,319.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;313;-531.3795,837.9017;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-516.5307,1170.185;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;306;-573.2311,1096.265;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-553.6569,1602.042;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-1237.635,1666.484;Inherit;False;Property;_KeyingAlgorithm;Keying Algorithm;28;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;365;-654.313,1392.103;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;278;-734.1923,1179.078;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;159;-768.6585,1081.899;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;158;-735.0719,753.1746;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;383;-764.8155,235.6926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;368;-610.8155,1725.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;378;-639.2465,938.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;-773.8223,548.4479;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;375;-625.2465,1289.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-908.3777,230.6901;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;374;-533.8155,1205.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;357;-1045.227,360.5497;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;379;23.13931,828.7266;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;297;-544.8914,982.6791;Inherit;False;PreKey;-1;;83;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;323;-903.3515,-92.11494;Inherit;False;Property;_ThresholdL;ThresholdL;5;0;Create;True;0;0;0;False;1;RemapSlidersFull1;False;-1,1,-1,1;-1,1,-1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;350;-781.4199,334.5551;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;364;-423.3582,1398.137;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;298;-540.85,624.4094;Inherit;False;PreKey;-1;;84;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;370;-372.8607,1034.727;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;294;-659.1216,454.1874;Inherit;False;PreKey;-1;;86;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;376;-316.8607,860.7266;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;382;-533.8607,241.7266;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;293;-534.3211,1250.645;Inherit;False;PreKey;-1;;85;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;367;-379.8607,1731.727;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;302;-936.5239,1655.347;Inherit;False;KeyStyle;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;373;-302.8607,1211.727;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;286;-730.8666,81.35452;Inherit;False;Property;_Threshold;Threshold;4;0;Create;True;0;0;0;False;1;RemapSlidersFull1;False;-1,1,-1,1;-1,1,-1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;314;-437.1522,505.0518;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;312;-281.9985,553.6985;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;310;-259.8358,753.5383;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;299;-194.8689,1297.984;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;324;-609.524,-82.22565;Inherit;False;tresholdB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;150;-391.5171,386.3198;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;111;-249.6569,1457.05;Inherit;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;309;-280.5964,1148.992;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;315;-380.8606,290.3941;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;50;-236.1712,643.1748;Inherit;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;325;-586.1021,-16.49899;Inherit;False;smoothB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;304;-292.6423,1623.13;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;60;-238.1814,1030.196;Inherit;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;307;-292.2138,934.6524;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;337;619.6178,662.4182;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;327;1089.135,1316.851;Inherit;False;324;tresholdB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;346;772.827,1019.616;Inherit;False;Property;_PortraitMode;PortraitMode;39;1;[Toggle];Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;316;-172.5787,428.1458;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;287;-349.0204,198.8042;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;349;828.4053,771.5069;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;311;-51.12589,653.5139;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;328;1075.665,1434.632;Inherit;False;325;smoothB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;300;-19.43263,1369.226;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;343;538.499,503.7491;Inherit;False;Property;_TransitionPosition;TransitionPosition;30;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;308;-80.38658,1041.268;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;288;-58.15187,135.198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;344;633.499,582.7491;Inherit;False;Property;_TransitionFalloff;TransitionFalloff;31;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;330;1391.916,1040.334;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;331;1392.231,898.4204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;51;283.7787,670.0588;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;320;1378.507,1509.939;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;151;-6.504723,452.7724;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;326;1367.511,1247.767;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;345;957.499,538.7491;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;359.4202,1270.184;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;61;125.5221,1056.977;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;340;1006.899,694.1491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;332;1706.548,982.3327;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;258;793.4918,371.8522;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;342;1090.499,486.7491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;1270.534,530.389;Inherit;False;Constant;_Float5;Float 5;26;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;335;1061.218,358.9373;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;1257.596,359.2685;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;1046.917,61.11723;Inherit;False;Property;_MaskPower;MaskPower;12;0;Create;True;0;0;0;False;0;False;0;9.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;262;980.7156,204.7556;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;1379.882,28.65171;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;437.587,964.9861;Inherit;False;Constant;_Float7;Float 7;26;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;124;391.9704,799.6143;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-1414.947,35.68357;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;224;1699.846,122.9193;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;138;3074.074,-198.7457;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;721.8488,864.4696;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;127;1976.728,100.5313;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;347;4036.394,547.0471;Inherit;True;Property;_CustomMask;CustomMask;33;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;338;3763.509,67.34172;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;223;1187.981,770.748;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;391;4536.238,429.1982;Inherit;False;Property;_cropTop;cropTop;35;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;348;4395.539,412.0651;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;1819.426,562.803;Inherit;False;Property;_Sharpen;Sharpen;18;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;126;1831.896,432.4939;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;2090.992,366.5024;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;385;2908.941,417.1809;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;390;4630.616,580.1221;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;406;4804.75,491.0521;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;2432.919,365.79;Inherit;False;Property;_despillChroma;despillChroma;13;0;Create;True;0;0;0;False;0;False;0;0.67;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;137;2274.255,391.2306;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;2625,455.9586;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;393;4984.288,526.4566;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;2398.288,578.8126;Inherit;False;Property;_despillLuma;despillLuma;14;0;Create;True;0;0;0;False;0;False;0;-1.83;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;396;5091.591,626.5516;Inherit;False;Property;_cropBottom;cropBottom;36;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;388;2493.033,-2.046274;Inherit;False;Property;_GreenCastRemove;GreenCastRemove;34;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;394;4659.839,248.0947;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;125;2372.783,231.8221;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;387;2766.284,151.657;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;397;5449.572,691.1716;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;400;5355.205,491.3647;Inherit;False;Property;_cropLeft;cropLeft;37;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;2735.213,590.3167;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;5474.065,385.0587;Inherit;False;Property;_cropRight;cropRight;38;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;264;3223.756,621.3891;Inherit;False;Property;_tint;tint;17;0;Create;True;1;Color Correction;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;407;5747.159,399.2397;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;3211.866,477.2195;Inherit;False;Property;_whiteBallance;whiteBallance;16;0;Create;True;0;0;0;False;0;False;1;0.469;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;3664.604,598.72;Inherit;False;Property;_Gamma;Gamma;15;1;[Header];Create;True;1;Color Correction;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;398;5125.123,412.8097;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;401;5702.125,524.646;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;90;2951.216,152.7343;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;265;3892.361,539.9419;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;399;5377.677,246.2843;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;403;5934.397,418.34;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;101;3546.617,247.3264;Inherit;False;ColorTemperature;6;;87;fccce2e41bca18a41863dccc23edb3ef;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;404;5609.949,139.9784;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;263;3992.126,364.1742;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;175;4692.845,-12.9416;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;420;5313.297,63.85392;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;339;3369.32,-475.3234;Inherit;True;Property;_noise_01;noise_01;29;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;227;-4.049268,-273.5655;Inherit;False;195;smoothA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;193;-325.8605,77.4147;Inherit;False;tresholdA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;362;-1117.851,1182.44;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TexelSizeNode;133;-2048.4,227.0914;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;361;-885.8984,1162.474;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceOpNode;213;490.8993,-571.3878;Inherit;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;363;-1105.282,1260.448;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;250;4681.453,-421.4633;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;188;-508.0722,-223.4149;Inherit;False;186;BValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;292;286.4067,425.3245;Inherit;False;PreKey;-1;;91;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;421;5870.386,-49.79963;Inherit;False;Constant;_Float0;Float 0;38;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;215;-546.6964,-431.7114;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;249;4536.024,-744.239;Inherit;True;Property;_Texture0;Texture 0;24;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;180;39.93354,-531.6168;Inherit;False;Property;_KeyColor3;KeyColor3;20;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4325296,0.8018868,0.4122908,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;194;-56.48098,-106.0002;Inherit;False;193;tresholdA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;209;-414.1124,-893.2585;Inherit;False;186;BValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;214;653.9028,-287.0926;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;253;5049.697,-343.6343;Inherit;False;Property;_Size;Size;32;0;Create;True;0;0;0;False;0;False;0;0;0;25.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;358;-877.924,799.6523;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;295;-408.0111,-338.7799;Inherit;False;PreKey;-1;;90;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;248;5536.051,-343.3506;Inherit;False;float Pi = 6.28318530718@ $    $$   $    float2 Radius = Size/iResolution.xy@$    $$    float2 UW@$UW = UV0/iResolution.xy@$   // float4 Col@$//Col = tex2D(tex, UW)@$    $    // Blur calculations$    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions - 15.0@$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.RegisterLocalVarNode;186;524.8491,378.485;Inherit;False;BValue;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;501.9122,291.8068;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;359;-1109.876,819.6179;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;183;758.3942,-473.175;Inherit;False;Property;_KeyColor6;KeyColor6;22;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;296;-226.3193,-702.5984;Inherit;False;PreKey;-1;;89;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;10;253.6099,-76.03759;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;211;532.4146,-96.04487;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;168;4490.38,1267.329;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;218;-583.4926,-557.0444;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;-32.00142,-36.57859;Inherit;False;195;smoothA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;220;-1385.53,-365.7618;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;282;133.9613,526.7722;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;319;3699.868,-186.9825;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;225;99.37017,370.6135;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;321;732.5998,1251.963;Inherit;False;324;tresholdB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;251;5120.483,-498.5719;Inherit;False;Property;_Directions;Directions;25;1;[IntRange];Create;True;0;0;0;False;0;False;0;44;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;219;-700.9157,-827.6005;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;360;-1097.307,897.6257;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;419;5901.652,27.24877;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;490.2103,184.284;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;284;1639.452,266.4608;Inherit;False;Constant;_Vector4;Vector 4;32;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;9;-184.8234,-296.2873;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;216;-415.6355,-736.0399;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;182;531.7302,-475.9702;Inherit;False;Property;_KeyColor5;KeyColor5;19;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;228;269.8509,-298.838;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;181;308.2706,-476.5816;Inherit;False;Property;_KeyColor4;KeyColor4;21;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;179;-970.3537,-844.4421;Inherit;False;Property;_KeyColor2;KeyColor2;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.472818,0.9245283,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;252;5082.483,-420.5719;Inherit;False;Property;_Quality;Quality;26;1;[IntRange];Create;True;0;0;0;False;0;False;0;9;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;177.2981,-622.1362;Inherit;False;186;BValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;226;-112.7684,-368.4001;Inherit;False;193;tresholdA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;210;50.45796,-715.1151;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;422;6032.92,86.05105;Inherit;False;Constant;_Float1;Float 0;38;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;92.09451,124.9227;Inherit;False;smoothA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;746.6285,1367.332;Inherit;False;325;smoothB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;411;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;413;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Motion Vectors;0;5;Motion Vectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-9;255;False;-1;255;True;-10;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=MotionVectors;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;409;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;META;0;1;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;408;6222.325,-121.5633;Float;False;True;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;ChromaLiveRTMaterial;53b46d85872c5b24c8f4f0a1c3fe4c87;True;GBuffer;0;0;GBuffer;35;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-14;255;False;-1;255;True;-13;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;True;0;True;-15;False;True;1;LightMode=GBuffer;False;0;;0;0;Standard;41;Surface Type;0;  Rendering Pass;1;  Refraction Model;0;    Blending Mode;0;    Blend Preserves Specular;1;  Receive Fog;1;  Back Then Front Rendering;0;  Transparent Depth Prepass;0;  Transparent Depth Postpass;0;  Transparent Writes Motion Vector;0;  Distortion;0;    Distortion Mode;0;    Distortion Depth Test;1;  ZWrite;0;  Z Test;4;Double-Sided;0;Alpha Clipping;1;  Use Shadow Threshold;0;Material Type,InvertActionOnDeselection;0;  Energy Conserving Specular;1;  Transmission;1;Receive Decals;0;Receives SSR;1;Motion Vectors;1;  Add Precomputed Velocity;0;Specular AA;0;Specular Occlusion Mode;1;Override Baked GI;0;Depth Offset;0;DOTS Instancing;0;LOD CrossFade;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position;1;0;11;True;True;True;True;True;True;False;False;False;False;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;412;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;DepthOnly;0;4;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;False;False;False;False;False;False;False;False;True;True;0;True;-7;255;False;-1;255;True;-8;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;415;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentBackface;0;7;TransparentBackface;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;True;1;0;True;-20;0;True;-21;1;0;True;-22;0;True;-23;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;False;True;True;True;True;True;0;True;-45;False;False;False;False;False;False;False;True;0;True;-24;True;0;True;-32;False;True;1;LightMode=TransparentBackface;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;414;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Distortion;0;6;Distortion;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;1;False;-1;1;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;0;True;-11;255;False;-1;255;True;-12;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;1;LightMode=DistortionVectors;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;410;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;418;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Forward;0;10;Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;True;1;0;True;-20;0;True;-21;1;0;True;-22;0;True;-23;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-29;False;False;False;True;True;True;True;True;0;True;-45;False;False;False;False;False;True;True;0;True;-5;255;False;-1;255;True;-6;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;0;True;-24;True;0;True;-31;False;True;1;LightMode=Forward;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;417;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPostpass;0;9;TransparentDepthPostpass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPostpass;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;416;6222.325,-121.5633;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;14;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPrepass;0;8;TransparentDepthPrepass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;-26;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPrepass;False;0;;0;0;Standard;0;False;0
WireConnection;274;0;273;0
WireConnection;54;0;48;0
WireConnection;54;1;56;0
WireConnection;44;2;3;0
WireConnection;55;0;53;0
WireConnection;55;1;56;0
WireConnection;156;0;91;0
WireConnection;156;1;30;0
WireConnection;58;0;57;0
WireConnection;58;1;56;0
WireConnection;108;0;107;0
WireConnection;108;1;56;0
WireConnection;59;0;44;0
WireConnection;59;1;58;0
WireConnection;52;0;44;0
WireConnection;52;1;55;0
WireConnection;42;0;44;0
WireConnection;42;1;54;0
WireConnection;291;1;156;0
WireConnection;291;2;275;0
WireConnection;405;0;178;0
WireConnection;405;1;30;0
WireConnection;109;0;44;0
WireConnection;109;1;108;0
WireConnection;356;0;405;0
WireConnection;49;0;3;0
WireConnection;49;1;52;0
WireConnection;49;2;205;0
WireConnection;41;0;3;0
WireConnection;41;1;42;0
WireConnection;41;2;205;0
WireConnection;354;0;196;0
WireConnection;62;0;3;0
WireConnection;62;1;59;0
WireConnection;62;2;205;0
WireConnection;110;0;3;0
WireConnection;110;1;109;0
WireConnection;110;2;205;0
WireConnection;184;0;291;0
WireConnection;303;0;156;0
WireConnection;160;0;110;0
WireConnection;160;1;30;0
WireConnection;157;0;41;0
WireConnection;157;1;30;0
WireConnection;159;0;62;0
WireConnection;159;1;30;0
WireConnection;158;0;49;0
WireConnection;158;1;30;0
WireConnection;379;0;190;0
WireConnection;379;1;380;0
WireConnection;379;2;381;0
WireConnection;297;1;159;0
WireConnection;297;2;278;0
WireConnection;350;0;317;0
WireConnection;350;1;357;0
WireConnection;350;2;355;0
WireConnection;364;0;305;0
WireConnection;364;1;365;0
WireConnection;364;2;366;0
WireConnection;298;1;158;0
WireConnection;298;2;277;0
WireConnection;370;0;306;0
WireConnection;370;1;371;0
WireConnection;370;2;372;0
WireConnection;294;1;157;0
WireConnection;294;2;276;0
WireConnection;376;0;313;0
WireConnection;376;1;377;0
WireConnection;376;2;378;0
WireConnection;382;0;189;0
WireConnection;382;1;383;0
WireConnection;382;2;384;0
WireConnection;293;1;160;0
WireConnection;293;2;279;0
WireConnection;367;0;192;0
WireConnection;367;1;368;0
WireConnection;367;2;369;0
WireConnection;302;0;301;0
WireConnection;373;0;191;0
WireConnection;373;1;374;0
WireConnection;373;2;375;0
WireConnection;312;0;158;0
WireConnection;312;1;376;0
WireConnection;299;0;160;0
WireConnection;299;1;364;0
WireConnection;324;0;323;1
WireConnection;150;0;382;0
WireConnection;150;1;294;0
WireConnection;111;0;293;0
WireConnection;111;1;367;0
WireConnection;315;0;157;0
WireConnection;315;1;350;0
WireConnection;50;0;298;0
WireConnection;50;1;379;0
WireConnection;325;0;323;2
WireConnection;60;0;297;0
WireConnection;60;1;373;0
WireConnection;307;0;159;0
WireConnection;307;1;370;0
WireConnection;316;0;315;0
WireConnection;316;1;150;0
WireConnection;316;2;314;0
WireConnection;287;0;286;1
WireConnection;349;0;337;2
WireConnection;311;0;312;0
WireConnection;311;1;50;0
WireConnection;311;2;310;0
WireConnection;300;0;299;0
WireConnection;300;1;111;0
WireConnection;300;2;304;0
WireConnection;308;0;307;0
WireConnection;308;1;60;0
WireConnection;308;2;309;0
WireConnection;288;0;286;2
WireConnection;330;0;311;0
WireConnection;330;1;327;0
WireConnection;330;2;328;0
WireConnection;331;0;316;0
WireConnection;331;1;327;0
WireConnection;331;2;328;0
WireConnection;51;0;311;0
WireConnection;51;1;287;0
WireConnection;51;2;288;0
WireConnection;320;0;300;0
WireConnection;320;1;327;0
WireConnection;320;2;328;0
WireConnection;151;0;316;0
WireConnection;151;1;287;0
WireConnection;151;2;288;0
WireConnection;326;0;308;0
WireConnection;326;1;327;0
WireConnection;326;2;328;0
WireConnection;345;0;343;0
WireConnection;345;1;344;0
WireConnection;112;0;300;0
WireConnection;112;1;287;0
WireConnection;112;2;288;0
WireConnection;61;0;308;0
WireConnection;61;1;287;0
WireConnection;61;2;288;0
WireConnection;340;0;349;0
WireConnection;340;1;337;1
WireConnection;340;2;346;0
WireConnection;332;0;331;0
WireConnection;332;1;330;0
WireConnection;332;2;326;0
WireConnection;332;3;320;0
WireConnection;258;0;151;0
WireConnection;258;1;51;0
WireConnection;258;2;61;0
WireConnection;258;3;112;0
WireConnection;342;0;340;0
WireConnection;342;1;343;0
WireConnection;342;2;345;0
WireConnection;335;0;258;0
WireConnection;335;1;332;0
WireConnection;335;2;342;0
WireConnection;259;0;335;0
WireConnection;259;1;260;0
WireConnection;262;0;259;0
WireConnection;262;1;30;0
WireConnection;173;0;262;0
WireConnection;173;1;76;0
WireConnection;124;0;160;0
WireConnection;124;1;157;0
WireConnection;124;2;158;0
WireConnection;124;3;159;0
WireConnection;4;0;3;0
WireConnection;4;1;44;0
WireConnection;4;2;56;0
WireConnection;224;0;4;0
WireConnection;138;0;173;0
WireConnection;104;0;124;0
WireConnection;104;1;114;0
WireConnection;127;0;224;0
WireConnection;338;1;138;0
WireConnection;338;2;4;4
WireConnection;223;0;104;0
WireConnection;348;1;338;0
WireConnection;348;2;347;1
WireConnection;126;0;127;0
WireConnection;126;1;223;0
WireConnection;132;0;126;0
WireConnection;132;1;120;0
WireConnection;385;0;348;0
WireConnection;406;0;391;0
WireConnection;137;0;132;0
WireConnection;389;0;88;0
WireConnection;389;1;385;0
WireConnection;393;0;390;2
WireConnection;393;1;406;0
WireConnection;394;1;348;0
WireConnection;394;2;393;0
WireConnection;125;0;127;0
WireConnection;125;1;137;0
WireConnection;387;0;389;0
WireConnection;387;1;388;0
WireConnection;397;0;390;2
WireConnection;397;1;396;0
WireConnection;386;0;385;0
WireConnection;386;1;161;0
WireConnection;407;0;402;0
WireConnection;398;0;394;0
WireConnection;398;2;397;0
WireConnection;401;0;390;1
WireConnection;401;1;400;0
WireConnection;90;0;125;0
WireConnection;90;1;387;0
WireConnection;90;2;386;0
WireConnection;90;3;125;0
WireConnection;265;1;163;0
WireConnection;399;0;398;0
WireConnection;399;2;401;0
WireConnection;403;0;390;1
WireConnection;403;1;407;0
WireConnection;101;2;90;0
WireConnection;101;5;103;0
WireConnection;101;6;264;0
WireConnection;404;1;399;0
WireConnection;404;2;403;0
WireConnection;263;0;101;0
WireConnection;263;1;265;0
WireConnection;175;0;263;0
WireConnection;175;3;404;0
WireConnection;420;0;175;0
WireConnection;193;0;287;0
WireConnection;133;0;3;0
WireConnection;361;0;62;0
WireConnection;361;1;362;0
WireConnection;361;2;363;0
WireConnection;213;0;212;0
WireConnection;213;1;180;0
WireConnection;292;1;225;0
WireConnection;292;2;282;0
WireConnection;215;0;218;0
WireConnection;215;1;30;0
WireConnection;214;1;213;0
WireConnection;214;2;211;0
WireConnection;358;0;49;0
WireConnection;358;1;359;0
WireConnection;358;2;360;0
WireConnection;295;1;215;0
WireConnection;248;0;3;0
WireConnection;248;1;251;0
WireConnection;248;2;252;0
WireConnection;248;3;253;0
WireConnection;248;5;250;0
WireConnection;186;0;292;0
WireConnection;296;1;216;0
WireConnection;10;0;9;0
WireConnection;10;1;194;0
WireConnection;10;2;197;0
WireConnection;211;1;228;0
WireConnection;211;2;10;0
WireConnection;218;0;178;0
WireConnection;220;0;91;0
WireConnection;319;0;248;7
WireConnection;319;3;248;7
WireConnection;225;0;104;0
WireConnection;219;0;179;0
WireConnection;419;0;420;0
WireConnection;9;0;188;0
WireConnection;9;1;295;0
WireConnection;216;0;219;0
WireConnection;216;1;30;0
WireConnection;228;0;210;0
WireConnection;228;1;226;0
WireConnection;228;2;227;0
WireConnection;210;0;209;0
WireConnection;210;1;296;0
WireConnection;195;0;288;0
WireConnection;408;0;175;0
WireConnection;408;4;421;0
WireConnection;408;7;422;0
WireConnection;408;9;419;3
ASEEND*/
//CHKSM=EFB386C67B401723CE4455F74BD5D99A928E0511