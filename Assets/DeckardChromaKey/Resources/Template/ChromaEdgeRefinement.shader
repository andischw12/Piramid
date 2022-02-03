// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ChromaEdgeRefinement"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[IntRange]_Directions("Directions", Range( 1 , 50)) = 0
		[IntRange]_Quality("Quality", Range( 1 , 50)) = 0
		_Size("Size", Range( 0 , 0.1)) = 0
		_Float2("Float 2", Range( 0 , 0.1)) = 0
		_Contract("Contract", Range( 0 , 1)) = 0
		_Sharpness("Sharpness", Float) = 0
		_despillChroma("despillChroma", Float) = 0
		_despillLuma("despillLuma", Float) = 0
		_whiteBallance("whiteBallance", Range( -1 , 1)) = 1
		_Gamma("Gamma", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _despillChroma;
			uniform float _despillLuma;
			uniform float _Contract;
			uniform float _Sharpness;
			uniform float _Directions;
			uniform float _Quality;
			uniform float _Size;
			uniform float _Gamma;
			uniform float _whiteBallance;
			uniform float _Float2;
			float4 Despill( float4 result, float _Despill, float _DespillLuminanceAdd, float4 color )
			{
				float v = (2*result.b+result.r)/4;
				                if(result.g > v) result.g = lerp(result.g, v, _Despill);
				                float4 dif = (color - result);
				                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b;
				                result += lerp(0, desaturatedDif, _DespillLuminanceAdd);
				return result;
			}
			
			float4 GaussianBlur14( sampler2D tex, float Directions, float Quality, float Size, float2 iResolution, float2 UV0, out float4 Col )
			{
				float Pi = 6.28318530718; 
				float2 Radius = Size/iResolution.xy;
				    float2 UW;
				UW = UV0/iResolution.xy;
				Col = tex2D(tex, UW);
				    
				    for( float d=0.0; d<Pi; d+=Pi/Directions)
				    {
						for(float i=1.0/Quality; i<=1.0; i+=1.0/Quality)
				        {
							Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i);		
				        }
				    }
				    
				    Col /= Quality * Directions - 15.0;
				    return  Col;
			}
			
			float3 temperature_Deckard1_g5( float3 In, float Temperature, float Tint )
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
			
			float4 GaussianBlur39( sampler2D tex, float Directions, float Quality, float Size, float2 iResolution, float2 UV0, out float4 Col )
			{
				float Pi = 6.28318530718; 
				float2 Radius = Size/iResolution.xy;
				    float2 UW;
				UW = UV0/iResolution.xy;
				Col = tex2D(tex, UW);
				    
				    for( float d=0.0; d<Pi; d+=Pi/Directions)
				    {
						for(float i=1.0/Quality; i<=1.0; i+=1.0/Quality)
				        {
							Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i);		
				        }
				    }
				    
				    Col /= Quality * Directions - 15.0;
				    return  Col;
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
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float3 temp_output_25_0 = (tex2D( _MainTex, uv_MainTex )).rgb;
				float4 result34 = float4( temp_output_25_0 , 0.0 );
				float _Despill34 = _despillChroma;
				sampler2D tex14 = _MainTex;
				float Directions14 = _Directions;
				float Quality14 = _Quality;
				float Size14 = _Size;
				float2 iResolution14 = float2( 1,1 );
				float2 texCoord5 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV014 = texCoord5;
				float4 Col14 = float4( 0,0,0,0 );
				float4 localGaussianBlur14 = GaussianBlur14( tex14 , Directions14 , Quality14 , Size14 , iResolution14 , UV014 , Col14 );
				float temp_output_17_0 = (Col14).w;
				float smoothstepResult28 = smoothstep( _Contract , ( _Contract + _Sharpness ) , temp_output_17_0);
				float _DespillLuminanceAdd34 = ( _despillLuma * ( 1.0 - smoothstepResult28 ) );
				float4 color34 = float4( temp_output_25_0 , 0.0 );
				float4 localDespill34 = Despill( result34 , _Despill34 , _DespillLuminanceAdd34 , color34 );
				float4 lerpResult18 = lerp( localDespill34 , float4( temp_output_25_0 , 0.0 ) , temp_output_17_0);
				float4 temp_cast_3 = (_Gamma).xxxx;
				float4 appendResult15 = (float4(pow( lerpResult18 , temp_cast_3 ).xyz , smoothstepResult28));
				float3 In1_g5 = appendResult15.xyz;
				float Temperature1_g5 = _whiteBallance;
				float Tint1_g5 = 0.0;
				float3 localtemperature_Deckard1_g5 = temperature_Deckard1_g5( In1_g5 , Temperature1_g5 , Tint1_g5 );
				float3 break44 = localtemperature_Deckard1_g5;
				sampler2D tex39 = _MainTex;
				float Directions39 = _Directions;
				float Quality39 = _Quality;
				float Size39 = _Float2;
				float2 iResolution39 = float2( 1,1 );
				float2 UV039 = texCoord5;
				float4 Col39 = float4( 0,0,0,0 );
				float4 localGaussianBlur39 = GaussianBlur39( tex39 , Directions39 , Quality39 , Size39 , iResolution39 , UV039 , Col39 );
				float4 appendResult43 = (float4(break44.x , break44.y , break44.z , (Col39).w));
				
				
				finalColor = appendResult43;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18801
244;309;1408;803;737.5196;928.1582;2.120737;True;True
Node;AmplifyShaderEditor.RangedFloatNode;4;-534.5488,-214.3083;Inherit;False;Property;_Directions;Directions;7;1;[IntRange];Create;False;0;0;0;False;0;False;0;16;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-608.254,-24.89551;Inherit;False;Property;_Size;Size;9;0;Create;True;0;0;0;False;0;False;0;0.056;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-597.5508,140.3402;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-578.9019,-112.4838;Inherit;False;Property;_Quality;Quality;8;1;[IntRange];Create;False;0;0;0;False;0;False;0;8;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1180.653,-194.2762;Inherit;True;Property;_MainTex;MainTex;3;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;29;418.0128,655.2075;Inherit;False;Property;_Sharpness;Sharpness;13;0;Create;True;0;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;14;-299.6331,151.9368;Inherit;False;float Pi = 6.28318530718@ $float2 Radius = Size/iResolution.xy@$    float2 UW@$UW = UV0/iResolution.xy@$$Col = tex2D(tex, UW)@$    $    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions - 15.0@$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.RangedFloatNode;27;322.7128,513.1079;Inherit;False;Property;_Contract;Contract;12;0;Create;True;0;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;17;-78.44047,498.415;Inherit;False;False;False;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;650.0128,506.4078;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;774.8127,336.8078;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;408.7598,-185.1507;Inherit;False;Property;_despillLuma;despillLuma;15;0;Create;True;0;0;0;False;0;False;0;1.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;1005.212,213.6083;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-180.172,-329.7414;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;489.7237,-285.2013;Inherit;False;Property;_despillChroma;despillChroma;14;0;Create;True;0;0;0;False;0;False;0;-0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;25;197.3139,-86.74448;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;695.807,-106.7333;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;34;837.8543,-327.9349;Inherit;False;float v = (2*result.b+result.r)/4@$                if(result.g > v) result.g = lerp(result.g, v, _Despill)@$                float4 dif = (color - result)@$                float desaturatedDif = 0.299*dif.r + 0.587*dif.g + 0.114*dif.b@$                result += lerp(0, desaturatedDif, _DespillLuminanceAdd)@$return result@;4;False;4;True;result;FLOAT4;0,0,0,0;In;;Float;False;True;_Despill;FLOAT;0;In;;Float;False;True;_DespillLuminanceAdd;FLOAT;0;In;;Float;False;True;color;FLOAT4;0,0,0,0;In;;Float;False;Despill;False;True;0;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;47;1432.338,15.01608;Inherit;False;Property;_Gamma;Gamma;18;0;Create;True;0;0;0;False;0;False;1;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;1252.194,-149.1291;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;45;1642.116,-67.80912;Inherit;False;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;1826.675,-68.38791;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;48;1717.805,-295.4041;Inherit;False;Property;_whiteBallance;whiteBallance;17;0;Create;True;0;0;0;False;0;False;1;-0.19;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-742.3466,756.248;Inherit;False;Property;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;0;0.013;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;39;-415.5462,406.6475;Inherit;False;float Pi = 6.28318530718@ $float2 Radius = Size/iResolution.xy@$    float2 UW@$UW = UV0/iResolution.xy@$$Col = tex2D(tex, UW)@$    $    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions - 15.0@$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.FunctionNode;46;2019.724,-180.2455;Inherit;False;ColorTemperature;0;;5;fccce2e41bca18a41863dccc23edb3ef;0;3;2;FLOAT3;0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;44;1936.425,79.86544;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ComponentMaskNode;41;-109.6448,646.0479;Inherit;False;False;False;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexelSizeNode;6;-1036.263,622.7615;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-691.2628,391.7614;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;37;1256.664,-342.7396;Inherit;False;Property;_Color0;Color 0;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.3396226,0.03265598,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;284.0602,207.8816;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;7;-923.8868,368.3773;Inherit;False;Property;_BlurDirection;BlurDirection;5;0;Create;False;0;0;0;False;0;False;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-994.8866,234.3774;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;9;-735.2626,608.7615;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-926.2629,503.7614;Inherit;False;Property;_Float;Float 0;6;0;Create;False;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;2241.413,89.67042;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-41.94794,374.246;Inherit;False;Property;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;0;0.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;26;239.8276,370.5992;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;24;40.16572,161.1499;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;31;1315.448,290.1213;Inherit;False;Normal From Height;-1;;6;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;1;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;3545.018,-51.14587;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-871.8868,113.3774;Inherit;False;Property;_Iterations;Iterations;4;0;Create;True;0;0;0;False;0;False;5;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3583.664,-368.0742;Float;False;True;-1;2;ASEMaterialInspector;100;1;ChromaEdgeRefinement;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;14;0;1;0
WireConnection;14;1;4;0
WireConnection;14;2;3;0
WireConnection;14;3;2;0
WireConnection;14;5;5;0
WireConnection;17;0;14;7
WireConnection;30;0;27;0
WireConnection;30;1;29;0
WireConnection;28;0;17;0
WireConnection;28;1;27;0
WireConnection;28;2;30;0
WireConnection;36;0;28;0
WireConnection;16;0;1;0
WireConnection;25;0;16;0
WireConnection;35;0;32;0
WireConnection;35;1;36;0
WireConnection;34;0;25;0
WireConnection;34;1;33;0
WireConnection;34;2;35;0
WireConnection;34;3;25;0
WireConnection;18;0;34;0
WireConnection;18;1;25;0
WireConnection;18;2;17;0
WireConnection;45;0;18;0
WireConnection;45;1;47;0
WireConnection;15;0;45;0
WireConnection;15;3;28;0
WireConnection;39;0;1;0
WireConnection;39;1;4;0
WireConnection;39;2;3;0
WireConnection;39;3;40;0
WireConnection;39;5;5;0
WireConnection;46;2;15;0
WireConnection;46;5;48;0
WireConnection;44;0;46;0
WireConnection;41;0;39;7
WireConnection;6;0;1;0
WireConnection;12;0;7;0
WireConnection;12;1;8;0
WireConnection;12;2;9;0
WireConnection;21;0;24;0
WireConnection;21;1;22;0
WireConnection;10;2;1;0
WireConnection;9;0;6;1
WireConnection;9;1;6;2
WireConnection;43;0;44;0
WireConnection;43;1;44;1
WireConnection;43;2;44;2
WireConnection;43;3;41;0
WireConnection;26;0;17;0
WireConnection;24;0;14;7
WireConnection;31;20;28;0
WireConnection;0;0;43;0
ASEEND*/
//CHKSM=768CD6190FF16FE2B1BED0977D9AD5494FFA3C45