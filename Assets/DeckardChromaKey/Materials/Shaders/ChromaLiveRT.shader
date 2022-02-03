// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ChromaLiveRT"
{
	Properties
	{
		[Toggle]_MatteKey("Matte Key", Float) = 0
		_KeyColor("KeyColor", Color) = (0,0,0,0)
		_garbageMatte("garbageMatte", 2D) = "white" {}
		_PreCorrectGamma("PreCorrectGamma", Range( 0 , 1)) = 0
		[RemapSlidersFull1]_Threshold("Threshold", Vector) = (-1,1,-1,1)
		[RemapSlidersFull1]_ThresholdL("ThresholdL", Vector) = (-1,1,-1,1)
		[RemapSlidersFull1]_SoftTransition("SoftTransition", Vector) = (0,0.65,0,1)
		[HideInInspector]_MainTex("_MainTex", 2D) = "white" {}
		_MaskPower("MaskPower", Float) = 0
		_despillChroma("despillChroma", Range( 0 , 2)) = 0
		_despillLuma("despillLuma", Range( -1 , 5)) = 0
		_DespillComposite("Composite Despill", Float) = 0
		_DespillLumaComposite("Composite Luma Despill", Float) = 0
		_GreenCastRemove("GreenCastRemove", Range( 0 , 1)) = 0
		[Header(REFINEMENT)]_Border("Border Defocus", Range( 0 , 0.003)) = 0
		_Cleanup("Cleanup Mask", Range( 0 , 0.5)) = 0
		_Range("Cleanup Range", Range( 0 , 0.5)) = 0
		[Header(COLOR CORRECTION)]_Gamma("Gamma", Float) = 1
		_whiteBallance("whiteBallance", Range( -1 , 1)) = 1
		_tint("tint", Range( -1 , 1)) = 1
		[HideInInspector]_Blurring("Blurring", Float) = 0
		_TransitionPosition("TransitionPosition", Range( 0 , 1)) = 0.5
		_TransitionFalloff("TransitionFalloff", Range( 0 , 1)) = 0.5
		[Toggle]_PortraitMode("PortraitMode", Range( 0 , 1)) = 0
		[Header(RELIGHTING GENERATION)][IntRange]_iterations("Iterations", Range( 1 , 16)) = 0
		_ColorInfluence("Color Influence", Range( 0 , 1)) = 0.5
		_NormalScale("_NormalScale", Range( 0 , 5)) = 0
		[Header(MATERIAL SETTINGS)]_BaseColor("_BaseColor", Color) = (0,0,0,0)
		[HDR]_EmissiveColor("_EmissiveColor", Color) = (0,0,0,0)
		_Smoothness("_Smoothness", Range( 0 , 1)) = 0
		_Metallic("_Metallic", Range( 0 , 1)) = 1
		[Toggle]_ScreenSpaceProjection("ScreenSpaceProjection", Float) = 0
		[Header(CROPPING)]_cropTop("cropTop", Range( 0 , 1)) = 1
		_cropBottom("cropBottom", Range( 0 , 1)) = 0
		_cropLeft("cropLeft", Range( 0 , 1)) = 0
		_cropRight("cropRight", Range( 0 , 1)) = 0
		_CustomMask("CustomMask", 2D) = "white" {}
		_InputTexture("InputTexture", 2D) = "black" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Metallic;
			uniform float _DespillLumaComposite;
			uniform float4 _EmissiveColor;
			uniform float _Range;
			uniform float4 _BaseColor;
			uniform float _Cleanup;
			uniform sampler2D _InputTexture;
			uniform float _Smoothness;
			uniform float _ScreenSpaceProjection;
			uniform float _NormalScale;
			uniform float _DespillComposite;
			uniform float _iterations;
			uniform float _ColorInfluence;
			uniform sampler2D _MainTex;
			uniform sampler2D _garbageMatte;
			uniform float4 _garbageMatte_ST;
			uniform float _Border;
			uniform float4 _KeyColor;
			uniform float _despillChroma;
			uniform float4 _Threshold;
			uniform float _PreCorrectGamma;
			uniform float _Blurring;
			uniform float _MatteKey;
			uniform float4 _ThresholdL;
			uniform float _TransitionPosition;
			uniform float _TransitionFalloff;
			uniform float _PortraitMode;
			uniform float _MaskPower;
			uniform sampler2D _CustomMask;
			uniform float _GreenCastRemove;
			uniform float _despillLuma;
			uniform float _whiteBallance;
			uniform float _tint;
			uniform float _Gamma;
			uniform float4 _SoftTransition;
			uniform float _cropTop;
			uniform float _cropBottom;
			uniform float _cropLeft;
			uniform float _cropRight;
			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			float3 temperature_Deckard1_g298( float3 In, float Temperature, float Tint )
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
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_garbageMatte = i.ase_texcoord1.xy * _garbageMatte_ST.xy + _garbageMatte_ST.zw;
				float2 appendResult453 = (float2(1.0 , 1.0));
				float2 temp_output_454_0 = ( uv_garbageMatte * appendResult453 );
				float4 tex2DNode4 = tex2Dbias( _MainTex, float4( temp_output_454_0, 0, _Border) );
				float3 hsvTorgb2_g296 = RGBToHSV( (tex2DNode4).rgb );
				float3 temp_output_218_0 = (_KeyColor).rgb;
				float3 hsvTorgb586 = RGBToHSV( temp_output_218_0 );
				float shift569 = ( 0.333333 - hsvTorgb586.x );
				float3 hsvTorgb3_g296 = HSVToRGB( float3(( hsvTorgb2_g296.x + shift569 ),hsvTorgb2_g296.y,hsvTorgb2_g296.z) );
				float3 temp_output_572_0 = hsvTorgb3_g296;
				float4 result90 = float4( temp_output_572_0 , 0.0 );
				float PreCorrectGamma474 = _PreCorrectGamma;
				float3 hsvTorgb2_g289 = RGBToHSV( (tex2Dbias( _MainTex, float4( ( temp_output_454_0 + ( float2( 0,1 ) * _Border ) ), 0, _Blurring) )).rgb );
				float3 hsvTorgb3_g289 = HSVToRGB( float3(( hsvTorgb2_g289.x + shift569 ),hsvTorgb2_g289.y,hsvTorgb2_g289.z) );
				float3 temp_cast_1 = (_PreCorrectGamma).xxx;
				float3 temp_output_157_0 = pow( hsvTorgb3_g289 , temp_cast_1 );
				float3 temp_cast_2 = (_PreCorrectGamma).xxx;
				float3 temp_output_1_0_g267 = pow( (tex2D( _garbageMatte, temp_output_454_0 )).rgb , temp_cast_2 );
				float3 break7_g267 = temp_output_1_0_g267;
				float cleanup274 = 0.5;
				float temp_output_2_0_g267 = cleanup274;
				float temp_output_3_0_g267 = ( break7_g267.y - ( ( break7_g267.x + break7_g267.z ) * temp_output_2_0_g267 ) );
				float3 appendResult8_g267 = (float3(temp_output_3_0_g267 , temp_output_3_0_g267 , temp_output_3_0_g267));
				float3 temp_output_556_0 = appendResult8_g267;
				float3 AValueClassic303 = temp_output_556_0;
				float3 hsvTorgb2_g266 = RGBToHSV( temp_output_218_0 );
				float3 hsvTorgb3_g266 = HSVToRGB( float3(( hsvTorgb2_g266.x + shift569 ),hsvTorgb2_g266.y,hsvTorgb2_g266.z) );
				float3 temp_cast_3 = (_PreCorrectGamma).xxx;
				float3 temp_output_405_0 = pow( hsvTorgb3_g266 , temp_cast_3 );
				float3 KeyColor356 = temp_output_405_0;
				float GarbageMate354 = (( _MatteKey )?( 0.0 ):( 1.0 ));
				float3 lerpResult350 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 AValue184 = temp_output_556_0;
				float3 lerpResult382 = lerp( AValue184 , KeyColor356 , GarbageMate354);
				float3 temp_output_1_0_g293 = temp_output_157_0;
				float3 break7_g293 = temp_output_1_0_g293;
				float temp_output_2_0_g293 = cleanup274;
				float temp_output_3_0_g293 = ( break7_g293.y - ( ( break7_g293.x + break7_g293.z ) * temp_output_2_0_g293 ) );
				float3 appendResult8_g293 = (float3(temp_output_3_0_g293 , temp_output_3_0_g293 , temp_output_3_0_g293));
				float KeyStyle302 = 0.5;
				float lerpResult316 = lerp( distance( temp_output_157_0 , lerpResult350 ) , distance( lerpResult382 , appendResult8_g293 ) , KeyStyle302);
				float smoothstepResult151 = smoothstep( pow( _Threshold.x , PreCorrectGamma474 ) , pow( _Threshold.y , PreCorrectGamma474 ) , lerpResult316);
				float3 hsvTorgb2_g290 = RGBToHSV( (tex2Dbias( _MainTex, float4( ( temp_output_454_0 + ( float2( 0,-1 ) * _Border ) ), 0, _Blurring) )).rgb );
				float3 hsvTorgb3_g290 = HSVToRGB( float3(( hsvTorgb2_g290.x + shift569 ),hsvTorgb2_g290.y,hsvTorgb2_g290.z) );
				float3 temp_output_577_0 = hsvTorgb3_g290;
				float3 temp_cast_4 = (_PreCorrectGamma).xxx;
				float3 temp_output_158_0 = pow( temp_output_577_0 , temp_cast_4 );
				float3 lerpResult376 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 temp_output_1_0_g292 = temp_output_158_0;
				float3 break7_g292 = temp_output_1_0_g292;
				float temp_output_2_0_g292 = cleanup274;
				float temp_output_3_0_g292 = ( break7_g292.y - ( ( break7_g292.x + break7_g292.z ) * temp_output_2_0_g292 ) );
				float3 appendResult8_g292 = (float3(temp_output_3_0_g292 , temp_output_3_0_g292 , temp_output_3_0_g292));
				float3 lerpResult379 = lerp( AValue184 , KeyColor356 , GarbageMate354);
				float lerpResult311 = lerp( distance( temp_output_158_0 , lerpResult376 ) , distance( appendResult8_g292 , lerpResult379 ) , KeyStyle302);
				float smoothstepResult51 = smoothstep( pow( _Threshold.x , PreCorrectGamma474 ) , pow( _Threshold.y , PreCorrectGamma474 ) , lerpResult311);
				float3 hsvTorgb2_g288 = RGBToHSV( (tex2Dbias( _MainTex, float4( ( uv_garbageMatte + ( float2( 1,0 ) * _Border ) ), 0, _Blurring) )).rgb );
				float3 hsvTorgb3_g288 = HSVToRGB( float3(( hsvTorgb2_g288.x + shift569 ),hsvTorgb2_g288.y,hsvTorgb2_g288.z) );
				float3 temp_output_576_0 = hsvTorgb3_g288;
				float3 temp_cast_5 = (_PreCorrectGamma).xxx;
				float3 temp_output_159_0 = pow( temp_output_576_0 , temp_cast_5 );
				float3 lerpResult370 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 temp_output_1_0_g294 = temp_output_159_0;
				float3 break7_g294 = temp_output_1_0_g294;
				float temp_output_2_0_g294 = cleanup274;
				float temp_output_3_0_g294 = ( break7_g294.y - ( ( break7_g294.x + break7_g294.z ) * temp_output_2_0_g294 ) );
				float3 appendResult8_g294 = (float3(temp_output_3_0_g294 , temp_output_3_0_g294 , temp_output_3_0_g294));
				float3 lerpResult373 = lerp( AValue184 , KeyColor356 , GarbageMate354);
				float lerpResult308 = lerp( distance( temp_output_159_0 , lerpResult370 ) , distance( appendResult8_g294 , lerpResult373 ) , KeyStyle302);
				float smoothstepResult61 = smoothstep( pow( _Threshold.x , PreCorrectGamma474 ) , pow( _Threshold.y , PreCorrectGamma474 ) , lerpResult308);
				float3 hsvTorgb2_g285 = RGBToHSV( tex2Dbias( _MainTex, float4( ( appendResult453 + ( float2( -1,0 ) * _Border ) ), 0, _Blurring) ).rgb );
				float3 hsvTorgb3_g285 = HSVToRGB( float3(( hsvTorgb2_g285.x + shift569 ),hsvTorgb2_g285.y,hsvTorgb2_g285.z) );
				float3 temp_cast_7 = (_PreCorrectGamma).xxx;
				float3 temp_output_160_0 = pow( hsvTorgb3_g285 , temp_cast_7 );
				float3 lerpResult364 = lerp( AValueClassic303 , KeyColor356 , GarbageMate354);
				float3 temp_output_1_0_g295 = temp_output_160_0;
				float3 break7_g295 = temp_output_1_0_g295;
				float temp_output_2_0_g295 = cleanup274;
				float temp_output_3_0_g295 = ( break7_g295.y - ( ( break7_g295.x + break7_g295.z ) * temp_output_2_0_g295 ) );
				float3 appendResult8_g295 = (float3(temp_output_3_0_g295 , temp_output_3_0_g295 , temp_output_3_0_g295));
				float3 lerpResult367 = lerp( AValue184 , KeyColor356 , GarbageMate354);
				float lerpResult300 = lerp( distance( temp_output_160_0 , lerpResult364 ) , distance( appendResult8_g295 , lerpResult367 ) , KeyStyle302);
				float smoothstepResult112 = smoothstep( pow( _Threshold.x , PreCorrectGamma474 ) , pow( _Threshold.y , PreCorrectGamma474 ) , lerpResult300);
				float tresholdB324 = pow( _ThresholdL.x , PreCorrectGamma474 );
				float smoothB325 = pow( _ThresholdL.y , PreCorrectGamma474 );
				float smoothstepResult331 = smoothstep( tresholdB324 , smoothB325 , lerpResult316);
				float smoothstepResult330 = smoothstep( tresholdB324 , smoothB325 , lerpResult311);
				float smoothstepResult326 = smoothstep( tresholdB324 , smoothB325 , lerpResult308);
				float smoothstepResult320 = smoothstep( tresholdB324 , smoothB325 , lerpResult300);
				float2 UVFlying455 = uv_garbageMatte;
				float2 break459 = UVFlying455;
				float lerpResult340 = lerp( ( 1.0 - break459.y ) , break459.x , _PortraitMode);
				float smoothstepResult342 = smoothstep( _TransitionPosition , ( _TransitionPosition + _TransitionFalloff ) , lerpResult340);
				float lerpResult335 = lerp( ( smoothstepResult151 + smoothstepResult51 + smoothstepResult61 + smoothstepResult112 ) , ( smoothstepResult331 + smoothstepResult330 + smoothstepResult326 + smoothstepResult320 ) , smoothstepResult342);
				float lerpResult338 = lerp( 0.0 , saturate( pow( pow( ( lerpResult335 * 0.25 ) , _PreCorrectGamma ) , _MaskPower ) ) , tex2DNode4.a);
				float lerpResult348 = lerp( 0.0 , lerpResult338 , tex2D( _CustomMask, UVFlying455 ).r);
				float temp_output_385_0 = ( 1.0 - lerpResult348 );
				float _Despill90 = ( ( _despillChroma * temp_output_385_0 ) + _GreenCastRemove );
				float _DespillLuminanceAdd90 = ( temp_output_385_0 * _despillLuma );
				float4 color90 = float4( temp_output_572_0 , 0.0 );
				float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
				float3 hsvTorgb2_g297 = RGBToHSV( localDespill90.xyz );
				float3 hsvTorgb3_g297 = HSVToRGB( float3(( hsvTorgb2_g297.x + ( shift569 * -1.0 ) ),hsvTorgb2_g297.y,hsvTorgb2_g297.z) );
				float3 In1_g298 = hsvTorgb3_g297;
				float Temperature1_g298 = _whiteBallance;
				float Tint1_g298 = _tint;
				float3 localtemperature_Deckard1_g298 = temperature_Deckard1_g298( In1_g298 , Temperature1_g298 , Tint1_g298 );
				float3 temp_cast_10 = (( 1.0 / _Gamma )).xxx;
				float2 break458 = UVFlying455;
				float lerpResult394 = lerp( 0.0 , lerpResult348 , step( break458.y , ( 1.0 - _cropTop ) ));
				float lerpResult398 = lerp( lerpResult394 , 0.0 , step( break458.y , _cropBottom ));
				float lerpResult399 = lerp( lerpResult398 , 0.0 , step( break458.x , _cropLeft ));
				float lerpResult404 = lerp( 0.0 , lerpResult399 , step( break458.x , ( 1.0 - _cropRight ) ));
				float smoothstepResult463 = smoothstep( _SoftTransition.x , _SoftTransition.y , lerpResult404);
				float4 appendResult175 = (float4(pow( localtemperature_Deckard1_g298 , temp_cast_10 ) , smoothstepResult463));
				
				
				finalColor = appendResult175;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
303;440;2033;763;4311.178;686.3757;1.83303;True;True
Node;AmplifyShaderEditor.ColorNode;178;-1678.357,-825.4505;Inherit;False;Property;_KeyColor;KeyColor;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1882353,0.5254902,0.3058823,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;451;-2037.938,-146.7748;Inherit;False;Constant;_ScaleY;ScaleY;54;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;450;-2021.938,-228.7748;Inherit;False;Constant;_ScaleX;ScaleX;54;1;[Header];Create;True;1;Flying Key;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;595;-2873.614,-618.8813;Inherit;True;Property;_garbageMatte;garbageMatte;2;0;Create;True;0;0;0;True;0;False;None;6366cec5ad51a4d48a449c35c0ce31e1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ComponentMaskNode;218;-1292.218,-848.2313;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;589;-1389.894,-548.3432;Inherit;False;Constant;_FineColorAdjustement;FineColorAdjustement;47;0;Create;True;0;0;0;False;0;False;0.333333;0.3333;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;586;-1189.137,-687.9498;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;453;-1625.938,-102.7748;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-2176.521,163.3885;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-2293.787,568.3889;Inherit;False;Property;_Border;Border Defocus;18;1;[Header];Create;False;1;REFINEMENT;0;0;False;0;False;0;0.00115;0;0.003;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;588;-954.017,-590.3781;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;454;-1670.938,78.22522;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;48;-1878.062,551.4337;Inherit;False;Constant;_Vector0;Vector 0;17;0;Create;True;0;0;0;False;0;False;0,1;-0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;53;-1756.371,817.3896;Inherit;False;Constant;_Vector1;Vector 1;18;0;Create;True;0;0;0;False;0;False;0,-1;0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;57;-1935.919,1231.268;Inherit;False;Constant;_Vector2;Vector 2;20;0;Create;True;0;0;0;False;0;False;1,0;0,0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;569;-769.8101,-619.3431;Inherit;False;shift;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1684.773,422.847;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;107;-1899.632,1453.065;Inherit;False;Constant;_Vector3;Vector 3;19;0;Create;True;0;0;0;False;0;False;-1,0;0,-0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;91;-2517.307,-617.3053;Inherit;True;Property;_GM;GM;1;0;Create;True;0;0;0;False;0;False;-1;None;ee190bc8acbed724fa5d82561712323e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1507.172,824.047;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;273;-2212.319,-361.7162;Inherit;False;Constant;_ColorCleanup;ColorCleanup;18;0;Create;True;0;0;0;True;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1595.721,1256.126;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-2833.848,-92.44072;Inherit;True;Property;_MainTex;_MainTex;10;1;[HideInInspector];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;570;-797.47,-520.9669;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1638.26,-235.2925;Inherit;False;Property;_PreCorrectGamma;PreCorrectGamma;3;0;Create;True;0;0;0;False;0;False;0;0.409;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1808.893,1030.857;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1301.745,706.6785;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-1732.334,1502.622;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-1979.732,1012.17;Inherit;False;Property;_Blurring;Blurring;24;1;[HideInInspector];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1574.236,280.4229;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;220;-2017.548,-589.4926;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;274;-1908.417,-380.2604;Inherit;False;cleanup;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-1608.806,1409.954;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;49;-1658.813,606.3782;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;156;-1258.074,-325.2135;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;41;-1427.174,269.1166;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;275;-1363.536,-414.32;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;567;-551.5956,-566.1874;Inherit;False;HueShift;-1;;266;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;62;-1576.299,941.5252;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;593;-1345.024,612.8817;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;573;-1215.488,1573.343;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;556;-908.4056,-389.5247;Inherit;True;PreKey;-1;;267;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;405;-299.3942,-562.555;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;578;-1279.251,751.7705;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;110;-1493.212,1464.922;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;580;-1230.496,566.5464;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;196;1039.276,-205.7241;Inherit;False;Property;_MatteKey;Matte Key;0;0;Create;False;0;0;0;False;0;False;0;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;575;-1372.05,1127.77;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;594;-1280.024,984.6816;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;592;-1269.623,478.9818;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;474;-1231.51,-108.7158;Inherit;False;PreCorrectGamma;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;354;1285.484,-206.7448;Inherit;False;GarbageMate;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;577;-1098.091,670.8065;Inherit;False;HueShift;-1;;290;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-567.8597,-225.5317;Inherit;False;AValue;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;356;220.9718,-542.459;Inherit;False;KeyColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;576;-1184.49,1074.006;Inherit;False;HueShift;-1;;288;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;579;-1028.636,554.3823;Inherit;False;HueShift;-1;;289;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;574;-1060.429,1496.179;Inherit;False;HueShift;-1;;285;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;303;-564.7075,-341.103;Inherit;False;AValueClassic;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;323;-1037.352,-140.1149;Inherit;False;Property;_ThresholdL;ThresholdL;5;0;Create;True;0;0;0;False;1;RemapSlidersFull1;False;-1,1,-1,1;0.05050505,0.2649579,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;378;-639.2465,938.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;375;-625.2465,1289.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-794.2064,641.0765;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;377;-569.9155,866.3926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-553.6569,1602.042;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;301;-1253.235,1702.884;Inherit;False;Constant;_KeyingAlgorithm;Keying Algorithm;53;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;477;-735.1618,196.8222;Inherit;False;474;PreCorrectGamma;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;357;-1045.227,360.5497;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;455;-1911.766,-21.40478;Inherit;False;UVFlying;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;-508.4467,728.4281;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;369;-702.2465,1809.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;372;-695.2465,1112.701;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;368;-610.8155,1725.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-516.5307,1170.185;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;160;-818.9855,1295.333;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;277;-780.9768,862.8534;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;380;-195.8155,855.6926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;306;-573.2311,1096.265;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;383;-854.8154,121.6926;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;158;-739.5725,734.7864;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;-837.3818,558.8248;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;-761.8969,1604.574;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;381;-257.2465,934.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;366;-749.644,1437.111;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;159;-859.6585,1052.899;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;374;-533.8155,1205.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;-627.1133,1528.396;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;371;-603.8155,1028.693;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;365;-654.313,1392.103;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;278;-734.1923,1180.078;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;384;-856.2465,319.7006;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-1063.378,205.6901;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;355;-1071.603,455.7284;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;-748.5085,287.1229;Inherit;False;184;AValue;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;313;-531.3795,837.9017;Inherit;False;303;AValueClassic;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;557;-455.6212,1325.945;Inherit;False;PreKey;-1;;295;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;559;-652.1216,450.1874;Inherit;False;PreKey;-1;;293;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;460;511.1059,787.038;Inherit;False;455;UVFlying;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;350;-803.5198,406.0551;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;379;13.13931,810.7266;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;364;-423.3582,1398.137;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;590;-742.354,-138.4249;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;558;-708.5913,921.6791;Inherit;False;PreKey;-1;;294;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;560;-540.85,624.4094;Inherit;False;PreKey;-1;;292;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;367;-379.8607,1731.727;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;373;-305.8607,1221.727;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;370;-372.8607,1034.727;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;376;-374.0607,858.1266;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;302;-954.5239,1700.347;Inherit;False;KeyStyle;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;382;-508.8607,348.7266;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;591;-735.354,-35.42493;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;286;-949.4902,7.46703;Inherit;False;Property;_Threshold;Threshold;4;0;Create;True;0;0;0;False;1;RemapSlidersFull1;False;-1,1,-1,1;0.04040404,0.1857105,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;111;-208.0569,1453.15;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;459;679.1061,790.2378;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;476;-512.2316,46.9235;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;50;-228.1712,648.1748;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;312;-281.9985,553.6985;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;304;-292.6423,1623.13;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;314;-437.1522,505.0518;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;309;-280.5964,1148.992;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;60;-238.1814,1030.196;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;310;-251.8358,759.5383;Inherit;False;302;KeyStyle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;478;-424.2466,188.5729;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;150;-391.5171,386.3198;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;315;-380.8606,290.3941;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;325;-554.1021,-56.49899;Inherit;False;smoothB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;307;-321.2138,981.6524;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;299;-194.8689,1297.984;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;324;-575.524,-140.2256;Inherit;False;tresholdB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;308;-80.38658,1041.268;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;346;772.827,1019.616;Inherit;False;Property;_PortraitMode;PortraitMode;32;1;[Toggle];Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;311;-51.12589,653.5139;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;328;1075.665,1434.632;Inherit;False;325;smoothB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;343;513.499,467.7491;Inherit;False;Property;_TransitionPosition;TransitionPosition;29;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;288;-102.3038,149.8756;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;287;-244.9379,56.07851;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;349;832.3052,761.1069;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;300;-19.43263,1369.226;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;316;-219.3788,403.4459;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;327;1089.135,1316.851;Inherit;False;324;tresholdB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;344;614.499,588.7491;Inherit;False;Property;_TransitionFalloff;TransitionFalloff;30;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;51;262.7787,634.0588;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;345;948.7991,585.649;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;340;1006.899,694.1491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;61;125.5221,1056.977;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;330;1391.916,1040.334;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;326;1367.511,1247.767;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;359.4202,1270.184;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;151;-0.3899524,453.8012;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;320;1378.507,1509.939;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;331;1392.231,898.4204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;332;1706.548,982.3327;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;342;1089.199,527.049;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;258;793.4918,371.8522;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;335;1061.218,358.9373;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;1270.534,530.389;Inherit;False;Constant;_Float5;Float 5;26;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;1257.596,359.2685;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;1046.917,61.11723;Inherit;False;Property;_MaskPower;MaskPower;11;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;262;1378.474,232.2491;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;1379.882,28.65171;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;138;3074.074,-198.7457;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1414.947,35.68357;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;MipBias;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;456;4445.397,-204.8451;Inherit;False;455;UVFlying;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;338;3763.509,67.34172;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;347;4021.448,700.113;Inherit;True;Property;_CustomMask;CustomMask;46;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;457;4390.267,805.2528;Inherit;False;455;UVFlying;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;391;4534.938,533.1982;Inherit;False;Property;_cropTop;cropTop;42;1;[Header];Create;True;1;CROPPING;0;0;False;0;False;1;0.016;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;348;4324.59,500.1496;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;406;4811.25,570.3521;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;385;2751.941,568.1809;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;458;4600.707,820.6378;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;88;2196.319,515.2901;Inherit;False;Property;_despillChroma;despillChroma;13;0;Create;True;0;0;0;False;0;False;0;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;224;1551.97,145.5646;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;161;2398.288,578.8126;Inherit;False;Property;_despillLuma;despillLuma;14;0;Create;True;0;0;0;False;0;False;0;-1;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;396;5091.591,626.5516;Inherit;False;Property;_cropBottom;cropBottom;43;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;393;4984.288,526.4566;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;571;1816.244,224.8497;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;2625,455.9586;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;388;2266.831,446.4537;Inherit;False;Property;_GreenCastRemove;GreenCastRemove;17;0;Create;True;0;0;0;False;0;False;0;0.486;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;5474.065,385.0587;Inherit;False;Property;_cropRight;cropRight;45;0;Create;True;0;0;0;False;0;False;0;0.157;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;572;2041.02,77.30007;Inherit;False;HueShift;-1;;296;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;400;5355.205,491.3647;Inherit;False;Property;_cropLeft;cropLeft;44;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;387;2800.984,354.557;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;582;3733.602,261.6841;Inherit;False;569;shift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;397;5449.572,691.1716;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;394;4913.339,310.4947;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;2956.213,640.3167;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;398;5125.123,412.8097;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;90;3282.216,6.43433;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;407;5747.159,399.2397;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;401;5702.125,524.646;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;583;3936.915,216.5384;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;3866.499,400.5335;Inherit;False;Property;_whiteBallance;whiteBallance;22;0;Create;True;0;0;0;False;0;False;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;264;3855.077,482.8788;Inherit;False;Property;_tint;tint;23;0;Create;True;1;Color Correction;0;0;False;0;False;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;399;5327.428,175.9382;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;4316.566,310.7035;Inherit;False;Property;_Gamma;Gamma;21;1;[Header];Create;True;1;COLOR CORRECTION;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;403;5194.275,257.6631;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;581;3976.813,30.17499;Inherit;False;HueShift;-1;;297;2956ffeda48e6d74cb568197403f2638;0;2;5;FLOAT3;0,0,0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;101;4270.889,58.50293;Inherit;False;ColorTemperature;7;;298;fccce2e41bca18a41863dccc23edb3ef;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;462;5651.055,201.9601;Inherit;False;Property;_SoftTransition;SoftTransition;6;0;Create;True;0;0;0;False;1;RemapSlidersFull1;False;0,0.65,0,1;0.7323229,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;265;4519.562,200.7419;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;404;5526.949,190.9784;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;263;4741.576,9.756815;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;463;5986.697,209.6883;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;746.6285,1367.332;Inherit;False;325;smoothB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;433;5908.944,-324.7903;Inherit;False;Property;_ScreenSpaceProjection;ScreenSpaceProjection;41;1;[Toggle];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;409;2462.888,116.4995;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;193;-77.26059,-64.4853;Inherit;False;tresholdA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;410;2911.625,151.2278;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;423;5412.641,-714.881;Inherit;False;Property;_NormalScale;_NormalScale;36;0;Create;True;0;0;0;True;0;False;0;0.95;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;555;-100.3703,-451.3309;Inherit;True;PreKey;-1;;299;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;249;4337.124,-740.3391;Inherit;True;Property;_Texture0;Texture 0;25;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;390;4569.527,616.3612;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;412;2400.776,253.8005;Inherit;False;Property;_DespilChromaOffset;DespilChromaOffset;33;0;Create;True;0;0;0;False;0;False;0;0;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;361;-901.8984,1170.474;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RGBToHSVNode;413;3142.901,320.7012;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;411;2766.437,161.2843;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;417;4444.409,-532.515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;561;286.4067,425.3245;Inherit;False;PreKey;-1;;300;723032daf2cdc3e49bc6b660c9cefeb1;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.HSVToRGBNode;414;3516.689,285.4995;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;186;524.8491,378.485;Inherit;False;BValue;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;253;4116.297,-231.8343;Inherit;False;Property;_Size;Size;31;0;Create;True;0;0;0;False;0;False;0;0;0;25.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;175;5804.332,-67.51196;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;225;99.37017,370.6135;Inherit;False;True;True;True;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;427;5547.57,-460.5093;Inherit;False;Property;_AlphaCutoff;Alpha Cutoff;12;0;Create;False;0;0;0;False;0;False;0.93;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;430;5875.276,-481.7969;Inherit;False;Property;_iterations;Iterations;34;2;[Header];[IntRange];Create;False;1;RELIGHTING GENERATION;0;0;True;0;False;0;8;1;16;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;472;6217.257,107.7712;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;464;5949.918,-243.237;Inherit;False;Property;_DespillComposite;Composite Despill;15;0;Create;False;0;0;0;True;0;False;0;-1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;362;-1117.851,1182.44;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;168;4490.38,1267.329;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;424;5449.042,-638.1811;Inherit;False;Property;_Cleanup;Cleanup Mask;19;0;Create;False;0;0;0;True;0;False;0;0.093;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;421;5320.34,-876.0809;Inherit;False;Property;_Smoothness;_Smoothness;39;0;Create;True;0;0;0;True;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;422;5365.837,-796.7808;Inherit;False;Property;_Metallic;_Metallic;40;0;Create;True;0;0;0;True;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;415;3376.289,243.8995;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;251;4198.783,-406.2719;Inherit;False;Property;_Directions;Directions;26;1;[IntRange];Create;True;0;0;0;False;0;False;0;44;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;429;5877.915,-407.8056;Inherit;False;Property;_ColorInfluence;Color Influence;35;0;Create;False;0;0;0;True;0;False;0.5;0.201;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;360;-1189.607,932.7255;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;465;5966.96,-163.2737;Inherit;False;Property;_DespillLumaComposite;Composite Luma Despill;16;0;Create;False;0;0;0;True;0;False;0;-2.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;420;4901.108,-1094.215;Inherit;False;Property;_EmissiveColor;_EmissiveColor;38;1;[HDR];Create;True;1;Plane Material Settings;0;0;True;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;425;5507.32,-555.6855;Inherit;False;Property;_Range;Cleanup Range;20;0;Create;False;0;0;0;True;0;False;0;0.136;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;416;5132.11,-1091.515;Inherit;False;Property;_BaseColor;_BaseColor;37;1;[Header];Create;True;1;MATERIAL SETTINGS;0;0;True;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;490.2103,184.284;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;363;-1105.282,1260.448;Inherit;False;354;GarbageMate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;359;-1169.676,845.618;Inherit;False;356;KeyColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexelSizeNode;133;-2662.301,321.0915;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;475;5856.507,598.5718;Inherit;False;474;PreCorrectGamma;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;501.9122,291.8068;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;321;732.5998,1251.963;Inherit;False;324;tresholdB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;4103.583,-329.5719;Inherit;False;Property;_Quality;Quality;27;1;[IntRange];Create;True;0;0;0;False;0;False;0;9;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;282;110.5613,560.5721;Inherit;False;274;cleanup;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;248;4611.867,-447.2937;Inherit;False;float Pi = 6.28318530718@ $    $$   $    float2 Radius = Size/iResolution.xy@$    $$    float2 UW@$UW = UV0/iResolution.xy@$   // float4 Col@$//Col = tex2D(tex, UW)@$    $    // Blur calculations$    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions - 15.0@$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.SamplerNode;339;3369.32,-475.3234;Inherit;True;Property;_noise_01;noise_01;28;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;358;-941.6237,821.7521;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;63.14589,141.4648;Inherit;False;smoothA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;434;5528.157,-1175.978;Inherit;True;Property;_InputTexture;InputTexture;47;0;Create;True;0;0;0;True;0;False;None;None;False;black;LockedToTexture2D;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;176;6441.576,-126.3859;Float;False;True;-1;2;ASEMaterialInspector;100;1;ChromaLiveRT;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;218;0;178;0
WireConnection;586;0;218;0
WireConnection;453;0;450;0
WireConnection;453;1;451;0
WireConnection;44;2;595;0
WireConnection;588;0;589;0
WireConnection;588;1;586;1
WireConnection;454;0;44;0
WireConnection;454;1;453;0
WireConnection;569;0;588;0
WireConnection;54;0;48;0
WireConnection;54;1;56;0
WireConnection;91;0;595;0
WireConnection;91;1;454;0
WireConnection;55;0;53;0
WireConnection;55;1;56;0
WireConnection;58;0;57;0
WireConnection;58;1;56;0
WireConnection;59;0;44;0
WireConnection;59;1;58;0
WireConnection;52;0;454;0
WireConnection;52;1;55;0
WireConnection;108;0;107;0
WireConnection;108;1;56;0
WireConnection;42;0;454;0
WireConnection;42;1;54;0
WireConnection;220;0;91;0
WireConnection;274;0;273;0
WireConnection;109;0;453;0
WireConnection;109;1;108;0
WireConnection;49;0;3;0
WireConnection;49;1;52;0
WireConnection;49;2;205;0
WireConnection;156;0;220;0
WireConnection;156;1;30;0
WireConnection;41;0;3;0
WireConnection;41;1;42;0
WireConnection;41;2;205;0
WireConnection;567;5;218;0
WireConnection;567;6;570;0
WireConnection;62;0;3;0
WireConnection;62;1;59;0
WireConnection;62;2;205;0
WireConnection;593;0;49;0
WireConnection;556;1;156;0
WireConnection;556;2;275;0
WireConnection;405;0;567;0
WireConnection;405;1;30;0
WireConnection;110;0;3;0
WireConnection;110;1;109;0
WireConnection;110;2;205;0
WireConnection;594;0;62;0
WireConnection;592;0;41;0
WireConnection;474;0;30;0
WireConnection;354;0;196;0
WireConnection;577;5;593;0
WireConnection;577;6;578;0
WireConnection;184;0;556;0
WireConnection;356;0;405;0
WireConnection;576;5;594;0
WireConnection;576;6;575;0
WireConnection;579;5;592;0
WireConnection;579;6;580;0
WireConnection;574;5;110;0
WireConnection;574;6;573;0
WireConnection;303;0;556;0
WireConnection;157;0;579;0
WireConnection;157;1;30;0
WireConnection;455;0;44;0
WireConnection;160;0;574;0
WireConnection;160;1;30;0
WireConnection;158;0;577;0
WireConnection;158;1;30;0
WireConnection;159;0;576;0
WireConnection;159;1;30;0
WireConnection;557;1;160;0
WireConnection;557;2;279;0
WireConnection;559;1;157;0
WireConnection;559;2;276;0
WireConnection;350;0;317;0
WireConnection;350;1;357;0
WireConnection;350;2;355;0
WireConnection;379;0;190;0
WireConnection;379;1;380;0
WireConnection;379;2;381;0
WireConnection;364;0;305;0
WireConnection;364;1;365;0
WireConnection;364;2;366;0
WireConnection;590;0;323;1
WireConnection;590;1;477;0
WireConnection;558;1;159;0
WireConnection;558;2;278;0
WireConnection;560;1;158;0
WireConnection;560;2;277;0
WireConnection;367;0;192;0
WireConnection;367;1;368;0
WireConnection;367;2;369;0
WireConnection;373;0;191;0
WireConnection;373;1;374;0
WireConnection;373;2;375;0
WireConnection;370;0;306;0
WireConnection;370;1;371;0
WireConnection;370;2;372;0
WireConnection;376;0;313;0
WireConnection;376;1;377;0
WireConnection;376;2;378;0
WireConnection;302;0;301;0
WireConnection;382;0;189;0
WireConnection;382;1;383;0
WireConnection;382;2;384;0
WireConnection;591;0;323;2
WireConnection;591;1;477;0
WireConnection;111;0;557;0
WireConnection;111;1;367;0
WireConnection;459;0;460;0
WireConnection;476;0;286;1
WireConnection;476;1;477;0
WireConnection;50;0;560;0
WireConnection;50;1;379;0
WireConnection;312;0;158;0
WireConnection;312;1;376;0
WireConnection;60;0;558;0
WireConnection;60;1;373;0
WireConnection;478;0;286;2
WireConnection;478;1;477;0
WireConnection;150;0;382;0
WireConnection;150;1;559;0
WireConnection;315;0;157;0
WireConnection;315;1;350;0
WireConnection;325;0;591;0
WireConnection;307;0;159;0
WireConnection;307;1;370;0
WireConnection;299;0;160;0
WireConnection;299;1;364;0
WireConnection;324;0;590;0
WireConnection;308;0;307;0
WireConnection;308;1;60;0
WireConnection;308;2;309;0
WireConnection;311;0;312;0
WireConnection;311;1;50;0
WireConnection;311;2;310;0
WireConnection;288;0;478;0
WireConnection;287;0;476;0
WireConnection;349;0;459;1
WireConnection;300;0;299;0
WireConnection;300;1;111;0
WireConnection;300;2;304;0
WireConnection;316;0;315;0
WireConnection;316;1;150;0
WireConnection;316;2;314;0
WireConnection;51;0;311;0
WireConnection;51;1;287;0
WireConnection;51;2;288;0
WireConnection;345;0;343;0
WireConnection;345;1;344;0
WireConnection;340;0;349;0
WireConnection;340;1;459;0
WireConnection;340;2;346;0
WireConnection;61;0;308;0
WireConnection;61;1;287;0
WireConnection;61;2;288;0
WireConnection;330;0;311;0
WireConnection;330;1;327;0
WireConnection;330;2;328;0
WireConnection;326;0;308;0
WireConnection;326;1;327;0
WireConnection;326;2;328;0
WireConnection;112;0;300;0
WireConnection;112;1;287;0
WireConnection;112;2;288;0
WireConnection;151;0;316;0
WireConnection;151;1;287;0
WireConnection;151;2;288;0
WireConnection;320;0;300;0
WireConnection;320;1;327;0
WireConnection;320;2;328;0
WireConnection;331;0;316;0
WireConnection;331;1;327;0
WireConnection;331;2;328;0
WireConnection;332;0;331;0
WireConnection;332;1;330;0
WireConnection;332;2;326;0
WireConnection;332;3;320;0
WireConnection;342;0;340;0
WireConnection;342;1;343;0
WireConnection;342;2;345;0
WireConnection;258;0;151;0
WireConnection;258;1;51;0
WireConnection;258;2;61;0
WireConnection;258;3;112;0
WireConnection;335;0;258;0
WireConnection;335;1;332;0
WireConnection;335;2;342;0
WireConnection;259;0;335;0
WireConnection;259;1;260;0
WireConnection;262;0;259;0
WireConnection;262;1;30;0
WireConnection;173;0;262;0
WireConnection;173;1;76;0
WireConnection;138;0;173;0
WireConnection;4;0;3;0
WireConnection;4;1;454;0
WireConnection;4;2;56;0
WireConnection;338;1;138;0
WireConnection;338;2;4;4
WireConnection;347;1;456;0
WireConnection;348;1;338;0
WireConnection;348;2;347;1
WireConnection;406;0;391;0
WireConnection;385;0;348;0
WireConnection;458;0;457;0
WireConnection;224;0;4;0
WireConnection;393;0;458;1
WireConnection;393;1;406;0
WireConnection;389;0;88;0
WireConnection;389;1;385;0
WireConnection;572;5;224;0
WireConnection;572;6;571;0
WireConnection;387;0;389;0
WireConnection;387;1;388;0
WireConnection;397;0;458;1
WireConnection;397;1;396;0
WireConnection;394;1;348;0
WireConnection;394;2;393;0
WireConnection;386;0;385;0
WireConnection;386;1;161;0
WireConnection;398;0;394;0
WireConnection;398;2;397;0
WireConnection;90;0;572;0
WireConnection;90;1;387;0
WireConnection;90;2;386;0
WireConnection;90;3;572;0
WireConnection;407;0;402;0
WireConnection;401;0;458;0
WireConnection;401;1;400;0
WireConnection;583;0;582;0
WireConnection;399;0;398;0
WireConnection;399;2;401;0
WireConnection;403;0;458;0
WireConnection;403;1;407;0
WireConnection;581;5;90;0
WireConnection;581;6;583;0
WireConnection;101;2;581;0
WireConnection;101;5;103;0
WireConnection;101;6;264;0
WireConnection;265;1;163;0
WireConnection;404;1;399;0
WireConnection;404;2;403;0
WireConnection;263;0;101;0
WireConnection;263;1;265;0
WireConnection;463;0;404;0
WireConnection;463;1;462;1
WireConnection;463;2;462;2
WireConnection;409;0;572;0
WireConnection;193;0;287;0
WireConnection;410;0;411;0
WireConnection;410;1;409;2
WireConnection;410;2;409;3
WireConnection;555;1;405;0
WireConnection;555;2;275;0
WireConnection;361;0;576;0
WireConnection;361;1;362;0
WireConnection;361;2;363;0
WireConnection;413;0;90;0
WireConnection;411;0;409;1
WireConnection;411;1;412;0
WireConnection;561;1;225;0
WireConnection;561;2;282;0
WireConnection;414;0;415;0
WireConnection;414;1;413;2
WireConnection;414;2;413;3
WireConnection;186;0;561;0
WireConnection;175;0;263;0
WireConnection;175;3;463;0
WireConnection;472;0;463;0
WireConnection;472;1;475;0
WireConnection;415;0;413;1
WireConnection;415;1;412;0
WireConnection;133;0;3;0
WireConnection;248;0;3;0
WireConnection;248;1;251;0
WireConnection;248;2;252;0
WireConnection;248;3;253;0
WireConnection;248;5;456;0
WireConnection;358;0;577;0
WireConnection;358;1;359;0
WireConnection;358;2;360;0
WireConnection;195;0;288;0
WireConnection;176;0;175;0
ASEEND*/
//CHKSM=19F1630478CA83ABD0DAFC3979993E64E5A43601