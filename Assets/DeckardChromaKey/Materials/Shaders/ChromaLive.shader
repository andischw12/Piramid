// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ChromaLive"
{
	Properties
	{
		_KEyColor("KEyColor", Color) = (0,0,0,0)
		_MainTex("_MainTex", 2D) = "white" {}
		_treshold("treshold", Float) = 0
		_smooth("smooth", Float) = 0
		_Power("Power", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_MEtalic("MEtalic", Float) = 1
		_Border("Border", Range( 0 , 0.01)) = 0
		_MaskPower("MaskPower", Float) = 0
		_leftCut("leftCut", Range( 0 , 1)) = 0
		_despillChroma("despillChroma", Float) = 0
		_ultimatte("ultimatte", 2D) = "white" {}
		_whiteBallance("whiteBallance", Range( -1 , 1)) = 1
		_Sharpen("Sharpen", Float) = 0
		_Contrast("Contrast", Float) = 1
		[Toggle]_MateKey("MateKey", Float) = 0
		_LuminanceRemapping("LuminanceRemapping", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Border;
		uniform float _Power;
		uniform float _Sharpen;
		uniform float _despillChroma;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _LuminanceRemapping;
		uniform float _whiteBallance;
		uniform float _Contrast;
		uniform float _MEtalic;
		uniform float _Smoothness;
		uniform float _treshold;
		uniform float _smooth;
		uniform float _MateKey;
		uniform sampler2D _ultimatte;
		uniform float4 _ultimatte_ST;
		uniform float4 _KEyColor;
		uniform float _MaskPower;
		uniform float _leftCut;


		float4 Despill( float4 result , float _Despill , float _DespillLuminanceAdd , float4 color )
		{
			float v = (2*result.b+result.r)/4;
			                if(result.g > v) result.g = lerp(result.g, v, _Despill);
			                float4 dif = (color - result);
			                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
			                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
			return result;
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

		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float3 temperature_Deckard1_g1( float3 In , float Temperature , float Tint )
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode4 = tex2D( _MainTex, uv0_MainTex );
			float4 tex2DNode110 = tex2D( _MainTex, ( uv0_MainTex + ( float2( -1,0 ) * _Border ) ) );
			float4 temp_cast_0 = (_Power).xxxx;
			float4 tex2DNode41 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 0,1 ) * _Border ) ) );
			float4 temp_cast_1 = (_Power).xxxx;
			float4 tex2DNode49 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 0,-1 ) * _Border ) ) );
			float4 temp_cast_2 = (_Power).xxxx;
			float4 tex2DNode62 = tex2D( _MainTex, ( uv0_MainTex + ( float2( 1,0 ) * _Border ) ) );
			float4 temp_cast_3 = (_Power).xxxx;
			float4 temp_output_125_0 = ( tex2DNode4 + saturate( ( ( tex2DNode4 - ( ( pow( tex2DNode110 , temp_cast_0 ) + pow( tex2DNode41 , temp_cast_1 ) + pow( tex2DNode49 , temp_cast_2 ) + pow( tex2DNode62 , temp_cast_3 ) ) * 0.25 ) ) * _Sharpen ) ) );
			float4 result90 = temp_output_125_0;
			float _Despill90 = _despillChroma;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor93 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
			float3 hsvTorgb95 = RGBToHSV( screenColor93.rgb );
			float temp_output_96_0 = (_LuminanceRemapping.z + (saturate( hsvTorgb95.z ) - _LuminanceRemapping.x) * (_LuminanceRemapping.w - _LuminanceRemapping.z) / (_LuminanceRemapping.y - _LuminanceRemapping.x));
			float _DespillLuminanceAdd90 = temp_output_96_0;
			float4 color90 = temp_output_125_0;
			float4 localDespill90 = Despill( result90 , _Despill90 , _DespillLuminanceAdd90 , color90 );
			float3 In1_g1 = localDespill90.xyz;
			float Temperature1_g1 = _whiteBallance;
			float Tint1_g1 = 0.0;
			float3 localtemperature_Deckard1_g1 = temperature_Deckard1_g1( In1_g1 , Temperature1_g1 , Tint1_g1 );
			float3 temp_cast_8 = (_Contrast).xxx;
			o.Emission = pow( saturate( localtemperature_Deckard1_g1 ) , temp_cast_8 );
			o.Metallic = _MEtalic;
			o.Smoothness = _Smoothness;
			float temp_output_13_0 = ( _treshold + _smooth );
			float4 temp_cast_9 = (_Power).xxxx;
			float2 uv_ultimatte = i.uv_texcoord * _ultimatte_ST.xy + _ultimatte_ST.zw;
			float smoothstepResult10 = smoothstep( _treshold , temp_output_13_0 , distance( pow( tex2DNode4 , temp_cast_9 ) , (( _MateKey )?( _KEyColor ):( tex2D( _ultimatte, uv_ultimatte ) )) ));
			float temp_output_75_0 = pow( smoothstepResult10 , _MaskPower );
			float smoothstepResult112 = smoothstep( _treshold , temp_output_13_0 , distance( tex2DNode110 , (( _MateKey )?( _KEyColor ):( tex2D( _ultimatte, uv_ultimatte ) )) ));
			float smoothstepResult51 = smoothstep( _treshold , temp_output_13_0 , distance( tex2DNode49 , (( _MateKey )?( _KEyColor ):( tex2D( _ultimatte, uv_ultimatte ) )) ));
			float smoothstepResult61 = smoothstep( _treshold , temp_output_13_0 , distance( (( _MateKey )?( _KEyColor ):( tex2D( _ultimatte, uv_ultimatte ) )) , tex2DNode62 ));
			float smoothstepResult151 = smoothstep( _treshold , temp_output_13_0 , distance( (( _MateKey )?( _KEyColor ):( tex2D( _ultimatte, uv_ultimatte ) )) , tex2DNode41 ));
			float temp_output_47_0 = ( smoothstepResult112 * smoothstepResult51 * smoothstepResult61 * smoothstepResult151 );
			float lerpResult86 = lerp( ( temp_output_75_0 * pow( temp_output_47_0 , _MaskPower ) ) , 0.0 , step( uv0_MainTex.x , _leftCut ));
			o.Alpha = saturate( lerpResult86 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17701
1218;408;1693;936;1987.623;41.94545;1.901711;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1665.924,-140.3253;Inherit;True;Property;_MainTex;_MainTex;5;0;Create;True;0;0;False;0;None;;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2191.087,572.2889;Inherit;False;Property;_Border;Border;14;0;Create;True;0;0;False;0;0;0.0017;0;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;57;-1768.219,1137.668;Inherit;False;Constant;_Vector2;Vector 2;20;0;Create;True;0;0;False;0;1,0;0,0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;107;-1731.932,1442.665;Inherit;False;Constant;_Vector3;Vector 3;19;0;Create;True;0;0;False;0;-1,0;0,-0.002;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;48;-1672.862,496.3339;Inherit;False;Constant;_Vector0;Vector 0;17;0;Create;True;0;0;False;0;0,1;-0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;53;-1756.371,817.3896;Inherit;False;Constant;_Vector1;Vector 1;18;0;Create;True;0;0;False;0;0,-1;0.002,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1507.172,824.047;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-1482.734,1449.322;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1519.021,1144.325;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1622.804,260.147;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1436.773,532.847;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1313.593,1026.957;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1316.236,397.4229;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1301.745,706.6785;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-1277.306,1331.954;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.001,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;62;-1126.499,1013.025;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-1360.696,-176.5023;Inherit;False;Property;_Power;Power;11;0;Create;True;0;0;False;0;0;1.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;-1133.213,677.2783;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1169.686,280.9294;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;110;-1090.211,1318.022;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;157;-570.3992,493.855;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;158;-682.954,775.6922;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;159;-606.0349,983.2991;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;160;-700.1854,1358.733;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;91;-1913.643,-439.9294;Inherit;True;Property;_ultimatte;ultimatte;20;0;Create;True;0;0;False;0;-1;de95aa1d908b6fa469321aba44d5eeb7;de95aa1d908b6fa469321aba44d5eeb7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;114;293.6662,981.0519;Inherit;False;Constant;_Float7;Float 7;26;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-1770.801,-683.1844;Inherit;False;Property;_KEyColor;KEyColor;3;0;Create;True;0;0;False;0;0,0,0,0;0.3411762,0.5294118,0.2901958,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1215.235,5.583747;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;124;612.4606,630.7542;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;174;-1391.388,-579.9988;Inherit;False;Property;_MateKey;MateKey;24;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;797.1655,805.3823;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;127;1976.728,100.5313;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-297.3124,187.9063;Inherit;False;Property;_smooth;smooth;10;0;Create;True;0;0;False;0;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;1819.426,562.803;Inherit;False;Property;_Sharpen;Sharpen;22;0;Create;True;0;0;False;0;0;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;93;2042.184,717.0014;Inherit;False;Global;_GrabScreen0;Grab Screen 0;22;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-322.3727,81.78047;Inherit;False;Property;_treshold;treshold;9;0;Create;True;0;0;False;0;0;-0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;126;1831.896,432.4939;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;92;-607.4353,-433.7401;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;156;-659.5197,-190.7379;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;2090.992,366.5024;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceOpNode;150;-264.8849,421.4899;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;60;-297.5894,1055.213;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;50;-283.3825,666.355;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;111;-364.1051,1347.119;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-48.72472,201.6401;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;95;2277.934,692.8798;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;137;2274.255,391.2306;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;51;13.41712,753.848;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;9;-77.70997,-106.8779;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;99;2450.18,906.6853;Inherit;False;Property;_LuminanceRemapping;LuminanceRemapping;25;0;Create;True;0;0;False;0;0,0,0,0;0,1,-2.53,1.96;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;61;-39.85593,1061.907;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;164;2522.155,655.9666;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;-105.6854,1338;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;151;-31.24823,478.6906;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;2350.231,111.46;Inherit;False;Property;_despillChroma;despillChroma;18;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;10;550.342,-167.2761;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;125;2372.783,231.8221;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;76;780.2933,-32.90762;Inherit;False;Property;_MaskPower;MaskPower;16;0;Create;True;0;0;False;0;0;1.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1212.895,207.9212;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;96;2692.662,637.9329;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;75;1135.139,-78.87489;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;2089.843,-0.8934394;Inherit;False;Property;_leftCut;leftCut;17;0;Create;True;0;0;False;0;0;0.353;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;3097.288,412.6246;Inherit;False;Property;_whiteBallance;whiteBallance;21;0;Create;True;0;0;False;0;1;0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;173;1379.882,28.65171;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;90;2792.686,132.733;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;101;3539.833,251.3967;Inherit;False;ColorTemperature;0;;1;fccce2e41bca18a41863dccc23edb3ef;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;1529.658,-228.0663;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;85;2419.223,-103.575;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;102;3891.648,222.48;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;86;2559.51,-186.1903;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;3878.43,-94.14172;Inherit;False;Property;_Contrast;Contrast;23;0;Create;True;0;0;False;0;1;1.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;155;-1055.503,-344.2372;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;162;4169.456,-32.4091;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;64;3384.884,-491.2192;Inherit;False;Property;_MEtalic;MEtalic;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;154;-761.9008,535.0508;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;138;3087.409,-208.3581;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;147;-703.9763,939.3295;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;152;668.0346,206.2971;Inherit;False;Constant;_Float3;Float 3;22;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-323.789,-951.0927;Inherit;False;Property;_KEyColor2;KEyColor2;4;0;Create;True;0;0;False;0;0,0,0,0;0.02883587,0.6792453,0.0954437,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;171;1506.254,-78.98003;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;142;-706.4825,-76.84003;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;170;2598.811,551.688;Inherit;False;Property;_Contour;Contour;15;0;Create;True;0;0;False;0;0;7.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;3043.017,-547.0101;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.4339623,0.4339623,0.4339623,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;3364.908,-397.2164;Inherit;False;Property;_Smoothness;Smoothness;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;146;-763.5262,687.6387;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LinearToGammaNode;144;3244.788,301.5865;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;167;2919.185,387.672;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.49;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;143;-820.7481,418.0169;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;139;2846.925,-62.29969;Inherit;False;Constant;_Color1;Color 1;26;0;Create;True;0;0;False;0;0,0.6320754,0.2043574,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexelSizeNode;133;-2004.543,345.9628;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;153;577.0273,301.396;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;2424.54,477.715;Inherit;False;Property;_despillLuma;despillLuma;19;0;Create;True;0;0;False;0;0;-1.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-630,207;Inherit;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;0;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;148;-752.4014,1232.19;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;169;2684.757,445.3208;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;168;4490.38,1267.329;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-403,291;Inherit;False;Property;_Float2;Float 2;8;0;Create;True;0;0;False;0;0;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-644,281;Inherit;False;Property;_Float1;Float 1;6;0;Create;True;0;0;False;0;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4367.13,-609.8487;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ChromaLive;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.49;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;53;0
WireConnection;55;1;56;0
WireConnection;108;0;107;0
WireConnection;108;1;56;0
WireConnection;58;0;57;0
WireConnection;58;1;56;0
WireConnection;44;2;3;0
WireConnection;54;0;48;0
WireConnection;54;1;56;0
WireConnection;59;0;44;0
WireConnection;59;1;58;0
WireConnection;42;0;44;0
WireConnection;42;1;54;0
WireConnection;52;0;44;0
WireConnection;52;1;55;0
WireConnection;109;0;44;0
WireConnection;109;1;108;0
WireConnection;62;0;3;0
WireConnection;62;1;59;0
WireConnection;49;0;3;0
WireConnection;49;1;52;0
WireConnection;41;0;3;0
WireConnection;41;1;42;0
WireConnection;110;0;3;0
WireConnection;110;1;109;0
WireConnection;157;0;41;0
WireConnection;157;1;30;0
WireConnection;158;0;49;0
WireConnection;158;1;30;0
WireConnection;159;0;62;0
WireConnection;159;1;30;0
WireConnection;160;0;110;0
WireConnection;160;1;30;0
WireConnection;4;0;3;0
WireConnection;4;1;44;0
WireConnection;124;0;160;0
WireConnection;124;1;157;0
WireConnection;124;2;158;0
WireConnection;124;3;159;0
WireConnection;174;0;91;0
WireConnection;174;1;2;0
WireConnection;104;0;124;0
WireConnection;104;1;114;0
WireConnection;127;0;4;0
WireConnection;126;0;127;0
WireConnection;126;1;104;0
WireConnection;92;0;174;0
WireConnection;156;0;4;0
WireConnection;156;1;30;0
WireConnection;132;0;126;0
WireConnection;132;1;120;0
WireConnection;150;0;92;0
WireConnection;150;1;41;0
WireConnection;60;0;92;0
WireConnection;60;1;62;0
WireConnection;50;0;49;0
WireConnection;50;1;92;0
WireConnection;111;0;110;0
WireConnection;111;1;92;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;95;0;93;0
WireConnection;137;0;132;0
WireConnection;51;0;50;0
WireConnection;51;1;11;0
WireConnection;51;2;13;0
WireConnection;9;0;156;0
WireConnection;9;1;92;0
WireConnection;61;0;60;0
WireConnection;61;1;11;0
WireConnection;61;2;13;0
WireConnection;164;0;95;3
WireConnection;112;0;111;0
WireConnection;112;1;11;0
WireConnection;112;2;13;0
WireConnection;151;0;150;0
WireConnection;151;1;11;0
WireConnection;151;2;13;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;10;2;13;0
WireConnection;125;0;127;0
WireConnection;125;1;137;0
WireConnection;47;0;112;0
WireConnection;47;1;51;0
WireConnection;47;2;61;0
WireConnection;47;3;151;0
WireConnection;96;0;164;0
WireConnection;96;1;99;1
WireConnection;96;2;99;2
WireConnection;96;3;99;3
WireConnection;96;4;99;4
WireConnection;75;0;10;0
WireConnection;75;1;76;0
WireConnection;173;0;47;0
WireConnection;173;1;76;0
WireConnection;90;0;125;0
WireConnection;90;1;88;0
WireConnection;90;2;96;0
WireConnection;90;3;125;0
WireConnection;101;2;90;0
WireConnection;101;5;103;0
WireConnection;172;0;75;0
WireConnection;172;1;173;0
WireConnection;85;0;44;1
WireConnection;85;1;87;0
WireConnection;102;0;101;0
WireConnection;86;0;172;0
WireConnection;86;2;85;0
WireConnection;155;0;174;0
WireConnection;155;1;30;0
WireConnection;162;0;102;0
WireConnection;162;1;163;0
WireConnection;138;0;86;0
WireConnection;147;0;62;0
WireConnection;171;0;75;0
WireConnection;171;1;47;0
WireConnection;142;0;4;0
WireConnection;94;0;93;0
WireConnection;146;0;49;0
WireConnection;144;0;90;0
WireConnection;167;0;96;0
WireConnection;167;2;169;0
WireConnection;143;0;41;0
WireConnection;133;0;3;0
WireConnection;148;0;110;0
WireConnection;169;0;86;0
WireConnection;169;1;170;0
WireConnection;0;2;162;0
WireConnection;0;3;64;0
WireConnection;0;4;63;0
WireConnection;0;9;138;0
ASEEND*/
//CHKSM=BE6D9E2BB16F5DEE35AFF0E304A60AD708F61345